using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Properties;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.SerialNumbers
{
    public class SerialNumber : DataEntity
    {
        /// <summary>
        /// Constructs an empty serial number instance.
        /// </summary>
        public SerialNumber()
            : base(RecordIdentifier.Empty, string.Empty)
        {
            ID = RecordIdentifier.Empty;
            ItemMasterID = RecordIdentifier.Empty;
            ItemDescription = string.Empty;
            ItemVariant = string.Empty;
            ItemID = string.Empty;
            SerialNo = string.Empty;
            SerialType = TypeOfSerial.SerialNumber;
            CreateDate = DateTime.Now;
            UsedDate = null;
            Reference = string.Empty;
            ManualEntry = false;
            Reserved = false;
        }

        /// <summary>
        /// MasterID of the item
        /// </summary>
        public RecordIdentifier ItemMasterID { get; set; }
        /// <summary>
        /// ItemID of the item
        /// </summary>
        public string ItemID { get; set; }
        /// <summary>
        /// Serial number
        /// </summary>
        public string SerialNo { get; set; }
        /// <summary>
        /// Serial type
        /// </summary>
        public TypeOfSerial SerialType { get; set; }
        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Used date = sold item date
        /// </summary>
        public DateTime? UsedDate { get; set; }
        /// <summary>
        /// When an item is added to the transaction is beeing marked as reserved, so that other transactions will not use this serial number.
        /// </summary>
        public bool Reserved { get; set; }
        /// <summary>
        /// Receipt ID
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Manual entry
        /// </summary>
        public bool ManualEntry { get; set; }
        /// <summary>
        /// Item description
        /// </summary>
        public string ItemDescription { get; set; }
        /// <summary>
        /// Item variant
        /// </summary>
        public string ItemVariant { get; set; }
        public static string GetTypeOfSerialString(TypeOfSerial typeOfSerial)
        {
            switch (typeOfSerial)
            {
                case TypeOfSerial.SerialNumber:
                    return Resources.SerialNumber;
                case TypeOfSerial.RFIDTag:
                    return Resources.RFIDTag;
                default:
                    return string.Empty;
            }
        }
    }
}
