//using LSRetail.Utilities.Locale;

using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    public partial class TerminalCustomerDisplayPage : UserControl, ITabView
    {
        private Terminal terminal;

        public TerminalCustomerDisplayPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalCustomerDisplayPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;

            tbText1.Text = terminal.CustomerDisplayText1;
            tbText2.Text = terminal.CustomerDisplayText2;

        }

        public bool DataIsModified()
        {
            if (tbText1.Text != terminal.CustomerDisplayText1) return true;
            if (tbText2.Text != terminal.CustomerDisplayText2) return true;

            return false;
        }

        public bool SaveData()
        {
            terminal.CustomerDisplayText1 = tbText1.Text;
            terminal.CustomerDisplayText2 = tbText2.Text;

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
            // Avoid Microsoft memory leak bug in ListViews
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        
    }
}
