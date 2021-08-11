using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    partial class AdministrationDecimalsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationDecimalsPage));
            this.lvDecimals = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEditValue = new ContextButton();
            this.SuspendLayout();
            // 
            // lvDecimals
            // 
            resources.ApplyResources(this.lvDecimals, "lvDecimals");
            this.lvDecimals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader7});
            this.lvDecimals.FullRowSelect = true;
            this.lvDecimals.HideSelection = false;
            this.lvDecimals.LockDrawing = false;
            this.lvDecimals.MultiSelect = false;
            this.lvDecimals.Name = "lvDecimals";
            this.lvDecimals.SortColumn = -1;
            this.lvDecimals.SortedBackwards = false;
            this.lvDecimals.UseCompatibleStateImageBehavior = false;
            this.lvDecimals.UseEveryOtherRowColoring = true;
            this.lvDecimals.View = System.Windows.Forms.View.Details;
            this.lvDecimals.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDecimals_ColumnClick);
            this.lvDecimals.SelectedIndexChanged += new System.EventHandler(this.lvDecimals_SelectedIndexChanged);
            this.lvDecimals.DoubleClick += new System.EventHandler(this.lvDecimals_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // btnEditValue
            // 
            resources.ApplyResources(this.btnEditValue, "btnEditValue");
            this.btnEditValue.Context = ButtonType.Edit;
            this.btnEditValue.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnEditValue.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnEditValue.Name = "btnEditValue";
            this.btnEditValue.Click += new System.EventHandler(this.btnEditValue_Click);
            // 
            // AdministrationDecimalsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditValue);
            this.Controls.Add(this.lvDecimals);
            this.Name = "AdministrationDecimalsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvDecimals;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private ContextButton btnEditValue;


    }
}
