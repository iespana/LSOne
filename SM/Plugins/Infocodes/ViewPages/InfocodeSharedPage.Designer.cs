﻿using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class InfocodeSharedPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfocodeSharedPage));
            this.lvInfocodes = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsAddRemove = new LSOne.Controls.ContextButtons();
            this.lvInfocodesImages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // lvInfocodes
            // 
            resources.ApplyResources(this.lvInfocodes, "lvInfocodes");
            this.lvInfocodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvInfocodes.FullRowSelect = true;
            this.lvInfocodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvInfocodes.HideSelection = false;
            this.lvInfocodes.LockDrawing = false;
            this.lvInfocodes.MultiSelect = false;
            this.lvInfocodes.Name = "lvInfocodes";
            this.lvInfocodes.SortColumn = -1;
            this.lvInfocodes.SortedBackwards = false;
            this.lvInfocodes.UseCompatibleStateImageBehavior = false;
            this.lvInfocodes.UseEveryOtherRowColoring = true;
            this.lvInfocodes.View = System.Windows.Forms.View.Details;
            this.lvInfocodes.SelectedIndexChanged += new System.EventHandler(this.lvInfocodes_SelectedIndexChanged);
            this.lvInfocodes.DoubleClick += new System.EventHandler(this.lvInfocodes_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnsAddRemove
            // 
            this.btnsAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddRemove, "btnsAddRemove");
            this.btnsAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsAddRemove.EditButtonEnabled = false;
            this.btnsAddRemove.Name = "btnsAddRemove";
            this.btnsAddRemove.RemoveButtonEnabled = false;
            this.btnsAddRemove.EditButtonClicked += new System.EventHandler(this.btnsAddRemove_EditButtonClicked);
            this.btnsAddRemove.AddButtonClicked += new System.EventHandler(this.btnsAddRemove_AddButtonClicked);
            this.btnsAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsAddRemove_RemoveButtonClicked);
            // 
            // lvInfocodesImages
            // 
            this.lvInfocodesImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.lvInfocodesImages, "lvInfocodesImages");
            this.lvInfocodesImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // InfocodeSharedPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsAddRemove);
            this.Controls.Add(this.lvInfocodes);
            this.DoubleBuffered = true;
            this.Name = "InfocodeSharedPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvInfocodes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsAddRemove;
        private System.Windows.Forms.ImageList lvInfocodesImages;

    }
}
