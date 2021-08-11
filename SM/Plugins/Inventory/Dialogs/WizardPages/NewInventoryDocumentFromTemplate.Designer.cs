namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewInventoryDocumentFromTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewInventoryDocumentFromTemplate));
            this.lvTemplates = new LSOne.Controls.ListView();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // lvTemplates
            // 
            resources.ApplyResources(this.lvTemplates, "lvTemplates");
            this.lvTemplates.BuddyControl = null;
            this.lvTemplates.Columns.Add(this.clmDescription);
            this.lvTemplates.Columns.Add(this.clmStore);
            this.lvTemplates.ContentBackColor = System.Drawing.Color.White;
            this.lvTemplates.DefaultRowHeight = ((short)(22));
            this.lvTemplates.DimSelectionWhenDisabled = true;
            this.lvTemplates.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTemplates.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvTemplates.HeaderHeight = ((short)(25));
            this.lvTemplates.Name = "lvTemplates";
            this.lvTemplates.OddRowColor = System.Drawing.Color.White;
            this.lvTemplates.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTemplates.SecondarySortColumn = ((short)(-1));
            this.lvTemplates.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTemplates.SortSetting = "0:1";
            this.lvTemplates.SelectionChanged += new System.EventHandler(this.lvTemplates_SelectionChanged);
            this.lvTemplates.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvTemplates_RowDoubleClick);
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.Clickable = false;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.Clickable = false;
            this.clmStore.DefaultStyle = null;
            resources.ApplyResources(this.clmStore, "clmStore");
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(10));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(50));
            // 
            // NewStockCountingFromTemplate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lvTemplates);
            this.Name = "NewStockCountingFromTemplate";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvTemplates;
        private Controls.Columns.Column clmDescription;
        private Controls.Columns.Column clmStore;
    }
}
