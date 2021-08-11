using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    internal partial class StoreStatementSettingsPage : UserControl, ITabView
    {
        private Store store;

        public StoreStatementSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreStatementSettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            store = (Store)internalContext;

            cmbTenderDeclarationCalculation.SelectedIndex = (int)store.TenderDeclarationCalculation;

            ntbTotal.Value = (double)store.MaximumPostingDifference;
            ntbPerStatementLine.Value = (double)store.MaximumTransactionDifference;
        }

        public bool DataIsModified()
        {
            if (cmbTenderDeclarationCalculation.SelectedIndex != (int)store.TenderDeclarationCalculation) { return true; }
            if (ntbTotal.Value != (double)store.MaximumPostingDifference) { return true; }
            if (ntbPerStatementLine.Value != (double)store.MaximumTransactionDifference) { return true; }

            return false;
        }

        public bool SaveData()
        {
            store.TenderDeclarationCalculation = (TenderDeclarationCalculation)cmbTenderDeclarationCalculation.SelectedIndex;
            store.MaximumPostingDifference = (decimal)ntbTotal.Value;
            store.MaximumTransactionDifference = (decimal)ntbPerStatementLine.Value;

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
    }
}
