using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Currencies;

namespace LSOne.ViewPlugins.Store.Views
{
    public partial class CompanyInfoView : ViewBase
    {
        private CompanyInfo companyInfo;
        IPlugin plugin;
        WeakReference currencyEditor;

        private TabControl.Tab generalTab;
        private TabControl.Tab logoTab;

        public CompanyInfoView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            addressField.DataModel = PluginEntry.DataModel;
            addressField.AddressFormat = PluginEntry.DataModel.Settings.AddressFormat;

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewCurrency", null);
            currencyEditor = plugin != null ? new WeakReference(plugin) : null;
            btnCurrenciesEdit.Visible = (currencyEditor != null);


            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("StoreManagementCurrency", companyInfo.ID, Properties.Resources.CompanyInformation, true));

            tabSheetTabs.GetAuditContexts(contexts);
        }

        public string Description
        {
            get
            {
                return Properties.Resources.CompanyInformation;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CompanyInformation;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            if (!isRevert)
            {
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.CompanyInfoGeneralPage.CreateInstance);
                logoTab = new TabControl.Tab(Properties.Resources.Logo, ViewPages.CompanyInfoLogoPage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);
                tabSheetTabs.AddTab(logoTab);
                
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this,companyInfo.ID);
            }

            tbDescription.Text = companyInfo.Text;
            tbRegistrationNumber.Text = companyInfo.RegistrationNumber;
            addressField.SetData(PluginEntry.DataModel, companyInfo.Address);
            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode, companyInfo.CurrencyCodeText);
            
            HeaderText = Description;

            tabSheetTabs.SetData(isRevert,companyInfo.ID, companyInfo);
        }

        protected override bool DataIsModified()
        {
            companyInfo.Dirty = false;

            bool tabsModified = tabSheetTabs.IsModified();

            companyInfo.Dirty = companyInfo.Dirty |
                tbDescription.Text != companyInfo.Text |
                tbRegistrationNumber.Text != companyInfo.RegistrationNumber |
                addressField.AddressRecord != companyInfo.Address |
                cmbCurrency.SelectedData.ID != companyInfo.CurrencyCode;

            if (cmbCurrency.SelectedData.ID != companyInfo.CurrencyCode)
            {
                MessageDialog.Show(Properties.Resources.ChangingCompanyCurrency);
            }

            return tabsModified | companyInfo.Dirty;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();

            if (companyInfo.Dirty)
            {
                companyInfo.Text = tbDescription.Text;
                companyInfo.RegistrationNumber = tbRegistrationNumber.Text;
                companyInfo.Address = addressField.AddressRecord;

                if (companyInfo.CurrencyCode != cmbCurrency.SelectedData.ID)
                {
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.InitialConfigurationDashboardID);
                }

                companyInfo.CurrencyCode = cmbCurrency.SelectedData.ID;

                Providers.CompanyInfoData.Save(PluginEntry.DataModel, companyInfo);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "CompanyInfo", companyInfo.ID, null);
            }
            

            return true;
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void btnCurrenciesEdit_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "ViewCurrency", cmbCurrency.SelectedData.ID);
            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        private void cmbCurrency_SelectedDataChanged(object sender, EventArgs e)
        {
            if(cmbCurrency.SelectedData.ID != "" && cmbCurrency.SelectedData.ID != RecordIdentifier.Empty)
            {
                // Check if the selected currency has exchange rate of 1
                bool exchangeRateExisted;
                decimal exchangeRateValue;

                exchangeRateValue = Providers.ExchangeRatesData.GetExchangeRate(PluginEntry.DataModel, cmbCurrency.SelectedData.ID, out exchangeRateExisted);

                if(!exchangeRateExisted || exchangeRateValue != 1M)
                {
                    if(LSOne.ViewCore.Dialogs.QuestionDialog.Show(
                        Properties.Resources.WrongExchangeRate + "\n\n" +
                        Properties.Resources.ChangeExchangeRateQuestion,
                        Properties.Resources.ExchangeRateChangeNeeded) == System.Windows.Forms.DialogResult.Yes)
                    {
                        ExchangeRate exchangeRate = new ExchangeRate();

                        exchangeRate.CurrencyCode = cmbCurrency.SelectedData.ID;
                        exchangeRate.FromDate = DateTime.Now;
                        exchangeRate.ExchangeRateValue = 100M;
                        exchangeRate.POSExchangeRateValue = 100M;

                        Providers.ExchangeRatesData.Save(PluginEntry.DataModel, exchangeRate, new DateTime(0001,01,01));
                    }
                    else
                    {
                        cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode, companyInfo.CurrencyCodeText);
                    }

                   

                }
            }
        }
    }
}
