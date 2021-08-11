using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    public partial class FunctionalProfileTerminalPage : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;

        public FunctionalProfileTerminalPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new FunctionalProfileTerminalPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile) internalContext;

            chkDisplayVoidedPayments.Checked = functionalityProfile.DisplayVoidedPayments;
            chkAllowTransactionsWithOpenDrawer.Checked = functionalityProfile.AllowSalesIfDrawerIsOpen;
            ntbDecimalsInNumpad.Value = functionalityProfile.DecimalsInNumpad;
            ntbMaxPrice.Value = (double) functionalityProfile.MaximumPrice;
            ntbMaxQuantity.Value = (double) functionalityProfile.MaximumQTY;

            chkSDUsesDenomination.Checked = functionalityProfile.SafeDropUsesDenomination;
            chkSDRevUsesDenomination.Checked = functionalityProfile.SafeDropRevUsesDenomination;
            chkBDUsesDenomination.Checked = functionalityProfile.BankDropUsesDenomination;
            chkBDRevUsesDenomination.Checked = functionalityProfile.BankDropRevUsesDenomination;
            chkTDUsesDenomination.Checked = functionalityProfile.TenderDeclUsesDenomination;
            chkAllowSaleAndReturn.Checked = functionalityProfile.AllowSaleAndReturnInSameTransaction;
            chkCustomerRequiredOnReturn.Checked = functionalityProfile.CustomerRequiredOnReturn;
            chkKeepDailyJournalOpenAfterPrintingReceipt.Checked = functionalityProfile.KeepDailyJournalOpenAfterPrintingReceipt;

            if (functionalityProfile.PollingInterval > 0)
            {
                chkUsePolling.Checked = true;
                ntbPollingInterval.Enabled = true;
                ntbPollingInterval.Value = functionalityProfile.PollingInterval;
            }
            else
            {
                chkUsePolling.Checked = false;
                ntbPollingInterval.Enabled = false;
                ntbPollingInterval.Text = "";
            }
            cmbClearSettings.SelectedIndex = (int) functionalityProfile.POSSettingsClear;
            ntbClearAfter.Enabled =
                lblMinutes.Enabled =
                    functionalityProfile.POSSettingsClear == FunctionalityProfile.SettingsClear.AfterLogOff;
            ntbClearAfter.Value = functionalityProfile.SettingsClearGracePeriod;

            cmbEventLog.SelectedIndex = Math.Max((int) functionalityProfile.LogLevel - 1, 0);
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (chkDisplayVoidedPayments.Checked != functionalityProfile.DisplayVoidedPayments) return true;
            if (chkAllowTransactionsWithOpenDrawer.Checked != functionalityProfile.AllowSalesIfDrawerIsOpen)
                return true;
            if (cmbEventLog.SelectedIndex != (int) functionalityProfile.LogLevel - 1) return true;
            if (ntbDecimalsInNumpad.Value != (int) functionalityProfile.DecimalsInNumpad) return true;
            if (ntbMaxPrice.Value != (double) functionalityProfile.MaximumPrice) return true;
            if (ntbMaxQuantity.Value != (double) functionalityProfile.MaximumQTY) return true;

            if (chkSDUsesDenomination.Checked != functionalityProfile.SafeDropUsesDenomination) return true;
            if (chkSDRevUsesDenomination.Checked != functionalityProfile.SafeDropRevUsesDenomination) return true;
            if (chkBDUsesDenomination.Checked != functionalityProfile.BankDropUsesDenomination) return true;
            if (chkBDRevUsesDenomination.Checked != functionalityProfile.BankDropRevUsesDenomination) return true;
            if (chkTDUsesDenomination.Checked != functionalityProfile.TenderDeclUsesDenomination) return true;
            if (chkAllowSaleAndReturn.Checked != functionalityProfile.AllowSaleAndReturnInSameTransaction) return true;
            if (chkCustomerRequiredOnReturn.Checked != functionalityProfile.CustomerRequiredOnReturn) return true;
            if (chkKeepDailyJournalOpenAfterPrintingReceipt.Checked != functionalityProfile.KeepDailyJournalOpenAfterPrintingReceipt) return true;
            if (ntbPollingInterval.Value != functionalityProfile.PollingInterval) return true;
            if (cmbClearSettings.SelectedIndex != (int) functionalityProfile.POSSettingsClear) return true;
            if (cmbClearSettings.SelectedIndex == (int) FunctionalityProfile.SettingsClear.AfterLogOff &&
                functionalityProfile.SettingsClearGracePeriod != (int) ntbClearAfter.Value) return true;

         
            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.DisplayVoidedPayments = chkDisplayVoidedPayments.Checked;
            functionalityProfile.AllowSalesIfDrawerIsOpen = chkAllowTransactionsWithOpenDrawer.Checked;
            functionalityProfile.LogLevel = (FunctionalityProfile.LogTraceLevel) (cmbEventLog.SelectedIndex + 1);
            functionalityProfile.DecimalsInNumpad = (int) ntbDecimalsInNumpad.Value;
            functionalityProfile.MaximumPrice = (int) ntbMaxPrice.Value;
            functionalityProfile.MaximumQTY = (int) ntbMaxQuantity.Value;

            functionalityProfile.SafeDropUsesDenomination = chkSDUsesDenomination.Checked;
            functionalityProfile.SafeDropRevUsesDenomination = chkSDRevUsesDenomination.Checked;
            functionalityProfile.BankDropUsesDenomination = chkBDUsesDenomination.Checked;
            functionalityProfile.BankDropRevUsesDenomination = chkBDRevUsesDenomination.Checked;
            functionalityProfile.TenderDeclUsesDenomination = chkTDUsesDenomination.Checked;
            functionalityProfile.AllowSaleAndReturnInSameTransaction = chkAllowSaleAndReturn.Checked;
            functionalityProfile.CustomerRequiredOnReturn = chkCustomerRequiredOnReturn.Checked;
            functionalityProfile.KeepDailyJournalOpenAfterPrintingReceipt = chkKeepDailyJournalOpenAfterPrintingReceipt.Checked;

            functionalityProfile.POSSettingsClear = (FunctionalityProfile.SettingsClear) cmbClearSettings.SelectedIndex;

            functionalityProfile.SettingsClearGracePeriod = (int) ntbClearAfter.Value;

            functionalityProfile.PollingInterval = chkUsePolling.Checked ? (int) ntbPollingInterval.Value : 0;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier,
            object param)
        {
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void chkUsePolling_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsePolling.Checked)
            {
                ntbPollingInterval.Value = 20;
            }
            else
            {
                ntbPollingInterval.Text = "";
                ntbPollingInterval.Value = 0;
            }
            lblSeconds.Enabled = ntbPollingInterval.Enabled = lblPollingInterval.Enabled = chkUsePolling.Checked;
        }

        private void cmbClearSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            ntbClearAfter.Enabled =
                lblMinutes.Enabled =
                    ((ComboBox) sender).SelectedIndex == (int) FunctionalityProfile.SettingsClear.AfterLogOff;
        }



    }
}
