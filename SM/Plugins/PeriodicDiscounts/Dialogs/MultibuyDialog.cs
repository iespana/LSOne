using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.PeriodicDiscounts.Properties;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MultibuyDialog : DialogBase
    {
        RecordIdentifier offerID;
        DiscountOffer offer;
        List<CachedOperation> changedLines;
        Dictionary<decimal, MultibuyDiscountLine> currentLines;
        private DiscountOffer.AccountCodeEnum currentAccountCode;
        private List<int> prioritiesInUse;
        private List<DiscountOffer> currentDiscountOffers; 

        WeakReference priceGroupEditor;

        public MultibuyDialog(RecordIdentifier offerID)
            : this()
        {
            List<MultibuyDiscountLine> lines;
            this.offerID = offerID;

            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy);

            tbDescription.Text = offer.Text;
            ntbPriority.Value = (double)offer.Priority;
            cmbDiscountType.SelectedIndex = (int)offer.DiscountType;

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

            lines = Providers.MultibuyDiscountLineData.GetAllForOffer(PluginEntry.DataModel, offerID);

            foreach (MultibuyDiscountLine line in lines)
            {
                AddLine(line);
            }

            CheckEnabled(this, EventArgs.Empty);

            lvValues.BestFitColumns();
        }

        public MultibuyDialog()
            : base()
        {
            offer = null;
            offerID = RecordIdentifier.Empty;

            InitializeComponent();

            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer);

            prioritiesInUse = Providers.DiscountOfferData.GetPrioritiesInUse(PluginEntry.DataModel);
            currentDiscountOffers = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel,
                                                                           true,
                                                                           DiscountOfferSorting.OfferNumber,
                                                                           false);

            cmbPriceGroup.SelectedData = new DataEntity("", "");
            cmbDiscountPeriodNumber.SelectedData = new DataEntity("", "");
            cmbDiscountType.SelectedIndex = 0;

            ntbPriority.Value = Providers.DiscountOfferData.GetNextPriority(PluginEntry.DataModel);

            currentLines = new Dictionary<decimal, MultibuyDiscountLine>();
            changedLines = new List<CachedOperation>();

            IPlugin plugin;
            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddPriceGroup", null);
            priceGroupEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddPriceGroup.Visible = (priceGroupEditor != null);
            currentAccountCode = DiscountOffer.AccountCodeEnum.None;
            cmbAccountCode.SelectedIndex = 0;
            cmbAccountSelection.SelectedData = new DataEntity("", "");
            cmbTriggering.SelectedIndex = 0;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            lvValues_SelectedIndexChanged(this, new EventArgs());
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
            if (cmbAccountCode.SelectedIndex != 0 && (cmbAccountSelection.SelectedData == null || cmbAccountSelection.SelectedData.ID.StringValue == ""))
            {
                errorProvider2.SetError(cmbAccountSelection, Properties.Resources.AccountSelectionCannotBeEmpty);
                return;
            }

            MultibuyDiscountLine line;
            bool editingExistingOffer = offer != null;

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
            offer.DiscountType = (DiscountOffer.PeriodicDiscountDiscountTypeEnum)cmbDiscountType.SelectedIndex;
            offer.ValidationPeriod = cmbDiscountPeriodNumber.SelectedData.ID;
            offer.OfferType = DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy;

            offer.PriceGroup = cmbPriceGroup.SelectedData.ID;
            offer.AccountCode = (DiscountOffer.AccountCodeEnum)cmbAccountCode.SelectedIndex;
            offer.AccountRelation = cmbAccountSelection.SelectedData.ID.HasSecondaryID ? cmbAccountSelection.SelectedData.ID[2] : cmbAccountSelection.SelectedData.ID;

            offer.Triggering = (DiscountOffer.TriggeringEnum)cmbTriggering.SelectedIndex;
            offer.BarCode = tbBarcode.Text;

            Providers.DiscountOfferData.Save(PluginEntry.DataModel, offer);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, editingExistingOffer ? DataEntityChangeType.Edit : DataEntityChangeType.Add, "PeriodicDiscount", offer.ID, offer.OfferType);

            offerID = offer.ID;

            foreach (CachedOperation operation in changedLines)
            {
                line = (MultibuyDiscountLine)operation.Entity;

                line.OfferID = offerID;

                if (operation.IsDeleteOperation)
                {
                    Providers.MultibuyDiscountLineData.Delete(PluginEntry.DataModel, line.ID);
                }
                else
                {
                    Providers.MultibuyDiscountLineData.Save(PluginEntry.DataModel, line);
                }
            }

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
                     ntbPriority.Value != (double)offer.Priority ||
                     cmbDiscountType.SelectedIndex != (int)offer.DiscountType ||
                     cmbDiscountPeriodNumber.SelectedData.ID != offer.ValidationPeriod ||
                     cmbPriceGroup.SelectedData.ID != offer.PriceGroup ||
                     changedLines.Count > 0 ||
                     cmbAccountCode.SelectedIndex != (int)offer.AccountCode ||
                     tbBarcode.Text != offer.BarCode ||
                     (cmbAccountSelection.SelectedData.ID.HasSecondaryID ? cmbAccountSelection.SelectedData.ID[2] != offer.AccountRelation : cmbAccountSelection.SelectedData.ID != offer.AccountRelation) ||
                     cmbTriggering.SelectedIndex != (int)offer.Triggering);

                if (ntbPriority.Text != "" && currentDiscountOffers.Any(p => p.ID != offer.ID && p.Priority == (int)ntbPriority.Value))
                {
                    errorProvider1.SetError(ntbPriority, Properties.Resources.PriorityAlreadyInUseNextIs.Replace("#1", GetNextAvailablePriority((int)ntbPriority.Value).ToString()));
                }
            }
            lblBarcode.Enabled = tbBarcode.Enabled = cmbTriggering.SelectedIndex == (int)DiscountOffer.TriggeringEnum.Manual;
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

        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MultibuyDiscountLine line;
            DecimalLimit secondColumnLimiter = PluginEntry.DataModel.GetDecimalSetting((cmbDiscountType.SelectedIndex == 1) ? DecimalSettingEnum.DiscountPercent : DecimalSettingEnum.Prices);

            CheckEnabled(null, EventArgs.Empty);

            lvValues.Columns[1].Text = cmbDiscountType.Text;

            foreach (ListViewItem item in lvValues.Items)
            {
                line = (MultibuyDiscountLine)item.Tag;

                item.SubItems[1].Text = line.PriceOrDiscountPercent.FormatWithLimits(secondColumnLimiter);
            }
        }

        private void AddLine(MultibuyDiscountLine line)
        {
            ListViewItem item;
            DecimalLimit secondColumnLimiter;
            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            secondColumnLimiter = PluginEntry.DataModel.GetDecimalSetting((cmbDiscountType.SelectedIndex == 1) ? DecimalSettingEnum.DiscountPercent : DecimalSettingEnum.Prices);

            item = new ListViewItem(((decimal)line.MinQuantity).FormatWithLimits(quantityLimiter));
            item.SubItems.Add(line.PriceOrDiscountPercent.FormatWithLimits(secondColumnLimiter));

            item.Tag = line;

            currentLines.Add((decimal)line.MinQuantity, line);

            lvValues.Add(item);
        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            MultibuyConfigurationDialog dlg = new MultibuyConfigurationDialog((cmbDiscountType.SelectedIndex) == 0 ? DiscountOffer.PeriodicDiscountDiscountTypeEnum.UnitPrice : DiscountOffer.PeriodicDiscountDiscountTypeEnum.DiscountPercent, currentLines);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddLine(dlg.DiscountLine);

                changedLines.Add(new CachedOperation(dlg.DiscountLine,false));

                lvValues.BestFitColumns();

                CheckEnabled(this, EventArgs.Empty);
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            MultibuyDiscountLine line;
            MultibuyConfigurationDialog dlg;
            DecimalLimit secondColumnLimiter;
            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            item = lvValues.SelectedItems[0];
            line = (MultibuyDiscountLine)item.Tag;
            MultibuyDiscountLine prevLine = (MultibuyDiscountLine)line.Clone();

            dlg = new MultibuyConfigurationDialog(line,(cmbDiscountType.SelectedIndex) == 0 ? DiscountOffer.PeriodicDiscountDiscountTypeEnum.UnitPrice : DiscountOffer.PeriodicDiscountDiscountTypeEnum.DiscountPercent, currentLines);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                secondColumnLimiter = PluginEntry.DataModel.GetDecimalSetting((cmbDiscountType.SelectedIndex == 1) ? DecimalSettingEnum.DiscountPercent : DecimalSettingEnum.Prices);

                item.Text = ((decimal)line.MinQuantity).FormatWithLimits(quantityLimiter);
                item.SubItems[1].Text = line.PriceOrDiscountPercent.FormatWithLimits(secondColumnLimiter);

                changedLines.Add(new CachedOperation(line, false));

                if (line.MinQuantity != prevLine.MinQuantity)
                {
                    // MinQuantity is part of the primary key so we need to delete the previous line so that we don't create additional multibuy lines
                    changedLines.Add(new CachedOperation(prevLine, true));
                }

                lvValues.BestFitColumns();

                CheckEnabled(this, EventArgs.Empty);
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
               Properties.Resources.RemoveConfigurationQuestion,
               Properties.Resources.RemoveConfiguration) == DialogResult.Yes)
            {
                changedLines.Add(new CachedOperation((MultibuyDiscountLine)lvValues.SelectedItems[0].Tag, true));
                currentLines.Remove((decimal)((MultibuyDiscountLine)lvValues.SelectedItems[0].Tag).MinQuantity);

                lvValues.Items.Remove(lvValues.SelectedItems[0]);
                
                lvValues_SelectedIndexChanged(null, EventArgs.Empty);

                CheckEnabled(this, EventArgs.Empty);
            }
        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = (lvValues.SelectedItems.Count == 1);
            btnsContextButtons.RemoveButtonEnabled = (lvValues.SelectedItems.Count >= 1);
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    100,
                    new EventHandler(btnAddValue_Click))
                    {
                        Image = ContextButtons.GetAddButtonImage(),
                        Enabled = btnsContextButtons.AddButtonEnabled
                    };

            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    new EventHandler(btnEditValue_Click))
                    {
                        Image = ContextButtons.GetEditButtonImage(),
                        Enabled = btnsContextButtons.EditButtonEnabled,
                        Default = true
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    new EventHandler(btnAddValue_Click))
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsContextButtons.AddButtonEnabled
            };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemoveValue_Click))
                    {
                        Image = ContextButtons.GetRemoveButtonImage(),
                        Enabled = btnsContextButtons.RemoveButtonEnabled
                    };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("MultibuyConfigurationList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvValues_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEditValue_Click(null, EventArgs.Empty);
            }
        }

        private void cmbPriceGroup_FormatData(object sender, DropDownFormatDataArgs e)
        {

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

        private void MultibuyDialog_Load(object sender, EventArgs e)
        {

        }

        private void cmbPriceGroup_FormatData_1(object sender, DropDownFormatDataArgs e)
        {
            e.TextToDisplay = (string)((DataEntity)e.Data).Text;
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
