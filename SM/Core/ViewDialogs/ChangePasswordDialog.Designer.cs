namespace LSOne.ViewCore.Dialogs
{
    partial class ChangePasswordDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOldPassword = new System.Windows.Forms.TextBox();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.tbConfirmPassword = new System.Windows.Forms.TextBox();
            this.chkNextLogon = new System.Windows.Forms.CheckBox();
            this.lblNextLogon = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnChange
            // 
            resources.ApplyResources(this.btnChange, "btnChange");
            this.btnChange.Name = "btnChange";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblNewPassword
            // 
            resources.ApplyResources(this.lblNewPassword, "lblNewPassword");
            this.lblNewPassword.Name = "lblNewPassword";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbOldPassword
            // 
            resources.ApplyResources(this.tbOldPassword, "tbOldPassword");
            this.tbOldPassword.Name = "tbOldPassword";
            this.tbOldPassword.UseSystemPasswordChar = true;
            this.tbOldPassword.TextChanged += new System.EventHandler(this.AnyTextBox_TextChanged);
            this.tbOldPassword.Validating += new System.ComponentModel.CancelEventHandler(this.AnyTextBox_Validating);
            // 
            // tbNewPassword
            // 
            resources.ApplyResources(this.tbNewPassword, "tbNewPassword");
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.UseSystemPasswordChar = true;
            this.tbNewPassword.TextChanged += new System.EventHandler(this.AnyTextBox_TextChanged);
            this.tbNewPassword.Validating += new System.ComponentModel.CancelEventHandler(this.AnyTextBox_Validating);
            // 
            // tbConfirmPassword
            // 
            resources.ApplyResources(this.tbConfirmPassword, "tbConfirmPassword");
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.UseSystemPasswordChar = true;
            this.tbConfirmPassword.TextChanged += new System.EventHandler(this.AnyTextBox_TextChanged);
            this.tbConfirmPassword.Validating += new System.ComponentModel.CancelEventHandler(this.AnyTextBox_Validating);
            // 
            // chkNextLogon
            // 
            resources.ApplyResources(this.chkNextLogon, "chkNextLogon");
            this.chkNextLogon.Name = "chkNextLogon";
            // 
            // lblNextLogon
            // 
            resources.ApplyResources(this.lblNextLogon, "lblNextLogon");
            this.lblNextLogon.Name = "lblNextLogon";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // ChangePasswordDialog
            // 
            this.AcceptButton = this.btnChange;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblNextLogon);
            this.Controls.Add(this.chkNextLogon);
            this.Controls.Add(this.tbConfirmPassword);
            this.Controls.Add(this.tbNewPassword);
            this.Controls.Add(this.tbOldPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "ChangePasswordDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnChange, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblNewPassword, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbOldPassword, 0);
            this.Controls.SetChildIndex(this.tbNewPassword, 0);
            this.Controls.SetChildIndex(this.tbConfirmPassword, 0);
            this.Controls.SetChildIndex(this.chkNextLogon, 0);
            this.Controls.SetChildIndex(this.lblNextLogon, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOldPassword;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.TextBox tbConfirmPassword;
        private System.Windows.Forms.CheckBox chkNextLogon;
        private System.Windows.Forms.Label lblNextLogon;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ErrorProvider errorProvider3;
        private System.Windows.Forms.Panel panel2;
    }
}