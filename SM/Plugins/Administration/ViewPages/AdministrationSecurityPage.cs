using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Administration.Properties;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationSecurityPage : ContainerControl, ITabViewV2
    {
        private Setting writeAuditLevel;
        private Setting passwordExpires;
        private Setting passwordLockoutThreshold;
        private Setting domain;
        private Setting domainEnabled;

        private ToolTip errorMessage;

        public AdministrationSecurityPage()
        {
            InitializeComponent();

            GetDataFromDatabase();
        }

        private void GetDataFromDatabase()
        {
            writeAuditLevel = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.WriteAuditing, SettingsLevel.System);
            passwordExpires = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.PasswordExpires, SettingsLevel.System);

            passwordLockoutThreshold = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.PasswordLockoutThreshold, SettingsLevel.System);

            domain = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.ActiveDirectoryDomain, SettingsLevel.System);
            domainEnabled = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.ActiveDirectoryEnabled, SettingsLevel.System);

            LoadData(false, 0, null);
        }

        private void PopulateControls()
        {
            cmbWriteAuditing.SelectedIndex = writeAuditLevel.IntValue;
            tbMaxPasswordAge.Value = passwordExpires.IntValue;
            ckbPasswordNeverExpires.Checked = passwordExpires.IntValue == 0;
            tbPasswordLockout.Value = passwordLockoutThreshold.IntValue;
            tbDomain.Text = domain.Value;
            chkEnabled.Checked = domainEnabled.BoolValue;
        }

        private void NewMethod(object sender, EventArgs args)
        {
            if (ckbPasswordNeverExpires.Checked)
            {
                ckbPasswordNeverExpires.Tag = tbMaxPasswordAge.Value;
                tbMaxPasswordAge.Value = 0;
            }
            else
            {
                tbMaxPasswordAge.Value = ((double)ckbPasswordNeverExpires.Tag == 0) ? 90 : (double)ckbPasswordNeverExpires.Tag;
            }
            
            tbMaxPasswordAge.Enabled = !ckbPasswordNeverExpires.Checked;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.AdministrationSecurityPage();
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }
        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            PopulateControls();
        }

        public bool DataIsModified()
        {
            bool returnValue = false;

            returnValue = returnValue || ((int)tbPasswordLockout.Value != passwordLockoutThreshold.IntValue);
            returnValue = returnValue || ((int)tbMaxPasswordAge.Value != passwordExpires.IntValue);
            //returnValue = returnValue || (ckbPasswordNeverExpires.Checked != (passwordExpires.IntValue == 0));

            returnValue = returnValue || (cmbWriteAuditing.SelectedIndex != writeAuditLevel.IntValue);

            returnValue = returnValue || (tbDomain.Text != domain.Value);
            returnValue = returnValue || (chkEnabled.Checked != domainEnabled.BoolValue);

            return returnValue;
        }

        public bool SaveData()
        {
            if ((int)tbMaxPasswordAge.Value != passwordExpires.IntValue)
            {
                passwordExpires.IntValue = (int)tbMaxPasswordAge.Value;
                
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.PasswordExpires, SettingsLevel.System, passwordExpires);
            }

            if (passwordLockoutThreshold.IntValue != (int)tbPasswordLockout.Value && (int)tbPasswordLockout.Value != 0)
            {
                passwordLockoutThreshold.IntValue = (int)tbPasswordLockout.Value;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.PasswordLockoutThreshold, SettingsLevel.System, passwordLockoutThreshold);
            }


            if (cmbWriteAuditing.SelectedIndex != writeAuditLevel.IntValue)
            {
                writeAuditLevel.IntValue = cmbWriteAuditing.SelectedIndex;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.WriteAuditing, SettingsLevel.System, writeAuditLevel);  
            }

            if (tbDomain.Text != domain.Value)
            {
                domain.Value = tbDomain.Text;
                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.ActiveDirectoryDomain, SettingsLevel.System, domain);
            }

            if (chkEnabled.Checked != domainEnabled.BoolValue)
            {
                domainEnabled.BoolValue = chkEnabled.Checked;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, Settings.ActiveDirectoryEnabled, SettingsLevel.System, domainEnabled);
            }
           

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            GetDataFromDatabase();
            PopulateControls();
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }
        #endregion

        private void AdministrationSecurityPage_Load(object sender, EventArgs e)
        {

        }

        private void tbMaxPasswordAge_ValueChanged(object sender, EventArgs e)
        {
            if (tbMaxPasswordAge.Value == 0)
            {
                ckbPasswordNeverExpires.Checked = true;
            }
        }

        private void tbPasswordLockout_Leave(object sender, EventArgs e)
        {
            if (tbPasswordLockout.Value == 0)
            {
                tbPasswordLockout.Value = 1;
            }
        }

        private void tbPasswordLockout_TextChanged(object sender, EventArgs e)
        {
            if (tbPasswordLockout.Text != string.Empty && tbPasswordLockout.Value == 0)
            {
                if (errorMessage == null)
                {
                    errorMessage = new ToolTip();
                    errorMessage.IsBalloon = true;
                    errorMessage.ShowAlways = true;
                    errorMessage.ToolTipIcon = ToolTipIcon.Error;
                    errorMessage.ToolTipTitle = Resources.LockOutCannotBe0;
                }
                errorMessage.Show(string.Empty, tbPasswordLockout, 0);//Because of a known positioning bug                
                errorMessage.Show(" ", tbPasswordLockout);
            }
            else if (errorMessage != null)
            {
                errorMessage.Hide(tbPasswordLockout);
            }
        }
    }
}
