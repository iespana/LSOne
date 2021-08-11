using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    partial class NewUserADIdentityPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUserADIdentityPage));
            this.label1 = new System.Windows.Forms.Label();
            this.tbLoginName = new System.Windows.Forms.TextBox();
            this.lblLoginName = new System.Windows.Forms.Label();
            this.fullNameField1 = new LSOne.Controls.FullNameControl();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.errorProvider4 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.errorProvider1.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider4.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment1"))));
            this.label1.Name = "label1";
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
            this.errorProvider1.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment"))));
            this.errorProvider4.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment1"))));
            resources.ApplyResources(this.lblLoginName, "lblLoginName");
            this.lblLoginName.Name = "lblLoginName";
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
            this.errorProvider1.SetIconAlignment(this.lblEmail, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblEmail.IconAlignment"))));
            this.errorProvider4.SetIconAlignment(this.lblEmail, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblEmail.IconAlignment1"))));
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
            // errorProvider4
            // 
            this.errorProvider4.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // NewUserADIdentityPage
            // 
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.tbLoginName);
            this.Controls.Add(this.lblLoginName);
            this.Controls.Add(this.fullNameField1);
            this.Controls.Add(this.label1);
            this.errorProvider4.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment"))));
            this.errorProvider1.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment1"))));
            this.Name = "NewUserADIdentityPage";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private FullNameControl fullNameField1;
        private System.Windows.Forms.TextBox tbLoginName;
        private System.Windows.Forms.Label lblLoginName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider4;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.Label lblEmail;
    }
}
