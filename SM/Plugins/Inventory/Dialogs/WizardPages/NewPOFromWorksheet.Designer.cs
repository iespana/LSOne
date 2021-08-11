namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewPOFromWorksheet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewPOFromWorksheet));
            this.lvPurchaseWorksheets = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // lvPurchaseWorksheets
            // 
            resources.ApplyResources(this.lvPurchaseWorksheets, "lvPurchaseWorksheets");
            this.lvPurchaseWorksheets.BuddyControl = null;
            this.lvPurchaseWorksheets.Columns.Add(this.column1);
            this.lvPurchaseWorksheets.Columns.Add(this.column2);
            this.lvPurchaseWorksheets.ContentBackColor = System.Drawing.Color.White;
            this.lvPurchaseWorksheets.DefaultRowHeight = ((short)(22));
            this.lvPurchaseWorksheets.DimSelectionWhenDisabled = true;
            this.lvPurchaseWorksheets.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPurchaseWorksheets.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvPurchaseWorksheets.HeaderHeight = ((short)(25));
            this.lvPurchaseWorksheets.Name = "lvPurchaseWorksheets";
            this.lvPurchaseWorksheets.OddRowColor = System.Drawing.Color.White;
            this.lvPurchaseWorksheets.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPurchaseWorksheets.SecondarySortColumn = ((short)(-1));
            this.lvPurchaseWorksheets.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPurchaseWorksheets.SortSetting = "0:1";
            this.lvPurchaseWorksheets.SelectionChanged += new System.EventHandler(this.lvPurchaseWorksheets_SelectionChanged);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // NewPOFromWorksheet
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lvPurchaseWorksheets);
            this.Name = "NewPOFromWorksheet";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvPurchaseWorksheets;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
    }
}
