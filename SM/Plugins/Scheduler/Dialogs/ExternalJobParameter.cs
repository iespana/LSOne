using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class ExternalJobParameter : DialogBase
    {

        public ExternalJobParameter()
        {
            InitializeComponent();
            }

        public DataEntity Parameter { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (Parameter != null)
            {
                tbName.Text = Parameter.ID.ToString();
                tbValue.Text = Parameter.Text;
            }
          
            base.OnLoad(e);
          

        }

        private void ExternalJobParameterDialog_Shown(object sender, EventArgs e)
        {
            tbName.Focus();
        }

        private void UpdateActions()
        {
            btnOK.Enabled = tbName.TextLength > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            // Create the new job
        
            if (!ValidateJob())
            {
                return;
            }
            if (Parameter == null)
            {
                Parameter = new DataEntity(tbName.Text, tbValue.Text);
            }
            else
            {
                Parameter.ID = tbName.Text;
                Parameter.Text = tbValue.Text;
            }
            // Save the new job


            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        
        private bool ValidateJob()
        {
            // Nothing yet
            return true;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }
    }
}
