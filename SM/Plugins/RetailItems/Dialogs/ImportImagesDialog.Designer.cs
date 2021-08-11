using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class ImportImagesDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportImagesDialog));
            this.pnlBottom = new DialogBottomPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblGrpImport = new System.Windows.Forms.Label();
            this.pnglImport = new GroupPanel();
            this.btnBrowseDataFile = new System.Windows.Forms.Button();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.lblDataFile = new System.Windows.Forms.Label();
            this.rbDataFile = new System.Windows.Forms.RadioButton();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.cmbImportImages = new System.Windows.Forms.ComboBox();
            this.lblImagesToImport = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lblImageFolder = new System.Windows.Forms.Label();
            this.lblGrpImageAlteration = new System.Windows.Forms.Label();
            this.pnlAlteration = new GroupPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMaxSize = new System.Windows.Forms.Label();
            this.cmbCompression = new System.Windows.Forms.ComboBox();
            this.chkSetMax = new System.Windows.Forms.CheckBox();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.maxPixelSize = new System.Windows.Forms.NumericUpDown();
            this.lblCompress = new System.Windows.Forms.Label();
            this.lblMaxPixels = new System.Windows.Forms.Label();
            this.lblGrpProgress = new System.Windows.Forms.Label();
            this.pnlProgress = new GroupPanel();
            this.lnkSkippedDetails = new System.Windows.Forms.LinkLabel();
            this.txtSkipped = new System.Windows.Forms.TextBox();
            this.lblSkippedFiles = new System.Windows.Forms.Label();
            this.lnkErrorDetails = new System.Windows.Forms.LinkLabel();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.txtSuccess = new System.Windows.Forms.TextBox();
            this.lblFailedImports = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lblImportedImages = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDeleteImages = new System.Windows.Forms.Label();
            this.chkDeleteImages = new System.Windows.Forms.CheckBox();
            this.pnlBottom.SuspendLayout();
            this.pnglImport.SuspendLayout();
            this.pnlAlteration.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixelSize)).BeginInit();
            this.pnlProgress.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Controls.Add(this.flowLayoutPanel1);
            this.pnlBottom.Name = "pnlBottom";
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.OnImportClick);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // lblGrpImport
            // 
            resources.ApplyResources(this.lblGrpImport, "lblGrpImport");
            this.lblGrpImport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblGrpImport.Name = "lblGrpImport";
            // 
            // pnglImport
            // 
            this.pnglImport.Controls.Add(this.chkDeleteImages);
            this.pnglImport.Controls.Add(this.lblDeleteImages);
            this.pnglImport.Controls.Add(this.btnBrowseDataFile);
            this.pnglImport.Controls.Add(this.txtDataFile);
            this.pnglImport.Controls.Add(this.lblDataFile);
            this.pnglImport.Controls.Add(this.rbDataFile);
            this.pnglImport.Controls.Add(this.rbFolder);
            this.pnglImport.Controls.Add(this.cmbImportImages);
            this.pnglImport.Controls.Add(this.lblImagesToImport);
            this.pnglImport.Controls.Add(this.btnBrowse);
            this.pnglImport.Controls.Add(this.txtFolder);
            this.pnglImport.Controls.Add(this.lblImageFolder);
            resources.ApplyResources(this.pnglImport, "pnglImport");
            this.pnglImport.Name = "pnglImport";
            // 
            // btnBrowseDataFile
            // 
            resources.ApplyResources(this.btnBrowseDataFile, "btnBrowseDataFile");
            this.btnBrowseDataFile.Name = "btnBrowseDataFile";
            this.btnBrowseDataFile.UseVisualStyleBackColor = true;
            this.btnBrowseDataFile.Click += new System.EventHandler(this.OnBrowseDataFileClick);
            // 
            // txtDataFile
            // 
            resources.ApplyResources(this.txtDataFile, "txtDataFile");
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.ReadOnly = true;
            this.txtDataFile.TabStop = false;
            // 
            // lblDataFile
            // 
            this.lblDataFile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDataFile, "lblDataFile");
            this.lblDataFile.Name = "lblDataFile";
            // 
            // rbDataFile
            // 
            resources.ApplyResources(this.rbDataFile, "rbDataFile");
            this.rbDataFile.BackColor = System.Drawing.Color.Transparent;
            this.rbDataFile.Name = "rbDataFile";
            this.rbDataFile.UseVisualStyleBackColor = false;
            this.rbDataFile.CheckedChanged += new System.EventHandler(this.OnDataFileChecked);
            // 
            // rbFolder
            // 
            resources.ApplyResources(this.rbFolder, "rbFolder");
            this.rbFolder.BackColor = System.Drawing.Color.Transparent;
            this.rbFolder.Checked = true;
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.TabStop = true;
            this.rbFolder.UseVisualStyleBackColor = false;
            this.rbFolder.Click += new System.EventHandler(this.OnFileFolderChecked);
            // 
            // cmbImportImages
            // 
            this.cmbImportImages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImportImages.FormattingEnabled = true;
            this.cmbImportImages.Items.AddRange(new object[] {
            resources.GetString("cmbImportImages.Items"),
            resources.GetString("cmbImportImages.Items1"),
            resources.GetString("cmbImportImages.Items2")});
            resources.ApplyResources(this.cmbImportImages, "cmbImportImages");
            this.cmbImportImages.Name = "cmbImportImages";
            // 
            // lblImagesToImport
            // 
            this.lblImagesToImport.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblImagesToImport, "lblImagesToImport");
            this.lblImagesToImport.Name = "lblImagesToImport";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.OnBrowseClick);
            // 
            // txtFolder
            // 
            resources.ApplyResources(this.txtFolder, "txtFolder");
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.TabStop = false;
            // 
            // lblImageFolder
            // 
            this.lblImageFolder.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblImageFolder, "lblImageFolder");
            this.lblImageFolder.Name = "lblImageFolder";
            // 
            // lblGrpImageAlteration
            // 
            resources.ApplyResources(this.lblGrpImageAlteration, "lblGrpImageAlteration");
            this.lblGrpImageAlteration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblGrpImageAlteration.Name = "lblGrpImageAlteration";
            // 
            // pnlAlteration
            // 
            this.pnlAlteration.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.pnlAlteration, "pnlAlteration");
            this.pnlAlteration.Name = "pnlAlteration";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lblMaxSize, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbCompression, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkSetMax, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkCompress, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.maxPixelSize, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCompress, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMaxPixels, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // lblMaxSize
            // 
            this.lblMaxSize.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaxSize, "lblMaxSize");
            this.lblMaxSize.Name = "lblMaxSize";
            // 
            // cmbCompression
            // 
            this.cmbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbCompression, "cmbCompression");
            this.cmbCompression.FormattingEnabled = true;
            this.cmbCompression.Name = "cmbCompression";
            // 
            // chkSetMax
            // 
            resources.ApplyResources(this.chkSetMax, "chkSetMax");
            this.chkSetMax.Name = "chkSetMax";
            this.chkSetMax.UseVisualStyleBackColor = true;
            this.chkSetMax.CheckedChanged += new System.EventHandler(this.OnCheckMaximumSizeChanged);
            // 
            // chkCompress
            // 
            resources.ApplyResources(this.chkCompress, "chkCompress");
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.UseVisualStyleBackColor = true;
            this.chkCompress.Click += new System.EventHandler(this.OnCheckCompressChanged);
            // 
            // maxPixelSize
            // 
            resources.ApplyResources(this.maxPixelSize, "maxPixelSize");
            this.maxPixelSize.Maximum = new decimal(new int[] {
            24000,
            0,
            0,
            0});
            this.maxPixelSize.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.maxPixelSize.Name = "maxPixelSize";
            this.maxPixelSize.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // lblCompress
            // 
            this.lblCompress.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCompress, "lblCompress");
            this.lblCompress.Name = "lblCompress";
            // 
            // lblMaxPixels
            // 
            resources.ApplyResources(this.lblMaxPixels, "lblMaxPixels");
            this.lblMaxPixels.Name = "lblMaxPixels";
            // 
            // lblGrpProgress
            // 
            resources.ApplyResources(this.lblGrpProgress, "lblGrpProgress");
            this.lblGrpProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblGrpProgress.Name = "lblGrpProgress";
            // 
            // pnlProgress
            // 
            this.pnlProgress.Controls.Add(this.lnkSkippedDetails);
            this.pnlProgress.Controls.Add(this.txtSkipped);
            this.pnlProgress.Controls.Add(this.lblSkippedFiles);
            this.pnlProgress.Controls.Add(this.lnkErrorDetails);
            this.pnlProgress.Controls.Add(this.txtErrors);
            this.pnlProgress.Controls.Add(this.txtSuccess);
            this.pnlProgress.Controls.Add(this.lblFailedImports);
            this.pnlProgress.Controls.Add(this.progress);
            this.pnlProgress.Controls.Add(this.lblImportedImages);
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.Name = "pnlProgress";
            // 
            // lnkSkippedDetails
            // 
            resources.ApplyResources(this.lnkSkippedDetails, "lnkSkippedDetails");
            this.lnkSkippedDetails.BackColor = System.Drawing.Color.Transparent;
            this.lnkSkippedDetails.Name = "lnkSkippedDetails";
            this.lnkSkippedDetails.TabStop = true;
            this.lnkSkippedDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkSkippedClicked);
            // 
            // txtSkipped
            // 
            resources.ApplyResources(this.txtSkipped, "txtSkipped");
            this.txtSkipped.Name = "txtSkipped";
            this.txtSkipped.ReadOnly = true;
            this.txtSkipped.TabStop = false;
            // 
            // lblSkippedFiles
            // 
            this.lblSkippedFiles.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSkippedFiles, "lblSkippedFiles");
            this.lblSkippedFiles.Name = "lblSkippedFiles";
            // 
            // lnkErrorDetails
            // 
            resources.ApplyResources(this.lnkErrorDetails, "lnkErrorDetails");
            this.lnkErrorDetails.BackColor = System.Drawing.Color.Transparent;
            this.lnkErrorDetails.Name = "lnkErrorDetails";
            this.lnkErrorDetails.TabStop = true;
            this.lnkErrorDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkErrorDetailsClick);
            // 
            // txtErrors
            // 
            resources.ApplyResources(this.txtErrors, "txtErrors");
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.TabStop = false;
            // 
            // txtSuccess
            // 
            resources.ApplyResources(this.txtSuccess, "txtSuccess");
            this.txtSuccess.Name = "txtSuccess";
            this.txtSuccess.ReadOnly = true;
            this.txtSuccess.TabStop = false;
            // 
            // lblFailedImports
            // 
            this.lblFailedImports.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFailedImports, "lblFailedImports");
            this.lblFailedImports.Name = "lblFailedImports";
            // 
            // progress
            // 
            this.progress.ForeColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.progress, "progress");
            this.progress.Name = "progress";
            // 
            // lblImportedImages
            // 
            this.lblImportedImages.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblImportedImages, "lblImportedImages");
            this.lblImportedImages.Name = "lblImportedImages";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.btnImport);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lblDeleteImages
            // 
            this.lblDeleteImages.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDeleteImages, "lblDeleteImages");
            this.lblDeleteImages.Name = "lblDeleteImages";
            // 
            // chkDeleteImages
            // 
            resources.ApplyResources(this.chkDeleteImages, "chkDeleteImages");
            this.chkDeleteImages.BackColor = System.Drawing.Color.Transparent;
            this.chkDeleteImages.Name = "chkDeleteImages";
            this.chkDeleteImages.UseVisualStyleBackColor = false;
            // 
            // ImportImagesDialog
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblGrpProgress);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.lblGrpImageAlteration);
            this.Controls.Add(this.pnlAlteration);
            this.Controls.Add(this.lblGrpImport);
            this.Controls.Add(this.pnglImport);
            this.Controls.Add(this.pnlBottom);
            this.HasHelp = true;
            this.Name = "ImportImagesDialog";
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.Controls.SetChildIndex(this.pnglImport, 0);
            this.Controls.SetChildIndex(this.lblGrpImport, 0);
            this.Controls.SetChildIndex(this.pnlAlteration, 0);
            this.Controls.SetChildIndex(this.lblGrpImageAlteration, 0);
            this.Controls.SetChildIndex(this.pnlProgress, 0);
            this.Controls.SetChildIndex(this.lblGrpProgress, 0);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnglImport.ResumeLayout(false);
            this.pnglImport.PerformLayout();
            this.pnlAlteration.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixelSize)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DialogBottomPanel pnlBottom;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblGrpImport;
        private GroupPanel pnglImport;
        private System.Windows.Forms.Label lblGrpImageAlteration;
        private GroupPanel pnlAlteration;
        private System.Windows.Forms.Label lblGrpProgress;
        private GroupPanel pnlProgress;
        private System.Windows.Forms.Button btnBrowseDataFile;
        private System.Windows.Forms.TextBox txtDataFile;
        private System.Windows.Forms.Label lblDataFile;
        private System.Windows.Forms.RadioButton rbDataFile;
        private System.Windows.Forms.RadioButton rbFolder;
        private System.Windows.Forms.ComboBox cmbImportImages;
        private System.Windows.Forms.Label lblImagesToImport;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label lblImageFolder;
        private System.Windows.Forms.ComboBox cmbCompression;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.Label lblMaxSize;
        private System.Windows.Forms.Label lblCompress;
        private System.Windows.Forms.CheckBox chkSetMax;
        private System.Windows.Forms.Label lblMaxPixels;
        private System.Windows.Forms.NumericUpDown maxPixelSize;
        private System.Windows.Forms.LinkLabel lnkSkippedDetails;
        private System.Windows.Forms.TextBox txtSkipped;
        private System.Windows.Forms.Label lblSkippedFiles;
        private System.Windows.Forms.LinkLabel lnkErrorDetails;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.TextBox txtSuccess;
        private System.Windows.Forms.Label lblFailedImports;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lblImportedImages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkDeleteImages;
        private System.Windows.Forms.Label lblDeleteImages;
    }
}