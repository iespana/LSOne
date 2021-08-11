using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.Views
{
    partial class TruncateAuditLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TruncateAuditLogs));
            this.lblDeleteAuditLogs = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lvOldTransactions = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTimeout = new LSOne.Controls.NumericTextBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tbTimeout);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.lvOldTransactions);
            this.pnlBottom.Controls.Add(this.btnDelete);
            this.pnlBottom.Controls.Add(this.dtpToDate);
            this.pnlBottom.Controls.Add(this.lblDeleteAuditLogs);
            this.pnlBottom.SizeChanged += new System.EventHandler(this.pnlBottom_SizeChanged);
            // 
            // lblDeleteAuditLogs
            // 
            this.lblDeleteAuditLogs.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDeleteAuditLogs, "lblDeleteAuditLogs");
            this.lblDeleteAuditLogs.Name = "lblDeleteAuditLogs";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lvOldTransactions
            // 
            resources.ApplyResources(this.lvOldTransactions, "lvOldTransactions");
            this.lvOldTransactions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvOldTransactions.FullRowSelect = true;
            this.lvOldTransactions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvOldTransactions.HideSelection = false;
            this.lvOldTransactions.LockDrawing = false;
            this.lvOldTransactions.MultiSelect = false;
            this.lvOldTransactions.Name = "lvOldTransactions";
            this.lvOldTransactions.SortColumn = -1;
            this.lvOldTransactions.SortedBackwards = false;
            this.lvOldTransactions.UseCompatibleStateImageBehavior = false;
            this.lvOldTransactions.UseEveryOtherRowColoring = true;
            this.lvOldTransactions.View = System.Windows.Forms.View.Details;
            this.lvOldTransactions.StyleChanged += new System.EventHandler(this.lvOldTransactions_StyleChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
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
            // tbTimeout
            // 
            this.tbTimeout.AllowDecimal = false;
            this.tbTimeout.AllowNegative = false;
            this.tbTimeout.CultureInfo = null;
            this.tbTimeout.DecimalLetters = 2;
            this.tbTimeout.ForeColor = System.Drawing.Color.Black;
            this.tbTimeout.HasMinValue = false;
            resources.ApplyResources(this.tbTimeout, "tbTimeout");
            this.tbTimeout.MaxValue = 0D;
            this.tbTimeout.MinValue = 0D;
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Value = 30D;
            this.tbTimeout.Leave += new System.EventHandler(this.tbTimeout_Leave);
            // 
            // TruncateAuditLogs
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 60;
            this.Name = "TruncateAuditLogs";
            this.Load += new System.EventHandler(this.TruncateAuditLogs_Load);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDeleteAuditLogs;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnDelete;
        private ExtendedListView lvOldTransactions;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private NumericTextBox tbTimeout;
    }
}
