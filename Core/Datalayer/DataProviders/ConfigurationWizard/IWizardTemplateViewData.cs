using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IWizardTemplateViewData : IDataProvider<WizardTemplateView>, ISequenceable
    {
        /// <summary>
        /// Get all templates from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sort">OrderField</param>
        /// <returns>Template List</returns>
        List<WizardTemplateView> GetTemplateList(IConnectionManager entry, string sort);
        
        /// <summary>
        /// Save TerminalId and StoreId into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of StoreId and terminalId</param>
        void SaveBusinessTemplate(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save ProfileIds into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of ProfileIds</param>
        void SaveProfiles(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save CurrencyId into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of CurrencyId</param>
        void SaveDefaultCurrency(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save HardwareProfileId into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of HardwareProfileId</param>
        void SaveHardwareProfile(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save Permission for RetailGroup into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of RetailPermission</param>
        void SaveRetailPermission(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save Permission for PosUsers into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of PosPermission</param>
        void SavePosPermission(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save Permission for TillLayout into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of TillLayoutPermission</param>
        void SaveTillLayoutPermission(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Save Permission for FormLayout into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="templateView">object containing record of FormLayoutPermission</param>
        void SaveFormLayoutPermission(IConnectionManager entry, WizardTemplateView templateView);

        /// <summary>
        /// Update the status of a template into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>current status description</returns>
        void UpdateStatus(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Update the last export date of a template into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>current status description</returns>
        void UpdateLastExport(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Check status of template into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        string StatusCompleted(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get country list.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Country list</returns>
        List<DataEntity> GetCountryList(IConnectionManager entry);

        WizardTemplateView Get(IConnectionManager entry, RecordIdentifier id);
    }
}