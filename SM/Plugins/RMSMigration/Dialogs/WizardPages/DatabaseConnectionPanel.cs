using System;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.Cryptography;
using LSOne.DataLayer.DatabaseUtil;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages;
using LSOne.ViewPlugins.RMSMigration.Helper;

namespace LSOne.ViewPlugins.RMSMigration.Dialogs.WizardPages
{
    public partial class DatabaseConnectionPanel : UserControl, IWizardPage
    {
        WizardBase parent;

        public DatabaseConnectionPanel(WizardBase parent)
            : this()
        {
            this.parent = parent;
        }

        private DatabaseConnectionPanel()
        {
            InitializeComponent();

            cmbConnectionType.Items.Add(new DataSelector { Code = ConnectionType.NamedPipes.ToString(), Text = "Named Pipes" });
            cmbConnectionType.Items.Add(new DataSelector { Code = ConnectionType.SharedMemory.ToString(), Text = "Shared Memory" });
            cmbConnectionType.Items.Add(new DataSelector { Code = ConnectionType.TCP_IP.ToString(), Text = "TCP/IP" });

            cmbConnectionType.SelectedIndex = 1;
            xDbWinAuth.Checked = false;
            btnTestConnection.Enabled = true;
        }

        #region Properties

        private string RMSConnectionString { get; set; }

        #endregion


        #region IWizardPage Members
        public bool HasFinish
        {
            get { return false; }
        }

        public bool HasForward
        {
            get { return true; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {

        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return new StoreLookupPanel(parent, RMSConnectionString);
        }

        public void ResetControls()
        {

        }

        #endregion

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            parent.NextEnabled = TestDatabaseConnection();
        }

        private bool TestDatabaseConnection()
        {
            bool useWindowsAuthentication = xDbWinAuth.Checked;

            ConnectionType connectionType = (ConnectionType)Enum.Parse(typeof(ConnectionType), (((DataSelector)cmbConnectionType.SelectedItem).Code));

            MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
            LoginResult result = entry.TestConnection(
                tbDbServer.Text,
                useWindowsAuthentication,
                tbDbUser.Text,
                SecureStringHelper.FromString(tbDbPwd.Text),
                tbDbDatabase.Text,
                connectionType,
                "");

            if (result == LoginResult.Success)
            {
                DatabaseUtility dbUtil = new DatabaseUtility("XYZ");
                RMSConnectionString = dbUtil.CreateConnectionString(tbDbServer.Text, tbDbDatabase.Text, useWindowsAuthentication, tbDbUser.Text, tbDbPwd.Text, connectionType);

                if (CheckRMSDatabase())
                {
                    MessageDialog.Show(RMSMigration.Properties.Resources.ConnectingWorked);
                    return true;
                }
                else
                {
                    MessageDialog.Show(RMSMigration.Properties.Resources.InvalidRMSDatabase, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

            }
            else if (result == LoginResult.UserAuthenticationFailed)
            {
                MessageDialog.Show(RMSMigration.Properties.Resources.AuthenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                MessageDialog.Show(RMSMigration.Properties.Resources.ConnectingToDatabaseError, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private bool CheckRMSDatabase()
        {
            MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
            LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
            if (result != LoginResult.Success)
            {
                return false;
            }

            List<string> databaseTables = new List<string>();
            IDataReader dr = entry.Connection.ExecuteReader(Constants.GET_ALL_TABLES_SQL);
            while (dr.Read())
            {
                databaseTables.Add(dr["name"].ToString());
            }
            dr.Close();
            return databaseTables.Intersect(Constants.RMS_TABLE_IDENTIFICATION_LIST).Count() == Constants.RMS_TABLE_IDENTIFICATION_LIST.Count();
        }

        private void cmbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbConnectionType.SelectedItem != null)
            {
                btnTestConnection.Enabled = true;
            }
            else
            {
                btnTestConnection.Enabled = false;
            }
        }

        private void xDbWinAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (xDbWinAuth.Checked)
            {
                tbDbPwd.Text = string.Empty;
                tbDbUser.Text = string.Empty;
                tbDbPwd.Enabled = false;
                tbDbUser.Enabled = false;
            }
            else
            {
                tbDbPwd.Enabled = true;
                tbDbUser.Enabled = true;
            }
        }
    }
}
