using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemLinkedItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemLinkedItemsPage));
            this.label1 = new System.Windows.Forms.Label();
            this.btns = new LSOne.Controls.ContextButtons();
            this.lvLinkedItems = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btns
            // 
            this.btns.AddButtonEnabled = true;
            resources.ApplyResources(this.btns, "btns");
            this.btns.BackColor = System.Drawing.Color.Transparent;
            this.btns.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btns.EditButtonEnabled = false;
            this.btns.Name = "btns";
            this.btns.RemoveButtonEnabled = false;
            this.btns.EditButtonClicked += new System.EventHandler(this.btns_EditButtonClicked);
            this.btns.AddButtonClicked += new System.EventHandler(this.btns_AddButtonClicked);
            this.btns.RemoveButtonClicked += new System.EventHandler(this.btns_RemoveButtonClicked);
            // 
            // lvLinkedItems
            // 
            resources.ApplyResources(this.lvLinkedItems, "lvLinkedItems");
            this.lvLinkedItems.BuddyControl = null;
            this.lvLinkedItems.Columns.Add(this.column1);
            this.lvLinkedItems.Columns.Add(this.column2);
            this.lvLinkedItems.Columns.Add(this.column3);
            this.lvLinkedItems.Columns.Add(this.column4);
            this.lvLinkedItems.Columns.Add(this.column5);
            this.lvLinkedItems.ContentBackColor = System.Drawing.Color.White;
            this.lvLinkedItems.DefaultRowHeight = ((short)(22));
            this.lvLinkedItems.DimSelectionWhenDisabled = true;
            this.lvLinkedItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLinkedItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLinkedItems.HeaderHeight = ((short)(25));
            this.lvLinkedItems.Name = "lvLinkedItems";
            this.lvLinkedItems.OddRowColor = System.Drawing.Color.White;
            this.lvLinkedItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLinkedItems.SecondarySortColumn = ((short)(-1));
            this.lvLinkedItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLinkedItems.SortSetting = "0:1";
            this.lvLinkedItems.SelectionChanged += new System.EventHandler(this.lvLinkedItems_SelectionChanged);
            this.lvLinkedItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvLinkedItems_RowDoubleClick);
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
            this.column2.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.NoTextWhenSmall = true;
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
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // ItemLinkedItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvLinkedItems);
            this.Controls.Add(this.btns);
            this.Controls.Add(this.label1);
            this.Name = "ItemLinkedItemsPage";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private ContextButtons btns;
        private ListView lvLinkedItems;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
    }
}
