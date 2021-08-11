using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreementMultiLineDiscountDialog : DialogBase
    {
        RecordIdentifier storeID;
        RecordIdentifier id;
        TradeAgreementEntry agreement;
        MultiLineDiscountDialogMode mode;
        WeakReference currencyEditor;
        int allIndex;

        public enum MultiLineDiscountDialogMode
        {
            ItemDiscountGroup = 0,
            Customer = 1,
            Group = 2
        };

        public TradeAgreementMultiLineDiscountDialog(RecordIdentifier id, RecordIdentifier agreementID, MultiLineDiscountDialogMode mode)
            : this(id,mode)
        {
            DecimalLimit priceLimiter;

            agreement = Providers.TradeAgreementData.Get(PluginEntry.DataModel, agreementID,TradeAgreementRelation.MultiLineDiscSales);

            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);


            if (mode == MultiLineDiscountDialogMode.ItemDiscountGroup)
            {
                cmbAccountCode.SelectedIndex = (int)agreement.AccountCode;
                cmbAccountSelection.SelectedData = new DataEntity(agreement.AccountRelation, agreement.AccountName);
            }
            else if (mode == MultiLineDiscountDialogMode.Customer)
            {
                cmbAccountCode.SelectedIndex = (int)agreement.ItemCode - 1;
                cmbAccountSelection.SelectedData = new DataEntity(agreement.ItemRelation, agreement.ItemRelationName);
            }
            else if (mode == MultiLineDiscountDialogMode.Group)
            {


                cmbAccountCode.SelectedIndex = (int)agreement.ItemCode - 1;
                cmbAccountSelection.SelectedData = new DataEntity(agreement.ItemRelation, agreement.ItemRelationName);
            }

            agreement.FromDate.ToDateControl(dtpFromDate);
            agreement.ToDate.ToDateControl(dtpToDate);

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            ntbQuantity.Text = agreement.QuantityAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity));
            ntbDiscount.Text = agreement.Amount.FormatWithLimits(priceLimiter);

            ntbPercentage1.Value = (double)agreement.Percent1;
            ntbPercentage2.Value = (double)agreement.Percent2;

            CheckEnabled(this, EventArgs.Empty);
        }

        public TradeAgreementMultiLineDiscountDialog(RecordIdentifier id,MultiLineDiscountDialogMode mode)
            : this()
        {
            string itemName = "";
            //int salesUnitDecimals = 0;
            //string salesUnitText = "";
            CompanyInfo companyInfo;

            agreement = null;

            this.mode = mode;
            this.id = id;

            if (mode == MultiLineDiscountDialogMode.Customer || mode == MultiLineDiscountDialogMode.Group)
            {
                cmbAccountCode.Items.Clear();
                cmbAccountCode.Items.Add(Properties.Resources.ItemDiscountGroup);
                cmbAccountCode.Items.Add(Properties.Resources.All);

                allIndex = 1;
            }

            // Automatically select a unit if applies
            //Datalayer.LookupValues.GetInfoForRetailItem(PluginEntry.Connection, itemID, ref itemName, ref salesUnit, ref salesUnitText, ref salesUnitDecimals);

            if(itemName != "") Text += " - " + itemName;

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);

            cmbAccountCode_SelectedIndexChanged(cmbAccountCode, EventArgs.Empty);
            cmbAccountSelection.SelectedData = new DataEntity("","");

        }

        public TradeAgreementMultiLineDiscountDialog()
            : base()
        {
            IPlugin plugin;

            allIndex = 2;
            storeID = RecordIdentifier.Empty;

            InitializeComponent();

            cmbCurrency.SelectedData = new DataEntity("", "");
            cmbAccountSelection.SelectedData = new DataEntity("", "");

            ntbQuantity.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity).Max;
            ntbDiscount.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);

            currencyEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnAddCurrency.Visible = (currencyEditor != null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }       

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
        }

        public RecordIdentifier ID
        {
            get
            {
                return agreement.ID;
            }
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            //btnOK.Enabled = (tbID.Text.Length > 0 && tbDescription.Text.Length > 0);
        }

        private void PopulateTAEntry(TradeAgreementEntry taEntry)
        {
            taEntry.Currency = cmbCurrency.SelectedData.ID;
            taEntry.QuantityAmount = (decimal)ntbQuantity.Value;
            taEntry.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.MultiLineDiscSales;
            taEntry.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();

            switch (mode)
            {
                case MultiLineDiscountDialogMode.ItemDiscountGroup:
                    taEntry.AccountCode = (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex;
                    taEntry.AccountRelation = cmbAccountSelection.SelectedData.ID;
                    taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Group;
                    taEntry.ItemRelation = id;
                    break;
                case MultiLineDiscountDialogMode.Customer:
                    taEntry.AccountCode = 0;
                    taEntry.AccountRelation = id;

                    if (cmbAccountCode.SelectedIndex == 0)
                    {
                        taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Group;
                        taEntry.ItemRelation = cmbAccountSelection.SelectedData.ID;
                    }
                    else
                    {
                        taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.All;
                        taEntry.ItemRelation = "";
                    }

                    break;
                case MultiLineDiscountDialogMode.Group:
                    taEntry.AccountCode = TradeAgreementEntryAccountCode.Group;
                    taEntry.AccountRelation = id;
                    taEntry.ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode)(cmbAccountCode.SelectedIndex + 1);
                    taEntry.ItemRelation = cmbAccountSelection.SelectedData.ID;
                    break;
            }
            taEntry.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();
            taEntry.Amount = (decimal)ntbDiscount.Value;
            taEntry.Percent1 = (decimal)ntbPercentage1.Value;
            taEntry.Percent2 = (decimal)ntbPercentage2.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Date.FromDateControl(dtpFromDate) > Date.FromDateControl(dtpToDate) && !Date.FromDateControl(dtpToDate).IsEmpty)
            {
                errorProvider1.SetError(dtpFromDate, Properties.Resources.FromDateCannotBeHigherThanToDate);
                return;
            }

            if (agreement == null)
            {
                TradeAgreementEntry testAgreement = new TradeAgreementEntry();
                PopulateTAEntry(testAgreement);
                if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, testAgreement.OldID))
                {
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
                    return;
                }

                agreement = new TradeAgreementEntry();
                PopulateTAEntry(agreement);
            }
            else
            {
                PopulateTAEntry(agreement);
                if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, agreement.OldID, agreement.ID))
                {
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
                    return;
                }
            }

            Providers.TradeAgreementData.Save(PluginEntry.DataModel, agreement, Permission.ManageTradeAgreementPrices);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool enabled;
            int accountCodeIndex;
            RecordIdentifier accountRelation = "";

            errorProvider1.Clear();

            enabled = cmbCurrency.SelectedData.ID != "" &&
                    (cmbAccountCode.SelectedIndex == allIndex || cmbAccountSelection.SelectedData.ID != "");

           
            if (agreement != null)
            {
                accountCodeIndex = cmbAccountCode.SelectedIndex;

                if (mode == MultiLineDiscountDialogMode.Group)
                {
                    accountCodeIndex = 1;

                    if (cmbAccountCode.SelectedIndex != allIndex)
                    {
                        enabled = enabled && (cmbAccountSelection.SelectedData.ID != "");
                    }
                }
                else if(mode == MultiLineDiscountDialogMode.Customer)
                {
                    accountCodeIndex = 0;

                    if (cmbAccountCode.SelectedIndex != allIndex)
                    {
                        enabled = enabled && (cmbAccountSelection.SelectedData.ID != "");
                    }
                }
                else
                {
                    if (cmbAccountCode.SelectedIndex != allIndex)
                    {
                        enabled = enabled && (cmbAccountSelection.SelectedData.ID != "");
                    }
                }

                if (mode == MultiLineDiscountDialogMode.ItemDiscountGroup)
                {
                    accountRelation = agreement.AccountRelation;
                }
                else if (mode == MultiLineDiscountDialogMode.Customer)
                {
                    accountRelation = agreement.ItemRelation;
                }
                else if (mode == MultiLineDiscountDialogMode.Group)
                {
                    accountRelation = agreement.ItemRelation;
                }

                enabled = enabled &&
                   (cmbCurrency.SelectedData.ID != agreement.Currency ||
                   (TradeAgreementEntryAccountCode)accountCodeIndex != agreement.AccountCode ||
                   cmbAccountSelection.SelectedData.ID != accountRelation ||
                   Date.FromDateControl(dtpFromDate).GetDateWithoutTime() != agreement.FromDate ||
                   Date.FromDateControl(dtpToDate).GetDateWithoutTime() != agreement.ToDate ||
                   (decimal)ntbQuantity.Value != agreement.QuantityAmount ||
                   (decimal)ntbDiscount.Value != agreement.Amount ||
                   (decimal)ntbPercentage1.Value != agreement.Percent1 ||
                   (decimal)ntbPercentage2.Value != agreement.Percent2);
            }

            btnOK.Enabled = enabled;
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbPrice_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbPriceUnit_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }
              
        private void cmbAccountCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mode == MultiLineDiscountDialogMode.ItemDiscountGroup)
            {
                if (cmbAccountCode.SelectedIndex == 0)
                {
                    lblAccountSelection.Text = Properties.Resources.Customer + ":";
                    cmbAccountSelection.Enabled = true;

                }
                else if (cmbAccountCode.SelectedIndex == 1)
                {
                    lblAccountSelection.Text = Properties.Resources.CustomerDiscountGroup + ":";
                    cmbAccountSelection.Enabled = true;
                }
                else
                {
                    lblAccountSelection.Text = Properties.Resources.Customer + ":";
                    cmbAccountSelection.Enabled = false;
                }
            }
            else if (mode == MultiLineDiscountDialogMode.Customer)
            {
                lblAccountSelection.Text = Properties.Resources.ItemDiscountGroup + ":";

                if (cmbAccountCode.SelectedIndex == 0)
                {
                    cmbAccountSelection.Enabled = true;
                }
                else
                {
                    cmbAccountSelection.Enabled = false;
                }
            }
            else if (mode == MultiLineDiscountDialogMode.Group)
            {
                //lblAccountSelection.Text = Properties.Resources.CustomerDiscountGroup + ":";
                lblAccountSelection.Text = Properties.Resources.ItemDiscountGroup + ":";

                if (cmbAccountCode.SelectedIndex == 0)
                {
                    cmbAccountSelection.Enabled = true;
                }
                else
                {
                    cmbAccountSelection.Enabled = false;
                }
            }

            cmbAccountSelection.SelectedData = new DataEntity("", "");

            CheckEnabled(this,EventArgs.Empty);
        }

        private void cmbAccountSelection_DropDown(object sender, DropDownEventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 0 && mode == MultiLineDiscountDialogMode.ItemDiscountGroup)
            {
                RecordIdentifier initialSearchText;
                bool textInitallyHighlighted;
                if (e.DisplayText != "")
                {
                    initialSearchText = e.DisplayText;
                    textInitallyHighlighted = false;
                }
                else
                {
                    initialSearchText = ((DataEntity)cmbAccountSelection.SelectedData).ID;
                    textInitallyHighlighted = true;
                }

                e.ControlToEmbed = new SingleSearchPanel(
                    PluginEntry.DataModel,
                    false,
                    initialSearchText,
                    SearchTypeEnum.Customers,
                    textInitallyHighlighted);
            }
        }

        private void cmbAccountSelection_RequestData(object sender, EventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 1 && mode == MultiLineDiscountDialogMode.ItemDiscountGroup)
            {
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel,PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.MultilineDiscountGroup),
                    null);
            }
            else if (cmbAccountCode.SelectedIndex == 0 && mode == MultiLineDiscountDialogMode.Customer)
            {
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Item, PriceDiscGroupEnum.MultilineDiscountGroup),
                    null);
            }
            else if (cmbAccountCode.SelectedIndex == 0 && mode == MultiLineDiscountDialogMode.Group)
            {
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Item, PriceDiscGroupEnum.MultilineDiscountGroup),
                    null);
            }
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "AddCurrency", cmbCurrency.SelectedData.ID);
            }
        }
    }
}
