namespace LSOne.ViewPlugins.VariantFramework.DialogPages
{
    partial class NewRetailItemVariantsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRetailItemVariantsPage));
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lvVariantItems = new LSOne.Controls.ListView();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.SuspendLayout();
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            resources.ApplyResources(this.btnSelectNone, "btnSelectNone");
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // lvVariantItems
            // 
            resources.ApplyResources(this.lvVariantItems, "lvVariantItems");
            this.lvVariantItems.BuddyControl = null;
            this.lvVariantItems.ContentBackColor = System.Drawing.Color.White;
            this.lvVariantItems.DefaultRowHeight = ((short)(22));
            this.lvVariantItems.DimSelectionWhenDisabled = true;
            this.lvVariantItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvVariantItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvVariantItems.HeaderHeight = ((short)(25));
            this.lvVariantItems.Name = "lvVariantItems";
            this.lvVariantItems.OddRowColor = System.Drawing.Color.White;
            this.lvVariantItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvVariantItems.SecondarySortColumn = ((short)(-1));
            this.lvVariantItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvVariantItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvVariantItems.SortSetting = "-1:1";
            this.lvVariantItems.CellAction += new LSOne.Controls.CellActionDelegate(this.lvVariantItems_CellAction);
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
            // 
            // NewRetailItemVariantsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.lvVariantItems);
            this.DoubleBuffered = true;
            this.Name = "NewRetailItemVariantsPage";
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.ListView lvVariantItems;
        private Controls.SearchBar searchBar;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
    }
}
