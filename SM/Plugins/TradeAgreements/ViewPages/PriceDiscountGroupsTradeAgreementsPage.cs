using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
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
    public partial class PriceDiscountGroupsTradeAgreementsPage : UserControl, ITabView
    {
        WeakReference owner;
        PriceDiscGroupEnum displayType;
        RecordIdentifier selectedID = "";
        RecordIdentifier groupID = "";
        WeakReference itemViewer;
        WeakReference groupViewer;

        public PriceDiscountGroupsTradeAgreementsPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            IPlugin customerPlugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            itemViewer = (customerPlugin != null) ? new WeakReference(customerPlugin) : null;

            
            IPlugin groupPlugin = PluginEntry.Framework.FindImplementor(this, "ItemDiscountGroups", null);
            groupViewer = (groupPlugin != null) ? new WeakReference(groupPlugin) : null;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditCustomerDiscGroups);
          
        }

        public PriceDiscountGroupsTradeAgreementsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PriceDiscountGroupsTradeAgreementsPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 0, Properties.Resources.PriceDiscountGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = ((RecordIdentifier)context).SecondaryID.SecondaryID;
            displayType = (PriceDiscGroupEnum)internalContext;

            if (displayType == PriceDiscGroupEnum.TotalDiscountGroup)
            {
                btnViewItem.Visible = false;
                btnViewItemGroup.Visible = false;
            }
            else if (displayType == PriceDiscGroupEnum.MultilineDiscountGroup)
            {
                btnViewItem.Visible = false;
                btnViewItemGroup.Visible = true;
            }
            else
            {
                btnViewItem.Visible = true;
                btnViewItemGroup.Visible = true;
            }
            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "TradeAgreement":
                    selectedID = changeIdentifier;
                    LoadLines();
                    break;
                case "RetailItem":
                    LoadLines();
                    break;
            }
        }

        public bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            return false;
        }

        public bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        private void LoadLines()
        {
            if (displayType == PriceDiscGroupEnum.PriceGroup)
            {
                return;
            }
            DecimalLimit priceLimiter;
            DecimalLimit qtyLimiter;


            List<TradeAgreementEntry> lines = null;

            lvValues.ClearRows();
            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            switch (displayType)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    lines = Providers.TradeAgreementData.GetForGroup(PluginEntry.DataModel, groupID, TradeAgreementRelation.LineDiscSales);
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    lines = Providers.TradeAgreementData.GetForGroup(PluginEntry.DataModel, groupID, TradeAgreementRelation.MultiLineDiscSales);
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    lines = Providers.TradeAgreementData.GetForGroup(PluginEntry.DataModel, groupID, TradeAgreementRelation.TotalDiscount);
                    break;
            }

            foreach (TradeAgreementEntry line in lines)
            {
                //var variantID = Providers.DimensionData.GetVariantIDFromDimID(PluginEntry.DataModel, line.InventDimID);
                //Dimension dimension = Providers.DimensionData.GetByVariantID(PluginEntry.DataModel, variantID);
               
                Row row = new Row();

                row.AddText((string)line.Currency);
                row.AddText(discountType(displayType));
                row.AddText(line.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.All ? itemConnection(line.ItemCode) : itemConnection(line.ItemCode) + " - " +
                    (line.ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Table ? line.ItemName : line.ItemRelationName));
                row.AddText(Resources.Group);
                row.AddText((line.VariantName == "") ? "" : line.VariantName);
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

        private string discountType(PriceDiscGroupEnum type)
        {
            string result = "";
            switch (type)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    result = Resources.LineDiscount;
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    result = Resources.MultilineDiscount;
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    result = Resources.TotalDiscount;
                    break;
            }
            return result;
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

            item = new ExtendedMenuItem(
                    Resources.ViewItemGroup + "...",
                    700,
                    new EventHandler(btnViewItemGroup_Click));
            item.Enabled = btnViewItemGroup.Enabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TradeAgreementSalesPriceList", lvValues.ContextMenuStrip, lvValues);
            e.Cancel = (menu.Items.Count == 0);
        }


        private void btnViewItem_Click(object sender, EventArgs e)
        {
            if (itemViewer.IsAlive)
            {
                ((IPlugin)itemViewer.Target).Message(this, "ViewItem", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemRelation);
            }
        }

        private void btnViewItemGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).Relation;
                PriceDiscGroupEnum groupType = displayType;
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Item, new RecordIdentifier((int)groupType, groupID));

                PluginOperations.ShowItemDiscountGroups(selectedID);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            switch (displayType)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    Dialogs.TradeAgreementLineDiscountCustGroupDialog lineDialog = new Dialogs.TradeAgreementLineDiscountCustGroupDialog(groupID, TradeAgreementEntryAccountCode.Group);
                    if (lineDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedID = lineDialog.ID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                        LoadLines();
                    }
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    Dialogs.TradeAgreementMultiLineDiscountDialog multiDialog = new Dialogs.TradeAgreementMultiLineDiscountDialog(groupID, Dialogs.TradeAgreementMultiLineDiscountDialog.MultiLineDiscountDialogMode.Group);
                    if (multiDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedID = multiDialog.ID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                        LoadLines();
                    }
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    Dialogs.TradeAgreementTotalDiscountDialog totalDialog = new Dialogs.TradeAgreementTotalDiscountDialog(groupID, TradeAgreementEntryAccountCode.Group);

                    if (totalDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedID = totalDialog.ID;
                        LoadLines();
                    }
                    break;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            switch (displayType)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    Dialogs.TradeAgreementLineDiscountCustGroupDialog lineDialog = new Dialogs.TradeAgreementLineDiscountCustGroupDialog(groupID, ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, TradeAgreementEntryAccountCode.Group);
                    if (lineDialog.ShowDialog() == DialogResult.OK)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);
                        LoadLines();
                    }
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    Dialogs.TradeAgreementMultiLineDiscountDialog multiDialog = new Dialogs.TradeAgreementMultiLineDiscountDialog(groupID, ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, Dialogs.TradeAgreementMultiLineDiscountDialog.MultiLineDiscountDialogMode.Group);
                    if (multiDialog.ShowDialog() == DialogResult.OK)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);
                        LoadLines();
                    }
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    Dialogs.TradeAgreementTotalDiscountDialog totalDialog = new Dialogs.TradeAgreementTotalDiscountDialog(groupID, ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, TradeAgreementEntryAccountCode.Group);
                    if (totalDialog.ShowDialog() == DialogResult.OK)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);
                        LoadLines();
                    }
                    break;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteTradeAgreementQuestion,
                Properties.Resources.DeleteTradeAgreement) == DialogResult.Yes)
            {
                Providers.TradeAgreementData.Delete(
                    PluginEntry.DataModel,
                    ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID,
                    Permission.ManageTradeAgreementPrices);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);

                LoadLines();
            }
        }

        private void lvValues_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, null);
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled = lvValues.Selection.Count > 0;
            if (btnsContextButtons.RemoveButtonEnabled)
            {
                selectedID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;
            }

            if (btnsContextButtons.RemoveButtonEnabled && PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
            {
                btnViewItemGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemCode == TradeAgreementEntry.TradeAgreementEntryItemCode.Group &&
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ItemRelation != ""));
            }
            else
            {
                btnViewItemGroup.Enabled = false;
            }
            if (btnsContextButtons.RemoveButtonEnabled && (itemViewer != null))
            {
                btnViewItem.Enabled = PluginOperations.ListViewItemIsRetailItem((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);
            }
            else
            {
                btnViewItem.Enabled = false;
            }
        }
    }
}
