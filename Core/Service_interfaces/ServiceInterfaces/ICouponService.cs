using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface ICouponService : IService
    {
        /// <summary>
        /// This function is called when the transaction is concluding itself. Here any final wrap-up of coupons already added needs to happen if necessary
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        void UpdateCoupons(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// This function is called if the barcode is of type Coupon (see <see cref="BarcodeInternalType"/>.
        /// 
        /// Here a customization should handle the actual scanned coupon, add what is needed to the transaction, call a webservice, retrieve data
        /// from the database and etc.
        /// 
        /// This functionality needs to start in either <see cref="IBarcodeService.CustomizedScanInput"/> or <see cref="IBarcodeService.ProcessBarcode(IConnectionManager,ref ISaleLineItem,ScanInfo,BarCode,string, string)"/> in the <see cref="IBarcodeService"/> service. 
        /// If the barcode is marked as being of type Coupon in PartnerProcessBarcode this function will be called for processing. 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <param name="scanInfo">The scan info object created by the scan operation or <see cref="IBarcodeService.CustomizedScanInput"/></param>
        /// <param name="barCode">Information about the barcode created by the scan operation or <see cref="IBarcodeService.ProcessBarcode(IConnectionManager,ref ISaleLineItem,ScanInfo,BarCode,string, string)"/></param>
        void ProcessCustomizedCoupons(IConnectionManager entry, IPosTransaction posTransaction, ScanInfo scanInfo, BarCode barCode);
    }
}
