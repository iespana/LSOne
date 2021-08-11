using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.LookupValues
{
    public class MsrCardLink : DataEntity
    {
        public enum LinkTypeEnum
        {
            POSUser = 0,
            Customer = 1
        }

        public MsrCardLink()
            : base()
        {
            LinkType = LinkTypeEnum.POSUser;
            LinkID = "";
        }

        public LinkTypeEnum LinkType { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier LinkID { get; set; }

    }
}
