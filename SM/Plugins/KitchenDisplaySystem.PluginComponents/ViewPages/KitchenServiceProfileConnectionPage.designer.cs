using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class KitchenServiceProfileConnectionPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KitchenServiceProfileConnectionPage));
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.ntbPort = new LSOne.Controls.NumericTextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.lblKitchenManagerHost = new System.Windows.Forms.Label();
            this.lblKitchenManagerPort = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTestConnection
            // 
            resources.ApplyResources(this.btnTestConnection, "btnTestConnection");
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConfig);
            this.groupBox1.Controls.Add(this.ntbPort);
            this.groupBox1.Controls.Add(this.tbHost);
            this.groupBox1.Controls.Add(this.lblKitchenManagerHost);
            this.groupBox1.Controls.Add(this.btnTestConnection);
            this.groupBox1.Controls.Add(this.lblKitchenManagerPort);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnConfig
            // 
            resources.ApplyResources(this.btnConfig, "btnConfig");
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.button1_Click);
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
            this.ntbPort.MaxValue = 0D;
            this.ntbPort.MinValue = 0D;
            this.ntbPort.Name = "ntbPort";
            this.ntbPort.Value = 17750D;
            // 
            // tbHost
            // 
            resources.ApplyResources(this.tbHost, "tbHost");
            this.tbHost.Name = "tbHost";
            // 
            // lblKitchenManagerHost
            // 
            resources.ApplyResources(this.lblKitchenManagerHost, "lblKitchenManagerHost");
            this.lblKitchenManagerHost.Name = "lblKitchenManagerHost";
            // 
            // lblKitchenManagerPort
            // 
            resources.ApplyResources(this.lblKitchenManagerPort, "lblKitchenManagerPort");
            this.lblKitchenManagerPort.Name = "lblKitchenManagerPort";
            // 
            // KitchenServiceProfileConnectionPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "KitchenServiceProfileConnectionPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.Label lblKitchenManagerHost;
        private System.Windows.Forms.Label lblKitchenManagerPort;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.Button btnConfig;

    }
}
