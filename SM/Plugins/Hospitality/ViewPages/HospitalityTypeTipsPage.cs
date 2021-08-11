using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Financials;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityTypeTipsPage : UserControl, ITabView
    {
        private HospitalityType hospitalityType;

        public HospitalityTypeTipsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityTypeTipsPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            hospitalityType = (HospitalityType)internalContext;

            tbTipsAmtLine1.Text = hospitalityType.TipsAmtLine1;
            tbTipsAmtLine2.Text = hospitalityType.TipsAmtLine2;
            tbTipsTotalLine.Text = hospitalityType.TipsTotalLine;
            // TODO: verify 
            cmbTipsIncomeAcc1.SelectedData = Providers.IncomeExpenseAccountData.Get(PluginEntry.DataModel, new RecordIdentifier(hospitalityType.TipsIncomeAcc1, hospitalityType.RestaurantID));
            cmbTipsIncomeAcc2.SelectedData = Providers.IncomeExpenseAccountData.Get(PluginEntry.DataModel, new RecordIdentifier(hospitalityType.TipsIncomeAcc2, hospitalityType.RestaurantID));
        }

        public bool DataIsModified()
        {
            if (tbTipsAmtLine1.Text != hospitalityType.TipsAmtLine1) return true;
            if (tbTipsAmtLine2.Text != hospitalityType.TipsAmtLine2) return true;
            if (tbTipsTotalLine.Text != hospitalityType.TipsTotalLine) return true;
            if (cmbTipsIncomeAcc1.SelectedData != null && cmbTipsIncomeAcc1.SelectedData.ID != hospitalityType.TipsIncomeAcc1) return true;
            if (cmbTipsIncomeAcc1.SelectedData != null && cmbTipsIncomeAcc2.SelectedData.ID != hospitalityType.TipsIncomeAcc2) return true;

            return false;
        }

        public bool SaveData()
        {
            hospitalityType.TipsAmtLine1 = tbTipsAmtLine1.Text;
            hospitalityType.TipsAmtLine2 = tbTipsAmtLine2.Text;
            hospitalityType.TipsTotalLine = tbTipsTotalLine.Text;
            hospitalityType.TipsIncomeAcc1 = cmbTipsIncomeAcc1.SelectedData != null ? cmbTipsIncomeAcc1.SelectedData.ID : "";
            hospitalityType.TipsIncomeAcc2 = cmbTipsIncomeAcc2.SelectedData != null ? cmbTipsIncomeAcc2.SelectedData.ID : "";

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

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbTipsIncomeAcc1_RequestData(object sender, EventArgs e)
        {
            cmbTipsIncomeAcc1.SetData(Providers.IncomeExpenseAccountData.GetList(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.IncomeAccount), null);
        }

        private void cmbTipsIncomeAcc2_RequestData(object sender, EventArgs e)
        {
            cmbTipsIncomeAcc2.SetData(Providers.IncomeExpenseAccountData.GetList(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.IncomeAccount), null);
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }
    }
}
