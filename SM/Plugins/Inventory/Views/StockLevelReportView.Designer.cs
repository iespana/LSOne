using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class StockLevelReportView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockLevelReportView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel1 = new GroupPanel();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFilterValue = new DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStore = new DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.btnShowReport);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.cmbFilter);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.cmbFilterValue);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.cmbStore);
            this.groupPanel1.Controls.Add(this.lblStore);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // cmbFilter
            // 
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            resources.GetString("cmbFilter.Items"),
            resources.GetString("cmbFilter.Items1"),
            resources.GetString("cmbFilter.Items2"),
            resources.GetString("cmbFilter.Items3")});
            resources.ApplyResources(this.cmbFilter, "cmbFilter");
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbFilterValue
            // 
            this.cmbFilterValue.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.cmbFilterValue, "cmbFilterValue");
            this.cmbFilterValue.MaxLength = 32767;
            this.cmbFilterValue.Name = "cmbFilterValue";
            this.cmbFilterValue.SelectedData = null;
            this.cmbFilterValue.SkipIDColumn = true;
            this.cmbFilterValue.RequestData += new System.EventHandler(this.cmbFilterValue_RequestData);
            this.cmbFilterValue.SelectedDataChanged += new System.EventHandler(this.CheckShowBtnEnabled);
            this.cmbFilterValue.RequestClear += new System.EventHandler(this.cmbFilterValue_RequestClear);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbStore
            // 
            this.cmbStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.SelectedData = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStoreSelect_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.CheckShowBtnEnabled);
            this.cmbStore.RequestClear += new System.EventHandler(this.cmbStoreSelect_RequestClear);
            // 
            // lblStore
            // 
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.Name = "lblStore";
            // 
            // btnShowReport
            // 
            resources.ApplyResources(this.btnShowReport, "btnShowReport");
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.UseVisualStyleBackColor = true;
            this.btnShowReport.Click += new System.EventHandler(this.btnShowReport_Click);
            // 
            // StockLevelReportView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 175;
            this.Name = "StockLevelReportView";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnShowReport;
        //private DevExpress.XtraEditors.RadioGroup rgrReportSelect;
        private System.Windows.Forms.Label lblStore;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbFilterValue;
        private System.Windows.Forms.Label label1;

    }
}
