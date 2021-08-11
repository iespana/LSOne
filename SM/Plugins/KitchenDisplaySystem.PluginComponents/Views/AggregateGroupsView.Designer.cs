using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class AggregateGroupsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AggregateGroupsView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lvAggregateItems = new LSOne.Controls.ListView();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.btnsContextButtonsGroups = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvAggregateGroups = new LSOne.Controls.ListView();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvAggregateGroups);
            this.pnlBottom.Controls.Add(this.groupBox1);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEditAggregateGroup_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAddAggregateGroup_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemoveAggregateGroup_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.groupPanel1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lvAggregateItems);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsGroups);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvAggregateItems
            // 
            resources.ApplyResources(this.lvAggregateItems, "lvAggregateItems");
            this.lvAggregateItems.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAggregateItems.BuddyControl = null;
            this.lvAggregateItems.Columns.Add(this.column5);
            this.lvAggregateItems.Columns.Add(this.column6);
            this.lvAggregateItems.Columns.Add(this.column2);
            this.lvAggregateItems.ContentBackColor = System.Drawing.Color.White;
            this.lvAggregateItems.DefaultRowHeight = ((short)(22));
            this.lvAggregateItems.DimSelectionWhenDisabled = true;
            this.lvAggregateItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAggregateItems.HeaderBackColor = System.Drawing.Color.White;
            this.lvAggregateItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAggregateItems.HeaderHeight = ((short)(25));
            this.lvAggregateItems.Name = "lvAggregateItems";
            this.lvAggregateItems.OddRowColor = System.Drawing.Color.White;
            this.lvAggregateItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAggregateItems.SecondarySortColumn = ((short)(-1));
            this.lvAggregateItems.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvAggregateItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvAggregateItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAggregateItems.SortSetting = "0:1";
            this.lvAggregateItems.VerticalScrollbar = false;
            this.lvAggregateItems.VerticalScrollbarValue = 0;
            this.lvAggregateItems.VerticalScrollbarYOffset = 0;
            this.lvAggregateItems.SelectionChanged += new System.EventHandler(this.lvAggregateItems_SelectedIndexChanged);
            this.lvAggregateItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAggregateItems_DoubleClick);
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
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
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
            // btnsContextButtonsGroups
            // 
            this.btnsContextButtonsGroups.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsGroups, "btnsContextButtonsGroups");
            this.btnsContextButtonsGroups.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsGroups.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtonsGroups.EditButtonEnabled = true;
            this.btnsContextButtonsGroups.Name = "btnsContextButtonsGroups";
            this.btnsContextButtonsGroups.RemoveButtonEnabled = true;
            this.btnsContextButtonsGroups.EditButtonClicked += new System.EventHandler(this.btnEditAggregateItem_Click);
            this.btnsContextButtonsGroups.AddButtonClicked += new System.EventHandler(this.btnAddAggregateItem_Click);
            this.btnsContextButtonsGroups.RemoveButtonClicked += new System.EventHandler(this.btnRemoveAggregateItem_Click);
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // lvAggregateGroups
            // 
            resources.ApplyResources(this.lvAggregateGroups, "lvAggregateGroups");
            this.lvAggregateGroups.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAggregateGroups.BuddyControl = null;
            this.lvAggregateGroups.Columns.Add(this.column3);
            this.lvAggregateGroups.Columns.Add(this.column4);
            this.lvAggregateGroups.Columns.Add(this.column1);
            this.lvAggregateGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvAggregateGroups.DefaultRowHeight = ((short)(22));
            this.lvAggregateGroups.DimSelectionWhenDisabled = true;
            this.lvAggregateGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAggregateGroups.HeaderBackColor = System.Drawing.Color.White;
            this.lvAggregateGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAggregateGroups.HeaderHeight = ((short)(25));
            this.lvAggregateGroups.Name = "lvAggregateGroups";
            this.lvAggregateGroups.OddRowColor = System.Drawing.Color.White;
            this.lvAggregateGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAggregateGroups.SecondarySortColumn = ((short)(-1));
            this.lvAggregateGroups.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvAggregateGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAggregateGroups.SortSetting = "0:1";
            this.lvAggregateGroups.VerticalScrollbar = false;
            this.lvAggregateGroups.VerticalScrollbarValue = 0;
            this.lvAggregateGroups.VerticalScrollbarYOffset = 0;
            this.lvAggregateGroups.SelectionChanged += new System.EventHandler(this.lvAggregateGroups_SelectedIndexChanged);
            this.lvAggregateGroups.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAggregateGroups_DoubleClick);
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
            // AggregateGroupsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AggregateGroupsView";
            this.pnlBottom.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.GroupBox groupBox1;
        private GroupPanel groupPanel1;
        private ContextButtons btnsContextButtonsGroups;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
        private ListView lvAggregateGroups;
        private ListView lvAggregateItems;
        private LSOne.Controls.Columns.Column column3;
        private LSOne.Controls.Columns.Column column4;
        private LSOne.Controls.Columns.Column column5;
        private LSOne.Controls.Columns.Column column6;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
    }
}
