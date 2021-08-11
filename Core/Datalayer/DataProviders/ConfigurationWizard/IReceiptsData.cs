using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IReceiptsData : IDataProviderBase<Form>
    {
        /// <summary>
        /// Get receipt layout list from database based on provided templateId.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        List<Receipts> GetReceiptLayoutList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Save all selected layouts into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layoutList">list of layouts</param>
        void SaveLayouts(IConnectionManager entry, List<Receipts> layoutList);

        /// <summary>
        /// Get selected list of form layouts from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">form layout id list</param>
        /// <returns>List of form layouts</returns>
        List<Form> GetSelectedReceipts(IConnectionManager entry, List<RecordIdentifier> idList);

        void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATEFORMLAYOUTS");
        Form Get(IConnectionManager entry, RecordIdentifier id);
    }
}