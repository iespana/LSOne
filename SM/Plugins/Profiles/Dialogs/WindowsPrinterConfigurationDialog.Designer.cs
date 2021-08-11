namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    partial class WindowsPrinterConfigurationDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsPrinterConfigurationDialog));
            this.ntbTopMargin = new LSOne.Controls.NumericTextBox();
            this.ntbBottomMargin = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.tbFolderLocation = new System.Windows.Forms.TextBox();
            this.lblFolderLocation = new System.Windows.Forms.Label();
            this.lblPrintDesignBoxes = new System.Windows.Forms.Label();
            this.chkPrintDesignBoxes = new System.Windows.Forms.CheckBox();
            this.ntbFontSize = new LSOne.Controls.NumericTextBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.tbFont = new System.Windows.Forms.TextBox();
            this.ntbLeftMargin = new LSOne.Controls.NumericTextBox();
            this.ntbRightMargin = new LSOne.Controls.NumericTextBox();
            this.lblFont = new System.Windows.Forms.Label();
            this.lblRightMargin = new System.Windows.Forms.Label();
            this.lblLeftMargin = new System.Windows.Forms.Label();
            this.ntbWideHighFontSize = new LSOne.Controls.NumericTextBox();
            this.lblWideHighFontSize = new System.Windows.Forms.Label();
            this.btnEditFontName = new LSOne.Controls.ContextButton();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.lblWindowsPrinter = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderLocationErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.folderLocationErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ntbTopMargin
            // 
            this.ntbTopMargin.AllowDecimal = false;
            this.ntbTopMargin.AllowNegative = false;
            this.ntbTopMargin.CultureInfo = null;
            this.ntbTopMargin.DecimalLetters = 2;
            this.ntbTopMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbTopMargin.HasMinValue = false;
            this.ntbTopMargin.Location = new System.Drawing.Point(206, 180);
            this.ntbTopMargin.MaxLength = 3;
            this.ntbTopMargin.MaxValue = 200D;
            this.ntbTopMargin.MinValue = 0D;
            this.ntbTopMargin.Name = "ntbTopMargin";
            this.ntbTopMargin.Size = new System.Drawing.Size(100, 20);
            this.ntbTopMargin.TabIndex = 9;
            this.ntbTopMargin.Tag = "99";
            this.ntbTopMargin.Text = "0";
            this.ntbTopMargin.Value = 0D;
            this.ntbTopMargin.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbBottomMargin
            // 
            this.ntbBottomMargin.AllowDecimal = false;
            this.ntbBottomMargin.AllowNegative = false;
            this.ntbBottomMargin.CultureInfo = null;
            this.ntbBottomMargin.DecimalLetters = 2;
            this.ntbBottomMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbBottomMargin.HasMinValue = false;
            this.ntbBottomMargin.Location = new System.Drawing.Point(206, 206);
            this.ntbBottomMargin.MaxLength = 3;
            this.ntbBottomMargin.MaxValue = 200D;
            this.ntbBottomMargin.MinValue = 0D;
            this.ntbBottomMargin.Name = "ntbBottomMargin";
            this.ntbBottomMargin.Size = new System.Drawing.Size(100, 20);
            this.ntbBottomMargin.TabIndex = 11;
            this.ntbBottomMargin.Tag = "99";
            this.ntbBottomMargin.Text = "0";
            this.ntbBottomMargin.Value = 0D;
            this.ntbBottomMargin.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 23);
            this.label4.TabIndex = 10;
            this.label4.Tag = "99";
            this.label4.Text = "Bottom margin (mm):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(12, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(188, 23);
            this.label7.TabIndex = 8;
            this.label7.Tag = "99";
            this.label7.Text = "Top margin (mm):";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectFolder.Location = new System.Drawing.Point(465, 331);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(28, 23);
            this.btnSelectFolder.TabIndex = 23;
            this.btnSelectFolder.Tag = "99";
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // tbFolderLocation
            // 
            this.tbFolderLocation.Location = new System.Drawing.Point(206, 332);
            this.tbFolderLocation.MaxLength = 500;
            this.tbFolderLocation.Name = "tbFolderLocation";
            this.tbFolderLocation.Size = new System.Drawing.Size(253, 20);
            this.tbFolderLocation.TabIndex = 22;
            this.tbFolderLocation.Tag = "99";
            this.tbFolderLocation.TextChanged += new System.EventHandler(this.tbFolderLocation_TextChanged);
            this.tbFolderLocation.Leave += new System.EventHandler(this.CheckFolderAndFileInformation);
            // 
            // lblFolderLocation
            // 
            this.lblFolderLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblFolderLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFolderLocation.Location = new System.Drawing.Point(12, 330);
            this.lblFolderLocation.Name = "lblFolderLocation";
            this.lblFolderLocation.Size = new System.Drawing.Size(188, 23);
            this.lblFolderLocation.TabIndex = 21;
            this.lblFolderLocation.Tag = "99";
            this.lblFolderLocation.Text = "Folder location:";
            this.lblFolderLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPrintDesignBoxes
            // 
            this.lblPrintDesignBoxes.BackColor = System.Drawing.Color.Transparent;
            this.lblPrintDesignBoxes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPrintDesignBoxes.Location = new System.Drawing.Point(12, 305);
            this.lblPrintDesignBoxes.Name = "lblPrintDesignBoxes";
            this.lblPrintDesignBoxes.Size = new System.Drawing.Size(188, 23);
            this.lblPrintDesignBoxes.TabIndex = 19;
            this.lblPrintDesignBoxes.Tag = "99";
            this.lblPrintDesignBoxes.Text = "Print design boxes:";
            this.lblPrintDesignBoxes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkPrintDesignBoxes
            // 
            this.chkPrintDesignBoxes.AutoSize = true;
            this.chkPrintDesignBoxes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkPrintDesignBoxes.Location = new System.Drawing.Point(206, 309);
            this.chkPrintDesignBoxes.Name = "chkPrintDesignBoxes";
            this.chkPrintDesignBoxes.Size = new System.Drawing.Size(44, 17);
            this.chkPrintDesignBoxes.TabIndex = 20;
            this.chkPrintDesignBoxes.Tag = "99";
            this.chkPrintDesignBoxes.Text = "Yes";
            this.chkPrintDesignBoxes.UseVisualStyleBackColor = true;
            this.chkPrintDesignBoxes.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbFontSize
            // 
            this.ntbFontSize.AllowDecimal = true;
            this.ntbFontSize.AllowNegative = false;
            this.ntbFontSize.CultureInfo = null;
            this.ntbFontSize.DecimalLetters = 1;
            this.ntbFontSize.ForeColor = System.Drawing.Color.Black;
            this.ntbFontSize.HasMinValue = false;
            this.ntbFontSize.Location = new System.Drawing.Point(206, 258);
            this.ntbFontSize.MaxLength = 4;
            this.ntbFontSize.MaxValue = 100D;
            this.ntbFontSize.MinValue = 1D;
            this.ntbFontSize.Name = "ntbFontSize";
            this.ntbFontSize.Size = new System.Drawing.Size(100, 20);
            this.ntbFontSize.TabIndex = 16;
            this.ntbFontSize.Tag = "99";
            this.ntbFontSize.Text = "0.0";
            this.ntbFontSize.Value = 0D;
            this.ntbFontSize.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblFontSize
            // 
            this.lblFontSize.BackColor = System.Drawing.Color.Transparent;
            this.lblFontSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFontSize.Location = new System.Drawing.Point(12, 256);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(188, 23);
            this.lblFontSize.TabIndex = 15;
            this.lblFontSize.Tag = "99";
            this.lblFontSize.Text = "Font size:";
            this.lblFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFont
            // 
            this.tbFont.Enabled = false;
            this.tbFont.Location = new System.Drawing.Point(206, 232);
            this.tbFont.MaxLength = 200;
            this.tbFont.Name = "tbFont";
            this.tbFont.Size = new System.Drawing.Size(253, 20);
            this.tbFont.TabIndex = 13;
            this.tbFont.TabStop = false;
            this.tbFont.Text = "Courier New";
            this.tbFont.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbLeftMargin
            // 
            this.ntbLeftMargin.AllowDecimal = false;
            this.ntbLeftMargin.AllowNegative = false;
            this.ntbLeftMargin.CultureInfo = null;
            this.ntbLeftMargin.DecimalLetters = 2;
            this.ntbLeftMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbLeftMargin.HasMinValue = false;
            this.ntbLeftMargin.Location = new System.Drawing.Point(206, 128);
            this.ntbLeftMargin.MaxLength = 3;
            this.ntbLeftMargin.MaxValue = 200D;
            this.ntbLeftMargin.MinValue = 0D;
            this.ntbLeftMargin.Name = "ntbLeftMargin";
            this.ntbLeftMargin.Size = new System.Drawing.Size(100, 20);
            this.ntbLeftMargin.TabIndex = 5;
            this.ntbLeftMargin.Tag = "99";
            this.ntbLeftMargin.Text = "0";
            this.ntbLeftMargin.Value = 0D;
            this.ntbLeftMargin.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbRightMargin
            // 
            this.ntbRightMargin.AllowDecimal = false;
            this.ntbRightMargin.AllowNegative = false;
            this.ntbRightMargin.CultureInfo = null;
            this.ntbRightMargin.DecimalLetters = 2;
            this.ntbRightMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbRightMargin.HasMinValue = false;
            this.ntbRightMargin.Location = new System.Drawing.Point(206, 154);
            this.ntbRightMargin.MaxLength = 3;
            this.ntbRightMargin.MaxValue = 200D;
            this.ntbRightMargin.MinValue = 0D;
            this.ntbRightMargin.Name = "ntbRightMargin";
            this.ntbRightMargin.Size = new System.Drawing.Size(100, 20);
            this.ntbRightMargin.TabIndex = 7;
            this.ntbRightMargin.Tag = "99";
            this.ntbRightMargin.Text = "0";
            this.ntbRightMargin.Value = 0D;
            this.ntbRightMargin.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblFont
            // 
            this.lblFont.BackColor = System.Drawing.Color.Transparent;
            this.lblFont.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFont.Location = new System.Drawing.Point(12, 230);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(188, 23);
            this.lblFont.TabIndex = 12;
            this.lblFont.Tag = "99";
            this.lblFont.Text = "Font:";
            this.lblFont.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRightMargin
            // 
            this.lblRightMargin.BackColor = System.Drawing.Color.Transparent;
            this.lblRightMargin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRightMargin.Location = new System.Drawing.Point(12, 152);
            this.lblRightMargin.Name = "lblRightMargin";
            this.lblRightMargin.Size = new System.Drawing.Size(188, 23);
            this.lblRightMargin.TabIndex = 6;
            this.lblRightMargin.Tag = "99";
            this.lblRightMargin.Text = "Right margin (mm):";
            this.lblRightMargin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLeftMargin
            // 
            this.lblLeftMargin.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftMargin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblLeftMargin.Location = new System.Drawing.Point(12, 126);
            this.lblLeftMargin.Name = "lblLeftMargin";
            this.lblLeftMargin.Size = new System.Drawing.Size(188, 23);
            this.lblLeftMargin.TabIndex = 4;
            this.lblLeftMargin.Tag = "99";
            this.lblLeftMargin.Text = "Left margin (mm):";
            this.lblLeftMargin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ntbWideHighFontSize
            // 
            this.ntbWideHighFontSize.AllowDecimal = true;
            this.ntbWideHighFontSize.AllowNegative = false;
            this.ntbWideHighFontSize.CultureInfo = null;
            this.ntbWideHighFontSize.DecimalLetters = 1;
            this.ntbWideHighFontSize.ForeColor = System.Drawing.Color.Black;
            this.ntbWideHighFontSize.HasMinValue = false;
            this.ntbWideHighFontSize.Location = new System.Drawing.Point(206, 284);
            this.ntbWideHighFontSize.MaxLength = 4;
            this.ntbWideHighFontSize.MaxValue = 100D;
            this.ntbWideHighFontSize.MinValue = 1D;
            this.ntbWideHighFontSize.Name = "ntbWideHighFontSize";
            this.ntbWideHighFontSize.Size = new System.Drawing.Size(100, 20);
            this.ntbWideHighFontSize.TabIndex = 18;
            this.ntbWideHighFontSize.Tag = "99";
            this.ntbWideHighFontSize.Text = "0.0";
            this.ntbWideHighFontSize.Value = 0D;
            this.ntbWideHighFontSize.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblWideHighFontSize
            // 
            this.lblWideHighFontSize.BackColor = System.Drawing.Color.Transparent;
            this.lblWideHighFontSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWideHighFontSize.Location = new System.Drawing.Point(12, 282);
            this.lblWideHighFontSize.Name = "lblWideHighFontSize";
            this.lblWideHighFontSize.Size = new System.Drawing.Size(188, 23);
            this.lblWideHighFontSize.TabIndex = 17;
            this.lblWideHighFontSize.Tag = "99";
            this.lblWideHighFontSize.Text = "Wide high font size:";
            this.lblWideHighFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditFontName
            // 
            this.btnEditFontName.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFontName.Context = LSOne.Controls.ButtonType.Edit;
            this.btnEditFontName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditFontName.Location = new System.Drawing.Point(465, 230);
            this.btnEditFontName.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnEditFontName.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnEditFontName.Name = "btnEditFontName";
            this.btnEditFontName.Size = new System.Drawing.Size(23, 23);
            this.btnEditFontName.TabIndex = 14;
            this.btnEditFontName.Click += new System.EventHandler(this.btnEditFontName_Click);
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.FormattingEnabled = true;
            this.cmbPrinter.Location = new System.Drawing.Point(206, 101);
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.Size = new System.Drawing.Size(253, 21);
            this.cmbPrinter.TabIndex = 3;
            this.cmbPrinter.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblWindowsPrinter
            // 
            this.lblWindowsPrinter.BackColor = System.Drawing.Color.Transparent;
            this.lblWindowsPrinter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWindowsPrinter.Location = new System.Drawing.Point(9, 99);
            this.lblWindowsPrinter.Name = "lblWindowsPrinter";
            this.lblWindowsPrinter.Size = new System.Drawing.Size(191, 23);
            this.lblWindowsPrinter.TabIndex = 2;
            this.lblWindowsPrinter.Tag = "99";
            this.lblWindowsPrinter.Text = "Windows printer:";
            this.lblWindowsPrinter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(-3, 368);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(588, 46);
            this.panel2.TabIndex = 24;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(410, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(491, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // folderLocationErrorProvider
            // 
            this.folderLocationErrorProvider.ContainerControl = this;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(206, 75);
            this.tbDescription.MaxLength = 100;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(253, 20);
            this.tbDescription.TabIndex = 1;
            this.tbDescription.Tag = "99";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDescription.Location = new System.Drawing.Point(12, 73);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(188, 23);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Tag = "99";
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WindowsPrinterConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(580, 409);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblWindowsPrinter);
            this.Controls.Add(this.cmbPrinter);
            this.Controls.Add(this.btnEditFontName);
            this.Controls.Add(this.ntbWideHighFontSize);
            this.Controls.Add(this.lblWideHighFontSize);
            this.Controls.Add(this.ntbTopMargin);
            this.Controls.Add(this.ntbBottomMargin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.tbFolderLocation);
            this.Controls.Add(this.lblFolderLocation);
            this.Controls.Add(this.lblPrintDesignBoxes);
            this.Controls.Add(this.chkPrintDesignBoxes);
            this.Controls.Add(this.ntbFontSize);
            this.Controls.Add(this.lblFontSize);
            this.Controls.Add(this.tbFont);
            this.Controls.Add(this.ntbLeftMargin);
            this.Controls.Add(this.ntbRightMargin);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.lblRightMargin);
            this.Controls.Add(this.lblLeftMargin);
            this.HasHelp = true;
            this.Header = "Windows printer configuration";
            this.Name = "WindowsPrinterConfigurationDialog";
            this.Text = "Windows printer configuration";
            this.Controls.SetChildIndex(this.lblLeftMargin, 0);
            this.Controls.SetChildIndex(this.lblRightMargin, 0);
            this.Controls.SetChildIndex(this.lblFont, 0);
            this.Controls.SetChildIndex(this.ntbRightMargin, 0);
            this.Controls.SetChildIndex(this.ntbLeftMargin, 0);
            this.Controls.SetChildIndex(this.tbFont, 0);
            this.Controls.SetChildIndex(this.lblFontSize, 0);
            this.Controls.SetChildIndex(this.ntbFontSize, 0);
            this.Controls.SetChildIndex(this.chkPrintDesignBoxes, 0);
            this.Controls.SetChildIndex(this.lblPrintDesignBoxes, 0);
            this.Controls.SetChildIndex(this.lblFolderLocation, 0);
            this.Controls.SetChildIndex(this.tbFolderLocation, 0);
            this.Controls.SetChildIndex(this.btnSelectFolder, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.ntbBottomMargin, 0);
            this.Controls.SetChildIndex(this.ntbTopMargin, 0);
            this.Controls.SetChildIndex(this.lblWideHighFontSize, 0);
            this.Controls.SetChildIndex(this.ntbWideHighFontSize, 0);
            this.Controls.SetChildIndex(this.btnEditFontName, 0);
            this.Controls.SetChildIndex(this.cmbPrinter, 0);
            this.Controls.SetChildIndex(this.lblWindowsPrinter, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.folderLocationErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.NumericTextBox ntbTopMargin;
        private Controls.NumericTextBox ntbBottomMargin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox tbFolderLocation;
        private System.Windows.Forms.Label lblFolderLocation;
        private System.Windows.Forms.Label lblPrintDesignBoxes;
        private System.Windows.Forms.CheckBox chkPrintDesignBoxes;
        private Controls.NumericTextBox ntbFontSize;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.TextBox tbFont;
        private Controls.NumericTextBox ntbLeftMargin;
        private Controls.NumericTextBox ntbRightMargin;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.Label lblRightMargin;
        private System.Windows.Forms.Label lblLeftMargin;
        private Controls.NumericTextBox ntbWideHighFontSize;
        private System.Windows.Forms.Label lblWideHighFontSize;
        private Controls.ContextButton btnEditFontName;
        private System.Windows.Forms.ComboBox cmbPrinter;
        private System.Windows.Forms.Label lblWindowsPrinter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider folderLocationErrorProvider;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
    }
}