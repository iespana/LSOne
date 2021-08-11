using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionObjects
{
    public class InventorySerial : DataEntity
    {
        public InventorySerial()
            :base()
        {
            RFIDTag = "";
            ItemID = "";
            ProductionDate = new DateTime(1900,1,1);
        }
        public RecordIdentifier RFIDTag { get; set; }
        public RecordIdentifier ItemID { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
