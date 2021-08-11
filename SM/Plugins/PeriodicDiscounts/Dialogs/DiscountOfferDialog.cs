using System;
using System.Collections.Generic;
using System.Linq;
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
using LSOne.ViewPlugins.PeriodicDiscounts.Properties;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class DiscountOfferDialog : DialogBase
    {
        RecordIdentifier offerID;
        DiscountOffer offer;
        WeakReference priceGroupEditor;
        private DiscountOffer.AccountCodeEnum currentAccountCode;
        private List<int> prioritiesInUse;
        private List<DiscountOffer> currentDiscountOffers; 

        public DiscountOfferDialog(RecordIdentifier offerID)
            : this()
        {
            this.offerID = offerID;

            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer);

            tbDescription.Text = offer.Text;
            ntbPriority.Value = (double)offer.Priority;
            ntbDiscount.Value = (double)offer.DiscountPercent;

            cmbPriceGroup.SelectedData = new DataEntity(offer.PriceGroup, offer.PriceGroupName);
            cmbDiscountPeriodNumber.SelectedData = new DataEntity(offer.ValidationPeriod, offer.ValidationPeriodDescription);
            cmbDiscountPeriodNumber_SelectedDataChanged(this, EventArgs.Empty);

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

            cmbTriggering.SelectedIndex = (int)offer.Triggering;
            tbBarcode.Text = offer.BarCode ?? "";

            CheckEnabled(this, EventArgs.Empty);
        }

        public DiscountOfferDialog()
        {
            offer = null;
            offerID = RecordIdentifier.Empty;

            InitializeComponent();

            prioritiesInUse = Providers.DiscountOfferData.GetPrioritiesInUse(PluginEntry.DataModel);            
            currentDiscountOffers = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel,
                                                                           true,
                                                                           DiscountOfferSorting.OfferNumber,
                                                                           false);

            ntbPriority.Value = Providers.DiscountOfferData.GetNextPriority(PluginEntry.DataModel);
            ntbDiscount.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent).Max;

            cmbPriceGroup.SelectedData = new DataEntity("", "");
            cmbDiscountPeriodNumber.SelectedData = new DataEntity("", "");
            currentAccountCode = DiscountOffer.AccountCodeEnum.None;
            cmbAccountCode.SelectedIndex = 0;
            cmbAccountSelection.SelectedData = new DataEntity("", "");

            cmbTriggering.SelectedIndex = 0;

            IPlugin plugin;
            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddPriceGroup", null);
            priceGroupEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddPriceGroup.Visible = (priceGroupEditor != null);
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
            errorProvider1.Clear();

            if (cmbAccountCode.SelectedIndex != 0 && (cmbAccountSelection.SelectedData == null || cmbAccountSelection.SelectedData.ID.StringValue == ""))
            {
                errorProvider2.SetError(cmbAccountSelection, Properties.Resources.AccountSelectionCannotBeEmpty);
                return;
            }

            bool editingExistingOffer = offer != null;

            if (ntbDiscount.Value == 0.0)
            {
                errorProvider1.SetError(lblDiscountPct, Properties.Resources.DiscountPercentCannotBeZero);
                ntbDiscount.Focus();
                return;
            }                       

            if (editingExistingOffer)
            {
                if (currentDiscountOffers.Any(p => p.ID != offer.ID && p.Priority == (int)ntbPriority.Value))
                {
                    errorProvider1.SetError(ntbPriority, Properties.Resources.PriorityAlreadyInUseNextIs.Replace("#1", GetNextAvailablePriority((int)ntbPriority.Value).ToString()));
                    ntbPriority.Focus();
                    return;
                }
            }
            else
            {
                if (prioritiesInUse.Contains((int)ntbPriority.Value))
                {
                    errorProvider1.SetError(ntbPriority, Properties.Resources.PriorityAlreadyInUseNextIs.Replace("#1", GetNextAvailablePriority((int)ntbPriority.Value).ToString()));
                    ntbPriority.Focus();
                    return;
                }

                offer = new DiscountOffer();
                offer.Enabled = false;   
            }

            if (Providers.DiscountOfferData.BarcodeExists(PluginEntry.DataModel, tbBarcode.Text) && tbBarcode.Text != "" && offer.BarCode != tbBarcode.Text)
            {
                errorProvider1.SetError(tbBarcode, Resources.BarcodeAlreadyAssignedToAnOffer);
                return;
            }

            offer.Text = tbDescription.Text;
            offer.Priority = (int)ntbPriority.Value;
            offer.DiscountPercent = (decimal)ntbDiscount.Value;
            offer.ValidationPeriod = cmbDiscountPeriodNumber.SelectedData.ID;
            offer.OfferType = DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer;
            offer.PriceGroup = cmbPriceGroup.SelectedData.ID;
            offer.AccountCode = (DiscountOffer.AccountCodeEnum) cmbAccountCode.SelectedIndex;
            offer.AccountRelation = cmbAccountSelection.SelectedData.ID.HasSecondaryID ? cmbAccountSelection.SelectedData.ID[2] : cmbAccountSelection.SelectedData.ID;
            offer.Triggering = (DiscountOffer.TriggeringEnum)cmbTriggering.SelectedIndex;
            offer.BarCode = tbBarcode.Text;
            offerID = offer.ID;

            Providers.DiscountOfferData.Save(PluginEntry.DataModel, offer);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, editingExistingOffer ? DataEntityChangeType.Edit : DataEntityChangeType.Add, "PeriodicDiscount", offer.ID, offer.OfferType);

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
            errorProvider1.Clear();

            if (offer == null)
            {
                btnOK.Enabled = tbDescription.Text != "";

                if (ntbPriority.Text != "" && prioritiesInUse.Contains((int)ntbPriority.Value))
                {
                    errorProvider1.SetError(ntbPriority, Properties.Resources.PriorityAlreadyInUseNextIs.Replace("#1", GetNextAvailablePriority((int)ntbPriority.Value).ToString()));
                }
            }
            else
            {
                btnOK.Enabled = tbDescription.Text != "" &&
                    (tbDescription.Text != offer.Text ||
                     (int)ntbPriority.Value != offer.Priority ||
                     (int)ntbDiscount.Value != offer.DiscountPercent ||
                     cmbDiscountPeriodNumber.SelectedData.ID != offer.ValidationPeriod ||
                     cmbPriceGroup.SelectedData.ID != offer.PriceGroup ||
                     cmbAccountCode.SelectedIndex != (int)offer.AccountCode ||
                     tbBarcode.Text != offer.BarCode ||
                     (cmbAccountSelection.SelectedData.ID.HasSecondaryID ? cmbAccountSelection.SelectedData.ID[2] != offer.AccountRelation : cmbAccountSelection.SelectedData.ID != offer.AccountRelation) ||
                     cmbTriggering.SelectedIndex != (int)offer.Triggering
                     );


                if (ntbPriority.Text != "" && currentDiscountOffers.Any(p => p.ID != offer.ID && p.Priority == (int)ntbPriority.Value))
                {
                    errorProvider1.SetError(ntbPriority, Properties.Resources.PriorityAlreadyInUseNextIs.Replace("#1", GetNextAvailablePriority((int)ntbPriority.Value).ToString()));
                }
            }

            lblBarcode.Enabled = tbBarcode.Enabled = cmbTriggering.SelectedIndex == (int) DiscountOffer.TriggeringEnum.Manual;

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

        private void cmbPriceGroup_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbPriceGroup_RequestData(object sender, EventArgs e)
        {
            cmbPriceGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel,PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.PriceGroup), null);
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

        private void cmbPriceGroup_FormatData_1(object sender, DropDownFormatDataArgs e)
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
            errorProvider2.Clear();

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

        private int GetNextAvailablePriority(int currentPriority)
        {
            int priority = currentPriority;            

            while (prioritiesInUse.Contains(priority))
            {
                priority++;
            }

            return priority;
        }

        private void cmbAccountSelection_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider2.Clear();
            CheckEnabled(this, EventArgs.Empty);
        }

        private void tbBarcode_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }
    }
}
