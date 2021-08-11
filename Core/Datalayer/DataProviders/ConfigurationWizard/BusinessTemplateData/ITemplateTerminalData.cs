using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard.BusinessTemplate;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard.BusinessTemplateData
{
    public interface ITemplateTerminalData : IDataProvider<TemplateTerminal>
    {
        /// <summary>
        /// Gets a list of all terminals based on selected store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">Store ID</param>
        /// <returns>A list of all terminals</returns>
        List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier storeID);

        RecordIdentifier GetterminalID(IConnectionManager entry, RecordIdentifier id);
    }
}