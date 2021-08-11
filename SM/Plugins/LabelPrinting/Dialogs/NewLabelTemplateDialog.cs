using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LabelPrinting.Dialogs
{ 
    public partial class NewLabelTemplateDialog : DialogBase
    {
        private LabelTemplate.ContextEnum context;
        LabelTemplate labelTemplate;

        public NewLabelTemplateDialog()
            : base()
        {
            this.Icon = Properties.Resources.Label;
            this.context = LabelTemplate.ContextEnum.Items;
            InitializeComponent();
            SetContext();
        }

        public NewLabelTemplateDialog(LabelTemplate.ContextEnum context)
            : this()
        {
            this.context = context;
            SetContext();
        }

        private void SetContext()
        {
            tbContext.Enabled = false;
            tbContext.Text = LabelTemplate.FromContext(context);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public LabelTemplate LabelTemplate
        {
            get { return labelTemplate; }
        }

        private bool Save()
        {
            if (Providers.LabelTemplateData.Exists(PluginEntry.DataModel, context, tbName.Text))
            {
                errorProvider1.SetError(tbDescription, Properties.Resources.LabelExistsError);
                return false;
            }

            LabelTemplate labelTemplate = new LabelTemplate
                {
                    Context = context,
                    Text = tbName.Text,
                    Description = tbDescription.Text
                };

            Providers.LabelTemplateData.Save(PluginEntry.DataModel, labelTemplate);

            this.labelTemplate = labelTemplate;

            return true;
        }
        
        private void btnOK_Click(object sender, EventArgs args)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = tbName.Text.Length > 0 &&
                            tbDescription.Text.Length > 0;
        }
    }
}
