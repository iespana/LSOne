using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class InventoryPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryPage));
            this.chkUseCentralizedInventoryLookup = new System.Windows.Forms.CheckBox();
            this.lblInventoryLookup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkUseCentralizedInventoryLookup
            // 
            resources.ApplyResources(this.chkUseCentralizedInventoryLookup, "chkUseCentralizedInventoryLookup");
            this.chkUseCentralizedInventoryLookup.Name = "chkUseCentralizedInventoryLookup";
            this.chkUseCentralizedInventoryLookup.UseVisualStyleBackColor = true;
            // 
            // lblInventoryLookup
            // 
            resources.ApplyResources(this.lblInventoryLookup, "lblInventoryLookup");
            this.lblInventoryLookup.Name = "lblInventoryLookup";
            // 
            // SettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseCentralizedInventoryLookup);
            this.Controls.Add(this.lblInventoryLookup);
            this.DoubleBuffered = true;
            this.Name = "SettingsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseCentralizedInventoryLookup;
        private System.Windows.Forms.Label lblInventoryLookup;



    }
}
