using System.Runtime.InteropServices;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    /// <summary>
    /// Information about the input that was received from a barcode scanner.
    /// </summary>
    public class ScanInfo
    {
        #region Properties
        
        /// <summary>
        /// The scanned input from the barcode
        /// </summary>
        public string ScanDataLabel { get; set; }

        /// <summary>
        /// The scanned input from the barcode with the prefix (F for EAN13 as an example)
        /// </summary>
        public string ScanData { get; set; }

        /// <summary>
        /// Type of barcode
        /// </summary>
        public int ScanDataType { get; set; }

        /// <summary>
        /// How the barcode was entered; manually or scanned
        /// </summary>
        public BarCode.BarcodeEntryType EntryType { get; set; }

        /// <summary>
        /// RFID tag read from the RFID scanner
        /// </summary>
        public string RFIDTag { get; set; }

        /// <summary>
        /// See information in <see cref="RFIDType"/>
        /// </summary>
        public RFIDType RFIDFound { get; set; }

        /// <summary>
        /// An object that partner can use for customizations. Is not used by LS One POS
        /// </summary>
        public object CustomInfo { get; set; }

        #endregion

        /// <summary>
        /// Default constructor, runs function <see cref="Clear"/>.
        /// </summary>
        public ScanInfo()
        {
            Clear();
        }

        /// <summary>
        /// Constructor that takes a value for property <see cref="ScanDataLabel"/>
        /// </summary>
        /// <param name="scanDataLabel">The scanned input from the barcode</param>
        public ScanInfo(string scanDataLabel)
            : this()
        {
            ScanDataLabel = scanDataLabel;
        }

        /// <summary>
        /// Constructor that takes a value for property <see cref="ScanDataLabel"/> and <see cref="RFIDTag"/>
        /// </summary>
        /// <param name="scanDataLabel">The scanned input from the barcode</param>
        /// <param name="rfidTag">RFID tag read from the RFID scanner</param>
        public ScanInfo(string scanDataLabel, string rfidTag)
            : this()
        {
            ScanDataLabel = scanDataLabel;
            RFIDTag = rfidTag;
        }

        /// <summary>
        /// If <see cref="ScanDataLabel"/>, <see cref="ScanData"/> and <see cref="ScanDataType"/> are empty this function return true
        /// </summary>
        /// <returns>Returns true if the object is empty</returns>
        public bool Empty()
        {
            return (ScanDataLabel == "" && ScanData == "" && ScanDataType == 0);
        }

        /// <summary>
        /// Clears all properties
        /// </summary>
        public void Clear()
        {
            ScanDataLabel = "";
            ScanData = "";
            ScanDataType = 0;
            EntryType = BarCode.BarcodeEntryType.ManuallyEntered;
            RFIDFound = RFIDType.None;
            CustomInfo = null;
        }

    }
}
