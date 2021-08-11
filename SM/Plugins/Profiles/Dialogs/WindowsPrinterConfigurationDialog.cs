using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class WindowsPrinterConfigurationDialog : DialogBase
    {
        private WindowsPrinterConfiguration configuration;
        private bool folderLocationErrorDisplayed = false;

        public WindowsPrinterConfigurationDialog(RecordIdentifier configID)
        {
            InitializeComponent();
            configuration = Providers.WindowsPrinterConfigurationData.Get(PluginEntry.DataModel, configID) ?? new WindowsPrinterConfiguration();
            Populate();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PrinterConfigurationID
        {
            get
            {
                return configuration == null ? RecordIdentifier.Empty : configuration.ID;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled =
            !string.IsNullOrWhiteSpace(tbDescription.Text) &&
            !string.IsNullOrEmpty(cmbPrinter.Text) &&
            (cmbPrinter.Text != configuration.PrinterDeviceName ||
            tbDescription.Text != configuration.Text ||
            ntbLeftMargin.Value != configuration.LeftMargin ||
            ntbRightMargin.Value != configuration.RightMargin ||
            ntbTopMargin.Value != configuration.TopMargin ||
            ntbBottomMargin.Value != configuration.BottomMargin ||
            tbFont.Text != configuration.FontName ||
            ntbFontSize.Value != (double)configuration.FontSize ||
            ntbWideHighFontSize.Value != (double)configuration.WideHighFontSize ||
            chkPrintDesignBoxes.Checked != configuration.PrintDesignBoxes ||
            tbFolderLocation.Text != configuration.FolderLocation);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            configuration.Text = tbDescription.Text;
            configuration.PrinterDeviceName = cmbPrinter.Text;
            configuration.LeftMargin = (int)ntbLeftMargin.Value;
            configuration.RightMargin = (int)ntbRightMargin.Value;
            configuration.TopMargin = (int)ntbTopMargin.Value;
            configuration.BottomMargin = (int)ntbBottomMargin.Value;
            configuration.FontName = tbFont.Text;
            configuration.FontSize = (decimal)ntbFontSize.Value;
            configuration.WideHighFontSize = (decimal)ntbWideHighFontSize.Value;
            configuration.PrintDesignBoxes = chkPrintDesignBoxes.Checked;
            configuration.FolderLocation = tbFolderLocation.Text;

            if (string.IsNullOrEmpty(configuration.FolderLocation))
            {
                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoFolderLocationSelected);
            }

            Providers.WindowsPrinterConfigurationData.Save(PluginEntry.DataModel, configuration);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = string.IsNullOrEmpty(tbFolderLocation.Text) ? string.Empty : tbFolderLocation.Text;
            dlg.Description = Properties.Resources.SelectFolderLocation;
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                folderLocationErrorProvider.Clear();
                folderLocationErrorDisplayed = false;
                tbFolderLocation.Text = dlg.SelectedPath;
            }
        }

        private void CheckFolderAndFileInformation(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFolderLocation.Text))
            {
                if (tbFolderLocation.Text.Substring(tbFolderLocation.TextLength - 1, 1) != @"\")
                {
                    tbFolderLocation.Text += @"\";
                }

                if (!Directory.Exists(tbFolderLocation.Text))
                {
                    if (!folderLocationErrorDisplayed)
                    {
                        MessageDialog.Show(Properties.Resources.TheFolderSelectedDoesNotExist.Replace("{0}", tbFolderLocation.Text), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbFolderLocation.Focus();
                    }

                    folderLocationErrorProvider.SetError(btnSelectFolder, Properties.Resources.TheFolderSelectedDoesNotExist.Replace("{0}", tbFolderLocation.Text));
                    folderLocationErrorDisplayed = true;
                }
            }
        }

        private void tbFolderLocation_TextChanged(object sender, EventArgs e)
        {
            folderLocationErrorProvider.Clear();
            folderLocationErrorDisplayed = false;
            CheckEnabled(sender, e);
        }

        private void btnEditFontName_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.AllowScriptChange = false;
            fontDialog.AllowSimulations = false;
            fontDialog.ShowApply = false;
            fontDialog.ShowColor = false;
            fontDialog.ShowEffects = false;
            fontDialog.ShowHelp = false;
            fontDialog.Font = new Font(tbFont.Text, (float)ntbFontSize.Value, FontStyle.Regular);

            if(fontDialog.ShowDialog() == DialogResult.OK)
            {
                tbFont.Text = fontDialog.Font.Name;
                ntbFontSize.Value = fontDialog.Font.Size;
            }
        }

        private void Populate()
        {
            cmbPrinter.Items.AddRange(PrinterSettings.InstalledPrinters.Cast<string>().ToArray());

            tbDescription.Text = configuration.Text;
            cmbPrinter.Text = configuration.PrinterDeviceName;
            ntbLeftMargin.Value = configuration.LeftMargin;
            ntbRightMargin.Value = configuration.RightMargin;
            ntbTopMargin.Value = configuration.TopMargin;
            ntbBottomMargin.Value = configuration.BottomMargin;
            tbFolderLocation.Text = configuration.FolderLocation;
            tbFont.Text = configuration.FontName;
            ntbFontSize.Value = (double)configuration.FontSize;
            ntbWideHighFontSize.Value = (double)configuration.WideHighFontSize;
            chkPrintDesignBoxes.Checked = configuration.PrintDesignBoxes;
        }
    }
}
