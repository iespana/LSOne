namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class SettingsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbServerDefaultPort = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbServerNetMode = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.tbServerHost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlBottom.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupBox1);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lbServerDefaultPort);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cmbServerNetMode);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tbServerPort);
            this.groupBox1.Controls.Add(this.tbServerHost);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lbServerDefaultPort
            // 
            resources.ApplyResources(this.lbServerDefaultPort, "lbServerDefaultPort");
            this.lbServerDefaultPort.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lbServerDefaultPort.Name = "lbServerDefaultPort";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // cmbServerNetMode
            // 
            this.cmbServerNetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServerNetMode.FormattingEnabled = true;
            this.cmbServerNetMode.Items.AddRange(new object[] {
            resources.GetString("cmbServerNetMode.Items"),
            resources.GetString("cmbServerNetMode.Items1")});
            resources.ApplyResources(this.cmbServerNetMode, "cmbServerNetMode");
            this.cmbServerNetMode.Name = "cmbServerNetMode";
            this.cmbServerNetMode.SelectedIndexChanged += new System.EventHandler(this.cmbServerNetMode_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tbServerPort
            // 
            resources.ApplyResources(this.tbServerPort, "tbServerPort");
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.TextChanged += new System.EventHandler(this.tbServerPort_TextChanged);
            this.tbServerPort.Validating += new System.ComponentModel.CancelEventHandler(this.tbServerPort_Validating);
            // 
            // tbServerHost
            // 
            resources.ApplyResources(this.tbServerHost, "tbServerHost");
            this.tbServerHost.Name = "tbServerHost";
            this.tbServerHost.Validating += new System.ComponentModel.CancelEventHandler(this.tbServerHost_Validating);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // SettingsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SettingsView";
            this.pnlBottom.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbServerNetMode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.TextBox tbServerHost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbServerDefaultPort;
        private System.Windows.Forms.ErrorProvider errorProvider;

    }
}
