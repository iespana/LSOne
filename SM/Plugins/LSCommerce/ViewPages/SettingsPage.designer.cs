using LSOne.Controls;

namespace LSOne.ViewPlugins.LSCommerce.ViewPages
{
    partial class SettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPage));
            this.tbAppID = new System.Windows.Forms.TextBox();
            this.tbDeviceID = new System.Windows.Forms.TextBox();
            this.tbLicenseKey = new System.Windows.Forms.TextBox();
            this.lblAppID = new System.Windows.Forms.Label();
            this.lblDeviceID = new System.Windows.Forms.Label();
            this.lblLicenseKey = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbAppID
            // 
            resources.ApplyResources(this.tbAppID, "tbAppID");
            this.tbAppID.Name = "tbAppID";
            this.tbAppID.ReadOnly = true;
            // 
            // tbDeviceID
            // 
            resources.ApplyResources(this.tbDeviceID, "tbDeviceID");
            this.tbDeviceID.Name = "tbDeviceID";
            this.tbDeviceID.ReadOnly = true;
            // 
            // tbLicenseKey
            // 
            resources.ApplyResources(this.tbLicenseKey, "tbLicenseKey");
            this.tbLicenseKey.Name = "tbLicenseKey";
            // 
            // lblAppID
            // 
            resources.ApplyResources(this.lblAppID, "lblAppID");
            this.lblAppID.Name = "lblAppID";
            // 
            // lblDeviceID
            // 
            resources.ApplyResources(this.lblDeviceID, "lblDeviceID");
            this.lblDeviceID.Name = "lblDeviceID";
            // 
            // lblLicenseKey
            // 
            resources.ApplyResources(this.lblLicenseKey, "lblLicenseKey");
            this.lblLicenseKey.Name = "lblLicenseKey";
            // 
            // SettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblLicenseKey);
            this.Controls.Add(this.lblDeviceID);
            this.Controls.Add(this.lblAppID);
            this.Controls.Add(this.tbLicenseKey);
            this.Controls.Add(this.tbDeviceID);
            this.Controls.Add(this.tbAppID);
            this.DoubleBuffered = true;
            this.Name = "SettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAppID;
        private System.Windows.Forms.TextBox tbDeviceID;
        private System.Windows.Forms.TextBox tbLicenseKey;
        private System.Windows.Forms.Label lblAppID;
        private System.Windows.Forms.Label lblDeviceID;
        private System.Windows.Forms.Label lblLicenseKey;
    }
}
