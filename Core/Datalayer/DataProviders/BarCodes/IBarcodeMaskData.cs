using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.BarCodes
{
    public interface IBarcodeMaskData : IDataProviderBase<BarcodeMask>, ISequenceable
    {
        /// <summary>
        /// Gets all barcode masks from row <paramref name="rowFrom"/> to <paramref name="rowTo"/> and returns the actual amounts of records fetched.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">The first row to get</param>
        /// <param name="rowTo">The last row to get</param>
        /// <param name="totalRecordsMatching">The actual number of rows that were fetched. I.e if the range from <paramref name="rowFrom"/> to <paramref name="rowTo"/> is larger than then actual number of records</param>
        /// <returns></returns>
        List<BarcodeMask> LoadBarcodeMasks(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecordsMatching);        
        BarcodeMask GetMaskForBarcode(IConnectionManager entry, BarCode barCode, CacheType cache = CacheType.CacheTypeNone);
        List<BarcodeMask> GetBarCodeMasks(IConnectionManager entry, int sortBy, bool backwardsSort);
        BarcodeMask Get(IConnectionManager entry, RecordIdentifier barcodeMaskID);
        bool MaskExists(IConnectionManager entry, string mask, RecordIdentifier excludeID);
        bool PrefixExists(IConnectionManager entry, string mask, RecordIdentifier excludeID);
        bool Exists(IConnectionManager entry, RecordIdentifier barCode);
        bool BarCodeMaskInUse(IConnectionManager entry, RecordIdentifier barCodeMask);
        void Delete(IConnectionManager entry, RecordIdentifier barCodeMaskID);
        void Save(IConnectionManager entry, BarcodeMask barCodeMask);
    }
}