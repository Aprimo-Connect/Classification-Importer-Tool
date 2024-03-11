using System;
using System.Collections.Generic;
using System.Text;

namespace ClassificationsIngestion
{
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class CreateClassificationRequest
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string parentNamePath;
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name;
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string identifier;
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Fields fields;
        [System.Runtime.Serialization.DataMemberAttribute(Name = "labels")]
        public List<LabelDTO> Labels { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "fields")]
    public partial class Fields
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<FieldsAddOrUpdate> addOrUpdate;
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "addOrUpdate")]
    public partial class FieldsAddOrUpdate
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string id;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<LocalizedValues> localizedValues;
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "localizedValues")]
    public partial class LocalizedValues
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string languageId;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public object value;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<string> values;

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public List<Children> children;

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public List<Parents> parents;
    }
    [System.Runtime.Serialization.DataContractAttribute]
    public  partial class LabelDTO //: IEquatable<LabelDTO>
    {
        [System.Runtime.Serialization.DataMemberAttribute(Name = "languageid")]
        public string Language { get; set; }

        /// <summary>
        /// The text value for this label
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = "value")]
        public string Text { get; set; }

    }
}
