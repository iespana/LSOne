using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class BlankOperation : DataEntity
    {
        public override string Text
        {
            get
            {
                return OperationDescription;
            }
            set
            {
                OperationDescription = value;
            }
        }

        public BlankOperation()
        {
            StoreID = RecordIdentifier.Empty;
            OperationDescription = "";
            //Id = "";
            OperationParameter = "";
            ManagerPermissionRequired = false;
        }

        public bool ManagerPermissionRequired { get; set; }

        public string OperationParameter { get; set; }

        //public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        public RecordIdentifier StoreID { get; set; }

        public string OperationDescription { get; set; }

        public bool CreatedOnPOS { get; set; }
    }
}
