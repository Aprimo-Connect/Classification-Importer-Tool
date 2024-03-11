using System.Collections.Generic;

namespace Models
{
    // Type created for JSON at <<root>>
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class FieldDefinition
    {

        //[System.Runtime.Serialization.DataMemberAttribute(Name = "_links")]
        //public FieldLinks _links;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<Items> items;
    }

    // Type created for JSON at <<root>> --> _links
    //[System.Runtime.Serialization.DataContractAttribute()]
    //public partial class FieldLinks
    //{        
    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "self")]
    //    public Self self;
    //}

    // Type created for JSON at <<root>> --> items
    [System.Runtime.Serialization.DataContractAttribute(Name = "items")]
    public partial class Items
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string id;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tag;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string label;

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public List<Labels> labels;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string dataType;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string scope;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string scopeCategory;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool indexed;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string languageMode;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool searchIndexRebuildRequired;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int sortIndex;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double accuracy;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string range;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isRequired;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isReadOnly;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isUniqueIdentifier;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isInheritable;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<string> memberships;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<string> enabledLanguages;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string inlineStyle;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string modifiedOn;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string createdOn;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string defaultValue;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<string> resetToDefaultTriggers;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string validation;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string validationErrorMessage;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string validationTrigger;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string storageMode;
    }
}