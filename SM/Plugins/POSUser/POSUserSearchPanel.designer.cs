using LSOne.Controls;

namespace LSOne.ViewPlugins.POSUser
{
    partial class POSUserSearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POSUserSearchPanel));
            this.lvPOSUsers = new ExtendedListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvPOSUsers
            // 
            resources.ApplyResources(this.lvPOSUsers, "lvPOSUsers");
            this.lvPOSUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvPOSUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvPOSUsers.FullRowSelect = true;
            this.lvPOSUsers.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvPOSUsers.Groups")))});
            this.lvPOSUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvPOSUsers.HideSelection = false;
            this.lvPOSUsers.MultiSelect = false;
            this.lvPOSUsers.Name = "lvPOSUsers";
            this.lvPOSUsers.ShowGroups = false;
            this.lvPOSUsers.SortColumn = -1;
            this.lvPOSUsers.SortedBackwards = false;
            this.lvPOSUsers.UseCompatibleStateImageBehavior = false;
            this.lvPOSUsers.View = System.Windows.Forms.View.Details;
            this.lvPOSUsers.Resize += new System.EventHandler(this.lvUserSearchResults_Resize);
            this.lvPOSUsers.SelectedIndexChanged += new System.EventHandler(this.lvPOSUsers_SelectedIndexChanged);
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
            // POSUserSearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvPOSUsers);
            this.Name = "POSUserSearchPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvPOSUsers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
