using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Forms.Dialogs
{
    public partial class FormTypeDialog: DialogBase
    {
        RecordIdentifier formTypeID = RecordIdentifier.Empty;

        public FormTypeDialog()
        {
            InitializeComponent();
        }

        public FormTypeDialog(RecordIdentifier formTypeID)
            : this()
        {
            this.formTypeID = formTypeID;
            FormType formType = Providers.FormTypeData.Get(PluginEntry.DataModel, formTypeID);
            tbDescription.Text = formType.Text;
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
            var formTypes = Providers.FormTypeData.GetFormTypes(PluginEntry.DataModel, FormTypeSorting.Description, false);
            if (!formTypes.Exists(x => x.Text == tbDescription.Text))
            {
                FormType formType = new FormType();
                formType.Text = tbDescription.Text;
                formType.ID = formTypeID;

                Providers.FormTypeData.Save(PluginEntry.DataModel, formType);

                formTypeID = formType.ID;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                errorProvider1.SetError(tbDescription, Properties.Resources.FormTypeExists);
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
            errorProvider1.Clear();
        }

        public RecordIdentifier FormTypeID
        {
            get { return formTypeID; }
        }
    }
}
