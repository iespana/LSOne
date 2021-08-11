namespace LSOne.ViewPlugins.CustomerLedger.ViewPages
{
    partial class CustomerLedgerViewPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLedgerViewPage));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRecreateCustLed = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnRecreateCustLed);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnRecreateCustLed
            // 
            resources.ApplyResources(this.btnRecreateCustLed, "btnRecreateCustLed");
            this.btnRecreateCustLed.Name = "btnRecreateCustLed";
            this.btnRecreateCustLed.UseVisualStyleBackColor = true;
            this.btnRecreateCustLed.Click += new System.EventHandler(this.button1_Click);
            // 
            // CustomerLedgerViewPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "CustomerLedgerViewPage";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRecreateCustLed;

    }
}
