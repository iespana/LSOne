using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.Themes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Customer.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using System.Linq;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerAddressPage : UserControl, ITabView
    {
        private DataLayer.BusinessObjects.Customers.Customer customer;
        private CustomerAddress selectedAddress;

        public CustomerAddressPage()
        {
            InitializeComponent();
            lvAddresses.ContextMenuStrip = new ContextMenuStrip();
            lvAddresses.ContextMenuStrip.Opening += lvAddresses_Opening;

            lvAddresses.ApplyTheme(new LSOneTheme());
            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
        }    

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerAddressPage();
        }

        void lvAddresses_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvAddresses.ContextMenuStrip;

            menu.Items.Clear();

            var item = ExtendedMenuItem.EditToMenu(btnsContextButtons, menu, btnEdit_Click);
            item.Default = true;

            ExtendedMenuItem.AddToMenu(btnsContextButtons, menu, btnAdd_Click);
            ExtendedMenuItem.RemoveToMenu(btnsContextButtons, menu, btnRemove_Click);

            item = new ExtendedMenuItem("-", 300);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.SetGroupAsDefault,
                    400,
                    SetAsDefault)
            {
                Enabled = CanSetAsDefault
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("CustomerAddressListMenu", lvAddresses.ContextMenuStrip, this);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadAddresses()
        {
            CustomerAddress tempAddress = selectedAddress;
            lvAddresses.ClearRows();
            selectedAddress = tempAddress;

            customer.Addresses = Providers.CustomerAddressData.GetListForCustomer(PluginEntry.DataModel, customer.ID);

            bool canEdit = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);

            foreach (var address in customer.Addresses)
            {
                var row = new Row {Tag = address};

                row.AddCell(new ExtendedCell(string.Empty, address.IsDefault ? Properties.Resources.dot_green_16 : null));
                row.AddText(Address.AddressTypeToString(address.Address.AddressType));
                row.AddText(address.ContactName);
                row.AddText(address.Telephone);
                row.AddText(address.MobilePhone);
                row.AddText(address.Email);
                row.AddText(address.TaxGroupName);

                if (canEdit)
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete);

                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
                }
                else
                {
                    row.AddText("");
                }

                lvAddresses.AddRow(row);

                if(selectedAddress != null && selectedAddress.ID == address.ID)
                {
                    lvAddresses.Selection.Set(lvAddresses.Rows.Count - 1);
                }
            }

            lvAddresses_SelectionChanged(this, EventArgs.Empty);
            lvAddresses.AutoSizeColumns(false);
        }

        #region ITabView Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			customer = (LSOne.DataLayer.BusinessObjects.Customers.Customer)internalContext;

			LoadAddresses();
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

        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new CustomerAddressDialog(customer);
            if (DialogResult.OK == dlg.ShowDialog())
            {
                bool hasDefaultAddress = Providers.CustomerAddressData.HasDefaultAddress(PluginEntry.DataModel, customer.ID, dlg.Address.Address.AddressType);
                dlg.Address.IsDefault = !hasDefaultAddress;
                Providers.CustomerAddressData.Save(PluginEntry.DataModel, dlg.Address);
                LoadAddresses();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedAddress == null)
                return;

            bool isDefault = selectedAddress.IsDefault;
            Address.AddressTypes currentType = selectedAddress.Address.AddressType;

            var dlg = new CustomerAddressDialog(selectedAddress);
            if (DialogResult.OK == dlg.ShowDialog())
            {
                //If the address type changed, update IsDefault
                if(currentType != dlg.Address.Address.AddressType)
                {
                    bool hasDefaultAddress = Providers.CustomerAddressData.HasDefaultAddress(PluginEntry.DataModel, customer.ID, dlg.Address.Address.AddressType);
                    dlg.Address.IsDefault = !hasDefaultAddress;
                }

                Providers.CustomerAddressData.Save(PluginEntry.DataModel, dlg.Address);

                //If the address type changed and the address was a default one, try to find another address to set as default
                if(isDefault && currentType != dlg.Address.Address.AddressType)
                {
                    CustomerAddress newDefaultAddress = customer.Addresses.FirstOrDefault(x => x.IsDefault == false && x.Address.AddressType == currentType);
                    if (newDefaultAddress != null)
                    {
                        newDefaultAddress.IsDefault = true;
                        Providers.CustomerAddressData.Save(PluginEntry.DataModel, newDefaultAddress);
                    }
                }

                LoadAddresses();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvAddresses.Selection.Count == 1)
            {
                if (DialogResult.Yes == MessageDialog.Show(Properties.Resources.VerifyDeleteAddress, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    Providers.CustomerAddressData.Delete(PluginEntry.DataModel, selectedAddress);

                    if(lvAddresses.Rows.Count > 1 && selectedAddress.IsDefault)
                    {
                        CustomerAddress newDefaultAddress = customer.Addresses.FirstOrDefault(x => x.IsDefault == false && x.ID != selectedAddress.ID && x.Address.AddressType == selectedAddress.Address.AddressType);
                        if(newDefaultAddress != null)
                        {
                            newDefaultAddress.IsDefault = true;
                            Providers.CustomerAddressData.Save(PluginEntry.DataModel, newDefaultAddress);
                        }
                    }

                    LoadAddresses();
                }
            }
        }

        private void lvAddresses_SelectionChanged(object sender, EventArgs e)
        {
            selectedAddress = (lvAddresses.Selection.Count == 1) ? (CustomerAddress)lvAddresses.Selection[0].Tag : null;
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);

            btnsContextButtons.EditButtonEnabled = (lvAddresses.Selection.Count == 1) && hasEditPermission;
            btnsContextButtons.RemoveButtonEnabled = (lvAddresses.Selection.Count == 1) && hasEditPermission;
            SetSelectionLabel();
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.TabMessage, "SelectedCustomerAddressChanged", null, null);
        }

        private void lvAddresses_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void SetSelectionLabel()
        {
            if(lvAddresses.Selection.Count == 0)
            {
                lblNoSelection.Visible = true;
                lblNoSelection.Text = Properties.Resources.NoSelection;
                addressControl.Visible = false;
                return;
            }

            CustomerAddress adr = (CustomerAddress)lvAddresses.Selection[0].Tag;

            if(adr.Address.IsEmpty)
            {
                lblNoSelection.Visible = true;
                lblNoSelection.Text = Properties.Resources.NoAddressInfo;
                addressControl.Visible = false;
            }
            else
            {
                lblNoSelection.Visible = false;
                addressControl.Visible = true;
                addressControl.SetData(PluginEntry.DataModel, adr.Address);
            }
        }

        public void SetAsDefault(object sender, EventArgs args)
        {
            if(selectedAddress != null)
            {
                Providers.CustomerAddressData.SetAddressAsDefault(PluginEntry.DataModel, customer.ID, selectedAddress);
                LoadAddresses();
            }
        }

        public bool CanSetAsDefault
        {
            get
            {
                return selectedAddress != null && !selectedAddress.IsDefault && PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
            }
        }
    }
}
