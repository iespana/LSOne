namespace LSOne.ViewPlugins.RMSMigration.Dialogs
{
    partial class RMSMigrationWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMSMigrationWizard));
            this.SuspendLayout();
            // 
            // RMSMigrationWizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.HasHelp = true;
            this.Name = "RMSMigrationWizard";
            this.NextEnabled = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RMSMigrationWizard_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

    }
}