using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using Form = LSOne.DataLayer.BusinessObjects.Forms.Form;

namespace LSOne.ViewPlugins.Forms.Views
{
    public partial class FormView : ViewBase
    {
        RecordIdentifier formID;
        Form form;
        int newFormWidth = 56;
        bool viewingNewForm = false;

        public FormView(RecordIdentifier id)
            : this()
        {
            formID = id;
        }

        public FormView()
        {
            formID = RecordIdentifier.Empty;

            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close;
            
            imgClose.Image = CloseImage;
            imgDelete.Image = DeleteImage;
            imgRevert.Image = RevertImage;
            imgSave.Image = SaveImage;

            btnEditDeviceName.Visible = PluginEntry.Framework.CanRunOperation("EditWindowsPrinters");

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);

            if (ReadOnly)
            {
                lnkSave.Visible = false;
                imgSave.Visible = false;
                lnkDelete.Visible = false;
                imgDelete.Visible = false;
            }
        }

        public FormView(RecordIdentifier id, int formWidth)
            : this()
        {
            formID = id;
            reportDesigner.FormWidth = formWidth;
            newFormWidth = formWidth;
            viewingNewForm = true;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Forms", formID, Properties.Resources.Form, true));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //TODO Figure this out
            /*AddHeaderLink(
                  Properties.Resources.Forms,
                  Properties.Resources.FormsImage,
                  new ListItemEventHandler(PluginOperations.ShowFormsSheet));*/


        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Form;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.Form + ": " + tbDescription.Text.Trim() + " (" + formID.ToString() + ")";
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return formID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {

            }

            form = Providers.FormData.Get(PluginEntry.DataModel, formID);

            tbID.Text = form.ID.ToString();
            tbDescription.Text = form.Text;
            spinSlipLineCount.Value = (decimal)form.LineCountPerPage;
            chkIsSlip.Checked = form.PrintAsSlip;
            chkWindowsPrinter.Checked = form.UseWindowsPrinter;

            switch (form.PrintBehavior)
            {
                case PrintBehaviors.AlwaysPrint:
                    optAlwaysPrint.Checked = true;
                    break;
                case PrintBehaviors.NeverPrint:
                    optNotPrint.Checked = true;
                    break;
                case PrintBehaviors.PromptUser:
                    optAskCustomer.Checked = true;
                    break;
                // TODO: case PrintBehaviors.ShowPreview:
                //    break;
            }

            WindowsPrinterConfiguration winPrinterConfiguration = Providers.WindowsPrinterConfigurationData.Get(PluginEntry.DataModel, form.WindowsPrinterConfigurationID);
            cmbDeviceName.SelectedData = winPrinterConfiguration ?? new DataEntity("", "");  

            if (viewingNewForm)
            {
                reportDesigner.LoadForm(form.HeaderXml, form.LineXml, form.FooterXml, newFormWidth);
            }
            else
            {
                reportDesigner.LoadForm(form.HeaderXml, form.LineXml, form.FooterXml, form.DefaultFormWidth);                
            }

            //HeaderIcon = Properties.Resources.FormsImage;
            HeaderText = HeaderText = Description;

            // The event isn't fired if you set "false" into the Checked property, because then it isn't changing the value from it's default value
            if(!chkWindowsPrinter.Checked)
            {
                chkWindowsPrinter_CheckedChanged(this, EventArgs.Empty);
            }

            ValidateDeviceName();
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != form.Text) return true;
            if (spinSlipLineCount.Value != (decimal)form.LineCountPerPage) return true;
            if (chkIsSlip.Checked != form.PrintAsSlip) return true;
            if (chkWindowsPrinter.Checked != form.UseWindowsPrinter) return true;

            PrintBehaviors printBehaviour;
            if (optAlwaysPrint.Checked)
            {
                printBehaviour = PrintBehaviors.AlwaysPrint;
            }
            else if (optNotPrint.Checked)
            {
                printBehaviour = PrintBehaviors.NeverPrint;
            }
            else
            {
                printBehaviour = PrintBehaviors.PromptUser;
            }            
            // TODO: PrintBehaviors.ShowPreview

            if (printBehaviour != form.PrintBehavior) return true;

            if (cmbDeviceName.SelectedData.ID != form.WindowsPrinterConfigurationID) return true;            

            if (reportDesigner.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            if (String.IsNullOrWhiteSpace(tbDescription.Text))
            {
                MessageDialog.Show(Properties.Resources.EmptyDescription);
                return false;
            }

            form.Text = tbDescription.Text;
            form.LineCountPerPage = (int)spinSlipLineCount.Value;
            form.PrintAsSlip = chkIsSlip.Checked;
            form.UseWindowsPrinter = chkWindowsPrinter.Checked;
            form.DefaultFormWidth = reportDesigner.FormWidth;

            if (optAlwaysPrint.Checked)
            {
                form.PrintBehavior = PrintBehaviors.AlwaysPrint;
            }
            else if (optNotPrint.Checked)
            {
                form.PrintBehavior = PrintBehaviors.NeverPrint;
            }
            else
            {
                form.PrintBehavior = PrintBehaviors.PromptUser;
            }
            // TODO: PrintBehaviors.ShowPreview
            
            form.WindowsPrinterConfigurationID = cmbDeviceName.SelectedDataID;

            var msgs = reportDesigner.WriteFormToXml();
            if (!string.IsNullOrEmpty(msgs))
            {
                MessageBox.Show(msgs, Properties.Resources.FieldsIntersect, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            form.HeaderXml = reportDesigner.HeaderXml;
            form.LineXml = reportDesigner.LineXml;
            form.FooterXml = reportDesigner.FooterXml;

            Providers.FormData.Save(PluginEntry.DataModel, form);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Form", form.ID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "Form")
            {
                if (changeHint == DataEntityChangeType.Delete && changeIdentifier == formID)
                {
                    PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                }
            }

        }

        private void lnkRevert_LinkClicked(object sender, EventArgs e)
        {
            Revert();
        }

        private void lnkSave_LinkClicked(object sender, EventArgs e)
        {
            ManualSave();
        }

        private void lnkClose_LinkClicked(object sender, EventArgs e)
        {
            ManualClose();
        }

        private void lnkDelete_LinkClicked(object sender, EventArgs e)
        {
            ManualDelete();
        }

        private void chkWindowsPrinter_CheckedChanged(object sender, EventArgs e)
        {            
            cmbDeviceName.Enabled = btnEditDeviceName.Enabled = chkWindowsPrinter.Checked;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteForm(formID);
        }

        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                (cmbDeviceName.SelectedData == null ||
                string.IsNullOrEmpty(cmbDeviceName.SelectedData.Text)))
            {
                errorProvider1.SetError(btnEditDeviceName, Properties.Resources.NoPrinterSelected);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void CmbDeviceName_SelectedDataChanged(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void BtnEditDeviceName_Click(object sender, EventArgs e)
        {            
            PluginOperationArguments args = new PluginOperationArguments(cmbDeviceName.SelectedData.ID, null);
            PluginEntry.Framework.RunOperation("EditWindowsPrinters", this, args);
        }

        private void CmbDeviceName_RequestData(object sender, EventArgs e)
        {
            cmbDeviceName.SetData(Providers.WindowsPrinterConfigurationData.GetDataEntityList(PluginEntry.DataModel), null);
        }

        private void CmbDeviceName_RequestClear(object sender, EventArgs e)
        {
            cmbDeviceName.SelectedData = new DataEntity("", "");
        }

        private void CmbDeviceName_SelectedDataCleared(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }
    }
}
