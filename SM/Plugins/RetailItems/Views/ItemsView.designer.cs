using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    partial class ItemsView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemsView));
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.btnExport = new System.Windows.Forms.Button();
            this.exportContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnImport = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCopyID = new System.Windows.Forms.Button();
            this.importContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lvItems = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmVariant = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.flowLayoutPanel1);
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvItems);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Image = global::LSOne.ViewPlugins.RetailItems.Properties.Resources.export_16;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.btnExport.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnExport_MouseDown);
            // 
            // exportContextMenu
            // 
            this.exportContextMenu.Name = "exportContextMenu";
            resources.ApplyResources(this.exportContextMenu, "exportContextMenu");
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Image = global::LSOne.ViewPlugins.RetailItems.Properties.Resources.import_16;
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnImport_MouseDown);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.btnExport);
            this.flowLayoutPanel1.Controls.Add(this.btnImport);
            this.flowLayoutPanel1.Controls.Add(this.btnCopyID);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnCopyID
            // 
            resources.ApplyResources(this.btnCopyID, "btnCopyID");
            this.btnCopyID.Name = "btnCopyID";
            this.btnCopyID.UseVisualStyleBackColor = true;
            this.btnCopyID.Click += new System.EventHandler(this.CopyID);
            // 
            // importContextMenu
            // 
            this.importContextMenu.Name = "importContextMenu";
            resources.ApplyResources(this.importContextMenu, "importContextMenu");
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BorderColor = System.Drawing.Color.DarkGray;
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.column1);
            this.lvItems.Columns.Add(this.clmDescription);
            this.lvItems.Columns.Add(this.clmVariant);
            this.lvItems.Columns.Add(this.column4);
            this.lvItems.Columns.Add(this.column5);
            this.lvItems.Columns.Add(this.column6);
            this.lvItems.Columns.Add(this.column7);
            this.lvItems.Columns.Add(this.column2);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderBackColor = System.Drawing.Color.White;
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.HorizontalScrollbar = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.VerticalScrollbarValue = 0;
            this.lvItems.VerticalScrollbarYOffset = 0;
            this.lvItems.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItems_HeaderClicked);
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.CellAction += new LSOne.Controls.CellActionDelegate(this.lvItems_CellAction);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            this.lvItems.RowExpanded += new LSOne.Controls.RowClickDelegate(this.lvItems_RowExpanded);
            this.lvItems.RowCollapsed += new LSOne.Controls.RowClickDelegate(this.lvItems_RowCollapsed);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(2));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(100));
            // 
            // clmVariant
            // 
            this.clmVariant.AutoSize = true;
            this.clmVariant.DefaultStyle = null;
            this.clmVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.clmVariant, "clmVariant");
            this.clmVariant.MaximumWidth = ((short)(0));
            this.clmVariant.MinimumWidth = ((short)(5));
            this.clmVariant.NoTextWhenSmall = true;
            this.clmVariant.SecondarySortColumn = ((short)(1));
            this.clmVariant.Sizable = false;
            this.clmVariant.Tag = null;
            this.clmVariant.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(70));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(70));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(70));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.HasSearchOption = true;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionChecked = true;
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.SearchTypesChanged += new System.EventHandler(this.searchBar1_SearchTypesChanged);
            this.searchBar1.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar1_UnknownControlRemove);
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar1_UnknownControlSetSelection);
            this.searchBar1.SearchOptionChanged += new System.EventHandler(this.searchBar1_SearchOptionChanged);
            this.searchBar1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.searchBar1_ControlRemoved);
            // 
            // ItemsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ItemsView";
            this.Load += new System.EventHandler(this.ItemsView_Load);
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.pnlBottom.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DatabasePageDisplay itemDataScroll;
        private ContextButtons btnsContextButtons;
        private SearchBar searchBar1;
        private ListView lvItems;
        private Column column1;
        private Column clmDescription;
        private Column column4;
        private Column column5;
        private Column column6;
        private Column column7;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ContextMenuStrip exportContextMenu;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ContextMenuStrip importContextMenu;
        private Column column2;
        private Column clmVariant;
        private System.Windows.Forms.Button btnCopyID;
    }
}
