using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IHospitalityOperationData : IDataProviderBase<HospitalityOperation>
    {
        /// <summary>
        /// Gets a list of all hospitality operations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of HospitalityOperation objects containing all hospitality operations</returns>
        List<HospitalityOperation> GetList(IConnectionManager entry);

        HospitalityOperation Get(IConnectionManager entry, RecordIdentifier hospitalityOperationID);
    }
}