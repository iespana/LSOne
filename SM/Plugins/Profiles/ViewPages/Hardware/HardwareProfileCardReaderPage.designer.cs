namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfileCardReaderPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfileCardReaderPage));
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDeviceName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbStartDigit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDiffDigit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFinalDigit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbMSR = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbStartDigit
            // 
            resources.ApplyResources(this.tbStartDigit, "tbStartDigit");
            this.tbStartDigit.Name = "tbStartDigit";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbDiffDigit
            // 
            resources.ApplyResources(this.tbDiffDigit, "tbDiffDigit");
            this.tbDiffDigit.Name = "tbDiffDigit";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbFinalDigit
            // 
            resources.ApplyResources(this.tbFinalDigit, "tbFinalDigit");
            this.tbFinalDigit.Name = "tbFinalDigit";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbMSR
            // 
            this.cmbMSR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMSR.FormattingEnabled = true;
            this.cmbMSR.Items.AddRange(new object[] {
            resources.GetString("cmbMSR.Items"),
            resources.GetString("cmbMSR.Items1"),
            resources.GetString("cmbMSR.Items2")});
            resources.ApplyResources(this.cmbMSR, "cmbMSR");
            this.cmbMSR.Name = "cmbMSR";
            this.cmbMSR.SelectedIndexChanged += new System.EventHandler(this.cmbMSR_SelectedIndexChanged);
            // 
            // HardwareProfileCardReaderPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbMSR);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbFinalDigit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDiffDigit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbStartDigit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfileCardReaderPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDeviceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbStartDigit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDiffDigit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbFinalDigit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cmbMSR;


    }
}
