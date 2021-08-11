using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    

    public partial class StoreTenderGeneralPage : UserControl, ITabView
    {
        RecordIdentifier storeAndPaymentMethodID;
        StorePaymentMethod paymentMethod;
        

        public StoreTenderGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreTenderGeneralPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            storeAndPaymentMethodID = context;
            paymentMethod = (StorePaymentMethod)internalContext;

            ntbMinAmountAllowed.Value = (double)paymentMethod.MinimumAmountAllowed;
            ntbMaxAmountAllowed.Value = (double)paymentMethod.MaximumAmountAllowed;

            if (paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.FloatTender || paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.DepositTender) 
            {
                DisableNonFloatingTenderControls(paymentMethod.DefaultFunction);
            }
            else
            {
                EnableNonFloatingTenderControls();
            }


            cmbPaymentMethodChange.SelectedData = new DataEntity(paymentMethod.ChangeTenderID, paymentMethod.ChangeTenderName);

            if (cmbPaymentMethodChange.SelectedData.ID == RecordIdentifier.Empty || cmbPaymentMethodChange.SelectedData.ID == "")
            {
                cmbPaymentMethodChange.SelectedData = new DataEntity("", "");
            }

            cmbPaymentUpperLimit.SelectedData = new DataEntity(paymentMethod.AboveMinimumTenderID, paymentMethod.AboveMinimumTenderName);

            ntbMinChangeAmount.Value = (double)paymentMethod.MinimumChangeAmount;
            cmbActions.LoadData(Providers.PosOperationData.GetPaymentOperations(PluginEntry.DataModel), paymentMethod.PosOperation.ToString());

            ntbRounding.SetValueWithLimit(paymentMethod.Rounding, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));


            // We check here to be sure in case if new rounding methods have been added later
            if ((int)paymentMethod.RoundingMethod < (cmbRoundingMethod.Items.Count))
            {
                cmbRoundingMethod.SelectedIndex = (int)paymentMethod.RoundingMethod;
            }

            chkOpenDrawer.Checked = paymentMethod.OpenDrawer;

            ntbMaxRecount.Value = paymentMethod.MaxRecount;
            ntbMaxTDDifference.Value = (double)paymentMethod.MaxCountingDifference;
            chkBlindRecount.Checked = paymentMethod.BlindRecountAllowed;
            chkPaymentMethod.Checked = paymentMethod.AnotherMethodIfAmountHigher;

            if (ntbMinChangeAmount.Value > 0 && cmbPaymentUpperLimit.SelectedData != null)
            {
                cmbPaymentUpperLimit.SelectedData = new DataEntity(paymentMethod.AboveMinimumTenderID, paymentMethod.AboveMinimumTenderName);
            }


        }

        public bool DataIsModified()
        {
            if (ntbMinAmountAllowed.Value != (double)paymentMethod.MinimumAmountAllowed) return true;
            if (ntbMaxAmountAllowed.Value != (double)paymentMethod.MaximumAmountAllowed) return true;
            if (cmbPaymentMethodChange.SelectedData.ID != paymentMethod.ChangeTenderID) return true;
            if (cmbPaymentUpperLimit.SelectedData.ID != paymentMethod.AboveMinimumTenderID) return true;
            if ((decimal)ntbMinChangeAmount.Value != paymentMethod.MinimumChangeAmount) return true;
            if ((string)cmbActions.SelectedID != paymentMethod.PosOperation.StringValue) return true;
            if ((decimal)ntbRounding.Value != paymentMethod.Rounding) return true;
            if (cmbRoundingMethod.SelectedIndex != (int)paymentMethod.RoundingMethod) return true;
            if (chkOpenDrawer.Checked != paymentMethod.OpenDrawer) return true;
            if (ntbMaxRecount.Value != paymentMethod.MaxRecount) return true;
            if (ntbMaxTDDifference.Value != (double)paymentMethod.MaxCountingDifference) return true;
            if (chkBlindRecount.Checked != paymentMethod.BlindRecountAllowed) return true;
            if (chkPaymentMethod.Checked != paymentMethod.AnotherMethodIfAmountHigher) return true;

            return false;
        }

        public bool SaveData()
        {
            paymentMethod.MinimumAmountAllowed = (decimal)ntbMinAmountAllowed.Value;
            paymentMethod.MaximumAmountAllowed = (decimal)ntbMaxAmountAllowed.Value;
            paymentMethod.ChangeTenderID = cmbPaymentMethodChange.SelectedData.ID;
            paymentMethod.AnotherMethodIfAmountHigher = chkPaymentMethod.Checked;
            paymentMethod.AboveMinimumTenderID = cmbPaymentUpperLimit.SelectedData.ID;
            paymentMethod.MinimumChangeAmount = (decimal)ntbMinChangeAmount.Value;
            paymentMethod.PosOperation = Convert.ToInt32((string)cmbActions.SelectedID);
            paymentMethod.Rounding = (decimal)ntbRounding.Value;
            paymentMethod.RoundingMethod = (StorePaymentMethod.PaymentRoundMethod) cmbRoundingMethod.SelectedIndex;
            paymentMethod.OpenDrawer = chkOpenDrawer.Checked;
            paymentMethod.MaxRecount = (int)ntbMaxRecount.Value;
            paymentMethod.MaxCountingDifference = (decimal)ntbMaxTDDifference.Value;
            paymentMethod.BlindRecountAllowed = chkBlindRecount.Checked;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PaymentMethod":
                    if (changeIdentifier == cmbPaymentMethodChange.SelectedData.ID && changeHint == DataEntityChangeType.Edit)
                    {
                        cmbPaymentMethodChange.SelectedData = new DataEntity(changeIdentifier, ((PaymentMethod)param).Text);

                    }
                break;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbPaymentMethodForChange_RequestData(object sender, EventArgs e)
        {
            // When setting up a float entry payment type the user should not 
            // be able to select the same tender type as the payment method for change since that will cause the 
            // balancing entry to cancel out the amount entered into the POS.
            if (paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.FloatTender || paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.DepositTender) 
            {
                cmbPaymentMethodChange.SetData(Providers.StorePaymentMethodData.GetReturnPaymentList(PluginEntry.DataModel, storeAndPaymentMethodID.PrimaryID).
                    Where(pm => pm.ID != storeAndPaymentMethodID.SecondaryID), imageList1.Images[0]);
            }
            else
            {
                cmbPaymentMethodChange.SetData(Providers.StorePaymentMethodData.GetReturnPaymentList(PluginEntry.DataModel, storeAndPaymentMethodID.PrimaryID),
                   imageList1.Images[0]);
            }
        }

        private void EnableNonFloatingTenderControls()
        {
            chkPaymentMethod.Enabled = true;
        }

        private void DisableNonFloatingTenderControls(PaymentMethodDefaultFunctionEnum defaultFunction)
        {
            ntbMaxRecount.Value = 0;
            ntbMaxTDDifference.Value = 0;
            chkPaymentMethod.Checked = false;

            chkPaymentMethod.Enabled = false;

            ntbMaxRecount.Enabled = false;
            ntbMaxTDDifference.Enabled = false;

            if (defaultFunction == PaymentMethodDefaultFunctionEnum.DepositTender)
            {
                chkOpenDrawer.Checked = false;
                chkOpenDrawer.Enabled = false;

                chkBlindRecount.Checked = false;
                chkBlindRecount.Enabled = false;

                cmbPaymentMethodChange.Enabled = false;
            }

        }

        private void cmbPaymentmethodupperLimit_RequestData(object sender, EventArgs e)
        {
            cmbPaymentUpperLimit.SetData(Providers.StorePaymentMethodData.GetReturnPaymentList(PluginEntry.DataModel, storeAndPaymentMethodID.PrimaryID),
               imageList1.Images[0]);
        }

        private void ClearHandler(object sender, EventArgs e)
        {
            if (paymentMethod.DefaultFunction != PaymentMethodDefaultFunctionEnum.FloatTender) 
            {
                ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            }
           
            
        }

        private void chkPaymentMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPaymentMethod.Checked)
            {
                ntbMinChangeAmount.Enabled = true;
                cmbPaymentUpperLimit.Enabled = true;
                cmbPaymentUpperLimit.SelectedData = new DataEntity("", "");

            }
            else
            {
                ntbMinChangeAmount.Enabled = false;
                cmbPaymentUpperLimit.Enabled = false;
            }

            if (!chkPaymentMethod.Checked)
            {
                ntbMinChangeAmount.Value = 0;
                cmbPaymentUpperLimit.SelectedData = new DataEntity("", "");

            }

        }

        
       
    }
}
