using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.Utilities.IO;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class ImportImagesDialog : DialogBase
    {
        private class ImageFile
        {
            public string ItemId { get; set; }
            public string File { get; set; }
        }
        
        private readonly IConnectionManager manager;
        private List<string> errorFiles;
        private List<string> skippedFiles;
        private readonly List<int> compression;
        private bool importing;
        private bool cancelImport;

        public ImportImagesDialog()
        {
            InitializeComponent();

            btnImport.Enabled = false;
            progress.Enabled = false;
            cmbImportImages.SelectedIndex = 0;

            compression = new List<int>();
            for (int i = 95; i > 5; i -= 5)
            {
                compression.Add(i);
            }

            cmbCompression.Items.Clear();
            foreach (int i in compression)
            {
                cmbCompression.Items.Add(i + "%");
            }
            cmbCompression.SelectedIndex = 0;

            EnableDisable(true);
        }

        public ImportImagesDialog(IConnectionManager manager)
            : this()
        {
            this.manager = manager;
        }

        private void OnBrowseClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog {ShowNewFolderButton = false, SelectedPath = txtFolder.Text};
            if (DialogResult.OK == fbd.ShowDialog())
            {
                txtFolder.Text = fbd.SelectedPath;
            }

            btnImport.Enabled = Directory.Exists(txtFolder.Text);
        }

        private void OnBrowseDataFileClick(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { CheckFileExists = true, Multiselect = false, FileName = txtDataFile.Text};
            if (DialogResult.OK == dlg.ShowDialog())
            {
                txtDataFile.Text = dlg.FileName;
            }

            btnImport.Enabled = File.Exists(txtDataFile.Text);
        }

        private void OnImportClick(object sender, EventArgs e)
        {
            if (importing)
            {
                cancelImport = true;
                return;
            }

            if (chkDeleteImages.Checked)
            {
                if (DialogResult.No ==
                    MessageDialog.Show(Properties.Resources.ImageImportDeleteExisting, MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question))
                {
                    return;
                }
            }

            importing = true;
            cancelImport = false;
            string importText = btnImport.Text;
            btnImport.Text = Properties.Resources.ImportImagesAbort;
            btnClose.Enabled = false;
            try
            {
                int success = 0;
                DialogResult = DialogResult.None;

                progress.Enabled = true;

                errorFiles = new List<string>();
                skippedFiles = new List<string>();

                // Get a list of files to import
                List<ImageFile> files = GetFileList(errorFiles);

                // Setup progress bar
                progress.Value = 0;
                progress.Minimum = 0;
                progress.Maximum = files.Count;
                progress.Step = 1;

                // Prepare for import task

                txtSuccess.Text = success.ToString();
                txtErrors.Text = errorFiles.Count.ToString();
                txtSkipped.Text = skippedFiles.Count.ToString();

                lnkErrorDetails.Visible = false;
                lnkSkippedDetails.Visible = false;

                Dictionary<string, int> seenItems = new Dictionary<string, int>();

                // Loop through files and import
                foreach (var importFile in files)
                {
                    if (cancelImport)
                    {
                        break;
                    }
                    progress.PerformStep();

                    try
                    {
                        Image img = Image.FromFile(importFile.File);

                        // Set maximum size / compression as needed
                        if (chkSetMax.Checked)
                        {
                            if (chkCompress.Checked)
                            {
                                img = ImageUtils.CompressAndResizeImage(img, compression[cmbCompression.SelectedIndex], (int)maxPixelSize.Value);
                            }
                            else
                            {
                                img = ImageUtils.ResizeImage(img, (int)maxPixelSize.Value);
                            }
                        }
                        else if (chkCompress.Checked)
                        {
                            img = ImageUtils.CompressAndResizeImage(img, compression[cmbCompression.SelectedIndex],
                                                                    Math.Max(img.Width, img.Height));
                        }

                        // Save the image
                        RecordIdentifier id = new RecordIdentifier(importFile.ItemId);
                        if (Providers.RetailItemData.Exists(PluginEntry.DataModel, id))
                        {
                            if (!seenItems.ContainsKey((string)id))
                            {
                                seenItems[(string)id] = 1;

                                // First time we see this item
                                if (chkDeleteImages.Checked)
                                {
                                    Providers.RetailItemData.DeleteImages(PluginEntry.DataModel, id);
                                }
                            }

                            if (chkDeleteImages.Checked)
                            {
                                int index = seenItems[(string)id];
                                Providers.RetailItemData.SaveImage(PluginEntry.DataModel, new ItemImage { ID = id, Image = img, ImageIndex = index });
                                seenItems[(string)id] = index + 1;
                            }
                            else
                            {
                                // Append to existing
                                Providers.RetailItemData.SaveImage(manager, RecordIdentifier.Empty, id, img, -1);
                            }
                            success++;
                        }
                        else
                        {
                            skippedFiles.Add(String.Format(Properties.Resources.ImportImagesItemNotFound, importFile.ItemId));
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            // Throws an exception if the file is not a known image file
                            ImageUtils.DeduceImageFormat(importFile.File);
                            errorFiles.Add(importFile.File + ":" + ex.Message);
                        }
                        catch
                        {
                        }
                    }

                    txtSuccess.Text = success.ToString();
                    txtErrors.Text = errorFiles.Count.ToString();
                    txtSkipped.Text = skippedFiles.Count.ToString();

                    Application.DoEvents();
                }

                if (errorFiles.Count > 0)
                {
                    lnkErrorDetails.Visible = true;
                }

                if (skippedFiles.Count > 0)
                {
                    lnkSkippedDetails.Visible = true;
                }

                progress.Enabled = false;
                if (!cancelImport)
                {
                    progress.Value = 0;
                }
            }
            finally
            {
                btnImport.Text = importText;
                btnClose.Enabled = true;

                importing = false;
            }
        }

        private List<ImageFile> GetFileList(List<string> errorFiles)
        {
            List<ImageFile> list = new List<ImageFile>();
            if (rbFolder.Checked)
            {
                int fileSelection = cmbImportImages.SelectedIndex;
                foreach (string file in Directory.GetFiles(txtFolder.Text))
                {
                    bool import = true;
                    switch (fileSelection)
                    {
                        case 1: // PNG files
                            import = Path.GetExtension(file).Equals(".png", StringComparison.CurrentCultureIgnoreCase);
                            break;
                        case 2: // JPG files
                            import = Path.GetExtension(file).Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                                     Path.GetExtension(file).Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase);
                            break;
                        case 0:
                            break;
                    }

                    // This assumes that the filename is the item id
                    if (import)
                    {
                        string itemId = "";
                        try
                        {
                            itemId = Path.GetFileNameWithoutExtension(file);

                            var imageFile = new ImageFile {ItemId = itemId, File = file};
                            list.Add(imageFile);
                        }
                        catch (Exception ex)
                        {
                            errorFiles.Add(itemId + ":" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(txtDataFile.Text, true))
                {
                    int lineNum = 0;
                    bool canContinue = true;
                    while (!sr.EndOfStream && canContinue)
                    {
                        lineNum++;
                        string line = sr.ReadLine();
                        if (line == null || line.Trim().Length == 0)
                            continue;

                        string[] items = line.Split(';');
                        if (items.Length != 2)
                        {
                            string errorMessage = Properties.Resources.ImportImagesDataFileFormatError + " " + Properties.Resources.ImportDataFileFormatDescription;
                            MessageDialog.ShowEx(errorMessage);
                            errorFiles.Add(string.Format("{0} [{1}]: {2}", txtDataFile.Text, lineNum, errorMessage));
                            canContinue = false;
                            break;
                        }
                        
                        // File name is in the second field, item id is the first field
                        var file = TrimWhitespace(items[1]);
                        try
                        {
                            if (!Path.IsPathRooted(file))
                            {
                                file = Path.Combine(Path.GetDirectoryName(TrimWhitespace(txtDataFile.Text)), file);
                            }

                            ImageFile imageFile = new ImageFile {File = file, ItemId = items[0]};
                            list.Add(imageFile);
                        }
                        catch (Exception ex)
                        {
                            errorFiles.Add(items[0] + ": " + ex.Message);
                        }
                    }
                }
            }
            return list;
        }

        private static string TrimWhitespace(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return file;
            }

            int i = 0;
            while (char.IsWhiteSpace(file[i]))
            {
                i++;
            }

            if (i > 0)
            {
                file = file.Substring(i);
            }

            i = file.Length - 1;
            if (i > 0)
            {
                while (char.IsWhiteSpace(file[i]))
                    i--;
            }

            if (i > 0)
            {
                file = file.Substring(0, i + 1);
            }

            return file;
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnLinkErrorDetailsClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowDetails(errorFiles);
        }

        private void OnLinkSkippedClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowDetails(skippedFiles);
        }

        private static void ShowDetails(IEnumerable<string> errors)
        {
            FolderItem file = FolderItem.GetTempFile(".txt");
            using (var sw = new StreamWriter(file.AbsolutePath, false, System.Text.Encoding.UTF8))
            {
                foreach (string error in errors)
                {
                    sw.WriteLine(error);
                }
            }
            System.Diagnostics.Process.Start(file.AbsolutePath);
        }

        private void OnCheckMaximumSizeChanged(object sender, EventArgs e)
        {
            maxPixelSize.Enabled = chkSetMax.Checked;
        }

        private void OnCheckCompressChanged(object sender, EventArgs e)
        {
            cmbCompression.Enabled = chkCompress.Checked;
        }

        private void OnFileFolderChecked(object sender, EventArgs e)
        {
            rbFolder.TabStop = true;
            rbDataFile.TabStop = true;

            EnableDisable(true);
        }

        private void OnDataFileChecked(object sender, EventArgs e)
        {
            rbFolder.TabStop = true;
            rbDataFile.TabStop = true;

            EnableDisable(false);
        }

        private void EnableDisable(bool enableFolder)
        {
            btnBrowse.Enabled = enableFolder;
            cmbImportImages.Enabled = enableFolder;

            btnBrowseDataFile.Enabled = !enableFolder;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }
    }
}
