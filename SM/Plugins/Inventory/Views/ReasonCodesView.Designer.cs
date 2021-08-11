using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class ReasonCodesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReasonCodesView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvReasons = new LSOne.Controls.ListView();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colAction = new LSOne.Controls.Columns.Column();
            this.colBeginDate = new LSOne.Controls.Columns.Column();
            this.colEndDate = new LSOne.Controls.Columns.Column();
            this.colSystemReason = new LSOne.Controls.Columns.Column();
            this.colPos = new LSOne.Controls.Columns.Column();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvReasons);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
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
            // lvReasons
            // 
            resources.ApplyResources(this.lvReasons, "lvReasons");
            this.lvReasons.BuddyControl = null;
            this.lvReasons.Columns.Add(this.colDescription);
            this.lvReasons.Columns.Add(this.colAction);
            this.lvReasons.Columns.Add(this.colBeginDate);
            this.lvReasons.Columns.Add(this.colEndDate);
            this.lvReasons.Columns.Add(this.colSystemReason);
            this.lvReasons.Columns.Add(this.colPos);
            this.lvReasons.ContentBackColor = System.Drawing.Color.White;
            this.lvReasons.DefaultRowHeight = ((short)(22));
            this.lvReasons.DimSelectionWhenDisabled = true;
            this.lvReasons.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvReasons.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvReasons.HeaderHeight = ((short)(25));
            this.lvReasons.HorizontalScrollbar = true;
            this.lvReasons.Name = "lvReasons";
            this.lvReasons.OddRowColor = System.Drawing.Color.White;
            this.lvReasons.RowLineColor = System.Drawing.Color.LightGray;
            this.lvReasons.SecondarySortColumn = ((short)(-1));
            this.lvReasons.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvReasons.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvReasons.SortSetting = "0:1";
            this.lvReasons.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvReasons_HeaderClicked);
            this.lvReasons.SelectionChanged += new System.EventHandler(this.lvReasons_SelectionChanged);
            this.lvReasons.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvReasons_RowDoubleClick);
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
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colAction
            // 
            this.colAction.AutoSize = true;
            this.colAction.DefaultStyle = null;
            resources.ApplyResources(this.colAction, "colAction");
            this.colAction.MaximumWidth = ((short)(0));
            this.colAction.MinimumWidth = ((short)(10));
            this.colAction.SecondarySortColumn = ((short)(-1));
            this.colAction.Tag = null;
            this.colAction.Width = ((short)(50));
            // 
            // colBeginDate
            // 
            this.colBeginDate.AutoSize = true;
            this.colBeginDate.DefaultStyle = null;
            resources.ApplyResources(this.colBeginDate, "colBeginDate");
            this.colBeginDate.MaximumWidth = ((short)(0));
            this.colBeginDate.MinimumWidth = ((short)(10));
            this.colBeginDate.SecondarySortColumn = ((short)(-1));
            this.colBeginDate.Tag = null;
            this.colBeginDate.Width = ((short)(50));
            // 
            // colEndDate
            // 
            this.colEndDate.AutoSize = true;
            this.colEndDate.DefaultStyle = null;
            resources.ApplyResources(this.colEndDate, "colEndDate");
            this.colEndDate.MaximumWidth = ((short)(0));
            this.colEndDate.MinimumWidth = ((short)(10));
            this.colEndDate.SecondarySortColumn = ((short)(-1));
            this.colEndDate.Tag = null;
            this.colEndDate.Width = ((short)(50));
            // 
            // colSystemReason
            // 
            this.colSystemReason.AutoSize = true;
            this.colSystemReason.DefaultStyle = null;
            resources.ApplyResources(this.colSystemReason, "colSystemReason");
            this.colSystemReason.MaximumWidth = ((short)(0));
            this.colSystemReason.MinimumWidth = ((short)(10));
            this.colSystemReason.SecondarySortColumn = ((short)(-1));
            this.colSystemReason.Tag = null;
            this.colSystemReason.Width = ((short)(50));
            // 
            // colPos
            // 
            this.colPos.AutoSize = true;
            this.colPos.DefaultStyle = null;
            resources.ApplyResources(this.colPos, "colPos");
            this.colPos.MaximumWidth = ((short)(0));
            this.colPos.MinimumWidth = ((short)(10));
            this.colPos.SecondarySortColumn = ((short)(-1));
            this.colPos.Tag = null;
            this.colPos.Width = ((short)(50));
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
            this.searchBar1.SearchOptionChanged += new System.EventHandler(this.searchBar1_SearchOptionChanged);
            // 
            // ReasonCodesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ReasonCodesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private ListView lvReasons;
        private SearchBar searchBar1;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colAction;
        private Controls.Columns.Column colBeginDate;
        private Controls.Columns.Column colEndDate;
        private Controls.Columns.Column colSystemReason;
        private Controls.Columns.Column colPos;
    }
}
