using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class StoreCardType
    {
        private RecordIdentifier storeID;
        private RecordIdentifier tenderTypeID;
        private RecordIdentifier cardTypeID;
        private string description;
        private bool checkModulus;
        private bool checkExpiredDate;
        private bool processLocally;
        private bool allowManualInput;

        public StoreCardType()
        {
            storeID = RecordIdentifier.Empty;
            tenderTypeID = RecordIdentifier.Empty;
            cardTypeID = RecordIdentifier.Empty;
            description = "";
            checkModulus = false;
            checkExpiredDate = false;
            processLocally = false;
            allowManualInput = false;
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }

        public RecordIdentifier CardTypeID
        {
            get { return cardTypeID; }
            set { cardTypeID = value; }
        }

        public RecordIdentifier TenderTypeID
        {
            get { return tenderTypeID; }
            set { tenderTypeID = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool CheckModulus
        {
            get { return checkModulus; }
            set { checkModulus = value; }
        }

        public bool CheckExpiredDate
        {
            get { return checkExpiredDate; }
            set { checkExpiredDate = value; }
        }

        public bool ProcessLocally
        {
            get { return processLocally; }
            set { processLocally = value; }
        }

        public bool AllowManualInput
        {
            get { return allowManualInput; }
            set { allowManualInput = value; }
        }
    }
}
