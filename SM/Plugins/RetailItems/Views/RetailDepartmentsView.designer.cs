using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    partial class RetailDepartmentsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailDepartmentsView));
            this.lvDepartments = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsDepartments = new LSOne.Controls.ContextButtons();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.groupPanelNoSelection = new LSOne.Controls.GroupPanel();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnCopyID = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.groupPanelNoSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCopyID);
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.groupPanelNoSelection);
            this.pnlBottom.Controls.Add(this.btnsDepartments);
            this.pnlBottom.Controls.Add(this.lvDepartments);
            // 
            // lvDepartments
            // 
            resources.ApplyResources(this.lvDepartments, "lvDepartments");
            this.lvDepartments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvDepartments.FullRowSelect = true;
            this.lvDepartments.HideSelection = false;
            this.lvDepartments.LockDrawing = false;
            this.lvDepartments.MultiSelect = false;
            this.lvDepartments.Name = "lvDepartments";
            this.lvDepartments.SortColumn = 0;
            this.lvDepartments.SortedBackwards = false;
            this.lvDepartments.UseCompatibleStateImageBehavior = false;
            this.lvDepartments.UseEveryOtherRowColoring = true;
            this.lvDepartments.View = System.Windows.Forms.View.Details;
            this.lvDepartments.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItems_ColumnClick);
            this.lvDepartments.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvDepartments.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnsDepartments
            // 
            this.btnsDepartments.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsDepartments, "btnsDepartments");
            this.btnsDepartments.BackColor = System.Drawing.Color.Transparent;
            this.btnsDepartments.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsDepartments.EditButtonEnabled = true;
            this.btnsDepartments.Name = "btnsDepartments";
            this.btnsDepartments.RemoveButtonEnabled = true;
            this.btnsDepartments.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsDepartments.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsDepartments.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // groupPanelNoSelection
            // 
            resources.ApplyResources(this.groupPanelNoSelection, "groupPanelNoSelection");
            this.groupPanelNoSelection.Controls.Add(this.lblNoSelection);
            this.groupPanelNoSelection.Name = "groupPanelNoSelection";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnCopyID
            // 
            resources.ApplyResources(this.btnCopyID, "btnCopyID");
            this.btnCopyID.Name = "btnCopyID";
            this.btnCopyID.UseVisualStyleBackColor = true;
            this.btnCopyID.Click += new System.EventHandler(this.CopyID);
            // 
            // RetailDepartmentsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RetailDepartmentsView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanelNoSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvDepartments;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsDepartments;
        private TabControl tabSheetTabs;
        private GroupPanel groupPanelNoSelection;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Button btnCopyID;
    }
}
