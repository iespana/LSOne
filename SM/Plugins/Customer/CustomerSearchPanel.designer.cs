using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer
{
    partial class CustomerSearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerSearchPanel));
            this.lvCustomerSearchResults = new ExtendedListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvCustomerSearchResults
            // 
            resources.ApplyResources(this.lvCustomerSearchResults, "lvCustomerSearchResults");
            this.lvCustomerSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvCustomerSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvCustomerSearchResults.FullRowSelect = true;
            this.lvCustomerSearchResults.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvCustomerSearchResults.Groups")))});
            this.lvCustomerSearchResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCustomerSearchResults.HideSelection = false;
            this.lvCustomerSearchResults.MultiSelect = false;
            this.lvCustomerSearchResults.Name = "lvCustomerSearchResults";
            this.lvCustomerSearchResults.ShowGroups = false;
            this.lvCustomerSearchResults.SortColumn = -1;
            this.lvCustomerSearchResults.SortedBackwards = false;
            this.lvCustomerSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvCustomerSearchResults.View = System.Windows.Forms.View.Details;
            this.lvCustomerSearchResults.Resize += new System.EventHandler(this.lvUserSearchResults_Resize);
            this.lvCustomerSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvCustomerSearchResults_SelectedIndexChanged);
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
            // CustomerSearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvCustomerSearchResults);
            this.Name = "CustomerSearchPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvCustomerSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
