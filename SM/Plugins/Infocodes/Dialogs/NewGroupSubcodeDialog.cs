using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Infocodes.Dialogs
{
    public partial class NewGroupSubcodeDialog : DialogBase
    {

        RecordIdentifier infocodeID;
        RecordIdentifier subcodeID;
        
        public NewGroupSubcodeDialog(RecordIdentifier SelectedGroupID)
        {
            infocodeID = SelectedGroupID;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SubInfocodeID
        {
            get { return subcodeID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            InfocodeSubcode subcode = new InfocodeSubcode();
            subcode.InfocodeId = infocodeID.PrimaryID;

            subcode.Text = tbDescription.Text;
            subcode.TriggerCode = new RecordIdentifier(TriggerFunctions.Infocode);

            Providers.InfocodeSubcodeData.Save(PluginEntry.DataModel, subcode);

            subcodeID = subcode.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = tbDescription.Text.Length > 0;
        }
    }
}
