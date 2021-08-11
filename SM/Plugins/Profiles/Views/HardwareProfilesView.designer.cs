using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class HardwareProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfilesView));
            this.lvHardwareProfiles = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvHardwareProfiles);
            // 
            // lvHardwareProfiles
            // 
            resources.ApplyResources(this.lvHardwareProfiles, "lvHardwareProfiles");
            this.lvHardwareProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvHardwareProfiles.FullRowSelect = true;
            this.lvHardwareProfiles.HideSelection = false;
            this.lvHardwareProfiles.LockDrawing = false;
            this.lvHardwareProfiles.MultiSelect = false;
            this.lvHardwareProfiles.Name = "lvHardwareProfiles";
            this.lvHardwareProfiles.SortColumn = -1;
            this.lvHardwareProfiles.SortedBackwards = false;
            this.lvHardwareProfiles.UseCompatibleStateImageBehavior = false;
            this.lvHardwareProfiles.UseEveryOtherRowColoring = true;
            this.lvHardwareProfiles.View = System.Windows.Forms.View.Details;
            this.lvHardwareProfiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTransactionServiceProfiles_ColumnClick);
            this.lvHardwareProfiles.SelectedIndexChanged += new System.EventHandler(this.lvHardwareProfiles_SelectedIndexChanged);
            this.lvHardwareProfiles.SizeChanged += new System.EventHandler(this.lvHardwareProfiles_SizeChanged);
            this.lvHardwareProfiles.DoubleClick += new System.EventHandler(this.lvHardwareProfiles_DoubleClick);
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
            // HardwareProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HardwareProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvHardwareProfiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsContextButtons;

    }
}
