using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class ReplicationCounterDialog : DialogBase
    {
        private JscRepCounter repCounter;

        public ReplicationCounterDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, JscRepCounter repCounter)
        {
            this.repCounter = repCounter;
            return base.ShowDialog(owner);
        }


        private void ReplicationCounterDialog_Shown(object sender, EventArgs e)
        {
            ObjectToForm(repCounter);
            tbValue.Focus();
        }

        private void ObjectToForm(JscRepCounter replicationCounter)
        {
            if (replicationCounter.Job != null && replicationCounter.JscJob == null)
            {
                replicationCounter.JscJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, replicationCounter.Job);
            }
            if (replicationCounter.SubJob != null && replicationCounter.JscSubJob == null)
            {
                replicationCounter.JscSubJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJob(PluginEntry.DataModel, replicationCounter.SubJob);
            }
            if (replicationCounter.Location != null && replicationCounter.JscLocation == null)
            {
                replicationCounter.JscLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(PluginEntry.DataModel, replicationCounter.Location);
            }
            tbJob.Text = replicationCounter.JscJob.Text;
            tbSubJob.Text = replicationCounter.JscSubJob.Description;
            tbLocation.Text = replicationCounter.JscLocation.Text;
            tbValue.Text = replicationCounter.Counter.ToString();
        }


        private bool ObjectFromForm(JscRepCounter replicationCounter)
        {
            int counter;
            bool ok = GetCounterValue(out counter, true);
            if (ok)
            {
                replicationCounter.Counter = counter;
            }

            return ok;
        }


        private bool GetCounterValue(out int value, bool focusOnError)
        {
            bool result = int.TryParse(tbValue.Text, out value);
            if (!result)
            {
                errorProvider.SetError(tbValue, Properties.Resources.FieldMustBeInteger);
                if (focusOnError)
                {
                    tbValue.Focus();
                }
            }

            return result;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ObjectFromForm(repCounter))
            {
                DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, repCounter);
                DialogResult = DialogResult.OK;
                
            }
        }

    }
}
