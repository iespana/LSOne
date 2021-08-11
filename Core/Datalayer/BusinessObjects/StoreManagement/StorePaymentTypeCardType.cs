using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class StorePaymentTypeCardType : DataEntity
    {
        public StorePaymentTypeCardType()
        {
            CardTypeID = "";
        }

        public RecordIdentifier CardTypeID;
    }
}
