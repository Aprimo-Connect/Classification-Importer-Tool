using System;
using System.Collections.Generic;
using System.Text;

namespace ClassificationsIngestion
{
    public class ClassificationSpecification
    {
        public string ClassificationName { get; set; }
        public string ParentClassificationNamepath { get; set; }
        public Dictionary<string, List<string>> ClassificationFields { get; set; }
        public string EncodedClassificationNamepath { get; set; }
        public string Id { get; set; }       
        public string Identifier { get; set; }

        public string Label { get; set; }

    }
    
}
