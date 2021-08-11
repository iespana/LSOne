using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class VisualProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualProfilesView));
            this.lvVisualProfiles = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvVisualProfiles);
            // 
            // lvVisualProfiles
            // 
            resources.ApplyResources(this.lvVisualProfiles, "lvVisualProfiles");
            this.lvVisualProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvVisualProfiles.FullRowSelect = true;
            this.lvVisualProfiles.HideSelection = false;
            this.lvVisualProfiles.LockDrawing = false;
            this.lvVisualProfiles.MultiSelect = false;
            this.lvVisualProfiles.Name = "lvVisualProfiles";
            this.lvVisualProfiles.SortColumn = -1;
            this.lvVisualProfiles.SortedBackwards = false;
            this.lvVisualProfiles.UseCompatibleStateImageBehavior = false;
            this.lvVisualProfiles.UseEveryOtherRowColoring = true;
            this.lvVisualProfiles.View = System.Windows.Forms.View.Details;
            this.lvVisualProfiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvVisualProfiles_ColumnClick);
            this.lvVisualProfiles.SelectedIndexChanged += new System.EventHandler(this.lvVisualProfiles_SelectedIndexChanged);
            this.lvVisualProfiles.SizeChanged += new System.EventHandler(this.lvVisualProfiles_SizeChanged);
            this.lvVisualProfiles.DoubleClick += new System.EventHandler(this.lvVisualProfiles_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // VisualProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "VisualProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvVisualProfiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsContextButtons;

    }
}
