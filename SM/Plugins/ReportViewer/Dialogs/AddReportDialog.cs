using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.Utilities.IO;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.ReportViewer.Dialogs
{
    public partial class AddReportDialog : DialogBase
    {
        FolderItem reportDescriptionFile;
        FolderItem reportFile;
   
        public AddReportDialog() : base()
        {
            InitializeComponent();
            cmbCategory.SelectedIndex = 2; //Default generic
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           

            DialogResult = DialogResult.Cancel;
            Close();
        }

        public FolderItem ReportFile
        {
            get
            {
                return reportFile;
            }
        }

        public FolderItem ReportDescriptionFile
        {
            get
            {
                return reportDescriptionFile;
            }
        }

        public ReportCategory ReportCategory
        {
            get
            {
                return (ReportCategory)cmbCategory.SelectedIndex;
            }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = (reportFile != null && reportFile.Exists) && (reportDescriptionFile != null && reportDescriptionFile.Exists);
        }

        private void btnSelectReportDescription_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = Properties.Resources.ReportDescriptionFile + " (*.rpdsc)|*.rpdsc";

            dlg.DefaultExt = ".rpdsc";

            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != System.Windows.Forms.DialogResult.Cancel)
            {
                reportDescriptionFile = new FolderItem(dlg.FileName);

                tbReportDescription.Text = reportDescriptionFile.FullName;

                CheckEnabled();
            }
        }

        private void btnSelectReport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = Properties.Resources.ReportFile + " (*.rdlc)|*.rdlc";

            dlg.DefaultExt = ".rdlc";

            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != System.Windows.Forms.DialogResult.Cancel)
            {
                reportFile = new FolderItem(dlg.FileName);

                tbReportFile.Text = reportFile.FullName;

                CheckEnabled();
            }
        }

        

        
    }
}
