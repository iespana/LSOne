using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IPeripheralsData : IDataProviderBase<Peripherals>
    {
        /// <summary>
        /// Get peripheral list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>peripheral list</returns>
        Peripherals GetPeripheralList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get selected hardware profile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">profileId</param>
        /// <param name="cacheType">cacheType</param>
        /// <returns>selected hardware profile</returns>
        HardwareProfile GetSelectedProfile(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);

        void Save(IConnectionManager entry, Peripherals item);
        bool Exists(IConnectionManager entry, RecordIdentifier ID);
        void Delete(IConnectionManager entry, RecordIdentifier ID, string table = "WIZARDTEMPLATEPERIPHERALS");
    }
}