using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class AggregateProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AggregateProfilesView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lvAggregateGroups = new LSOne.Controls.ListView();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.btnsContextButtonsGroups = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvAggregateProfiles = new LSOne.Controls.ListView();
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
            this.pnlBottom.Controls.Add(this.lvAggregateProfiles);
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEditAggregateProfile_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAddAggregateProfile_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemoveAggregateProfile_Click);
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
            this.groupPanel1.Controls.Add(this.lvAggregateGroups);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsGroups);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvAggregateGroups
            // 
            resources.ApplyResources(this.lvAggregateGroups, "lvAggregateGroups");
            this.lvAggregateGroups.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAggregateGroups.BuddyControl = null;
            this.lvAggregateGroups.Columns.Add(this.column5);
            this.lvAggregateGroups.Columns.Add(this.column6);
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
            // btnsContextButtonsGroups
            // 
            this.btnsContextButtonsGroups.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsGroups, "btnsContextButtonsGroups");
            this.btnsContextButtonsGroups.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsGroups.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtonsGroups.EditButtonEnabled = true;
            this.btnsContextButtonsGroups.Name = "btnsContextButtonsGroups";
            this.btnsContextButtonsGroups.RemoveButtonEnabled = true;
            this.btnsContextButtonsGroups.EditButtonClicked += new System.EventHandler(this.btnEditAggregateGroup_Click);
            this.btnsContextButtonsGroups.AddButtonClicked += new System.EventHandler(this.btnAddAggregateGroup_Click);
            this.btnsContextButtonsGroups.RemoveButtonClicked += new System.EventHandler(this.btnRemoveAggregateGroup_Click);
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
            // lvAggregateProfiles
            // 
            resources.ApplyResources(this.lvAggregateProfiles, "lvAggregateProfiles");
            this.lvAggregateProfiles.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAggregateProfiles.BuddyControl = null;
            this.lvAggregateProfiles.Columns.Add(this.column3);
            this.lvAggregateProfiles.Columns.Add(this.column4);
            this.lvAggregateProfiles.Columns.Add(this.column1);
            this.lvAggregateProfiles.ContentBackColor = System.Drawing.Color.White;
            this.lvAggregateProfiles.DefaultRowHeight = ((short)(22));
            this.lvAggregateProfiles.DimSelectionWhenDisabled = true;
            this.lvAggregateProfiles.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAggregateProfiles.HeaderBackColor = System.Drawing.Color.White;
            this.lvAggregateProfiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAggregateProfiles.HeaderHeight = ((short)(25));
            this.lvAggregateProfiles.Name = "lvAggregateProfiles";
            this.lvAggregateProfiles.OddRowColor = System.Drawing.Color.White;
            this.lvAggregateProfiles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAggregateProfiles.SecondarySortColumn = ((short)(-1));
            this.lvAggregateProfiles.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvAggregateProfiles.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAggregateProfiles.SortSetting = "0:1";
            this.lvAggregateProfiles.VerticalScrollbar = false;
            this.lvAggregateProfiles.VerticalScrollbarValue = 0;
            this.lvAggregateProfiles.VerticalScrollbarYOffset = 0;
            this.lvAggregateProfiles.SelectionChanged += new System.EventHandler(this.lvAggregateProfiles_SelectedIndexChanged);
            this.lvAggregateProfiles.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAggregateProfiles_DoubleClick);
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
            // AggregateProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AggregateProfilesView";
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
        private ListView lvAggregateProfiles;
        private ListView lvAggregateGroups;
        private LSOne.Controls.Columns.Column column3;
        private LSOne.Controls.Columns.Column column4;
        private LSOne.Controls.Columns.Column column5;
        private LSOne.Controls.Columns.Column column6;
        private LSOne.Controls.Columns.Column column1;
    }
}
