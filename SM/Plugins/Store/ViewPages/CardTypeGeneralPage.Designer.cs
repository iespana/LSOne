namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class CardTypeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardTypeGeneralPage));
            this.chkAllowManualEntering = new System.Windows.Forms.CheckBox();
            this.chkInternalauthorization = new System.Windows.Forms.CheckBox();
            this.chkExpiryDate = new System.Windows.Forms.CheckBox();
            this.chkModulusCheck = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkAllowManualEntering
            // 
            resources.ApplyResources(this.chkAllowManualEntering, "chkAllowManualEntering");
            this.chkAllowManualEntering.Name = "chkAllowManualEntering";
            this.chkAllowManualEntering.UseVisualStyleBackColor = true;
            this.chkAllowManualEntering.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // chkInternalauthorization
            // 
            resources.ApplyResources(this.chkInternalauthorization, "chkInternalauthorization");
            this.chkInternalauthorization.Name = "chkInternalauthorization";
            this.chkInternalauthorization.UseVisualStyleBackColor = true;
            this.chkInternalauthorization.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // chkExpiryDate
            // 
            resources.ApplyResources(this.chkExpiryDate, "chkExpiryDate");
            this.chkExpiryDate.Name = "chkExpiryDate";
            this.chkExpiryDate.UseVisualStyleBackColor = true;
            this.chkExpiryDate.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // chkModulusCheck
            // 
            resources.ApplyResources(this.chkModulusCheck, "chkModulusCheck");
            this.chkModulusCheck.Name = "chkModulusCheck";
            this.chkModulusCheck.UseVisualStyleBackColor = true;
            this.chkModulusCheck.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // CardTypeGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkAllowManualEntering);
            this.Controls.Add(this.chkInternalauthorization);
            this.Controls.Add(this.chkExpiryDate);
            this.Controls.Add(this.chkModulusCheck);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Name = "CardTypeGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAllowManualEntering;
        private System.Windows.Forms.CheckBox chkInternalauthorization;
        private System.Windows.Forms.CheckBox chkExpiryDate;
        private System.Windows.Forms.CheckBox chkModulusCheck;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;

    }
}
