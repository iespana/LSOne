using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerLedger.Dialogs
{
	public partial class LedgerCardDialog : DialogBase
	{
		private RecordIdentifier dataObjectId;
        private CustomerLedgerEntries dataObject;
        private CompanyInfo companyInfo;

        private Parameters prmsData;

	    private SiteServiceProfile siteServiceProfile;

	    public LedgerCardDialog(RecordIdentifier dataObjectId, Parameters paramsData, SiteServiceProfile siteServiceProfile)
			:this(siteServiceProfile)
		{
			this.dataObjectId = dataObjectId;

            prmsData = paramsData;

            ntbCurrencyAmount.Enabled = false;

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel, false);

	        if (dataObject == null)
	        {
	        
	            cmbCurrency.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
	            ntbCurrencyAmount.Text = "";
	            ntbAmount.Text = "";
	            ntbDiscountAmount.Text = "";
	        }
		}

		public LedgerCardDialog(SiteServiceProfile siteServiceProfile)
		{
			InitializeComponent();

		    this.siteServiceProfile = siteServiceProfile;
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

        private void CalcAmount()
        {
            if (cmbCurrency.SelectedData.ID != RecordIdentifier.Empty && ntbCurrencyAmount.Text.Length > 0 && ntbCurrencyAmount.Value != 0)
                ntbAmount.Value = (double)CalcAmount((decimal)ntbCurrencyAmount.Value, cmbCurrency.SelectedData.ID, true);                  
        }

        private decimal CalcAmount(decimal amount, RecordIdentifier currency, bool toCompany)
        {
            //if (cmbCurrency.SelectedData.ID != RecordIdentifier.Empty && ntbCurrencyAmount.Text.Length > 0 && ntbCurrencyAmount.Value!=0)
            //    ntbAmount.Value = (ntbCurrencyAmount.Value*(double)Providers.ExchangeRatesData.GetExchangeRate(PluginEntry.DataModel, cmbCurrency.SelectedData.ID));   

            if(toCompany)
                return (amount * Providers.ExchangeRatesData.GetExchangeRate(PluginEntry.DataModel, currency));
            return (amount / Providers.ExchangeRatesData.GetExchangeRate(PluginEntry.DataModel, currency));
        }

	    private void btnOK_Click(object sender, EventArgs e)
		{
            if (dataObject == null)
            {
                dataObject = new CustomerLedgerEntries();
                //dataObject.ID = "You somehow have to create this Id string";
            }

            dataObject.PostingDate = dtpDate.Value;
            dataObject.Customer = dataObjectId;
            dataObject.EntryType = CustomerLedgerEntries.TypeEnum.Payment;
            dataObject.DocumentNo = tbDocumentNo.Text;
            dataObject.Description = tbDescription.Text;
            
            if(cmbCurrency.SelectedData.ID != RecordIdentifier.Empty )
                dataObject.Currency = cmbCurrency.SelectedData.ID.ToString();

            if (ntbCurrencyAmount.Text != "")
                dataObject.CurrencyAmount = (decimal)ntbCurrencyAmount.Value;
            else
                dataObject.CurrencyAmount = CalcAmount((decimal)ntbAmount.Value, cmbCurrency.SelectedData.ID, false); 

            dataObject.Amount = (decimal)ntbAmount.Value;
            dataObject.RemainingAmount = (decimal)ntbAmount.Value;

            dataObject.Status = CustomerLedgerEntries.StatusEnum.Open;
            dataObject.UserId = new Guid(PluginEntry.DataModel.CurrentUser.ID.ToString());
            dataObject.ID = RecordIdentifier.Empty;
	        dataObject.StoreId = PluginEntry.DataModel.CurrentStoreID;


            CustomerLedgerEntries dataObjectDiscount = null;

            if (ntbDiscountAmount.Text.Length != 0 && ntbDiscountAmount.Value > 0)
            {
                dataObjectDiscount = new CustomerLedgerEntries
                    {
                        PostingDate = dtpDate.Value,
                        Customer = dataObjectId,
                        EntryType = CustomerLedgerEntries.TypeEnum.Discount,
                        DocumentNo = tbDocumentNo.Text,
                        Description = tbDescription.Text,
                        Currency = cmbCurrency.SelectedData.ID.ToString(),
                        CurrencyAmount = CalcAmount((decimal) ntbDiscountAmount.Value, cmbCurrency.SelectedData.ID, false),
                        Amount = (decimal) ntbDiscountAmount.Value,
                        RemainingAmount = 0,
                        UserId = new Guid(PluginEntry.DataModel.CurrentUser.ID.ToString()),
                        Status = CustomerLedgerEntries.StatusEnum.Closed,
                        ID = RecordIdentifier.Empty, 
                        StoreId = PluginEntry.DataModel.CurrentStoreID
                    };
            }

            ISiteServiceService service = null;
            try
            {
                service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                service.SaveCustomerLedgerEntries(PluginEntry.DataModel, siteServiceProfile, dataObject, PluginOperations.UseCentralCustomer(prmsData.SiteServiceProfile), false);

                if (dataObjectDiscount != null)
                {
                    service.SaveCustomerLedgerEntries(PluginEntry.DataModel, siteServiceProfile, dataObjectDiscount, PluginOperations.UseCentralCustomer(prmsData.SiteServiceProfile), false);
                }

                service.UpdateRemainingAmount(PluginEntry.DataModel, siteServiceProfile, dataObjectId, PluginOperations.UseCentralCustomer(prmsData.SiteServiceProfile), false);
                
                service.Disconnect(PluginEntry.DataModel);
            }
            catch (Exception ex)
            {
                MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                return;
            }

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		// Check for the Ok btn being enabled
		private void CheckEnabled(object sender, EventArgs e)
		{
		    bool enabled = (tbDescription.Text.Length > 0) && (ntbAmount.Text.Length > 0);
		    btnOK.Enabled = enabled;
		}

	    private void cmbCurrency_RequestData(object sender, EventArgs e)
	    {
	        var items = Providers.CurrencyData.GetList(PluginEntry.DataModel);

	        //cmbCurrency.SetData(items,
            //    PluginEntry.Framework.GetImageList(),
            //    -1, true);

            cmbCurrency.SetData(items, null);
	    }

	    private void cmbCurrency_RequestClear(object sender, EventArgs e)
        {
            cmbCurrency.SelectedData = new DataEntity(RecordIdentifier.Empty, Properties.Resources.NoSelection);
        }

        private void ntbCurrencyAmount_TextChanged(object sender, EventArgs e)
        {
            CalcAmount();
        }

        private void cmbCurrency_SelectedDataChanged(object sender, EventArgs e)
        {
            CalcAmount();

            if (cmbCurrency.SelectedData.ID == companyInfo.CurrencyCode)
            {
                ntbCurrencyAmount.Enabled = false;
                ntbCurrencyAmount.Text = "";
            }
            else
            {
                ntbCurrencyAmount.Enabled = true;
            }
        }
	}
}
