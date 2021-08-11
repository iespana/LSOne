using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using LSRetail.DD.Common.Data.Versioning;

namespace LSRetail.SiteManager.Plugins.Scheduler.Dialogs
{
    public partial class UpdaterDialog : LSRetail.StoreController.SharedCore.Dialogs.DialogBase
    {
        private Updater updater;
        private CultureInfo uiCultureInfo;

        public UpdaterDialog()
        {
            InitializeComponent();
            uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
        }

        public DialogResult ShowDialog(IWin32Window owner, Updater updater)
        {
            this.updater = updater;
            return ShowDialog(owner);
        }

        private void UpdaterDialog_Shown(object sender, EventArgs e)
        {
            lblStatus.Text = null;
            progressBar.Visible = true;
            backgroundWorker.RunWorkerAsync();
        }





        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = uiCultureInfo;
            this.updater.ExecutingCommand += new EventHandler<UpdaterEventArgs>(updater_ExecutingCommand);
            try
            {
                updater.Update();
            }
            finally
            {
                this.updater.ExecutingCommand -= new EventHandler<UpdaterEventArgs>(updater_ExecutingCommand);
            }
        }

        void updater_ExecutingCommand(object sender, UpdaterEventArgs e)
        {
            backgroundWorker.ReportProgress(0, e);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var updaterEventArgs = (UpdaterEventArgs)e.UserState;
            string msg = string.Format(Properties.Resources.DatabaseUpdateExecutingMsg, updaterEventArgs.ScriptName);
            lblStatus.Text = msg;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(this, e.Error.Message, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

    }
}
