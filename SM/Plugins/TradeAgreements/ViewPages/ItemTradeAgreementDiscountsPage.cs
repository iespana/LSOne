using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
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
using LSOne.ViewPlugins.TradeAgreements.Dialogs;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class ItemTradeAgreementDiscountsPage : UserControl, ITabViewV2
    {
        WeakReference viewReference;
        WeakReference owner;
        RetailItem item;
        RecordIdentifier selectedID;
        private RecordIdentifier lineDiscountGroupID;
        private RecordIdentifier multilineDiscountGroupID;
        WeakReference customerViewer;
        WeakReference groupViewer;

        public ItemTradeAgreementDiscountsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            var tabControl = (TabControl)sender;
            var page = new ViewPages.ItemTradeAgreementDiscountsPage((TabControl)sender);
            var view = tabControl.Parent;
            while (view != null && !(view is ViewBase))
                view = view.Parent;
            if (view != null)
                page.viewReference = new WeakReference(view);
            return page;
        }

        public ItemTradeAgreementDiscountsPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            IPlugin groupPlugin = PluginEntry.Framework.FindImplementor(this, "CustomerPriceDiscGroups", null);
            groupViewer = (groupPlugin != null) ? new WeakReference(groupPlugin) : null;

            IPlugin customerPlugin = PluginEntry.Framework.FindImplementor(this, "CanEditCustomer", null);
            customerViewer = (customerPlugin != null) ? new WeakReference(customerPlugin) : null;

        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)((Dictionary<string, object>)internalContext)["Item"];
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            lineDiscountGroupID = (RecordIdentifier)((Dictionary<string, object>)internalContext)["LineDiscount"];
            multilineDiscountGroupID = (RecordIdentifier)((Dictionary<string, object>)internalContext)["MultilineDiscount"];

            LoadLines();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)((Dictionary<string, object>)internalContext)["Item"];
            lineDiscountGroupID = (RecordIdentifier) ((Dictionary<string, object>) internalContext)["LineDiscount"];
            multilineDiscountGroupID = (RecordIdentifier)((Dictionary<string, object>)internalContext)["MultilineDiscount"];

            LoadLines();
        }

        private void LoadLines()
        {
            var view = viewReference == null ? null : viewReference.Target as ViewBase;
            if (view != null)
            {
                view.ShowProgress((sender1, e1) => LoadLinesInProgress(true));
            }
            else
            {
                LoadLinesInProgress(false);
            }
        }

        private void LoadLinesInProgress(bool inProgress)
        {
            try
            {
                DecimalLimit priceLimiter;
                DecimalLimit qtyLimiter;

                List<TradeAgreementEntry> lines = null;

                lvValues.ClearRows();

                lines = Providers.TradeAgreementData.GetForItemAndItemGroups(PluginEntry.DataModel, item.ID,
                    lineDiscountGroupID, multilineDiscountGroupID);

                priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
                Row row;
                foreach (TradeAgreementEntry line in lines)
                {
                    row = new Row();

                    row.AddText((string)line.Currency);
                    row.AddText(discountType(line.Relation));
                    row.AddText((line.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Group)
                        ? itemConnection(line.ItemCode) + " - " + line.ItemRelationName
                        : itemConnection(line.ItemCode));
                    row.AddText(line.AccountCode == TradeAgreementEntryAccountCode.All
                        ? line.AccountCodeText
                        : line.AccountCodeText + " - " + line.AccountName);
                    row.AddText((line.VariantName == "")
                        ? ""
                        : line.VariantName);
                    row.AddText(((string)line.UnitDescription == "") ? "" : (string)line.UnitDescription);
                    row.AddCell(new DateTimeCell(line.FromDate.ToShortDateString(), line.FromDate.DateTime));
                    row.AddCell(new DateTimeCell(line.ToDate.IsEmpty ? "" : line.ToDate.ToShortDateString(), line.ToDate.DateTime));
                    row.AddCell(new NumericCell(line.QuantityAmount.FormatWithLimits(qtyLimiter), line.QuantityAmount));
                    row.AddCell(new NumericCell(line.Amount.FormatWithLimits(priceLimiter), line.Amount));
                    row.AddCell(new NumericCell(line.Percent1.ToString("N2"), line.Percent1));
                    row.AddCell(new NumericCell(line.Percent2.ToString("N2"), line.Percent2));

                    row.Tag = line;
                    lvValues.AddRow(row);
                }

                lvValues_SelectionChanged(this, EventArgs.Empty);

                lvValues.AutoSizeColumns();
            }
            finally
            {
                if (inProgress)
                {
                    var view = viewReference.Target as ViewBase;
                    view.HideProgress();
                }
            }
        }

        private string discountType(TradeAgreementEntry.TradeAgreementEntryRelation type)
        {
            string result = "";
            switch (type)
            {
                case TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales:
                    result = Resources.LineDiscount;
                    break;
                case TradeAgreementEntry.TradeAgreementEntryRelation.MultiLineDiscSales:
                    result = Resources.MultilineDiscount;
                    break;
            }
            return result;
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
            return item.Dirty;
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
                Resources.ViewCustomer + "...",
                600,
                new EventHandler(btnViewCustomer_Click));
            item.Enabled = btnViewCustomer.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.ViewCustomerGroup + "...",
                    700,
                    new EventHandler(btnViewCustomerGroup_Click));
            item.Enabled = btnViewCustomerGroup.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.ViewItemGroup + "...",
                    700,
                    new EventHandler(btnViewItemGroup_Click));
            item.Enabled = btnViewItemGroup.Enabled;
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
            TradeAgreemenItemLineDiscountDialog addDialog = new TradeAgreemenItemLineDiscountDialog(item.ID);
            Dictionary<string, object> addLineResults = new Dictionary<string, object>();
            if ((addDialog.ShowDialog() == DialogResult.OK))
            {
                if (item.ID == RecordIdentifier.Empty)
                {
                    // Multiedit

                }
                else
                {
                    selectedID = addDialog.ID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                    LoadLines();
                }
            }
        }

        private void btnViewCustomer_Click(object sender, EventArgs e)
        {
            if (customerViewer.IsAlive)
            {
                List<IDataEntity> list = new List<IDataEntity>();
                foreach (var item in lvValues.Rows)
                {
                    if (PluginOperations.ListViewItemIsCustomer((TradeAgreementEntry)item.Tag))
                    {
                        list.Add(new DataEntity(((TradeAgreementEntry)item.Tag).AccountRelation, ""));
                    }
                }

                ((IPlugin)customerViewer.Target).Message(this, "EditCustomer", new object[] { ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountRelation, list });
            }
        }

        private void btnViewItemGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).Relation;
                PriceDiscGroupEnum groupType = TradeAgreementEntryRelationToPriceDiscGroupEnum(type);
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Item, new RecordIdentifier((int)groupType, groupID));
                if (type == TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales)
                {
                    PluginOperations.ShowItemDiscountGroups(selectedID);
                }
                else
                {
                    PluginOperations.ShowItemDiscountGroups(selectedID);
                }
            }
        }

        private void btnViewCustomerGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).Relation;
                PriceDiscGroupEnum groupType = TradeAgreementEntryRelationToPriceDiscGroupEnum(type);
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer, new RecordIdentifier((int)groupType, groupID));
                if (type == TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales)
                {
                    PluginOperations.ShowCustDiscountGroups(selectedID);
                }
                else
                {
                    PluginOperations.ShowCustDiscountGroups(selectedID);
                }
            }
        }

        private PriceDiscGroupEnum TradeAgreementEntryRelationToPriceDiscGroupEnum(TradeAgreementEntry.TradeAgreementEntryRelation relation)
        {
            PriceDiscGroupEnum groupEnum = PriceDiscGroupEnum.PriceGroup;
            switch (relation)
            {
                case TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales:
                    groupEnum = PriceDiscGroupEnum.LineDiscountGroup;
                    break;
                case TradeAgreementEntry.TradeAgreementEntryRelation.MultiLineDiscSales:
                    groupEnum = PriceDiscGroupEnum.MultilineDiscountGroup;
                    break;
            }
            return groupEnum;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var editDialog = new TradeAgreemenItemLineDiscountDialog(item.ID, selectedID);
            if (editDialog.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", selectedID, null);
                LoadLines();
            }
        }

        private void lvValues_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            bool selected = lvValues.Selection.Count > 0;
            btnsContextButtons.RemoveButtonEnabled = false;
            if (selected)
            {
                selectedID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;
                btnsContextButtons.RemoveButtonEnabled = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Table;
            }
            if (selected && (customerViewer != null))
            {
                btnViewCustomer.Enabled = PluginOperations.ListViewItemIsCustomer((TradeAgreementEntry) lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);
            }
            else
            {
                btnViewCustomer.Enabled = false;
            }

            if (selected && PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
            {
                btnViewCustomerGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountCode == TradeAgreementEntryAccountCode.Group &&
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountRelation != ""));
            }
            else
            {
                btnViewCustomerGroup.Enabled = false;
            }
            if (selected && PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
            {
                btnViewItemGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Group &&
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemRelationName != ""));
            }
            else
            {
                btnViewItemGroup.Enabled = false;
            }
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled;
        }
    }
}
