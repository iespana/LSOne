namespace LSOne.ViewPlugins.VariantFramework.ViewPages
{
    partial class ItemViewVariantItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemViewVariantItemsPage));
            this.btnEditDimension = new System.Windows.Forms.Button();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvVariantItems = new LSOne.Controls.ListView();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.SuspendLayout();
            // 
            // btnEditDimension
            // 
            resources.ApplyResources(this.btnEditDimension, "btnEditDimension");
            this.btnEditDimension.BackColor = System.Drawing.Color.Transparent;
            this.btnEditDimension.Name = "btnEditDimension";
            this.btnEditDimension.UseVisualStyleBackColor = true;
            this.btnEditDimension.Click += new System.EventHandler(this.btnEditDimension_Click);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = true;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = true;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
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
            this.lvVariantItems.HorizontalScrollbar = true;
            this.lvVariantItems.Name = "lvVariantItems";
            this.lvVariantItems.OddRowColor = System.Drawing.Color.White;
            this.lvVariantItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvVariantItems.SecondarySortColumn = ((short)(-1));
            this.lvVariantItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvVariantItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvVariantItems.SortSetting = "-1:1";
            this.lvVariantItems.SelectionChanged += new System.EventHandler(this.lvVariantItems_SelectionChanged);
            this.lvVariantItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvVariantItems_RowDoubleClick);
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
            // ItemViewVariantItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.lvVariantItems);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.btnEditDimension);
            this.DoubleBuffered = true;
            this.Name = "ItemViewVariantItemsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnEditDimension;
        private Controls.ContextButtons btnsContextButtons;
        private Controls.ListView lvVariantItems;
        private Controls.SearchBar searchBar;
    }
}
