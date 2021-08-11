using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    partial class AdministrationSecurityPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationSecurityPage));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbPasswordNeverExpires = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPasswordLockout = new LSOne.Controls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMaxPasswordAge = new LSOne.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMask = new System.Windows.Forms.Label();
            this.tbDomain = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbWriteAuditing = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.lblEnabled = new System.Windows.Forms.Label();
            this.lblDomain = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.ckbPasswordNeverExpires);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbPasswordLockout);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbMaxPasswordAge);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblMask);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ckbPasswordNeverExpires
            // 
            resources.ApplyResources(this.ckbPasswordNeverExpires, "ckbPasswordNeverExpires");
            this.ckbPasswordNeverExpires.Name = "ckbPasswordNeverExpires";
            this.ckbPasswordNeverExpires.UseVisualStyleBackColor = true;
            this.ckbPasswordNeverExpires.CheckedChanged += new System.EventHandler(this.NewMethod);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbPasswordLockout
            // 
            this.tbPasswordLockout.AllowDecimal = false;
            this.tbPasswordLockout.AllowNegative = false;
            this.tbPasswordLockout.CultureInfo = null;
            this.tbPasswordLockout.DecimalLetters = 2;
            this.tbPasswordLockout.HasMinValue = false;
            resources.ApplyResources(this.tbPasswordLockout, "tbPasswordLockout");
            this.tbPasswordLockout.MaxValue = 99D;
            this.tbPasswordLockout.MinValue = 1D;
            this.tbPasswordLockout.Name = "tbPasswordLockout";
            this.tbPasswordLockout.Value = 1D;
            this.tbPasswordLockout.TextChanged += new System.EventHandler(this.tbPasswordLockout_TextChanged);
            this.tbPasswordLockout.Leave += new System.EventHandler(this.tbPasswordLockout_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbMaxPasswordAge
            // 
            this.tbMaxPasswordAge.AllowDecimal = false;
            this.tbMaxPasswordAge.AllowNegative = false;
            this.tbMaxPasswordAge.CultureInfo = null;
            this.tbMaxPasswordAge.DecimalLetters = 2;
            this.tbMaxPasswordAge.HasMinValue = false;
            resources.ApplyResources(this.tbMaxPasswordAge, "tbMaxPasswordAge");
            this.tbMaxPasswordAge.MaxValue = 0D;
            this.tbMaxPasswordAge.MinValue = 0D;
            this.tbMaxPasswordAge.Name = "tbMaxPasswordAge";
            this.tbMaxPasswordAge.Value = 0D;
            this.tbMaxPasswordAge.ValueChanged += new System.EventHandler(this.tbMaxPasswordAge_ValueChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lblMask
            // 
            this.lblMask.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMask, "lblMask");
            this.lblMask.Name = "lblMask";
            // 
            // tbDomain
            // 
            resources.ApplyResources(this.tbDomain, "tbDomain");
            this.tbDomain.Name = "tbDomain";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.cmbWriteAuditing);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cmbWriteAuditing
            // 
            this.cmbWriteAuditing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWriteAuditing.FormattingEnabled = true;
            this.cmbWriteAuditing.Items.AddRange(new object[] {
            resources.GetString("cmbWriteAuditing.Items"),
            resources.GetString("cmbWriteAuditing.Items1")});
            resources.ApplyResources(this.cmbWriteAuditing, "cmbWriteAuditing");
            this.cmbWriteAuditing.Name = "cmbWriteAuditing";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.chkEnabled);
            this.groupBox3.Controls.Add(this.tbDomain);
            this.groupBox3.Controls.Add(this.lblEnabled);
            this.groupBox3.Controls.Add(this.lblDomain);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // lblEnabled
            // 
            this.lblEnabled.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblEnabled, "lblEnabled");
            this.lblEnabled.Name = "lblEnabled";
            // 
            // lblDomain
            // 
            this.lblDomain.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDomain, "lblDomain");
            this.lblDomain.Name = "lblDomain";
            // 
            // AdministrationSecurityPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AdministrationSecurityPage";
            this.Load += new System.EventHandler(this.AdministrationSecurityPage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMask;
        private NumericTextBox tbMaxPasswordAge;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private NumericTextBox tbPasswordLockout;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbWriteAuditing;
        private System.Windows.Forms.TextBox tbDomain;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label lblEnabled;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.CheckBox ckbPasswordNeverExpires;
        private System.Windows.Forms.Label label4;

    }
}
