using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    internal partial class TerminalEODSettingsPage : UserControl, ITabView
    {
        private Terminal terminal;

        public TerminalEODSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalEODSettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;

            chkIncludeTerminal.Checked = terminal.IncludeTerminalInStatement;
            cmbStatementPosting.SelectedIndex = (int)terminal.AllowTerminalStatementPosting;
        }

        public bool DataIsModified()
        {
            if (chkIncludeTerminal.Checked != terminal.IncludeTerminalInStatement) { return true; }
            if (cmbStatementPosting.SelectedIndex != (int)terminal.AllowTerminalStatementPosting) { return true; }

            return false;
        }

        public bool SaveData()
        {
            terminal.IncludeTerminalInStatement = chkIncludeTerminal.Checked;
            terminal.AllowTerminalStatementPosting = (AllowTerminalStatementPostingEnum)cmbStatementPosting.SelectedIndex;

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

        private void chkIncludeTerminal_CheckedChanged(object sender, System.EventArgs e)
        {
            cmbStatementPosting.Enabled = chkIncludeTerminal.Checked;
            lblStatementPosting.Enabled = chkIncludeTerminal.Checked;
        }
    }
}
