using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IWindowsPrinterConfigurationData : IDataProvider<WindowsPrinterConfiguration>, ISequenceable
    {
        /// <summary>
        /// Get the windows printer configuration with the specified ID
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="id">ID of the windows printer configuration</param>
        /// <param name="cacheType">Cache type</param>
        /// <returns></returns>
        WindowsPrinterConfiguration Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Get a list of windows printer configurations
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <returns></returns>
        List<WindowsPrinterConfiguration> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of windows printer configurations as data entity
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <returns></returns>
        List<DataEntity> GetDataEntityList(IConnectionManager entry);
    }
}
