using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class ItemVendorPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemVendorPage));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnsAddRemove = new LSOne.Controls.ContextButtons();
            this.btnSetAsDefault = new System.Windows.Forms.Button();
            this.lvItems = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.lblLocalDatabase = new System.Windows.Forms.Label();
            this.btnViewVendor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnsAddRemove
            // 
            this.btnsAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddRemove, "btnsAddRemove");
            this.btnsAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsAddRemove.EditButtonEnabled = true;
            this.btnsAddRemove.Name = "btnsAddRemove";
            this.btnsAddRemove.RemoveButtonEnabled = true;
            this.btnsAddRemove.EditButtonClicked += new System.EventHandler(this.btnsAddRemove_EditButtonClicked);
            this.btnsAddRemove.AddButtonClicked += new System.EventHandler(this.AddButtonClicked);
            this.btnsAddRemove.RemoveButtonClicked += new System.EventHandler(this.RemoveButtonClicked);
            // 
            // btnSetAsDefault
            // 
            resources.ApplyResources(this.btnSetAsDefault, "btnSetAsDefault");
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.UseVisualStyleBackColor = true;
            this.btnSetAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.column1);
            this.lvItems.Columns.Add(this.column2);
            this.lvItems.Columns.Add(this.column3);
            this.lvItems.Columns.Add(this.column4);
            this.lvItems.Columns.Add(this.column5);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.CellAction += new LSOne.Controls.CellActionDelegate(this.lvItems_CellAction);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
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
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(100));
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
            this.column2.Sizable = false;
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
            this.column4.Sizable = false;
            this.column4.Tag = null;
            this.column4.Width = ((short)(100));
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
            this.column5.Sizable = false;
            this.column5.Tag = null;
            this.column5.Width = ((short)(100));
            // 
            // lblLocalDatabase
            // 
            resources.ApplyResources(this.lblLocalDatabase, "lblLocalDatabase");
            this.lblLocalDatabase.ForeColor = System.Drawing.Color.Red;
            this.lblLocalDatabase.Name = "lblLocalDatabase";
            // 
            // btnViewVendor
            // 
            resources.ApplyResources(this.btnViewVendor, "btnViewVendor");
            this.btnViewVendor.Name = "btnViewVendor";
            this.btnViewVendor.UseVisualStyleBackColor = true;
            this.btnViewVendor.Click += new System.EventHandler(this.ViewVendor);
            // 
            // ItemVendorPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnViewVendor);
            this.Controls.Add(this.lblLocalDatabase);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnSetAsDefault);
            this.Controls.Add(this.btnsAddRemove);
            this.DoubleBuffered = true;
            this.Name = "ItemVendorPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButtons btnsAddRemove;
        private System.Windows.Forms.Button btnSetAsDefault;
        private ListView lvItems;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private System.Windows.Forms.Label lblLocalDatabase;
        private Controls.Columns.Column column3;
        private System.Windows.Forms.Button btnViewVendor;
    }
}
