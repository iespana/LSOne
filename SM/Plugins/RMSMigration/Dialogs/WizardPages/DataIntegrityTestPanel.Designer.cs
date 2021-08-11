namespace LSOne.ViewPlugins.RMSMigration.Dialogs.WizardPages
{
    partial class DataIntegrityTestPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataIntegrityTestPanel));
            this.lblNoErrors = new System.Windows.Forms.Label();
            this.lblErrorInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNoErrors
            // 
            this.lblNoErrors.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblNoErrors, "lblNoErrors");
            this.lblNoErrors.Name = "lblNoErrors";
            // 
            // lblErrorInfo
            // 
            this.lblErrorInfo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblErrorInfo, "lblErrorInfo");
            this.lblErrorInfo.Name = "lblErrorInfo";
            // 
            // DataIntegrityTestPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblErrorInfo);
            this.Controls.Add(this.lblNoErrors);
            this.Name = "DataIntegrityTestPanel";
            this.Load += new System.EventHandler(this.DataIntegrityTestPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblNoErrors;
        private System.Windows.Forms.Label lblErrorInfo;
    }
}
