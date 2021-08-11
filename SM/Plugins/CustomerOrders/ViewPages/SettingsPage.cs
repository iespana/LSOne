using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Interfaces;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.CustomerOrders.ViewPages
{
    public partial class SettingsPage : ContainerControl, ITabView
    {
        WeakReference owner;
        private CustomerOrderType settingsType;
        private CustomerOrderSettings currentSettings;

        private List<CustomerOrderSettings> settingsList;

        public SettingsPage(CustomerOrderType settingsType)
        {
            InitializeComponent();
            this.settingsType = settingsType;

            chkAcceptsDeposits.Visible =
                lblAcceptsDeposits.Visible =
                    ntbMinDeposit.Visible =
                        lblMinDeposits.Visible =
                            lblPercent.Visible =
                                lnkDeposits.Visible = (settingsType == CustomerOrderType.CustomerOrder);

            currentSettings = Providers.CustomerOrderSettingsData.Get(PluginEntry.DataModel, settingsType);

            ntbMinDeposit.Enabled = false;
        }

        public SettingsPage(TabControl owner, CustomerOrderType settingsType)
            : this(settingsType)
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            CustomerOrderType settingsType = tab.Text == Properties.Resources.Orders ? CustomerOrderType.CustomerOrder : CustomerOrderType.Quote;
            return new SettingsPage((TabControl)sender, settingsType);
        }

        public bool DataIsModified()
        {
            if (currentSettings.Empty()) return true;

            if (chkAcceptsDeposits.Checked != currentSettings.AcceptsDeposits) return true;
            if (chkSelectSource.Checked != currentSettings.SelectSource) return true;
            if (chkSelectDelivery.Checked != currentSettings.SelectDelivery) return true;
            if (ntbMinDeposit.Value != (double) currentSettings.MinimumDeposits) return true;
            if (ntbExpiresIn.Value != (int) currentSettings.ExpireTimeValue) return true;
            if (cmbExpiresIn.SelectedIndex != (int) currentSettings.ExpirationTimeUnit) return true;
            if ((cmbNumberSeries.SelectedDataID == null && currentSettings.NumberSeries.StringValue != "") || (cmbNumberSeries.SelectedDataID != null && cmbNumberSeries.SelectedDataID.StringValue != currentSettings.NumberSeries.StringValue)) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {

            settingsList = (List<CustomerOrderSettings>) internalContext;
            currentSettings = settingsList.FirstOrDefault(f => f.SettingsType == settingsType) ?? new CustomerOrderSettings();

            chkAcceptsDeposits.Checked = currentSettings.AcceptsDeposits;
            ntbMinDeposit.Value = (double) currentSettings.MinimumDeposits;

            chkSelectSource.Checked = currentSettings.SelectSource;
            chkSelectDelivery.Checked = currentSettings.SelectDelivery;

            ntbExpiresIn.Value = (int) currentSettings.ExpireTimeValue;
            cmbExpiresIn.SelectedIndex = (int)currentSettings.ExpirationTimeUnit;

            if (currentSettings.NumberSeries == null || currentSettings.NumberSeries.StringValue == "")
            {
                cmbNumberSeries.SelectedData = new DataEntity();
            }
            else
            {
                NumberSequence sequence = Providers.NumberSequenceData.Get(PluginEntry.DataModel, currentSettings.NumberSeries);
                if (sequence != null)
                {
                    cmbNumberSeries.SelectedData = sequence;
                }
            }

        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            if (currentSettings == null)
            {
                currentSettings = new CustomerOrderSettings();
            }

            currentSettings.AcceptsDeposits = chkAcceptsDeposits.Checked;
            currentSettings.SelectSource = chkSelectSource.Checked;
            currentSettings.SelectDelivery = chkSelectDelivery.Checked;

            currentSettings.MinimumDeposits = (decimal)ntbMinDeposit.Value;
            currentSettings.ExpireTimeValue = (int)ntbExpiresIn.Value;
            currentSettings.ExpirationTimeUnit = (TimeUnitEnum) cmbExpiresIn.SelectedIndex;

            currentSettings.NumberSeries = cmbNumberSeries.SelectedDataID ?? RecordIdentifier.Empty;

            currentSettings.SettingsType = settingsType;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, settingsType == CustomerOrderType.CustomerOrder ? "CustomerOrder" : "Quote", (int)settingsType, currentSettings);

            return true;
        }

        public void SaveUserInterface()
        {
            throw new NotImplementedException();
        }

        private void cmbNumberSeries_RequestData(object sender, EventArgs e)
        {
            List<NumberSequence> sequences = Providers.NumberSequenceData.GetList(PluginEntry.DataModel);
            cmbNumberSeries.SetData(sequences, null, true);
        }

        private void cmbNumberSeries_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void chkAcceptsDeposits_CheckedChanged(object sender, EventArgs e)
        {
            ntbMinDeposit.Enabled = chkAcceptsDeposits.Checked;
            if (ntbMinDeposit.Value < ntbMinDeposit.MinValue)
            {
                ntbMinDeposit.Value = ntbMinDeposit.MinValue;
            }

            if (ntbMinDeposit.Value > ntbMinDeposit.MaxValue)
            {
                ntbMinDeposit.Value = ntbMinDeposit.MaxValue;
            }
        }

        //private void ntbMinDeposit_Leave(object sender, EventArgs e)
        //{
        //    if (ntbMinDeposit.Value < ntbMinDeposit.MinValue)
        //    {
        //        MessageDialog.Show(Properties.Resources.MinimumValueCannotBeLess);
        //        ntbMinDeposit.Value = ntbMinDeposit.MinValue;
        //    }
        //}
    }
}
