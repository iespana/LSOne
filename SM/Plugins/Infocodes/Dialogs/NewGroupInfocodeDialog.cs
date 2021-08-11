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
    public partial class NewGroupInfocodeDialog : DialogBase
    {
        RecordIdentifier infocodeID;

        public NewGroupInfocodeDialog(UsageCategoriesEnum usageCategory = UsageCategoriesEnum.None)
        {
            InitializeComponent();
            infocodeID = RecordIdentifier.Empty;
            cmbInfocodeGroupType.SelectedIndex = (int)usageCategory;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier InfocodeID
        {
            get { return infocodeID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            Infocode infocode = new Infocode();
            infocode.Text = tbDescription.Text;
            infocode.UsageCategory = (UsageCategoriesEnum)cmbInfocodeGroupType.SelectedIndex;
            infocode.InputType = InputTypesEnum.Group;

            Providers.InfocodeData.Save(PluginEntry.DataModel, infocode);

            infocodeID = infocode.ID;

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
            btnOK.Enabled =  tbDescription.Text.Length > 0;
        }
    }
}
