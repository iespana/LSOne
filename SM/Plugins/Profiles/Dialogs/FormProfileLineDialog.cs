using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using Form = LSOne.DataLayer.BusinessObjects.Forms.Form;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class FormProfileLineDialog: DialogBase
    {
        private RecordIdentifier profileID = "";
        private RecordIdentifier formTypeID = "";
        private FormProfileLine profileLine;

        public FormProfileLineDialog()
        {
            InitializeComponent();
        }

        public FormProfileLineDialog(RecordIdentifier profileID)
            :this()
        {
            this.profileID = profileID;
        }

        public FormProfileLineDialog(RecordIdentifier profileID, RecordIdentifier formTypeID)
            :this()
        {
            this.profileID = profileID;
            this.formTypeID = formTypeID;

            this.profileLine = Providers.FormProfileLineData.GetFormProfileLine(PluginEntry.DataModel, profileID, this.formTypeID);
            cmbFormType.SelectedData = Providers.FormTypeData.Get(PluginEntry.DataModel, this.formTypeID);
            cmbFormLayout.SelectedData = Providers.FormData.Get(PluginEntry.DataModel, profileLine.FormLayoutID);
            ntbCopies.Value = profileLine.NumberOfCopies;

            cmbFormType.Enabled = profileID != FormProfile.DefaultProfileID && profileID != FormProfile.EmailProfileID;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbFormLayout_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Form form = Providers.FormData.Get(PluginEntry.DataModel, cmbFormLayout.SelectedDataID);
            
            FormProfileLine profileLine = new FormProfileLine();
            profileLine.ReceiptTypeID = cmbFormType.SelectedData.ID;
            profileLine.FormLayoutID = cmbFormLayout.SelectedDataID;
            profileLine.Text = form.Text;
            profileLine.TypeDescription = cmbFormType.Text;
            profileLine.ProfileID = profileID;
            profileLine.NumberOfCopies =  ntbCopies.Value < 1 ? 1 : (int)ntbCopies.Value;
            Providers.FormProfileLineData.Save(PluginEntry.DataModel, profileLine);

            DialogResult = DialogResult.OK;
            Close();
        }


        public RecordIdentifier ProfileID
        {
            get { return profileID; }
        }

        public RecordIdentifier FormTypeID
        {
            get { return formTypeID; }
        }

        private void cmbFormType_RequestData(object sender, EventArgs e)
        {
            cmbFormType.SetData(Providers.FormTypeData.GetUnusedFormTypes(PluginEntry.DataModel, ProfileID), null);
        }

        private void btnEditFormType_Click(object sender, EventArgs e)
        {
            if ((cmbFormType.SelectedData) != null )
            {
                formTypeID = cmbFormType.SelectedData.ID;
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanEditFormType", null);
                if (plugin != null)
                {
                    plugin.Message(null, "EditFormTypeForProfileLine", new object[] { profileID, formTypeID });
                }
            }
            FormType type = Providers.FormTypeData.Get(PluginEntry.DataModel, formTypeID);
            cmbFormType.SelectedData = type;
        }

        private void btnAddFormType_Click(object sender, EventArgs e)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanAddFormType", null);
            if (plugin != null)
            {
                plugin.Message(null, "AddFormTypeForProfileLine", profileID);
            }
        }

        private void btnEditFormLayout_Click(object sender, EventArgs e)
        {
            if (cmbFormLayout.SelectedData != null)
            {
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanEditForm", null);
                var formTypeID = cmbFormType.SelectedData != null ? cmbFormType.SelectedData.ID : RecordIdentifier.Empty;
                var formID = cmbFormLayout.SelectedData != null ? cmbFormLayout.SelectedData.ID : RecordIdentifier.Empty;
                if (plugin != null)
                {
                    plugin.Message(null, "EditFormForProfileLine", new object[] { formTypeID, formID });
                }
            }
        }

        private void btnAddFormLayout_Click(object sender, EventArgs e)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanAddForm", null);
            var formTypeID = cmbFormType.SelectedData != null ? cmbFormType.SelectedData.ID : RecordIdentifier.Empty;
            if (plugin != null)
            {
                plugin.Message(null, "AddFormForProfileLine", formTypeID);
            }
        }

        private void cmbFormType_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool exists = false;
            if (cmbFormType.SelectedData != null)
            {
                formTypeID = cmbFormType.SelectedData.ID;
                exists = Providers.FormProfileLineData.IDExists(PluginEntry.DataModel, profileID, formTypeID);
                if (exists)
                {
                    errorProvider1.SetError(btnAddFormType, Properties.Resources.OneTypePerProfile);
                }
            }
            cmbFormLayout.SelectedData = null;
        }

        private void cmbFormLayout_RequestData(object sender, EventArgs e)
        {
            if (cmbFormType.SelectedData != null)
            {
                var forms = Providers.FormData.GetFormsOfType(PluginEntry.DataModel, cmbFormType.SelectedData.ID, FormSorting.Description, false);
                cmbFormLayout.SetData(forms, null);
            }
            else
            {
                var forms = Providers.FormData.GetLists(PluginEntry.DataModel, FormSorting.Description, false);
                cmbFormLayout.SetData(forms, null);
            }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = cmbFormLayout.SelectedData != null
                         && cmbFormType.SelectedData != null
                         && (profileLine == null || cmbFormLayout.SelectedData.ID != profileLine.FormLayoutID || (int)ntbCopies.Value != profileLine.NumberOfCopies);
        }

        private void ntbCopies_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }
    }
}
