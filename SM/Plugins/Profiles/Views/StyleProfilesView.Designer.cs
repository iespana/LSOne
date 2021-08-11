using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
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
            this.btnsEditAddRemove = new ContextButtons();
            this.lvStylesProfiles = new ExtendedListView();
            this.colProfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvStylesProfiles);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvStylesProfiles
            // 
            resources.ApplyResources(this.lvStylesProfiles, "lvStylesProfiles");
            this.lvStylesProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colProfile});
            this.lvStylesProfiles.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lvStylesProfiles.FullRowSelect = true;
            this.lvStylesProfiles.HideSelection = false;
            this.lvStylesProfiles.LockDrawing = false;
            this.lvStylesProfiles.MultiSelect = false;
            this.lvStylesProfiles.Name = "lvStylesProfiles";
            this.lvStylesProfiles.SortColumn = 0;
            this.lvStylesProfiles.SortedBackwards = false;
            this.lvStylesProfiles.UseCompatibleStateImageBehavior = false;
            this.lvStylesProfiles.UseEveryOtherRowColoring = true;
            this.lvStylesProfiles.View = System.Windows.Forms.View.Details;
            this.lvStylesProfiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStylesProfiles_ColumnClick);
            this.lvStylesProfiles.SelectedIndexChanged += new System.EventHandler(this.lvStyleProfiles_SelectedIndexChanged);
            this.lvStylesProfiles.DoubleClick += new System.EventHandler(this.lvDataObjects_DoubleClick);
            // 
            // colProfile
            // 
            resources.ApplyResources(this.colProfile, "colProfile");
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

        private ExtendedListView lvStylesProfiles;
        private System.Windows.Forms.ColumnHeader colProfile;
        private ContextButtons btnsEditAddRemove;
    }
}
