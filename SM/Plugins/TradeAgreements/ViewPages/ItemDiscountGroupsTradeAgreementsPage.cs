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
    public partial class ItemDiscountGroupsTradeAgreementsPage : UserControl, ITabView
    {
        WeakReference owner;
        PriceDiscGroupEnum displayType;
        RecordIdentifier selectedID = "";
        RecordIdentifier groupID = "";
        WeakReference customerViewer;
        WeakReference groupViewer;

        public ItemDiscountGroupsTradeAgreementsPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            IPlugin groupPlugin = PluginEntry.Framework.FindImplementor(this, "CustomerPriceDiscGroups", null);
            groupViewer = (groupPlugin != null) ? new WeakReference(groupPlugin) : null;

            IPlugin customerPlugin = PluginEntry.Framework.FindImplementor(this, "CanEditCustomer", null);
            customerViewer = (customerPlugin != null) ? new WeakReference(customerPlugin) : null;
           
        }

        public ItemDiscountGroupsTradeAgreementsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.ItemDiscountGroupsTradeAgreementsPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 0, Properties.Resources.PriceDiscountGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = ((RecordIdentifier)context).SecondaryID.SecondaryID;
            displayType = (PriceDiscGroupEnum)internalContext;

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
            DecimalLimit priceLimiter;
            DecimalLimit qtyLimiter;


            List<TradeAgreementEntry> lines = null;

            lvValues.ClearRows();

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            qtyLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            
            //Line discounts
            if (displayType == PriceDiscGroupEnum.LineDiscountGroup)
            {
                lines = Providers.TradeAgreementData.GetForItemDiscountGroup(PluginEntry.DataModel, groupID, TradeAgreementRelation.LineDiscSales);
            }
            //Multiline discounts
            else
            {
                lines = Providers.TradeAgreementData.GetForItemDiscountGroup(PluginEntry.DataModel, groupID, TradeAgreementRelation.MultiLineDiscSales);
            }
            foreach (TradeAgreementEntry line in lines)
            {
                Row row = new Row();

                row.AddText((string)line.Currency);
                row.AddText(discountType(displayType));
                row.AddText(Resources.Group);
                row.AddText(line.AccountCode == TradeAgreementEntryAccountCode.All ? line.AccountCodeText : line.AccountCodeText + " - " + line.AccountName);
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

            PluginEntry.Framework.ContextMenuNotify("TradeAgreementSalesPriceList", lvValues.ContextMenuStrip, lvValues);
            e.Cancel = (menu.Items.Count == 0);
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

        private void btnViewCustomerGroup_Click(object sender, EventArgs e)
        {
            if (groupViewer.IsAlive)
            {
                TradeAgreementEntry.TradeAgreementEntryRelation type = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).Relation;
                PriceDiscGroupEnum groupType = displayType;
                RecordIdentifier groupID = ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountRelation;
                RecordIdentifier selectedID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer, new RecordIdentifier((int)groupType, groupID));
                
                PluginOperations.ShowCustDiscountGroups(selectedID);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (displayType == PriceDiscGroupEnum.LineDiscountGroup)
            {
                Dialogs.TradeAgreemenItemGroupLineDiscountDialog dlg = new Dialogs.TradeAgreemenItemGroupLineDiscountDialog(groupID);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedID = dlg.ID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                    LoadLines();
                }
            }
            else
            {
                Dialogs.TradeAgreementMultiLineDiscountDialog dlg = new Dialogs.TradeAgreementMultiLineDiscountDialog(groupID, Dialogs.TradeAgreementMultiLineDiscountDialog.MultiLineDiscountDialogMode.ItemDiscountGroup);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedID = dlg.ID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "TradeAgreement", selectedID, null);
                    LoadLines();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (displayType == PriceDiscGroupEnum.LineDiscountGroup)
            {
                Dialogs.TradeAgreemenItemGroupLineDiscountDialog dlg = new Dialogs.TradeAgreemenItemGroupLineDiscountDialog(groupID, ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);
                    LoadLines();
                }
            }
            else
            {
                Dialogs.TradeAgreementMultiLineDiscountDialog dlg = new Dialogs.TradeAgreementMultiLineDiscountDialog(groupID, ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, Dialogs.TradeAgreementMultiLineDiscountDialog.MultiLineDiscountDialogMode.ItemDiscountGroup);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TradeAgreement", ((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID, null);
                    LoadLines();
                }
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
                btnEdit_Click(this, EventArgs.Empty);
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
                btnViewCustomerGroup.Enabled =
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountCode == TradeAgreementEntryAccountCode.Group &&
                    (((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).AccountRelation != ""));
            }
            else
            {
                btnViewCustomerGroup.Enabled = false;
            }
            if (btnsContextButtons.RemoveButtonEnabled && (customerViewer != null))
            {
                btnViewCustomer.Enabled = PluginOperations.ListViewItemIsCustomer((TradeAgreementEntry)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);
            }
            else
            {
                btnViewCustomer.Enabled = false;
            }
        }
    }
}
