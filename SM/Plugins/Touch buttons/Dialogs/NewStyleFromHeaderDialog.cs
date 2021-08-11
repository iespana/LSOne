using System.Windows.Forms;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    public partial class NewStyleFromHeaderDialog  : DialogBase
    {
        public string StyleHeader;
        
        public NewStyleFromHeaderDialog()
        {
            InitializeComponent();
        }
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            StyleHeader = tbDescription.Text;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void tbDescription_TextChanged(object sender, System.EventArgs e)
        {
            if (tbDescription.TextLength > 0)
            {
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
        }
    }
}
