using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class ReplicationCountersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationCountersView));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSubJobs = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbJobs = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextButtonEdit = new LSOne.Controls.ContextButton();
            this.contextButtonRemove = new LSOne.Controls.ContextButton();
            this.lvRepCounters = new LSOne.Controls.ListView();
            this.colJob = new LSOne.Controls.Columns.Column();
            this.colSubjob = new LSOne.Controls.Columns.Column();
            this.colLocation = new LSOne.Controls.Columns.Column();
            this.colcounter = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvRepCounters);
            this.pnlBottom.Controls.Add(this.contextButtonRemove);
            this.pnlBottom.Controls.Add(this.contextButtonEdit);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Controls.Add(this.cmbLocations);
            this.groupPanel1.Controls.Add(this.label3);
            this.groupPanel1.Controls.Add(this.cmbSubJobs);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.cmbJobs);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbLocations
            // 
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLocations, "cmbLocations");
            this.cmbLocations.Name = "cmbLocations";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbSubJobs
            // 
            this.cmbSubJobs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubJobs.FormattingEnabled = true;
            resources.ApplyResources(this.cmbSubJobs, "cmbSubJobs");
            this.cmbSubJobs.Name = "cmbSubJobs";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbJobs
            // 
            this.cmbJobs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJobs.FormattingEnabled = true;
            resources.ApplyResources(this.cmbJobs, "cmbJobs");
            this.cmbJobs.Name = "cmbJobs";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // contextButtonEdit
            // 
            resources.ApplyResources(this.contextButtonEdit, "contextButtonEdit");
            this.contextButtonEdit.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonEdit.Context = LSOne.Controls.ButtonType.Edit;
            this.contextButtonEdit.Name = "contextButtonEdit";
            this.contextButtonEdit.Click += new System.EventHandler(this.contextButtonEdit_Click);
            // 
            // contextButtonRemove
            // 
            resources.ApplyResources(this.contextButtonRemove, "contextButtonRemove");
            this.contextButtonRemove.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonRemove.Context = LSOne.Controls.ButtonType.Remove;
            this.contextButtonRemove.Name = "contextButtonRemove";
            this.contextButtonRemove.Click += new System.EventHandler(this.contextButtonRemove_Click);
            // 
            // lvRepCounters
            // 
            resources.ApplyResources(this.lvRepCounters, "lvRepCounters");
            this.lvRepCounters.BorderColor = System.Drawing.Color.DarkGray;
            this.lvRepCounters.BuddyControl = null;
            this.lvRepCounters.Columns.Add(this.colJob);
            this.lvRepCounters.Columns.Add(this.colSubjob);
            this.lvRepCounters.Columns.Add(this.colLocation);
            this.lvRepCounters.Columns.Add(this.colcounter);
            this.lvRepCounters.ContentBackColor = System.Drawing.Color.White;
            this.lvRepCounters.ContextMenuStrip = this.contextMenuStrip;
            this.lvRepCounters.DefaultRowHeight = ((short)(22));
            this.lvRepCounters.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvRepCounters.HeaderBackColor = System.Drawing.Color.White;
            this.lvRepCounters.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvRepCounters.HeaderHeight = ((short)(25));
            this.lvRepCounters.Name = "lvRepCounters";
            this.lvRepCounters.OddRowColor = System.Drawing.Color.White;
            this.lvRepCounters.RowLineColor = System.Drawing.Color.LightGray;
            this.lvRepCounters.SecondarySortColumn = ((short)(-1));
            this.lvRepCounters.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvRepCounters.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvRepCounters.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvRepCounters.SortSetting = "0:1";
            this.lvRepCounters.VerticalScrollbarValue = 0;
            this.lvRepCounters.VerticalScrollbarYOffset = 0;
            this.lvRepCounters.SelectionChanged += new System.EventHandler(this.lvRepCounters_SelectionChanged);
            this.lvRepCounters.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvRepCounters_RowDoubleClick);
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
            // colSubjob
            // 
            this.colSubjob.AutoSize = true;
            this.colSubjob.DefaultStyle = null;
            resources.ApplyResources(this.colSubjob, "colSubjob");
            this.colSubjob.InternalSort = true;
            this.colSubjob.MaximumWidth = ((short)(0));
            this.colSubjob.MinimumWidth = ((short)(10));
            this.colSubjob.SecondarySortColumn = ((short)(-1));
            this.colSubjob.Tag = null;
            this.colSubjob.Width = ((short)(50));
            // 
            // colLocation
            // 
            this.colLocation.AutoSize = true;
            this.colLocation.DefaultStyle = null;
            resources.ApplyResources(this.colLocation, "colLocation");
            this.colLocation.InternalSort = true;
            this.colLocation.MaximumWidth = ((short)(0));
            this.colLocation.MinimumWidth = ((short)(10));
            this.colLocation.SecondarySortColumn = ((short)(-1));
            this.colLocation.Tag = null;
            this.colLocation.Width = ((short)(50));
            // 
            // colcounter
            // 
            this.colcounter.AutoSize = true;
            this.colcounter.DefaultStyle = null;
            resources.ApplyResources(this.colcounter, "colcounter");
            this.colcounter.InternalSort = true;
            this.colcounter.MaximumWidth = ((short)(0));
            this.colcounter.MinimumWidth = ((short)(10));
            this.colcounter.SecondarySortColumn = ((short)(-1));
            this.colcounter.Tag = null;
            this.colcounter.Width = ((short)(50));
            // 
            // ReplicationCountersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 120;
            this.Name = "ReplicationCountersView";
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbJobs;
        private System.Windows.Forms.ComboBox cmbLocations;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSubJobs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private ContextButton contextButtonRemove;
        private ContextButton contextButtonEdit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private ListView lvRepCounters;
        private LSOne.Controls.Columns.Column colJob;
        private LSOne.Controls.Columns.Column colSubjob;
        private LSOne.Controls.Columns.Column colLocation;
        private LSOne.Controls.Columns.Column colcounter;
    }
}
