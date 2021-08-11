using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class InventoryAdjustmentView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryAdjustmentView));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lvJournalEntries = new LSOne.Controls.ListView();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.groupPanel3 = new LSOne.Controls.GroupPanel();
            this.lvJournalTrans = new LSOne.Controls.ListView();
            this.column11 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.btnAddAdjustmentLine = new LSOne.Controls.ContextButton();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnAddInventoryAdjustment = new LSOne.Controls.ContextButton();
            this.pnlBottom.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lvJournalEntries, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAddInventoryAdjustment, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // lvJournalEntries
            // 
            resources.ApplyResources(this.lvJournalEntries, "lvJournalEntries");
            this.lvJournalEntries.BuddyControl = null;
            this.lvJournalEntries.Columns.Add(this.column7);
            this.lvJournalEntries.Columns.Add(this.column8);
            this.lvJournalEntries.Columns.Add(this.column9);
            this.lvJournalEntries.Columns.Add(this.column10);
            this.tableLayoutPanel1.SetColumnSpan(this.lvJournalEntries, 2);
            this.lvJournalEntries.ContentBackColor = System.Drawing.Color.White;
            this.lvJournalEntries.DefaultRowHeight = ((short)(22));
            this.lvJournalEntries.DimSelectionWhenDisabled = true;
            this.lvJournalEntries.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvJournalEntries.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvJournalEntries.HeaderHeight = ((short)(25));
            this.lvJournalEntries.HorizontalScrollbar = true;
            this.lvJournalEntries.Name = "lvJournalEntries";
            this.lvJournalEntries.OddRowColor = System.Drawing.Color.White;
            this.lvJournalEntries.RowLineColor = System.Drawing.Color.LightGray;
            this.lvJournalEntries.SecondarySortColumn = ((short)(-1));
            this.lvJournalEntries.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvJournalEntries.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJournalEntries.SortSetting = "0:1";
            this.lvJournalEntries.SelectionChanged += new System.EventHandler(this.lvJournalEntries_SelectionChanged);
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.InternalSort = true;
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.InternalSort = true;
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.InternalSort = true;
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.SecondarySortColumn = ((short)(-1));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
            // 
            // column10
            // 
            this.column10.AutoSize = true;
            this.column10.DefaultStyle = null;
            resources.ApplyResources(this.column10, "column10");
            this.column10.InternalSort = true;
            this.column10.MaximumWidth = ((short)(0));
            this.column10.MinimumWidth = ((short)(10));
            this.column10.SecondarySortColumn = ((short)(-1));
            this.column10.Tag = null;
            this.column10.Width = ((short)(50));
            // 
            // groupPanel3
            // 
            resources.ApplyResources(this.groupPanel3, "groupPanel3");
            this.tableLayoutPanel1.SetColumnSpan(this.groupPanel3, 2);
            this.groupPanel3.Controls.Add(this.lvJournalTrans);
            this.groupPanel3.Controls.Add(this.lblGroupHeader);
            this.groupPanel3.Controls.Add(this.btnAddAdjustmentLine);
            this.groupPanel3.Controls.Add(this.lblNoSelection);
            this.groupPanel3.Name = "groupPanel3";
            // 
            // lvJournalTrans
            // 
            resources.ApplyResources(this.lvJournalTrans, "lvJournalTrans");
            this.lvJournalTrans.BuddyControl = null;
            this.lvJournalTrans.Columns.Add(this.column11);
            this.lvJournalTrans.Columns.Add(this.column1);
            this.lvJournalTrans.Columns.Add(this.column2);
            this.lvJournalTrans.Columns.Add(this.column3);
            this.lvJournalTrans.Columns.Add(this.column4);
            this.lvJournalTrans.Columns.Add(this.column5);
            this.lvJournalTrans.Columns.Add(this.column6);
            this.lvJournalTrans.ContentBackColor = System.Drawing.Color.White;
            this.lvJournalTrans.DefaultRowHeight = ((short)(22));
            this.lvJournalTrans.DimSelectionWhenDisabled = true;
            this.lvJournalTrans.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvJournalTrans.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvJournalTrans.HeaderHeight = ((short)(25));
            this.lvJournalTrans.HorizontalScrollbar = true;
            this.lvJournalTrans.Name = "lvJournalTrans";
            this.lvJournalTrans.OddRowColor = System.Drawing.Color.White;
            this.lvJournalTrans.RowLineColor = System.Drawing.Color.LightGray;
            this.lvJournalTrans.SecondarySortColumn = ((short)(-1));
            this.lvJournalTrans.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvJournalTrans.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJournalTrans.SortSetting = "1:1";
            // 
            // column11
            // 
            this.column11.AutoSize = true;
            this.column11.DefaultStyle = null;
            resources.ApplyResources(this.column11, "column11");
            this.column11.InternalSort = true;
            this.column11.MaximumWidth = ((short)(0));
            this.column11.MinimumWidth = ((short)(10));
            this.column11.SecondarySortColumn = ((short)(-1));
            this.column11.Tag = null;
            this.column11.Width = ((short)(50));
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
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            this.column2.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.NoTextWhenSmall = true;
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
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
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // btnAddAdjustmentLine
            // 
            resources.ApplyResources(this.btnAddAdjustmentLine, "btnAddAdjustmentLine");
            this.btnAddAdjustmentLine.BackColor = System.Drawing.Color.Transparent;
            this.btnAddAdjustmentLine.Context = LSOne.Controls.ButtonType.Add;
            this.btnAddAdjustmentLine.Name = "btnAddAdjustmentLine";
            this.btnAddAdjustmentLine.Click += new System.EventHandler(this.AddAdjustmentLine);
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnAddInventoryAdjustment
            // 
            resources.ApplyResources(this.btnAddInventoryAdjustment, "btnAddInventoryAdjustment");
            this.btnAddInventoryAdjustment.BackColor = System.Drawing.Color.Transparent;
            this.btnAddInventoryAdjustment.Context = LSOne.Controls.ButtonType.Add;
            this.btnAddInventoryAdjustment.Name = "btnAddInventoryAdjustment";
            this.btnAddInventoryAdjustment.Click += new System.EventHandler(this.AddInventoryAdjustment);
            // 
            // InventoryAdjustmentView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "InventoryAdjustmentView";
            this.pnlBottom.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private GroupPanel groupPanel3;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
        private ContextButton btnAddInventoryAdjustment;
        private ContextButton btnAddAdjustmentLine;
        private ListView lvJournalEntries;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private ListView lvJournalTrans;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
        private Controls.Columns.Column column8;
        private Controls.Columns.Column column9;
        private Controls.Columns.Column column10;
        private Controls.Columns.Column column11;
    }
}
