using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class PromotionDialog : DialogBase
    {
        RecordIdentifier offerID;
        DiscountOffer offer;
        WeakReference priceGroupEditor;
        private DiscountOffer.AccountCodeEnum currentAccountCode;
        private bool isCustomerRelation;
        private RecordIdentifier accountRelation;

        public PromotionDialog(RecordIdentifier offerID)
            : this()
        {
            this.offerID = offerID;

            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion);

            tbDescription.Text = offer.Text;
            ntbDiscount.Value = (double)offer.DiscountPercent;

            currentAccountCode = offer.AccountCode;
            cmbAccountCode.SelectedIndex = (int)currentAccountCode;

            if (currentAccountCode == DiscountOffer.AccountCodeEnum.Customer)
            {
                cmbAccountSelection.SelectedData =
                    Providers.CustomerData.Get(PluginEntry.DataModel, offer.AccountRelation, UsageIntentEnum.Normal) ??
                    new DataEntity("", "");
            }
            else if (currentAccountCode == DiscountOffer.AccountCodeEnum.CustomerGroup)
            {
                RecordIdentifier groupID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer,
                                           new RecordIdentifier((int)PriceDiscGroupEnum.LineDiscountGroup, offer.AccountRelation));

                cmbAccountSelection.SelectedData =
                    Providers.PriceDiscountGroupData.Get(PluginEntry.DataModel, groupID) ??
                    new DataEntity("", "");
            }

            cmbPriceGroup.SelectedData = new DataEntity(offer.PriceGroup, offer.PriceGroupName);
            cmbDiscountPeriodNumber.SelectedData = new DataEntity(offer.ValidationPeriod, offer.ValidationPeriodDescription);
            cmbDiscountPeriodNumber_SelectedDataChanged(this, EventArgs.Empty);

            if (offer.ValidationPeriod != "")
            {
                tbStartingDate.Text = offer.StartingDate.ToShortDateString();
                tbEndingDate.Text = offer.EndingDate.ToShortDateString();
            }
            else
            {
                tbStartingDate.Text = "";
                tbEndingDate.Text = "";
            }
        }

        public PromotionDialog()
            : base()
        {
            offer = null;
            offerID = RecordIdentifier.Empty;

            InitializeComponent();

            ntbDiscount.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent).Max;

            currentAccountCode = DiscountOffer.AccountCodeEnum.None;
            cmbAccountCode.SelectedIndex = 0;
            cmbAccountSelection.SelectedData = new DataEntity("", "");

            cmbPriceGroup.SelectedData = new DataEntity("", "");
            cmbDiscountPeriodNumber.SelectedData = new DataEntity("", "");

            IPlugin plugin;
            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddPriceGroup", null);
            priceGroupEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddPriceGroup.Visible = (priceGroupEditor != null);
        }

        public bool IsCustomerRelation
        {
            get { return isCustomerRelation; }
        }

        public RecordIdentifier AccountRelation
        {
            get { return accountRelation; }
        }

        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier OfferID
        {
            get { return offerID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbAccountCode.SelectedIndex != 0 && (cmbAccountSelection.SelectedData == null || cmbAccountSelection.SelectedData.ID.ToString() == ""))
            {
                errorProvider1.SetError(cmbAccountSelection, Properties.Resources.AccountSelectionCannotBeEmpty);
                return;
            }

            if (offer == null)
            {
                offer = new DiscountOffer();
                offer.Enabled = false;
            }

            offer.Text = tbDescription.Text;
            offer.DiscountPercent = (decimal)ntbDiscount.Value;
            offer.ValidationPeriod = cmbDiscountPeriodNumber.SelectedData.ID;
            offer.OfferType = DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion;

            offer.PriceGroup = cmbPriceGroup.SelectedData.ID;

            offer.AccountCode = (DiscountOffer.AccountCodeEnum)cmbAccountCode.SelectedIndex;
            offer.AccountRelation = cmbAccountSelection.SelectedData.ID.HasSecondaryID ? cmbAccountSelection.SelectedData.ID[2] : cmbAccountSelection.SelectedData.ID;
            
            Providers.DiscountOfferData.Save(PluginEntry.DataModel, offer);
            offerID = offer.ID;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PromotionOffer", offerID, null);

            if (offer.AccountCode == DiscountOffer.AccountCodeEnum.Customer)
            {
                isCustomerRelation = true;
            }

            accountRelation = offer.AccountRelation;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (offer == null)
            {
                btnOK.Enabled = tbDescription.Text != "";
            }
            else
            {
                btnOK.Enabled = tbDescription.Text != "" &&
                    (tbDescription.Text != offer.Text ||
                     ntbDiscount.Value != (double)offer.DiscountPercent ||
                     cmbDiscountPeriodNumber.SelectedData.ID != offer.ValidationPeriod ||
                     cmbPriceGroup.SelectedData.ID != offer.PriceGroup ||
                     cmbAccountCode.SelectedIndex != (int)offer.AccountCode ||
                     (cmbAccountSelection.SelectedData.ID.HasSecondaryID ? cmbAccountSelection.SelectedData.ID[2] != offer.AccountRelation : cmbAccountSelection.SelectedData.ID != offer.AccountRelation)
                     );
            }
        }

        private void btnAddDiscountPeriod_Click(object sender, EventArgs e)
        {
            ValidationPeriodDialog dlg = new ValidationPeriodDialog();

            dlg.ShowDialog();
        }

        private void cmbDiscountPeriodNumber_RequestClear(object sender, EventArgs e)
        {
            cmbDiscountPeriodNumber.SelectedData = new DataEntity("", "");
            cmbDiscountPeriodNumber_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void cmbDiscountPeriodNumber_RequestData(object sender, EventArgs e)
        {
            cmbDiscountPeriodNumber.SetData(Providers.DiscountPeriodData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbDiscountPeriodNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            DiscountPeriod period;

            period = Providers.DiscountPeriodData.Get(PluginEntry.DataModel, cmbDiscountPeriodNumber.SelectedData.ID);

            if (period != null)
            {
                tbStartingDate.Text = period.StartingDate.ToShortDateString();
                tbEndingDate.Text = period.EndingDate.ToShortDateString();
            }

            if (cmbDiscountPeriodNumber.SelectedData.ID != "")
            {
                btnsDiscountPeriod.EditButtonEnabled = true;
            }
            else
            {
                btnsDiscountPeriod.EditButtonEnabled = false;
            }

            CheckEnabled(null, EventArgs.Empty);
        }

        private void cmbPriceGroup_RequestData(object sender, EventArgs e)
        {
            cmbPriceGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.PriceGroup), null);
        }

        private void cmbPriceGroup_RequestClear(object sender, EventArgs e)
        {
            cmbPriceGroup.SelectedData = new DataEntity("", "");

            CheckEnabled(this, EventArgs.Empty);
        }

        private void btnAddPriceGroup_Click(object sender, EventArgs e)
        {
            if (priceGroupEditor.IsAlive)
            {
                ((IPlugin)priceGroupEditor.Target).Message(this, "AddPriceGroup", null);
            }
        }

        private void cmbPriceGroup_FormatData(object sender, DropDownFormatDataArgs e)
        {
            e.TextToDisplay = (string)((DataEntity)e.Data).ID;
        }

        private void btnsDiscountPeriod_EditButtonClicked(object sender, EventArgs e)
        {
            ValidationPeriodDialog dlg = new ValidationPeriodDialog(cmbDiscountPeriodNumber.SelectedData.ID);

            DialogResult dlgResult = dlg.ShowDialog();
            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                cmbDiscountPeriodNumber.SelectedData = dlg.DiscountPeriodEntity;
            }
        }

        private void cmbAccountCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if ((int)currentAccountCode != cmbAccountCode.SelectedIndex)
            {
                cmbAccountSelection.SelectedData = new DataEntity("", "");
            }

            currentAccountCode = (DiscountOffer.AccountCodeEnum)cmbAccountCode.SelectedIndex;

            cmbAccountSelection.Enabled = currentAccountCode != DiscountOffer.AccountCodeEnum.None;

            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbAccountSelection_RequestData(object sender, EventArgs e)
        {
            if (currentAccountCode == DiscountOffer.AccountCodeEnum.CustomerGroup)
            {
                // Currently we only look at line discount groups
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.LineDiscountGroup), null);
            }
        }

        private void cmbAccountSelection_DropDown(object sender, DropDownEventArgs e)
        {
            if (currentAccountCode == DiscountOffer.AccountCodeEnum.Customer)
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

        private void cmbAccountSelection_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckEnabled(this, EventArgs.Empty);
        }

               
    }
}
