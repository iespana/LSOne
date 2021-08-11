using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class VendorItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendorItemsPage));
            this.btnViewRetailItem = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditAllLines = new System.Windows.Forms.Button();
            this.btnUnitConversions = new System.Windows.Forms.Button();
            this.lvItems = new LSOne.Controls.ListView();
            this.colVendorItemID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.colUnit = new LSOne.Controls.Columns.Column();
            this.colDefaultPurchasePrice = new LSOne.Controls.Columns.Column();
            this.colLastPurchPrice = new LSOne.Controls.Columns.Column();
            this.colLastOrderingDate = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.vendoritemsDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnViewRetailItem
            // 
            resources.ApplyResources(this.btnViewRetailItem, "btnViewRetailItem");
            this.btnViewRetailItem.Name = "btnViewRetailItem";
            this.btnViewRetailItem.UseVisualStyleBackColor = true;
            this.btnViewRetailItem.Click += new System.EventHandler(this.btnViewRetailItem_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnEditAllLines, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUnitConversions, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnViewRetailItem, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnEditAllLines
            // 
            resources.ApplyResources(this.btnEditAllLines, "btnEditAllLines");
            this.btnEditAllLines.Name = "btnEditAllLines";
            this.btnEditAllLines.UseVisualStyleBackColor = true;
            this.btnEditAllLines.Click += new System.EventHandler(this.btnEditAllLines_Click);
            // 
            // btnUnitConversions
            // 
            resources.ApplyResources(this.btnUnitConversions, "btnUnitConversions");
            this.btnUnitConversions.Name = "btnUnitConversions";
            this.btnUnitConversions.UseVisualStyleBackColor = true;
            this.btnUnitConversions.Click += new System.EventHandler(this.btnUnitConversions_Click);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colVendorItemID);
            this.lvItems.Columns.Add(this.colDescription);
            this.lvItems.Columns.Add(this.colVariant);
            this.lvItems.Columns.Add(this.colUnit);
            this.lvItems.Columns.Add(this.colDefaultPurchasePrice);
            this.lvItems.Columns.Add(this.colLastPurchPrice);
            this.lvItems.Columns.Add(this.colLastOrderingDate);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.HorizontalScrollbar = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItems_HeaderClicked);
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // colVendorItemID
            // 
            this.colVendorItemID.AutoSize = true;
            this.colVendorItemID.DefaultStyle = null;
            resources.ApplyResources(this.colVendorItemID, "colVendorItemID");
            this.colVendorItemID.MaximumWidth = ((short)(0));
            this.colVendorItemID.MinimumWidth = ((short)(10));
            this.colVendorItemID.SecondarySortColumn = ((short)(-1));
            this.colVendorItemID.Tag = null;
            this.colVendorItemID.Width = ((short)(50));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colVariant
            // 
            this.colVariant.AutoSize = true;
            this.colVariant.DefaultStyle = null;
            this.colVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariant, "colVariant");
            this.colVariant.MaximumWidth = ((short)(0));
            this.colVariant.MinimumWidth = ((short)(10));
            this.colVariant.NoTextWhenSmall = true;
            this.colVariant.SecondarySortColumn = ((short)(-1));
            this.colVariant.Tag = null;
            this.colVariant.Width = ((short)(50));
            // 
            // colUnit
            // 
            this.colUnit.AutoSize = true;
            this.colUnit.DefaultStyle = null;
            resources.ApplyResources(this.colUnit, "colUnit");
            this.colUnit.MaximumWidth = ((short)(0));
            this.colUnit.MinimumWidth = ((short)(10));
            this.colUnit.SecondarySortColumn = ((short)(-1));
            this.colUnit.Tag = null;
            this.colUnit.Width = ((short)(50));
            // 
            // colDefaultPurchasePrice
            // 
            this.colDefaultPurchasePrice.AutoSize = true;
            this.colDefaultPurchasePrice.DefaultStyle = null;
            resources.ApplyResources(this.colDefaultPurchasePrice, "colDefaultPurchasePrice");
            this.colDefaultPurchasePrice.MaximumWidth = ((short)(0));
            this.colDefaultPurchasePrice.MinimumWidth = ((short)(10));
            this.colDefaultPurchasePrice.SecondarySortColumn = ((short)(-1));
            this.colDefaultPurchasePrice.Tag = null;
            this.colDefaultPurchasePrice.Width = ((short)(50));
            // 
            // colLastPurchPrice
            // 
            this.colLastPurchPrice.AutoSize = true;
            this.colLastPurchPrice.DefaultStyle = null;
            resources.ApplyResources(this.colLastPurchPrice, "colLastPurchPrice");
            this.colLastPurchPrice.MaximumWidth = ((short)(0));
            this.colLastPurchPrice.MinimumWidth = ((short)(10));
            this.colLastPurchPrice.SecondarySortColumn = ((short)(-1));
            this.colLastPurchPrice.Tag = null;
            this.colLastPurchPrice.Width = ((short)(50));
            // 
            // colLastOrderingDate
            // 
            this.colLastOrderingDate.AutoSize = true;
            this.colLastOrderingDate.DefaultStyle = null;
            resources.ApplyResources(this.colLastOrderingDate, "colLastOrderingDate");
            this.colLastOrderingDate.MaximumWidth = ((short)(0));
            this.colLastOrderingDate.MinimumWidth = ((short)(10));
            this.colLastOrderingDate.SecondarySortColumn = ((short)(-1));
            this.colLastOrderingDate.Tag = null;
            this.colLastOrderingDate.Width = ((short)(50));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar_UnknownControlAdd);
            this.searchBar.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar_UnknownControlRemove);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // vendoritemsDataScroll
            // 
            resources.ApplyResources(this.vendoritemsDataScroll, "vendoritemsDataScroll");
            this.vendoritemsDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.vendoritemsDataScroll.Name = "vendoritemsDataScroll";
            this.vendoritemsDataScroll.PageSize = 0;
            this.vendoritemsDataScroll.PageChanged += new System.EventHandler(this.vendoritemsDataScroll_PageChanged);
            // 
            // VendorItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.vendoritemsDataScroll);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnsEditAddRemove);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "VendorItemsPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.Button btnViewRetailItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnUnitConversions;
        private System.Windows.Forms.Button btnEditAllLines;
        private ListView lvItems;
        private Controls.Columns.Column colVendorItemID;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colLastPurchPrice;
        private Controls.Columns.Column colLastOrderingDate;
        private Controls.Columns.Column colUnit;
        private Controls.Columns.Column colVariant;
        private Controls.Columns.Column colDefaultPurchasePrice;
        private DatabasePageDisplay vendoritemsDataScroll;
        private SearchBar searchBar;
    }
}
