using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Hospitality.Views
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
            this.btnPreview = new System.Windows.Forms.Button();
            this.lvPosMenuHeaders = new LSOne.Controls.ListView();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.lvPosMenuHeaders);
            this.pnlBottom.Controls.Add(this.btnPreview);
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
            this.lvPosMenuHeaders.Columns.Add(this.column5);
            this.lvPosMenuHeaders.Columns.Add(this.column6);
            this.lvPosMenuHeaders.Columns.Add(this.column7);
            this.lvPosMenuHeaders.Columns.Add(this.column8);
            this.lvPosMenuHeaders.Columns.Add(this.column1);
            this.lvPosMenuHeaders.Columns.Add(this.column2);
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
            this.lvPosMenuHeaders.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPosMenuHeaders.SortSetting = "0:1";
            this.lvPosMenuHeaders.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvPosMenuHeaders_HeaderClicked);
            this.lvPosMenuHeaders.SelectionChanged += new System.EventHandler(this.lvPosMenuHeaders_SelectedIndexChanged);
            this.lvPosMenuHeaders.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvPosMenuHeaders_RowDoubleClick);
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(100));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(150));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.Clickable = false;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.Clickable = false;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
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
            this.column1.Width = ((short)(150));
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
            this.column2.Width = ((short)(61));
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
            // ButtonMenus
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ButtonMenus";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddRemovePosMenu;
        private System.Windows.Forms.Button btnPreview;
        private ListView lvPosMenuHeaders;
        private Column column5;
        private Column column6;
        private Column column7;
        private Column column8;
        private SearchBar searchBar;
        private Column column1;
        private Column column2;
    }
}