using ClassificationsIngestion;
using Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Models
{
    public static class ConfigurationHelper
    {
        public static string GetClassificationBasedOnNamepath(string encodedNamepath, string RESTEndpoint)
        {
            // Perform request
            var accessHelper = AccessHelper.Instance;
            var client = new RestClient(RESTEndpoint);
            var request = new RestRequest(string.Format("classification/?namepath={0}", encodedNamepath), Method.Get);
            var accessToken = accessHelper.GetToken();
            request.AddHeader("Authorization", string.Format("Bearer " + accessToken));
            request.AddHeader("Accept", "application/hal+json");
            request.AddHeader("API-VERSION", "1");
            //request.AddHeader("namepath", string.Format("{0}", encodedNamepath));

            RestResponse response = client.Execute(request);
            if (response.StatusCode.ToString().Equals("unauthorized", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = accessHelper.GetToken();
                request.AddOrUpdateParameter("Authorization", string.Format("Bearer " + accessToken));
                response = client.Execute(request);
            }
            if (response.StatusCode.ToString().Equals("OK", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var classification = JsonHelper.Deserialize<Classification>(response.Content);
                    if (classification != null)
                    {
                        return classification.id;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return "";
        }

        public static FieldDefinition GetFieldIdByName(string fieldName, string RESTEndpoint)
        {
            var accessHelper = AccessHelper.Instance;
            var client = new RestClient(RESTEndpoint);
            var request = new RestRequest("fielddefinitions", Method.Get);
            var accessToken = accessHelper.GetToken();

            request.AddHeader("Authorization", string.Format("Bearer " + accessToken));
            request.AddHeader("API-VERSION", "1");
            //request.AddHeader("User-Agent", userAgent);
            request.AddHeader("filter", string.Format("name='{0}'", fieldName));

            RestResponse response = client.Execute(request);
            if (response.StatusCode.ToString().Equals("unauthorized", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = accessHelper.GetToken();
                request.AddOrUpdateParameter("Authorization", string.Format("Bearer " + accessToken));
                response = client.Execute(request);
            }
            if (response.StatusCode.ToString().Equals("OK", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var fieldDefinition = JsonHelper.Deserialize<FieldDefinition>(response.Content);
                    return fieldDefinition;
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }

        public static string Serialize<T>(T t)
        {
            var settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("o"),
            };
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T), settings);
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static string CreateClassification(ClassificationSpecification classification, string RESTEndpoint)
        {
            var accessHelper = AccessHelper.Instance;
            var accessToken = accessHelper.GetToken();

            // Perform request
            string result = String.Empty;
            var client = new RestClient(RESTEndpoint);
            var request = new RestRequest("classifications", Method.Post);
            request.AddHeader("Authorization", string.Format("Bearer " + accessToken));
            request.AddHeader("API-VERSION", "1");
            //request.AddHeader("User-Agent", indexingParameters.UserAgent);
            request.AddHeader("Accept", "application/hal+json");
            //request.AddHeader("registration", indexingParameters.Registration);
            request.RequestFormat = DataFormat.Json;

            var bodyRequest = new CreateClassificationRequest();

            bodyRequest.name = classification.ClassificationName;
            bodyRequest.parentNamePath = classification.ParentClassificationNamepath;
            bodyRequest.identifier = classification.Identifier;
            bodyRequest.Labels = new List<LabelDTO>();
            LabelDTO label = new LabelDTO() { Language = "c2bd4f9bbb954bcb80c31e924c9c26dc", Text = classification.Label };
            bodyRequest.Labels.Add(label);

            if(classification.ClassificationFields.Count > 0)
            {
                bodyRequest.fields = new Fields();
                bodyRequest.fields.addOrUpdate = new List<FieldsAddOrUpdate>();
                foreach(string fieldId in classification.ClassificationFields.Keys)
                {
                    if (!string.IsNullOrEmpty(classification.ClassificationFields[fieldId].First()))
                    {
                        var fieldToAdd = new FieldsAddOrUpdate();
                        fieldToAdd.id = fieldId;
                        fieldToAdd.localizedValues = new List<LocalizedValues>();
                        var values = new LocalizedValues();
                        values.languageId = "c2bd4f9bbb954bcb80c31e924c9c26dc"; //default language
                        //uncomment for list fields
                        values.values = classification.ClassificationFields[fieldId];
                        //uncomment for text or single value fields
                        //values.value = classification.ClassificationFields[fieldId];
                        fieldToAdd.localizedValues.Add(values);
                        bodyRequest.fields.addOrUpdate.Add(fieldToAdd);
                    }
                }
            }

            request.AddJsonBody(Serialize(bodyRequest));
            RestResponse response = client.Execute(request);
            if (response.StatusCode.ToString().Equals("unauthorized", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = accessHelper.GetToken();
                request.AddOrUpdateParameter("Authorization", string.Format("Bearer " + accessToken));
                response = client.Execute(request);
            }

            if (!response.StatusCode.ToString().Equals("Created"))
            {
                //EditClassification(classification, RESTEndpoint);
                throw new Exception(string.Format("{0}, {1}", response.StatusCode, response.Content));
            }

            dynamic o = JsonConvert.DeserializeObject(response.Content);
            return o.id.ToString();
        }

        public static string EditClassification(ClassificationSpecification classification, string RESTEndpoint)
        {
            var accessHelper = AccessHelper.Instance;
            var accessToken = accessHelper.GetToken();

            // Perform request
            string result = String.Empty;
            var client = new RestClient(RESTEndpoint);
            var request = new RestRequest(string.Format("classification/{0}", classification.Id), Method.Put);
            request.AddHeader("Authorization", string.Format("Bearer " + accessToken));
            request.AddHeader("API-VERSION", "1");
            request.AddHeader("Accept", "application/hal+json");
            request.RequestFormat = DataFormat.Json;

            var bodyRequest = new CreateClassificationRequest();

            bodyRequest.name = classification.ClassificationName;
            bodyRequest.identifier = classification.Identifier;
            bodyRequest.Labels = new List<LabelDTO>();
            LabelDTO label = new LabelDTO() { Language = "c2bd4f9bbb954bcb80c31e924c9c26dc", Text = classification.Label };
            bodyRequest.Labels.Add(label);

            if (classification.ClassificationFields.Count > 0)
            {
                bodyRequest.fields = new Fields();
                bodyRequest.fields.addOrUpdate = new List<FieldsAddOrUpdate>();
                foreach (string fieldId in classification.ClassificationFields.Keys)
                {
                    if (!string.IsNullOrEmpty(classification.ClassificationFields[fieldId].First()))
                    {
                        var fieldToAdd = new FieldsAddOrUpdate();
                        fieldToAdd.id = fieldId;
                        fieldToAdd.localizedValues = new List<LocalizedValues>();
                        var values = new LocalizedValues();
                        values.languageId = "c2bd4f9bbb954bcb80c31e924c9c26dc"; //default language
                                                                                //uncomment for list fields
                        values.values = classification.ClassificationFields[fieldId];
                        //uncomment for text or single value fields
                        //values.value = classification.ClassificationFields[fieldId];
                        fieldToAdd.localizedValues.Add(values);
                        bodyRequest.fields.addOrUpdate.Add(fieldToAdd);
                    }
                }
            }

            request.AddJsonBody(Serialize(bodyRequest));
            RestResponse response = client.Execute(request);
            if (response.StatusCode.ToString().Equals("unauthorized", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = accessHelper.GetToken();
                request.AddOrUpdateParameter("Authorization", string.Format("Bearer " + accessToken));
                response = client.Execute(request);
            }

            if (!response.StatusCode.ToString().Equals("NoContent"))
            {
                throw new Exception(string.Format("{0}, {1}", response.StatusCode, response.Content));
            }

            return classification.Id;
        }
    }
}
