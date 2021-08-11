using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
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
using LSOne.ViewPlugins.TradeAgreements.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class PriceDiscountGroupSalesPricesPage : UserControl, ITabView
    {
        WeakReference owner;

        PriceDiscGroupEnum displayType;
        RecordIdentifier selectedID = "";
        RecordIdentifier groupID = "";

        WeakReference itemViewer;
        WeakReference groupViewer;

        public PriceDiscountGroupSalesPricesPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public PriceDiscountGroupSalesPricesPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            IPlugin itemPlugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            itemViewer = (itemPlugin != null) ? new WeakReference(itemPlugin) : null;

            IPlugin groupPlugin = PluginEntry.Framework.FindImplementor(this, "ItemDiscountGroups", null);
            groupViewer = (groupPlugin != null) ? new WeakReference(groupPlugin) : null;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);
           
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PriceDiscountGroupSalesPricesPage((TabControl)sender);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = ((RecordIdentifier)context).SecondaryID.SecondaryID;
            displayType = (PriceDiscGroupEnum)internalContext;

            LoadLines();
        }

        private void LoadLines()
        {
            DecimalLimit priceLimiter;
            DecimalLimit qtyLimiter;

            List<TradeAgreementEntry> lines = null;

            lvValues.ClearRows(); 

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            //Sales prices
            lines = Providers.TradeAgreementData.GetForGroup(PluginEntry.DataModel, groupID, TradeAgreementRelation.PriceSales);
            foreach (TradeAgreementEntry line in lines)
            {


                Row row = new Row();

                row.AddText((string)line.Currency);
                row.AddText(Resources.SalesPrices);
                row.AddText(itemConnection(line.ItemCode) + " - " + line.ItemName);
                row.AddText(Resources.Group);
                row.AddText((line.VariantName == "") ? "" : line.VariantName);
                row.AddText(((string)line.UnitDescription == "") ? "" : (string)line.UnitDescription);
                row.AddCell(new DateTimeCell(line.FromDate.ToShortDateString(), line.FromDate.DateTime));
                row.AddCell(new DateTimeCell(line.ToDate.IsEmpty ? "" : line.ToDate.ToShortDateString(), line.ToDate.DateTime));
                row.AddCell(new NumericCell(line.QuantityAmount.FormatWithLimits(qtyLimiter), line.QuantityAmount));
                row.AddCell(new NumericCell(line.Amount.FormatWithLimits(priceLimiter), line.Amount));
                row.AddCell(new NumericCell(line.AmountIncludingTax.FormatWithLimits(priceLimiter), line.AmountIncludingTax));

                row.Tag = line;
                lvValues.AddRow(row);


            }
            lvValues_SelectionChanged(this, EventArgs.Empty);

            lvValues.AutoSizeColumns();
        }

        private string itemConnection(TradeAgreementEntry.TradeAgreementEntryItemCode itemCode)
        {
            string result = "";
            switch (itemCode)
            {
                case TradeAgreementEntry.TradeAgreementEntryItemCode.Table:
                    result = Resources.Item;
                    break;
                case TradeAgreementEntry.TradeAgreementEntryItemCode.Group:
                    result = Resources.Group;
                    break;
                case TradeAgreementEntry.TradeAgreementEntryItemCode.All:
                    result = Resources.AllItems;
                    break;
            }
            return result;
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

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "TradeAgreement")
            {
                LoadLines();
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Resources.EditText,
                    300,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Add,
                    300,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                Resources.ViewItem + "...",
                600,
                new EventHandler(btnViewItem_Click));
            item.Enabled = btnViewItem.Enabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TradeAgreementSalesPriceList", lvValues.ContextMenuStrip, lvValues);
            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Resources.DeleteTradeAgreementQuestion,
                Resources.DeleteTradeAgreement) == DialogResult.Yes)
            {
                Providers.TradeAgreementData.Delete(
                    PluginEntry.DataModel,
                    ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID,
                    Permission.ManageTradeAgreementPrices);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);

                LoadLines();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.TradeAgreementPricesCustGroupDialog dlg = new Dialogs.TradeAgreementPricesCustGroupDialog(groupID, TradeAgreementEntryAccountCode.Group);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.ID;
                LoadLines();
            }
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            if (itemViewer.IsAlive)
            {
                ((IPlugin)itemViewer.Target).Message(this, "ViewItem", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemRelation);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.TradeAgreementPricesCustGroupDialog dlg = new Dialogs.TradeAgreementPricesCustGroupDialog(groupID, ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, TradeAgreementEntryAccountCode.Group);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);
                LoadLines();
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.RemoveButtonEnabled = false;
            bool selected = lvValues.Selection.Count > 0;
            if (selected)
            {
                selectedID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;
                btnsContextButtons.RemoveButtonEnabled =
                    ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountCode ==
                    TradeAgreementEntryAccountCode.Group
                    && PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);
            }
            if (selected && (itemViewer != null))
            {
                btnViewItem.Enabled = PluginOperations.ListViewItemIsRetailItem((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);
            }
            else
            {
                btnViewItem.Enabled = false;
            }
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled;
        }

        private void lvValues_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, null);
            }
        }
    }
}
