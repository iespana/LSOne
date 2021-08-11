using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class SettingsPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;
        private WeakReference salesTaxGroupEditor;

        public SettingsPage()
        {
            InitializeComponent();
            
            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "ViewSalesTaxGroups", null);
            salesTaxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditSalesTaxGroup.Visible = (salesTaxGroupEditor != null);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;

            chkCentralizedCustomers.Checked = profile.CheckCustomer;
            chkUseCentralizedInventoryLookup.Checked = profile.UseInventoryLookup;
            chkUseGiftcard.Checked = profile.UseGiftCards;
            chkUseCentralSuspension.Checked = profile.UseCentralSuspensions;
            cmbGiftCardOption.SelectedIndex = (int)profile.IssueGiftCardOption;
            chkUserConfirmation.Checked = profile.UserConfirmation;
            chkUseCreditVouchers.Checked = profile.UseCreditVouchers;
            cmbSalesTaxGroup.SelectedData = new DataEntity(profile.NewCustomerDefaultTaxGroup, profile.NewCustomerDefaultTaxGroupName);
            cmbCashCustomer.SelectedIndex = (int)profile.CashCustomerSetting;
            cmbRefillableGiftcard.SelectedIndex = (int) profile.GiftCardRefillSetting;
            ntbMaximumGiftCardAmount.Value = (double) profile.MaximumGiftCardAmount;
            
            if (!chkUseGiftcard.Checked)
            {
                cmbGiftCardOption.Enabled = false;
            }

            if (!chkUseCentralSuspension.Checked)
            {
                chkUserConfirmation.Enabled = false;
            }

            if (!chkCentralizedCustomers.Checked)
            {
                lblDefaultSalesTaxGroup.Enabled = false;
                cmbSalesTaxGroup.Enabled = false;
                btnEditSalesTaxGroup.Enabled = false;
                cmbCashCustomer.Enabled = false;
                lblCashCustomer.Enabled = false;
            }
            
        }

        public bool DataIsModified()
        {
            if (chkCentralizedCustomers.Checked != profile.CheckCustomer) return true;
            if (chkUseCentralizedInventoryLookup.Checked != profile.UseInventoryLookup) return true;
            if (chkUseGiftcard.Checked != profile.UseGiftCards) return true;
            if (chkUseCentralSuspension.Checked != profile.UseCentralSuspensions) return true;
            if (cmbGiftCardOption.SelectedIndex != (int)profile.IssueGiftCardOption) return true;
            if (chkUserConfirmation.Checked != profile.UserConfirmation) return true;
            if (chkUseCreditVouchers.Checked != profile.UseCreditVouchers) return true;
            if (cmbSalesTaxGroup.SelectedData.ID != profile.NewCustomerDefaultTaxGroup) return true;
            if (cmbCashCustomer.SelectedIndex != (int)profile.CashCustomerSetting) return true;
            if (cmbRefillableGiftcard.SelectedIndex != (int)profile.GiftCardRefillSetting) return true;
            if (ntbMaximumGiftCardAmount.Value != (double) profile.MaximumGiftCardAmount) return true;            

            return false;
        }

        public bool SaveData()
        {
            profile.CheckCustomer = chkCentralizedCustomers.Checked;
            profile.UseInventoryLookup = chkUseCentralizedInventoryLookup.Checked;
            profile.UseGiftCards = chkUseGiftcard.Checked;
            profile.UseCentralSuspensions = chkUseCentralSuspension.Checked;
            profile.IssueGiftCardOption = (SiteServiceProfile.IssueGiftCardOptionEnum)cmbGiftCardOption.SelectedIndex;
            profile.UserConfirmation = chkUserConfirmation.Checked;
            profile.UseCreditVouchers = chkUseCreditVouchers.Checked;
            profile.NewCustomerDefaultTaxGroup = cmbSalesTaxGroup.SelectedData.ID;
            profile.NewCustomerDefaultTaxGroupName = cmbSalesTaxGroup.SelectedData.Text;
            profile.CashCustomerSetting = (SiteServiceProfile.CashCustomerSettingEnum)cmbCashCustomer.SelectedIndex;
            profile.GiftCardRefillSetting = (SiteServiceProfile.GiftCardRefillSettingEnum)cmbRefillableGiftcard.SelectedIndex;
            profile.MaximumGiftCardAmount = (decimal) ntbMaximumGiftCardAmount.Value;            
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

        private void chkUseGiftcard_CheckedChanged(object sender, EventArgs e)
        {
            cmbGiftCardOption.Enabled = chkUseGiftcard.Checked;
        }

        private void chkUseCentralSuspension_CheckedChanged(object sender, EventArgs e)
        {
            chkUserConfirmation.Checked = chkUserConfirmation.Enabled = chkUseCentralSuspension.CheckState == CheckState.Checked;
        }

        private void btnEditSalesTaxGroup_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupEditor.IsAlive)
            {
                ((IPlugin)salesTaxGroupEditor.Target).Message(this, "ViewSalesTaxGroups", cmbSalesTaxGroup.SelectedData.ID);
            }
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel, TaxGroupTypeFilter.Standard), null);
        }

        private void DualDataComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void chkCentralizedCustomers_CheckedChanged(object sender, EventArgs e)
        {
            lblDefaultSalesTaxGroup.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            cmbSalesTaxGroup.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            btnEditSalesTaxGroup.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            cmbCashCustomer.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            lblCashCustomer.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
        }
    }
}
