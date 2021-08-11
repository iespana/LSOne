using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class RetailDivisionDepartmentPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailDivisionDepartmentPage));
            this.btnsContextButtonsItems = new ContextButtons();
            this.depDataScroll = new DatabasePageDisplay();
            this.lvDepList = new ExtendedListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = ButtonTypes.AddRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = true;
            this.btnsContextButtonsItems.AddButtonClicked += new System.EventHandler(this.btnAddDepartment_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemoveDepartment_Click);
            // 
            // depDataScroll
            // 
            resources.ApplyResources(this.depDataScroll, "depDataScroll");
            this.depDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.depDataScroll.Name = "depDataScroll";
            this.depDataScroll.PageSize = 0;
            this.depDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // lvDepList
            // 
            resources.ApplyResources(this.lvDepList, "lvDepList");
            this.lvDepList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.lvDepList.FullRowSelect = true;
            this.lvDepList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvDepList.HideSelection = false;
            this.lvDepList.LabelEdit = true;
            this.lvDepList.LockDrawing = false;
            this.lvDepList.MultiSelect = false;
            this.lvDepList.Name = "lvDepList";
            this.lvDepList.SortColumn = -1;
            this.lvDepList.SortedBackwards = false;
            this.lvDepList.UseCompatibleStateImageBehavior = false;
            this.lvDepList.UseEveryOtherRowColoring = true;
            this.lvDepList.View = System.Windows.Forms.View.Details;
            this.lvDepList.SelectedIndexChanged += new System.EventHandler(this.lvDepList_SelectedIndexChanged);
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
            // RetailDivisionDepartmentPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsContextButtonsItems);
            this.Controls.Add(this.depDataScroll);
            this.Controls.Add(this.lvDepList);
            this.Controls.Add(this.lblNoSelection);
            this.Name = "RetailDivisionDepartmentPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtonsItems;
        private DatabasePageDisplay depDataScroll;
        private ExtendedListView lvDepList;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label lblNoSelection;
    }
}
