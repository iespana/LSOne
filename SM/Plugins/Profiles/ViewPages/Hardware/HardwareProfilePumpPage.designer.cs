﻿namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfilePumpPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfilePumpPage));
            this.chkPumpActive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPumpControlIDs = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkPumpActive
            // 
            resources.ApplyResources(this.chkPumpActive, "chkPumpActive");
            this.chkPumpActive.Name = "chkPumpActive";
            this.chkPumpActive.UseVisualStyleBackColor = true;
            this.chkPumpActive.CheckedChanged += new System.EventHandler(this.chkPumpActive_CheckedChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbPumpControlIDs
            // 
            resources.ApplyResources(this.tbPumpControlIDs, "tbPumpControlIDs");
            this.tbPumpControlIDs.Name = "tbPumpControlIDs";
            // 
            // HardwareProfilePumpPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbPumpControlIDs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkPumpActive);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfilePumpPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPumpActive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPumpControlIDs;


    }
}
