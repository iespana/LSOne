using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class ConnectionPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionPage));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.siteServiceButtonLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnConfiguration = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ntbMaxMessageSize = new LSOne.Controls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbTimeout = new LSOne.Controls.NumericTextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.ntbPort = new LSOne.Controls.NumericTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox3.SuspendLayout();
            this.siteServiceButtonLayout.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.siteServiceButtonLayout);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // siteServiceButtonLayout
            // 
            this.siteServiceButtonLayout.Controls.Add(this.btnStop);
            this.siteServiceButtonLayout.Controls.Add(this.btnStart);
            this.siteServiceButtonLayout.Controls.Add(this.btnConfiguration);
            resources.ApplyResources(this.siteServiceButtonLayout, "siteServiceButtonLayout");
            this.siteServiceButtonLayout.Name = "siteServiceButtonLayout";
            // 
            // btnStop
            // 
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.Name = "btnStop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnConfiguration
            // 
            resources.ApplyResources(this.btnConfiguration, "btnConfiguration");
            this.btnConfiguration.Image = global::LSOne.ViewPlugins.SiteService.Properties.Resources.LockedSmallImage;
            this.btnConfiguration.Name = "btnConfiguration";
            this.btnConfiguration.UseVisualStyleBackColor = true;
            this.btnConfiguration.Click += new System.EventHandler(this.btnConfiguration_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ntbMaxMessageSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ntbTimeout);
            this.groupBox1.Controls.Add(this.lblTimeout);
            this.groupBox1.Controls.Add(this.btnTestConnection);
            this.groupBox1.Controls.Add(this.lblHost);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.tbHost);
            this.groupBox1.Controls.Add(this.ntbPort);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ntbMaxMessageSize
            // 
            this.ntbMaxMessageSize.AllowDecimal = false;
            this.ntbMaxMessageSize.AllowNegative = false;
            this.ntbMaxMessageSize.CultureInfo = null;
            this.ntbMaxMessageSize.DecimalLetters = 2;
            this.ntbMaxMessageSize.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxMessageSize.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxMessageSize, "ntbMaxMessageSize");
            this.ntbMaxMessageSize.MaxValue = 2147483647D;
            this.ntbMaxMessageSize.MinValue = 0D;
            this.ntbMaxMessageSize.Name = "ntbMaxMessageSize";
            this.ntbMaxMessageSize.Value = 0D;
            this.ntbMaxMessageSize.TextChanged += new System.EventHandler(this.OnServerValuesChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbTimeout
            // 
            this.ntbTimeout.AllowDecimal = false;
            this.ntbTimeout.AllowNegative = false;
            this.ntbTimeout.CultureInfo = null;
            this.ntbTimeout.DecimalLetters = 2;
            this.ntbTimeout.ForeColor = System.Drawing.Color.Black;
            this.ntbTimeout.HasMinValue = false;
            resources.ApplyResources(this.ntbTimeout, "ntbTimeout");
            this.ntbTimeout.MaxValue = 65535D;
            this.ntbTimeout.MinValue = 0D;
            this.ntbTimeout.Name = "ntbTimeout";
            this.ntbTimeout.Value = 0D;
            this.ntbTimeout.TextChanged += new System.EventHandler(this.OnServerValuesChanged);
            // 
            // lblTimeout
            // 
            resources.ApplyResources(this.lblTimeout, "lblTimeout");
            this.lblTimeout.Name = "lblTimeout";
            // 
            // btnTestConnection
            // 
            resources.ApplyResources(this.btnTestConnection, "btnTestConnection");
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // lblHost
            // 
            resources.ApplyResources(this.lblHost, "lblHost");
            this.lblHost.Name = "lblHost";
            // 
            // lblPort
            // 
            resources.ApplyResources(this.lblPort, "lblPort");
            this.lblPort.Name = "lblPort";
            // 
            // tbHost
            // 
            resources.ApplyResources(this.tbHost, "tbHost");
            this.tbHost.Name = "tbHost";
            this.tbHost.TextChanged += new System.EventHandler(this.OnServerValuesChanged);
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
            this.ntbPort.TextChanged += new System.EventHandler(this.OnServerValuesChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ConnectionPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "ConnectionPage";
            this.groupBox3.ResumeLayout(false);
            this.siteServiceButtonLayout.ResumeLayout(false);
            this.siteServiceButtonLayout.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox tbHost;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.FlowLayoutPanel siteServiceButtonLayout;
        private System.Windows.Forms.Button btnConfiguration;
        private NumericTextBox ntbMaxMessageSize;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbTimeout;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
