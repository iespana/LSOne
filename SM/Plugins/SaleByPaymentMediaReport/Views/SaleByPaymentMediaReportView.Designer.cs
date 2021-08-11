using LSOne.Controls;

namespace LSOne.ViewPlugins.SaleByPaymentMediaReport.Views
{
    partial class SaleByPaymentMediaReportView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaleByPaymentMediaReportView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.cmbUse = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnGetReport = new System.Windows.Forms.Button();
            this.lvPayments = new LSOne.Controls.ListView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvPayments);
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
            this.groupPanel1.Controls.Add(this.tableLayoutPanel1);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // cmbUse
            // 
            this.cmbUse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUse.FormattingEnabled = true;
            resources.ApplyResources(this.cmbUse, "cmbUse");
            this.cmbUse.Name = "cmbUse";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            this.cmbStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStoreSelect_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStoreSelect_SelectedDataChanged);
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            this.lblStore.Name = "lblStore";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged_1);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // dtpToDate
            // 
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
            // lvPayments
            // 
            resources.ApplyResources(this.lvPayments, "lvPayments");
            this.lvPayments.BuddyControl = null;
            this.lvPayments.ContentBackColor = System.Drawing.Color.White;
            this.lvPayments.DefaultRowHeight = ((short)(22));
            this.lvPayments.EvenRowColor = System.Drawing.Color.White;
            this.lvPayments.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPayments.HeaderHeight = ((short)(25));
            this.lvPayments.HorizontalScrollbar = true;
            this.lvPayments.Name = "lvPayments";
            this.lvPayments.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPayments.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPayments.SecondarySortColumn = ((short)(-1));
            this.lvPayments.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPayments.SortSetting = "-1:1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbStore, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbUse, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblStore, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dtpFromDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dtpToDate, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // SaleByPaymentMediaReportView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 175;
            this.Name = "SaleByPaymentMediaReportView";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Label lblStore;
        private LSOne.Controls.DualDataComboBox cmbStore;
        private ListView lvPayments;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbUse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}
