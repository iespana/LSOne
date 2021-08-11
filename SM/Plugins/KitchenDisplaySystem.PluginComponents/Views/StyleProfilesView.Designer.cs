using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class StyleProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfilesView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvProfiles = new LSOne.Controls.ExtendedListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvProfiles);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvProfiles
            // 
            resources.ApplyResources(this.lvProfiles, "lvProfiles");
            this.lvProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.lvProfiles.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lvProfiles.FullRowSelect = true;
            this.lvProfiles.HideSelection = false;
            this.lvProfiles.LockDrawing = false;
            this.lvProfiles.Name = "lvProfiles";
            this.lvProfiles.SortColumn = -1;
            this.lvProfiles.SortedBackwards = false;
            this.lvProfiles.UseCompatibleStateImageBehavior = false;
            this.lvProfiles.UseEveryOtherRowColoring = true;
            this.lvProfiles.View = System.Windows.Forms.View.Details;
            this.lvProfiles.SelectedIndexChanged += new System.EventHandler(this.lvProfiles_SelectedIndexChanged);
            this.lvProfiles.DoubleClick += new System.EventHandler(this.lvDataObjects_DoubleClick);
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // StyleProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StyleProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvProfiles;
        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
