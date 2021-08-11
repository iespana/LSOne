using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems
{
    partial class RetailItemSearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailItemSearchPanel));
            this.lvRetailItemSearchResults = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvRetailItemSearchResults
            // 
            resources.ApplyResources(this.lvRetailItemSearchResults, "lvRetailItemSearchResults");
            this.lvRetailItemSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvRetailItemSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvRetailItemSearchResults.FullRowSelect = true;
            this.lvRetailItemSearchResults.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvRetailItemSearchResults.Groups")))});
            this.lvRetailItemSearchResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRetailItemSearchResults.HideSelection = false;
            this.lvRetailItemSearchResults.LockDrawing = false;
            this.lvRetailItemSearchResults.MultiSelect = false;
            this.lvRetailItemSearchResults.Name = "lvRetailItemSearchResults";
            this.lvRetailItemSearchResults.ShowGroups = false;
            this.lvRetailItemSearchResults.SortColumn = -1;
            this.lvRetailItemSearchResults.SortedBackwards = false;
            this.lvRetailItemSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvRetailItemSearchResults.UseEveryOtherRowColoring = true;
            this.lvRetailItemSearchResults.View = System.Windows.Forms.View.Details;
            this.lvRetailItemSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvStoreSearchResults_SelectedIndexChanged);
            this.lvRetailItemSearchResults.Resize += new System.EventHandler(this.lvUserSearchResults_Resize);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // RetailItemSearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvRetailItemSearchResults);
            this.Name = "RetailItemSearchPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvRetailItemSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
