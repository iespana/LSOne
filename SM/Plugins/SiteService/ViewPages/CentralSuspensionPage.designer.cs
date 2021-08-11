using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class CentralSuspensionPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CentralSuspensionPage));
            this.chkUserConfirmation = new System.Windows.Forms.CheckBox();
            this.chkUseCentralSuspension = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserConfirmation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkUserConfirmation
            // 
            this.chkUserConfirmation.Checked = true;
            this.chkUserConfirmation.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkUserConfirmation, "chkUserConfirmation");
            this.chkUserConfirmation.Name = "chkUserConfirmation";
            this.chkUserConfirmation.UseVisualStyleBackColor = true;
            // 
            // chkUseCentralSuspension
            // 
            resources.ApplyResources(this.chkUseCentralSuspension, "chkUseCentralSuspension");
            this.chkUseCentralSuspension.Name = "chkUseCentralSuspension";
            this.chkUseCentralSuspension.UseVisualStyleBackColor = true;
            this.chkUseCentralSuspension.CheckedChanged += new System.EventHandler(this.chkUseCentralSuspension_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblUserConfirmation
            // 
            resources.ApplyResources(this.lblUserConfirmation, "lblUserConfirmation");
            this.lblUserConfirmation.Name = "lblUserConfirmation";
            // 
            // CentralSuspensionPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUserConfirmation);
            this.Controls.Add(this.chkUseCentralSuspension);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUserConfirmation);
            this.DoubleBuffered = true;
            this.Name = "CentralSuspensionPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUserConfirmation;
        private System.Windows.Forms.CheckBox chkUseCentralSuspension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUserConfirmation;



    }
}
