using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    

    public partial class StoreTenderUsagePage : UserControl, ITabView
    {
        StorePaymentMethod paymentMethod;


        public StoreTenderUsagePage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreTenderUsagePage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            paymentMethod = (StorePaymentMethod)internalContext;

            if (paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.FloatTender || paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.DepositTender) 
            {
                SetFloatingTenderControls(false);
            }
            else
            {
                SetFloatingTenderControls(true);
            }

            label6.Enabled = chkAllowNegativePaymentAmounts.Enabled = (paymentMethod.PosOperation != (int)POSOperations.PayCreditMemo && paymentMethod.PosOperation != (int)POSOperations.PayGiftCertificate);
            chkAllowNegativePaymentAmounts.Checked = chkAllowNegativePaymentAmounts.Enabled && paymentMethod.AllowNegativePaymentAmounts;

            chkPaymentTypeCanBeVoided.Checked = paymentMethod.PaymentTypeCanBeVoided;

            chkCountingRequired.Checked = paymentMethod.CountingRequired;
            chkAllowFloat.Checked = paymentMethod.AllowFloat;
            chkAllowBankDrop.Checked = paymentMethod.AllowBankDrop;
            chkAllowSafeDrop.Checked = paymentMethod.AllowSafeDrop;
            chkAmountInPOSLimiting.Checked = paymentMethod.MaximumAmountInPOSEnabled;
            ntbMaximumAmount.Value = (double)paymentMethod.MaximumAmountInPOS;
        }

        public bool DataIsModified()
        {
            if (chkAllowNegativePaymentAmounts.Checked != paymentMethod.AllowNegativePaymentAmounts) return true;
            if (chkPaymentTypeCanBeVoided.Checked != paymentMethod.PaymentTypeCanBeVoided) return true;
            if (chkCountingRequired.Checked != paymentMethod.CountingRequired) return true;
            if (chkAllowFloat.Checked != paymentMethod.AllowFloat) return true;
            if (chkAllowBankDrop.Checked != paymentMethod.AllowBankDrop) return true;
            if (chkAllowSafeDrop.Checked != paymentMethod.AllowSafeDrop) return true;
            if (chkAmountInPOSLimiting.Checked != paymentMethod.MaximumAmountInPOSEnabled) return true;
            if (ntbMaximumAmount.Value != (double) paymentMethod.MaximumAmountInPOS) return true;
            return false;
        }

        public bool SaveData()
        {
            paymentMethod.AllowNegativePaymentAmounts = chkAllowNegativePaymentAmounts.Checked;
            paymentMethod.PaymentTypeCanBeVoided = chkPaymentTypeCanBeVoided.Checked;
            paymentMethod.CountingRequired = chkCountingRequired.Checked;
            paymentMethod.AllowSafeDrop = chkAllowSafeDrop.Checked;
            paymentMethod.AllowFloat = chkAllowFloat.Checked;
            paymentMethod.AllowBankDrop = chkAllowBankDrop.Checked;
            paymentMethod.MaximumAmountInPOSEnabled = chkAmountInPOSLimiting.Checked;
            paymentMethod.MaximumAmountInPOS = (decimal)ntbMaximumAmount.Value;
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

        private void SetFloatingTenderControls(bool enabled)
        {
            chkCountingRequired.Enabled = enabled;
            chkAllowBankDrop.Enabled = enabled;
            chkAllowFloat.Enabled = enabled;
            chkAllowSafeDrop.Enabled = enabled;
            chkAmountInPOSLimiting.Enabled = enabled;
            ntbMaximumAmount.Enabled = enabled;
        }

        private void chkAmountInPOSLimiting_CheckedChanged(object sender, EventArgs e)
        {
            lblMaximumAmount.Enabled = ntbMaximumAmount.Enabled = chkAmountInPOSLimiting.Checked;
        }

        
       
    }
}
