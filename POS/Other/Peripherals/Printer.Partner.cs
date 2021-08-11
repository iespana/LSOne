using System.Drawing;
using BarcodeLib;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.Utilities.BarcodeUtilities;
using LSOne.Utilities.Development;

namespace LSOne.Peripherals
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	static public partial class Printer
	{
		[LSOneUsage(CodeUsage.Partner)]
		private static bool ManageReceiptBarcode(ref FormLineBarcode barcodeLine)
		{
			/************************************

			The following barcode types need to be customized if they are to be used
			for barcode printing on the receipt

			If this function returns false then the POS will check for the other barcode types and try and print them

			If any other barcode types are managed in this function the return value must be false so that the default code doesn't try to manage it as well

			Use BarcodeReceiptUtil and BarcodeUtilities when and if needed

			FormLineBarcode object also has the size of the barcode on it - so that can also be customized here if need be

			************************************/
			switch (barcodeLine.TypeOfBarcode)
			{
			   case BarcodeType.EAN128:
					//Needs to be customized
					break;
				case BarcodeType.Code128:
					//Needs to be customized
					break;
				case BarcodeType.UPCA:
					//Needs to be customized
					break;
				case BarcodeType.UPCE:
					//Needs to be customized
					break;
				case BarcodeType.PDF417:
					//Needs to be customized
					break;
				case BarcodeType.MaxiCode:
					//Needs to be customized
					break;
				case BarcodeType.QRCode:
					//Needs to be customized
					break;
			}
			return false;
		}

		[LSOneUsage(CodeUsage.Partner)]
		private static int PrintBarcode(Graphics g, string barcode, float x, float y)
		{
			var type = TYPE.UNSPECIFIED;
			switch (DLLEntry.Settings.Store.BarcodeSymbology)
			{
				case BarcodeType.EAN128:
					type = TYPE.CODE128A;
					break;
				case BarcodeType.Interleaved2of5:
					type = TYPE.Interleaved2of5;
					BarcodeUtilities.AdjustBarcodeI2of5(ref barcode);
					break;
				case BarcodeType.Code128:
					type = TYPE.CODE128;
					break;
				case BarcodeType.UPCA:
					type = TYPE.UPCA;
					break;
				case BarcodeType.UPCE:
					type = TYPE.UPCE;
					break;
				case BarcodeType.EAN13:
					type = TYPE.EAN13;
					BarcodeUtilities.AdjustBarcodeEAN13(ref barcode);
					break;
				case BarcodeType.EAN8:
					type = TYPE.EAN8;
					BarcodeUtilities.AdjustBarcodeEAN8(ref barcode);
					break;
				case BarcodeType.Code39:
					type = TYPE.CODE39;
					break;
				case BarcodeType.PDF417:
				case BarcodeType.MaxiCode:
				default:
					break;
			}

			try
			{
				if (type != TYPE.UNSPECIFIED)
				{
					var b = new Barcode {IncludeLabel = true};
					var bounds = g.VisibleClipBounds;
					int width = (int) (bounds.Width - 40);
					const int height = 50;
					var barcodeImage = b.Encode(type, barcode, width, height);
					if (barcodeImage != null)
					{

						g.DrawImage(barcodeImage, 20, y);
						return barcodeImage.Height;
					}
				}
			}
			catch 
			{ }

			return 0;
		}
	}
}
