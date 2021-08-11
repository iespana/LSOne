using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.EOD
{
    public interface IZReportData : IDataProviderBase<ZReport>, ISequenceable
    {
        /// <summary>
        /// Returns a list of all Z reports
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of Z reports</returns>
        List<ZReport> GetList(IConnectionManager entry);

        /// <summary>
        /// Returns a list of Z reports for a specific store and terminal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store the Z reports should have been created on</param>
        /// <param name="terminalID">The terminal the Z reports should have been created on</param>
        /// <returns>A list of Z reports</returns>
        List<ZReport> GetList(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID);

        void Save(IConnectionManager entry, ZReport zReport);

        void CreateNewId(IConnectionManager entry, int initialId);
    }
}