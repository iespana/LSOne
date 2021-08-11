using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class JobLogControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobLogControl));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblDeleteAuditLogs = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lvJobLog = new LSOne.Controls.ListView();
            this.colTime = new LSOne.Controls.Columns.Column();
            this.colJob = new LSOne.Controls.Columns.Column();
            this.colMessage = new LSOne.Controls.Columns.Column();
            this.lvSubLogs = new LSOne.Controls.ListView();
            this.colSubJob = new LSOne.Controls.Columns.Column();
            this.colStartTime = new LSOne.Controls.Columns.Column();
            this.colEndTime = new LSOne.Controls.Columns.Column();
            this.colStartReplicationCounter = new LSOne.Controls.Columns.Column();
            this.colEndreplicationCounter = new LSOne.Controls.Columns.Column();
            this.coleRunAsNormal = new LSOne.Controls.Columns.Column();
            this.colLocation = new LSOne.Controls.Columns.Column();
            this.btnReExecute = new System.Windows.Forms.Button();
            this.chkSpecificLocation = new System.Windows.Forms.CheckBox();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Controls.Add(this.lblDeleteAuditLogs);
            this.groupPanel1.Controls.Add(this.dtpFromDate);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.dtpToDate);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblDeleteAuditLogs
            // 
            this.lblDeleteAuditLogs.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDeleteAuditLogs, "lblDeleteAuditLogs");
            this.lblDeleteAuditLogs.Name = "lblDeleteAuditLogs";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Checked = false;
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
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
            // 
            // lvJobLog
            // 
            resources.ApplyResources(this.lvJobLog, "lvJobLog");
            this.lvJobLog.BuddyControl = null;
            this.lvJobLog.Columns.Add(this.colTime);
            this.lvJobLog.Columns.Add(this.colJob);
            this.lvJobLog.Columns.Add(this.colMessage);
            this.lvJobLog.ContentBackColor = System.Drawing.Color.White;
            this.lvJobLog.DefaultRowHeight = ((short)(22));
            this.lvJobLog.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvJobLog.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvJobLog.HeaderHeight = ((short)(25));
            this.lvJobLog.Name = "lvJobLog";
            this.lvJobLog.OddRowColor = System.Drawing.Color.White;
            this.lvJobLog.RowLineColor = System.Drawing.Color.LightGray;
            this.lvJobLog.SecondarySortColumn = ((short)(-1));
            this.lvJobLog.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJobLog.SortSetting = "0:1";
            this.lvJobLog.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvJobLog_HeaderClicked);
            this.lvJobLog.SelectionChanged += new System.EventHandler(this.lvJobLog_SelectionChanged);
            // 
            // colTime
            // 
            this.colTime.AutoSize = true;
            this.colTime.DefaultStyle = null;
            resources.ApplyResources(this.colTime, "colTime");
            this.colTime.InternalSort = true;
            this.colTime.MaximumWidth = ((short)(0));
            this.colTime.MinimumWidth = ((short)(10));
            this.colTime.SecondarySortColumn = ((short)(-1));
            this.colTime.Tag = null;
            this.colTime.Width = ((short)(50));
            // 
            // colJob
            // 
            this.colJob.AutoSize = true;
            this.colJob.DefaultStyle = null;
            resources.ApplyResources(this.colJob, "colJob");
            this.colJob.InternalSort = true;
            this.colJob.MaximumWidth = ((short)(0));
            this.colJob.MinimumWidth = ((short)(10));
            this.colJob.SecondarySortColumn = ((short)(-1));
            this.colJob.Tag = null;
            this.colJob.Width = ((short)(50));
            // 
            // colMessage
            // 
            this.colMessage.AutoSize = true;
            this.colMessage.Clickable = false;
            this.colMessage.DefaultStyle = null;
            resources.ApplyResources(this.colMessage, "colMessage");
            this.colMessage.MaximumWidth = ((short)(0));
            this.colMessage.MinimumWidth = ((short)(10));
            this.colMessage.SecondarySortColumn = ((short)(-1));
            this.colMessage.Tag = null;
            this.colMessage.Width = ((short)(50));
            // 
            // lvSubLogs
            // 
            resources.ApplyResources(this.lvSubLogs, "lvSubLogs");
            this.lvSubLogs.BuddyControl = null;
            this.lvSubLogs.Columns.Add(this.colSubJob);
            this.lvSubLogs.Columns.Add(this.colStartTime);
            this.lvSubLogs.Columns.Add(this.colEndTime);
            this.lvSubLogs.Columns.Add(this.colStartReplicationCounter);
            this.lvSubLogs.Columns.Add(this.colEndreplicationCounter);
            this.lvSubLogs.Columns.Add(this.coleRunAsNormal);
            this.lvSubLogs.Columns.Add(this.colLocation);
            this.lvSubLogs.ContentBackColor = System.Drawing.Color.White;
            this.lvSubLogs.DefaultRowHeight = ((short)(22));
            this.lvSubLogs.DimSelectionWhenDisabled = true;
            this.lvSubLogs.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvSubLogs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSubLogs.HeaderHeight = ((short)(25));
            this.lvSubLogs.Name = "lvSubLogs";
            this.lvSubLogs.OddRowColor = System.Drawing.Color.White;
            this.lvSubLogs.RowLineColor = System.Drawing.Color.LightGray;
            this.lvSubLogs.SecondarySortColumn = ((short)(-1));
            this.lvSubLogs.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvSubLogs.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSubLogs.SortSetting = "0:1";
            // 
            // colSubJob
            // 
            this.colSubJob.AutoSize = true;
            this.colSubJob.DefaultStyle = null;
            resources.ApplyResources(this.colSubJob, "colSubJob");
            this.colSubJob.MaximumWidth = ((short)(0));
            this.colSubJob.MinimumWidth = ((short)(10));
            this.colSubJob.SecondarySortColumn = ((short)(-1));
            this.colSubJob.Tag = null;
            this.colSubJob.Width = ((short)(50));
            // 
            // colStartTime
            // 
            this.colStartTime.AutoSize = true;
            this.colStartTime.DefaultStyle = null;
            resources.ApplyResources(this.colStartTime, "colStartTime");
            this.colStartTime.MaximumWidth = ((short)(0));
            this.colStartTime.MinimumWidth = ((short)(10));
            this.colStartTime.SecondarySortColumn = ((short)(-1));
            this.colStartTime.Tag = null;
            this.colStartTime.Width = ((short)(50));
            // 
            // colEndTime
            // 
            this.colEndTime.AutoSize = true;
            this.colEndTime.DefaultStyle = null;
            resources.ApplyResources(this.colEndTime, "colEndTime");
            this.colEndTime.MaximumWidth = ((short)(0));
            this.colEndTime.MinimumWidth = ((short)(10));
            this.colEndTime.SecondarySortColumn = ((short)(-1));
            this.colEndTime.Tag = null;
            this.colEndTime.Width = ((short)(50));
            // 
            // colStartReplicationCounter
            // 
            this.colStartReplicationCounter.AutoSize = true;
            this.colStartReplicationCounter.DefaultStyle = null;
            resources.ApplyResources(this.colStartReplicationCounter, "colStartReplicationCounter");
            this.colStartReplicationCounter.MaximumWidth = ((short)(0));
            this.colStartReplicationCounter.MinimumWidth = ((short)(10));
            this.colStartReplicationCounter.SecondarySortColumn = ((short)(-1));
            this.colStartReplicationCounter.Tag = null;
            this.colStartReplicationCounter.Width = ((short)(50));
            // 
            // colEndreplicationCounter
            // 
            this.colEndreplicationCounter.AutoSize = true;
            this.colEndreplicationCounter.DefaultStyle = null;
            resources.ApplyResources(this.colEndreplicationCounter, "colEndreplicationCounter");
            this.colEndreplicationCounter.MaximumWidth = ((short)(0));
            this.colEndreplicationCounter.MinimumWidth = ((short)(10));
            this.colEndreplicationCounter.SecondarySortColumn = ((short)(-1));
            this.colEndreplicationCounter.Tag = null;
            this.colEndreplicationCounter.Width = ((short)(50));
            // 
            // coleRunAsNormal
            // 
            this.coleRunAsNormal.AutoSize = true;
            this.coleRunAsNormal.DefaultStyle = null;
            resources.ApplyResources(this.coleRunAsNormal, "coleRunAsNormal");
            this.coleRunAsNormal.MaximumWidth = ((short)(0));
            this.coleRunAsNormal.MinimumWidth = ((short)(10));
            this.coleRunAsNormal.SecondarySortColumn = ((short)(-1));
            this.coleRunAsNormal.Tag = null;
            this.coleRunAsNormal.Width = ((short)(50));
            // 
            // colLocation
            // 
            this.colLocation.AutoSize = true;
            this.colLocation.DefaultStyle = null;
            resources.ApplyResources(this.colLocation, "colLocation");
            this.colLocation.MaximumWidth = ((short)(0));
            this.colLocation.MinimumWidth = ((short)(10));
            this.colLocation.SecondarySortColumn = ((short)(-1));
            this.colLocation.Tag = null;
            this.colLocation.Width = ((short)(50));
            // 
            // btnReExecute
            // 
            resources.ApplyResources(this.btnReExecute, "btnReExecute");
            this.btnReExecute.Image = global::LSOne.ViewPlugins.Scheduler.Properties.Resources.run_options_16;
            this.btnReExecute.Name = "btnReExecute";
            this.btnReExecute.UseVisualStyleBackColor = true;
            this.btnReExecute.Click += new System.EventHandler(this.btnReExecute_Click);
            // 
            // chkSpecificLocation
            // 
            resources.ApplyResources(this.chkSpecificLocation, "chkSpecificLocation");
            this.chkSpecificLocation.Name = "chkSpecificLocation";
            this.chkSpecificLocation.UseVisualStyleBackColor = true;
            // 
            // JobLogControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkSpecificLocation);
            this.Controls.Add(this.btnReExecute);
            this.Controls.Add(this.lvSubLogs);
            this.Controls.Add(this.lvJobLog);
            this.Controls.Add(this.groupPanel1);
            this.Name = "JobLogControl";
            resources.ApplyResources(this, "$this");
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblDeleteAuditLogs;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private ListView lvJobLog;
        private LSOne.Controls.Columns.Column colTime;
        private LSOne.Controls.Columns.Column colJob;
        private LSOne.Controls.Columns.Column colMessage;
        private ListView lvSubLogs;
        private LSOne.Controls.Columns.Column colSubJob;
        private LSOne.Controls.Columns.Column colStartTime;
        private LSOne.Controls.Columns.Column colEndTime;
        private LSOne.Controls.Columns.Column colStartReplicationCounter;
        private LSOne.Controls.Columns.Column colEndreplicationCounter;
        private LSOne.Controls.Columns.Column coleRunAsNormal;
        private System.Windows.Forms.Button btnReExecute;
        private System.Windows.Forms.CheckBox chkSpecificLocation;
        private LSOne.Controls.Columns.Column colLocation;
    }
}
