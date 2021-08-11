namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    partial class TerminalsLsPayPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalsLsPayPage));
            this.lblServerName = new System.Windows.Forms.Label();
            this.tbServerName = new System.Windows.Forms.TextBox();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.labelUseLocalServer = new System.Windows.Forms.Label();
            this.chkUseLocalServer = new System.Windows.Forms.CheckBox();
            this.labelPaymentPlugin = new System.Windows.Forms.Label();
            this.cmbPlugin = new LSOne.Controls.DualDataComboBox();
            this.btnGetPluginList = new System.Windows.Forms.Button();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.ntbServerPort = new LSOne.Controls.NumericTextBox();
            this.lblRefunds = new System.Windows.Forms.Label();
            this.chkRefRefunds = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            this.lblServerName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblServerName, "lblServerName");
            this.lblServerName.Name = "lblServerName";
            // 
            // tbServerName
            // 
            resources.ApplyResources(this.tbServerName, "tbServerName");
            this.tbServerName.Name = "tbServerName";
            // 
            // lblServerPort
            // 
            this.lblServerPort.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblServerPort, "lblServerPort");
            this.lblServerPort.Name = "lblServerPort";
            // 
            // labelUseLocalServer
            // 
            this.labelUseLocalServer.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelUseLocalServer, "labelUseLocalServer");
            this.labelUseLocalServer.Name = "labelUseLocalServer";
            // 
            // chkUseLocalServer
            // 
            resources.ApplyResources(this.chkUseLocalServer, "chkUseLocalServer");
            this.chkUseLocalServer.Name = "chkUseLocalServer";
            this.chkUseLocalServer.UseVisualStyleBackColor = true;
            this.chkUseLocalServer.CheckedChanged += new System.EventHandler(this.chkUseLocalServer_CheckedChanged);
            // 
            // labelPaymentPlugin
            // 
            this.labelPaymentPlugin.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelPaymentPlugin, "labelPaymentPlugin");
            this.labelPaymentPlugin.Name = "labelPaymentPlugin";
            // 
            // cmbPlugin
            // 
            this.cmbPlugin.AddList = null;
            this.cmbPlugin.AllowKeyboardSelection = false;
            this.cmbPlugin.EnableTextBox = true;
            resources.ApplyResources(this.cmbPlugin, "cmbPlugin");
            this.cmbPlugin.MaxLength = 32767;
            this.cmbPlugin.Name = "cmbPlugin";
            this.cmbPlugin.NoChangeAllowed = false;
            this.cmbPlugin.OnlyDisplayID = false;
            this.cmbPlugin.RemoveList = null;
            this.cmbPlugin.RowHeight = ((short)(22));
            this.cmbPlugin.SecondaryData = null;
            this.cmbPlugin.SelectedData = null;
            this.cmbPlugin.SelectedDataID = null;
            this.cmbPlugin.SelectionList = null;
            this.cmbPlugin.SkipIDColumn = true;
            this.cmbPlugin.RequestData += new System.EventHandler(this.cmbPluginID_RequestData);
            this.cmbPlugin.RequestClear += new System.EventHandler(this.cmbPlugin_RequestClear);
            // 
            // btnGetPluginList
            // 
            resources.ApplyResources(this.btnGetPluginList, "btnGetPluginList");
            this.btnGetPluginList.Name = "btnGetPluginList";
            this.btnGetPluginList.UseVisualStyleBackColor = true;
            this.btnGetPluginList.Click += new System.EventHandler(this.btnGetPluginList_Click);
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Mode = LSOne.Controls.LinkFields.ModeEnum.Tripple;
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // ntbServerPort
            // 
            this.ntbServerPort.AllowDecimal = false;
            this.ntbServerPort.AllowNegative = false;
            this.ntbServerPort.CultureInfo = null;
            this.ntbServerPort.DecimalLetters = 2;
            this.ntbServerPort.DecimalLimit = null;
            this.ntbServerPort.ForeColor = System.Drawing.Color.Black;
            this.ntbServerPort.HasMinValue = false;
            resources.ApplyResources(this.ntbServerPort, "ntbServerPort");
            this.ntbServerPort.MaxValue = 0D;
            this.ntbServerPort.MinValue = 0D;
            this.ntbServerPort.Name = "ntbServerPort";
            this.ntbServerPort.Value = 0D;
            // 
            // lblRefunds
            // 
            this.lblRefunds.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRefunds, "lblRefunds");
            this.lblRefunds.Name = "lblRefunds";
            // 
            // chkRefRefunds
            // 
            resources.ApplyResources(this.chkRefRefunds, "chkRefRefunds");
            this.chkRefRefunds.Name = "chkRefRefunds";
            this.chkRefRefunds.UseVisualStyleBackColor = true;
            // 
            // TerminalsLsPayPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblRefunds);
            this.Controls.Add(this.chkRefRefunds);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.btnGetPluginList);
            this.Controls.Add(this.labelPaymentPlugin);
            this.Controls.Add(this.cmbPlugin);
            this.Controls.Add(this.labelUseLocalServer);
            this.Controls.Add(this.chkUseLocalServer);
            this.Controls.Add(this.lblServerPort);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.ntbServerPort);
            this.DoubleBuffered = true;
            this.Name = "TerminalsLsPayPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblServerPort;
        private LSOne.Controls.NumericTextBox ntbServerPort;
        private System.Windows.Forms.TextBox tbServerName;
        private System.Windows.Forms.Label labelUseLocalServer;
        private System.Windows.Forms.CheckBox chkUseLocalServer;
        private System.Windows.Forms.Label labelPaymentPlugin;
        private Controls.DualDataComboBox cmbPlugin;
        private System.Windows.Forms.Button btnGetPluginList;
        private Controls.LinkFields linkFields1;
        private System.Windows.Forms.Label lblRefunds;
        private System.Windows.Forms.CheckBox chkRefRefunds;
    }
}
