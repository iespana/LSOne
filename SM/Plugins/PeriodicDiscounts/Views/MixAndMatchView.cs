using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.PeriodicDiscounts.Dialogs;
using LSOne.ViewPlugins.PeriodicDiscounts.Properties;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    public partial class MixAndMatchView : ViewBase
    {
        private RecordIdentifier selectedDiscountOfferID;
        private List<DiscountOffer> allOffers; 

        public MixAndMatchView(RecordIdentifier id)
            : this()
        {
            selectedDiscountOfferID = id;
        }

        public MixAndMatchView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            lvOffers.Columns[0].Tag = DiscountOfferSorting.OfferNumber;
            lvOffers.Columns[1].Tag = DiscountOfferSorting.Description;
            lvOffers.Columns[2].Tag = DiscountOfferSorting.Status;
            lvOffers.Columns[3].Tag = DiscountOfferSorting.Priority;
            lvOffers.Columns[4].Tag = DiscountOfferSorting.DiscountType;
            //lvOffers.Columns[5].Tag = DiscountOfferSorting.DiscountPercentValue;
            lvOffers.Columns[6].Tag = DiscountOfferSorting.NumberOfItemsNeeded;
            lvOffers.Columns[7].Tag = DiscountOfferSorting.DiscountValidationPeriod;
            lvOffers.SetSortColumn(0, true);

            lvOffers.ContextMenuStrip = new ContextMenuStrip();
            lvOffers.ContextMenuStrip.Opening += lvOffers_Opening;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts);

            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("MixAndMatchOffers", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch, Properties.Resources.MixAndMatch, false));
            contexts.Add(new AuditDescriptor("POSMMLINEGROUPS", RecordIdentifier.Empty, Properties.Resources.MixAndMatchLineGroups, false));
            contexts.Add(new AuditDescriptor("DiscountOfferLine", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch, Properties.Resources.MixAndMatchLines, false));
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.PeriodicDiscountPriorities, null, PluginOperations.ShowPeriodicDiscountPrioritiesView), 100);
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.MixAndMatch;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadItems();
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.MixAndMatch;
        }

        protected override bool DataIsModified()
        {
           return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "DiscountOffer":
                    LoadItems();
                    break;

                case "PeriodicDiscount":
                    DiscountOffer.PeriodicDiscountOfferTypeEnum offerType = (DiscountOffer.PeriodicDiscountOfferTypeEnum)param;

                    if (offerType == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                    {
                        LoadItems();
                    }

                    break;
            }
        }

        private void LoadItems()
        {
            RecordIdentifier selectedID = selectedDiscountOfferID;
            allOffers = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel, true, DiscountOfferSorting.OfferNumber, false);

            lvOffers.ClearRows();

            List<DiscountOffer> offers = Providers.DiscountOfferData.GetOffers(
                PluginEntry.DataModel,
                DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch, 
                (DiscountOfferSorting)lvOffers.SortColumn.Tag, 
                !lvOffers.SortedAscending);

            foreach (DiscountOffer offer in offers)
            {
                Row row = new Row();
                row.AddText((string)offer.ID);
                row.AddText(offer.Text);
                row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);

                if (allOffers.Any(p => p.ID != offer.ID && p.Priority == offer.Priority))
                {
                    IconButton iconButton = new IconButton(Properties.Resources.Warning16, Properties.Resources.ViewConflictingPriorities);
                    IconButtonCell ibtnCell = new IconButtonCell(iconButton,
                                                                 IconButtonCell.IconButtonIconAlignmentEnum.Left,
                                                                 offer.Priority.ToString());

                    row.AddCell(ibtnCell);
                }
                else
                {
                    row.AddCell(new NumericCell(offer.Priority.ToString(), offer.Priority));
                }

                row.AddText(offer.MixAndMatchDiscountTypeText);

                switch (offer.MixAndMatchDiscountType)
                {
                    case DiscountOffer.MixAndMatchDiscountTypeEnum.DealPrice:
                        row.AddCell(new NumericCell(offer.DealPrice.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)), offer.DealPrice));
                        break;

                    case DiscountOffer.MixAndMatchDiscountTypeEnum.DiscountPercent:
                        row.AddCell(new NumericCell(offer.DiscountPercent.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent)), offer.DiscountPercent));
                        break;

                    case DiscountOffer.MixAndMatchDiscountTypeEnum.DiscountAmount:
                        row.AddCell(new NumericCell(offer.DiscountAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)), offer.DiscountAmount));
                        break;

                    case DiscountOffer.MixAndMatchDiscountTypeEnum.LeastExpensive:
                        row.AddCell(new NumericCell(offer.NumberOfLeastExpensiveLines.ToString(), offer.NumberOfLeastExpensiveLines));
                        break;

                    case DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific:
                        row.AddText("");
                        break;

                    default:
                        row.AddText("");
                        break;
                }
                row.AddCell(new NumericCell(offer.NumberOfItemsNeeded.ToString(), offer.NumberOfItemsNeeded));

                row.AddText((string)offer.ValidationPeriod + " - " + offer.ValidationPeriodDescription);

                row.AddText(offer.PriceGroupName);

                if (offer.AccountCode == DiscountOffer.AccountCodeEnum.Customer)
                {
                    row.AddText(Properties.Resources.Customer + " - " + offer.CustomerName);
                }
                else if (offer.AccountCode == DiscountOffer.AccountCodeEnum.CustomerGroup)
                {
                    row.AddText(Properties.Resources.Group + " - " + offer.CustomerGroupName);
                }
                else
                {
                    row.AddText("");
                }

                row.AddText(offer.TriggeringText);

                row.AddText(offer.Triggering == DiscountOffer.TriggeringEnum.Manual ? offer.BarCode : "");

                row.Tag = offer;

                lvOffers.AddRow(row);

                if (offer.ID == selectedID)
                {
                    lvOffers.Selection.Set(lvOffers.RowCount - 1);
                }
            }

            lvOffers_SelectionChanged(this, EventArgs.Empty);
            lvOffers.AutoSizeColumns();

            if (lvOffers.Selection.Count > 0)
            {
                lvOffers.ScrollRowIntoView(lvOffers.Selection.FirstSelectedRow);
            }
        }

        private void LoadLines()
        {
            decimal taxAmount;
            decimal offerPriceWithTax;
            DecimalLimit priceLimiter;
            DecimalLimit discountLimiter;            
            RetailItem item = null;

            if (lvOffers.Selection.Count == 0)
            {
                return;
            }

            lvValues.ClearRows();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            discountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);


            List<DiscountOfferLine> lines = Providers.DiscountOfferLineData.GetMixAndMatchLines(PluginEntry.DataModel, SelectedOffer.ID,SelectedOffer.MixAndMatchDiscountType);

            foreach (DiscountOfferLine line in lines)
            {
                Row row = new Row();

                row.AddCell(new ColorBoxCell(5, line.LineColor, Color.Black));
                row.AddText(line.TypeText);            

                row.AddText((string)line.ItemRelation);
              
                row.AddText(line.Text);
                row.AddText(line.VariantName??string.Empty);
                if (SelectedOffer.MixAndMatchDiscountType == DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific)
                {
                    row.AddCell(new NumericCell(line.DiscountPercent.FormatWithLimits(line.DiscountType == DiscountOfferLine.MixAndMatchDiscountTypeEnum.DealPrice ? priceLimiter : discountLimiter), line.DiscountPercent));
                    row.AddText(line.DiscountTypeText);
                }

                row.AddText(line.MMGDescription);
                row.AddCell(new NumericCell(line.StandardPrice.FormatWithLimits(priceLimiter), line.StandardPrice));

                if (line.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
                {
                    item = Providers.RetailItemData.Get(PluginEntry.DataModel, line.TargetMasterID);
                }
               

                if (item != null)
                {
                    taxAmount = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTaxForAmount(PluginEntry.DataModel, item.SalesTaxItemGroupID, line.StandardPrice);

                    offerPriceWithTax = line.StandardPrice + taxAmount;
                    row.AddCell(new NumericCell(offerPriceWithTax.FormatWithLimits(priceLimiter), offerPriceWithTax));
                }
                else
                {
                    row.AddText("");
                }
             
                row.Tag = line.ID;                

                bool removeButtonEnabled = lvOffers.Selection.Count > 0 &&
                                           !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;

                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, removeButtonEnabled);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                lvValues.AddRow(row);
            }

            lvValues_SelectionChanged(this, EventArgs.Empty);
            lvValues.AutoSizeColumns(true);
        }

        private DiscountOffer SelectedOffer
        {
            get { return (DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag; }
        }

        private void lvOffers_SelectionChanged(object sender, EventArgs e)
        {
            selectedDiscountOfferID = (lvOffers.Selection.Count > 0) ? ((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).ID : "";
            btnActivation.Enabled = (lvOffers.Selection.Count > 0) && !ReadOnly;
            btnsContextButtons.EditButtonEnabled = btnActivation.Enabled && !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
            btnLineGroups.Enabled = (lvOffers.Selection.Count == 1) && !ReadOnly && !SelectedOffer.Enabled;

            if (lvOffers.Selection.Count > 0)
            {
                btnActivation.Text = ((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled ? Properties.Resources.Disable : Properties.Resources.Enable;

                
                lblGroupHeader.Visible = true;
                lvValues.Visible = true;
                btnsContextButtonsItems.Visible = true;
                btnEditAll.Visible = true;
                btnEditAll.Enabled = !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
                lblNoSelection.Visible = false;
                

                btnsContextButtonsItems.AddButtonEnabled = !ReadOnly && SelectedOffer.Enabled == false;

                if (((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).MixAndMatchDiscountType == DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific)
                {
                    if (lvValues.Columns.Count != 11)
                    {
                        lvValues.Columns.Clear();

                        var hdr = new Column(Resources.LineGroup, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.Type, 50, 10, 0, 0, true) {Clickable = true,InternalSort = true};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.ItemRelation, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.Description, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.Variant, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.DealPriceDiscountPct, 50, 10, 0, 0, true) { Clickable = false };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.DiscountType, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.LineGroup, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.PriceUnit, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.PriceUnitIncludingTax, 50, 10, 0, 0, true) { Clickable = false };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column("", 50, 10, 0, 10, false) {Clickable = false};
                        lvValues.Columns.Add(hdr);
                    }
                }
                else
                {
                    if (lvValues.Columns.Count != 9)
                    {
                        lvValues.Columns.Clear();

                        var hdr = new Column(Resources.LineGroup, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.Type, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.ItemRelation, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.Description, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.Variant, 50, 10, 0, 0, true) { Clickable = true, InternalSort = true };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.LineGroup, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.PriceUnit, 50, 10, 0, 0, true) {Clickable = false};
                        lvValues.Columns.Add(hdr);

                        hdr = new Column(Resources.PriceUnitIncludingTax, 50, 10, 0, 0, true) { Clickable = false };
                        lvValues.Columns.Add(hdr);

                        hdr = new Column("", 50, 10, 0, 10, false) {Clickable = false};
                        lvValues.Columns.Add(hdr);
                    }
                }

                LoadLines();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvValues.Visible = false;
                btnsContextButtonsItems.Visible = false;
                btnEditAll.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            bool offerIsActive = lvOffers.Selection.Count > 0 && SelectedOffer.Enabled;

            btnsContextButtonsItems.AddButtonEnabled = !offerIsActive && !ReadOnly;
            btnsContextButtonsItems.EditButtonEnabled = lvValues.Selection.Count == 1 && btnsContextButtonsItems.AddButtonEnabled;
            btnsContextButtonsItems.RemoveButtonEnabled = lvValues.Selection.Count > 0 && btnsContextButtonsItems.AddButtonEnabled;
        }

        void lvOffers_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvOffers.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                Properties.Resources.EditText + "...",
                100,
                btnEdit_Click)
                        {
                            Image = ContextButtons.GetEditButtonImage(),
                            Enabled = btnsContextButtons.EditButtonEnabled,
                            Default = true
                        };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add + "...",
                200,
                btnAdd_Click)
                    {
                        Image = ContextButtons.GetAddButtonImage(),
                        Enabled = btnsContextButtons.AddButtonEnabled
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete + "...",
                300,
                btnRemove_Click)
                    {
                        Image = ContextButtons.GetRemoveButtonImage(),
                        Enabled = btnsContextButtons.RemoveButtonEnabled
                    };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                btnActivation.Text,
                500,
                btnActivation_Click);

            item.Enabled = btnActivation.Enabled;

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                btnLineGroups.Text + "...",
                600,
                btnLineGroups_Click);

            item.Enabled = btnLineGroups.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.ViewPriorities,
                700,
                ViewPrioritiesHandler);

            item.Enabled = btnActivation.Enabled;

            menu.Items.Add(item);
                

            PluginEntry.Framework.ContextMenuNotify("DiscountOfferList", lvOffers.ContextMenuStrip, lvOffers);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                Properties.Resources.Add + "...",
                200,
                btnAddValue_Click)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsContextButtonsItems.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    btnEditValue_Click)
                    {
                        Image = ContextButtons.GetEditButtonImage(),
                        Enabled = btnsContextButtonsItems.EditButtonEnabled,
                        Default = true
                    };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemoveValue_Click)
                       {
                           Image = ContextButtons.GetRemoveButtonImage(),
                           Enabled = btnsContextButtonsItems.RemoveButtonEnabled
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.EditAll,
                    500,
                    MultiEdit)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtonsItems.AddButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DiscountOfferLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }       

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.MixAndMatchDialog dlg = new Dialogs.MixAndMatchDialog(selectedDiscountOfferID);
            dlg.ShowDialog();
            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.MixAndMatchDialog dlg = new Dialogs.MixAndMatchDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedDiscountOfferID = dlg.OfferID;
                LoadItems();
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteMixAndMatchQuestion,
                Properties.Resources.DeleteMixAndMatch) == DialogResult.Yes)
            {
                Providers.DiscountOfferData.Delete(PluginEntry.DataModel, selectedDiscountOfferID);

                for (int i = 0; i < lvValues.Rows.Count; i++)
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvValues.Row(i).Tag);
                }

                List<MixAndMatchLineGroup> mixAndMatchLineGroups = Providers.MixAndMatchLineGroupData.GetGroups(PluginEntry.DataModel, selectedDiscountOfferID, 0, false);

                foreach (MixAndMatchLineGroup mixAndMatchLineGroup in mixAndMatchLineGroups)
                {
                    Providers.MixAndMatchLineGroupData.Delete(PluginEntry.DataModel, mixAndMatchLineGroup.ID);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PeriodicDiscount", new RecordIdentifier(selectedDiscountOfferID), DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch);

                LoadItems();
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            RecordIdentifier currentlySelected = ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);

            try
            {
                Dialogs.MixAndMatchLineDialog dlg = new Dialogs.MixAndMatchLineDialog(SelectedOffer, SelectedOffer.MixAndMatchDiscountType, currentlySelected);

                if (dlg.ShowDialog() == DialogResult.OK || dlg.LineGroupsChanged)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "DiscountOffer", currentlySelected, null);
                    LoadLines();
                }
            }
            catch(DataIntegrityException ex)
            {
                if (ex.EntityType == typeof(Dimension))
                {
                    MessageDialog.Show(Properties.Resources.VariationNotExistsError);
                }
                else if(ex.EntityType == typeof(RetailItem))
                {
                    MessageDialog.Show(Properties.Resources.RetailItemNotExistsError);
                }
            }
        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            Dialogs.MixAndMatchLineDialog dlg = new Dialogs.MixAndMatchLineDialog(SelectedOffer, SelectedOffer.MixAndMatchDiscountType);

            if (dlg.ShowDialog() == DialogResult.OK || dlg.LineGroupsChanged)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "DiscountOffer", RecordIdentifier.Empty, null);
                LoadLines();
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            string question = lvValues.Selection.Count > 1
                                  ? Properties.Resources.DeleteDiscountOfferLinesQuestion
                                  : Properties.Resources.DeleteDiscountOfferLineQuestion;

            string headerText = lvValues.Selection.Count > 1
                                    ? Properties.Resources.DeleteDiscountOfferLines
                                    : Properties.Resources.DeleteDiscountOfferLine;

            if (QuestionDialog.Show(question, headerText) == DialogResult.Yes)
            {
                while (lvValues.Selection.Count > 0)
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "DiscountOffer", (RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag, null);
                    lvValues.RemoveRow(lvValues.Selection.FirstSelectedRow);
                }

                lvValues.AutoSizeColumns(true);
            }
        }

        private void btnActivation_Click(object sender, EventArgs e)
        { 
            DiscountOffer offer = (DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag;
            Providers.DiscountOfferData.UpdateStatus(PluginEntry.DataModel, offer.ID, !offer.Enabled);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PeriodicDiscount", selectedDiscountOfferID, DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch);
            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
        }

        private void btnLineGroups_Click(object sender, EventArgs e)
        {
            Dialogs.MixAndMatchLineGroupsDialog dlg = new Dialogs.MixAndMatchLineGroupsDialog(selectedDiscountOfferID);

            dlg.ShowDialog();

            if (dlg.Changed)
            {
                LoadLines();
            }
        }

        private void lvOffers_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvOffers.SortColumn == args.Column)
            {
                lvOffers.SetSortColumn(args.Column, !lvOffers.SortedAscending);
                LoadItems();
            }
            else
            {
                lvOffers.SetSortColumn(args.Column, true);
                LoadItems();
            }
        }

        private void ViewPrioritiesHandler(object sender, EventArgs args)
        {
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedDiscountOfferID);
        }

        private void lvOffers_CellAction(object sender, CellEventArgs args)
        {
            RecordIdentifier selectedID = ((DiscountOffer)lvOffers.Row(args.RowNumber).Tag).ID;
            // The only cell that can fire this event is the icon button cell tied to the priority column
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedID);
        }

        private void lvOffers_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvValues_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtonsItems.EditButtonEnabled)
            {
                btnEditValue_Click(this, EventArgs.Empty);
            }
        }

        private void lvValues_CellAction(object sender, CellEventArgs args)
        {
            if (btnsContextButtonsItems.RemoveButtonEnabled)
            {
                RemoveOfferLine(args.RowNumber);
            }
        }

        private void RemoveOfferLine(int rowNumber)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteDiscountOfferLineQuestion, Properties.Resources.DeleteDiscountOfferLine) == DialogResult.Yes)
            {
                RecordIdentifier lineID = (RecordIdentifier)lvValues.Row(rowNumber).Tag;
                Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, lineID);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "DiscountOffer", lineID, null);
                lvValues.RemoveRow(rowNumber);

                lvValues.AutoSizeColumns(true);
            }
        }

        private void MultiEdit(object sender, EventArgs args)
        {
            MixAndMatchLineMultiEditDialog dlg = new MixAndMatchLineMultiEditDialog(selectedDiscountOfferID);
            dlg.ShowDialog();
            LoadLines();
        }
    }
}
