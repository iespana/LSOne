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
    public partial class TerminalEftPage : UserControl, ITabView
    {
        private Terminal terminal;

        public TerminalEftPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalEftPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;

            tbIPAddress.Text = terminal.IPAddress;
            tbEFTStoreID.Text = terminal.EftStoreID;
            tbEftTerminalID.Text = terminal.EftTerminalID;
            tbCustom1.Text = terminal.EftCustomField1;
            tbCustom2.Text = terminal.EftCustomField2;
        }

        public bool DataIsModified()
        {
            if (tbIPAddress.Text != terminal.IPAddress) return true;
            if (tbEftTerminalID.Text != terminal.EftTerminalID) return true;
            if (tbEFTStoreID.Text != terminal.EftStoreID) return true;
            if (tbCustom1.Text != terminal.EftCustomField1) return true;
            if (tbCustom2.Text != terminal.EftCustomField2) return true;

            return false;
        }

        public bool SaveData()
        {
            terminal.IPAddress = tbIPAddress.Text;
            terminal.EftStoreID = tbEFTStoreID.Text;
            terminal.EftTerminalID = tbEftTerminalID.Text;
            terminal.EftCustomField1 = tbCustom1.Text;
            terminal.EftCustomField2 = tbCustom2.Text;

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
