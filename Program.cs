using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Helpers;
using Models;

namespace ClassificationsIngestion
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string path = "";
            // Test if input arguments were supplied:
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter filepath for classification import");
                path = System.Console.ReadLine();
            }
            else
            {
                path = args[0];
            }

            if (string.IsNullOrEmpty(path)) return;

            var registration = ConfigurationManager.AppSettings.Get("registration");
            var token = ConfigurationManager.AppSettings.Get("token");
            var clientId = ConfigurationManager.AppSettings.Get("clientId");
            var userAgent = ConfigurationManager.AppSettings.Get("userAgent");
            var tokenEndpoint = ConfigurationManager.AppSettings.Get("TokenEndpoint");
            var RESTEndpoint = ConfigurationManager.AppSettings.Get("RESTEndpoint");

            var accessHelper = AccessHelper.Instance;
            //we need to properly intialize AccessHelper with client relevant data
            //this is done only once!
            accessHelper.Create(token, tokenEndpoint, clientId);

            //get all classifications configuration requirements from a CSV file
            //file should contain these columns: Classification Name, Parent Classification Path, field columns to represent individual fields on classification
            var classificationsToImport = new List<ClassificationSpecification>();
            using (System.IO.StreamReader reader = System.IO.File.OpenText(path))
            {
               
                string line = string.Empty;
                var firstLine = true;
                List<string> columnNames = new List<string>();
                Dictionary<int,FieldDefinition> fieldDefinitions = new Dictionary<int, FieldDefinition>();
                do
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] fields = line.Split(';');

                    if (firstLine)
                    {
                        firstLine = false;
                        columnNames = new List<string>(fields);
                        
                        continue;
                    }
                    var classification = new ClassificationSpecification()
                    {
                        ClassificationName = fields[0].Trim(),
                        ParentClassificationNamepath = fields[1].Trim(),
                        Label = System.Web.HttpUtility.HtmlEncode(fields[2].Trim()),
                        Identifier = fields[3].Trim(),
                        ClassificationFields = new Dictionary<string, List<string>>()
                    };
                    string classificationNamepath = classification.ParentClassificationNamepath + "/" + classification.ClassificationName;
                    classification.EncodedClassificationNamepath = HttpUtility.UrlEncode(classificationNamepath);
                    for (int i = 4; i < fields.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(fields[i]))
                        {
                            FieldDefinition fieldDef;
                            if (fieldDefinitions.ContainsKey(i))
                            {
                                fieldDef = fieldDefinitions[i];
                            }
                            else
                            {
                                fieldDef = ConfigurationHelper.GetFieldIdByName(columnNames[i], RESTEndpoint);
                            }
                            if (fieldDef != null)
                            {
                                if (!fieldDefinitions.ContainsKey(i))
                                {
                                    fieldDefinitions.Add(i, fieldDef);
                                }
                                if (fieldDef.items[0].dataType.Equals("ClassificationList"))
                                {
                                    HashSet<string> fieldValue = new HashSet<string>();
                                    foreach (string clsPath in fields[i].Split("###"))
                                    {
                                        //get classification ID based on namepath provided in the field column for classification list field
                                        var classificationId = ConfigurationHelper.GetClassificationBasedOnNamepath(HttpUtility.UrlEncode(clsPath.Trim()), RESTEndpoint);
                                        fieldValue.Add(classificationId);
                                    }
                                    classification.ClassificationFields.Add(fieldDef.items.FirstOrDefault().id, fieldValue.ToList());
                                }
                                else
                                {
                                    classification.ClassificationFields.Add(fieldDef.items.FirstOrDefault().id, new List<string> { fields[i] });
                                }
                            }
                        }
                    }                    
                    classificationsToImport.Add(classification);

                } while (!string.IsNullOrEmpty(line));
            }

            StringBuilder reportErrors = new StringBuilder();
            StringBuilder reportUpdates = new StringBuilder();
            StringBuilder reportCreation = new StringBuilder();

            foreach (ClassificationSpecification classificationSpec in classificationsToImport)
            {
                //check if classification arleady exists first
                classificationSpec.Id = ConfigurationHelper.GetClassificationBasedOnNamepath(classificationSpec.EncodedClassificationNamepath, RESTEndpoint);

                if (string.IsNullOrEmpty(classificationSpec.Id))
                {
                    try
                    {
                        classificationSpec.Id = ConfigurationHelper.CreateClassification(classificationSpec, RESTEndpoint);
                        Console.WriteLine("CREATED Classification {0}, id is {1}", classificationSpec.ClassificationName, classificationSpec.Id);
                        reportCreation.AppendLine(string.Format("CREATED Classification {0}, id is {1}", classificationSpec.ClassificationName, classificationSpec.Id));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR CREATING classification {0}, error message is: {1}", classificationSpec.ClassificationName, ex.Message);
                        reportErrors.AppendLine(string.Format("ERROR CREATING classification {0}, error message is: {1}", classificationSpec.ClassificationName, ex.Message));
                    }
            }
                else
            {
                try
                {
                    classificationSpec.Id = ConfigurationHelper.EditClassification(classificationSpec, RESTEndpoint);
                    Console.WriteLine("UPDATED Classification {0}, id which {1} already existed.", classificationSpec.ClassificationName, classificationSpec.Id);
                    reportUpdates.AppendLine(string.Format("UPDATED Classification {0}, id which {1} already existed.", classificationSpec.ClassificationName, classificationSpec.Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR EDITING classification {0}, error message is: {1}", classificationSpec.ClassificationName, ex.Message);
                    reportErrors.AppendLine(string.Format("ERROR EDITING classification {0}, error message is: {1}", classificationSpec.ClassificationName, ex.Message));
                }
            }
        }
            var report = reportErrors.ToString() + Environment.NewLine + reportCreation.ToString() + Environment.NewLine + reportUpdates.ToString();

            //System.IO.File.WriteAllText(@"C:\___Customers\Boston Scientific\PorfolioIngestionReport.txt", report);
            System.Console.WriteLine("Import finished. Copy these logs if needed, before closing this window.");
            path = System.Console.ReadLine();
        }

        public static string SetEncodedNamepath(string classificationNamepath)
        {            
            var bytes = System.Text.Encoding.UTF8.GetBytes(classificationNamepath);
            return System.Convert.ToBase64String(bytes);
        }
    }
}
