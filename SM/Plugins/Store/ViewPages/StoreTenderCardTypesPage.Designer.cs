using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreTenderCardTypesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTenderCardTypesPage));
            this.lvCardTypes = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEditCardTypes = new System.Windows.Forms.Button();
            this.btnsContextButtons = new ContextButtons();
            this.SuspendLayout();
            // 
            // lvCardTypes
            // 
            resources.ApplyResources(this.lvCardTypes, "lvCardTypes");
            this.lvCardTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5});
            this.lvCardTypes.FullRowSelect = true;
            this.lvCardTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCardTypes.HideSelection = false;
            this.lvCardTypes.LockDrawing = false;
            this.lvCardTypes.MultiSelect = false;
            this.lvCardTypes.Name = "lvCardTypes";
            this.lvCardTypes.OwnerDraw = true;
            this.lvCardTypes.SortColumn = -1;
            this.lvCardTypes.SortedBackwards = false;
            this.lvCardTypes.UseCompatibleStateImageBehavior = false;
            this.lvCardTypes.UseEveryOtherRowColoring = true;
            this.lvCardTypes.View = System.Windows.Forms.View.Details;
            this.lvCardTypes.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvCardTypes_DrawColumnHeader);
            this.lvCardTypes.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvCardTypes_DrawItem);
            this.lvCardTypes.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvCardTypes_DrawSubItem);
            this.lvCardTypes.SelectedIndexChanged += new System.EventHandler(this.lvCardTypes_SelectedIndexChanged);
            this.lvCardTypes.SizeChanged += new System.EventHandler(this.lvCardTypes_SizeChanged);
            this.lvCardTypes.DoubleClick += new System.EventHandler(this.lvCardTypes_DoubleClick);
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
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // btnEditCardTypes
            // 
            resources.ApplyResources(this.btnEditCardTypes, "btnEditCardTypes");
            this.btnEditCardTypes.Name = "btnEditCardTypes";
            this.btnEditCardTypes.UseVisualStyleBackColor = true;
            this.btnEditCardTypes.Click += new System.EventHandler(this.btnEditCardTypes_Click);
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
            // StoreTenderCardTypesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.btnEditCardTypes);
            this.Controls.Add(this.lvCardTypes);
            this.DoubleBuffered = true;
            this.Name = "StoreTenderCardTypesPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvCardTypes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnEditCardTypes;
        private ContextButtons btnsContextButtons;
    }
}
