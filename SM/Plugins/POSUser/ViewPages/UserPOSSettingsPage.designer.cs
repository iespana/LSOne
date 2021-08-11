using LSOne.Controls;

namespace LSOne.ViewPlugins.POSUser.ViewPages
{
    partial class UserPOSSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserPOSSettingsPage));
            this.labelUserProfile = new System.Windows.Forms.Label();
            this.tbNameOnReceipt = new System.Windows.Forms.TextBox();
            this.labelNameOnReceipt = new System.Windows.Forms.Label();
            this.btnEditEmail = new LSOne.Controls.ContextButton();
            this.btnEditUserProfile = new LSOne.Controls.ContextButton();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbUserProfile = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
            // 
            // labelUserProfile
            // 
            this.labelUserProfile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelUserProfile, "labelUserProfile");
            this.labelUserProfile.Name = "labelUserProfile";
            // 
            // tbNameOnReceipt
            // 
            this.tbNameOnReceipt.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbNameOnReceipt, "tbNameOnReceipt");
            this.tbNameOnReceipt.Name = "tbNameOnReceipt";
            // 
            // labelNameOnReceipt
            // 
            this.labelNameOnReceipt.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelNameOnReceipt, "labelNameOnReceipt");
            this.labelNameOnReceipt.Name = "labelNameOnReceipt";
            // 
            // btnEditEmail
            // 
            this.btnEditEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnEditEmail.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditEmail, "btnEditEmail");
            this.btnEditEmail.Name = "btnEditEmail";
            this.btnEditEmail.Click += new System.EventHandler(this.btnEditEmail_Click);
            // 
            // btnEditUserProfile
            // 
            this.btnEditUserProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnEditUserProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditUserProfile, "btnEditUserProfile");
            this.btnEditUserProfile.Name = "btnEditUserProfile";
            this.btnEditUserProfile.Click += new System.EventHandler(this.btnEditUserProfile_Click);
            // 
            // txtEmail
            // 
            resources.ApplyResources(this.txtEmail, "txtEmail");
            this.txtEmail.Name = "txtEmail";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbUserProfile
            // 
            this.cmbUserProfile.AddList = null;
            this.cmbUserProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUserProfile, "cmbUserProfile");
            this.cmbUserProfile.MaxLength = 32767;
            this.cmbUserProfile.Name = "cmbUserProfile";
            this.cmbUserProfile.NoChangeAllowed = false;
            this.cmbUserProfile.OnlyDisplayID = false;
            this.cmbUserProfile.RemoveList = null;
            this.cmbUserProfile.RowHeight = ((short)(22));
            this.cmbUserProfile.SecondaryData = null;
            this.cmbUserProfile.SelectedData = null;
            this.cmbUserProfile.SelectedDataID = null;
            this.cmbUserProfile.SelectionList = null;
            this.cmbUserProfile.SkipIDColumn = true;
            this.cmbUserProfile.RequestData += new System.EventHandler(this.cmbUserProfile_RequestData);
            this.cmbUserProfile.SelectedDataChanged += new System.EventHandler(this.cmbUserProfile_SelectedDataChanged);
            // 
            // UserPOSSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditEmail);
            this.Controls.Add(this.tbNameOnReceipt);
            this.Controls.Add(this.labelUserProfile);
            this.Controls.Add(this.btnEditUserProfile);
            this.Controls.Add(this.cmbUserProfile);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.labelNameOnReceipt);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.Name = "UserPOSSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DualDataComboBox cmbUserProfile;
        private System.Windows.Forms.Label labelUserProfile;
        private System.Windows.Forms.TextBox tbNameOnReceipt;
        private System.Windows.Forms.Label labelNameOnReceipt;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label2;
        private ContextButton btnEditUserProfile;
        private ContextButton btnEditEmail;
    }
}
