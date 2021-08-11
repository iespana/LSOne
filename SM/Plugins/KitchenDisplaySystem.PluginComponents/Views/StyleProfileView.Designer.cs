﻿using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class StyleProfileView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileView));
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.tbProfileName);
            this.pnlBottom.Controls.Add(this.label3);
            // 
            // tbProfileName
            // 
            resources.ApplyResources(this.tbProfileName, "tbProfileName");
            this.tbProfileName.Name = "tbProfileName";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // StyleProfileView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 50;
            this.Name = "StyleProfileView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.Label label3;
        private TabControlLeft tabSheetTabs;
    }
}
