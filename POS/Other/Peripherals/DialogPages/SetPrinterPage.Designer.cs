using LSOne.Controls;

namespace LSOne.Peripherals.DialogPages
{
    partial class SetPrinterPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPrinterPage));
            this.lblPrinter = new System.Windows.Forms.Label();
            this.cmbPrinter = new LSOne.Controls.DualDataComboBox();
            this.chkDecimalConversion = new LSOne.Controls.TouchCheckBox();
            this.lblPrinterCharset = new System.Windows.Forms.Label();
            this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
            this.ntbPrinterCharset = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.lblExtralines = new System.Windows.Forms.Label();
            this.ntbExtraLines = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.lblPrinterName = new System.Windows.Forms.Label();
            this.btnTest = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Peripherals.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // lblPrinter
            // 
            resources.ApplyResources(this.lblPrinter, "lblPrinter");
            this.lblPrinter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPrinter.Name = "lblPrinter";
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.AddList = null;
            this.cmbPrinter.AllowKeyboardSelection = false;
            this.cmbPrinter.EnableTextBox = true;
            resources.ApplyResources(this.cmbPrinter, "cmbPrinter");
            this.cmbPrinter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbPrinter.IsPOSControl = true;
            this.cmbPrinter.MaxLength = 32767;
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.NoChangeAllowed = false;
            this.cmbPrinter.OnlyDisplayID = false;
            this.cmbPrinter.ReadOnly = true;
            this.cmbPrinter.RemoveList = null;
            this.cmbPrinter.RowHeight = ((short)(22));
            this.cmbPrinter.SecondaryData = null;
            this.cmbPrinter.SelectedData = null;
            this.cmbPrinter.SelectedDataID = null;
            this.cmbPrinter.SelectionList = null;
            this.cmbPrinter.ShowDropDownOnTyping = true;
            this.cmbPrinter.SkipIDColumn = true;
            this.cmbPrinter.Touch = true;
            this.cmbPrinter.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbPrinter_DropDown);
            this.cmbPrinter.SelectedDataChanged += new System.EventHandler(this.cmbPrinter_SelectedDataChanged);
            // 
            // chkDecimalConversion
            // 
            resources.ApplyResources(this.chkDecimalConversion, "chkDecimalConversion");
            this.chkDecimalConversion.Name = "chkDecimalConversion";
            // 
            // lblPrinterCharset
            // 
            resources.ApplyResources(this.lblPrinterCharset, "lblPrinterCharset");
            this.lblPrinterCharset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPrinterCharset.Name = "lblPrinterCharset";
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.AddList = null;
            this.cmbDeviceName.AllowKeyboardSelection = false;
            this.cmbDeviceName.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
            this.cmbDeviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbDeviceName.IsPOSControl = true;
            this.cmbDeviceName.MaxLength = 32767;
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.NoChangeAllowed = false;
            this.cmbDeviceName.OnlyDisplayID = false;
            this.cmbDeviceName.RemoveList = null;
            this.cmbDeviceName.RowHeight = ((short)(50));
            this.cmbDeviceName.SecondaryData = null;
            this.cmbDeviceName.SelectedData = null;
            this.cmbDeviceName.SelectedDataID = null;
            this.cmbDeviceName.SelectionList = null;
            this.cmbDeviceName.SkipIDColumn = true;
            this.cmbDeviceName.Tag = "0";
            this.cmbDeviceName.Touch = true;
            this.cmbDeviceName.RequestData += new System.EventHandler(this.cmbDeviceName_RequestData);
            this.cmbDeviceName.SelectedDataChanged += new System.EventHandler(this.cmbDeviceName_SelectedDataChanged);
            this.cmbDeviceName.SelectedDataCleared += new System.EventHandler(this.cmbDeviceName_SelectedDataCleared);
            // 
            // ntbPrinterCharset
            // 
            this.ntbPrinterCharset.BackColor = System.Drawing.Color.White;
            this.ntbPrinterCharset.EndCharacter = null;
            this.ntbPrinterCharset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbPrinterCharset.LastTrack = null;
            resources.ApplyResources(this.ntbPrinterCharset, "ntbPrinterCharset");
            this.ntbPrinterCharset.ManualEntryOfTrack = true;
            this.ntbPrinterCharset.MaxLength = 32767;
            this.ntbPrinterCharset.Name = "ntbPrinterCharset";
            this.ntbPrinterCharset.NumericOnly = true;
            this.ntbPrinterCharset.SelectionStart = 1;
            this.ntbPrinterCharset.Seperator = null;
            this.ntbPrinterCharset.StartCharacter = null;
            this.ntbPrinterCharset.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // lblExtralines
            // 
            resources.ApplyResources(this.lblExtralines, "lblExtralines");
            this.lblExtralines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblExtralines.Name = "lblExtralines";
            // 
            // ntbExtraLines
            // 
            this.ntbExtraLines.BackColor = System.Drawing.Color.White;
            this.ntbExtraLines.EndCharacter = null;
            this.ntbExtraLines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbExtraLines.LastTrack = null;
            resources.ApplyResources(this.ntbExtraLines, "ntbExtraLines");
            this.ntbExtraLines.ManualEntryOfTrack = true;
            this.ntbExtraLines.MaxLength = 32767;
            this.ntbExtraLines.Name = "ntbExtraLines";
            this.ntbExtraLines.NumericOnly = true;
            this.ntbExtraLines.SelectionStart = 1;
            this.ntbExtraLines.Seperator = null;
            this.ntbExtraLines.StartCharacter = null;
            this.ntbExtraLines.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // lblPrinterName
            // 
            resources.ApplyResources(this.lblPrinterName, "lblPrinterName");
            this.lblPrinterName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPrinterName.Name = "lblPrinterName";
            // 
            // btnTest
            // 
            this.btnTest.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnTest.BackgroundImage = global::LSOne.Peripherals.Properties.Resources.Checkmark_white_32px;
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnTest.DrawBorder = false;
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // SetPrinterPage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblExtralines);
            this.Controls.Add(this.ntbExtraLines);
            this.Controls.Add(this.lblPrinterCharset);
            this.Controls.Add(this.ntbPrinterCharset);
            this.Controls.Add(this.chkDecimalConversion);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.cmbPrinter);
            this.Controls.Add(this.lblPrinterName);
            this.Controls.Add(this.lblPrinter);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.Name = "SetPrinterPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrinter;
        private LSOne.Controls.DualDataComboBox cmbPrinter;
        private TouchCheckBox chkDecimalConversion;
        private LSOne.Controls.DualDataComboBox cmbDeviceName;
        private System.Windows.Forms.Label lblPrinterCharset;
        private MSRTextBoxTouch ntbPrinterCharset;
        private System.Windows.Forms.Label lblExtralines;
        private MSRTextBoxTouch ntbExtraLines;
        private System.Windows.Forms.Label lblPrinterName;
        private LSOne.Controls.TouchButton btnTest;
        private TouchErrorProvider errorProvider;
    }
}
