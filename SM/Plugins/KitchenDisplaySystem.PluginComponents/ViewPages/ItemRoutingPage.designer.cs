using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class ItemRoutingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemRoutingPage));
            this.btnsItemConnection = new LSOne.Controls.ContextButtons();
            this.chkAllItems = new System.Windows.Forms.CheckBox();
            this.lvItemConnections = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // btnsItemConnection
            // 
            this.btnsItemConnection.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsItemConnection, "btnsItemConnection");
            this.btnsItemConnection.BackColor = System.Drawing.Color.Transparent;
            this.btnsItemConnection.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsItemConnection.EditButtonEnabled = false;
            this.btnsItemConnection.Name = "btnsItemConnection";
            this.btnsItemConnection.RemoveButtonEnabled = true;
            this.btnsItemConnection.EditButtonClicked += new System.EventHandler(this.btnsItemConnection_EditButtonClicked);
            this.btnsItemConnection.AddButtonClicked += new System.EventHandler(this.btnsItemConnection_AddButtonClicked);
            this.btnsItemConnection.RemoveButtonClicked += new System.EventHandler(this.btnsItemConnection_RemoveButtonClicked);
            // 
            // chkAllItems
            // 
            resources.ApplyResources(this.chkAllItems, "chkAllItems");
            this.chkAllItems.Name = "chkAllItems";
            this.chkAllItems.UseVisualStyleBackColor = true;
            // 
            // lvItemConnections
            // 
            resources.ApplyResources(this.lvItemConnections, "lvItemConnections");
            this.lvItemConnections.BorderColor = System.Drawing.Color.DarkGray;
            this.lvItemConnections.BuddyControl = null;
            this.lvItemConnections.Columns.Add(this.column1);
            this.lvItemConnections.Columns.Add(this.column2);
            this.lvItemConnections.Columns.Add(this.column3);
            this.lvItemConnections.Columns.Add(this.column4);
            this.lvItemConnections.ContentBackColor = System.Drawing.Color.White;
            this.lvItemConnections.DefaultRowHeight = ((short)(18));
            this.lvItemConnections.DimSelectionWhenDisabled = true;
            this.lvItemConnections.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItemConnections.HeaderBackColor = System.Drawing.Color.White;
            this.lvItemConnections.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItemConnections.HeaderHeight = ((short)(25));
            this.lvItemConnections.HorizontalScrollbar = true;
            this.lvItemConnections.Name = "lvItemConnections";
            this.lvItemConnections.OddRowColor = System.Drawing.Color.White;
            this.lvItemConnections.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItemConnections.SecondarySortColumn = ((short)(-1));
            this.lvItemConnections.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvItemConnections.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItemConnections.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItemConnections.SortSetting = "0:1";
            this.lvItemConnections.VerticalScrollbarValue = 0;
            this.lvItemConnections.VerticalScrollbarYOffset = 0;
            this.lvItemConnections.SelectionChanged += new System.EventHandler(this.lvItemConnections_SelectionChanged);
            this.lvItemConnections.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItemConnections_RowDoubleClick);
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
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // ItemRoutingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvItemConnections);
            this.Controls.Add(this.chkAllItems);
            this.Controls.Add(this.btnsItemConnection);
            this.DoubleBuffered = true;
            this.Name = "ItemRoutingPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ContextButtons btnsItemConnection;
        private System.Windows.Forms.CheckBox chkAllItems;
        private ListView lvItemConnections;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
        private LSOne.Controls.Columns.Column column3;
        private LSOne.Controls.Columns.Column column4;
    }
}
