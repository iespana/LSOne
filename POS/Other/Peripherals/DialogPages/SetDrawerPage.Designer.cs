using LSOne.Controls;

namespace LSOne.Peripherals.DialogPages
{
    partial class SetDrawerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetDrawerPage));
            this.lblPrinterName = new System.Windows.Forms.Label();
            this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
            this.chkDrawerConnected = new LSOne.Controls.TouchCheckBox();
            this.lblOpenText = new System.Windows.Forms.Label();
            this.tbOpenText = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.btnTest = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Peripherals.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // lblPrinterName
            // 
            resources.ApplyResources(this.lblPrinterName, "lblPrinterName");
            this.lblPrinterName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPrinterName.Name = "lblPrinterName";
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.AddList = null;
            this.cmbDeviceName.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
            this.cmbDeviceName.EnableTextBox = true;
            this.cmbDeviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbDeviceName.IsPOSControl = true;
            this.cmbDeviceName.MaxLength = 32767;
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.NoChangeAllowed = false;
            this.cmbDeviceName.OnlyDisplayID = false;
            this.cmbDeviceName.ReadOnly = true;
            this.cmbDeviceName.RemoveList = null;
            this.cmbDeviceName.RowHeight = ((short)(22));
            this.cmbDeviceName.SecondaryData = null;
            this.cmbDeviceName.SelectedData = null;
            this.cmbDeviceName.SelectedDataID = null;
            this.cmbDeviceName.SelectionList = null;
            this.cmbDeviceName.ShowDropDownOnTyping = true;
            this.cmbDeviceName.SkipIDColumn = true;
            this.cmbDeviceName.Touch = true;
            this.cmbDeviceName.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbDeviceName_DropDown);
            this.cmbDeviceName.SelectedDataChanged += new System.EventHandler(this.cmbDeviceName_SelectedDataChanged);
            this.cmbDeviceName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
            // 
            // chkDrawerConnected
            // 
            resources.ApplyResources(this.chkDrawerConnected, "chkDrawerConnected");
            this.chkDrawerConnected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.chkDrawerConnected.Name = "chkDrawerConnected";
            this.chkDrawerConnected.CheckedChanged += new System.EventHandler(this.chkDrawerConnected_CheckedChanged);
            // 
            // lblOpenText
            // 
            resources.ApplyResources(this.lblOpenText, "lblOpenText");
            this.lblOpenText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblOpenText.Name = "lblOpenText";
            // 
            // tbOpenText
            // 
            this.tbOpenText.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbOpenText, "tbOpenText");
            this.tbOpenText.EndCharacter = null;
            this.tbOpenText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbOpenText.LastTrack = null;
            this.tbOpenText.ManualEntryOfTrack = true;
            this.tbOpenText.MaxLength = 32767;
            this.tbOpenText.Name = "tbOpenText";
            this.tbOpenText.NumericOnly = false;
            this.tbOpenText.Seperator = null;
            this.tbOpenText.StartCharacter = null;
            this.tbOpenText.TrackSeperation = LSOne.Controls.TrackSeperation.None;
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
            // SetDrawerPage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblOpenText);
            this.Controls.Add(this.tbOpenText);
            this.Controls.Add(this.chkDrawerConnected);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.lblPrinterName);
            this.Name = "SetDrawerPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrinterName;
        private LSOne.Controls.DualDataComboBox cmbDeviceName;
        private TouchCheckBox chkDrawerConnected;
        private System.Windows.Forms.Label lblOpenText;
        private MSRTextBoxTouch tbOpenText;
        private LSOne.Controls.TouchButton btnTest;
        private TouchErrorProvider errorProvider;
    }
}
