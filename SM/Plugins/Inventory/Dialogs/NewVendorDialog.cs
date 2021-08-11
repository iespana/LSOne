using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class NewVendorDialog : DialogBase
    {
        RecordIdentifier vendorID;
        WeakReference currencyAdder;
        bool manuallyEnterID = false;
       
        public NewVendorDialog()
        {
            IPlugin plugin;

            vendorID = RecordIdentifier.Empty;

            InitializeComponent();

            Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            manuallyEnterID = parameters.ManuallyEnterVendorID;

            tbID.Visible = manuallyEnterID;
            lblID.Visible = manuallyEnterID;

            cmbCurrency.SelectedData = new DataEntity("", "");

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);

            if (plugin != null)
            {
                btnAddCurrency.Visible = true;

                currencyAdder = new WeakReference(plugin);
            }
            else
            {
                currencyAdder = null;
            }

            addressField.DataModel = PluginEntry.DataModel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            addressField.SetData(PluginEntry.DataModel, new Address(PluginEntry.DataModel.Settings.AddressFormat));
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier VendorID
        {
            get { return vendorID; }
        }

        private bool Save()
        {
            Vendor vendor = new Vendor();

            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                if (manuallyEnterID)
                {
                    if (tbID.Text.Trim() == "")
                    {
                        if (QuestionDialog.Show(Properties.Resources.IDMissingQuestion, Properties.Resources.IDMissing) != System.Windows.Forms.DialogResult.Yes)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!tbID.Text.IsAlphaNumeric())
                        {
                            errorProvider1.SetError(tbID, Properties.Resources.OnlyCharAndNumbers);
                            return false;
                        }

                        vendor.ID = tbID.Text.Trim();



                        if (Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).VendorExists(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            vendor.ID,
                            true))
                        {
                            errorProvider1.SetError(tbID, Properties.Resources.VendorIDExists);
                            return false;
                        }
                    }
                }

                vendor.Text = tbDescription.Text;
                vendor.Address2 = addressField.AddressRecord.Address2;
                vendor.Address1 = addressField.AddressRecord.Address1;
                vendor.City = addressField.AddressRecord.City;
                vendor.ZipCode = addressField.AddressRecord.Zip;
                vendor.State = addressField.AddressRecord.State;
                vendor.Country = addressField.AddressRecord.Country;
                vendor.AddressFormat = addressField.AddressFormat;
                vendor.CurrencyID = cmbCurrency.SelectedData.ID;

                //Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveVendor(
                //    PluginEntry.DataModel,
                //    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
                //    vendor,
                //    true);
                
                vendor = service.SaveAndReturnVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendor, true);
                vendorID = vendor.ID;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void cmbCurrency_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == RecordIdentifier.Empty || ((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = ((DataEntity)e.Data).Text;
            }
            else
            {
                e.TextToDisplay = (string)((DataEntity)e.Data).ID + " - " + ((DataEntity)e.Data).Text;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = tbDescription.Text.Length > 0 && cmbCurrency.SelectedData.ID != "";
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            ((IPlugin)currencyAdder.Target).Message(this, "AddCurrency", null);
        }

        private void cmbCurrency_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }
    }
}
