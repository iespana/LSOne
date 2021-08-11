using LSOne.Controls;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.DialogPanels
{
    partial class TouristInformationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TouristInformationPanel));
            this.tbName = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblTouristName = new System.Windows.Forms.Label();
            this.tdbHeader = new LSOne.Controls.TouchDialogBanner();
            this.label1 = new System.Windows.Forms.Label();
            this.tbEmail = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblEmail = new System.Windows.Forms.Label();
            this.cmbNationality = new LSOne.Controls.DualDataComboBox();
            this.actTourist = new LSOne.Controls.AddressControlTouch();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.BackColor = System.Drawing.Color.White;
            this.tbName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbName, "tbName");
            this.tbName.MaxLength = 60;
            this.tbName.Name = "tbName";
            // 
            // lblTouristName
            // 
            resources.ApplyResources(this.lblTouristName, "lblTouristName");
            this.lblTouristName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTouristName.Name = "lblTouristName";
            // 
            // tdbHeader
            // 
            this.tdbHeader.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdbHeader, "tdbHeader");
            this.tdbHeader.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbHeader.Name = "tdbHeader";
            this.tdbHeader.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label1.Name = "label1";
            // 
            // tbEmail
            // 
            this.tbEmail.BackColor = System.Drawing.Color.White;
            this.tbEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbEmail, "tbEmail");
            this.tbEmail.MaxLength = 60;
            this.tbEmail.Name = "tbEmail";
            // 
            // lblEmail
            // 
            resources.ApplyResources(this.lblEmail, "lblEmail");
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblEmail.Name = "lblEmail";
            // 
            // cmbNationality
            // 
            this.cmbNationality.AddList = null;
            this.cmbNationality.AllowKeyboardSelection = false;
            this.cmbNationality.EnableTextBox = true;
            resources.ApplyResources(this.cmbNationality, "cmbNationality");
            this.cmbNationality.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbNationality.IsPOSControl = true;
            this.cmbNationality.MaxLength = 32767;
            this.cmbNationality.Name = "cmbNationality";
            this.cmbNationality.NoChangeAllowed = false;
            this.cmbNationality.OnlyDisplayID = false;
            this.cmbNationality.RemoveList = null;
            this.cmbNationality.RowHeight = ((short)(50));
            this.cmbNationality.SecondaryData = null;
            this.cmbNationality.SelectedData = null;
            this.cmbNationality.SelectedDataID = null;
            this.cmbNationality.SelectionList = null;
            this.cmbNationality.ShowDropDownOnTyping = true;
            this.cmbNationality.SkipIDColumn = true;
            this.cmbNationality.Touch = true;
            this.cmbNationality.RequestData += new System.EventHandler(this.cmbNationality_RequestData);
            // 
            // actTourist
            // 
            this.actTourist.AddressFormatChangeable = true;
            this.actTourist.BackColor = System.Drawing.Color.Transparent;
            this.actTourist.DataModel = null;
            this.actTourist.FocusedText = "";
            resources.ApplyResources(this.actTourist, "actTourist");
            this.actTourist.Name = "actTourist";
            this.actTourist.States = null;
            // 
            // TouristInformationPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbNationality);
            this.Controls.Add(this.actTourist);
            this.Controls.Add(this.tdbHeader);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblTouristName);
            this.Name = "TouristInformationPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.ShadeTextBoxTouch tbName;
        private System.Windows.Forms.Label lblTouristName;
        private TouchDialogBanner tdbHeader;
        private LSOne.Controls.DualDataComboBox cmbNationality;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.ShadeTextBoxTouch tbEmail;
        private System.Windows.Forms.Label lblEmail;
        private AddressControlTouch actTourist;
    }
}
