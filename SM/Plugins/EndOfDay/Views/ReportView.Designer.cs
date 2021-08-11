using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.Views
{
    partial class ReportView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.cmbStatement = new LSOne.Controls.DualDataComboBox();
            this.cmbStoreSelect = new LSOne.Controls.DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.rbDateSpan = new System.Windows.Forms.RadioButton();
            this.rbTodaysReport = new System.Windows.Forms.RadioButton();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnViewReport = new System.Windows.Forms.Button();
            this.lblStatement = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnViewReport);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lblStatement);
            this.groupPanel1.Controls.Add(this.cmbStatement);
            this.groupPanel1.Controls.Add(this.cmbStoreSelect);
            this.groupPanel1.Controls.Add(this.lblStore);
            this.groupPanel1.Controls.Add(this.rbDateSpan);
            this.groupPanel1.Controls.Add(this.rbTodaysReport);
            this.groupPanel1.Controls.Add(this.dtpFromDate);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.dtpToDate);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // cmbStatement
            // 
            this.cmbStatement.AddList = null;
            this.cmbStatement.AllowKeyboardSelection = false;
            this.cmbStatement.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.cmbStatement, "cmbStatement");
            this.cmbStatement.MaxLength = 32767;
            this.cmbStatement.Name = "cmbStatement";
            this.cmbStatement.OnlyDisplayID = false;
            this.cmbStatement.RemoveList = null;
            this.cmbStatement.RowHeight = ((short)(22));
            this.cmbStatement.SelectedData = null;
            this.cmbStatement.SelectedDataID = null;
            this.cmbStatement.SelectionList = null;
            this.cmbStatement.SkipIDColumn = false;
            this.cmbStatement.RequestData += new System.EventHandler(this.cmbStatement_RequestData);
            this.cmbStatement.SelectedDataChanged += new System.EventHandler(this.cmbStatement_SelectedDataChanged);
            this.cmbStatement.RequestClear += new System.EventHandler(this.cmbStatement_RequestClear);
            // 
            // cmbStoreSelect
            // 
            this.cmbStoreSelect.AddList = null;
            this.cmbStoreSelect.AllowKeyboardSelection = false;
            this.cmbStoreSelect.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.cmbStoreSelect, "cmbStoreSelect");
            this.cmbStoreSelect.MaxLength = 32767;
            this.cmbStoreSelect.Name = "cmbStoreSelect";
            this.cmbStoreSelect.OnlyDisplayID = false;
            this.cmbStoreSelect.RemoveList = null;
            this.cmbStoreSelect.RowHeight = ((short)(22));
            this.cmbStoreSelect.SelectedData = null;
            this.cmbStoreSelect.SelectedDataID = null;
            this.cmbStoreSelect.SelectionList = null;
            this.cmbStoreSelect.SkipIDColumn = false;
            this.cmbStoreSelect.RequestData += new System.EventHandler(this.cmbStoreSelect_RequestData);
            this.cmbStoreSelect.SelectedDataChanged += new System.EventHandler(this.cmbStoreSelect_SelectedDataChanged);
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            this.lblStore.Name = "lblStore";
            // 
            // rbDateSpan
            // 
            resources.ApplyResources(this.rbDateSpan, "rbDateSpan");
            this.rbDateSpan.BackColor = System.Drawing.Color.Transparent;
            this.rbDateSpan.Name = "rbDateSpan";
            this.rbDateSpan.Tag = "1";
            this.rbDateSpan.UseVisualStyleBackColor = false;
            this.rbDateSpan.Click += new System.EventHandler(this.rbTodaysReport_Click);
            // 
            // rbTodaysReport
            // 
            resources.ApplyResources(this.rbTodaysReport, "rbTodaysReport");
            this.rbTodaysReport.BackColor = System.Drawing.Color.Transparent;
            this.rbTodaysReport.Checked = true;
            this.rbTodaysReport.Name = "rbTodaysReport";
            this.rbTodaysReport.TabStop = true;
            this.rbTodaysReport.Tag = "0";
            this.rbTodaysReport.UseVisualStyleBackColor = false;
            this.rbTodaysReport.Click += new System.EventHandler(this.rbTodaysReport_Click);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Checked = false;
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            this.dtpFromDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStmtFrom_KeyUp_1);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Checked = false;
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            this.dtpToDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStmtFrom_KeyUp_1);
            // 
            // btnViewReport
            // 
            resources.ApplyResources(this.btnViewReport, "btnViewReport");
            this.btnViewReport.Name = "btnViewReport";
            this.btnViewReport.UseVisualStyleBackColor = true;
            this.btnViewReport.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblStatement
            // 
            resources.ApplyResources(this.lblStatement, "lblStatement");
            this.lblStatement.BackColor = System.Drawing.Color.Transparent;
            this.lblStatement.Name = "lblStatement";
            // 
            // ReportView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 175;
            this.Name = "ReportView";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnViewReport;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.RadioButton rbDateSpan;
        private System.Windows.Forms.RadioButton rbTodaysReport;
        private DualDataComboBox cmbStoreSelect;
        private System.Windows.Forms.Label lblStore;
        private DualDataComboBox cmbStatement;
        private System.Windows.Forms.Label lblStatement;

    }
}
