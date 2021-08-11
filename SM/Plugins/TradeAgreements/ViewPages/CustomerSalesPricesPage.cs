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
    public partial class CustomerSalesPricesPage : UserControl, ITabView
    {
        WeakReference owner;
        Customer customer;
        RecordIdentifier selectedID;
        WeakReference itemViewer;
        WeakReference groupViewer;
        WeakReference customerPriceDiscGroupEditor;

        public CustomerSalesPricesPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerSalesPricesPage((TabControl)sender);
        }

        public CustomerSalesPricesPage()
        {
            InitializeComponent();

            IPlugin plugin;

            plugin = PluginEntry.Framework.FindImplementor(this, "CustomerPriceDiscGroups", null);

            customerPriceDiscGroupEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnEditPriceGroup.Visible = (customerPriceDiscGroupEditor != null);

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

            cmbPriceGroup.SelectedData = new DataEntity(customer.PriceGroupID, customer.PriceGroupDescription);
           
            LoadLines();
        }

        private void LoadLines()
        {
            DecimalLimit priceLimiter;
            DecimalLimit qtyLimiter;
            
            List<TradeAgreementEntry> lines;
            //ListViewItem listItem;

            lvValues.ClearRows();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            //Sales prices
            lines = Providers.TradeAgreementData.GetForCustomerAndGroup(PluginEntry.DataModel, customer.ID, cmbPriceGroup.SelectedData.ID, TradeAgreementRelation.PriceSales);

            Row row;

            foreach (TradeAgreementEntry line in lines)
            {
                //var variantID = Providers.DimensionData.GetVariantIDFromDimID(PluginEntry.DataModel, line.InventDimID);
                //Dimension dimension = Providers.DimensionData.GetByVariantID(PluginEntry.DataModel, variantID);

                row = new Row();
                row.AddText((string)line.Currency);
                row.AddText(line.AccountCode == TradeAgreementEntryAccountCode.Customer ? line.AccountCodeText : line.AccountCodeText + " - " + line.AccountName);
                row.AddText(itemConnection(line.ItemCode) + " - " + line.ItemName);
                row.AddText((line.VariantName == "") ? "" : line.VariantName);
                row.AddText((line.UnitDescription == "") ? "" : line.UnitDescription);
                row.AddCell(new DateTimeCell(line.FromDate.ToShortDateString(), line.FromDate.DateTime));
                row.AddCell(new DateTimeCell(line.ToDate.IsEmpty ? "" : line.ToDate.ToShortDateString(), line.ToDate.DateTime));
                row.AddCell(new NumericCell(line.QuantityAmount.FormatWithLimits(qtyLimiter), line.QuantityAmount));
                row.AddCell(new NumericCell(line.Amount.FormatWithLimits(priceLimiter),line.Amount));
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
            if ((cmbPriceGroup.SelectedData.ID != customer.PriceGroupID))
            {
                customer.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            customer.PriceGroupID = cmbPriceGroup.SelectedData.ID;

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

        private void cmbPriceGroup_RequestData(object sender, EventArgs e)
        {
            cmbPriceGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(
                    PluginEntry.DataModel, PriceDiscountModuleEnum.Customer,
                    PriceDiscGroupEnum.PriceGroup),
                null);
        }

        private void btnEditPriceGroup_Click(object sender, EventArgs e)
        {
            if (customerPriceDiscGroupEditor.IsAlive)
            {
                ((IPlugin)customerPriceDiscGroupEditor.Target).Message(this, "ViewCustomerPriceDiscGroups", (int)PriceDiscGroupEnum.PriceGroup);
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = lvValues.Selection.Count > 0 ? ((TradeAgreementEntry)lvValues.Selection[0].Tag).ID : null;
            
            btnsContextButtons.RemoveButtonEnabled = selectedID != null 
                && ((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountCode == TradeAgreementEntryAccountCode.Customer 
                && PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled;

            if (selectedID != null && (itemViewer != null))
            {
                btnViewItem.Enabled = PluginOperations.RowIsRetailItem(lvValues.Selection[0]);
            }
            else
            {
                btnViewItem.Enabled = false;
            }

            if (selectedID != null && PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
            {
                btnViewCustomerGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountCode == TradeAgreementEntryAccountCode.Group &&
                    (((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountRelation != ""));
            }
            else
            {
                btnViewCustomerGroup.Enabled = false;
            }
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
                btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addDialog = new TradeAgreementPricesCustGroupDialog(customer.ID, TradeAgreementEntryAccountCode.Customer);
            if ((addDialog.ShowDialog() == DialogResult.OK))
            {
                selectedID = addDialog.ID;
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

        private void btnViewCustomerGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Selection[0].Tag).AccountRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer, new RecordIdentifier((int)PriceDiscGroupEnum.PriceGroup, groupID));
                PluginOperations.ShowPriceGroups(selectedID);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var editDialog = new TradeAgreementPricesCustGroupDialog(customer.ID, selectedID, TradeAgreementEntryAccountCode.Customer);
            if (editDialog.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                LoadLines();
            }
        }

        private void cmbPriceGroup_SelectedDataChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void lvValues_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            btnEdit_Click(this, EventArgs.Empty);
        }
    }
}
