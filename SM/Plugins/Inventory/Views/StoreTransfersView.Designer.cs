﻿namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class StoreTransfersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTransfersView));
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            this.tabSheetTabs.SelectedTabChanged += new System.EventHandler(this.tabSheetTabs_SelectedTabChanged);
            // 
            // StoreTransfersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StoreTransfersView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewCore.Controls.TabControlLeft tabSheetTabs;
    }
}
