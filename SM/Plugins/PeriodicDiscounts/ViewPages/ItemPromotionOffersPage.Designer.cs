using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    partial class ItemPromotionOffersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemPromotionOffersPage));
            this.lvValues = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.btnsContextButtonsItems = new LSOne.Controls.ContextButtons();
            this.btnShowOffer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.BuddyControl = null;
            this.lvValues.Columns.Add(this.column1);
            this.lvValues.Columns.Add(this.column2);
            this.lvValues.Columns.Add(this.column3);
            this.lvValues.Columns.Add(this.column4);
            this.lvValues.Columns.Add(this.column5);
            this.lvValues.Columns.Add(this.column6);
            this.lvValues.Columns.Add(this.column7);
            this.lvValues.Columns.Add(this.column8);
            this.lvValues.Columns.Add(this.column9);
            this.lvValues.ContentBackColor = System.Drawing.Color.White;
            this.lvValues.DefaultRowHeight = ((short)(22));
            this.lvValues.DimSelectionWhenDisabled = true;
            this.lvValues.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvValues.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvValues.HeaderHeight = ((short)(25));
            this.lvValues.HorizontalScrollbar = true;
            this.lvValues.Name = "lvValues";
            this.lvValues.OddRowColor = System.Drawing.Color.White;
            this.lvValues.RowLineColor = System.Drawing.Color.LightGray;
            this.lvValues.SecondarySortColumn = ((short)(-1));
            this.lvValues.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvValues.SortSetting = "0:1";
            this.lvValues.SelectionChanged += new System.EventHandler(this.lvValues_SelectionChanged);
            this.lvValues.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvValues_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
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
            this.column2.SecondarySortColumn = ((short)(-1));
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
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            this.column4.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.NoTextWhenSmall = true;
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.InternalSort = true;
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.InternalSort = true;
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.InternalSort = true;
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.SecondarySortColumn = ((short)(-1));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = false;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = LSOne.Controls.ButtonTypes.EditRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = false;
            this.btnsContextButtonsItems.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnShowOffer
            // 
            resources.ApplyResources(this.btnShowOffer, "btnShowOffer");
            this.btnShowOffer.Name = "btnShowOffer";
            this.btnShowOffer.UseVisualStyleBackColor = true;
            this.btnShowOffer.Click += new System.EventHandler(this.showOffer);
            // 
            // ItemPromotionOffersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnShowOffer);
            this.Controls.Add(this.lvValues);
            this.Controls.Add(this.btnsContextButtonsItems);
            this.Name = "ItemPromotionOffersPage";
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsContextButtonsItems;
        private ListView lvValues;
        private Controls.Columns.Column column9;
        private Controls.Columns.Column column8;
        private Controls.Columns.Column column7;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column1;
        private System.Windows.Forms.Button btnShowOffer;
    }
}
