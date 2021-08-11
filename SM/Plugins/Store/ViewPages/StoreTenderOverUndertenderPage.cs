using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

//using LSRetail.Utilities.Locale;


namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class StoreTenderOverUndertenderPage : UserControl, ITabView
    {
        RecordIdentifier storeAndPaymentMethodID;
        StorePaymentMethod paymentMethod;

        public StoreTenderOverUndertenderPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreTenderOverUndertenderPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            storeAndPaymentMethodID = context;
            paymentMethod = (StorePaymentMethod)internalContext;

            chkPaymentTypeCanBePartOfSplitPayment.Checked = paymentMethod.PaymentTypeCanBePartOfSplitPayment;
            chkOverTenderAllowed.Checked = paymentMethod.AllowOverTender;
            ntbMaxOverTender.Value = (double)paymentMethod.MaximumOverTenderAmount;
            chkUnderTenderAllowed.Checked = paymentMethod.AllowUnderTender;
            ntbUndertenderAmount.Value = (double)paymentMethod.UnderTenderAmount;
            
        }

        public bool DataIsModified()
        {
            if (chkOverTenderAllowed.Checked != paymentMethod.AllowOverTender) return true;
            if (ntbMaxOverTender.Value != (double)paymentMethod.MaximumOverTenderAmount) return true;
            if (chkUnderTenderAllowed.Checked != paymentMethod.AllowUnderTender) return true;
            if (ntbUndertenderAmount.Value != (double)paymentMethod.UnderTenderAmount) return true;
            if (chkPaymentTypeCanBePartOfSplitPayment.Checked != paymentMethod.PaymentTypeCanBePartOfSplitPayment) return true;

            return false;
        }

        public bool SaveData()
        {
            paymentMethod.AllowOverTender = chkOverTenderAllowed.Checked;
            paymentMethod.MaximumOverTenderAmount = (decimal)ntbMaxOverTender.Value;
            paymentMethod.AllowUnderTender = chkUnderTenderAllowed.Checked;
            paymentMethod.UnderTenderAmount = (decimal)ntbUndertenderAmount.Value;
            paymentMethod.PaymentTypeCanBePartOfSplitPayment = chkPaymentTypeCanBePartOfSplitPayment.Checked;

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

        private void chkOverTenderAllowed_CheckedChanged(object sender, EventArgs e)
        {
            lblMaxOverTender.Enabled =  ntbMaxOverTender.Enabled = chkOverTenderAllowed.Checked;
        }

        private void chkUnderTenderAllowed_CheckedChanged(object sender, EventArgs e)
        {
            lblUndertenderAmount.Enabled = ntbUndertenderAmount.Enabled = chkUnderTenderAllowed.Checked;
        }

        private void chkPaymentTypeCanBePartOfSplitPayment_CheckedChanged(object sender, EventArgs e)
        {

            if (!chkPaymentTypeCanBePartOfSplitPayment.Checked)
            {
                chkOverTenderAllowed.Checked = false;
                chkUnderTenderAllowed.Checked = false;
            }

            chkOverTenderAllowed.Enabled = chkPaymentTypeCanBePartOfSplitPayment.Checked;
            chkUnderTenderAllowed.Enabled = chkPaymentTypeCanBePartOfSplitPayment.Checked;
        }
    }
}
