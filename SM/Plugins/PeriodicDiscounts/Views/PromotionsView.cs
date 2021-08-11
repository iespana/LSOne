using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.PeriodicDiscounts.Dialogs;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    public partial class PromotionsView : ViewBase
    {

        private RecordIdentifier selectedDiscountOfferID;
        private RecordIdentifier selectedPromotionLineID;

        public PromotionsView(RecordIdentifier id)
            : this()
        {
            selectedDiscountOfferID = id;
        }

        public PromotionsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;


            lvOffers.Columns[0].Tag = DiscountOfferSorting.OfferNumber;
            lvOffers.Columns[1].Tag = DiscountOfferSorting.Description;
            lvOffers.Columns[2].Tag = DiscountOfferSorting.Status;
            lvOffers.Columns[3].Tag = DiscountOfferSorting.DiscountValidationPeriod;
            lvOffers.Columns[4].Tag = DiscountOfferSorting.StartingDate;
            lvOffers.Columns[5].Tag = DiscountOfferSorting.EndingDate;
            lvOffers.SetSortColumn(0, true);

            lvOffers.ContextMenuStrip = new ContextMenuStrip();
            lvOffers.ContextMenuStrip.Opening += lvOffers_Opening;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            HeaderText = Properties.Resources.Promotions;
            //HeaderIcon = Properties.Resources.PriceTag16;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts);

            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("DiscountOffers", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion, Properties.Resources.Promotions, false));
            contexts.Add(new AuditDescriptor("PromotionLine", RecordIdentifier.Empty, Properties.Resources.PromotionLines, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Promotions;
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

            LoadItems(selectedDiscountOfferID);
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
            if (objectName == "PromotionOffer")
            {
                LoadLines();
            }
        }

        private void LoadItems(RecordIdentifier idToSelect)
        {            
            lvOffers.ClearRows();

            List<DiscountOffer> offers = Providers.DiscountOfferData.GetOffers(PluginEntry.DataModel,
                                                                     DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion,
                                                                     (DiscountOfferSorting)lvOffers.SortColumn.Tag,
                                                                     !lvOffers.SortedAscending);           

            foreach (DiscountOffer offer in offers)
            {
                Row row = new Row();

                row.AddText((string)offer.ID);
                row.AddText(offer.Text);
                row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                row.AddText((string)offer.ValidationPeriod + " - " + offer.ValidationPeriodDescription);
                row.AddCell(new DateTimeCell(offer.StartingDate.ToShortDateString(), offer.StartingDate.DateTime));
                row.AddCell(new DateTimeCell(offer.EndingDate.ToShortDateString(), offer.EndingDate.DateTime));
                row.AddText(offer.PriceGroupName);
                row.Tag = offer;                
                
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

                lvOffers.AddRow(row);                

                if (idToSelect == offer.ID)
                {
                    lvOffers.Selection.Set(lvOffers.RowCount - 1);
                }
            }

            lvOffers_SelectionChanged(this, EventArgs.Empty);
            lvOffers.AutoSizeColumns();            
        }

        private void LoadLines()
        {
            DecimalLimit priceLimiter;
            DecimalLimit discountLimiter;
            decimal standardPriceWithTax;
            RecordIdentifier itemID;
            bool itemExists = true;

            if (lvOffers.Selection.Count == 0)
            {
                return;
            }

            RecordIdentifier selectedID = selectedPromotionLineID;

            lvValues.ClearRows();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            discountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            List<PromotionOfferLine> lines = Providers.DiscountOfferLineData.GetPromotionLines(PluginEntry.DataModel, selectedDiscountOfferID, PromotionOfferLineSorting.Type, false);

            foreach (PromotionOfferLine line in lines)
            {
                Row row = new Row();

                itemExists = true;
                row.AddText(line.TypeText);
                //TODO: switch to oldtyle              
                row.AddText((string)line.ItemRelation);

                row.AddText(line.Text);
                row.AddText(line.VariantName??string.Empty);

                if (line.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item ||
                    line.Type == DiscountOfferLine.DiscountOfferTypeEnum.Variant)
                {
                    itemID = "";

                    itemID = line.TargetMasterID;

                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);

                    if (item == null)
                    {
                        itemExists = false;
                    }
                    else
                    {
                        standardPriceWithTax = line.StandardPrice +
                                               Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                                   .GetItemTax(PluginEntry.DataModel, item);
                        row.AddCell(new NumericCell(standardPriceWithTax.FormatWithLimits(priceLimiter), standardPriceWithTax));
                        row.AddCell(new NumericCell(line.DiscountPercent.FormatWithLimits(discountLimiter), line.DiscountPercent));

                        row.AddCell(new NumericCell(line.OfferPrice.FormatWithLimits(priceLimiter), line.OfferPrice));
                        row.AddCell(new NumericCell(line.OfferPriceIncludeTax.FormatWithLimits(priceLimiter), line.OfferPriceIncludeTax));
                        row.AddCell(new NumericCell(line.DiscountAmount.FormatWithLimits(priceLimiter), line.DiscountAmount));
                        row.AddCell(new NumericCell(line.DiscountamountIncludeTax.FormatWithLimits(priceLimiter), line.DiscountamountIncludeTax));
                    }

                }
                else
                {
                    row.AddText("");
                    row.AddCell(new NumericCell(line.DiscountPercent.FormatWithLimits(discountLimiter), line.DiscountPercent));

                    row.AddText("");
                    row.AddText("");
                    row.AddText("");
                    row.AddText("");
                }

                row.Tag = line.ID;                                

                for (int i = 0; i < row.CellCount; i++)
                {
                    if (row[(uint)i].Text == "0")
                    {
                        row[(uint)i].Text = "-";
                    }

                    if (!itemExists)
                    {
                        Style style = new Style(lvValues.DefaultStyle);
                        style.TextColor = ColorPalette.RedDark;
                        style.Font = new Font(style.Font, FontStyle.Bold);

                        row[(uint)i].SetStyle(style);
                    }
                }

                bool removeButtonEnabled = lvOffers.Selection.Count > 0 &&
                                           !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;

                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, removeButtonEnabled);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                lvValues.AddRow(row);

                if (line.ID == selectedID)
                {
                    lvValues.Selection.Set(lvValues.RowCount - 1);
                }
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
            btnsContextButtons.EditButtonEnabled = (lvOffers.Selection.Count > 0) && btnActivation.Enabled && !((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;

            if (lvOffers.Selection.Count > 0)
            {
                btnActivation.Text = ((DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled ? Properties.Resources.Disable : Properties.Resources.Enable;

                lblGroupHeader.Visible = true;
                lvValues.Visible = true;
                btnsContextButtonsItems.Visible = true;
                btnEditAll.Visible = true;
                btnEditAll.Enabled = !((DiscountOffer) lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag).Enabled;
                lblNoSelection.Visible = false;

                btnsContextButtonsItems.AddButtonEnabled = !ReadOnly && SelectedOffer.Enabled == false;

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

            selectedPromotionLineID = lvValues.Selection.Count > 0
                                          ? (RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag
                                          : "";
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
                        Enabled = btnsContextButtons.AddButtonEnabled
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete + "...",
                300,
                new EventHandler(btnRemove_Click))
                    {
                        Image = ContextButtons.GetRemoveButtonImage(),
                        Enabled = btnsContextButtons.RemoveButtonEnabled
                    };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                btnActivation.Text,
                500,
                new EventHandler(btnActivation_Click));

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

            PluginEntry.Framework.ContextMenuNotify("DiscountOfferLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }       

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.PromotionDialog dlg = new Dialogs.PromotionDialog(selectedDiscountOfferID);
            dlg.ShowDialog();
          
            LoadItems(selectedDiscountOfferID);

            if (dlg.DialogResult == DialogResult.OK && dlg.IsCustomerRelation)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this,DataEntityChangeType.Edit, "Customer",dlg.AccountRelation.PrimaryID,null);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.PromotionDialog dlg = new Dialogs.PromotionDialog();
            dlg.ShowDialog();
            LoadItems(dlg.OfferID);

            if (dlg.DialogResult == DialogResult.OK && dlg.IsCustomerRelation)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", dlg.AccountRelation.PrimaryID, null);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeletePromotionQuestion,
                Properties.Resources.DeletePromotion) == DialogResult.Yes)
            {
                Providers.DiscountOfferData.Delete(PluginEntry.DataModel, selectedDiscountOfferID);

                for (int i = 0; i < lvValues.Rows.Count; i++)
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvValues.Row(i).Tag);
                }

                DiscountOffer offer = (DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag;
                
                if(offer.AccountCode == DiscountOffer.AccountCodeEnum.Customer)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", offer.AccountRelation, null);
                }

                LoadItems(null);
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            RecordIdentifier currentlySelected = ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);            
            try
            {
                Dialogs.PromotionLineDialog dlg = Dialogs.PromotionLineDialog.CreateForEditing(SelectedOffer.ID, currentlySelected);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PromotionOffer", currentlySelected, null);
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
            Dialogs.PromotionLineDialog dlg = Dialogs.PromotionLineDialog.CreateForNew(SelectedOffer.ID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "PromotionOffer", RecordIdentifier.Empty, null);
                LoadLines();
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            string question = lvValues.Selection.Count > 1
                                  ? Properties.Resources.DeletePromotionLinesQuestion
                                  : Properties.Resources.DeletePromotionLineQuestion;

            string headerText = lvValues.Selection.Count > 1
                                    ? Properties.Resources.DeletePromotionLines
                                    : Properties.Resources.DeletePromotionLine;

            if (QuestionDialog.Show(question, headerText) == DialogResult.Yes)
            {
                while (lvValues.Selection.Count > 0)  
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).PrimaryID);
                    Providers.DiscountOfferLineData.DeletePromotion(PluginEntry.DataModel, ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).PrimaryID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PromotionOffer", ((RecordIdentifier)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).PrimaryID, null);
                    lvValues.RemoveRow(lvValues.Selection.FirstSelectedRow);  
                }

                lvValues.AutoSizeColumns(true);
            }
        }

        private void btnActivation_Click(object sender, EventArgs e)
        {
            DiscountOffer offer = (DiscountOffer)lvOffers.Row(lvOffers.Selection.FirstSelectedRow).Tag;

            Providers.DiscountOfferData.UpdateStatus(PluginEntry.DataModel, offer.ID, !offer.Enabled);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PromotionOffer", offer.ID, null);

            LoadItems(offer.ID);
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
        
        private void MultiEdit(object sender, EventArgs args)
        {
            PromotionLineMultiEditDialog dlg = new PromotionLineMultiEditDialog(selectedDiscountOfferID);
            dlg.ShowDialog();
            LoadLines();
        }

        private void RemoveOfferLine(int rowNumber)
        {
            if(QuestionDialog.Show(Properties.Resources.DeletePromotionLineQuestion, Properties.Resources.DeletePromotionLine) == DialogResult.Yes)
            {
                RecordIdentifier lineID = (RecordIdentifier)lvValues.Row(rowNumber).Tag;
                Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, lineID);
                Providers.DiscountOfferLineData.DeletePromotion(PluginEntry.DataModel, lineID);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PromotionOffer", lineID, null);
                lvValues.RemoveRow(rowNumber);

                lvValues.AutoSizeColumns(true);
            }
        }
        
    }
}
