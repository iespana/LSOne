using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
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
using LSOne.ViewPlugins.TradeAgreements.Dialogs;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class CustomerTradeAgreementsPage : UserControl, ITabView
    {

        WeakReference owner;
        Customer customer;
        RecordIdentifier selectedID;
        WeakReference itemViewer;
        WeakReference groupViewer;
        WeakReference customerPriceDiscGroupEditor;

        public CustomerTradeAgreementsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerTradeAgreementsPage((TabControl)sender);
        }

        public CustomerTradeAgreementsPage()
        {
            InitializeComponent();

            IPlugin plugin;

            plugin = PluginEntry.Framework.FindImplementor(this, "CustomerPriceDiscGroups", null);

            customerPriceDiscGroupEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnEditTotalDiscountGroup.Visible =
                btnEditLineDiscount.Visible =
                btnEditMultilineDiscount.Visible = (customerPriceDiscGroupEditor != null);

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            IPlugin groupPlugin = PluginEntry.Framework.FindImplementor(this, "CustomerPriceDiscGroups", null);
            groupViewer = (groupPlugin != null) ? new WeakReference(groupPlugin) : null;

            IPlugin itemPlugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            itemViewer = (itemPlugin != null) ? new WeakReference(itemPlugin) : null;
            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customer = (Customer)internalContext;

            cmbTotalDiscountGroup.SelectedData = new DataEntity(customer.FinalDiscountID, customer.FinalDiscountDescription);
            cmbLineDiscount.SelectedData = new DataEntity(customer.LineDiscountID, customer.LineDiscountDescription);
            cmbMultilineDiscount.SelectedData = new DataEntity(customer.MultiLineDiscountID, customer.MultiLineDiscountDescription);

            LoadLines();
        }

        private void LoadLines()
        {
            DecimalLimit priceLimiter;
            DecimalLimit qtyLimiter;
            DecimalLimit percentLimiter;

            List<TradeAgreementEntry> lines;

            lvValues.ClearRows();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            percentLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            //Line discounts
            lines = Providers.TradeAgreementData.GetForCustomerAndGroup(PluginEntry.DataModel, customer.ID, cmbLineDiscount.SelectedData.ID,TradeAgreementRelation.LineDiscSales);

            Row row;

            foreach (TradeAgreementEntry line in lines)
            {

                //var variantID = Providers.DimensionData.GetVariantIDFromDimID(PluginEntry.DataModel, line.InventDimID);
                //Dimension dimension = Providers.DimensionData.GetByVariantID(PluginEntry.DataModel, variantID);

                row = new Row();
                row.AddText((string)line.Currency);


                row.AddText(tradeAgreementType(TradeAgreementRelation.LineDiscSales));

                String itemConnectionString = itemConnection(line.ItemCode);
                if (line.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Table)
                {
                    itemConnectionString += " - " + line.ItemName;
                }
                else if (line.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Group)
                {
                    itemConnectionString += " - " + line.ItemRelationName;
                }

                row.AddText(line.AccountCode == TradeAgreementEntryAccountCode.Group ? line.AccountCodeText + " - " + line.AccountName : line.AccountCodeText);
                row.AddText(itemConnectionString);
                row.AddText((line.VariantName == "") ? "" : line.VariantName);
                row.AddText((line.UnitDescription == "") ? "" : line.UnitDescription);
                row.AddCell(new DateTimeCell(line.FromDate.ToShortDateString(), line.FromDate.DateTime));
                row.AddCell(new DateTimeCell(line.ToDate.IsEmpty ? "" : line.ToDate.ToShortDateString(), line.ToDate.DateTime));
                row.AddCell(new NumericCell(line.QuantityAmount.FormatWithLimits(qtyLimiter), line.QuantityAmount));
                row.AddCell(new NumericCell(line.Amount.FormatWithLimits(priceLimiter), line.Amount));
                row.AddCell(new NumericCell(line.Percent1.FormatWithLimits(percentLimiter), line.Percent1));
                row.AddCell(new NumericCell(line.Percent2.FormatWithLimits(percentLimiter), line.Percent2));

                row.Tag = line;

                lvValues.AddRow(row);
            }
            //Multiline discounts
            lines = Providers.TradeAgreementData.GetForCustomerAndGroup(PluginEntry.DataModel, customer.ID, cmbMultilineDiscount.SelectedData.ID, TradeAgreementRelation.MultiLineDiscSales);
            foreach (TradeAgreementEntry line in lines)
            {
                //var variantID = Providers.DimensionData.GetVariantIDFromDimID(PluginEntry.DataModel, line.InventDimID);
                //Dimension dimension = Providers.DimensionData.GetByVariantID(PluginEntry.DataModel, variantID);

                row = new Row();
                row.AddText((string)line.Currency);

                row.AddText(tradeAgreementType(TradeAgreementRelation.MultiLineDiscSales));
                row.AddText(line.AccountCode == TradeAgreementEntryAccountCode.Group ? line.AccountCodeText + " - " + line.AccountName : line.AccountCodeText);
                row.AddText(line.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.All ? itemConnection(line.ItemCode) : itemConnection(line.ItemCode) + " - " + line.ItemRelationName);
                row.AddText((line.VariantName == "") ? "" : line.VariantName);
                row.AddText((line.UnitDescription == "") ? "" : line.UnitDescription);
                row.AddCell(new DateTimeCell(line.FromDate.ToShortDateString(), line.FromDate.DateTime));
                row.AddCell(new DateTimeCell(line.ToDate.IsEmpty ? "" : line.ToDate.ToShortDateString(), line.ToDate.DateTime));
                row.AddCell(new NumericCell(line.QuantityAmount.FormatWithLimits(qtyLimiter), line.QuantityAmount));
                row.AddCell(new NumericCell(line.Amount.FormatWithLimits(priceLimiter), line.Amount));
                row.AddCell(new NumericCell(line.Percent1.FormatWithLimits(percentLimiter), line.Percent1));
                row.AddCell(new NumericCell(line.Percent2.FormatWithLimits(percentLimiter), line.Percent2));

                row.Tag = line;

                lvValues.AddRow(row);
            }
            //Total discounts
            lines = Providers.TradeAgreementData.GetTotalDiscForCustomer(PluginEntry.DataModel, customer.ID, cmbTotalDiscountGroup.SelectedData.ID);
            foreach (TradeAgreementEntry line in lines)
            {
                row = new Row();
                row.AddText((string)line.Currency);

                row.AddText(tradeAgreementType(TradeAgreementRelation.TotalDiscount));
                row.AddText(line.AccountCode == TradeAgreementEntryAccountCode.Customer ? line.AccountCodeText : line.AccountCodeText + " - " + line.AccountName);
                row.AddText("");
                row.AddText("");
                row.AddText("");
                row.AddCell(new DateTimeCell(line.FromDate.ToShortDateString(), line.FromDate.DateTime));
                row.AddCell(new DateTimeCell(line.ToDate.IsEmpty ? "" : line.ToDate.ToShortDateString(), line.ToDate.DateTime));
                row.AddCell(new NumericCell(line.QuantityAmount.FormatWithLimits(qtyLimiter), line.QuantityAmount));
                row.AddCell(new NumericCell(line.Amount.FormatWithLimits(priceLimiter), line.Amount));
                row.AddCell(new NumericCell(line.Percent1.FormatWithLimits(percentLimiter), line.Percent1));
                row.AddCell(new NumericCell(line.Percent2.FormatWithLimits(percentLimiter), line.Percent2));

                row.Tag = line;

                lvValues.AddRow(row);
            }
            lvValues_SelectionChanged(this, EventArgs.Empty);

            lvValues.AutoSizeColumns(); 
        }

        private string tradeAgreementType(TradeAgreementRelation relation)
        {
            string result = "";
            switch (relation)
            {
                case TradeAgreementRelation.LineDiscSales:
                    result = Resources.LineDiscount;
                    break;
                case TradeAgreementRelation.MultiLineDiscSales:
                    result = Resources.MultilineDiscount;
                    break;
                case TradeAgreementRelation.TotalDiscount:
                    result = Resources.TotalDiscount;
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
            if ((cmbLineDiscount.SelectedData.ID != customer.LineDiscountID) ||
                (cmbMultilineDiscount.SelectedData.ID != customer.MultiLineDiscountID) ||
                (cmbTotalDiscountGroup.SelectedData.ID != customer.FinalDiscountID))
            {
                customer.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            customer.LineDiscountID = cmbLineDiscount.SelectedData.ID;
            customer.MultiLineDiscountID = cmbMultilineDiscount.SelectedData.ID;
            customer.FinalDiscountID = cmbTotalDiscountGroup.SelectedData.ID;

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

        private void cmbTotalDiscountGroup_RequestData(object sender, EventArgs e)
        {
            cmbTotalDiscountGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(
                    PluginEntry.DataModel, PriceDiscountModuleEnum.Customer,
                    PriceDiscGroupEnum.TotalDiscountGroup),
                null);
        }

        private void cmbLineDiscount_RequestData(object sender, EventArgs e)
        {
            cmbLineDiscount.SetData(Providers.PriceDiscountGroupData.GetGroupList(
                    PluginEntry.DataModel, PriceDiscountModuleEnum.Customer,
                    PriceDiscGroupEnum.LineDiscountGroup),
                null);
        }

        private void cmbMultilineDiscount_RequestData(object sender, EventArgs e)
        {
            cmbMultilineDiscount.SetData(Providers.PriceDiscountGroupData.GetGroupList(
                    PluginEntry.DataModel, PriceDiscountModuleEnum.Customer,
                    PriceDiscGroupEnum.MultilineDiscountGroup),
                null);
        }

        private void btnEditTotalDiscountGroup_Click(object sender, EventArgs e)
        {
            if (customerPriceDiscGroupEditor.IsAlive)
            {
                ((IPlugin)customerPriceDiscGroupEditor.Target).Message(this, "ViewCustomerPriceDiscGroups", (int)PriceDiscGroupEnum.TotalDiscountGroup);
            }
        }

        private void btnEditLineDiscount_Click(object sender, EventArgs e)
        {
            if (customerPriceDiscGroupEditor.IsAlive)
            {
                ((IPlugin)customerPriceDiscGroupEditor.Target).Message(this, "ViewCustomerPriceDiscGroups", (int)PriceDiscGroupEnum.LineDiscountGroup);
            }
        }

        private void btnEditMultilineDiscount_Click(object sender, EventArgs e)
        {
            if (customerPriceDiscGroupEditor.IsAlive)
            {
                ((IPlugin)customerPriceDiscGroupEditor.Target).Message(this, "ViewCustomerPriceDiscGroups", (int)PriceDiscGroupEnum.MultilineDiscountGroup);
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.RemoveButtonEnabled = false;
            bool selected = lvValues.Selection.Count > 0;
            if (selected)
            {
                selectedID = ((TradeAgreementEntry)lvValues.Selection[0].Tag).ID;
                btnsContextButtons.RemoveButtonEnabled = ((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountCode == 0 && PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
            }
            if (selected && (itemViewer != null))
            {
                btnViewItem.Enabled = PluginOperations.RowIsRetailItem(lvValues.Selection[0]);
            }
            else
            {
                btnViewItem.Enabled = false;
            }

            if (selected && PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
            {
                btnViewCustomerGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountCode == TradeAgreementEntryAccountCode.Group &&
                    (((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountRelation != ""));
            }
            else
            {
                btnViewCustomerGroup.Enabled = false;
            }
            if (selected && PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
            {
                btnViewItemGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Selection[0].Tag).ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Group &&
                    (((TradeAgreementEntry)lvValues.Selection[0].Tag).ItemRelationName != ""));
            }
            else
            {
                btnViewItemGroup.Enabled = false;
            }
            btnsContextButtons.EditButtonEnabled = (btnsContextButtons.RemoveButtonEnabled);
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
                btnViewItem_Click);
            item.Enabled = btnViewItem.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.ViewCustomerGroup + "...",
                    700,
                    btnViewCustomerGroup_Click);
            item.Enabled = btnViewCustomerGroup.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.ViewItemGroup + "...",
                    700,
                    btnViewItemGroup_Click);
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
                    ((TradeAgreementEntry)lvValues.Selection[0].Tag).ID,
                    Permission.ManageTradeAgreementPrices);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "TradeAgreement", ((TradeAgreementEntry)lvValues.Selection[0].Tag).ID, null);

                LoadLines();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addDialog = new CustomerTradeAgreementDialog(customer.ID);
            if ((addDialog.ShowDialog() == DialogResult.OK))
            {
                selectedID = addDialog.SelectedId();
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                LoadLines();
            }
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            if (itemViewer.IsAlive)
            {
                ((IPlugin)itemViewer.Target).Message(this, "ViewItem", ((TradeAgreementEntry)lvValues.Selection[0].Tag).ItemRelation);
            }
        }

        private void btnViewItemGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Selection[0].Tag).Relation;
                PriceDiscGroupEnum groupType = TradeAgreementEntryRelationToPriceDiscGroupEnum(type);
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Selection[0].Tag).ItemRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Item, new RecordIdentifier((int)groupType, groupID));
                switch (type)
                {
                    case TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales:
                        PluginOperations.ShowItemDiscountGroups(selectedID);
                        break;
                    case TradeAgreementEntry.TradeAgreementEntryRelation.MultiLineDiscSales:
                        PluginOperations.ShowItemDiscountGroups(selectedID);
                        break;
                    case TradeAgreementEntry.TradeAgreementEntryRelation.EndDiscSales:
                        PluginOperations.ShowItemDiscountGroups(selectedID);
                        break;
                }
            }
        }

        private void btnViewCustomerGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Selection[0].Tag).Relation;
                PriceDiscGroupEnum groupType = TradeAgreementEntryRelationToPriceDiscGroupEnum(type);
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer, new RecordIdentifier((int)groupType, groupID));
                PluginOperations.ShowCustDiscountGroups(selectedID);
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
                case TradeAgreementEntry.TradeAgreementEntryRelation.EndDiscSales:
                    groupEnum = PriceDiscGroupEnum.TotalDiscountGroup;
                    break;
            }
            return groupEnum;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Selection[0].Tag).Relation;
            DialogResult result = DialogResult.Cancel;
            switch (type)
            {
                case TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales:
                    Dialogs.TradeAgreementLineDiscountCustGroupDialog lineDialog = new Dialogs.TradeAgreementLineDiscountCustGroupDialog(customer.ID, selectedID,TradeAgreementEntryAccountCode.Customer);
                    result = lineDialog.ShowDialog();
                    break;
                case TradeAgreementEntry.TradeAgreementEntryRelation.MultiLineDiscSales:
                    Dialogs.TradeAgreementMultiLineDiscountDialog multilineDialog = new Dialogs.TradeAgreementMultiLineDiscountDialog(customer.ID, selectedID, Dialogs.TradeAgreementMultiLineDiscountDialog.MultiLineDiscountDialogMode.Customer);
                    result = multilineDialog.ShowDialog();
                    break;
                case TradeAgreementEntry.TradeAgreementEntryRelation.EndDiscSales:
                    Dialogs.TradeAgreementTotalDiscountDialog totalDialog = new Dialogs.TradeAgreementTotalDiscountDialog(customer.ID, selectedID, TradeAgreementEntryAccountCode.Customer);
                    result = totalDialog.ShowDialog();
                    break;
            }
            if (result == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                LoadLines();
            }
        }

        private void cmb_SelectedDataChanged(object sender, EventArgs e)
        {
            LoadLines();
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
