using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts.MultiEditing;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    internal partial class ItemSalesPricePage : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        RecordIdentifier itemId;

        RecordIdentifier selectedID = "";
        WeakReference owner;
        WeakReference customerViewer;
        WeakReference tradeAgreementViewer;
        WeakReference groupViewer;
        RecordIdentifier defaulStoreSalesTaxGroup;

        private class ExtendedCellWithFlags : ExtendedCell
        {
            public bool Saved;
            public bool AddAction;
        }

        public ItemSalesPricePage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public ItemSalesPricePage()
        {
            InitializeComponent();

            lvItemAgreements.ContextMenuStrip = new ContextMenuStrip();
            lvItemAgreements.ContextMenuStrip.Opening += lvItems_Opening;

            IPlugin tradePlugin = PluginEntry.Framework.FindImplementor(this, "CanAddTradeAgreementItem", null);
            tradeAgreementViewer = (tradePlugin != null) ? new WeakReference(tradePlugin) : null;
            btnsContextButtons.AddButtonEnabled = tradeAgreementViewer != null;
            
            IPlugin groupPlugin = PluginEntry.Framework.FindImplementor(this, "CustomerPriceDiscGroups", null);
            groupViewer = (groupPlugin != null) ? new WeakReference(groupPlugin) : null;

            IPlugin customerPlugin = PluginEntry.Framework.FindImplementor(this, "CanEditCustomer", null);
            customerViewer = (customerPlugin != null)? new WeakReference(customerPlugin) : null;
            
        }

        private void RemoveAction(object sender, EventArgs args)
        {
            lvItemAgreements.RemoveRow(lvItemAgreements.Selection.FirstSelectedRow);
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItemAgreements.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            if (itemId == RecordIdentifier.Empty)
            {
                item = new ExtendedMenuItem(
                    Properties.Resources.RemoveAction,
                    330, new EventHandler(RemoveAction));

                item.Image = PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete);

                item.Enabled = lvItemAgreements.Selection.Count > 0 && (!((ExtendedCellWithFlags)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow)[9]).Saved);

                menu.Items.Add(item);
            }

            menu.Items.Add(new ExtendedMenuItem("-", 400));


            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.ViewCustomer + "...",
                600,
                new EventHandler(btnViewCustomer_Click));

            item.Enabled = btnViewCustomer.Enabled;

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.ViewGroup + "...",
                    700,
                    new EventHandler(btnViewGroup_Click));


            item.Enabled = btnViewGroup.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TradeAgreementSalesPriceList", lvItemAgreements.ContextMenuStrip, lvItemAgreements);

            e.Cancel = (menu.Items.Count == 0);
        }


        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.ItemSalesPricePage((TabControl)sender);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            itemId = context;
            LoadItems();
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {

        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            RetailItem item = (RetailItem)internalContext;

            itemId = item.ID;

            if (item.ID == RecordIdentifier.Empty)
            {
                // We are in multi edit mode
                btnsContextButtons.RemoveButtonEnabled = true;

                // In multiedit we cannot sort since that would distory the time order of the transactions.
                for (int i = 0; i < lvItemAgreements.Columns.Count; i++)
                {
                    lvItemAgreements.Columns[i].Clickable = false;
                    lvItemAgreements.Columns[i].InternalSort = false;
                }

                lvItemAgreements.Columns.Add(new LSOne.Controls.Columns.Column() { HeaderText = Properties.Resources.Action, Clickable = false, AutoSize = true, Sizable = false });
                lvItemAgreements.Columns.Add(new LSOne.Controls.Columns.Column() { HeaderText = "", Clickable = false, AutoSize = true, Sizable = false, MinimumWidth = 20 });

                lvItemAgreements.AutoSizeColumns();

                lvItemAgreements.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRoundedColorMerge;
            }
        }

        private void LoadItems()
        {
            DecimalLimit priceLimiter;
            DecimalLimit qtyLimiter;

            List<TradeAgreementEntry> items = null;

            lvItemAgreements.ClearRows();
            items = Providers.TradeAgreementData.GetForItem(PluginEntry.DataModel, itemId, TradeAgreementRelation.PriceSales);

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            foreach (TradeAgreementEntry item in items)
            {

                Row row = new Row();

                row.AddText((string)item.Currency);
                row.AddText((item.AccountCode == TradeAgreementEntryAccountCode.All)
                    ? item.AccountCodeText
                    : item.AccountCodeText + " - " + item.AccountName);
                row.AddText(item.VariantName);
                row.AddText(item.UnitDescription);
                row.AddCell(new DateTimeCell(item.FromDate.ToShortDateString(), item.FromDate.DateTime));
                row.AddCell(new DateTimeCell(item.ToDate.IsEmpty ? "" : item.ToDate.ToShortDateString(), item.ToDate.DateTime));
                row.AddCell(new NumericCell(item.QuantityAmount.FormatWithLimits(qtyLimiter), item.QuantityAmount));
                row.AddCell(new NumericCell(item.Amount.FormatWithLimits(priceLimiter), item.Amount));
                row.AddCell(new NumericCell(item.AmountIncludingTax.FormatWithLimits(priceLimiter), item.AmountIncludingTax));

                row.Tag = item;
                lvItemAgreements.AddRow(row);
               
            }
            lvItemAgreements.AutoSizeColumns(true);
            btnViewCustomer.Enabled =btnViewGroup.Enabled = false;
        }
        
        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("TradeAgreementsSalesPriceItems", new RecordIdentifier(0, new RecordIdentifier(itemId.PrimaryID, 4)), Properties.Resources.TradeAgreements, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "TradeAgreement")
            {
                LoadItems();   
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void AddMultiEditRecord(TradeAgreementEntry agreement, bool addAction)
        {
            DecimalLimit qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            Row row = new Row();

            row.BackColor = ColorPalette.MultiEditHighlight;

            row.AddText((string)agreement.Currency);
            row.AddText((agreement.AccountCode == TradeAgreementEntryAccountCode.All)
                ? agreement.AccountCodeText
                : agreement.AccountCodeText + " - " + agreement.AccountName);
            row.AddText(agreement.VariantName);
            row.AddText(Properties.Resources.SameAsOnItem);
            row.AddCell(new DateTimeCell(agreement.FromDate.ToShortDateString(), agreement.FromDate.DateTime));
            row.AddCell(new DateTimeCell(agreement.ToDate.IsEmpty ? "" : agreement.ToDate.ToShortDateString(), agreement.ToDate.DateTime));
            row.AddCell(new NumericCell(agreement.QuantityAmount.FormatWithLimits(qtyLimiter), agreement.QuantityAmount));
          

        

            if (addAction)
            {
                DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                
                if((agreement as TradeAgreementEntryMultiEdit).CalculateFromPrice)
                {
                    row.AddCell(new NumericCell(agreement.Amount.FormatWithLimits(priceLimiter), agreement.Amount));
                    row.AddText("");
                }
                else
                {
                    row.AddText("");
                    row.AddCell(new NumericCell(agreement.AmountIncludingTax.FormatWithLimits(priceLimiter), agreement.AmountIncludingTax));
                }
                
            }
            else
            {
                row.AddText("");
                row.AddText("");
            }

            row.AddCell(new ExtendedCellWithFlags() { Image = addAction ? ContextButtons.GetAddButtonImage() : ContextButtons.GetRemoveButtonImage(), Saved = false, AddAction = addAction, Text = addAction ? Properties.Resources.Add : Properties.Resources.Delete });
            row.AddCell(new IconButtonCell(new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.RemoveAction), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter));

            row.Tag = agreement;
            lvItemAgreements.AddRow(row);

            lvItemAgreements.AutoSizeColumns(true);
        }
    
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tradeAgreementViewer.IsAlive)
            {
                Dialogs.TradeAgreementItemDialog addDlg = Dialogs.TradeAgreementItemDialog.CreateForAdding(itemId);
                if (addDlg.ShowDialog() == DialogResult.OK)
                {
                    if (itemId == RecordIdentifier.Empty)
                    {
                        // We are in multiedit
                        AddMultiEditRecord(addDlg.AgreementEntry, true);
                    }
                    else
                    {
                        // We are in single edit
                        selectedID = addDlg.ID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                        LoadItems();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tradeAgreementViewer.IsAlive)
            {

                Dialogs.TradeAgreementItemDialog editDlg = Dialogs.TradeAgreementItemDialog.CreateForEditing(itemId, ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).ID);

                if (editDlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).ID, null);
                    LoadItems();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (itemId == RecordIdentifier.Empty)
            {
                // Multiedit

                // In multiedit then we are basically just sceduling delete transaction so we pop up dialog to define what kind of keys should be deleted.

                Dialogs.TradeAgreementItemDialog dlg = Dialogs.TradeAgreementItemDialog.CreateForDeleting(itemId);

                if(dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    AddMultiEditRecord(dlg.AgreementEntry, false);
                }
            }
            else
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteTradeAgreementQuestion,
                    Properties.Resources.DeleteTradeAgreement) == DialogResult.Yes)
                {
                    Providers.TradeAgreementData.Delete(
                        PluginEntry.DataModel,
                        ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).ID,
                        Permission.ManageTradeAgreementPrices);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "TradeAgreement", ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).ID, null);

                    LoadItems();

                }
            }
        }

        private void btnViewCustomer_Click(object sender, EventArgs e)
        {
            if (customerViewer.IsAlive)
            {
                List<IDataEntity> list = new List<IDataEntity>();
                foreach (var row in lvItemAgreements.Rows)
                {
                    if (PluginOperations.ListViewItemIsCustomer((TradeAgreementEntry)row.Tag))
                    {
                        list.Add(new DataEntity(((TradeAgreementEntry)row.Tag).AccountRelation, ""));
                    }
                }
                //foreach (ListViewItem item in lvItems.Items)
                //{
                //    if (PluginOperations.ListViewItemIsCustomer(item))
                //    {
                //        list.Add(new DataEntity(((TradeAgreementEntry)item.Tag).AccountRelation, ""));
                //    }
                //}

                ((IPlugin)customerViewer.Target).Message(this, "EditCustomer", new object[] { ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).AccountRelation, list });
            }
        }

        private void btnViewGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                RecordIdentifier groupID = ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).AccountRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer, new RecordIdentifier((int)PriceDiscGroupEnum.PriceGroup, groupID));
                PluginOperations.ShowPriceGroups(selectedID);
            }
        }


        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            foreach (Row row in lvItemAgreements.Rows)
            {
                if (!((ExtendedCellWithFlags)row[9]).Saved)
                {
                    return true;
                }
            }

            return false;
        }

        public void MultiEditSaveSecondaryRecords(DataLayer.GenericConnector.Interfaces.IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            decimal taxAmount;
            RecordIdentifier salesTaxItemGroupID;

            TradeAgreementEntryMultiEdit agreement;
            RetailItemMultiEdit item = (RetailItemMultiEdit)dataEntity;

            if(item.HasValidSalesTaxItemGroupID)
            {
                salesTaxItemGroupID = item.SalesTaxItemGroupID;
            }
            else
            {
                if(item.OldPrices == null)
                {
                    item.OldPrices = Providers.RetailItemData.GetItemPrice(threadedConnection, primaryRecordID.PrimaryID);
                }

                salesTaxItemGroupID = item.OldPrices.SalesTaxItemGroupID;
            }

            foreach (Row row in lvItemAgreements.Rows)
            {
                if (!((ExtendedCellWithFlags)row[9]).Saved)
                {
                    if (item.OldSalesUnit == null)
                    {
                        // We have no choice we need to look up the old sales unit
                        item.OldSalesUnit = Providers.RetailItemData.GetSalesUnitID(threadedConnection, primaryRecordID);
                    }

                    agreement = (TradeAgreementEntryMultiEdit)row.Tag;
                    agreement.ID = RecordIdentifier.Empty; // Force new record
                    agreement.ItemRelation = primaryRecordID.SecondaryID;
                    agreement.UnitID = item.OldSalesUnit;
                   
                    if (((ExtendedCellWithFlags)row[9]).AddAction)
                    {
                        // We are adding new aggreement

                        if (defaulStoreSalesTaxGroup == null)
                        {
                            defaulStoreSalesTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(threadedConnection);
                        }

                        // We need to calculate the trade aggreement either from price or from price with tax depending on how the user entered it.
                        if (agreement.CalculateFromPrice)
                        {
                            taxAmount = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculateTax(threadedConnection, agreement.Amount, salesTaxItemGroupID, defaulStoreSalesTaxGroup);

                            agreement.AmountIncludingTax = agreement.Amount + taxAmount;
                        }
                        else
                        {
                            agreement.Amount = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceFromPriceWithTax(threadedConnection, agreement.AmountIncludingTax, salesTaxItemGroupID, defaulStoreSalesTaxGroup);
                        }

                        // We need to fetch the ID in case if same record exists, in which case updating is triggered.
                        agreement.ID = Providers.TradeAgreementData.GetTradeAgreementID(threadedConnection, agreement);

                        Providers.TradeAgreementData.Save(threadedConnection, agreement, Permission.ManageDiscounts);
                    }
                    else
                    {
                        // We are removing aggreement
                        Providers.TradeAgreementData.Delete(threadedConnection, agreement);
                    }
                }
            }
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            foreach (Row row in lvItemAgreements.Rows)
            {
                if (!((ExtendedCellWithFlags)row[9]).Saved)
                {
                    ((ExtendedCellWithFlags)row[9]).Saved = true;
                    row.BackColor = System.Drawing.Color.Empty;
                }

                row[10] = new Cell("");
            }

            lvItemAgreements.Invalidate();

            defaulStoreSalesTaxGroup = null;
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            if (control == lvItemAgreements)
            {
                if (lvItemAgreements.Selection.Count > 0)
                {
                    lvItemAgreements.RemoveRow(lvItemAgreements.Selection.FirstSelectedRow);
                }

                lvItemAgreements.Invalidate();
            }
        }

        private void lvItemAgreements_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvItemAgreements_SelectionChanged(object sender, EventArgs e)
        {
            if (lvItemAgreements.Selection.Count > 0)
            {
                selectedID = ((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).ID;
            }

            if (itemId == RecordIdentifier.Empty)
            {
                // Multiedit
                btnsContextButtons.EditButtonEnabled = false; // Never enabled in multi edit
            }
            else
            {
                btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = lvItemAgreements.Selection.Count > 0;
            }


            if (btnsContextButtons.RemoveButtonEnabled = lvItemAgreements.Selection.Count > 0 && (customerViewer != null))
            {
                btnViewCustomer.Enabled = PluginOperations.ListViewItemIsCustomer((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag);
            }
            else
            {
                btnViewCustomer.Enabled = false;
            }

            if (btnsContextButtons.RemoveButtonEnabled = lvItemAgreements.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
            {
                btnViewGroup.Enabled =
                    (((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).AccountCode == TradeAgreementEntryAccountCode.Group &&
                    (((TradeAgreementEntry)lvItemAgreements.Row(lvItemAgreements.Selection.FirstSelectedRow).Tag).AccountRelation != ""));
            }
            else
            {
                btnViewGroup.Enabled = false;
            }
        }

        private void lvItemAgreements_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            lvItemAgreements.RemoveRow(args.RowNumber);
        }
    }
}
