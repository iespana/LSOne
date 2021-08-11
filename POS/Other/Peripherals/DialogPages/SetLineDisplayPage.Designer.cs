namespace LSOne.Peripherals.DialogPages
{
    partial class SetLineDisplayPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetLineDisplayPage));
            this.lblDisplayBalanceText = new System.Windows.Forms.Label();
            this.lblDisplayTotalText = new System.Windows.Forms.Label();
            this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
            this.lblDeiviceName = new System.Windows.Forms.Label();
            this.lblTillClosedLine1 = new System.Windows.Forms.Label();
            this.cmbDisplay = new LSOne.Controls.DualDataComboBox();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.lblTillClosedLine2 = new System.Windows.Forms.Label();
            this.lblCharset = new System.Windows.Forms.Label();
            this.btnTest = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Peripherals.TouchErrorProvider();
            this.ntbCharset = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbTillClosedLine2 = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbDisplayBalanceText = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbDisplayTotalText = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.chkLineDisplayBinaryConversion = new LSOne.Controls.TouchCheckBox();
            this.tbTillClosedLine1 = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.SuspendLayout();
            // 
            // lblDisplayBalanceText
            // 
            resources.ApplyResources(this.lblDisplayBalanceText, "lblDisplayBalanceText");
            this.lblDisplayBalanceText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDisplayBalanceText.Name = "lblDisplayBalanceText";
            // 
            // lblDisplayTotalText
            // 
            resources.ApplyResources(this.lblDisplayTotalText, "lblDisplayTotalText");
            this.lblDisplayTotalText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDisplayTotalText.Name = "lblDisplayTotalText";
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
            this.cmbDeviceName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
            // 
            // lblDeiviceName
            // 
            resources.ApplyResources(this.lblDeiviceName, "lblDeiviceName");
            this.lblDeiviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDeiviceName.Name = "lblDeiviceName";
            // 
            // lblTillClosedLine1
            // 
            resources.ApplyResources(this.lblTillClosedLine1, "lblTillClosedLine1");
            this.lblTillClosedLine1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTillClosedLine1.Name = "lblTillClosedLine1";
            // 
            // cmbDisplay
            // 
            this.cmbDisplay.AddList = null;
            this.cmbDisplay.AllowKeyboardSelection = false;
            this.cmbDisplay.EnableTextBox = true;
            resources.ApplyResources(this.cmbDisplay, "cmbDisplay");
            this.cmbDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbDisplay.IsPOSControl = true;
            this.cmbDisplay.MaxLength = 32767;
            this.cmbDisplay.Name = "cmbDisplay";
            this.cmbDisplay.NoChangeAllowed = false;
            this.cmbDisplay.OnlyDisplayID = false;
            this.cmbDisplay.ReadOnly = true;
            this.cmbDisplay.RemoveList = null;
            this.cmbDisplay.RowHeight = ((short)(22));
            this.cmbDisplay.SecondaryData = null;
            this.cmbDisplay.SelectedData = null;
            this.cmbDisplay.SelectedDataID = null;
            this.cmbDisplay.SelectionList = null;
            this.cmbDisplay.ShowDropDownOnTyping = true;
            this.cmbDisplay.SkipIDColumn = true;
            this.cmbDisplay.Touch = true;
            this.cmbDisplay.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbDisplay_DropDown);
            this.cmbDisplay.SelectedDataChanged += new System.EventHandler(this.cmbDisplay_SelectedDataChanged);
            // 
            // lblDisplay
            // 
            resources.ApplyResources(this.lblDisplay, "lblDisplay");
            this.lblDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDisplay.Name = "lblDisplay";
            // 
            // lblTillClosedLine2
            // 
            resources.ApplyResources(this.lblTillClosedLine2, "lblTillClosedLine2");
            this.lblTillClosedLine2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTillClosedLine2.Name = "lblTillClosedLine2";
            // 
            // lblCharset
            // 
            resources.ApplyResources(this.lblCharset, "lblCharset");
            this.lblCharset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCharset.Name = "lblCharset";
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
            // ntbCharset
            // 
            this.ntbCharset.BackColor = System.Drawing.Color.White;
            this.ntbCharset.EndCharacter = null;
            this.ntbCharset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbCharset.LastTrack = null;
            resources.ApplyResources(this.ntbCharset, "ntbCharset");
            this.ntbCharset.ManualEntryOfTrack = true;
            this.ntbCharset.MaxLength = 32767;
            this.ntbCharset.Name = "ntbCharset";
            this.ntbCharset.NumericOnly = true;
            this.ntbCharset.SelectionStart = 1;
            this.ntbCharset.Seperator = null;
            this.ntbCharset.StartCharacter = null;
            this.ntbCharset.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.ntbCharset.TextChanged += new System.EventHandler(this.ntbCharset_TextChanged);
            // 
            // tbTillClosedLine2
            // 
            this.tbTillClosedLine2.BackColor = System.Drawing.Color.White;
            this.tbTillClosedLine2.EndCharacter = null;
            this.tbTillClosedLine2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbTillClosedLine2.LastTrack = null;
            resources.ApplyResources(this.tbTillClosedLine2, "tbTillClosedLine2");
            this.tbTillClosedLine2.ManualEntryOfTrack = true;
            this.tbTillClosedLine2.MaxLength = 32767;
            this.tbTillClosedLine2.Name = "tbTillClosedLine2";
            this.tbTillClosedLine2.NumericOnly = false;
            this.tbTillClosedLine2.Seperator = null;
            this.tbTillClosedLine2.StartCharacter = null;
            this.tbTillClosedLine2.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // tbDisplayBalanceText
            // 
            this.tbDisplayBalanceText.BackColor = System.Drawing.Color.White;
            this.tbDisplayBalanceText.EndCharacter = null;
            this.tbDisplayBalanceText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbDisplayBalanceText.LastTrack = null;
            resources.ApplyResources(this.tbDisplayBalanceText, "tbDisplayBalanceText");
            this.tbDisplayBalanceText.ManualEntryOfTrack = true;
            this.tbDisplayBalanceText.MaxLength = 32767;
            this.tbDisplayBalanceText.Name = "tbDisplayBalanceText";
            this.tbDisplayBalanceText.NumericOnly = false;
            this.tbDisplayBalanceText.Seperator = null;
            this.tbDisplayBalanceText.StartCharacter = null;
            this.tbDisplayBalanceText.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // tbDisplayTotalText
            // 
            this.tbDisplayTotalText.BackColor = System.Drawing.Color.White;
            this.tbDisplayTotalText.EndCharacter = null;
            this.tbDisplayTotalText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbDisplayTotalText.LastTrack = null;
            resources.ApplyResources(this.tbDisplayTotalText, "tbDisplayTotalText");
            this.tbDisplayTotalText.ManualEntryOfTrack = true;
            this.tbDisplayTotalText.MaxLength = 32767;
            this.tbDisplayTotalText.Name = "tbDisplayTotalText";
            this.tbDisplayTotalText.NumericOnly = false;
            this.tbDisplayTotalText.Seperator = null;
            this.tbDisplayTotalText.StartCharacter = null;
            this.tbDisplayTotalText.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // chkLineDisplayBinaryConversion
            // 
            resources.ApplyResources(this.chkLineDisplayBinaryConversion, "chkLineDisplayBinaryConversion");
            this.chkLineDisplayBinaryConversion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.chkLineDisplayBinaryConversion.Name = "chkLineDisplayBinaryConversion";
            // 
            // tbTillClosedLine1
            // 
            this.tbTillClosedLine1.BackColor = System.Drawing.Color.White;
            this.tbTillClosedLine1.EndCharacter = null;
            this.tbTillClosedLine1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbTillClosedLine1.LastTrack = null;
            resources.ApplyResources(this.tbTillClosedLine1, "tbTillClosedLine1");
            this.tbTillClosedLine1.ManualEntryOfTrack = true;
            this.tbTillClosedLine1.MaxLength = 32767;
            this.tbTillClosedLine1.Name = "tbTillClosedLine1";
            this.tbTillClosedLine1.NumericOnly = false;
            this.tbTillClosedLine1.Seperator = null;
            this.tbTillClosedLine1.StartCharacter = null;
            this.tbTillClosedLine1.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // SetLineDisplayPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblCharset);
            this.Controls.Add(this.ntbCharset);
            this.Controls.Add(this.lblTillClosedLine2);
            this.Controls.Add(this.tbTillClosedLine2);
            this.Controls.Add(this.cmbDisplay);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.lblDisplayBalanceText);
            this.Controls.Add(this.tbDisplayBalanceText);
            this.Controls.Add(this.lblDisplayTotalText);
            this.Controls.Add(this.tbDisplayTotalText);
            this.Controls.Add(this.chkLineDisplayBinaryConversion);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.lblDeiviceName);
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.lblTillClosedLine1);
            this.Controls.Add(this.tbTillClosedLine1);
            this.Name = "SetLineDisplayPage";
            this.Load += new System.EventHandler(this.SetLineDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDisplayBalanceText;
        private LSOne.Controls.MSRTextBoxTouch tbDisplayBalanceText;
        private System.Windows.Forms.Label lblDisplayTotalText;
        private LSOne.Controls.MSRTextBoxTouch tbDisplayTotalText;
        private LSOne.Controls.TouchCheckBox chkLineDisplayBinaryConversion;
        private LSOne.Controls.DualDataComboBox cmbDeviceName;
        private System.Windows.Forms.Label lblDeiviceName;
        private System.Windows.Forms.Label lblTillClosedLine1;
        private LSOne.Controls.MSRTextBoxTouch tbTillClosedLine1;
        private System.Windows.Forms.Label lblCharset;
        private LSOne.Controls.MSRTextBoxTouch ntbCharset;
        private System.Windows.Forms.Label lblTillClosedLine2;
        private LSOne.Controls.MSRTextBoxTouch tbTillClosedLine2;
        private LSOne.Controls.DualDataComboBox cmbDisplay;
        private System.Windows.Forms.Label lblDisplay;
        private LSOne.Controls.TouchButton btnTest;
        private LSOne.Peripherals.TouchErrorProvider errorProvider;
    }
}
