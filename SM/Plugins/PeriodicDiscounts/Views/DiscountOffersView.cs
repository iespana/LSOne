using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
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

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    public partial class DiscountOffersView : ViewBase
    {
        private const int DefaultMaxLines = 100;
        private static object valueRowLock = new object();
        private RecordIdentifier selectedDiscountOfferID;
        private List<DiscountOffer> allOffers;

        public DiscountOffersView(RecordIdentifier id)
            : this()
        {
            selectedDiscountOfferID = id;
        }

        public DiscountOffersView()
        {
            InitializeComponent();

            lblLinesCount.Text = lblOfferCount.Text = "";

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;


            lvOffers.Columns[0].Tag = DiscountOfferSorting.OfferNumber;
            lvOffers.Columns[1].Tag = DiscountOfferSorting.Description;
            lvOffers.Columns[2].Tag = DiscountOfferSorting.Status;
            lvOffers.Columns[3].Tag = DiscountOfferSorting.DiscountPercentValue;
            lvOffers.Columns[4].Tag = DiscountOfferSorting.Priority;
            lvOffers.Columns[5].Tag = DiscountOfferSorting.DiscountValidationPeriod;
            lvOffers.Columns[6].Tag = DiscountOfferSorting.StartingDate;
            lvOffers.Columns[7].Tag = DiscountOfferSorting.EndingDate;
            lvOffers.SetSortColumn(0, true);

            lvOffers.ContextMenuStrip = new ContextMenuStrip();
            lvOffers.ContextMenuStrip.Opening += lvOffers_Opening;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            HeaderText = Properties.Resources.DiscountOffers;
          //  HeaderIcon = Properties.Resources.PriceTag16;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts);
            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("DiscountOffers", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer, Properties.Resources.DiscountOffers, false));
            contexts.Add(new AuditDescriptor("DiscountOfferLine", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer, Properties.Resources.DiscountOfferLines, false));
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
                return Properties.Resources.DiscountOffers;
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
            allOffers = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel, true, DiscountOfferSorting.OfferNumber, false);

            LoadItems();
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
                    LoadLines(DefaultMaxLines);
                    break;

                case "PeriodicDiscount":
                    var offerType = (DiscountOffer.PeriodicDiscountOfferTypeEnum)param;

                    if (offerType == DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer)
                    {
                        LoadData(false);
                    }

                    break;
            }
        }

        private void LoadItems()
        {
            RecordIdentifier selectedID = selectedDiscountOfferID;

            lvOffers.ClearRows();

            List<DiscountOffer> offers = Providers.DiscountOfferData.GetOffers(PluginEntry.DataModel, 
                                                                     DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer, 
                                                                     (DiscountOfferSorting)lvOffers.SortColumn.Tag, 
                                                                     !lvOffers.SortedAscending);

            lblOfferCount.Text = Properties.Resources.OfferCount.Replace("#1", offers.Count.ToString("n0"));

            foreach (DiscountOffer offer in offers)
            {
                Row row = new Row();

                row.AddText((string)offer.ID);
                row.AddText(offer.Text);
                row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                row.AddCell(new NumericCell(offer.DiscountPercent.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent)), offer.DiscountPercent));

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
                    row.AddText(offer.Priority.ToString());
                }

                row.AddText(offer.ValidationPeriodDescription);
                row.AddCell(new DateTimeCell(offer.StartingDate.ToShortDateString(), offer.StartingDate.DateTime));
                row.AddCell(new DateTimeCell(offer.EndingDate.IsEmpty ? "" : offer.EndingDate.ToShortDateString(), offer.EndingDate.DateTime));
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

            lvOffers.AutoSizeColumns();             
            lvOffers_SelectionChanged(this, EventArgs.Empty);

            if (lvOffers.Selection.Count > 0)
            {
                lvOffers.ScrollRowIntoView(lvOffers.Selection.FirstSelectedRow);
            }
        }

        private void LoadLines(int maxLines, int sortBy= 0, bool backwards = false)
        {
            DiscountLineSortEnum sorter;

            switch (sortBy)
            {
                case 0:
                    sorter = DiscountLineSortEnum.ProductType;
                    break;
                case 1:
                    sorter = DiscountLineSortEnum.TargetID;
                    break;
                case 2:
                    sorter = DiscountLineSortEnum.Description;
                    break;
                case 3:
                    sorter = DiscountLineSortEnum.VariantName;
                    break;
                default:
                    sorter = DiscountLineSortEnum.None;
                    break;
            }
            if (sorter != DiscountLineSortEnum.None)

            {
                sorter = (DiscountLineSortEnum)((int)sorter + (backwards ? 100 : 0)); // 100 added for Desc
            }
            if (lvOffers.Selection.Count == 0)
            {
                return;
            }

            lock (valueRowLock)
            {
                lvValues.ClearRows();

                var priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                var discountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

                int lineCount;
                List<DiscountOfferLine> lines = Providers.DiscountOfferLineData.GetLines(PluginEntry.DataModel,
                    SelectedOffer.ID, sorter, maxLines, out lineCount);

                btnLoadAll.Visible = lineCount > maxLines;

                if (maxLines < lineCount)
                    lblLinesCount.Text = Properties.Resources.OfferLinesLoaded.Replace("#1", maxLines.ToString("n0")).Replace("#2", lineCount.ToString("n0"));
                else
                    lblLinesCount.Text = Properties.Resources.OfferLinesCount.Replace("#1", lines.Count.ToString("n0"));

                var linesWithPrices =
                    Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                        .GetLinesWithPrices(PluginEntry.DataModel, lines);

                foreach (DiscountOfferLineWithPrice line in linesWithPrices)
                {
                    Row row = new Row();
                    row.AddText(line.DiscountOfferLine.TypeText);
                    row.AddText((string)line.DiscountOfferLine.ItemRelation);
                  
                    row.AddText(line.DiscountOfferLine.Text);

                    if (line.DiscountOfferLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
                    {
                        row.AddText(line.DiscountOfferLine.VariantName ?? string.Empty);
                       
                        row.AddCell(new NumericCell(line.StandardPriceWithTax.FormatWithLimits(priceLimiter), line.StandardPriceWithTax));
                        row.AddCell(new NumericCell(line.DiscountOfferLine.DiscountPercent.FormatWithLimits(priceLimiter), line.DiscountOfferLine.DiscountPercent));

                        row.AddCell(new NumericCell(line.OfferPriceWithTax.FormatWithLimits(priceLimiter), line.OfferPriceWithTax));
                        row.AddCell(new NumericCell(line.DiscountAmountWithTax.FormatWithLimits(priceLimiter), line.DiscountAmountWithTax));
                    }
                    else
                    {
                        row.AddText(string.Empty); // Variant info - not applicable here
                        row.AddText("");
                        row.AddCell(new NumericCell(line.DiscountOfferLine.DiscountPercent.FormatWithLimits(priceLimiter), line.DiscountOfferLine.DiscountPercent));

                        row.AddText("");
                        row.AddText("");
                    }

                    row.Tag = line.DiscountOfferLine;

                    bool removeButtonEnabled = lvOffers.Selection.Count > 0 &&
                                               !((DiscountOffer) lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag)
                                                   .Enabled;

                    IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete),
                        Properties.Resources.Delete, removeButtonEnabled);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                    lvValues.AddRow(row);
                }


                lvValues_SelectionChanged(this, EventArgs.Empty);

                lvValues.AutoSizeColumns(true);
            }

         
        }
     
        private DiscountOffer SelectedOffer
        {
            get { return (DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag; }
        }       

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            bool offerIsActive = lvOffers.Selection.Count > 0 && SelectedOffer.Enabled;

            btnsContextButtonsItems.AddButtonEnabled = !offerIsActive && !ReadOnly;
            btnsContextButtonsItems.EditButtonEnabled = lvValues.Selection.Count == 1 && btnsContextButtonsItems.AddButtonEnabled;
            btnsContextButtonsItems.RemoveButtonEnabled = lvValues.Selection.Count > 0 && btnsContextButtonsItems.AddButtonEnabled;   
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
                Properties.Resources.ViewPriorities,
                600,
                ViewPrioritiesHandler);

            item.Enabled = btnActivation.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DiscountOfferList", lvOffers.ContextMenuStrip, lvOffers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DiscountOfferDialog dlg = new DiscountOfferDialog(selectedDiscountOfferID);

            dlg.ShowDialog();

            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.DiscountOfferDialog dlg = new Dialogs.DiscountOfferDialog();
            dlg.ShowDialog();
          
            selectedDiscountOfferID = dlg.OfferID;
            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);


        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteDiscountOfferQuestion,
                Properties.Resources.DeleteDiscountOffer) == DialogResult.Yes)
            {
                Providers.DiscountOfferData.Delete(PluginEntry.DataModel, selectedDiscountOfferID);

                for(int i = 0; i < lvValues.Rows.Count; i++)
                {
                    RecordIdentifier lineID = ((DiscountOfferLine)lvValues.Row(i).Tag).ID;
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, lineID);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PeriodicDiscount", new RecordIdentifier(selectedDiscountOfferID), DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer);

                selectedDiscountOfferID = RecordIdentifier.Empty;
                LoadItems();

                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            RecordIdentifier currentlySelected = ((DiscountOfferLine)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;

            try
            {
                DiscountOfferLineDialog dlg = new DiscountOfferLineDialog(SelectedOffer, currentlySelected);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "DiscountOffer", currentlySelected, null);
                    LoadLines(DefaultMaxLines);
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
            DiscountOfferLineDialog dlg = new DiscountOfferLineDialog(SelectedOffer);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(
                    
                    this, 
                    DataEntityChangeType.Add, 
                    "DiscountOffer",
                    RecordIdentifier.Empty, 
                    null);
            LoadLines(DefaultMaxLines);
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
                    var id = ((DiscountOfferLine) lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, id);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "DiscountOffer", id, null);
                    lvValues.RemoveRow(lvValues.Selection.FirstSelectedRow);
                }
                lvValues.AutoSizeColumns(true);
            }
        }

        private void btnActivation_Click(object sender, EventArgs e)
        {
            DiscountOffer offer = (DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag;

            Providers.DiscountOfferData.UpdateStatus(PluginEntry.DataModel, offer.ID, !offer.Enabled);
            selectedDiscountOfferID = offer.ID;
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PeriodicDiscount", selectedDiscountOfferID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer);
            LoadItems();

            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
        }

        private void lvOffers_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvOffers.SortColumn == args.Column)
            {
                lvOffers.SetSortColumn(args.Column, !lvOffers.SortedAscending);
                LoadData(false);
            }
            else
            {
                lvOffers.SetSortColumn(args.Column, true);
                LoadData(false);
            }
        }

        private void lvOffers_SelectionChanged(object sender, EventArgs e)
        {
            selectedDiscountOfferID = (lvOffers.Selection.Count > 0) ? ((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).ID : "";
            btnActivation.Enabled = (lvOffers.Selection.Count > 0) && !ReadOnly;
            btnsContextButtons.EditButtonEnabled = lvOffers.Selection.Count > 0 && btnActivation.Enabled && !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
            btnsContextButtonsItems.AddButtonEnabled = lvOffers.Selection.Count > 0 && btnActivation.Enabled && !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;


            if (lvOffers.Selection.Count > 0)
            {
                btnActivation.Text = ((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled ? Properties.Resources.Disable : Properties.Resources.Enable;

                
                lblGroupHeader.Visible = true;
                lvValues.Visible = true;
                btnsContextButtonsItems.Visible = true;
                btnEditAll.Visible = true;
                btnEditAll.Enabled = !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
                lblNoSelection.Visible = false;

                btnsContextButtonsItems.AddButtonEnabled = !ReadOnly && !SelectedOffer.Enabled;

                LoadLines(DefaultMaxLines);
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvValues.Visible = false;
                btnsContextButtonsItems.Visible = false;
                btnEditAll.Visible = false;
                lblNoSelection.Visible = true;
            }

            lvValues_SelectionChanged(sender, e);
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

        private void RemoveOfferLine(int rowNumber)
        {
            if (QuestionDialog.Show(Properties.Resources.DeletePromotionLineQuestion, Properties.Resources.DeletePromotionLine) == DialogResult.Yes)
            {
                RecordIdentifier lineID = ((DiscountOfferLine)lvValues.Row(rowNumber).Tag).ID;
                Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, lineID);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "DiscountOffer", lineID, null);
                lvValues.RemoveRow(rowNumber);

                lvValues.AutoSizeColumns(true);  
            }
        }
        private void lvValues_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvValues.SortColumn == args.Column)
            {
                lvValues.SetSortColumn(args.Column, !lvValues.SortedAscending); 
            }
            else
            {
                lvValues.SetSortColumn(args.Column, true);
            }

            LoadLines(DefaultMaxLines, args.ColumnNumber, !lvValues.SortedAscending);
        }
        private void lvValues_CellAction(object sender, CellEventArgs args)
        {
            if (btnsContextButtonsItems.RemoveButtonEnabled)
            {
                RemoveOfferLine(args.RowNumber);
            }
        }

        private void MultiEdit(object sender, EventArgs args)
        {
            var dlg = new DiscountOfferLineMultiEditDialog(selectedDiscountOfferID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer);
            dlg.ShowDialog();
            LoadLines(DefaultMaxLines);
        }

        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            LoadLines(Int32.MaxValue);
        }
    }
}
