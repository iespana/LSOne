using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    partial class TouchButtonListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TouchButtonListView));
            this.lvLayouts = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new ContextButtons();
            this.btnButtonGridMenus = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.importDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvLayouts);
            this.pnlBottom.Controls.Add(this.btnExport);
            this.pnlBottom.Controls.Add(this.btnButtonGridMenus);
            this.pnlBottom.Controls.Add(this.btnImport);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lvLayouts
            // 
            resources.ApplyResources(this.lvLayouts, "lvLayouts");
            this.lvLayouts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.importDateTime});
            this.lvLayouts.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lvLayouts.FullRowSelect = true;
            this.lvLayouts.HideSelection = false;
            this.lvLayouts.LockDrawing = false;
            this.lvLayouts.Name = "lvLayouts";
            this.lvLayouts.SortColumn = -1;
            this.lvLayouts.SortedBackwards = false;
            this.lvLayouts.UseCompatibleStateImageBehavior = false;
            this.lvLayouts.UseEveryOtherRowColoring = true;
            this.lvLayouts.View = System.Windows.Forms.View.Details;
            this.lvLayouts.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvActions_ColumnClick);
            this.lvLayouts.SelectedIndexChanged += new System.EventHandler(this.lvActions_SelectedIndexChanged);
            this.lvLayouts.DoubleClick += new System.EventHandler(this.lvActions_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnButtonGridMenus
            // 
            resources.ApplyResources(this.btnButtonGridMenus, "btnButtonGridMenus");
            this.btnButtonGridMenus.Name = "btnButtonGridMenus";
            this.btnButtonGridMenus.UseVisualStyleBackColor = true;
            this.btnButtonGridMenus.Click += new System.EventHandler(this.btnButtonGridMenus_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // importDateTime
            // 
            resources.ApplyResources(this.importDateTime, "importDateTime");
            // 
            // TouchButtonListView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TouchButtonListView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvLayouts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.Button btnButtonGridMenus;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ColumnHeader importDateTime;

    }
}
