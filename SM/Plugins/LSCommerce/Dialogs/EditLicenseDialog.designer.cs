using LSOne.Controls;

namespace LSOne.ViewPlugins.LSCommerce.Dialogs
{
    partial class EditLicenseDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditLicenseDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblLicenseKey = new System.Windows.Forms.Label();
            this.lblDeviceID = new System.Windows.Forms.Label();
            this.lblAppID = new System.Windows.Forms.Label();
            this.tbLicenseKey = new System.Windows.Forms.TextBox();
            this.tbDeviceID = new System.Windows.Forms.TextBox();
            this.tbAppID = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
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
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblLicenseKey
            // 
            resources.ApplyResources(this.lblLicenseKey, "lblLicenseKey");
            this.lblLicenseKey.Name = "lblLicenseKey";
            // 
            // lblDeviceID
            // 
            resources.ApplyResources(this.lblDeviceID, "lblDeviceID");
            this.lblDeviceID.Name = "lblDeviceID";
            // 
            // lblAppID
            // 
            resources.ApplyResources(this.lblAppID, "lblAppID");
            this.lblAppID.Name = "lblAppID";
            // 
            // tbLicenseKey
            // 
            resources.ApplyResources(this.tbLicenseKey, "tbLicenseKey");
            this.tbLicenseKey.Name = "tbLicenseKey";
            this.tbLicenseKey.TextChanged += new System.EventHandler(this.tbLicenseKey_TextChanged);
            // 
            // tbDeviceID
            // 
            resources.ApplyResources(this.tbDeviceID, "tbDeviceID");
            this.tbDeviceID.Name = "tbDeviceID";
            this.tbDeviceID.ReadOnly = true;
            // 
            // tbAppID
            // 
            resources.ApplyResources(this.tbAppID, "tbAppID");
            this.tbAppID.Name = "tbAppID";
            this.tbAppID.ReadOnly = true;
            // 
            // EditLicenseDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblLicenseKey);
            this.Controls.Add(this.lblDeviceID);
            this.Controls.Add(this.lblAppID);
            this.Controls.Add(this.tbLicenseKey);
            this.Controls.Add(this.tbDeviceID);
            this.Controls.Add(this.tbAppID);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "EditLicenseDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbAppID, 0);
            this.Controls.SetChildIndex(this.tbDeviceID, 0);
            this.Controls.SetChildIndex(this.tbLicenseKey, 0);
            this.Controls.SetChildIndex(this.lblAppID, 0);
            this.Controls.SetChildIndex(this.lblDeviceID, 0);
            this.Controls.SetChildIndex(this.lblLicenseKey, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblLicenseKey;
        private System.Windows.Forms.Label lblDeviceID;
        private System.Windows.Forms.Label lblAppID;
        private System.Windows.Forms.TextBox tbLicenseKey;
        private System.Windows.Forms.TextBox tbDeviceID;
        private System.Windows.Forms.TextBox tbAppID;
    }
}