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
    public partial class NewInfocodeSpecificGroupConnectionDialog : DialogBase
    {
        RecordIdentifier groupTrigger;
        InfocodeSubcode subcode;

        public NewInfocodeSpecificGroupConnectionDialog(RecordIdentifier groupTrigger)
        {
            InitializeComponent();
            this.groupTrigger = groupTrigger.PrimaryID;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SubcodeID
        {
            get { return subcode.ID ; }
        }

        public RecordIdentifier SelectedInfocode
        {
            get { return cmbInfocodeLink.SelectedData.ID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            subcode = new InfocodeSubcode();
            subcode.InfocodeId = groupTrigger.PrimaryID;
            subcode.TriggerCode = cmbInfocodeLink.SelectedData.ID;
            subcode.TriggerFunction = TriggerFunctions.Infocode;
            Infocode infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, subcode.TriggerCode);
            subcode.UsageCategory = infocode.UsageCategory;
            subcode.Text = infocode.Text;

            Providers.InfocodeSubcodeData.Save(PluginEntry.DataModel, subcode);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled()
        {
            errorProvider1.Clear();
            btnOK.Enabled = (cmbInfocodeLink.SelectedData.ID != RecordIdentifier.Empty);
        }

        private void cmbInfocodeLink_RequestData(object sender, EventArgs e)
        {
            cmbInfocodeLink.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new InputTypesEnum[] { InputTypesEnum.SubCodeButtons, InputTypesEnum.SubCodeList }, true, RefTableEnum.All), null);
        }

        private void cmbInfocodeLink_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }
    }
}
