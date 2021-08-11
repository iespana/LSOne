using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    partial class InventoryTemplatesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplatesView));
            this.lvTemplates = new LSOne.Controls.ListView();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmType = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvTemplates);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lvTemplates
            // 
            resources.ApplyResources(this.lvTemplates, "lvTemplates");
            this.lvTemplates.BuddyControl = null;
            this.lvTemplates.Columns.Add(this.clmDescription);
            this.lvTemplates.Columns.Add(this.clmStore);
            this.lvTemplates.Columns.Add(this.clmType);
            this.lvTemplates.ContentBackColor = System.Drawing.Color.White;
            this.lvTemplates.DefaultRowHeight = ((short)(22));
            this.lvTemplates.DimSelectionWhenDisabled = true;
            this.lvTemplates.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTemplates.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvTemplates.HeaderHeight = ((short)(25));
            this.lvTemplates.Name = "lvTemplates";
            this.lvTemplates.OddRowColor = System.Drawing.Color.White;
            this.lvTemplates.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTemplates.SecondarySortColumn = ((short)(-1));
            this.lvTemplates.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvTemplates.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTemplates.SortSetting = "0:1";
            this.lvTemplates.SelectionChanged += new System.EventHandler(this.lvTemplates_SelectionChanged);
            this.lvTemplates.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvTemplates_RowDoubleClick);
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.InternalSort = true;
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(100));
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.DefaultStyle = null;
            resources.ApplyResources(this.clmStore, "clmStore");
            this.clmStore.InternalSort = true;
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(10));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(100));
            // 
            // clmType
            // 
            this.clmType.AutoSize = true;
            this.clmType.DefaultStyle = null;
            resources.ApplyResources(this.clmType, "clmType");
            this.clmType.InternalSort = true;
            this.clmType.MaximumWidth = ((short)(0));
            this.clmType.MinimumWidth = ((short)(10));
            this.clmType.SecondarySortColumn = ((short)(-1));
            this.clmType.Tag = null;
            this.clmType.Width = ((short)(100));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
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
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // InventoryTemplatesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "InventoryTemplatesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvTemplates;
        private ContextButtons btnsEditAddRemove;
        private Column clmDescription;
        private Column clmStore;
        private Column clmType;
        private SearchBar searchBar;
    }
}
