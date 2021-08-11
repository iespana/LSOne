using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.RFID
{
    public class RFID : DataEntity
    {
        public RFID()
            :base()
        {
            TransactionID = "";
            ItemID = "";
        }
        [RecordIdentifierValidation(24)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionID { get; set; }
        public RecordIdentifier ItemID { get; set; }
        public DateTime ScannedTime { get; set; }
    }
}
