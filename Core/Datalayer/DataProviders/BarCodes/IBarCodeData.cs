using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders.Attributes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.BarCodes
{
    [TableName("INVENTITEMBARCODE")]
    public interface IBarCodeData : IDataProvider<BarCode>, ICompareListGetter<BarCode>, ISequenceable
    {
        BarCode GetBarCodeForItem(IConnectionManager entry, RecordIdentifier itemID, CacheType cache = CacheType.CacheTypeNone);
        bool ShowForItemHasBeenUsed(IConnectionManager entry, RecordIdentifier itemID);
        BarCode Get(IConnectionManager entry, RecordIdentifier barCodeID, CacheType cache = CacheType.CacheTypeNone);
        List<BarCode> GetList(IConnectionManager entry, RecordIdentifier itemID, BarCodeSorting sortBy, bool backwardsSort, bool getDeletedBarcodes = false);

        void DeleteWithItemID(IConnectionManager entry, RecordIdentifier itemId);


        List<BarCode> GetListForVariant(IConnectionManager entry, RecordIdentifier variantID, bool getDeletedBarcodes = false);

        /// <summary>
        /// Returns a list of barcodes that exist for all variants of the given header item ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The master ID of the header item</param>
        /// <param name="getDeletedBarcodes"></param>
        /// <returns></returns>
        List<BarCode> GetListForHeaderItem(IConnectionManager entry, RecordIdentifier itemID, bool getDeletedBarcodes = false);

        string GetBarcodeWithShowForItem(IConnectionManager entry, RecordIdentifier itemID);
        void AddInformationToBarcode(IConnectionManager entry, BarCode barCode);
        List<BarCode> GetListOfBarCodes(IConnectionManager entry, BarCode barCode);

        /// <summary>
        /// Processes the different barcode segments
        /// </summary>
        List<BarcodeMaskSegment> GetBarcodeSegments(IConnectionManager entry, BarCode barCode);

        /// <summary>
        /// Returns a list of all barcodes
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<BarCode> GetAllBarcodes(IConnectionManager entry);

        void UndeleteBarcodeWithItemID(IConnectionManager entry, RecordIdentifier itemID);
        void UndeleteBarcode(IConnectionManager entry, RecordIdentifier barcode);
        bool IsDeleted(IConnectionManager entry, RecordIdentifier barcode);

        /// <summary>
        /// Loads all available barcodes within the given segment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">starting row</param>
        /// <param name="rowTo">the end row</param>
        /// <param name="totalRecordsMatching">how many rows are there</param>
        /// <returns></returns>
        List<BarCode> LoadBarCodes(
            IConnectionManager entry,
            int rowFrom,
            int rowTo,
            out int totalRecordsMatching);

        /// <summary>
        /// Checks if a barcode with a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="barCode">The id of the barcode to checkfor</param>
        /// <returns></returns>
        bool ExistsWithID(IConnectionManager entry, RecordIdentifier barCode);

        /// <summary>
        /// Remove all barcodes from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        void DeleteAll(IConnectionManager entry);
    }
}