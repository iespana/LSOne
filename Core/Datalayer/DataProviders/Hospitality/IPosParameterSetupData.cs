using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IPosParameterSetupData : IDataProviderBase<PosParameterSetup>
    {
        /// <summary>
        /// Gets all pos parameter setup lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PosParameterSetup objects containing all pos marameter setup lines</returns>
        List<PosParameterSetup> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of parameters for a given operation ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="operationId">The id of the operation</param>
        /// <returns>A list of all parameters for the given operation</returns>
        List<PosParameterSetup> GetList(IConnectionManager entry, RecordIdentifier operationId);

        PosParameterSetup Get(IConnectionManager entry, RecordIdentifier posParameterSetupID);
    }
}