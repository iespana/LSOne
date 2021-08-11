using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Settings;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
    public partial class EditDecimalFormatDialog : DialogBase
    {
        DecimalSetting setting;


        public EditDecimalFormatDialog(RecordIdentifier id)
            : this()
        {
            setting = Providers.DecimalSettingsData.Get(PluginEntry.DataModel,id);

            tbID.Text = (string)setting.ID;
            tbDescription.Text = setting.Text;
            ntbMin.Value = (double)setting.Min;
            ntbMax.Value = (double)setting.Max;
        }

        public EditDecimalFormatDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ntbMin.Value > ntbMax.Value)
            {
                errorProvider1.SetError(ntbMax, Properties.Resources.MaxCannotBeLessThanMin);
                return;
            }

            setting.Text = tbDescription.Text;
            setting.Min = (int)ntbMin.Value;
            setting.Max = (int)ntbMax.Value;

            Providers.DecimalSettingsData.Save(PluginEntry.DataModel, setting);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = (tbDescription.Text != setting.Text ||
                ntbMin.Value != (double)setting.Min ||
                ntbMax.Value != (double)setting.Max)
                && tbDescription.Text != "";
        }


    }
}
