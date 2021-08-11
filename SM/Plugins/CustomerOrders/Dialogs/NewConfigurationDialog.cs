using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.CustomerOrders.ViewPages;

namespace LSOne.ViewPlugins.CustomerOrders.Dialogs
{
    enum TypeIndex
    {
        Delivery = 0,
        Source = 1
    }

    public partial class NewConfigurationDialog : DialogBase
    {
        public CustomerOrderAdditionalConfigurations Configuration { get; set; }
        public bool MultipleItemsCreated { get; set; }

        private bool editConfiguration;
        private List<CustomerOrderAdditionalConfigurations> configList;
        
        

        public NewConfigurationDialog(CustomerOrderAdditionalConfigurations config, bool configIsInUse)
        {
            InitializeComponent();

            this.Configuration = config ?? new CustomerOrderAdditionalConfigurations();
            editConfiguration = this.Configuration.ID != Guid.Empty;
            MultipleItemsCreated = false;
            chkCreateAnother.Enabled = !editConfiguration;

            Header = editConfiguration ? Properties.Resources.EditConfiguration : Properties.Resources.NewConfiguration;
            Text = editConfiguration ? Properties.Resources.EditConfiguration : Properties.Resources.NewConfiguration;
            cmbTypes.Items.Add(Properties.Resources.Delivery);
            cmbTypes.Items.Add(Properties.Resources.Source);

            configList = Providers.CustomerOrderAdditionalConfigData.GetList(PluginEntry.DataModel);

            if (Configuration.ID != Guid.Empty)
            {
                tbDescription.Text = Configuration.Text;
                cmbTypes.SelectedIndex = SelectedIndex(Configuration.AdditionalType);
                cmbTypes.Enabled = !configIsInUse;
            }

            CheckEnabled(this, new EventArgs());
        }

        private int SelectedIndex(ConfigurationType type)
        {
            if (type == ConfigurationType.Delivery)
            {
                return (int) TypeIndex.Delivery;
            }

            return (int)TypeIndex.Source;
        }

        private ConfigurationType SelectedIndex(int index)
        {
            if (index == (int) TypeIndex.Delivery)
            {
                return ConfigurationType.Delivery;
            }

            return ConfigurationType.Source;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider.Clear();

            //If either type or description is empty then OK button should be disabled
            if (cmbTypes.SelectedIndex == -1 || string.IsNullOrEmpty(tbDescription.Text.Trim()))
            {
                btnOK.Enabled = false;
                return;
            }

            if (editConfiguration && tbDescription.Text != Configuration.Text)
            {
                btnOK.Enabled = true;
                return;
            }
            
            if (editConfiguration && SelectedIndex(cmbTypes.SelectedIndex) != Configuration.AdditionalType || (!editConfiguration && SelectedIndex(cmbTypes.SelectedIndex) >= ConfigurationType.None))
            {
                btnOK.Enabled = true;
                return;
            }

            btnOK.Enabled = false;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CustomerOrderAdditionalConfigurations exists = configList.FirstOrDefault(f => f.Text.ToLower() == tbDescription.Text.ToLower());
            if (exists != null)
            {
                if (!(editConfiguration && Configuration.AdditionalType != SelectedIndex(cmbTypes.SelectedIndex)))
                {
                    errorProvider.SetError(tbDescription, Properties.Resources.ConfigurationDescriptionAlreadyExists);
                    btnOK.Enabled = false;
                    return;
                }
            }

            Configuration.Text = tbDescription.Text;
            Configuration.AdditionalType = SelectedIndex(cmbTypes.SelectedIndex);

            Providers.CustomerOrderAdditionalConfigData.Save(PluginEntry.DataModel, Configuration);

            if (chkCreateAnother.Checked)
            {
                tbDescription.Text = "";
                tbDescription.Focus();
                MultipleItemsCreated = true;

                configList.Add((CustomerOrderAdditionalConfigurations)Configuration.Clone());

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Configuration", Configuration.ID, Configuration);
                Configuration.Clear();
            }
            else
            {
                if (MultipleItemsCreated)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiAdd, "Configuration", null, null);
                }
                else
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, editConfiguration ? DataEntityChangeType.Edit : DataEntityChangeType.Add, "Configuration", Configuration.ID, Configuration);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MultipleItemsCreated)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiAdd, "Configuration", null, null);
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }
    }
}
