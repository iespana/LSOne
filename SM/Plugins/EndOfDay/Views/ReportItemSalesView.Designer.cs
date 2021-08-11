using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.Views
{
    partial class ReportItemSalesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportItemSalesView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel1 = new GroupPanel();
            this.cmbStoreSelect = new DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.rgrReportSelect = new DevExpress.XtraEditors.RadioGroup();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnGetReport = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgrReportSelect.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.btnGetReport);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.cmbStoreSelect);
            this.groupPanel1.Controls.Add(this.lblStore);
            this.groupPanel1.Controls.Add(this.rgrReportSelect);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.dtpFromDate);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.dtpToDate);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // cmbStoreSelect
            // 
            this.cmbStoreSelect.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.cmbStoreSelect, "cmbStoreSelect");
            this.cmbStoreSelect.MaxLength = 32767;
            this.cmbStoreSelect.Name = "cmbStoreSelect";
            this.cmbStoreSelect.SelectedData = null;
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
            // rgrReportSelect
            // 
            resources.ApplyResources(this.rgrReportSelect, "rgrReportSelect");
            this.rgrReportSelect.Name = "rgrReportSelect";
            this.rgrReportSelect.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rgrReportSelect.Properties.Appearance.Options.UseBackColor = true;
            this.rgrReportSelect.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rgrReportSelect.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((object)(resources.GetObject("rgrReportSelect.Properties.Items"))), resources.GetString("rgrReportSelect.Properties.Items1")),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((object)(resources.GetObject("rgrReportSelect.Properties.Items2"))), resources.GetString("rgrReportSelect.Properties.Items3")),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((object)(resources.GetObject("rgrReportSelect.Properties.Items4"))), resources.GetString("rgrReportSelect.Properties.Items5"))});
            this.rgrReportSelect.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Checked = false;
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged_1);
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
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // btnGetReport
            // 
            resources.ApplyResources(this.btnGetReport, "btnGetReport");
            this.btnGetReport.Name = "btnGetReport";
            this.btnGetReport.UseVisualStyleBackColor = true;
            this.btnGetReport.Click += new System.EventHandler(this.btnGetReport_Click);
            // 
            // ReportItemSalesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 175;
            this.Name = "ReportItemSalesView";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgrReportSelect.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGetReport;
        private DevExpress.XtraEditors.RadioGroup rgrReportSelect;
        private System.Windows.Forms.Label lblStore;
        private DualDataComboBox cmbStoreSelect;

    }
}
