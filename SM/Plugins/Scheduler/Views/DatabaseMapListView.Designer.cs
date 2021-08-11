using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class DatabaseMapListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseMapListView));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.cmbToDesign = new LSOne.ViewPlugins.Scheduler.Controls.DatabaseDesignComboBox();
            this.cmbFromDesign = new LSOne.ViewPlugins.Scheduler.Controls.DatabaseDesignComboBox();
            this.contextButtonsMaps = new LSOne.Controls.ContextButtons();
            this.lblNoResults = new System.Windows.Forms.Label();
            this.lvDatabaseMap = new LSOne.Controls.ListView();
            this.colFromDatabase = new LSOne.Controls.Columns.Column();
            this.colToDatabase = new LSOne.Controls.Columns.Column();
            this.colFromTable = new LSOne.Controls.Columns.Column();
            this.colToTable = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvDatabaseMap);
            this.pnlBottom.Controls.Add(this.contextButtonsMaps);
            this.pnlBottom.Controls.Add(this.lblNoResults);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.cmbToDesign);
            this.groupPanel1.Controls.Add(this.cmbFromDesign);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // cmbToDesign
            // 
            this.cmbToDesign.DatabaseDesignId = null;
            this.cmbToDesign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToDesign.FormattingEnabled = true;
            resources.ApplyResources(this.cmbToDesign, "cmbToDesign");
            this.cmbToDesign.Name = "cmbToDesign";
            // 
            // cmbFromDesign
            // 
            this.cmbFromDesign.DatabaseDesignId = null;
            this.cmbFromDesign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromDesign.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFromDesign, "cmbFromDesign");
            this.cmbFromDesign.Name = "cmbFromDesign";
            // 
            // contextButtonsMaps
            // 
            this.contextButtonsMaps.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtonsMaps, "contextButtonsMaps");
            this.contextButtonsMaps.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonsMaps.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.contextButtonsMaps.EditButtonEnabled = true;
            this.contextButtonsMaps.Name = "contextButtonsMaps";
            this.contextButtonsMaps.RemoveButtonEnabled = true;
            this.contextButtonsMaps.EditButtonClicked += new System.EventHandler(this.EditMapClicked);
            this.contextButtonsMaps.AddButtonClicked += new System.EventHandler(this.AddMapClicked);
            this.contextButtonsMaps.RemoveButtonClicked += new System.EventHandler(this.RemoveMapClicked);
            // 
            // lblNoResults
            // 
            resources.ApplyResources(this.lblNoResults, "lblNoResults");
            this.lblNoResults.BackColor = System.Drawing.Color.Transparent;
            this.lblNoResults.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoResults.Name = "lblNoResults";
            // 
            // lvDatabaseMap
            // 
            resources.ApplyResources(this.lvDatabaseMap, "lvDatabaseMap");
            this.lvDatabaseMap.BuddyControl = null;
            this.lvDatabaseMap.Columns.Add(this.colFromDatabase);
            this.lvDatabaseMap.Columns.Add(this.colToDatabase);
            this.lvDatabaseMap.Columns.Add(this.colFromTable);
            this.lvDatabaseMap.Columns.Add(this.colToTable);
            this.lvDatabaseMap.ContentBackColor = System.Drawing.Color.White;
            this.lvDatabaseMap.DefaultRowHeight = ((short)(22));
            this.lvDatabaseMap.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDatabaseMap.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDatabaseMap.HeaderHeight = ((short)(25));
            this.lvDatabaseMap.Name = "lvDatabaseMap";
            this.lvDatabaseMap.OddRowColor = System.Drawing.Color.White;
            this.lvDatabaseMap.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDatabaseMap.SecondarySortColumn = ((short)(-1));
            this.lvDatabaseMap.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvDatabaseMap.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDatabaseMap.SortSetting = "0:1";
            this.lvDatabaseMap.SelectionChanged += new System.EventHandler(this.lvDatabaseMap_SelectionChanged);
            this.lvDatabaseMap.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvDatabaseMap_RowDoubleClick);
            // 
            // colFromDatabase
            // 
            this.colFromDatabase.AutoSize = true;
            this.colFromDatabase.DefaultStyle = null;
            resources.ApplyResources(this.colFromDatabase, "colFromDatabase");
            this.colFromDatabase.InternalSort = true;
            this.colFromDatabase.MaximumWidth = ((short)(0));
            this.colFromDatabase.MinimumWidth = ((short)(10));
            this.colFromDatabase.Tag = null;
            this.colFromDatabase.Width = ((short)(50));
            // 
            // colToDatabase
            // 
            this.colToDatabase.AutoSize = true;
            this.colToDatabase.DefaultStyle = null;
            resources.ApplyResources(this.colToDatabase, "colToDatabase");
            this.colToDatabase.InternalSort = true;
            this.colToDatabase.MaximumWidth = ((short)(0));
            this.colToDatabase.MinimumWidth = ((short)(10));
            this.colToDatabase.Tag = null;
            this.colToDatabase.Width = ((short)(50));
            // 
            // colFromTable
            // 
            this.colFromTable.AutoSize = true;
            this.colFromTable.DefaultStyle = null;
            resources.ApplyResources(this.colFromTable, "colFromTable");
            this.colFromTable.InternalSort = true;
            this.colFromTable.MaximumWidth = ((short)(0));
            this.colFromTable.MinimumWidth = ((short)(10));
            this.colFromTable.Tag = null;
            this.colFromTable.Width = ((short)(50));
            // 
            // colToTable
            // 
            this.colToTable.AutoSize = true;
            this.colToTable.DefaultStyle = null;
            resources.ApplyResources(this.colToTable, "colToTable");
            this.colToTable.InternalSort = true;
            this.colToTable.MaximumWidth = ((short)(0));
            this.colToTable.MinimumWidth = ((short)(10));
            this.colToTable.Tag = null;
            this.colToTable.Width = ((short)(50));
            // 
            // DatabaseMapListView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 90;
            this.Name = "DatabaseMapListView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private GroupPanel groupPanel1;
        private ContextButtons contextButtonsMaps;
        private Controls.DatabaseDesignComboBox cmbToDesign;
        private Controls.DatabaseDesignComboBox cmbFromDesign;
        private System.Windows.Forms.Label lblNoResults;
        private ListView lvDatabaseMap;
        private LSOne.Controls.Columns.Column colFromDatabase;
        private LSOne.Controls.Columns.Column colToDatabase;
        private LSOne.Controls.Columns.Column colFromTable;
        private LSOne.Controls.Columns.Column colToTable;
    }
}
