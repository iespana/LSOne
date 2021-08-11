using LSRetail.DD.Common.Data.Versioning;
using System.Windows.Forms;
using LSRetail.SiteManager.Plugins.Scheduler.Dialogs;

namespace LSRetail.SiteManager.Plugins.Scheduler.Versioning
{
    public static class DatabaseVersioning
    {
        public enum UpdateStatus
        {
            NoUpdates,
            Updated,
            Invalid,
        }

        public static UpdateStatus CheckAndPerformUpdates(Updater updater, IWin32Window ownerWindow, string messageBoxHeader)
        {
            var updateType = updater.CheckUpdate();

            if (updateType == LSRetail.DD.Common.Data.Versioning.UpdateType.None)
            {
                return UpdateStatus.NoUpdates;
            }

            if (updateType == LSRetail.DD.Common.Data.Versioning.UpdateType.Downgrade)
            {
                string msg = Properties.Resources.PluginOutdated;
                MessageBox.Show(ownerWindow, msg, messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return UpdateStatus.Invalid;
            }

            if (updateType == LSRetail.DD.Common.Data.Versioning.UpdateType.Major)
            {
                if (MessageBox.Show(ownerWindow, Properties.Resources.DatabaseOutdatedMajor, messageBoxHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes)
                {
                    return UpdateStatus.Invalid;
                }
            }

            using (UpdaterDialog dialog = new UpdaterDialog())
            {
                var dialogResult = dialog.ShowDialog(ownerWindow, updater);
                if (dialogResult == DialogResult.OK)
                {
                    return UpdateStatus.Updated;
                }
                else
                {
                    return UpdateStatus.Invalid;
                }
            }
        }
    }
}
