using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using Form = LSOne.DataLayer.BusinessObjects.Forms.Form;

namespace LSOne.ViewPlugins.Forms.Dialogs
{
    public partial class NewFormDialog : DialogBase
    {
        public NewFormDialog()
        {
            InitializeComponent();

            cmbCopyFrom.SelectedData = new DataEntity("","");

            var values = Enum.GetValues(typeof(PrintBehaviors));
            foreach (PrintBehaviors value in values)
            {
                cmbPrintBehavior.Items.Add(GetLocalizedPrintBehavior(value));
            }
            cmbPrintBehavior.SelectedIndex = 0;
        }

        public NewFormDialog(RecordIdentifier typeID)
            :this()
        {
            cmbFormType.SelectedData = Providers.FormTypeData.Get(PluginEntry.DataModel, typeID);
        }

        public NewFormDialog(RecordIdentifier typeID, RecordIdentifier formID)
            :this()
        {
            cmbFormType.SelectedData = Providers.FormTypeData.Get(PluginEntry.DataModel, typeID);

            var selectedForm = Providers.FormData.Get(PluginEntry.DataModel, formID);
            tbDescription.Text = selectedForm.Text;
            chkIsSlip.Checked = selectedForm.PrintAsSlip;
            cmbPrintBehavior.SelectedIndex = (int)selectedForm.PrintBehavior;
            tbPromptText.Text = selectedForm.PromptText;
            ntbFormWidth.Value = selectedForm.DefaultFormWidth;

            cmbCopyFrom.Visible = false;
            lblCopyFrom.Visible = false;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tbDescription.Focus();
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
            bool copy = cmbCopyFrom.SelectedData.ID != "";
            Form form = copy ? (Form)cmbCopyFrom.SelectedData : new Form();               
               
            form.FormTypeID = cmbFormType.SelectedData.ID;
            form.Text = tbDescription.Text;

            if (!copy)
            {
                form.PrintAsSlip = chkIsSlip.Checked;
                form.PrintBehavior = (PrintBehaviors)cmbPrintBehavior.SelectedIndex;
                form.PromptQuestion = (PrintBehaviors)cmbPrintBehavior.SelectedIndex == PrintBehaviors.PromptUser;
                form.PromptText = tbPromptText.Text;
                form.DefaultFormWidth = FormWidth;
            }
            else
            {
                form.ID = RecordIdentifier.Empty;
            }

            Providers.FormData.Save(PluginEntry.DataModel,form);
            FormID = (string)form.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        public RecordIdentifier FormID { get; private set; }

        public int FormWidth
        {
            get
            {
                return ntbFormWidth.Value != 0 ? Convert.ToInt32(ntbFormWidth.Value) : 56;
            }
        }   

        private void cmbFormType_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void cmbFormType_RequestData(object sender, EventArgs e)
        {
            cmbFormType.SetData(Providers.FormTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = cmbFormType.SelectedData != null && tbDescription.Text.Length > 0;
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            bool hasSelection = cmbCopyFrom.SelectedData.ID != "";

            chkIsSlip.Enabled = cmbPrintBehavior.Enabled = tbPromptText.Enabled = 
                ntbFormWidth.Enabled = !hasSelection;

            if (!hasSelection) return;
            var selectedForm = ((Form) cmbCopyFrom.SelectedData);
            chkIsSlip.Checked = selectedForm.PrintAsSlip;
            cmbPrintBehavior.SelectedIndex = (int) selectedForm.PrintBehavior;
            tbPromptText.Text = selectedForm.PromptText;
            ntbFormWidth.Value = selectedForm.DefaultFormWidth;

        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            cmbCopyFrom.SetData(Providers.FormData.GetLists(PluginEntry.DataModel, FormSorting.Description, false), null);
        }

        private static string GetLocalizedPrintBehavior(PrintBehaviors value)
        {
            string result = "";
            switch (value)
            {
                case PrintBehaviors.AlwaysPrint:
                    result = Properties.Resources.AlwaysPrint;
                    break;
                case PrintBehaviors.NeverPrint:
                    result = Properties.Resources.NeverPrint;
                    break;
                case PrintBehaviors.PromptUser:
                    result = Properties.Resources.PromptUser;
                    break;
                /*case PrintBehaviors.ShowPreview:
                    result = Properties.Resources.ShowPreview;
                    break;*/
            }
            return result;
        }

        private void cmbPrintBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbPromptText.Enabled = (PrintBehaviors) cmbPrintBehavior.SelectedIndex == PrintBehaviors.PromptUser;
        }

        private void cmbCopyFrom_RequestClear(object sender, EventArgs e)
        {
            cmbCopyFrom.SelectedData = new DataEntity("","");
        }
    }
}
