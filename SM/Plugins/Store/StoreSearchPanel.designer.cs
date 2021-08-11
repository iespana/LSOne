using LSOne.Controls;

namespace LSOne.ViewPlugins.Store
{
    partial class StoreSearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreSearchPanel));
            this.lvStoreSearchResults = new ExtendedListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvStoreSearchResults
            // 
            resources.ApplyResources(this.lvStoreSearchResults, "lvStoreSearchResults");
            this.lvStoreSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvStoreSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvStoreSearchResults.FullRowSelect = true;
            this.lvStoreSearchResults.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvStoreSearchResults.Groups")))});
            this.lvStoreSearchResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStoreSearchResults.HideSelection = false;
            this.lvStoreSearchResults.MultiSelect = false;
            this.lvStoreSearchResults.Name = "lvStoreSearchResults";
            this.lvStoreSearchResults.ShowGroups = false;
            this.lvStoreSearchResults.SortColumn = -1;
            this.lvStoreSearchResults.SortedBackwards = false;
            this.lvStoreSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvStoreSearchResults.View = System.Windows.Forms.View.Details;
            this.lvStoreSearchResults.Resize += new System.EventHandler(this.lvUserSearchResults_Resize);
            this.lvStoreSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvStoreSearchResults_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // StoreSearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvStoreSearchResults);
            this.Name = "StoreSearchPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvStoreSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
