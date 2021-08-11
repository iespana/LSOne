using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.BarCodes.Dialogs
{
    public partial class NewBarcodeSetupDialog : DialogBase
    {
        RecordIdentifier barCodeSetupID = "";

        public NewBarcodeSetupDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            BarCodeSetup barCodeSetup;

            barCodeSetup = new BarCodeSetup();

            barCodeSetup.Text = tbDescription.Text;

            Providers.BarCodeSetupData.Save(PluginEntry.DataModel, barCodeSetup);

            barCodeSetupID = (string)barCodeSetup.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        public RecordIdentifier BarCodeSetupID
        {
            get { return barCodeSetupID; }
        }
    }
}
