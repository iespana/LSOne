using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    partial class InventoryTemplateFilterPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplateFilterPage));
            this.lblFilterResults = new System.Windows.Forms.Label();
            this.lblFirstShown = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lvPreview = new LSOne.Controls.ListView();
            this.clmID = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.clmInventoryUnit = new LSOne.Controls.Columns.Column();
            this.clmRetailGroup = new LSOne.Controls.Columns.Column();
            this.clmRetailDepartment = new LSOne.Controls.Columns.Column();
            this.clmVendor = new LSOne.Controls.Columns.Column();
            this.clmWarning = new LSOne.Controls.Columns.Column();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFilterResults
            // 
            resources.ApplyResources(this.lblFilterResults, "lblFilterResults");
            this.lblFilterResults.BackColor = System.Drawing.Color.Transparent;
            this.lblFilterResults.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblFilterResults.Name = "lblFilterResults";
            // 
            // lblFirstShown
            // 
            resources.ApplyResources(this.lblFirstShown, "lblFirstShown");
            this.lblFirstShown.BackColor = System.Drawing.Color.Transparent;
            this.lblFirstShown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblFirstShown.Name = "lblFirstShown";
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lvPreview);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvPreview
            // 
            resources.ApplyResources(this.lvPreview, "lvPreview");
            this.lvPreview.BuddyControl = null;
            this.lvPreview.Columns.Add(this.clmID);
            this.lvPreview.Columns.Add(this.clmDescription);
            this.lvPreview.Columns.Add(this.colVariant);
            this.lvPreview.Columns.Add(this.clmInventoryUnit);
            this.lvPreview.Columns.Add(this.clmRetailGroup);
            this.lvPreview.Columns.Add(this.clmRetailDepartment);
            this.lvPreview.Columns.Add(this.clmVendor);
            this.lvPreview.Columns.Add(this.clmWarning);
            this.lvPreview.ContentBackColor = System.Drawing.Color.White;
            this.lvPreview.DefaultRowHeight = ((short)(22));
            this.lvPreview.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPreview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPreview.HeaderHeight = ((short)(25));
            this.lvPreview.HorizontalScrollbar = true;
            this.lvPreview.Name = "lvPreview";
            this.lvPreview.OddRowColor = System.Drawing.Color.White;
            this.lvPreview.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPreview.SecondarySortColumn = ((short)(-1));
            this.lvPreview.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPreview.SortSetting = "0:1";
            this.lvPreview.CellAction += new LSOne.Controls.CellActionDelegate(this.lvPreview_CellAction);
            // 
            // clmID
            // 
            this.clmID.AutoSize = true;
            this.clmID.DefaultStyle = null;
            resources.ApplyResources(this.clmID, "clmID");
            this.clmID.InternalSort = true;
            this.clmID.MaximumWidth = ((short)(0));
            this.clmID.MinimumWidth = ((short)(10));
            this.clmID.SecondarySortColumn = ((short)(-1));
            this.clmID.Sizable = false;
            this.clmID.Tag = null;
            this.clmID.Width = ((short)(70));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.InternalSort = true;
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Sizable = false;
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(70));
            // 
            // colVariant
            // 
            this.colVariant.AutoSize = true;
            this.colVariant.DefaultStyle = null;
            this.colVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariant, "colVariant");
            this.colVariant.InternalSort = true;
            this.colVariant.MaximumWidth = ((short)(0));
            this.colVariant.MinimumWidth = ((short)(10));
            this.colVariant.NoTextWhenSmall = true;
            this.colVariant.SecondarySortColumn = ((short)(-1));
            this.colVariant.Tag = null;
            this.colVariant.Width = ((short)(50));
            // 
            // clmInventoryUnit
            // 
            this.clmInventoryUnit.AutoSize = true;
            this.clmInventoryUnit.DefaultStyle = null;
            resources.ApplyResources(this.clmInventoryUnit, "clmInventoryUnit");
            this.clmInventoryUnit.InternalSort = true;
            this.clmInventoryUnit.MaximumWidth = ((short)(0));
            this.clmInventoryUnit.MinimumWidth = ((short)(10));
            this.clmInventoryUnit.SecondarySortColumn = ((short)(-1));
            this.clmInventoryUnit.Sizable = false;
            this.clmInventoryUnit.Tag = null;
            this.clmInventoryUnit.Width = ((short)(50));
            // 
            // clmRetailGroup
            // 
            this.clmRetailGroup.AutoSize = true;
            this.clmRetailGroup.DefaultStyle = null;
            resources.ApplyResources(this.clmRetailGroup, "clmRetailGroup");
            this.clmRetailGroup.InternalSort = true;
            this.clmRetailGroup.MaximumWidth = ((short)(0));
            this.clmRetailGroup.MinimumWidth = ((short)(10));
            this.clmRetailGroup.SecondarySortColumn = ((short)(-1));
            this.clmRetailGroup.Sizable = false;
            this.clmRetailGroup.Tag = null;
            this.clmRetailGroup.Width = ((short)(70));
            // 
            // clmRetailDepartment
            // 
            this.clmRetailDepartment.AutoSize = true;
            this.clmRetailDepartment.DefaultStyle = null;
            resources.ApplyResources(this.clmRetailDepartment, "clmRetailDepartment");
            this.clmRetailDepartment.InternalSort = true;
            this.clmRetailDepartment.MaximumWidth = ((short)(0));
            this.clmRetailDepartment.MinimumWidth = ((short)(10));
            this.clmRetailDepartment.SecondarySortColumn = ((short)(-1));
            this.clmRetailDepartment.Sizable = false;
            this.clmRetailDepartment.Tag = null;
            this.clmRetailDepartment.Width = ((short)(70));
            // 
            // clmVendor
            // 
            this.clmVendor.AutoSize = true;
            this.clmVendor.DefaultStyle = null;
            resources.ApplyResources(this.clmVendor, "clmVendor");
            this.clmVendor.InternalSort = true;
            this.clmVendor.MaximumWidth = ((short)(0));
            this.clmVendor.MinimumWidth = ((short)(10));
            this.clmVendor.SecondarySortColumn = ((short)(-1));
            this.clmVendor.Tag = null;
            this.clmVendor.Width = ((short)(50));
            // 
            // clmWarning
            // 
            this.clmWarning.AutoSize = true;
            this.clmWarning.DefaultStyle = null;
            resources.ApplyResources(this.clmWarning, "clmWarning");
            this.clmWarning.InternalSort = true;
            this.clmWarning.MaximumWidth = ((short)(0));
            this.clmWarning.MinimumWidth = ((short)(10));
            this.clmWarning.SecondarySortColumn = ((short)(-1));
            this.clmWarning.Tag = null;
            this.clmWarning.Width = ((short)(50));
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.CanLoadDefault = false;
            this.searchBar1.CanSaveDefault = false;
            this.searchBar1.EnableCheckbox = false;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.AdjustSize += new LSOne.Controls.AdjustSizeDelegate(this.searchBar1_AdjustSize);
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.ResetSections += new System.EventHandler(this.searchBar1_ResetSections);
            this.searchBar1.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar1_UnknownControlRemove);
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar1_UnknownControlSetSelection);
            // 
            // InventoryTemplateFilterPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.lblFirstShown);
            this.Controls.Add(this.lblFilterResults);
            this.Controls.Add(this.groupPanel1);
            this.Name = "InventoryTemplateFilterPage";
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblFilterResults;
        private System.Windows.Forms.Label lblFirstShown;
        private SearchBar searchBar1;
        private ListView lvPreview;
        private Column clmID;
        private Column clmDescription;
        private Column clmInventoryUnit;
        private Column clmRetailGroup;
        private Column clmRetailDepartment;
        private Column clmVendor;
        private Column clmWarning;
        private Column colVariant;
    }
}
