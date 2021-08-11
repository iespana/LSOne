using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Labels
{
    public interface ILabelQueueData : IDataProviderBase<LabelQueue>
    {
        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="batch">Batch to get, or null to get all</param>
        /// <returns>An instance of <see cref="LabelQueue"/></returns>
        List<LabelQueue> GetList(IConnectionManager entry, string batch);

        void SetPrinted(IConnectionManager entry, RecordIdentifier labelQueueID, string message);
        void Save(IConnectionManager entry, LabelQueue labelQueue);
    }
}