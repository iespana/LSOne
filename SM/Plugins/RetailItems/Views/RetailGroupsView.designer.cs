using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    partial class RetailGroupsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailGroupsView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.groupPanelNoSelection = new LSOne.Controls.GroupPanel();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvGroups = new LSOne.Controls.ListView();
            this.colGroup = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colDepartment = new LSOne.Controls.Columns.Column();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.pnlBottom.SuspendLayout();
            this.groupPanelNoSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvGroups);
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.groupPanelNoSelection);
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // groupPanelNoSelection
            // 
            resources.ApplyResources(this.groupPanelNoSelection, "groupPanelNoSelection");
            this.groupPanelNoSelection.Controls.Add(this.lblNoSelection);
            this.groupPanelNoSelection.Name = "groupPanelNoSelection";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // lvGroups
            // 
            resources.ApplyResources(this.lvGroups, "lvGroups");
            this.lvGroups.BuddyControl = null;
            this.lvGroups.Columns.Add(this.colGroup);
            this.lvGroups.Columns.Add(this.colDescription);
            this.lvGroups.Columns.Add(this.colDepartment);
            this.lvGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvGroups.DefaultRowHeight = ((short)(22));
            this.lvGroups.DimSelectionWhenDisabled = true;
            this.lvGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvGroups.HeaderHeight = ((short)(25));
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.OddRowColor = System.Drawing.Color.White;
            this.lvGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvGroups.SecondarySortColumn = ((short)(-1));
            this.lvGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvGroups.SortSetting = "0:1";
            this.lvGroups.SelectionChanged += new System.EventHandler(this.lvGroups_SelectionChanged);
            this.lvGroups.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvGroups_RowDoubleClick);
            // 
            // colGroup
            // 
            this.colGroup.AutoSize = true;
            this.colGroup.DefaultStyle = null;
            resources.ApplyResources(this.colGroup, "colGroup");
            this.colGroup.InternalSort = true;
            this.colGroup.MaximumWidth = ((short)(0));
            this.colGroup.MinimumWidth = ((short)(10));
            this.colGroup.SecondarySortColumn = ((short)(-1));
            this.colGroup.Sizable = false;
            this.colGroup.Tag = null;
            this.colGroup.Width = ((short)(150));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Sizable = false;
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(200));
            // 
            // colDepartment
            // 
            this.colDepartment.AutoSize = true;
            this.colDepartment.DefaultStyle = null;
            resources.ApplyResources(this.colDepartment, "colDepartment");
            this.colDepartment.InternalSort = true;
            this.colDepartment.MaximumWidth = ((short)(0));
            this.colDepartment.MinimumWidth = ((short)(10));
            this.colDepartment.SecondarySortColumn = ((short)(-1));
            this.colDepartment.Sizable = false;
            this.colDepartment.Tag = null;
            this.colDepartment.Width = ((short)(200));
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar1_UnknownControlRemove);
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar1_UnknownControlSetSelection);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // RetailGroupsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RetailGroupsView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanelNoSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private TabControl tabSheetTabs;
        private GroupPanel groupPanelNoSelection;
        private System.Windows.Forms.Label lblNoSelection;
        private ListView lvGroups;
        private Controls.Columns.Column colGroup;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colDepartment;
        private SearchBar searchBar1;
        private DatabasePageDisplay itemDataScroll;

    }
}
