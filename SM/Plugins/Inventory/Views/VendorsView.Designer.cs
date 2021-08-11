using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class VendorsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendorsView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvVendors = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colPhone = new LSOne.Controls.Columns.Column();
            this.colAddress = new LSOne.Controls.Columns.Column();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.colName = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvVendors);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
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
            // lvVendors
            // 
            resources.ApplyResources(this.lvVendors, "lvVendors");
            this.lvVendors.BuddyControl = null;
            this.lvVendors.Columns.Add(this.colID);
            this.lvVendors.Columns.Add(this.colName);
            this.lvVendors.Columns.Add(this.colPhone);
            this.lvVendors.Columns.Add(this.colAddress);
            this.lvVendors.ContentBackColor = System.Drawing.Color.White;
            this.lvVendors.DefaultRowHeight = ((short)(22));
            this.lvVendors.DimSelectionWhenDisabled = true;
            this.lvVendors.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvVendors.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvVendors.HeaderHeight = ((short)(25));
            this.lvVendors.HorizontalScrollbar = true;
            this.lvVendors.Name = "lvVendors";
            this.lvVendors.OddRowColor = System.Drawing.Color.White;
            this.lvVendors.RowLineColor = System.Drawing.Color.LightGray;
            this.lvVendors.SecondarySortColumn = ((short)(-1));
            this.lvVendors.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvVendors.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvVendors.SortSetting = "0:1";
            this.lvVendors.SelectionChanged += new System.EventHandler(this.lvVendors_SelectedIndexChanged);
            this.lvVendors.DoubleClick += new System.EventHandler(this.lvVendors_DoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.InternalSort = true;
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(50));
            // 
            // colPhone
            // 
            this.colPhone.AutoSize = true;
            this.colPhone.Clickable = false;
            this.colPhone.DefaultStyle = null;
            resources.ApplyResources(this.colPhone, "colPhone");
            this.colPhone.MaximumWidth = ((short)(0));
            this.colPhone.MinimumWidth = ((short)(10));
            this.colPhone.SecondarySortColumn = ((short)(-1));
            this.colPhone.Tag = null;
            this.colPhone.Width = ((short)(50));
            // 
            // colAddress
            // 
            this.colAddress.AutoSize = true;
            this.colAddress.Clickable = false;
            this.colAddress.DefaultStyle = null;
            resources.ApplyResources(this.colAddress, "colAddress");
            this.colAddress.MaximumWidth = ((short)(0));
            this.colAddress.MinimumWidth = ((short)(10));
            this.colAddress.SecondarySortColumn = ((short)(-1));
            this.colAddress.Tag = null;
            this.colAddress.Width = ((short)(50));
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.InternalSort = true;
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.SecondarySortColumn = ((short)(-1));
            this.colName.Tag = null;
            this.colName.Width = ((short)(50));
            // 
            // VendorsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "VendorsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private ListView lvVendors;
        private SearchBar searchBar1;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colPhone;
        private Controls.Columns.Column colAddress;
        private Controls.Columns.Column colName;
    }
}
