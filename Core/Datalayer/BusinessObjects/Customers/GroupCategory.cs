using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Customers
{
    public class GroupCategory : DataEntity
    {
        public bool Empty
        {
            get
            {
                return ID == RecordIdentifier.Empty || ID.StringValue == "";
            }
        }

        public GroupCategory() : base()
        {

        }
    }
}
