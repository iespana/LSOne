using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class SpecialGroupDialog : DialogBase
    {
        private DataEntity specialGroup;

        public SpecialGroupDialog(RecordIdentifier specialGroupId)
            : this()
        {
            specialGroup = Providers.SpecialGroupData.Get(PluginEntry.DataModel, specialGroupId);

            tbDescription.Text = specialGroup.Text;

            Text = Properties.Resources.EditSpecialGroup;
            Header = Properties.Resources.EditDescriptionForTheSelectedSpecialGroup;
        }

        public SpecialGroupDialog()
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

            if (specialGroup == null)
            {
                // group is null if we are creating a new group
                specialGroup = new DataEntity();
            }

            specialGroup.Text = tbDescription.Text;

            Providers.SpecialGroupData.Save(PluginEntry.DataModel, specialGroup);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            bool descriptionEmpty = (tbDescription.Text == "");
            btnOK.Enabled = !descriptionEmpty;
        }

        public RecordIdentifier GetSelectedId()
        {
            return specialGroup.ID;
        }

        public string GetSelectedText()
        {
            return specialGroup.Text;
        }
    }
}
