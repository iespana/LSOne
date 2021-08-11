using LSOne.Controls;

namespace LSOne.ViewPlugins.Pharmacy.ViewPages
{
    partial class HardwareProfilePharmacyPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfilePharmacyPage));
			this.lblActive = new System.Windows.Forms.Label();
			this.chkPharmacyActive = new System.Windows.Forms.CheckBox();
			this.lblHost = new System.Windows.Forms.Label();
			this.ntbPort = new LSOne.Controls.NumericTextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblActive
			// 
			this.lblActive.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblActive, "lblActive");
			this.lblActive.Name = "lblActive";
			// 
			// chkPharmacyActive
			// 
			resources.ApplyResources(this.chkPharmacyActive, "chkPharmacyActive");
			this.chkPharmacyActive.Name = "chkPharmacyActive";
			this.chkPharmacyActive.UseVisualStyleBackColor = true;
			this.chkPharmacyActive.CheckedChanged += new System.EventHandler(this.chkPharmacyActive_CheckedChanged);
			// 
			// lblHost
			// 
			this.lblHost.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblHost, "lblHost");
			this.lblHost.Name = "lblHost";
			// 
			// ntbPort
			// 
			this.ntbPort.AllowDecimal = false;
			this.ntbPort.AllowNegative = false;
			this.ntbPort.CultureInfo = null;
			this.ntbPort.DecimalLetters = 2;
			this.ntbPort.ForeColor = System.Drawing.Color.Black;
			this.ntbPort.HasMinValue = false;
			resources.ApplyResources(this.ntbPort, "ntbPort");
			this.ntbPort.MaxValue = 65535D;
			this.ntbPort.MinValue = 0D;
			this.ntbPort.Name = "ntbPort";
			this.ntbPort.Value = 0D;
			// 
			// lblPort
			// 
			this.lblPort.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblPort, "lblPort");
			this.lblPort.Name = "lblPort";
			// 
			// tbHost
			// 
			resources.ApplyResources(this.tbHost, "tbHost");
			this.tbHost.Name = "tbHost";
			// 
			// HardwareProfilePharmacyPage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.tbHost);
			this.Controls.Add(this.ntbPort);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.lblHost);
			this.Controls.Add(this.lblActive);
			this.Controls.Add(this.chkPharmacyActive);
			this.DoubleBuffered = true;
			this.Name = "HardwareProfilePharmacyPage";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.CheckBox chkPharmacyActive;
        private System.Windows.Forms.Label lblHost;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox tbHost;
    }
}
