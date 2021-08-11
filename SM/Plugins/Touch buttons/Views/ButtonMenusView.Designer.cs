using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    partial class ButtonMenusView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonMenusView));
            this.btnsEditAddRemovePosMenu = new LSOne.Controls.ContextButtons();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.btnPreview = new System.Windows.Forms.Button();
            this.lvPosMenuHeaders = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colMenuDescription = new LSOne.Controls.Columns.Column();
            this.colColumns = new LSOne.Controls.Columns.Column();
            this.colRows = new LSOne.Controls.Columns.Column();
            this.colStyle = new LSOne.Controls.Columns.Column();
            this.colImportTime = new LSOne.Controls.Columns.Column();
            this.btnCreateStylesFromHeader = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.colMainMenu = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.tableLayoutPanel1);
            this.pnlBottom.Controls.Add(this.lvPosMenuHeaders);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemovePosMenu);
            // 
            // btnsEditAddRemovePosMenu
            // 
            this.btnsEditAddRemovePosMenu.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemovePosMenu, "btnsEditAddRemovePosMenu");
            this.btnsEditAddRemovePosMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemovePosMenu.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemovePosMenu.EditButtonEnabled = false;
            this.btnsEditAddRemovePosMenu.Name = "btnsEditAddRemovePosMenu";
            this.btnsEditAddRemovePosMenu.RemoveButtonEnabled = false;
            this.btnsEditAddRemovePosMenu.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenu_EditButtonClicked);
            this.btnsEditAddRemovePosMenu.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenu_AddButtonClicked);
            this.btnsEditAddRemovePosMenu.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenu_RemoveButtonClicked);
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
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
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
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // btnPreview
            // 
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // lvPosMenuHeaders
            // 
            resources.ApplyResources(this.lvPosMenuHeaders, "lvPosMenuHeaders");
            this.lvPosMenuHeaders.BuddyControl = null;
            this.lvPosMenuHeaders.Columns.Add(this.colID);
            this.lvPosMenuHeaders.Columns.Add(this.colMenuDescription);
            this.lvPosMenuHeaders.Columns.Add(this.colColumns);
            this.lvPosMenuHeaders.Columns.Add(this.colRows);
            this.lvPosMenuHeaders.Columns.Add(this.colStyle);
            this.lvPosMenuHeaders.Columns.Add(this.colImportTime);
            this.lvPosMenuHeaders.Columns.Add(this.colMainMenu);
            this.lvPosMenuHeaders.ContentBackColor = System.Drawing.Color.White;
            this.lvPosMenuHeaders.DefaultRowHeight = ((short)(22));
            this.lvPosMenuHeaders.DimSelectionWhenDisabled = true;
            this.lvPosMenuHeaders.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPosMenuHeaders.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPosMenuHeaders.HeaderHeight = ((short)(25));
            this.lvPosMenuHeaders.Name = "lvPosMenuHeaders";
            this.lvPosMenuHeaders.OddRowColor = System.Drawing.Color.White;
            this.lvPosMenuHeaders.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPosMenuHeaders.SecondarySortColumn = ((short)(-1));
            this.lvPosMenuHeaders.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvPosMenuHeaders.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPosMenuHeaders.SortSetting = "0:1";
            this.lvPosMenuHeaders.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvPosMenuHeaders_HeaderClicked);
            this.lvPosMenuHeaders.SelectionChanged += new System.EventHandler(this.lvPosMenuHeaders_SelectedIndexChanged);
            this.lvPosMenuHeaders.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvPosMenuHeaders_RowDoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(100));
            // 
            // colMenuDescription
            // 
            this.colMenuDescription.AutoSize = true;
            this.colMenuDescription.DefaultStyle = null;
            resources.ApplyResources(this.colMenuDescription, "colMenuDescription");
            this.colMenuDescription.MaximumWidth = ((short)(0));
            this.colMenuDescription.MinimumWidth = ((short)(150));
            this.colMenuDescription.SecondarySortColumn = ((short)(-1));
            this.colMenuDescription.Tag = null;
            this.colMenuDescription.Width = ((short)(150));
            // 
            // colColumns
            // 
            this.colColumns.AutoSize = true;
            this.colColumns.Clickable = false;
            this.colColumns.DefaultStyle = null;
            resources.ApplyResources(this.colColumns, "colColumns");
            this.colColumns.MaximumWidth = ((short)(0));
            this.colColumns.MinimumWidth = ((short)(10));
            this.colColumns.SecondarySortColumn = ((short)(-1));
            this.colColumns.Tag = null;
            this.colColumns.Width = ((short)(50));
            // 
            // colRows
            // 
            this.colRows.AutoSize = true;
            this.colRows.Clickable = false;
            this.colRows.DefaultStyle = null;
            resources.ApplyResources(this.colRows, "colRows");
            this.colRows.MaximumWidth = ((short)(0));
            this.colRows.MinimumWidth = ((short)(10));
            this.colRows.SecondarySortColumn = ((short)(-1));
            this.colRows.Tag = null;
            this.colRows.Width = ((short)(50));
            // 
            // colStyle
            // 
            this.colStyle.AutoSize = true;
            this.colStyle.DefaultStyle = null;
            resources.ApplyResources(this.colStyle, "colStyle");
            this.colStyle.MaximumWidth = ((short)(0));
            this.colStyle.MinimumWidth = ((short)(10));
            this.colStyle.SecondarySortColumn = ((short)(-1));
            this.colStyle.Tag = null;
            this.colStyle.Width = ((short)(120));
            // 
            // colImportTime
            // 
            this.colImportTime.AutoSize = true;
            this.colImportTime.DefaultStyle = null;
            resources.ApplyResources(this.colImportTime, "colImportTime");
            this.colImportTime.MaximumWidth = ((short)(0));
            this.colImportTime.MinimumWidth = ((short)(10));
            this.colImportTime.SecondarySortColumn = ((short)(-1));
            this.colImportTime.Tag = null;
            this.colImportTime.Width = ((short)(120));
            // 
            // btnCreateStylesFromHeader
            // 
            resources.ApplyResources(this.btnCreateStylesFromHeader, "btnCreateStylesFromHeader");
            this.btnCreateStylesFromHeader.Name = "btnCreateStylesFromHeader";
            this.btnCreateStylesFromHeader.UseVisualStyleBackColor = true;
            this.btnCreateStylesFromHeader.Click += new System.EventHandler(this.btnCreateStylesFromHeader_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnPreview, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCreateStylesFromHeader, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar_UnknownControlAdd);
            this.searchBar.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar_UnknownControlRemove);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // colMainMenu
            // 
            this.colMainMenu.AutoSize = true;
            this.colMainMenu.Clickable = false;
            this.colMainMenu.DefaultStyle = null;
            resources.ApplyResources(this.colMainMenu, "colMainMenu");
            this.colMainMenu.MaximumWidth = ((short)(0));
            this.colMainMenu.MinimumWidth = ((short)(10));
            this.colMainMenu.SecondarySortColumn = ((short)(-1));
            this.colMainMenu.Tag = null;
            this.colMainMenu.Width = ((short)(61));
            // 
            // ButtonMenusView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ButtonMenusView";
            this.pnlBottom.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddRemovePosMenu;
        private System.Windows.Forms.Button btnPreview;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private ListView lvPosMenuHeaders;
        private Column colID;
        private Column colMenuDescription;
        private Column colColumns;
        private Column colRows;
        private Column colStyle;
        private Column colImportTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCreateStylesFromHeader;
        private SearchBar searchBar;
        private Column colMainMenu;
    }
}