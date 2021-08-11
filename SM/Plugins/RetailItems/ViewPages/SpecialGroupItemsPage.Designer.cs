using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class SpecialGroupItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecialGroupItemsPage));
            this.btnsContextButtonsItems = new LSOne.Controls.ContextButtons();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvItemsList = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.btnViewItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = false;
            this.btnsContextButtonsItems.EditButtonClicked += new System.EventHandler(this.btnEditItem_Click);
            this.btnsContextButtonsItems.AddButtonClicked += new System.EventHandler(this.btnAddItem_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // lvItemsList
            // 
            resources.ApplyResources(this.lvItemsList, "lvItemsList");
            this.lvItemsList.BuddyControl = null;
            this.lvItemsList.Columns.Add(this.column1);
            this.lvItemsList.Columns.Add(this.column2);
            this.lvItemsList.Columns.Add(this.column3);
            this.lvItemsList.ContentBackColor = System.Drawing.Color.White;
            this.lvItemsList.DefaultRowHeight = ((short)(22));
            this.lvItemsList.DimSelectionWhenDisabled = true;
            this.lvItemsList.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItemsList.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItemsList.HeaderHeight = ((short)(25));
            this.lvItemsList.HorizontalScrollbar = true;
            this.lvItemsList.Name = "lvItemsList";
            this.lvItemsList.OddRowColor = System.Drawing.Color.White;
            this.lvItemsList.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItemsList.SecondarySortColumn = ((short)(-1));
            this.lvItemsList.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItemsList.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItemsList.SortSetting = "0:1";
            this.lvItemsList.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItemsList_HeaderClicked);
            this.lvItemsList.SelectionChanged += new System.EventHandler(this.lvItemsList_SelectionChanged);
            this.lvItemsList.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItemsList_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
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
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // btnViewItem
            // 
            resources.ApplyResources(this.btnViewItem, "btnViewItem");
            this.btnViewItem.BackColor = System.Drawing.Color.Transparent;
            this.btnViewItem.Name = "btnViewItem";
            this.btnViewItem.UseVisualStyleBackColor = true;
            this.btnViewItem.Click += new System.EventHandler(this.btnViewItem_Click);
            // 
            // SpecialGroupItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnViewItem);
            this.Controls.Add(this.lvItemsList);
            this.Controls.Add(this.btnsContextButtonsItems);
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.lblNoSelection);
            this.Name = "SpecialGroupItemsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButtons btnsContextButtonsItems;
        private DatabasePageDisplay itemDataScroll;
        private System.Windows.Forms.Label lblNoSelection;
        private ListView lvItemsList;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private System.Windows.Forms.Button btnViewItem;
    }
}
