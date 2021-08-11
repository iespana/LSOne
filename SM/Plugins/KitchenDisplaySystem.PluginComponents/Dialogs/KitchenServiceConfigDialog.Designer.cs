using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class KitchenServiceConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KitchenServiceConfigDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGetConfig = new System.Windows.Forms.Button();
            this.btnSendConfig = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ntbPOSPort = new LSOne.Controls.NumericTextBox();
            this.ntbDSPort = new LSOne.Controls.NumericTextBox();
            this.ntbPort = new LSOne.Controls.NumericTextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.lblKitchenServiceHost = new System.Windows.Forms.Label();
            this.lblKitchenServicePort = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.buttonLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.buttonLayout);
            this.panel2.Name = "panel2";
            // 
            // buttonLayout
            // 
            resources.ApplyResources(this.buttonLayout, "buttonLayout");
            this.buttonLayout.Controls.Add(this.btnGetConfig);
            this.buttonLayout.Controls.Add(this.btnSendConfig);
            this.buttonLayout.Controls.Add(this.btnCancel);
            this.buttonLayout.Name = "buttonLayout";
            // 
            // btnGetConfig
            // 
            resources.ApplyResources(this.btnGetConfig, "btnGetConfig");
            this.btnGetConfig.Name = "btnGetConfig";
            this.btnGetConfig.UseVisualStyleBackColor = true;
            this.btnGetConfig.Click += new System.EventHandler(this.btnGetConfig_Click);
            // 
            // btnSendConfig
            // 
            resources.ApplyResources(this.btnSendConfig, "btnSendConfig");
            this.btnSendConfig.Name = "btnSendConfig";
            this.btnSendConfig.UseVisualStyleBackColor = true;
            this.btnSendConfig.Click += new System.EventHandler(this.btnSendConfig_Click);
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // ntbPOSPort
            // 
            this.ntbPOSPort.AllowDecimal = false;
            this.ntbPOSPort.AllowNegative = false;
            this.ntbPOSPort.CultureInfo = null;
            this.ntbPOSPort.DecimalLetters = 2;
            resources.ApplyResources(this.ntbPOSPort, "ntbPOSPort");
            this.ntbPOSPort.ForeColor = System.Drawing.Color.Black;
            this.ntbPOSPort.HasMinValue = false;
            this.ntbPOSPort.MaxValue = 0D;
            this.ntbPOSPort.MinValue = 0D;
            this.ntbPOSPort.Name = "ntbPOSPort";
            this.ntbPOSPort.Value = 17800D;
            // 
            // ntbDSPort
            // 
            this.ntbDSPort.AllowDecimal = false;
            this.ntbDSPort.AllowNegative = false;
            this.ntbDSPort.CultureInfo = null;
            this.ntbDSPort.DecimalLetters = 2;
            resources.ApplyResources(this.ntbDSPort, "ntbDSPort");
            this.ntbDSPort.ForeColor = System.Drawing.Color.Black;
            this.ntbDSPort.HasMinValue = false;
            this.ntbDSPort.MaxValue = 0D;
            this.ntbDSPort.MinValue = 0D;
            this.ntbDSPort.Name = "ntbDSPort";
            this.ntbDSPort.Value = 17800D;
            // 
            // ntbPort
            // 
            this.ntbPort.AllowDecimal = false;
            this.ntbPort.AllowNegative = false;
            this.ntbPort.CultureInfo = null;
            this.ntbPort.DecimalLetters = 2;
            resources.ApplyResources(this.ntbPort, "ntbPort");
            this.ntbPort.ForeColor = System.Drawing.Color.Black;
            this.ntbPort.HasMinValue = false;
            this.ntbPort.MaxValue = 0D;
            this.ntbPort.MinValue = 0D;
            this.ntbPort.Name = "ntbPort";
            this.ntbPort.Value = 17750D;
            this.ntbPort.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbHost
            // 
            resources.ApplyResources(this.tbHost, "tbHost");
            this.tbHost.Name = "tbHost";
            this.tbHost.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblKitchenServiceHost
            // 
            resources.ApplyResources(this.lblKitchenServiceHost, "lblKitchenServiceHost");
            this.lblKitchenServiceHost.Name = "lblKitchenServiceHost";
            // 
            // lblKitchenServicePort
            // 
            resources.ApplyResources(this.lblKitchenServicePort, "lblKitchenServicePort");
            this.lblKitchenServicePort.Name = "lblKitchenServicePort";
            // 
            // KitchenServiceConfigDialog
            // 
            this.AcceptButton = this.btnSendConfig;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ntbPOSPort);
            this.Controls.Add(this.ntbDSPort);
            this.Controls.Add(this.ntbPort);
            this.Controls.Add(this.lblKitchenServicePort);
            this.Controls.Add(this.tbHost);
            this.Controls.Add(this.lblKitchenServiceHost);
            this.HasHelp = true;
            this.Name = "KitchenServiceConfigDialog";
            this.Controls.SetChildIndex(this.lblKitchenServiceHost, 0);
            this.Controls.SetChildIndex(this.tbHost, 0);
            this.Controls.SetChildIndex(this.lblKitchenServicePort, 0);
            this.Controls.SetChildIndex(this.ntbPort, 0);
            this.Controls.SetChildIndex(this.ntbDSPort, 0);
            this.Controls.SetChildIndex(this.ntbPOSPort, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.buttonLayout.ResumeLayout(false);
            this.buttonLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSendConfig;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnGetConfig;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.Label lblKitchenServiceHost;
        private System.Windows.Forms.Label lblKitchenServicePort;
        private System.Windows.Forms.FlowLayoutPanel buttonLayout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbPOSPort;
        private NumericTextBox ntbDSPort;
    }
}