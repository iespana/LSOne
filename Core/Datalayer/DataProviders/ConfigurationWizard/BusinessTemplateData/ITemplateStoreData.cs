using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard.BusinessTemplate;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard.BusinessTemplateData
{
    public interface ITemplateStoreData : IDataProvider<TemplateStore>
    {
        /// <summary>
        /// Gets a list of all stores
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all stores</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        RecordIdentifier GetStoreID(IConnectionManager entry, RecordIdentifier id);
    }
}