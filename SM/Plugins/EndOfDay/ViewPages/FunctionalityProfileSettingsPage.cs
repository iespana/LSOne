using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    public partial class FunctionalityProfileSettingsPage : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;

        public FunctionalityProfileSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.FunctionalityProfileSettingsPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;
            chkIncludeTenderDeclaration.Checked = functionalityProfile.ZReportConfig.IncludeTenderDeclaration;
            chkIncludeFloatInCash.Checked = functionalityProfile.ZReportConfig.IncludeFloatInCashSummary;
            chkCombineGrandTotal.Checked = functionalityProfile.ZReportConfig.CombineGrandTotalSalesandReturns;
            chkCombineSalesReport.Checked = functionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport;
            chkDisplayOtherInfo.Checked = functionalityProfile.ZReportConfig.DisplayOtherInfoSection;
            chkDisplayReturns.Checked = functionalityProfile.ZReportConfig.DisplayReturnInfo;
            chkDisplaySuspended.Checked = functionalityProfile.ZReportConfig.DisplaySuspendedInfo;
            cmbGTDisplayAmounts.SelectedIndex = (int)functionalityProfile.ZReportConfig.GrandTotalAmountDisplay;
            cmbSRDisplayAmount.SelectedIndex = (int)functionalityProfile.ZReportConfig.SalesReportAmountDisplay;
            cmbOrderDeposit.SelectedIndex = (int) functionalityProfile.ZReportConfig.OrderByDepositInfo;
            chkDisplayDeposit.Checked = functionalityProfile.ZReportConfig.DisplayDepositInfo;
            ntbReportWidth.Value = functionalityProfile.ZReportConfig.ReportWidth;
            chkDisplayOverShortAmount.Checked = functionalityProfile.ZReportConfig.DisplayOverShortAmount;
            chkPrintGrandTotals.Checked = functionalityProfile.ZReportConfig.PrintGrandTotals;
            chkShowIndividualDeposits.Checked = functionalityProfile.ZReportConfig.ShowIndividualDeposits;

            chkPrintGrandTotals_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkIncludeFloatInCash.Checked != functionalityProfile.ZReportConfig.IncludeFloatInCashSummary) return true;
            if (chkCombineGrandTotal.Checked != functionalityProfile.ZReportConfig.CombineGrandTotalSalesandReturns) return true;
            if (chkIncludeTenderDeclaration.Checked != functionalityProfile.ZReportConfig.IncludeTenderDeclaration) return true;
            if (chkCombineSalesReport.Checked != functionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport) return true;
            if (chkDisplayOtherInfo.Checked != functionalityProfile.ZReportConfig.DisplayOtherInfoSection) return true;
            if (chkDisplayReturns.Checked != functionalityProfile.ZReportConfig.DisplayReturnInfo) return true;
            if (chkDisplaySuspended.Checked != functionalityProfile.ZReportConfig.DisplaySuspendedInfo) return true;
            if (chkDisplayDeposit.Checked != functionalityProfile.ZReportConfig.DisplayDepositInfo) return true;
            if ((int)ntbReportWidth.Value != functionalityProfile.ZReportConfig.ReportWidth) return true;
            if (cmbGTDisplayAmounts.SelectedIndex != (int)functionalityProfile.ZReportConfig.GrandTotalAmountDisplay) return true;
            if (cmbSRDisplayAmount.SelectedIndex != (int)functionalityProfile.ZReportConfig.SalesReportAmountDisplay) return true;
            if (cmbOrderDeposit.SelectedIndex != (int)functionalityProfile.ZReportConfig.OrderByDepositInfo) return true;
            if (chkDisplayOverShortAmount.Checked != functionalityProfile.ZReportConfig.DisplayOverShortAmount) return true;
            if (chkPrintGrandTotals.Checked != functionalityProfile.ZReportConfig.PrintGrandTotals) return true;
            if (chkShowIndividualDeposits.Checked != functionalityProfile.ZReportConfig.ShowIndividualDeposits) return true;

            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.ZReportConfig.IncludeTenderDeclaration= chkIncludeTenderDeclaration.Checked;
            functionalityProfile.ZReportConfig.IncludeFloatInCashSummary = chkIncludeFloatInCash.Checked;
            functionalityProfile.ZReportConfig.CombineGrandTotalSalesandReturns = chkCombineGrandTotal.Checked;
            functionalityProfile.ZReportConfig.CombineSaleAndReturnXZReport = chkCombineSalesReport.Checked;
            functionalityProfile.ZReportConfig.DisplayOtherInfoSection = chkDisplayOtherInfo.Checked;
            functionalityProfile.ZReportConfig.DisplayReturnInfo = chkDisplayReturns.Checked;
            functionalityProfile.ZReportConfig.DisplaySuspendedInfo = chkDisplaySuspended.Checked;
            functionalityProfile.ZReportConfig.GrandTotalAmountDisplay = (GrandTotalAmtDisplay)cmbGTDisplayAmounts.SelectedIndex;
            functionalityProfile.ZReportConfig.SalesReportAmountDisplay = (SalesReportAmtdisplay)cmbSRDisplayAmount.SelectedIndex;
            functionalityProfile.ZReportConfig.DisplayDepositInfo = chkDisplayDeposit.Checked;
            functionalityProfile.ZReportConfig.ReportWidth = (int)ntbReportWidth.Value;
            functionalityProfile.ZReportConfig.OrderByDepositInfo = (DepositOrderBy)cmbOrderDeposit.SelectedIndex;
            functionalityProfile.ZReportConfig.DisplayOverShortAmount = chkDisplayOverShortAmount.Checked;
            functionalityProfile.ZReportConfig.PrintGrandTotals = chkPrintGrandTotals.Checked;
            functionalityProfile.ZReportConfig.ShowIndividualDeposits = chkShowIndividualDeposits.Checked;

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

        private void chkPrintGrandTotals_CheckedChanged(object sender, System.EventArgs e)
        {
            lblCombineGrandTotal.Enabled = chkCombineGrandTotal.Enabled =
                lblDisplayOverShortAmount.Enabled = chkDisplayOverShortAmount.Enabled =
                lblGTDisplayAmounts.Enabled = cmbGTDisplayAmounts.Enabled = chkPrintGrandTotals.Checked;
        }
    }
}