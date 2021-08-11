namespace LSOne.Peripherals.DialogPages
{
    partial class SetCardReaderPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetCardReaderPage));
            this.lblDiffDigit = new System.Windows.Forms.Label();
            this.lblStartDigit = new System.Windows.Forms.Label();
            this.lblDeiviceName = new System.Windows.Forms.Label();
            this.lblFinalDigit = new System.Windows.Forms.Label();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.btnTest = new LSOne.Controls.TouchButton();
            this.tbFinalDigit = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbDiffDigit = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbStartDigit = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.cmbMSR = new LSOne.Controls.DualDataComboBox();
            this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
            this.errorProvider = new LSOne.Peripherals.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // lblDiffDigit
            // 
            resources.ApplyResources(this.lblDiffDigit, "lblDiffDigit");
            this.lblDiffDigit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDiffDigit.Name = "lblDiffDigit";
            // 
            // lblStartDigit
            // 
            resources.ApplyResources(this.lblStartDigit, "lblStartDigit");
            this.lblStartDigit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStartDigit.Name = "lblStartDigit";
            // 
            // lblDeiviceName
            // 
            resources.ApplyResources(this.lblDeiviceName, "lblDeiviceName");
            this.lblDeiviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDeiviceName.Name = "lblDeiviceName";
            // 
            // lblFinalDigit
            // 
            resources.ApplyResources(this.lblFinalDigit, "lblFinalDigit");
            this.lblFinalDigit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFinalDigit.Name = "lblFinalDigit";
            // 
            // lblDisplay
            // 
            resources.ApplyResources(this.lblDisplay, "lblDisplay");
            this.lblDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDisplay.Name = "lblDisplay";
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
            // tbFinalDigit
            // 
            this.tbFinalDigit.BackColor = System.Drawing.Color.White;
            this.tbFinalDigit.EndCharacter = null;
            resources.ApplyResources(this.tbFinalDigit, "tbFinalDigit");
            this.tbFinalDigit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbFinalDigit.LastTrack = null;
            this.tbFinalDigit.ManualEntryOfTrack = true;
            this.tbFinalDigit.MaxLength = 32767;
            this.tbFinalDigit.Name = "tbFinalDigit";
            this.tbFinalDigit.NumericOnly = false;
            this.tbFinalDigit.Seperator = null;
            this.tbFinalDigit.StartCharacter = null;
            this.tbFinalDigit.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // tbDiffDigit
            // 
            this.tbDiffDigit.BackColor = System.Drawing.Color.White;
            this.tbDiffDigit.EndCharacter = null;
            resources.ApplyResources(this.tbDiffDigit, "tbDiffDigit");
            this.tbDiffDigit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbDiffDigit.LastTrack = null;
            this.tbDiffDigit.ManualEntryOfTrack = true;
            this.tbDiffDigit.MaxLength = 32767;
            this.tbDiffDigit.Name = "tbDiffDigit";
            this.tbDiffDigit.NumericOnly = false;
            this.tbDiffDigit.Seperator = null;
            this.tbDiffDigit.StartCharacter = null;
            this.tbDiffDigit.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // tbStartDigit
            // 
            this.tbStartDigit.BackColor = System.Drawing.Color.White;
            this.tbStartDigit.EndCharacter = null;
            resources.ApplyResources(this.tbStartDigit, "tbStartDigit");
            this.tbStartDigit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbStartDigit.LastTrack = null;
            this.tbStartDigit.ManualEntryOfTrack = true;
            this.tbStartDigit.MaxLength = 32767;
            this.tbStartDigit.Name = "tbStartDigit";
            this.tbStartDigit.NumericOnly = false;
            this.tbStartDigit.Seperator = null;
            this.tbStartDigit.StartCharacter = null;
            this.tbStartDigit.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // cmbMSR
            // 
            this.cmbMSR.AddList = null;
            this.cmbMSR.AllowKeyboardSelection = false;
            this.cmbMSR.EnableTextBox = true;
            resources.ApplyResources(this.cmbMSR, "cmbMSR");
            this.cmbMSR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbMSR.IsPOSControl = true;
            this.cmbMSR.MaxLength = 32767;
            this.cmbMSR.Name = "cmbMSR";
            this.cmbMSR.NoChangeAllowed = false;
            this.cmbMSR.OnlyDisplayID = false;
            this.cmbMSR.ReadOnly = true;
            this.cmbMSR.RemoveList = null;
            this.cmbMSR.RowHeight = ((short)(22));
            this.cmbMSR.SecondaryData = null;
            this.cmbMSR.SelectedData = null;
            this.cmbMSR.SelectedDataID = null;
            this.cmbMSR.SelectionList = null;
            this.cmbMSR.ShowDropDownOnTyping = true;
            this.cmbMSR.SkipIDColumn = true;
            this.cmbMSR.Touch = true;
            this.cmbMSR.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbMSR_DropDown);
            this.cmbMSR.SelectedDataChanged += new System.EventHandler(this.cmbMSR_SelectedDataChanged);
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
            // 
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // SetCardReaderPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.cmbMSR);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblFinalDigit);
            this.Controls.Add(this.tbFinalDigit);
            this.Controls.Add(this.lblDiffDigit);
            this.Controls.Add(this.tbDiffDigit);
            this.Controls.Add(this.lblStartDigit);
            this.Controls.Add(this.tbStartDigit);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.lblDeiviceName);
            this.Name = "SetCardReaderPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDiffDigit;
        private LSOne.Controls.MSRTextBoxTouch tbDiffDigit;
        private System.Windows.Forms.Label lblStartDigit;
        private LSOne.Controls.MSRTextBoxTouch tbStartDigit;
        private LSOne.Controls.DualDataComboBox cmbDeviceName;
        private System.Windows.Forms.Label lblDeiviceName;
        private System.Windows.Forms.Label lblFinalDigit;
        private LSOne.Controls.MSRTextBoxTouch tbFinalDigit;
        private LSOne.Controls.TouchButton btnTest;
        private LSOne.Controls.DualDataComboBox cmbMSR;
        private System.Windows.Forms.Label lblDisplay;
        private TouchErrorProvider errorProvider;
    }
}

