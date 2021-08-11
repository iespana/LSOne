using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using System.Drawing;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Method to process the original barcode to populate the different barcode properties.
    /// </summary>
    public interface IBarcodeService : IService
    {
        /// <summary>
        /// Quantity scanned
        /// </summary>
        decimal Quantity { get; set; } 

        /// <summary>
        /// Takes two parameters, processes them (thereby searches the database) and returns an instance of 'BarCode'.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem"></param>
        /// <param name="scanInfo">Describes the way a barcode has been entered into the system.</param>
        /// <param name="barCode"></param>
        /// <param name="barcode">A string barcode as entered into the system.</param>
        /// <param name="selectedItemID"></param>
        /// <returns>BarCode is returned to the application core.</returns>
        /// <seealso cref="BarCode"/>
        /// <example> This sample shows a call to this method from the class ProcessInput.cs  
        /// <code> 
        /// BarCode BarCode = 
        /// LSRetailPosis.ApplicationServices.IBarcodes.ProcessBarcode(input, BarcodeEntryType.ManuallyEntered); 
        /// </code> 
        /// </example>
        BarCode ProcessBarcode(IConnectionManager entry, ref ISaleLineItem saleLineItem, ScanInfo scanInfo, BarCode barCode, string barcode, string selectedItemID = "");

        /// <summary>
        /// Takes two parameters, processes them (thereby searches the database) and returns an instance of 'BarCode'.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="entrytype">Describes the way a barcode has been entered into the system.</param>
        /// <param name="barcode">A string barcode as entered into the system.</param>
        /// <param name="selectedItemID"></param>
        /// <returns></returns>
        BarCode ProcessBarcode(IConnectionManager entry, BarCode.BarcodeEntryType entrytype, string barcode, string selectedItemID = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="scanInfo"></param>
        /// <param name="selectedItemID"></param>
        /// <returns></returns>
        BarCode ProcessBarcode(IConnectionManager entry, ScanInfo scanInfo, string selectedItemID = "");

        /// <summary>
        /// After the POS has called <see cref="ProcessBarcode(IConnectionManager, ref ISaleLineItem, ScanInfo, BarCode, string, string)"/> and the barcode is of type Customized this function is called 
        /// to allow any customized functionality that is needed for the scanned barcode. 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="posTransaction">Current transaction</param>
        /// <param name="scanInfo">Information about the barcode that was scanned</param>
        /// <param name="barCode"></param>
        void ProcessCustomizedBarcode(IConnectionManager entry, IPosTransaction posTransaction, ScanInfo scanInfo, BarCode barCode);

        /// <summary>
        /// This function is called if the barcode is of type QR (see <see cref="BarcodeInternalType"/>).
        /// Here  customization should handle the actual scanned input, add what is needed to the transaction, call a webservice, retrieve data
        /// from the database and etc.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <param name="scanInfo">The scan info object created by the scan operation or <see cref="CustomizedScanInput"/></param>
        /// <param name="barCode">Optional parameter. Information about the barcode created by the scan operation or PartnerProcessBarcode</param>
        void ProcessQR(IConnectionManager entry, IPosTransaction posTransaction, ScanInfo scanInfo, BarCode barCode = null);

        /// <summary>
        /// Process barcode segments into barcode
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="barCode"></param>
        void ProcessMaskSegments(IConnectionManager entry, BarCode barCode);

        /// <summary>
        /// When the POS receives something scanned from a barcode this is called first so that if the input string is f.ex. a QR code or boarding pass or any other 
        /// input that cannot be configured with the barcode masks can be recognized and then processed. This function should only set the <see cref="ScanInfo"/> properties
        /// as needed. No processing should be done here. Next step is <see cref="ProcessBarcode(IConnectionManager, ref ISaleLineItem, ScanInfo, BarCode, string, string)"/> function
        ///
        /// Note! This function should only be used if the scanned string cannot be configured through barcode masks
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="operationInfo">Information from the operation that was being run in the POS</param>
        /// <param name="transaction">Rhe current transaction</param>
        /// <param name="input">The string from the scanner</param>
        /// <returns>A <see cref="ScanInfo"/> object that has information about the string that was scanned</returns>
        /// <example>
        /// <code>
        ///        if (Input.Length > 10 &amp;&amp; Input.Substring(0, 11) == "my prefix")
        ///        {
        ///            return = new ScanInfo("my prefix"){ ScanData = Input};
        ///        }
        /// </code>
        /// </example>
        ScanInfo CustomizedScanInput(IConnectionManager entry, OperationInfo operationInfo, IPosTransaction transaction, string input);

        /// <summary>
        /// Parse a receipt barcode to get store and terminal information
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="receiptID">Receipt ID</param>
        /// <returns>Struct containing Store ID, Terminal ID and Receipt ID</returns>
        BarcodeReceiptParseInfo ParseBarcodeReceipt(IConnectionManager entry, string receiptID);

        /// <summary>
        /// Get receipt barcode data based on mask segments and barcode parsed info
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="parseInfo">Parsed info of the barcode</param>
        /// <returns></returns>
        string GetReceiptBarCodeData(IConnectionManager entry, BarcodeReceiptParseInfo parseInfo);

        /// <summary>
        /// Get the barcode symbology based on the type of barcode
        /// </summary>
        /// <param name="typeOfBarcode">Type of barcode</param>
        /// <param name="barcodeSymbology">Barcode symbology reference</param>
        /// <param name="barcode">Barcode reference</param>
        /// <param name="barcodeMessage">Barcode message reference</param>
        /// <param name="size">Size reference</param>
        void ManageReceiptBarcode(BarcodeType typeOfBarcode, ref int barcodeSymbology, ref string barcode, ref string barcodeMessage, ref Size size);
    }
}
