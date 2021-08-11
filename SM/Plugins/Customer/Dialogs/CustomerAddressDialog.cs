using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    public partial class CustomerAddressDialog : DialogBase
    {
        public CustomerAddress Address { get; private set; }

        public CustomerAddressDialog()
        {
            InitializeComponent();

            addressField.DataModel = PluginEntry.DataModel;

            cmAddressType.Items.Clear();
            btnOK.Enabled = false;
        }

        // Editing a previous address
        public CustomerAddressDialog(CustomerAddress address) : this()
        {
            Address = address;
            addressField.SetData(PluginEntry.DataModel, address.Address);

            tbContact.Text = address.ContactName;
            tbPhone.Text = address.Telephone;
            tbMobilePhone.Text = address.MobilePhone;
            tbEmail.Text = address.Email;

            AddAddressTypesToCombo(new List<Address.AddressTypes>
            { Utilities.DataTypes.Address.AddressTypes.Shipping,
              Utilities.DataTypes.Address.AddressTypes.Billing },
                Utilities.DataTypes.Address.AddressTypeToString(address.Address.AddressType));
            btnOK.Enabled = false;
        }

        // Adding a new address
        public CustomerAddressDialog(IDataEntity customer) : this()
        {
            Address = new CustomerAddress { Address = { AddressType = Utilities.DataTypes.Address.AddressTypes.Shipping }, CustomerID = customer.ID };

            addressField.SetData(PluginEntry.DataModel, Address.Address);

            tbContact.Text = Address.ContactName;
            tbPhone.Text = Address.Telephone;
            tbMobilePhone.Text = Address.MobilePhone;
            tbEmail.Text = Address.Email;

            AddAddressTypesToCombo(new List<Address.AddressTypes>
            { Utilities.DataTypes.Address.AddressTypes.Shipping,
              Utilities.DataTypes.Address.AddressTypes.Billing }, null);
            btnOK.Enabled = false;
        }

        private void AddAddressTypesToCombo(IEnumerable<Address.AddressTypes> addressTypes, string current)
        {
            var sorted = new SortedList<string, string>();
            cmAddressType.Items.Clear();
            foreach (var addressType in addressTypes)
            {
                var value = LSOne.Utilities.DataTypes.Address.AddressTypeToString(addressType);
                sorted[value] = value;
            }
            int index = 0;
            int selected = 0;
            foreach (var value in sorted.Values)
            {
                cmAddressType.Items.Add(value);
                if (value == current)
                    selected = index;
                index++;
            }
            cmAddressType.SelectedIndex = selected;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            cmbSalesTaxGroup.SelectedData = new DataEntity(Address.TaxGroup, Address.TaxGroupName);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private bool Save()
        {
            var addressType = LSOne.Utilities.DataTypes.Address.StringToAddressType(cmAddressType.SelectedItem.ToString());
            if (addressType != Address.Address.AddressType ||
                addressField.IsDirty ||
                IsDifferent(tbContact.Text, Address.ContactName) ||
                IsDifferent(tbPhone.Text, Address.Telephone) ||
                IsDifferent(tbMobilePhone.Text, Address.MobilePhone) ||
                IsDifferent(tbEmail.Text, Address.Email) ||
                ((DataEntity)cmbSalesTaxGroup.SelectedData).ID != Address.TaxGroup)
            {
                Address.Address.AddressType = addressType;

                Address.ContactName = tbContact.Text;
                Address.Telephone = tbPhone.Text;
                Address.MobilePhone = tbMobilePhone.Text;
                Address.Email = tbEmail.Text;

                var tax = ((DataEntity)cmbSalesTaxGroup.SelectedData);
                Address.TaxGroup = tax.ID.ToString();
                if (tax.ID == RecordIdentifier.Empty)
                    Address.TaxGroupName = "";
                else
                    Address.TaxGroupName = tax.Text;
                Address.Address = addressField.AddressRecord;

                Address.Address.AddressType = LSOne.Utilities.DataTypes.Address.StringToAddressType(cmAddressType.SelectedItem.ToString());

                return true;
            }

            return false;
        }

        private static bool IsDifferent(string a, string b)
        {
            return 0 != string.Compare(a, b);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            CheckEnabled(sender, e);
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel),
                null);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool dataChanged = Address == null ||
                              (Address.Address != addressField.AddressRecord ||
                               Address.ContactName != tbContact.Text ||
                               Address.Telephone != tbPhone.Text ||
                               Address.MobilePhone != tbMobilePhone.Text ||
                               (Address.Email != tbEmail.Text && tbEmail.Text.IsEmail()) ||
                               cmbSalesTaxGroup.SelectedDataID != Address.TaxGroup);

            btnOK.Enabled = dataChanged;
        }
    }
}
