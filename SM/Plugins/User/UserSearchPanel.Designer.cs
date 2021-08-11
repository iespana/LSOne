using LSOne.Controls;

namespace LSOne.ViewPlugins.User
{
    partial class UserSearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSearchPanel));
            this.lvUserSearchResults = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvUserSearchResults
            // 
            resources.ApplyResources(this.lvUserSearchResults, "lvUserSearchResults");
            this.lvUserSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvUserSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvUserSearchResults.FullRowSelect = true;
            this.lvUserSearchResults.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvUserSearchResults.Groups")))});
            this.lvUserSearchResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvUserSearchResults.HideSelection = false;
            this.lvUserSearchResults.LockDrawing = false;
            this.lvUserSearchResults.MultiSelect = false;
            this.lvUserSearchResults.Name = "lvUserSearchResults";
            this.lvUserSearchResults.ShowGroups = false;
            this.lvUserSearchResults.SortColumn = -1;
            this.lvUserSearchResults.SortedBackwards = false;
            this.lvUserSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvUserSearchResults.UseEveryOtherRowColoring = true;
            this.lvUserSearchResults.View = System.Windows.Forms.View.Details;
            this.lvUserSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvUserSearchResults_SelectedIndexChanged);
            this.lvUserSearchResults.Resize += new System.EventHandler(this.lvUserSearchResults_Resize);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // UserSearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvUserSearchResults);
            this.Name = "UserSearchPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvUserSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
