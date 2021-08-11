using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.BarCodes
{
    public interface IBarCodeMaskSegmentData : IDataProviderBase<BarcodeMaskSegment>
    {
        /// <summary>
        /// Gets a list of all existing barcode mask segments
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<BarcodeMaskSegment> GetAllSegments(IConnectionManager entry);

        /// <summary>
        /// Gets all barcode mask segments from row <paramref name="rowFrom"/> to <paramref name="rowTo"/> and returns the actual amounts of records fetched.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">The first row to get</param>
        /// <param name="rowTo">The last row to get</param>
        /// <param name="totalRecordsMatching">The actual number of rows that were fetched. I.e if the range from <paramref name="rowFrom"/> to <paramref name="rowTo"/> is larger than then actual number of records</param>
        /// <returns></returns>
        List<BarcodeMaskSegment> LoadSegments(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecordsMatching);

        List<BarcodeMaskSegment> Get(IConnectionManager entry, RecordIdentifier maskID);
        void DeleteAllSegments(IConnectionManager entry, RecordIdentifier barCodeMaskID);
        void Save(IConnectionManager entry, RecordIdentifier barCodeMaskID,List<BarcodeMaskSegment> segments);

        /// <summary>
        /// Deletes the barcode mask segment with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="barcodeMaskSegmentID">The ID of the barcode mask segment. This should be combination of <see cref="BarcodeMaskSegment.MaskId"/> and <see cref="BarcodeMaskSegment.SegmentNum"/></param>
        void Delete(IConnectionManager entry, RecordIdentifier barcodeMaskSegmentID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="barcodeMaskSegmentID"></param>
        /// <returns></returns>
        bool Exists(IConnectionManager entry, RecordIdentifier barcodeMaskSegmentID);
    }
}