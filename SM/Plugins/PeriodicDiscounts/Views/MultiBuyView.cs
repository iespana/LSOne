using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
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
    public partial class MultiBuyView : ViewBase
    {

        private RecordIdentifier selectedDiscountOfferID;
        private RecordIdentifier selectedTaxCode;
        private List<DiscountOffer> allOffers; 

        public MultiBuyView(RecordIdentifier id)
            : this()
        {
            selectedDiscountOfferID = id;
        }

        public MultiBuyView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            lvValues.SetSortColumn(0, true);

            lvOffers.Columns[0].Tag = DiscountOfferSorting.OfferNumber;
            lvOffers.Columns[1].Tag = DiscountOfferSorting.Description;
            lvOffers.Columns[2].Tag = DiscountOfferSorting.Status;
            lvOffers.Columns[3].Tag = DiscountOfferSorting.DiscountType;
            lvOffers.Columns[4].Tag = DiscountOfferSorting.Priority;
            lvOffers.Columns[5].Tag = DiscountOfferSorting.DiscountValidationPeriod;
            lvOffers.Columns[6].Tag = DiscountOfferSorting.StartingDate;
            lvOffers.Columns[7].Tag = DiscountOfferSorting.EndingDate;
            lvOffers.SetSortColumn(0, true);
            
            lvOffers.ContextMenuStrip = new ContextMenuStrip();
            lvOffers.ContextMenuStrip.Opening += lvOffers_Opening;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts);

            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;
            selectedDiscountOfferID = RecordIdentifier.Empty;

            HeaderText = Properties.Resources.Multibuy;
            //HeaderIcon = Properties.Resources.PriceTag16;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("DiscountOffers", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy, Properties.Resources.Multibuy, false));
            contexts.Add(new AuditDescriptor("MultibuyDiscountLine", 0, Properties.Resources.MultibuyConfigurations, false));
            contexts.Add(new AuditDescriptor("DiscountOfferLine", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy, Properties.Resources.MultibuyLines, false));
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
                return Properties.Resources.Multibuy;
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
                    LoadLines(lvValues.Columns.IndexOf(lvValues.SortColumn), !lvValues.SortedAscending);
                    break;

                case "PeriodicDiscount":
                    DiscountOffer.PeriodicDiscountOfferTypeEnum offerType = (DiscountOffer.PeriodicDiscountOfferTypeEnum)param;

                    if (offerType == DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy)
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
                                                                     DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy, 
                                                                     (DiscountOfferSorting)lvOffers.SortColumn.Tag, 
                                                                     !lvOffers.SortedAscending );

            foreach (DiscountOffer offer in offers)
            {                
                Row row = new Row();

                row.AddText((string)offer.ID);
                row.AddText(offer.Text);
                row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                row.AddText(offer.DiscountTypeText);


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

                row.AddText((string)offer.ValidationPeriod + " - " + offer.ValidationPeriodDescription);
                row.AddCell(new DateTimeCell(offer.StartingDate.ToShortDateString(), offer.StartingDate.DateTime));
                row.AddCell(new DateTimeCell(offer.EndingDate.ToShortDateString(), offer.EndingDate.DateTime));
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

        private void LoadLines(int sortBy, bool backwards)
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
                sorter = (DiscountLineSortEnum)((int)sorter + (backwards ? 0 : 100)); // 100 added for Desc
            }

            if (lvOffers.Selection.Count <= 0)
            {
                return;
            }

            lvValues.ClearRows();

            int lineCount;
            List<DiscountOfferLine> lines = Providers.DiscountOfferLineData.GetLines(PluginEntry.DataModel, SelectedOffer.ID, sorter, Int32.MaxValue, out lineCount);

            foreach (DiscountOfferLine line in lines)
            {
                Row row = new Row();

                row.AddText(line.TypeText);
                //TODO Switch to oldstyæe
                row.AddText((string)line.ItemRelation);
             
                row.AddText(line.Text);
                row.AddText(line.VariantName??string.Empty);
                row.AddText(RecordIdentifier.IsEmptyOrNull(line.Unit) ? string.Empty : Providers.UnitData.GetUnitDescription(PluginEntry.DataModel, line.Unit));
                
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

            btnsContextButtons.EditButtonEnabled = lvOffers.Selection.Count > 0 && btnActivation.Enabled && !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;

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

                LoadLines(lvValues.Columns.IndexOf(lvValues.SortColumn), !lvValues.SortedAscending);
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
            selectedTaxCode = (lvValues.Selection.Count > 0) ? ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).SecondaryID : "";

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
                new EventHandler(btnEdit_Click))
                        {
                            Image = ContextButtons.GetEditButtonImage(),
                            Enabled = btnsContextButtons.EditButtonEnabled,
                            Default = true
                        };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add + "...",
                200,
                new EventHandler(btnAdd_Click))
                    {
                        Image = ContextButtons.GetAddButtonImage(),
                        Enabled = btnsContextButtons.AddButtonEnabled,
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete + "...",
                300,
                new EventHandler(btnRemove_Click))
                    {
                        Image = ContextButtons.GetRemoveButtonImage(),
                        Enabled = btnsContextButtons.RemoveButtonEnabled,
                    };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                btnActivation.Text,
                500,
                new EventHandler(btnActivation_Click));

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
                        Enabled = btnsContextButtonsItems.AddButtonEnabled
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    new EventHandler(btnEditValue_Click))
                    {
                        Image = ContextButtons.GetEditButtonImage(),
                        Enabled = btnsContextButtonsItems.EditButtonEnabled,
                        Default = true
                    };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemoveValue_Click))
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

            PluginEntry.Framework.ContextMenuNotify("MultibuyOfferLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }        

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.MultibuyDialog dlg = new Dialogs.MultibuyDialog(selectedDiscountOfferID);
            dlg.ShowDialog();
            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.MultibuyDialog dlg = new Dialogs.MultibuyDialog();
            dlg.ShowDialog();
            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteMultibuyQuestion,
                Properties.Resources.DeleteMultibuy) == DialogResult.Yes)
            {
                Providers.DiscountOfferData.Delete(PluginEntry.DataModel, selectedDiscountOfferID);

                for (int i = 0; i < lvValues.Rows.Count; i++)
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvValues.Row(i).Tag);
                }

                List<MultibuyDiscountLine> multibuyDiscountLines = Providers.MultibuyDiscountLineData.GetAllForOffer(PluginEntry.DataModel, selectedDiscountOfferID);
                foreach (MultibuyDiscountLine multibuyDiscountLine in multibuyDiscountLines)
                {
                    Providers.MultibuyDiscountLineData.Delete(PluginEntry.DataModel, multibuyDiscountLine.ID);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PeriodicDiscount", new RecordIdentifier(selectedDiscountOfferID), DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy);

                LoadItems();
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            RecordIdentifier currentlySelected = ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);

            try
            {
                Dialogs.MultibuyLineDialog dlg = new Dialogs.MultibuyLineDialog(SelectedOffer.ID, currentlySelected);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "DiscountOffer", currentlySelected, null);
                    LoadLines(lvValues.Columns.IndexOf(lvValues.SortColumn), !lvValues.SortedAscending);
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
            Dialogs.MultibuyLineDialog dlg = new Dialogs.MultibuyLineDialog(SelectedOffer.ID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "DiscountOffer", RecordIdentifier.Empty, null);
                LoadLines(lvValues.Columns.IndexOf(lvValues.SortColumn), !lvValues.SortedAscending);
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            string question = lvValues.Selection.Count > 1
                                  ? Properties.Resources.DeleteMultibuyLinesQuestion
                                  : Properties.Resources.DeleteMultibuyLineQuestion;

            string headerText = lvValues.Selection.Count > 1
                                    ? Properties.Resources.DeleteMultibuyLines
                                    : Properties.Resources.DeleteMultibuyLine;


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
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PeriodicDiscount", offer.ID, DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy);

            LoadItems();
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);
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
    
        private void lvOffers_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvOffers_CellAction(object sender, CellEventArgs args)
        {
            RecordIdentifier selectedID = ((DiscountOffer)lvOffers.Row(args.RowNumber).Tag).ID;

            // The only cell that can fire this event is the icon button cell tied to the priority column
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedID);
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

        private void lvValues_CellAction(object sender, CellEventArgs args)
        {
            if (btnsContextButtonsItems.RemoveButtonEnabled)
            {
                RemoveOfferLine(args.RowNumber);
            }
        }

        private void RemoveOfferLine(int rowNumber)
        {
            if (QuestionDialog.Show(Properties.Resources.DeletePromotionLineQuestion, Properties.Resources.DeletePromotionLine) == DialogResult.Yes)
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
            DiscountOfferLineMultiEditDialog dlg = new DiscountOfferLineMultiEditDialog(selectedDiscountOfferID, DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy);
            dlg.ShowDialog();
            LoadLines(lvValues.Columns.IndexOf(lvValues.SortColumn), !lvValues.SortedAscending);
        }
    }
}
