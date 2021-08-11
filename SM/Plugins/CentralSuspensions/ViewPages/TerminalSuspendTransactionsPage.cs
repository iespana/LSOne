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

namespace LSOne.ViewPlugins.CentralSuspensions.ViewPages
{
    public partial class TerminalSuspendTransactionsPage : UserControl, ITabView
    {
        Terminal terminal;

        public TerminalSuspendTransactionsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.TerminalSuspendTransactionsPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;
            cmbAllowEOD.SelectedIndex = EnumToIndex(terminal.SuspendedTransactionsStatementPosting);
        }

        public bool DataIsModified()
        {
            if (cmbAllowEOD.SelectedIndex != EnumToIndex(terminal.SuspendedTransactionsStatementPosting)) return true;      
            return false;
        }

        public bool SaveData()
        {
            terminal.SuspendedTransactionsStatementPosting = IndexToEnum(cmbAllowEOD.SelectedIndex);

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

        private int EnumToIndex(SuspendedTransactionsStatementPostingEnum allowEODEnum)
        {
            switch (allowEODEnum)
            {
                case SuspendedTransactionsStatementPostingEnum.StoreDefault:
                    return 0;
                case SuspendedTransactionsStatementPostingEnum.Yes:
                    return 1;
                case SuspendedTransactionsStatementPostingEnum.No:
                    return 2;
            }

            return 0;
        }

        private SuspendedTransactionsStatementPostingEnum IndexToEnum(int index)
        {
            switch (index)
            {
                case 0:
                    return SuspendedTransactionsStatementPostingEnum.StoreDefault;
                case 1:
                    return SuspendedTransactionsStatementPostingEnum.Yes;
                case 2:
                    return SuspendedTransactionsStatementPostingEnum.No;
            }

            return SuspendedTransactionsStatementPostingEnum.No;
        }
    }
}
