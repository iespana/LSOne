using LSOne.Controls;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerDetailsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerDetailsPanel));
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReceiptEmail = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.cmbTitle = new LSOne.Controls.DualDataComboBox();
            this.cmbReceiptOption = new LSOne.Controls.DualDataComboBox();
            this.btnVerify = new LSOne.Controls.TouchButton();
            this.tbDisplayName = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbID = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbSearchAlias = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbPhone = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbEmail = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbReceiptEmail = new LSOne.Controls.ShadeTextBoxTouch();
            this.touchErrorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.tbLastName = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbFirstName = new LSOne.Controls.ShadeTextBoxTouch();
            this.SuspendLayout();
            // 
            // lblDisplayName
            // 
            resources.ApplyResources(this.lblDisplayName, "lblDisplayName");
            this.lblDisplayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDisplayName.Name = "lblDisplayName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblReceiptEmail
            // 
            resources.ApplyResources(this.lblReceiptEmail, "lblReceiptEmail");
            this.lblReceiptEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblReceiptEmail.Name = "lblReceiptEmail";
            // 
            // lblEmail
            // 
            resources.ApplyResources(this.lblEmail, "lblEmail");
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblEmail.Name = "lblEmail";
            // 
            // lblPhone
            // 
            resources.ApplyResources(this.lblPhone, "lblPhone");
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPhone.Name = "lblPhone";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label3.Name = "label3";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTitle.Name = "lblTitle";
            // 
            // lblFirstName
            // 
            resources.ApplyResources(this.lblFirstName, "lblFirstName");
            this.lblFirstName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFirstName.Name = "lblFirstName";
            // 
            // lblLastName
            // 
            resources.ApplyResources(this.lblLastName, "lblLastName");
            this.lblLastName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblLastName.Name = "lblLastName";
            // 
            // cmbTitle
            // 
            this.cmbTitle.AddList = null;
            this.cmbTitle.AllowKeyboardSelection = false;
            this.cmbTitle.EnableTextBox = true;
            resources.ApplyResources(this.cmbTitle, "cmbTitle");
            this.cmbTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbTitle.IsPOSControl = true;
            this.cmbTitle.MaxLength = 32767;
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.NoChangeAllowed = false;
            this.cmbTitle.OnlyDisplayID = false;
            this.cmbTitle.ReadOnly = true;
            this.cmbTitle.RemoveList = null;
            this.cmbTitle.RowHeight = ((short)(22));
            this.cmbTitle.SecondaryData = null;
            this.cmbTitle.SelectedData = null;
            this.cmbTitle.SelectedDataID = null;
            this.cmbTitle.SelectionList = null;
            this.cmbTitle.ShowDropDownOnTyping = true;
            this.cmbTitle.SkipIDColumn = true;
            this.cmbTitle.Touch = true;
            this.cmbTitle.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTitle_DropDown);
            this.cmbTitle.RequestClear += new System.EventHandler(this.cmbTitle_RequestClear);
            // 
            // cmbReceiptOption
            // 
            this.cmbReceiptOption.AddList = null;
            this.cmbReceiptOption.AllowKeyboardSelection = false;
            this.cmbReceiptOption.EnableTextBox = true;
            resources.ApplyResources(this.cmbReceiptOption, "cmbReceiptOption");
            this.cmbReceiptOption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbReceiptOption.IsPOSControl = true;
            this.cmbReceiptOption.MaxLength = 32767;
            this.cmbReceiptOption.Name = "cmbReceiptOption";
            this.cmbReceiptOption.NoChangeAllowed = false;
            this.cmbReceiptOption.OnlyDisplayID = false;
            this.cmbReceiptOption.ReadOnly = true;
            this.cmbReceiptOption.RemoveList = null;
            this.cmbReceiptOption.RowHeight = ((short)(22));
            this.cmbReceiptOption.SecondaryData = null;
            this.cmbReceiptOption.SelectedData = null;
            this.cmbReceiptOption.SelectedDataID = null;
            this.cmbReceiptOption.SelectionList = null;
            this.cmbReceiptOption.ShowDropDownOnTyping = true;
            this.cmbReceiptOption.SkipIDColumn = true;
            this.cmbReceiptOption.Touch = true;
            this.cmbReceiptOption.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbReceiptOption_DropDown);
            // 
            // btnVerify
            // 
            this.btnVerify.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnVerify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnVerify.BackgroundImage = global::LSOne.Services.Properties.Resources.Checkmark_white_32px;
            resources.ApplyResources(this.btnVerify, "btnVerify");
            this.btnVerify.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnVerify.DrawBorder = false;
            this.btnVerify.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Image = global::LSOne.Services.Properties.Resources.verifyID_24_black;
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.UseVisualStyleBackColor = false;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // tbDisplayName
            // 
            this.tbDisplayName.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbDisplayName, "tbDisplayName");
            this.tbDisplayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbDisplayName.MaxLength = 60;
            this.tbDisplayName.Name = "tbDisplayName";
            this.tbDisplayName.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.Color.White;
            this.tbID.Cursor = System.Windows.Forms.Cursors.Default;
            this.tbID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.MaxLength = 20;
            this.tbID.Name = "tbID";
            // 
            // tbSearchAlias
            // 
            this.tbSearchAlias.BackColor = System.Drawing.Color.White;
            this.tbSearchAlias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbSearchAlias, "tbSearchAlias");
            this.tbSearchAlias.MaxLength = 80;
            this.tbSearchAlias.Name = "tbSearchAlias";
            this.tbSearchAlias.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // tbPhone
            // 
            this.tbPhone.BackColor = System.Drawing.Color.White;
            this.tbPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbPhone, "tbPhone");
            this.tbPhone.MaxLength = 80;
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // tbEmail
            // 
            this.tbEmail.BackColor = System.Drawing.Color.White;
            this.tbEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbEmail, "tbEmail");
            this.tbEmail.MaxLength = 80;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // tbReceiptEmail
            // 
            this.tbReceiptEmail.BackColor = System.Drawing.Color.White;
            this.tbReceiptEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbReceiptEmail, "tbReceiptEmail");
            this.tbReceiptEmail.MaxLength = 80;
            this.tbReceiptEmail.Name = "tbReceiptEmail";
            this.tbReceiptEmail.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // touchErrorProvider
            // 
            resources.ApplyResources(this.touchErrorProvider, "touchErrorProvider");
            this.touchErrorProvider.BackColor = System.Drawing.Color.White;
            this.touchErrorProvider.Name = "touchErrorProvider";
            // 
            // tbLastName
            // 
            this.tbLastName.BackColor = System.Drawing.Color.White;
            this.tbLastName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbLastName, "tbLastName");
            this.tbLastName.MaxLength = 60;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
            // 
            // tbFirstName
            // 
            this.tbFirstName.BackColor = System.Drawing.Color.White;
            this.tbFirstName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbFirstName, "tbFirstName");
            this.tbFirstName.MaxLength = 60;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.TextChanged += new System.EventHandler(this.tbFirstName_TextChanged);
            // 
            // CustomerDetailsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cmbTitle);
            this.Controls.Add(this.cmbReceiptOption);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.lblDisplayName);
            this.Controls.Add(this.tbDisplayName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.tbSearchAlias);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.tbReceiptEmail);
            this.Controls.Add(this.lblReceiptEmail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.touchErrorProvider);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "CustomerDetailsPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.ShadeTextBoxTouch tbDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private LSOne.Controls.ShadeTextBoxTouch tbID;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.TouchButton btnVerify;
        private System.Windows.Forms.Label label2;
        private LSOne.Controls.ShadeTextBoxTouch tbSearchAlias;
        private System.Windows.Forms.Label lblReceiptEmail;
        private LSOne.Controls.ShadeTextBoxTouch tbReceiptEmail;
        private System.Windows.Forms.Label lblEmail;
        private LSOne.Controls.ShadeTextBoxTouch tbEmail;
        private System.Windows.Forms.Label lblPhone;
        private LSOne.Controls.ShadeTextBoxTouch tbPhone;
        private System.Windows.Forms.Label label3;
        private Controls.Dialogs.TouchErrorProvider touchErrorProvider;
        private System.Windows.Forms.Label lblTitle;
        private ShadeTextBoxTouch tbFirstName;
        private System.Windows.Forms.Label lblFirstName;
        private ShadeTextBoxTouch tbLastName;
        private System.Windows.Forms.Label lblLastName;
        private DualDataComboBox cmbReceiptOption;
        private DualDataComboBox cmbTitle;
    }
}
