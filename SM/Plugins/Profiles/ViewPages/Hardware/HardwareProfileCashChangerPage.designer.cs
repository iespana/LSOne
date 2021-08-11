namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfileCashChangerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfileCashChangerPage));
            this.chkDeviceConnected = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPortSettings = new System.Windows.Forms.TextBox();
            this.tbInitSettings = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkDeviceConnected
            // 
            resources.ApplyResources(this.chkDeviceConnected, "chkDeviceConnected");
            this.chkDeviceConnected.Name = "chkDeviceConnected";
            this.chkDeviceConnected.UseVisualStyleBackColor = true;
            this.chkDeviceConnected.CheckedChanged += new System.EventHandler(this.chkDeviceConnected_CheckedChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbPortSettings
            // 
            resources.ApplyResources(this.tbPortSettings, "tbPortSettings");
            this.tbPortSettings.Name = "tbPortSettings";
            // 
            // tbInitSettings
            // 
            resources.ApplyResources(this.tbInitSettings, "tbInitSettings");
            this.tbInitSettings.Name = "tbInitSettings";
            // 
            // HardwareProfileCashChangerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbInitSettings);
            this.Controls.Add(this.tbPortSettings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkDeviceConnected);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfileCashChangerPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDeviceConnected;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPortSettings;
        private System.Windows.Forms.TextBox tbInitSettings;


    }
}
