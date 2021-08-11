using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

using System;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
	public partial class EditNumberSequenceDialog : DialogBase
	{
		private NumberSequence numberSequence;

		public EditNumberSequenceDialog(RecordIdentifier id)
			:this()
		{
			numberSequence = Providers.NumberSequenceData.Get(PluginEntry.DataModel, id);

			tbID.Text = (string)numberSequence.ID;
			tbDescription.Text = numberSequence.Text;
			ntbHighest.Value = (double)numberSequence.Highest;
			ntbNextValue.Value = (double) numberSequence.NextRecord;
			tbFormat.Text = numberSequence.Format;

			chkEmbedStoreID.Checked = numberSequence.EmbedStoreID;
			chkEmbedStoreID.Enabled = numberSequence.CanBeDeleted;
			chkEmbedTerminalID.Checked = numberSequence.EmbedTerminalID;
			chkEmbedTerminalID.Enabled = numberSequence.CanBeDeleted;
		}

		public EditNumberSequenceDialog()
		{
			InitializeComponent();
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
			if (!tbFormat.Text.Contains("#"))
			{
				errorProvider1.SetError(tbFormat, Properties.Resources.FormatMustContainHash);
				DialogResult = DialogResult.None;
				return;
			}

			if (tbFormat.Text.Count(c => c == '#') < ntbHighest.Text.Length)
			{
				errorProvider1.SetError(ntbHighest, Properties.Resources.HighestMustMatchFormat);
				DialogResult = DialogResult.None;
				return;
			}

			numberSequence.Text = tbDescription.Text;
			numberSequence.Highest = (int)ntbHighest.Value;
			numberSequence.NextRecord = (int) ntbNextValue.Value;
			numberSequence.Format = tbFormat.Text;
			numberSequence.EmbedStoreID = chkEmbedStoreID.Checked;
			numberSequence.EmbedTerminalID = chkEmbedTerminalID.Checked;

			Providers.NumberSequenceData.Save(PluginEntry.DataModel, numberSequence);

			DialogResult = DialogResult.OK;
			Close();
		}

		private void CheckChanged(object sender, EventArgs e)
		{
			errorProvider1.Clear();

			btnOK.Enabled = (tbDescription.Text != numberSequence.Text ||
				ntbHighest.Value != (double)numberSequence.Highest ||
				ntbNextValue.Value != (double)numberSequence.NextRecord ||
				tbFormat.Text != numberSequence.Format ||
				chkEmbedStoreID.Checked != numberSequence.EmbedStoreID ||
				chkEmbedTerminalID.Checked != numberSequence.EmbedTerminalID
				&& tbDescription.Text != "");
		}

		private void btnEnableNextValue_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == MessageBox.Show(Properties.Resources.NumberSequenceWarning,
					Properties.Resources.NumberSequenceWarningTitle,
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
			{
				ntbNextValue.Enabled = true;
				btnEnableNextValue.Enabled = false;
			}
		}
	}
}
