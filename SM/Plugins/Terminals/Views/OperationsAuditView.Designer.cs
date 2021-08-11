using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.Views
{
    partial class OperationsAuditView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperationsAuditView));
            this.lvOperations = new LSOne.Controls.ListView();
            this.colTime = new LSOne.Controls.Columns.Column();
            this.colOperation = new LSOne.Controls.Columns.Column();
            this.colOperator = new LSOne.Controls.Columns.Column();
            this.colManagerOverrideID = new LSOne.Controls.Columns.Column();
            this.colStore = new LSOne.Controls.Columns.Column();
            this.colTerminal = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.operationsDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.btnGenerateExcelFile = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnGenerateExcelFile);
            this.pnlBottom.Controls.Add(this.operationsDataScroll);
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.lvOperations);
            // 
            // lvOperations
            // 
            resources.ApplyResources(this.lvOperations, "lvOperations");
            this.lvOperations.BuddyControl = null;
            this.lvOperations.Columns.Add(this.colTime);
            this.lvOperations.Columns.Add(this.colOperation);
            this.lvOperations.Columns.Add(this.colOperator);
            this.lvOperations.Columns.Add(this.colManagerOverrideID);
            this.lvOperations.Columns.Add(this.colStore);
            this.lvOperations.Columns.Add(this.colTerminal);
            this.lvOperations.ContentBackColor = System.Drawing.Color.White;
            this.lvOperations.DefaultRowHeight = ((short)(22));
            this.lvOperations.EvenRowColor = System.Drawing.Color.White;
            this.lvOperations.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvOperations.HeaderHeight = ((short)(25));
            this.lvOperations.Name = "lvOperations";
            this.lvOperations.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvOperations.RowLineColor = System.Drawing.Color.LightGray;
            this.lvOperations.SecondarySortColumn = ((short)(-1));
            this.lvOperations.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvOperations.SortSetting = "0:1";
            // 
            // colTime
            // 
            this.colTime.AutoSize = true;
            this.colTime.Clickable = false;
            this.colTime.DefaultStyle = null;
            resources.ApplyResources(this.colTime, "colTime");
            this.colTime.MaximumWidth = ((short)(0));
            this.colTime.MinimumWidth = ((short)(10));
            this.colTime.Tag = null;
            this.colTime.Width = ((short)(100));
            // 
            // colOperation
            // 
            this.colOperation.AutoSize = true;
            this.colOperation.Clickable = false;
            this.colOperation.DefaultStyle = null;
            resources.ApplyResources(this.colOperation, "colOperation");
            this.colOperation.MaximumWidth = ((short)(0));
            this.colOperation.MinimumWidth = ((short)(10));
            this.colOperation.Tag = null;
            this.colOperation.Width = ((short)(100));
            // 
            // colOperator
            // 
            this.colOperator.AutoSize = true;
            this.colOperator.Clickable = false;
            this.colOperator.DefaultStyle = null;
            resources.ApplyResources(this.colOperator, "colOperator");
            this.colOperator.MaximumWidth = ((short)(0));
            this.colOperator.MinimumWidth = ((short)(10));
            this.colOperator.Tag = null;
            this.colOperator.Width = ((short)(100));
            // 
            // colManagerOverrideID
            // 
            this.colManagerOverrideID.AutoSize = true;
            this.colManagerOverrideID.Clickable = false;
            this.colManagerOverrideID.DefaultStyle = null;
            resources.ApplyResources(this.colManagerOverrideID, "colManagerOverrideID");
            this.colManagerOverrideID.MaximumWidth = ((short)(0));
            this.colManagerOverrideID.MinimumWidth = ((short)(10));
            this.colManagerOverrideID.Tag = null;
            this.colManagerOverrideID.Width = ((short)(100));
            // 
            // colStore
            // 
            this.colStore.AutoSize = true;
            this.colStore.Clickable = false;
            this.colStore.DefaultStyle = null;
            resources.ApplyResources(this.colStore, "colStore");
            this.colStore.MaximumWidth = ((short)(0));
            this.colStore.MinimumWidth = ((short)(10));
            this.colStore.Tag = null;
            this.colStore.Width = ((short)(100));
            // 
            // colTerminal
            // 
            this.colTerminal.AutoSize = true;
            this.colTerminal.Clickable = false;
            this.colTerminal.DefaultStyle = null;
            resources.ApplyResources(this.colTerminal, "colTerminal");
            this.colTerminal.MaximumWidth = ((short)(0));
            this.colTerminal.MinimumWidth = ((short)(10));
            this.colTerminal.Tag = null;
            this.colTerminal.Width = ((short)(100));
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Name = "searchBar";
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar_UnknownControlAdd);
            this.searchBar.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar_UnknownControlRemove);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            // 
            // operationsDataScroll
            // 
            resources.ApplyResources(this.operationsDataScroll, "operationsDataScroll");
            this.operationsDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.operationsDataScroll.Name = "operationsDataScroll";
            this.operationsDataScroll.PageSize = 0;
            this.operationsDataScroll.PageChanged += new System.EventHandler(this.operationsDataScroll_PageChanged);
            // 
            // btnGenerateExcelFile
            // 
            resources.ApplyResources(this.btnGenerateExcelFile, "btnGenerateExcelFile");
            this.btnGenerateExcelFile.Name = "btnGenerateExcelFile";
            this.btnGenerateExcelFile.UseVisualStyleBackColor = true;
            this.btnGenerateExcelFile.Click += new System.EventHandler(this.btnGenerateExcelFile_Click);
            // 
            // OperationsAuditView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "OperationsAuditView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SearchBar searchBar;
        private ListView lvOperations;
        private DatabasePageDisplay operationsDataScroll;
        private Controls.Columns.Column colTime;
        private Controls.Columns.Column colOperation;
        private Controls.Columns.Column colOperator;
        private Controls.Columns.Column colManagerOverrideID;
        private Controls.Columns.Column colStore;
        private Controls.Columns.Column colTerminal;
        private System.Windows.Forms.Button btnGenerateExcelFile;

    }
}
