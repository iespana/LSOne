using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class FuncProfileInfocodeTabPage : UserControl, ITabView
    {
        
        private FunctionalityProfile functionalityProfile;

        public FuncProfileInfocodeTabPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.FuncProfileInfocodeTabPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (internalContext is FunctionalityProfile)
            {
                functionalityProfile = (FunctionalityProfile)internalContext;

                Infocode infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeStartOfTransaction);
                cmbFuncStartTrans.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkStartTrans.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeEndOfTransaction);
                cmbFuncEndTrans.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkEndTrans.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeTenderDecl);
                cmbFuncTenderDecl.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkTenderDecl.Checked = infocode != null;
                
                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeItemNotFound);
                cmbFuncItemNotFound.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkItemNotFound.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeItemDiscount);
                cmbFuncItemDisc.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkItemDisc.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeTotalDiscount);
                cmbFuncTotalDisc.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkTotalDisc.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodePriceOverride);
                cmbFuncPriceOverride.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkPriceOver.Checked = infocode != null;
                
                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeReturnItem);
                cmbFuncReturnItem.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkReturnItem.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeReturnTransaction);
                cmbFuncReturnTrans.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkReturnTrans.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeVoidItem);
                cmbFuncVoidItem.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkVoidItem.Checked = infocode != null;
                
                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeVoidPayment);
                cmbFuncVoidPayment.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkVoidPayment.Checked = infocode != null;
                
                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeVoidTransaction);
                cmbFuncVoidTransaction.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkVoidTrans.Checked = infocode != null;
                
                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeAddSalesPerson);
                cmbFuncAddSalesp.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkAddSalesPerson.Checked = infocode != null;

                infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, functionalityProfile.InfocodeOpenDrawer);
                cmbFuncOpenDrawer.SelectedData = infocode != null ? new DataEntity(infocode.ID, infocode.Text) : new DataEntity();
                chkOpenDrawer.Checked = infocode != null;
                
                cmbStartOfTransactionType.SelectedIndex = (int)functionalityProfile.InfocodeStartOfTransactionType;
            }
            SetCheckEnable();
        
        }

        public bool DataIsModified()
        {
            if (cmbFuncStartTrans.Text != functionalityProfile.InfocodeStartOfTransaction) return true;
            if (cmbFuncEndTrans.Text != functionalityProfile.InfocodeEndOfTransaction) return true;
            if (cmbFuncTenderDecl.Text != functionalityProfile.InfocodeTenderDecl) return true;
            if (cmbFuncItemNotFound.Text != functionalityProfile.InfocodeItemNotFound) return true;
            if (cmbFuncItemDisc.Text != functionalityProfile.InfocodeItemDiscount) return true;
            if (cmbFuncTotalDisc.Text != functionalityProfile.InfocodeTotalDiscount) return true;
            if (cmbFuncPriceOverride.Text != functionalityProfile.InfocodePriceOverride) return true;
            if (cmbFuncReturnItem.Text != functionalityProfile.InfocodeReturnItem) return true;
            if (cmbFuncReturnTrans.Text != functionalityProfile.InfocodeReturnTransaction) return true;
            if (cmbFuncVoidItem.Text != functionalityProfile.InfocodeVoidItem) return true;
            if (cmbFuncVoidPayment.Text != functionalityProfile.InfocodeVoidPayment) return true;
            if (cmbFuncVoidTransaction.Text != functionalityProfile.InfocodeVoidTransaction) return true;
            if (cmbFuncAddSalesp.Text != functionalityProfile.InfocodeAddSalesPerson) return true;
            if (cmbFuncOpenDrawer.Text != functionalityProfile.InfocodeOpenDrawer) return true;
            if (cmbStartOfTransactionType.SelectedIndex != (int) functionalityProfile.InfocodeStartOfTransactionType) return true;

            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.InfocodeStartOfTransaction = cmbFuncStartTrans.SelectedData.ID;
            functionalityProfile.InfocodeEndOfTransaction = cmbFuncEndTrans.SelectedData.ID;
            functionalityProfile.InfocodeTenderDecl = cmbFuncTenderDecl.SelectedData.ID;
            functionalityProfile.InfocodeItemNotFound = cmbFuncItemNotFound.SelectedData.ID;
            functionalityProfile.InfocodeItemDiscount = cmbFuncItemDisc.SelectedData.ID;
            functionalityProfile.InfocodeTotalDiscount = cmbFuncTotalDisc.SelectedData.ID;
            functionalityProfile.InfocodePriceOverride = cmbFuncPriceOverride.SelectedData.ID;
            functionalityProfile.InfocodeReturnItem = cmbFuncReturnItem.SelectedData.ID;
            functionalityProfile.InfocodeReturnTransaction = cmbFuncReturnTrans.SelectedData.ID;
            functionalityProfile.InfocodeVoidItem = cmbFuncVoidItem.SelectedData.ID;
            functionalityProfile.InfocodeVoidPayment = cmbFuncVoidPayment.SelectedData.ID;
            functionalityProfile.InfocodeVoidTransaction = cmbFuncVoidTransaction.SelectedData.ID;
            functionalityProfile.InfocodeAddSalesPerson = cmbFuncAddSalesp.SelectedData.ID;
            functionalityProfile.InfocodeOpenDrawer = cmbFuncOpenDrawer.SelectedData.ID;
            functionalityProfile.InfocodeStartOfTransactionType = (FunctionalityProfile.StartOfTransactionTypes) cmbStartOfTransactionType.SelectedIndex;
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

        private void cmbFuncTenderDecl_RequestData(object sender, EventArgs e)
        {
            
            (sender as DualDataComboBox).SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new InputTypesEnum[] { InputTypesEnum.Group }, false, InfocodeSorting.InfocodeDescription, false, RefTableEnum.All), null);
        }

        private void cmbFuncTenderDecl_RequestDateExcludingItemAndCust(object sender,EventArgs e)
        {
            InputTypesEnum[] inputTypesEnumToExclude = new InputTypesEnum[3];
            inputTypesEnumToExclude[0] = InputTypesEnum.Customer;
            inputTypesEnumToExclude[1] = InputTypesEnum.Item;
            inputTypesEnumToExclude[2] = InputTypesEnum.Group;

            (sender as DualDataComboBox).SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, inputTypesEnumToExclude, false, InfocodeSorting.InfocodeDescription, false, RefTableEnum.All), null);
        }

        private void chkTenderDecl_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckEnable();
        }

        private void SetCheckEnable()
        {
            cmbFuncStartTrans.Enabled = false;
            cmbFuncEndTrans.Enabled = false;
            cmbFuncAddSalesp.Enabled = false;
            cmbFuncItemDisc.Enabled = false;
            cmbFuncItemNotFound.Enabled = false;
            cmbFuncPriceOverride.Enabled = false;
            cmbFuncReturnItem.Enabled = false;
            cmbFuncReturnTrans.Enabled = false;
            cmbFuncTenderDecl.Enabled = false;
            cmbFuncTotalDisc.Enabled = false;
            cmbFuncVoidItem.Enabled = false;
            cmbFuncVoidPayment.Enabled = false;
            cmbFuncVoidTransaction.Enabled = false;
            cmbFuncOpenDrawer.Enabled = false;

            if (chkStartTrans.Checked)
                cmbFuncStartTrans.Enabled = true;
            else
                cmbFuncStartTrans.SelectedData = new DataEntity();

            if (chkEndTrans.Checked)
                cmbFuncEndTrans.Enabled = true;
            else
                cmbFuncEndTrans.SelectedData = new DataEntity();

            if (chkAddSalesPerson.Checked)
                cmbFuncAddSalesp.Enabled = true;
            else
                cmbFuncAddSalesp.SelectedData = new DataEntity();

            if (chkItemDisc.Checked)
                cmbFuncItemDisc.Enabled = true;
            else
                cmbFuncItemDisc.SelectedData = new DataEntity();

            if (chkItemNotFound.Checked)
                cmbFuncItemNotFound.Enabled = true;
            else
                cmbFuncItemNotFound.SelectedData = new DataEntity();

            if (chkPriceOver.Checked)
                cmbFuncPriceOverride.Enabled = true;
            else
                cmbFuncPriceOverride.SelectedData = new DataEntity();

            if (chkReturnItem.Checked)
                cmbFuncReturnItem.Enabled = true;
            else
                cmbFuncReturnItem.SelectedData = new DataEntity();

            if (chkReturnTrans.Checked)
                cmbFuncReturnTrans.Enabled = true;
            else
                cmbFuncReturnTrans.SelectedData = new DataEntity();

            if (chkTenderDecl.Checked)
                cmbFuncTenderDecl.Enabled = true;
            else
                cmbFuncTenderDecl.SelectedData = new DataEntity();

            if (chkTotalDisc.Checked)
                cmbFuncTotalDisc.Enabled = true;
            else
                cmbFuncTotalDisc.SelectedData = new DataEntity();

            if (chkVoidItem.Checked)
                cmbFuncVoidItem.Enabled = true;
            else
                cmbFuncVoidItem.SelectedData = new DataEntity();

            if (chkVoidPayment.Checked)
                cmbFuncVoidPayment.Enabled = true;
            else
                cmbFuncVoidPayment.SelectedData = new DataEntity();

            if (chkVoidTrans.Checked)
                cmbFuncVoidTransaction.Enabled = true;
            else
                cmbFuncVoidTransaction.SelectedData = new DataEntity();

            if (chkOpenDrawer.Checked)
                cmbFuncOpenDrawer.Enabled = true;
            else
                cmbFuncOpenDrawer.SelectedData = new DataEntity();
        }
    }
}
