using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewCore.Dialogs
{
    public class UpdateItemPricesTaxQuestionDialog
    {
        public RecordIdentifier DefaultStoreID { get; set; }

        public UpdateItemPricesTaxQuestionDialog()
        {
            DefaultStoreID = RecordIdentifier.Empty;
        }
        
        public DialogResult Show(IConnectionManager entry)
        {
            RecordIdentifier defaultStoreID = DefaultStoreID == null || DefaultStoreID.StringValue == "" ? Providers.StoreData.GetDefaultStoreID(entry) : DefaultStoreID;
            
            bool defaultStoreUsesPriceWithTax = defaultStoreID == "" || Providers.StoreData.GetPriceWithTaxForStore(entry, defaultStoreID);

            string questionString = Properties.Resources.UpdatePricesAutomatically + ". ";

            if (defaultStoreUsesPriceWithTax)
            {
                questionString += Properties.Resources.PricesUpdatedFromPriceWithTax;
            }
            else
            {
                questionString += Properties.Resources.PricesUpdatedFromPriceWithoutTax;
            }

            return QuestionDialog.Show(questionString, Properties.Resources.UpdatePrices);
        }
    }
}
