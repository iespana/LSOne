using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class RetailDepartmentRetailGroupsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailDepartmentRetailGroupsPage));
            this.btnsGroups = new LSOne.Controls.ContextButtons();
            this.lvGroups = new LSOne.Controls.ExtendedListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnViewRetailGroup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnsGroups
            // 
            this.btnsGroups.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsGroups, "btnsGroups");
            this.btnsGroups.BackColor = System.Drawing.Color.Transparent;
            this.btnsGroups.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsGroups.EditButtonEnabled = false;
            this.btnsGroups.Name = "btnsGroups";
            this.btnsGroups.RemoveButtonEnabled = true;
            this.btnsGroups.AddButtonClicked += new System.EventHandler(this.btnsGroups_AddButtonClicked);
            this.btnsGroups.RemoveButtonClicked += new System.EventHandler(this.btnsGroups_RemoveButtonClicked);
            // 
            // lvGroups
            // 
            resources.ApplyResources(this.lvGroups, "lvGroups");
            this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.HideSelection = false;
            this.lvGroups.LockDrawing = false;
            this.lvGroups.MultiSelect = false;
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.SortColumn = 0;
            this.lvGroups.SortedBackwards = false;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.UseEveryOtherRowColoring = true;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.SelectedIndexChanged += new System.EventHandler(this.lvGroups_SelectedIndexChanged);
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnViewRetailGroup
            // 
            resources.ApplyResources(this.btnViewRetailGroup, "btnViewRetailGroup");
            this.btnViewRetailGroup.Name = "btnViewRetailGroup";
            this.btnViewRetailGroup.UseVisualStyleBackColor = true;
            this.btnViewRetailGroup.Click += new System.EventHandler(this.ShowRetailGroupView);
            // 
            // RetailDepartmentRetailGroupsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnViewRetailGroup);
            this.Controls.Add(this.btnsGroups);
            this.Controls.Add(this.lvGroups);
            this.Controls.Add(this.lblNoSelection);
            this.Name = "RetailDepartmentRetailGroupsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButtons btnsGroups;
        private ExtendedListView lvGroups;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Button btnViewRetailGroup;
    }
}
