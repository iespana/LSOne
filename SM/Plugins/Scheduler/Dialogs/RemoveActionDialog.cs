using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class RemoveActionDialog : DialogBase
    {
        private RemoveActionDialog()
        {
            InitializeComponent();
        }

        public ReplicationAction Action { get; set; }

        public JscTableDesign ActionTable { get; set; }


        public RemoveActionDialog(ReplicationAction action, JscTableDesign actionTable):this()
        {
            tbCounterReference.Text = action.ActionId.ToString();
            Action = action;
            ActionTable = actionTable;

        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbOlderCounters.Checked)
            {
                DialogResult proceed = MessageBox.Show(
                    "Doing this WILL remove all counters lower than and includeing the selected one, for this subjob.  This cannot be undone or reverted.  " +
                    "And might take a long time to finish. " +
                    Environment.NewLine + "Do you want to continue?",
                    "Remove Action Counters", MessageBoxButtons.YesNo);

                if (proceed == DialogResult.Yes)
                {
                    DataProviderFactory.Instance.Get<IReplicationActionData, ReplicationAction>()
                        .DeleteOlder(PluginEntry.DataModel, Action.ActionTarget, ActionTable.TableName, Action.ActionId);
                }

            }
            else
            {
                DialogResult proceed = MessageBox.Show(
                    "Doing this WILL remove the selected counter. This cannot be undone or reverted.   " +
                    Environment.NewLine + "Do you want to continue?", 
                    "Remove Action Counter", MessageBoxButtons.YesNo);

                if (proceed == DialogResult.Yes)
                {
                    DataProviderFactory.Instance.Get<IReplicationActionData, ReplicationAction>()
                        .Delete(PluginEntry.DataModel, Action.ActionId);
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void rbThisCounter_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

     

    }

}
