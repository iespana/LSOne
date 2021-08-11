using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals
{
    partial class TerminalSearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalSearchPanel));
            this.lvTerminalSearchResults = new ExtendedListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvTerminalSearchResults
            // 
            resources.ApplyResources(this.lvTerminalSearchResults, "lvTerminalSearchResults");
            this.lvTerminalSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvTerminalSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvTerminalSearchResults.FullRowSelect = true;
            this.lvTerminalSearchResults.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvTerminalSearchResults.Groups")))});
            this.lvTerminalSearchResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTerminalSearchResults.HideSelection = false;
            this.lvTerminalSearchResults.MultiSelect = false;
            this.lvTerminalSearchResults.Name = "lvTerminalSearchResults";
            this.lvTerminalSearchResults.ShowGroups = false;
            this.lvTerminalSearchResults.SortColumn = -1;
            this.lvTerminalSearchResults.SortedBackwards = false;
            this.lvTerminalSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvTerminalSearchResults.View = System.Windows.Forms.View.Details;
            this.lvTerminalSearchResults.Resize += new System.EventHandler(this.lvUserSearchResults_Resize);
            this.lvTerminalSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvTerminalSearchResults_SelectedIndexChanged);
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
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // TerminalSearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvTerminalSearchResults);
            this.Name = "TerminalSearchPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvTerminalSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
