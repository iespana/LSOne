using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IStoreSettingsData : IDataProviderBase<StoreSettings>
    {
        /// <summary>
        /// Get SalesTaxGroup list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of SalesTaxGroup</returns>
        List<StoreSettings> GetSalesTaxGroupList(IConnectionManager entry);

        /// <summary>
        /// Get ItemTaxGroup list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of ItemTaxGroup</returns>
        List<StoreSettings> GetItemTaxGroupList(IConnectionManager entry);

        /// <summary>
        /// Get Unit list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of Units</returns>
        List<Unit> GetUnitList(IConnectionManager entry);

        /// <summary>
        /// Get Unit list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>List Of Units</returns>
        List<StoreSettings> GetUnitList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Save TaxGroups and Units into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeList">StoreSettings object list</param>
        void SaveTaxGroups(IConnectionManager entry, List<StoreSettings> storeList);

        /// <summary>
        /// Get TaxGroup list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>TaxGroup List</returns>
        List<StoreSettings> GetTaxList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get selected SalesTaxGroups from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">SalesTaxGroupId list</param>
        /// <returns>SalesTaxGroup list</returns>
        List<SalesTaxGroup> GetSelectedSalesTaxGroupList(IConnectionManager entry, List<RecordIdentifier> idList);

        /// <summary>
        /// Get selected ItemTaxGroup list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">ItemTaxGroupId list</param>
        /// <returns>ItemTaxGroup list</returns>
        List<ItemSalesTaxGroup> GetSelectedItemTaxGroupList(IConnectionManager entry, List<RecordIdentifier> idList);

        /// <summary>
        /// Get selected Unit list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">UnitId List</param>
        /// <returns>Unit list</returns>
        List<Unit> GetSelectedUnitList(IConnectionManager entry, List<RecordIdentifier> idList);

        /// <summary>
        /// Get selected FuntionalityProfile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">FuntionalityProfileId</param>
        /// <returns>FuntionalityProfile</returns>
        FunctionalityProfile GetSelectedFuntionalityProfileList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get selected StoreServerProfile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">StoreServerProfileId</param>
        /// <returns>StoreServerProfile</returns>
        SiteServiceProfile GetSelectedStoreServerProfileList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get selected VisualProfile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">VisualProfileId</param>
        /// <returns>VisualProfile</returns>
        VisualProfile GetSelectedVisualProfileList(IConnectionManager entry, RecordIdentifier id);

        void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATETAX");
    }
}