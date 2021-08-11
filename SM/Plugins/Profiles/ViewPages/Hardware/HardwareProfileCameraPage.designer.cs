using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfileCameraPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfileCameraPage));
            this.chkCameraOn = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.ntbPort = new LSOne.Controls.NumericTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCamera = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkCameraOn
            // 
            resources.ApplyResources(this.chkCameraOn, "chkCameraOn");
            this.chkCameraOn.Name = "chkCameraOn";
            this.chkCameraOn.UseVisualStyleBackColor = true;
            this.chkCameraOn.CheckedChanged += new System.EventHandler(this.chkCameraOn_CheckedChanged);
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
            // tbServer
            // 
            resources.ApplyResources(this.tbServer, "tbServer");
            this.tbServer.Name = "tbServer";
            // 
            // ntbPort
            // 
            this.ntbPort.AllowDecimal = false;
            this.ntbPort.AllowNegative = false;
            this.ntbPort.CultureInfo = null;
            this.ntbPort.DecimalLetters = 2;
            this.ntbPort.HasMinValue = false;
            resources.ApplyResources(this.ntbPort, "ntbPort");
            this.ntbPort.MaxValue = 65535D;
            this.ntbPort.MinValue = 0D;
            this.ntbPort.Name = "ntbPort";
            this.ntbPort.Value = 0D;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbCamera
            // 
            resources.ApplyResources(this.tbCamera, "tbCamera");
            this.tbCamera.Name = "tbCamera";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // HardwareProfileCameraPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbCamera);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkCameraOn);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfileCameraPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCameraOn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbServer;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbCamera;
        private System.Windows.Forms.Label label2;


    }
}
