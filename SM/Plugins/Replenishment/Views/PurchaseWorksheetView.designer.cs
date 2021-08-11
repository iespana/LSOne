using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    partial class PurchaseWorksheetView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseWorksheetView));
            this.lvPurchaseWorksheet = new LSOne.Controls.ListView();
            this.clBarcode = new LSOne.Controls.Columns.Column();
            this.clItemId = new LSOne.Controls.Columns.Column();
            this.clDescription = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.clVendor = new LSOne.Controls.Columns.Column();
            this.clQuantity = new LSOne.Controls.Columns.Column();
            this.clSuggestedQuantity = new LSOne.Controls.Columns.Column();
            this.clInventoryOnHand = new LSOne.Controls.Columns.Column();
            this.clReorderPoint = new LSOne.Controls.Columns.Column();
            this.clMaxInventory = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.btnsLines = new LSOne.Controls.ContextButtons();
            this.btnPost = new System.Windows.Forms.Button();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            this.pnlBottom.Controls.Add(this.btnPost);
            this.pnlBottom.Controls.Add(this.btnsLines);
            this.pnlBottom.Controls.Add(this.lvPurchaseWorksheet);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lvPurchaseWorksheet
            // 
            resources.ApplyResources(this.lvPurchaseWorksheet, "lvPurchaseWorksheet");
            this.lvPurchaseWorksheet.BuddyControl = null;
            this.lvPurchaseWorksheet.Columns.Add(this.clItemId);
            this.lvPurchaseWorksheet.Columns.Add(this.clDescription);
            this.lvPurchaseWorksheet.Columns.Add(this.colVariant);
            this.lvPurchaseWorksheet.Columns.Add(this.clBarcode);
            this.lvPurchaseWorksheet.Columns.Add(this.clVendor);
            this.lvPurchaseWorksheet.Columns.Add(this.clQuantity);
            this.lvPurchaseWorksheet.Columns.Add(this.clSuggestedQuantity);
            this.lvPurchaseWorksheet.Columns.Add(this.clInventoryOnHand);
            this.lvPurchaseWorksheet.Columns.Add(this.clReorderPoint);
            this.lvPurchaseWorksheet.Columns.Add(this.clMaxInventory);
            this.lvPurchaseWorksheet.Columns.Add(this.column7);
            this.lvPurchaseWorksheet.ContentBackColor = System.Drawing.Color.White;
            this.lvPurchaseWorksheet.DefaultRowHeight = ((short)(22));
            this.lvPurchaseWorksheet.DimSelectionWhenDisabled = true;
            this.lvPurchaseWorksheet.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPurchaseWorksheet.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvPurchaseWorksheet.HeaderHeight = ((short)(25));
            this.lvPurchaseWorksheet.HorizontalScrollbar = true;
            this.lvPurchaseWorksheet.Name = "lvPurchaseWorksheet";
            this.lvPurchaseWorksheet.OddRowColor = System.Drawing.Color.White;
            this.lvPurchaseWorksheet.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPurchaseWorksheet.SecondarySortColumn = ((short)(-1));
            this.lvPurchaseWorksheet.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvPurchaseWorksheet.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPurchaseWorksheet.SortSetting = "3:1";
            this.lvPurchaseWorksheet.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvPurchaseWorksheet_HeaderClicked);
            this.lvPurchaseWorksheet.SelectionChanged += new System.EventHandler(this.lvPurchaseWorksheets_SelectionChanged);
            this.lvPurchaseWorksheet.CellAction += new LSOne.Controls.CellActionDelegate(this.lvPurchaseWorksheet_CellAction);
            this.lvPurchaseWorksheet.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvPurchaseWorksheets_RowDoubleClick);
            // 
            // clBarcode
            // 
            this.clBarcode.AutoSize = true;
            this.clBarcode.DefaultStyle = null;
            resources.ApplyResources(this.clBarcode, "clBarcode");
            this.clBarcode.MaximumWidth = ((short)(0));
            this.clBarcode.MinimumWidth = ((short)(10));
            this.clBarcode.RelativeSize = 0;
            this.clBarcode.SecondarySortColumn = ((short)(-1));
            this.clBarcode.Tag = null;
            this.clBarcode.Width = ((short)(100));
            // 
            // clItemId
            // 
            this.clItemId.AutoSize = true;
            this.clItemId.DefaultStyle = null;
            resources.ApplyResources(this.clItemId, "clItemId");
            this.clItemId.MaximumWidth = ((short)(0));
            this.clItemId.MinimumWidth = ((short)(10));
            this.clItemId.SecondarySortColumn = ((short)(-1));
            this.clItemId.Tag = null;
            this.clItemId.Width = ((short)(100));
            // 
            // clDescription
            // 
            this.clDescription.AutoSize = true;
            this.clDescription.DefaultStyle = null;
            resources.ApplyResources(this.clDescription, "clDescription");
            this.clDescription.MaximumWidth = ((short)(0));
            this.clDescription.MinimumWidth = ((short)(10));
            this.clDescription.SecondarySortColumn = ((short)(-1));
            this.clDescription.Tag = null;
            this.clDescription.Width = ((short)(50));
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
            // clVendor
            // 
            this.clVendor.AutoSize = true;
            this.clVendor.DefaultStyle = null;
            resources.ApplyResources(this.clVendor, "clVendor");
            this.clVendor.MaximumWidth = ((short)(0));
            this.clVendor.MinimumWidth = ((short)(10));
            this.clVendor.SecondarySortColumn = ((short)(-1));
            this.clVendor.Tag = null;
            this.clVendor.Width = ((short)(100));
            // 
            // clQuantity
            // 
            this.clQuantity.AutoSize = true;
            this.clQuantity.DefaultStyle = null;
            resources.ApplyResources(this.clQuantity, "clQuantity");
            this.clQuantity.MaximumWidth = ((short)(0));
            this.clQuantity.MinimumWidth = ((short)(10));
            this.clQuantity.SecondarySortColumn = ((short)(-1));
            this.clQuantity.Tag = null;
            this.clQuantity.Width = ((short)(100));
            // 
            // clSuggestedQuantity
            // 
            this.clSuggestedQuantity.AutoSize = true;
            this.clSuggestedQuantity.DefaultStyle = null;
            resources.ApplyResources(this.clSuggestedQuantity, "clSuggestedQuantity");
            this.clSuggestedQuantity.MaximumWidth = ((short)(0));
            this.clSuggestedQuantity.MinimumWidth = ((short)(10));
            this.clSuggestedQuantity.SecondarySortColumn = ((short)(-1));
            this.clSuggestedQuantity.Tag = null;
            this.clSuggestedQuantity.Width = ((short)(50));
            // 
            // clInventoryOnHand
            // 
            this.clInventoryOnHand.AutoSize = true;
            this.clInventoryOnHand.Clickable = false;
            this.clInventoryOnHand.DefaultStyle = null;
            resources.ApplyResources(this.clInventoryOnHand, "clInventoryOnHand");
            this.clInventoryOnHand.MaximumWidth = ((short)(0));
            this.clInventoryOnHand.MinimumWidth = ((short)(10));
            this.clInventoryOnHand.SecondarySortColumn = ((short)(-1));
            this.clInventoryOnHand.Tag = null;
            this.clInventoryOnHand.Width = ((short)(20));
            // 
            // clReorderPoint
            // 
            this.clReorderPoint.AutoSize = true;
            this.clReorderPoint.DefaultStyle = null;
            resources.ApplyResources(this.clReorderPoint, "clReorderPoint");
            this.clReorderPoint.MaximumWidth = ((short)(0));
            this.clReorderPoint.MinimumWidth = ((short)(10));
            this.clReorderPoint.SecondarySortColumn = ((short)(-1));
            this.clReorderPoint.Tag = null;
            this.clReorderPoint.Width = ((short)(50));
            // 
            // clMaxInventory
            // 
            this.clMaxInventory.AutoSize = true;
            this.clMaxInventory.DefaultStyle = null;
            resources.ApplyResources(this.clMaxInventory, "clMaxInventory");
            this.clMaxInventory.MaximumWidth = ((short)(0));
            this.clMaxInventory.MinimumWidth = ((short)(10));
            this.clMaxInventory.SecondarySortColumn = ((short)(-1));
            this.clMaxInventory.Tag = null;
            this.clMaxInventory.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.Clickable = false;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // btnsLines
            // 
            this.btnsLines.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsLines, "btnsLines");
            this.btnsLines.BackColor = System.Drawing.Color.Transparent;
            this.btnsLines.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsLines.EditButtonEnabled = false;
            this.btnsLines.Name = "btnsLines";
            this.btnsLines.RemoveButtonEnabled = false;
            this.btnsLines.EditButtonClicked += new System.EventHandler(this.btnsLines_EditButtonClicked);
            this.btnsLines.AddButtonClicked += new System.EventHandler(this.btnsLines_AddButtonClicked);
            this.btnsLines.RemoveButtonClicked += new System.EventHandler(this.btnsLines_RemoveButtonClicked);
            // 
            // btnPost
            // 
            resources.ApplyResources(this.btnPost, "btnPost");
            this.btnPost.Name = "btnPost";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // PurchaseWorksheetView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PurchaseWorksheetView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvPurchaseWorksheet;
        private Column clBarcode;
        private Column clItemId;
        private Column clQuantity;
        private Column clVendor;
        private ContextButtons btnsLines;
        private Column clSuggestedQuantity;
        private Column clReorderPoint;
        private Column clMaxInventory;
        private Column clDescription;
        private System.Windows.Forms.Button btnPost;
        private Column column7;
        private Column clInventoryOnHand;
        private Column colVariant;
        private DatabasePageDisplay itemDataScroll;
    }
}
