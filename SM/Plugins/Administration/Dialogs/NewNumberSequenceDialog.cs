using System;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
    public partial class NewNumberSequenceDialog : DialogBase
    {
        RecordIdentifier numberSequenceID;

        public NewNumberSequenceDialog()            
        {
            numberSequenceID = RecordIdentifier.Empty;

            InitializeComponent();
        }

        public RecordIdentifier NumberSequenceID
        {
            get
            {
                return numberSequenceID;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            var numberSequence = new NumberSequence();

            if (!tbFormat.Text.Contains("#"))
            {
                errorProvider1.SetError(tbFormat, Properties.Resources.FormatMustContainHash);
                return;
            }

            if (tbFormat.Text.Count(c => c == '#') < ntbHighest.Text.Length)
            {
                errorProvider1.SetError(ntbHighest, Properties.Resources.HighestMustMatchFormat);
                return;
            }

            numberSequence.ID = tbID.Text;
            numberSequence.Text = tbDescription.Text;
            //numberSequence.Lowest = (int)ntbLowest.Value;
            numberSequence.Highest = (int)ntbHighest.Value;
            numberSequence.Format = tbFormat.Text;
            numberSequence.EmbedStoreID = chkEmbedStoreID.Checked;
            numberSequence.EmbedTerminalID = chkEmbedTerminalID.Checked;
            numberSequence.CanBeDeleted = chkCanBeDeleted.Checked;
            //numberSequence.WrapAround = chkWraparound.Checked;
            numberSequence.NextRecord = 1;

            Providers.NumberSequenceData.Save(PluginEntry.DataModel, numberSequence);

            numberSequenceID = numberSequence.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = tbID.Text != "" &&
                tbDescription.Text != "" &&
                //ntbLowest.Value < ntbHighest.Value &&
                ntbHighest.Value > 0 &&
                tbFormat.Text != "" &&
                tbDescription.Text != "";
        }
    }
}
