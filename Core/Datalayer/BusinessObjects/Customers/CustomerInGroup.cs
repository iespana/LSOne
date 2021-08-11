using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Customers
{
    public class CustomerInGroup : DataEntity
    {
        public CustomerInGroup()
        {
            Name = new Name();
            GroupID = RecordIdentifier.Empty;
            GroupName = "";
            Default = false;
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(base.ID, GroupID);
            }
            set
            {
                base.ID = value.PrimaryID;
                GroupID = value.SecondaryID;
            }
            
        }

        public Name Name { get; set; }
        public RecordIdentifier GroupID { get; set; }
        public string GroupName { get; set; }
        public bool Default { get; set; }
    }
}
