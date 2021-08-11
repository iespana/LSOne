using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    partial class PriceDiscountGroupStoresPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PriceDiscountGroupStoresPage));
            this.gpStores = new LSOne.Controls.GroupPanel();
            this.btnViewStore = new System.Windows.Forms.Button();
            this.contextButtonsStores = new LSOne.Controls.ContextButtons();
            this.lblStoresGroupHeader = new System.Windows.Forms.Label();
            this.lvStores = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.gpStores.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpStores
            // 
            resources.ApplyResources(this.gpStores, "gpStores");
            this.gpStores.Controls.Add(this.lvStores);
            this.gpStores.Controls.Add(this.btnViewStore);
            this.gpStores.Controls.Add(this.contextButtonsStores);
            this.gpStores.Controls.Add(this.lblStoresGroupHeader);
            this.gpStores.Name = "gpStores";
            // 
            // btnViewStore
            // 
            resources.ApplyResources(this.btnViewStore, "btnViewStore");
            this.btnViewStore.Name = "btnViewStore";
            this.btnViewStore.UseVisualStyleBackColor = true;
            this.btnViewStore.Click += new System.EventHandler(this.btnEditStore_Click);
            // 
            // contextButtonsStores
            // 
            this.contextButtonsStores.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtonsStores, "contextButtonsStores");
            this.contextButtonsStores.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonsStores.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextButtonsStores.EditButtonEnabled = false;
            this.contextButtonsStores.Name = "contextButtonsStores";
            this.contextButtonsStores.RemoveButtonEnabled = true;
            this.contextButtonsStores.AddButtonClicked += new System.EventHandler(this.btnAddStore_Click);
            this.contextButtonsStores.RemoveButtonClicked += new System.EventHandler(this.btnRemoveStore_Click);
            // 
            // lblStoresGroupHeader
            // 
            resources.ApplyResources(this.lblStoresGroupHeader, "lblStoresGroupHeader");
            this.lblStoresGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblStoresGroupHeader.Name = "lblStoresGroupHeader";
            // 
            // lvStores
            // 
            resources.ApplyResources(this.lvStores, "lvStores");
            this.lvStores.BuddyControl = null;
            this.lvStores.Columns.Add(this.column1);
            this.lvStores.Columns.Add(this.column2);
            this.lvStores.Columns.Add(this.column3);
            this.lvStores.ContentBackColor = System.Drawing.Color.White;
            this.lvStores.DefaultRowHeight = ((short)(22));
            this.lvStores.DimSelectionWhenDisabled = true;
            this.lvStores.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvStores.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvStores.HeaderHeight = ((short)(25));
            this.lvStores.HorizontalScrollbar = true;
            this.lvStores.Name = "lvStores";
            this.lvStores.OddRowColor = System.Drawing.Color.White;
            this.lvStores.RowLineColor = System.Drawing.Color.LightGray;
            this.lvStores.SecondarySortColumn = ((short)(-1));
            this.lvStores.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvStores.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvStores.SortSetting = "-1:1";
            this.lvStores.SelectionChanged += new System.EventHandler(this.lvStores_SelectionChanged);
            this.lvStores.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvStores_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // PriceDiscountGroupStoresPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpStores);
            this.Name = "PriceDiscountGroupStoresPage";
            this.gpStores.ResumeLayout(false);
            this.gpStores.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel gpStores;
        private System.Windows.Forms.Button btnViewStore;
        private ContextButtons contextButtonsStores;
        private System.Windows.Forms.Label lblStoresGroupHeader;
        private ListView lvStores;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
    }
}
