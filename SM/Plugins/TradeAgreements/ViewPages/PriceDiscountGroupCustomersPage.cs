using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Dialogs;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class PriceDiscountGroupCustomersPage : UserControl, ITabView
    {
        private WeakReference customerViewer;

        PriceDiscGroupEnum displayType;

        WeakReference owner;
        RecordIdentifier groupID = "";

        internal PriceDiscountGroupCustomersPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }
        
        public PriceDiscountGroupCustomersPage()
        {
            InitializeComponent();

            displayType = PriceDiscGroupEnum.PriceGroup;
            
            lvCustomers.ContextMenuStrip = new ContextMenuStrip();
            lvCustomers.ContextMenuStrip.Opening += lvCustomers_Opening;

            customerDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            customerDataScroll.Reset();

            var plugin = PluginEntry.Framework.FindImplementor(this, "CanEditCustomer", null);
            customerViewer = plugin != null ? new WeakReference(plugin) : null;

            contextButtonsCustomers.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditCustomerDiscGroups);
           
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PriceDiscountGroupCustomersPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 1, Properties.Resources.PriceDiscountGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = context;
            displayType = (PriceDiscGroupEnum)internalContext;

            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Customer":
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

        private void LoadLines()
        {
            if (displayType == PriceDiscGroupEnum.PriceGroup)
            {
                LoadPriceGroupLines();
                return;
            }

            lvCustomers.ClearRows();

            var groupType = GetSelectedType();
            string customerGroup = groupID.SecondaryID.SecondaryID.ToString();

            var customers = Providers.PriceDiscountGroupData
                .GetCustomersInGroupList(PluginEntry.DataModel, 
                    groupType, 
                    customerGroup,
                    customerDataScroll.StartRecord,
                    customerDataScroll.EndRecord + 1);

            customerDataScroll.RefreshState(customers);

            foreach (var customer in customers)
            {
                Row row = new Row();

                row.AddText((string)customer.ID);
                row.AddText(customer.Text);

                row.Tag = customer.ID;
                lvCustomers.AddRow(row);
            }

            lvCustomers_SelectionChanged(this, EventArgs.Empty);

            lvCustomers.AutoSizeColumns();
        }

        private void LoadPriceGroupLines()
        {
            var groupType = GetSelectedType();

            // Set the Customers in group lines

            lvCustomers.ClearRows();

            string customerGroup = groupID.SecondaryID.SecondaryID.ToString();

            var customers = Providers.PriceDiscountGroupData.GetCustomersInGroupList(PluginEntry.DataModel,
                                                                        groupType,
                                                                        customerGroup,
                                                                        customerDataScroll.StartRecord,
                                                                        customerDataScroll.EndRecord + 1);

            customerDataScroll.RefreshState(customers);

            foreach (var customer in customers)
            {
                Row row = new Row();

                row.AddText((string)customer.ID);
                row.AddText(customer.Text);

                row.Tag = customer.ID;
                lvCustomers.AddRow(row);
            }

            lvCustomers_SelectionChanged(this, EventArgs.Empty);

            lvCustomers.AutoSizeColumns();
        }
        void lvCustomers_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCustomers.ContextMenuStrip;


            menu.Items.Clear();

            // We can optionally add our own items right here

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    100,
                    btnAddCustomer_Click)
            {
                Enabled = contextButtonsCustomers.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    200,
                    btnRemoveCustomer_Click)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = contextButtonsCustomers.RemoveButtonEnabled
            };
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 300));

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewCustomer + "...",
                    400,
                    btnEditCustomer_Click)
            {
                Enabled = btnViewCustomer.Enabled,
                Default = true,
                Image = ContextButtons.GetEditButtonImage()
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("CustomersInDiscGroupList", lvCustomers.ContextMenuStrip, lvCustomers);

            e.Cancel = (menu.Items.Count == 0);
        }


        private PriceDiscGroupEnum GetSelectedType()
        {
            return displayType;
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void btnRemoveCustomer_Click(object sender, EventArgs e)
        {
            var customerId = (RecordIdentifier) lvCustomers.Row(lvCustomers.Selection.FirstSelectedRow).Tag;
            Providers.PriceDiscountGroupData.RemoveCustomerFromGroup(PluginEntry.DataModel, customerId, GetSelectedType());
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", customerId, null);
            LoadLines();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string groupId = groupID.SecondaryID.SecondaryID.ToString();
            var dlg = new CustomerSearchDialog(PluginEntry.DataModel, PluginEntry.Framework, (int)GetSelectedType(), groupId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                RecordIdentifier customerAccountNumber = dlg.GetCustomerAccountNumber();

                Providers.PriceDiscountGroupData.AddCustomerToGroup(PluginEntry.DataModel, 
                                                                 customerAccountNumber,
                                                                 GetSelectedType(),
                                                                 groupId);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", customerAccountNumber, null);

                LoadLines();
            }
        }

      

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (customerViewer != null && customerViewer.IsAlive)
            {
                var list = new List<IDataEntity>();

                foreach (var item in lvCustomers.Rows)
                {
                    list.Add(new DataEntity((RecordIdentifier)item.Tag, ""));
                }

                ((IPlugin)customerViewer.Target).Message(this, "EditCustomer", new object[] { ((RecordIdentifier)lvCustomers.Row(lvCustomers.Selection.FirstSelectedRow).Tag).PrimaryID, list });
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void lvCustomers_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnViewCustomer.Enabled)
            {
                btnEditCustomer_Click(this, null);
            }
        }

        private void lvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            contextButtonsCustomers.RemoveButtonEnabled = (lvCustomers.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);
            btnViewCustomer.Enabled = (lvCustomers.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.CustomerView) && (customerViewer != null && customerViewer.IsAlive);

        }
    }
}
