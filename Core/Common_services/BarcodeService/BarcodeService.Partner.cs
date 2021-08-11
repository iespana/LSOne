using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class BarcodeService
	{
		/// <summary>
		/// When the POS receives something scanned from a barcode this function called first so that if the input string is f.ex. a QR code or boarding pass or any other 
		/// input that cannot be configured with the barcode masks can be recognized and then processed. This function should only set the <see cref="ScanInfo"/> properties
		/// as needed. No processing should be done here. Next step is <see cref="PartnerProcessBarcode"/> function.
		///
		/// Note! This function should only be used if the scanned string cannot be configured through barcode masks
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="operationInfo">Information from the operation that was being run in the POS</param>
		/// <param name="transaction">The current transaction</param>
		/// <param name="input">The string from the scanner</param>
		/// <returns>A <see cref="ScanInfo"/> object that has information about the string that was scanned. 
		///          If null is returned the POS will use the default functionality to figure out what was scanned</returns>
		/// <example>
		///        if (Input.Substring(0, 11) == "my prefix")
		///        {
		///            return = new ScanInfo("my prefix"){ ScanData = Input};
		///        }
		/// </example>
		[LSOneUsage(CodeUsage.Partner)]
		public virtual ScanInfo CustomizedScanInput(IConnectionManager entry, OperationInfo operationInfo, IPosTransaction transaction, string input)
		{
			//To test the different types of customized inputs follow the instructions below
			//NOTE!! These values here below are only for demonstration purposes
			//if (input == "QR")
			//{
			//    //Set a breakpoint in function PartnerProcessBarcode (here below)
			//    return new ScanInfo("QR") { ScanData = "Your QR code" };
			//}
			
			//if (input == "Coupon")
			//{
			//    //Set a breakpoint in function PartnerProcessBarcode (here below)
			//    return new ScanInfo("Coupon") { ScanData = "Your Coupon" };
			//}

			//if (input == "DataEntry")
			//{
			//    //Set a breakpoint in function PartnerProcessBarcode (here below)
			//    return new ScanInfo("DataEntry") { ScanData = "Your DataEntry" };
			//}

			//if (input == "Custom")
			//{
			//    //Set a breakpoint in function PartnerProcessBarcode (here below)
			//    return new ScanInfo("Custom") { ScanData = "Your custom barcode" };
			//}

			return null;  
		}

		/// <summary>
		/// This function is called shortly after <see cref="CustomizedScanInput"/> by ProcessBarcode. This function will tell the POS what type of barcode this is
		/// i.e. Coupon, Item, Customer (see <see cref="BarcodeInternalType"/>). Next step is <see cref="ProcessCustomizedBarcode"/> function
		/// 
		/// If this method returns non-null, then the internal LS One barcode processing will not be used (see private function ProcessBarcode)
		/// 
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="scanInfo">The scan info object created by the scan operation or <see cref="CustomizedScanInput"/></param>
		/// <returns>A <see cref="BarCode"/> object with information about the barcode. If this is NOT null then the barcode service will assume that no more parsing is needed and will not do any more processing</returns>
		/// <example>
		/// 
		///         if (scanInfo.ScanDataLabel == "my prefix")
		///         {
		///             //Note! Setting the InternalType as Customized will force the POS to call ProcessCustomizedBarcode
		///             //but the InternalType can be set as any other option available. 
		///             BarCode barcode = new BarCode();
		///             barcode.InternalType = BarcodeInternalType.Customized;
		///             return barcode;
		///         }
		/// 
		/// 
		/// </example>
		[LSOneUsage(CodeUsage.Partner)]
		protected virtual BarCode PartnerProcessBarcode(IConnectionManager entry, ScanInfo scanInfo)
		{

			//if (scanInfo.ScanDataLabel == "QR")
			//{
			//    //Set a breakpoint in function ProcessQR (here below)
			//    return new BarCode() {InternalType = BarcodeInternalType.QR};
			//}

			//if (scanInfo.ScanDataLabel == "Coupon")
			//{
			//    //Set a breakpoint in function CouponService.ProcessCustomizedCoupons
			//    return new BarCode() { InternalType = BarcodeInternalType.Coupon };
			//}

			//if (scanInfo.ScanDataLabel == "DataEntry")
			//{
			//    //Set a breakpoing in function ProcessCustomizedBarcode (here below)
			//    return new BarCode() { InternalType = BarcodeInternalType.DataEntry };
			//}

			//if (scanInfo.ScanDataLabel == "Custom")
			//{
			//    //Set a breakpoing in function ProcessCustomizedBarcode (here below)
			//    return new BarCode() { InternalType = BarcodeInternalType.Customized };
			//}


			return null;
		}

		/// <summary>
		/// This function is called if the barcode is of type DataEntry or Customized (see <see cref="BarcodeInternalType"/>).
		/// Here  customization should handle the actual scanned input, add what is needed to the transaction, call a webservice, retrieve data
		/// from the database and etc.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="posTransaction">Current transaction</param>
		/// <param name="scanInfo">The scan info object created by the scan operation or <see cref="CustomizedScanInput"/></param>
		/// <param name="barCode">Information about the barcode created by the scan operation or <see cref="PartnerProcessBarcode"/></param>
		[LSOneUsage(CodeUsage.Partner)]
		public virtual void ProcessCustomizedBarcode(IConnectionManager entry, IPosTransaction posTransaction, ScanInfo scanInfo, BarCode barCode)
		{
			//***************************************************************************************************
			// In this function any processing that needs to be done if the user scannes in a customized barcode
			//***************************************************************************************************

			//Services.Interfaces.Services.DialogService(entry).ShowMessage("You scanned in: " + scanInfo.ScanData);
		}

		/// <summary>
		/// This function is called if the barcode is of type QR (see <see cref="BarcodeInternalType"/>).
		/// Here  customization should handle the actual scanned input, add what is needed to the transaction, call a webservice, retrieve data
		/// from the database and etc.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="posTransaction">Current transaction</param>
		/// <param name="scanInfo">The scan info object created by the scan operation or <see cref="CustomizedScanInput"/></param>
		/// <param name="barCode">Information about the barcode created by the scan operation or <see cref="PartnerProcessBarcode"/>, 
		///                       if value is null then this function was called from the ScanQR internal operation from within the POS </param>
		[LSOneUsage(CodeUsage.Partner)]
		public virtual void ProcessQR(IConnectionManager entry, IPosTransaction posTransaction, ScanInfo scanInfo, BarCode barCode = null)
		{
			//***************************************************************************************************
			// In this function any processing that needs to be done if the user scannes in a QR code
			//***************************************************************************************************

			//Services.Interfaces.Services.DialogService(entry).ShowMessage("You scanned in: " + scanInfo.ScanData);

			//*********************************************
			//Sample code on how to parse QR code that comes from LS Omni Hospitality Loyalty app can be found in the function below
			//*********************************************
			//scanInfo.ScanDataLabel = "HospLoy";
			//Interfaces.Services.CouponService(entry).ProcessCustomizedCoupons(entry, posTransaction, scanInfo, barCode);
		}
	}
}