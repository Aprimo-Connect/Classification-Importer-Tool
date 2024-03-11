using System.Collections.Generic;

namespace Models
{
    // Type created for JSON at <<root>>
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class Classification
    {

        //[System.Runtime.Serialization.DataMemberAttribute(Name = "_links")]
        //public ClassificationLinks _links;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string id;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string identifier;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string contentType;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int sortIndex;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string sortOrder;

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public List<Labels> labels;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public object[] registeredFields;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<RegisteredFieldGroups> registeredFieldGroups;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isRoot;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string parentId;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public object hasChildren;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tag;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string modifiedOn;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string createdOn;
    }

    //// Type created for JSON at <<root>> --> _links
    //[System.Runtime.Serialization.DataContractAttribute()]
    //public partial class ClassificationLinks
    //{

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "parent")]
    //    public Memberships parent;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "ancestors")]
    //    public Memberships ancestors;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "children")]
    //    public Memberships children;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "image")]
    //    public Memberships image;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "fields")]
    //    public Memberships fields;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "recordpermissions")]
    //    public Memberships recordpermissions;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "downloadpermissions")]
    //    public Memberships downloadpermissions;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "classificationtreepermissions")]
    //    public Memberships classificationtreepermissions;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "slaveclassifications")]
    //    public Memberships slaveclassifications;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "modifiedby")]
    //    public Memberships modifiedby;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "createdby")]
    //    public Memberships createdby;

    //    [System.Runtime.Serialization.DataMemberAttribute(Name = "self")]
    //    public Self self;
    //}

    // Type created for JSON at <<root>> --> registeredFieldGroups
    [System.Runtime.Serialization.DataContractAttribute(Name = "registeredFieldGroups")]
    public partial class RegisteredFieldGroups
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string fieldGroupId;
    }
}