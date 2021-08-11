namespace LSOne.ViewPlugins.SiteService.Dialogs
{
	partial class AuthorizationDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorizationDialog));
			this.lblPassword = new System.Windows.Forms.Label();
			this.mtxtPassword = new System.Windows.Forms.MaskedTextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblPassword
			// 
			this.lblPassword.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblPassword, "lblPassword");
			this.lblPassword.Name = "lblPassword";
			// 
			// mtxtPassword
			// 
			this.mtxtPassword.Culture = new System.Globalization.CultureInfo("");
			resources.ApplyResources(this.mtxtPassword, "mtxtPassword");
			this.mtxtPassword.Name = "mtxtPassword";
			this.mtxtPassword.PasswordChar = '*';
			this.mtxtPassword.TextChanged += new System.EventHandler(this.mtxtPassword_TextChanged);
			// 
			// panel2
			// 
			resources.ApplyResources(this.panel2, "panel2");
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.panel2.Controls.Add(this.btnOK);
			this.panel2.Controls.Add(this.btnCancel);
			this.panel2.Name = "panel2";
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// AuthorizationDialog
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.mtxtPassword);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HasHelp = true;
			this.Name = "AuthorizationDialog";
			this.ShowInTaskbar = false;
			this.Shown += new System.EventHandler(this.AuthenticationDialog_Shown);
			this.Controls.SetChildIndex(this.mtxtPassword, 0);
			this.Controls.SetChildIndex(this.lblPassword, 0);
			this.Controls.SetChildIndex(this.panel2, 0);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.MaskedTextBox mtxtPassword;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}