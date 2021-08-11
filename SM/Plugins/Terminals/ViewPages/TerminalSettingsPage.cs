using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    public partial class TerminalSettingsPage : UserControl, ITabView
    {
        private Terminal terminal;

        public TerminalSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalSettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;

            chkAutomaticLogout.Checked = (terminal.AutoLogOffTimeout > 0);
            ntbAutomaticLogoutTime.Value = terminal.AutoLogOffTimeout;
            ntbAutomaticLogoutTime.Enabled = chkAutomaticLogout.Checked;

            chkAutomaticLock.Checked = (terminal.AutoLockTimeout > 0);
            ntbAutomaticLockTime.Value = terminal.AutoLockTimeout;
            ntbAutomaticLockTime.Enabled = chkAutomaticLock.Checked;

            chkOpenDrawer.Checked = terminal.OpenDrawerAtLoginLogout;            
            chkExitAfterTransaction.Checked = terminal.ExitAfterEachTransaction;
            chkIsActivated.Checked = terminal.Activated;
            dtLastActivated.Value = terminal.LastActivatedDate;
            dtLastActivated.Checked = terminal.LastActivatedDate.Date != new DateTime(1900, 1, 1);
            cmbReceiptNumberSeries.SelectedData = Providers.NumberSequenceData
                .Get(PluginEntry.DataModel, terminal.ReceiptIDNumberSequence);
            txtFormInfo1.Text = terminal.FormInfoField1;
            txtFormInfo2.Text = terminal.FormInfoField2;
            txtFormInfo3.Text = terminal.FormInfoField3;
            txtFormInfo4.Text = terminal.FormInfoField4;

        }

        public bool DataIsModified()
        {
            if ((int)ntbAutomaticLogoutTime.Value != terminal.AutoLogOffTimeout) return true;
            if ((int)ntbAutomaticLockTime.Value != terminal.AutoLockTimeout) return true;
            if (chkOpenDrawer.Checked != terminal.OpenDrawerAtLoginLogout) return true;            
            if (chkExitAfterTransaction.Checked != terminal.ExitAfterEachTransaction) return true;
            if (chkIsActivated.Checked != terminal.Activated) return true;
            if (cmbReceiptNumberSeries.SelectedData != null && cmbReceiptNumberSeries.SelectedData.ID != terminal.ReceiptIDNumberSequence) return true;
            if (txtFormInfo1.Text != terminal.FormInfoField1) return true;
            if (txtFormInfo2.Text != terminal.FormInfoField2) return true;
            if (txtFormInfo3.Text != terminal.FormInfoField3) return true;
            if (txtFormInfo4.Text != terminal.FormInfoField4) return true;

            return false;
        }

        public bool SaveData()
        {
            terminal.AutoLogOffTimeout = (int)ntbAutomaticLogoutTime.Value;
            terminal.AutoLockTimeout = (int)ntbAutomaticLockTime.Value;
            terminal.OpenDrawerAtLoginLogout = chkOpenDrawer.Checked;            
            terminal.ExitAfterEachTransaction = chkExitAfterTransaction.Checked;
            if (!terminal.Activated && chkIsActivated.Checked)
            {
                terminal.LastActivatedDate = DateTime.Now;
            }
            terminal.Activated = chkIsActivated.Checked;         
            terminal.ReceiptIDNumberSequence = cmbReceiptNumberSeries.SelectedData != null ? cmbReceiptNumberSeries.SelectedData.ID : "";
            terminal.FormInfoField1 = txtFormInfo1.Text;
            terminal.FormInfoField2 = txtFormInfo2.Text;
            terminal.FormInfoField3 = txtFormInfo3.Text;
            terminal.FormInfoField4 = txtFormInfo4.Text;

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

        private void chkAutomaticLogout_CheckedChanged(object sender, EventArgs e)
        {
            // Automatic logout takes presedence over automatic lockout so if we select logout we disable logout
            if (chkAutomaticLogout.Checked)
            {
                ntbAutomaticLogoutTime.Value = 10;
                ntbAutomaticLogoutTime.Enabled = true;

                chkAutomaticLock.Enabled = false;
                chkAutomaticLock.Checked = false;
                ntbAutomaticLockTime.Enabled = false;
                ntbAutomaticLockTime.Value = 0;
            }
            else
            {
                ntbAutomaticLogoutTime.Value = 0;
                ntbAutomaticLogoutTime.Enabled = false;

                chkAutomaticLock.Enabled = true;
                chkAutomaticLock_CheckedChanged(this, EventArgs.Empty);
            }
        }

        private void chkAutomaticLock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutomaticLock.Checked)
            {
                ntbAutomaticLockTime.Value = 10;
                ntbAutomaticLockTime.Enabled = true;
            }
            else
            {
                ntbAutomaticLockTime.Value = 0;
                ntbAutomaticLockTime.Enabled = false;
            }
        }

        private void cmbReceiptNumberSeries_RequestData(object sender, EventArgs e)
        {
            List<NumberSequence> sequences = Providers.NumberSequenceData.GetList(PluginEntry.DataModel);

            sequences.Insert(0, new NumberSequence { ID = "", Text = "" });

            cmbReceiptNumberSeries.SetWidth(450);
            cmbReceiptNumberSeries.SetData(sequences, null, true);
        }

        private void chkIsActivated_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkIsActivated.Checked && terminal.Activated)
            {
                chkIsActivated.Checked =  MessageBox.Show(Properties.Resources.ReactivationMessage,
                    Properties.Resources.ReactivationWarning,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
                               
            }
        }
        
    }
}
