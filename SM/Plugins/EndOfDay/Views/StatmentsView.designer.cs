using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.EndOfDay.Views
{
    partial class StatementsView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatementsView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvItems = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnEdit = new LSOne.Controls.ContextButton();
            this.lvValues = new LSOne.Controls.ListView();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnStatementButtons = new LSOne.Controls.ContextButtons();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnRecalculate = new System.Windows.Forms.Button();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnStatementButtons);
            this.pnlBottom.Controls.Add(this.btnRecalculate);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.btnPost);
            this.pnlBottom.Controls.Add(this.btnCalculate);
            this.pnlBottom.Controls.Add(this.lvItems);
            this.pnlBottom.Controls.Add(this.cmbStore);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.btnShowReport);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5});
            this.lvItems.FullRowSelect = true;
            this.lvItems.HideSelection = false;
            this.lvItems.LockDrawing = false;
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.SortColumn = 0;
            this.lvItems.SortedBackwards = false;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.UseEveryOtherRowColoring = true;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItems_ColumnClick);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnEdit);
            this.groupPanel1.Controls.Add(this.lvValues);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.BuddyControl = null;
            this.lvValues.Columns.Add(this.column10);
            this.lvValues.Columns.Add(this.column1);
            this.lvValues.Columns.Add(this.column3);
            this.lvValues.Columns.Add(this.column4);
            this.lvValues.Columns.Add(this.column5);
            this.lvValues.Columns.Add(this.column6);
            this.lvValues.Columns.Add(this.column7);
            this.lvValues.Columns.Add(this.column8);
            this.lvValues.Columns.Add(this.column2);
            this.lvValues.Columns.Add(this.column9);
            this.lvValues.ContentBackColor = System.Drawing.Color.White;
            this.lvValues.DefaultRowHeight = ((short)(18));
            this.lvValues.EvenRowColor = System.Drawing.Color.White;
            this.lvValues.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvValues.HeaderHeight = ((short)(20));
            this.lvValues.Name = "lvValues";
            this.lvValues.OddRowColor = System.Drawing.Color.White;
            this.lvValues.RowLineColor = System.Drawing.Color.LightGray;
            this.lvValues.SecondarySortColumn = ((short)(2));
            this.lvValues.SortSetting = "0:1";
            this.lvValues.SelectionChanged += new System.EventHandler(this.lvValues_SelectedIndexChanged);
            this.lvValues.RowExpanded += new LSOne.Controls.RowClickDelegate(this.lvValues_RowExpanded);
            this.lvValues.DoubleClick += new System.EventHandler(this.lvValues_DoubleClick);
            // 
            // column10
            // 
            this.column10.AutoSize = true;
            this.column10.DefaultStyle = null;
            resources.ApplyResources(this.column10, "column10");
            this.column10.MaximumWidth = ((short)(0));
            this.column10.MinimumWidth = ((short)(10));
            this.column10.SecondarySortColumn = ((short)(-1));
            this.column10.Tag = null;
            this.column10.Width = ((short)(50));
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
            this.column4.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
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
            this.column8.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
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
            this.column2.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.Clickable = false;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.SecondarySortColumn = ((short)(-1));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
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
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.FormatData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnCalculate
            // 
            resources.ApplyResources(this.btnCalculate, "btnCalculate");
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.CalculateStatement);
            // 
            // btnStatementButtons
            // 
            this.btnStatementButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnStatementButtons, "btnStatementButtons");
            this.btnStatementButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnStatementButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnStatementButtons.EditButtonEnabled = true;
            this.btnStatementButtons.Name = "btnStatementButtons";
            this.btnStatementButtons.RemoveButtonEnabled = true;
            this.btnStatementButtons.EditButtonClicked += new System.EventHandler(this.btnStatementButtons_EditButtonClicked);
            this.btnStatementButtons.AddButtonClicked += new System.EventHandler(this.btnStatementButtons_AddButtonClicked);
            this.btnStatementButtons.RemoveButtonClicked += new System.EventHandler(this.btnStatementButtons_RemoveButtonClicked);
            // 
            // btnPost
            // 
            resources.ApplyResources(this.btnPost, "btnPost");
            this.btnPost.Name = "btnPost";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // btnRecalculate
            // 
            resources.ApplyResources(this.btnRecalculate, "btnRecalculate");
            this.btnRecalculate.Name = "btnRecalculate";
            this.btnRecalculate.UseVisualStyleBackColor = true;
            this.btnRecalculate.Click += new System.EventHandler(this.RecalculateStatement);
            // 
            // btnShowReport
            // 
            resources.ApplyResources(this.btnShowReport, "btnShowReport");
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.UseVisualStyleBackColor = true;
            this.btnShowReport.Click += new System.EventHandler(this.btnShowReport_Click);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.Context = LSOne.Controls.ButtonType.Edit;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEditValue_Click);
            // 
            // StatementsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 40;
            this.Name = "StatementsView";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ExtendedListView lvItems;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCalculate;
        private ContextButtons btnStatementButtons;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnRecalculate;
        private ListView lvValues;
        private Column column1;
        private Column column3;
        private Column column4;
        private Column column5;
        private Column column6;
        private Column column7;
        private Column column8;
        private Column column10;
        private System.Windows.Forms.Button btnShowReport;
        private Column column2;
        private Column column9;
        private ContextButton btnEdit;
    }
}
