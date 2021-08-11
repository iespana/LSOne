using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerDescriptionPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerDescriptionPanel));
            this.tbDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.fullNameControlTouch = new LSOne.Controls.FullNameControlTouch();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnVerify = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSearchAlias = new System.Windows.Forms.TextBox();
            this.lblReceiptEmail = new System.Windows.Forms.Label();
            this.tbReceiptEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.cmbReceiptOption = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbDisplayName
            // 
            resources.ApplyResources(this.tbDisplayName, "tbDisplayName");
            this.tbDisplayName.Name = "tbDisplayName";
            this.tbDisplayName.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // lblDisplayName
            // 
            resources.ApplyResources(this.lblDisplayName, "lblDisplayName");
            this.lblDisplayName.Name = "lblDisplayName";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // fullNameControlTouch
            // 
            resources.ApplyResources(this.fullNameControlTouch, "fullNameControlTouch");
            this.fullNameControlTouch.FirstName = "";
            this.fullNameControlTouch.FocusedText = "";
            this.fullNameControlTouch.LastName = "";
            this.fullNameControlTouch.MiddleName = "";
            this.fullNameControlTouch.Name = "fullNameControlTouch";
            this.fullNameControlTouch.Prefix = "";
            this.fullNameControlTouch.Suffix = "";
            this.fullNameControlTouch.ValueChanged += new System.EventHandler(this.fullNameControlTouch_ValueChanged);
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.Icon = ((System.Drawing.Image)(resources.GetObject("touchDialogBanner1.Icon")));
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnVerify
            // 
            resources.ApplyResources(this.btnVerify, "btnVerify");
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbSearchAlias
            // 
            resources.ApplyResources(this.tbSearchAlias, "tbSearchAlias");
            this.tbSearchAlias.Name = "tbSearchAlias";
            this.tbSearchAlias.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // lblReceiptEmail
            // 
            resources.ApplyResources(this.lblReceiptEmail, "lblReceiptEmail");
            this.lblReceiptEmail.Name = "lblReceiptEmail";
            // 
            // tbReceiptEmail
            // 
            resources.ApplyResources(this.tbReceiptEmail, "tbReceiptEmail");
            this.tbReceiptEmail.Name = "tbReceiptEmail";
            this.tbReceiptEmail.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // lblEmail
            // 
            resources.ApplyResources(this.lblEmail, "lblEmail");
            this.lblEmail.Name = "lblEmail";
            // 
            // tbEmail
            // 
            resources.ApplyResources(this.tbEmail, "tbEmail");
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // lblPhone
            // 
            resources.ApplyResources(this.lblPhone, "lblPhone");
            this.lblPhone.Name = "lblPhone";
            // 
            // tbPhone
            // 
            resources.ApplyResources(this.tbPhone, "tbPhone");
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.TextChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // cmbReceiptOption
            // 
            this.cmbReceiptOption.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("cmbReceiptOption.AutoCompleteCustomSource"),
            resources.GetString("cmbReceiptOption.AutoCompleteCustomSource1"),
            resources.GetString("cmbReceiptOption.AutoCompleteCustomSource2")});
            this.cmbReceiptOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbReceiptOption, "cmbReceiptOption");
            this.cmbReceiptOption.FormattingEnabled = true;
            this.cmbReceiptOption.Items.AddRange(new object[] {
            resources.GetString("cmbReceiptOption.Items"),
            resources.GetString("cmbReceiptOption.Items1"),
            resources.GetString("cmbReceiptOption.Items2")});
            this.cmbReceiptOption.Name = "cmbReceiptOption";
            this.cmbReceiptOption.SelectedIndexChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CustomerDescriptionPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbReceiptOption);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblReceiptEmail);
            this.Controls.Add(this.tbReceiptEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSearchAlias);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.tbDisplayName);
            this.Controls.Add(this.lblDisplayName);
            this.Controls.Add(this.fullNameControlTouch);
            this.DoubleBuffered = true;
            this.Name = "CustomerDescriptionPanel";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private FullNameControlTouch fullNameControlTouch;
        private LinkFields linkFields1;
        private TouchDialogBanner touchDialogBanner1;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSearchAlias;
        private System.Windows.Forms.Label lblReceiptEmail;
        private System.Windows.Forms.TextBox tbReceiptEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.ComboBox cmbReceiptOption;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
