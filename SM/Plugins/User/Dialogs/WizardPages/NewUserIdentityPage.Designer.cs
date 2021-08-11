using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    partial class NewUserIdentityPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUserIdentityPage));
            this.label1 = new System.Windows.Forms.Label();
            this.tbLoginName = new System.Windows.Forms.TextBox();
            this.lblLoginName = new System.Windows.Forms.Label();
            this.tbConfirmPassword = new System.Windows.Forms.TextBox();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.fullNameField1 = new LSOne.Controls.FullNameControl();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider4 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider5 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider5)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.errorProvider2.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider1.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment1"))));
            this.errorProvider4.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment2"))));
            this.errorProvider3.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment3"))));
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tbLoginName
            // 
            resources.ApplyResources(this.tbLoginName, "tbLoginName");
            this.tbLoginName.Name = "tbLoginName";
            this.tbLoginName.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            this.tbLoginName.Validating += new System.ComponentModel.CancelEventHandler(this.tbLoginName_Validating);
            // 
            // lblLoginName
            // 
            this.errorProvider2.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment"))));
            this.errorProvider1.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment1"))));
            this.errorProvider4.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment2"))));
            this.errorProvider3.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment3"))));
            resources.ApplyResources(this.lblLoginName, "lblLoginName");
            this.lblLoginName.Name = "lblLoginName";
            // 
            // tbConfirmPassword
            // 
            resources.ApplyResources(this.tbConfirmPassword, "tbConfirmPassword");
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            this.tbConfirmPassword.Validating += new System.ComponentModel.CancelEventHandler(this.tbConfirmPassword_Validating);
            // 
            // tbNewPassword
            // 
            resources.ApplyResources(this.tbNewPassword, "tbNewPassword");
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            this.tbNewPassword.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewPassword_Validating);
            // 
            // label2
            // 
            this.errorProvider2.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider1.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment1"))));
            this.errorProvider4.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment2"))));
            this.errorProvider3.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment3"))));
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblNewPassword
            // 
            this.errorProvider2.SetIconAlignment(this.lblNewPassword, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblNewPassword.IconAlignment"))));
            this.errorProvider1.SetIconAlignment(this.lblNewPassword, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblNewPassword.IconAlignment1"))));
            this.errorProvider4.SetIconAlignment(this.lblNewPassword, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblNewPassword.IconAlignment2"))));
            this.errorProvider3.SetIconAlignment(this.lblNewPassword, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblNewPassword.IconAlignment3"))));
            resources.ApplyResources(this.lblNewPassword, "lblNewPassword");
            this.lblNewPassword.Name = "lblNewPassword";
            // 
            // fullNameField1
            // 
            this.fullNameField1.Alias = "";
            resources.ApplyResources(this.fullNameField1, "fullNameField1");
            this.fullNameField1.BackColor = System.Drawing.Color.Transparent;
            this.fullNameField1.FirstName = "";
            this.fullNameField1.LastName = "";
            this.fullNameField1.MiddleName = "";
            this.fullNameField1.Name = "fullNameField1";
            this.fullNameField1.Prefix = "";
            this.fullNameField1.ShowAlias = false;
            this.fullNameField1.Suffix = "";
            this.fullNameField1.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            this.fullNameField1.Validating += new System.ComponentModel.CancelEventHandler(this.fullNameField1_Validating);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblEmail
            // 
            this.errorProvider2.SetIconAlignment(this.lblEmail, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblEmail.IconAlignment"))));
            this.errorProvider1.SetIconAlignment(this.lblEmail, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblEmail.IconAlignment1"))));
            this.errorProvider4.SetIconAlignment(this.lblEmail, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblEmail.IconAlignment2"))));
            this.errorProvider3.SetIconAlignment(this.lblEmail, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblEmail.IconAlignment3"))));
            resources.ApplyResources(this.lblEmail, "lblEmail");
            this.lblEmail.Name = "lblEmail";
            // 
            // txtEmail
            // 
            resources.ApplyResources(this.txtEmail, "txtEmail");
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // errorProvider4
            // 
            this.errorProvider4.ContainerControl = this;
            // 
            // errorProvider5
            // 
            this.errorProvider5.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // NewUserIdentityPage
            // 
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.tbConfirmPassword);
            this.Controls.Add(this.tbNewPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.tbLoginName);
            this.Controls.Add(this.lblLoginName);
            this.Controls.Add(this.fullNameField1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.errorProvider3.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment"))));
            this.errorProvider4.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment1"))));
            this.errorProvider1.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment2"))));
            this.errorProvider2.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment3"))));
            this.Name = "NewUserIdentityPage";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private FullNameControl fullNameField1;
        private System.Windows.Forms.TextBox tbLoginName;
        private System.Windows.Forms.Label lblLoginName;
        private System.Windows.Forms.TextBox tbConfirmPassword;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ErrorProvider errorProvider3;
        private System.Windows.Forms.ErrorProvider errorProvider4;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.ErrorProvider errorProvider5;
        private System.Windows.Forms.Panel panel1;
    }
}
