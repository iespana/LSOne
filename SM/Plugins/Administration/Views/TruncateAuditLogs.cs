using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Administration.DataLayer;
using LSOne.ViewPlugins.Administration.QueryResults;

namespace LSOne.ViewPlugins.Administration.Views
{
    public partial class TruncateAuditLogs : ViewBase
    {
        public TruncateAuditLogs()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar | ViewAttributes.Close | ViewAttributes.Help;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ManageAuditLogs;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                // If our sheet would be multi-instance sheet then we would return context identifier UUID here,
                // such as User.GUID that identifies that particular User. For single instance sheets we return 
                // Guid.Empty to tell the framework that there can only be one instace of this sheet, which will
                // make the framework make sure there is only one instance in the viewstack.
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.ManageAuditLogs;

            LoadSignatureLog();
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {

        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            
        }

        private void lvOldTransactions_StyleChanged(object sender, EventArgs e)
        {
            // Autosize the reason column
            lvOldTransactions.Columns[2].Width = -2;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SignatureDialog dlg = new SignatureDialog(PluginEntry.DataModel, PluginEntry.Framework, new Guid(PluginEntry.DeleteAuditLogAction));

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var result = DataProviderFactory.Instance.Get<IAuditingData, AuditLogResult>().DeleteAuditLogs(PluginEntry.DataModel, (int)tbTimeout.Value, dtpToDate.Value.Date);

                switch (result)
                {
                    case DeleteAuditLogsResult.TimeoutException:
                        MessageDialog.Show(Properties.Resources.DeleteAuditLogsTimeoutException, MessageBoxIcon.Error);
                        break;
                    case DeleteAuditLogsResult.UnknownException:
                        MessageDialog.Show(Properties.Resources.DeleteAuditLogsUnknownException, MessageBoxIcon.Error);
                        break;
                    default:
                        break;
                }

                LoadSignatureLog();
            }
        }

        private void LoadSignatureLog()
        {
            List<ListViewItem> items;

            lvOldTransactions.Items.Clear();

            items = DataProviderFactory.Instance.Get<IAdministrationData, ListViewItem>().GetSignedActions(PluginEntry.DataModel,
                new Guid(PluginEntry.DeleteAuditLogAction));

            foreach (ListViewItem item in items)
            {
                if (lvOldTransactions.Items.Count % 2 == 1)
                {
                    item.BackColor = ColorPalette.GrayLight;
                }
                else
                {
                    item.BackColor = ColorPalette.White;
                }

                lvOldTransactions.Items.Add(item);
            }

            lvOldTransactions.Columns[2].Width = -2;
        }

        private void pnlBottom_SizeChanged(object sender, EventArgs e)
        {
            lvOldTransactions.Columns[2].Width = -2;
        }

        private void TruncateAuditLogs_Load(object sender, EventArgs e)
        {
            dtpToDate.Value = DateTime.Now.Date;
        }

        protected override void OnClose()
        {
            lvOldTransactions.SmallImageList = null;

            base.OnClose();
        }

        private void tbTimeout_Leave(object sender, EventArgs e)
        {
            if (tbTimeout.Text == string.Empty)
            {
                tbTimeout.Value = 30;
            }
        }
    }
}