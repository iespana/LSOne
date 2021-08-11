using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.EMails
{
    public interface IEMailSettingData : IDataProviderBase<EMailSetting>
    {
        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>An instance of <see cref="EMailSetting"/></returns>
        List<EMailSetting> GetList(IConnectionManager entry);

        EMailSetting Get(IConnectionManager entry, RecordIdentifier storeID);
        void Save(IConnectionManager entry, EMailSetting emailSetting);
    }
}