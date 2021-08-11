using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class JobListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobListView));
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.chkShowDisabled = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRunJob = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lvJobs = new LSOne.Controls.ListView();
            this.colImage = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colSource = new LSOne.Controls.Columns.Column();
            this.colDestination = new LSOne.Controls.Columns.Column();
            this.colTrigger = new LSOne.Controls.Columns.Column();
            this.ttRunJob = new System.Windows.Forms.ToolTip(this.components);
            this.pnlBottom.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvJobs);
            this.pnlBottom.Controls.Add(this.flowLayoutPanel1);
            this.pnlBottom.Controls.Add(this.btnRunJob);
            this.pnlBottom.Controls.Add(this.contextButtons);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // contextButtons
            // 
            this.contextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtons, "contextButtons");
            this.contextButtons.BackColor = System.Drawing.Color.Transparent;
            this.contextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.contextButtons.EditButtonEnabled = true;
            this.contextButtons.Name = "contextButtons";
            this.contextButtons.RemoveButtonEnabled = true;
            this.contextButtons.EditButtonClicked += new System.EventHandler(this.contextButtons_EditButtonClicked);
            this.contextButtons.AddButtonClicked += new System.EventHandler(this.contextButtons_AddButtonClicked);
            this.contextButtons.RemoveButtonClicked += new System.EventHandler(this.contextButtons_RemoveButtonClicked);
            // 
            // chkShowDisabled
            // 
            resources.ApplyResources(this.chkShowDisabled, "chkShowDisabled");
            this.chkShowDisabled.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDisabled.Name = "chkShowDisabled";
            this.chkShowDisabled.UseVisualStyleBackColor = false;
            this.chkShowDisabled.CheckedChanged += new System.EventHandler(this.chkShowDisabled_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnRunJob
            // 
            resources.ApplyResources(this.btnRunJob, "btnRunJob");
            this.btnRunJob.Image = global::LSOne.ViewPlugins.Scheduler.Properties.Resources.run_options_16;
            this.btnRunJob.Name = "btnRunJob";
            this.ttRunJob.SetToolTip(this.btnRunJob, resources.GetString("btnRunJob.ToolTip"));
            this.btnRunJob.UseVisualStyleBackColor = true;
            this.btnRunJob.Click += new System.EventHandler(this.btnRunJob_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.chkShowDisabled);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lvJobs
            // 
            resources.ApplyResources(this.lvJobs, "lvJobs");
            this.lvJobs.BuddyControl = null;
            this.lvJobs.Columns.Add(this.colImage);
            this.lvJobs.Columns.Add(this.colDescription);
            this.lvJobs.Columns.Add(this.colSource);
            this.lvJobs.Columns.Add(this.colDestination);
            this.lvJobs.Columns.Add(this.colTrigger);
            this.lvJobs.ContentBackColor = System.Drawing.Color.White;
            this.lvJobs.DefaultRowHeight = ((short)(22));
            this.lvJobs.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvJobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvJobs.HeaderHeight = ((short)(25));
            this.lvJobs.Name = "lvJobs";
            this.lvJobs.OddRowColor = System.Drawing.Color.White;
            this.lvJobs.RowLineColor = System.Drawing.Color.LightGray;
            this.lvJobs.SecondarySortColumn = ((short)(-1));
            this.lvJobs.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvJobs.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJobs.SortSetting = "1:1";
            this.lvJobs.SelectionChanged += new System.EventHandler(this.lvJobs_SelectionChanged);
            this.lvJobs.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvJobs_RowDoubleClick);
            // 
            // colImage
            // 
            this.colImage.AutoSize = true;
            this.colImage.Clickable = false;
            this.colImage.DefaultStyle = null;
            resources.ApplyResources(this.colImage, "colImage");
            this.colImage.MaximumWidth = ((short)(0));
            this.colImage.MinimumWidth = ((short)(10));
            this.colImage.SecondarySortColumn = ((short)(-1));
            this.colImage.Sizable = false;
            this.colImage.Tag = null;
            this.colImage.Width = ((short)(16));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colSource
            // 
            this.colSource.AutoSize = true;
            this.colSource.DefaultStyle = null;
            resources.ApplyResources(this.colSource, "colSource");
            this.colSource.InternalSort = true;
            this.colSource.MaximumWidth = ((short)(0));
            this.colSource.MinimumWidth = ((short)(10));
            this.colSource.SecondarySortColumn = ((short)(-1));
            this.colSource.Tag = null;
            this.colSource.Width = ((short)(50));
            // 
            // colDestination
            // 
            this.colDestination.AutoSize = true;
            this.colDestination.DefaultStyle = null;
            resources.ApplyResources(this.colDestination, "colDestination");
            this.colDestination.InternalSort = true;
            this.colDestination.MaximumWidth = ((short)(0));
            this.colDestination.MinimumWidth = ((short)(10));
            this.colDestination.SecondarySortColumn = ((short)(-1));
            this.colDestination.Tag = null;
            this.colDestination.Width = ((short)(50));
            // 
            // colTrigger
            // 
            this.colTrigger.AutoSize = true;
            this.colTrigger.DefaultStyle = null;
            resources.ApplyResources(this.colTrigger, "colTrigger");
            this.colTrigger.InternalSort = true;
            this.colTrigger.MaximumWidth = ((short)(0));
            this.colTrigger.MinimumWidth = ((short)(10));
            this.colTrigger.SecondarySortColumn = ((short)(-1));
            this.colTrigger.Tag = null;
            this.colTrigger.Width = ((short)(50));
            // 
            // JobListView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 40;
            this.Name = "JobListView";
            this.pnlBottom.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons contextButtons;
        private System.Windows.Forms.CheckBox chkShowDisabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRunJob;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ListView lvJobs;
        private LSOne.Controls.Columns.Column colDescription;
        private LSOne.Controls.Columns.Column colSource;
        private LSOne.Controls.Columns.Column colDestination;
        private LSOne.Controls.Columns.Column colTrigger;
        private LSOne.Controls.Columns.Column colImage;
        private System.Windows.Forms.ToolTip ttRunJob;
    }
}
