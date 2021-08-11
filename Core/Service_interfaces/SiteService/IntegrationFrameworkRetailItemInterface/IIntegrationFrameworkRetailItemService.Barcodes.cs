using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface
{
    public partial interface IIntegrationFrameworkRetailItemService
    {
        /// <summary>
        /// Get a single <see cref="BarCode"/> from the database.
        /// </summary>
        /// <param name="barCodeID">The ID of the barcode to get</param>
        [OperationContract]
        BarCode GetBarCode(RecordIdentifier barCodeID);

        /// <summary>
        /// Get all <see cref="BarCode"/> from the database
        /// </summary>
        /// <returns>A list of all barcode from the database</returns>
        [OperationContract]
        List<BarCode> GetBarCodes();

        /// <summary>
        /// Saves a single <see cref="BarCode"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="barcode">The barcode to save</param>
        [OperationContract]
        SaveResult SaveBarCode(BarCode barcode);

        /// <summary>
        /// Saves a list of <see cref="BarCode"/> to the database. If any barcode does not exists it will be created.
        /// </summary>
        /// <param name="barCodes">The barcodes to save</param>
        [OperationContract]
        SaveResult SaveBarCodeList(List<BarCode> barCodes);

        /// <summary>
        /// Delete a barcode.
        /// </summary>
        /// <param name="barCodeID">The ID of the barcode to delete</param>
        [OperationContract]
        void DeleteBarCode(RecordIdentifier barCodeID);
    }
}
