using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    partial class FunctionalProfileStartOfDay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalProfileStartOfDay));
            this.chkUseStartOfDay = new System.Windows.Forms.CheckBox();
            this.lblUseStartOfDay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkUseStartOfDay
            // 
            resources.ApplyResources(this.chkUseStartOfDay, "chkUseStartOfDay");
            this.chkUseStartOfDay.BackColor = System.Drawing.Color.Transparent;
            this.chkUseStartOfDay.Name = "chkUseStartOfDay";
            this.chkUseStartOfDay.UseVisualStyleBackColor = false;
            // 
            // lblUseStartOfDay
            // 
            this.lblUseStartOfDay.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUseStartOfDay, "lblUseStartOfDay");
            this.lblUseStartOfDay.Name = "lblUseStartOfDay";
            // 
            // FunctionalProfileStartOfDay
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseStartOfDay);
            this.Controls.Add(this.lblUseStartOfDay);
            this.DoubleBuffered = true;
            this.Name = "FunctionalProfileStartOfDay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseStartOfDay;
        private System.Windows.Forms.Label lblUseStartOfDay;
    }
}
