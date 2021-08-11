using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class JobSubJobsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobSubJobsPage));
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.label8 = new System.Windows.Forms.Label();
            this.btnMoveDown = new LSOne.Controls.ContextButton();
            this.btnMoveUp = new LSOne.Controls.ContextButton();
            this.cmbSubJobJob = new System.Windows.Forms.ComboBox();
            this.lvJobSubJobs = new LSOne.Controls.ListView();
            this.colEnabled = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colMethod = new LSOne.Controls.Columns.Column();
            this.colTableProcedure = new LSOne.Controls.Columns.Column();
            this.colDesign = new LSOne.Controls.Columns.Column();
            this.colMoveActions = new LSOne.Controls.Columns.Column();
            this.btnOpenJob = new LSOne.Controls.ContextButton();
            this.SuspendLayout();
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
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnMoveDown
            // 
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // cmbSubJobJob
            // 
            this.cmbSubJobJob.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubJobJob.FormattingEnabled = true;
            resources.ApplyResources(this.cmbSubJobJob, "cmbSubJobJob");
            this.cmbSubJobJob.Name = "cmbSubJobJob";
            this.cmbSubJobJob.SelectedIndexChanged += new System.EventHandler(this.cmbSubJobJob_SelectedIndexChanged);
            // 
            // lvJobSubJobs
            // 
            resources.ApplyResources(this.lvJobSubJobs, "lvJobSubJobs");
            this.lvJobSubJobs.BuddyControl = null;
            this.lvJobSubJobs.Columns.Add(this.colEnabled);
            this.lvJobSubJobs.Columns.Add(this.colDescription);
            this.lvJobSubJobs.Columns.Add(this.colMethod);
            this.lvJobSubJobs.Columns.Add(this.colTableProcedure);
            this.lvJobSubJobs.Columns.Add(this.colDesign);
            this.lvJobSubJobs.Columns.Add(this.colMoveActions);
            this.lvJobSubJobs.ContentBackColor = System.Drawing.Color.White;
            this.lvJobSubJobs.DefaultRowHeight = ((short)(22));
            this.lvJobSubJobs.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvJobSubJobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvJobSubJobs.HeaderHeight = ((short)(25));
            this.lvJobSubJobs.Name = "lvJobSubJobs";
            this.lvJobSubJobs.OddRowColor = System.Drawing.Color.White;
            this.lvJobSubJobs.RowLineColor = System.Drawing.Color.LightGray;
            this.lvJobSubJobs.SecondarySortColumn = ((short)(-1));
            this.lvJobSubJobs.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvJobSubJobs.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJobSubJobs.SortSetting = "0:1";
            this.lvJobSubJobs.SelectionChanged += new System.EventHandler(this.lvJobSubJobs_SelectionChanged);
            this.lvJobSubJobs.CellAction += new LSOne.Controls.CellActionDelegate(this.lvJobSubJobs_CellAction);
            this.lvJobSubJobs.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvJobSubJobs_RowDoubleClick);
            // 
            // colEnabled
            // 
            this.colEnabled.AutoSize = true;
            this.colEnabled.Clickable = false;
            this.colEnabled.DefaultStyle = null;
            resources.ApplyResources(this.colEnabled, "colEnabled");
            this.colEnabled.MaximumWidth = ((short)(0));
            this.colEnabled.MinimumWidth = ((short)(10));
            this.colEnabled.SecondarySortColumn = ((short)(-1));
            this.colEnabled.Tag = null;
            this.colEnabled.Width = ((short)(20));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.Clickable = false;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colMethod
            // 
            this.colMethod.AutoSize = true;
            this.colMethod.Clickable = false;
            this.colMethod.DefaultStyle = null;
            resources.ApplyResources(this.colMethod, "colMethod");
            this.colMethod.MaximumWidth = ((short)(0));
            this.colMethod.MinimumWidth = ((short)(10));
            this.colMethod.SecondarySortColumn = ((short)(-1));
            this.colMethod.Tag = null;
            this.colMethod.Width = ((short)(50));
            // 
            // colTableProcedure
            // 
            this.colTableProcedure.AutoSize = true;
            this.colTableProcedure.Clickable = false;
            this.colTableProcedure.DefaultStyle = null;
            resources.ApplyResources(this.colTableProcedure, "colTableProcedure");
            this.colTableProcedure.MaximumWidth = ((short)(0));
            this.colTableProcedure.MinimumWidth = ((short)(10));
            this.colTableProcedure.SecondarySortColumn = ((short)(-1));
            this.colTableProcedure.Tag = null;
            this.colTableProcedure.Width = ((short)(50));
            // 
            // colDesign
            // 
            this.colDesign.AutoSize = true;
            this.colDesign.Clickable = false;
            this.colDesign.DefaultStyle = null;
            resources.ApplyResources(this.colDesign, "colDesign");
            this.colDesign.MaximumWidth = ((short)(0));
            this.colDesign.MinimumWidth = ((short)(10));
            this.colDesign.SecondarySortColumn = ((short)(-1));
            this.colDesign.Tag = null;
            this.colDesign.Width = ((short)(50));
            // 
            // colMoveActions
            // 
            this.colMoveActions.AutoSize = true;
            this.colMoveActions.DefaultStyle = null;
            resources.ApplyResources(this.colMoveActions, "colMoveActions");
            this.colMoveActions.MaximumWidth = ((short)(0));
            this.colMoveActions.MinimumWidth = ((short)(10));
            this.colMoveActions.SecondarySortColumn = ((short)(-1));
            this.colMoveActions.Tag = null;
            this.colMoveActions.Width = ((short)(50));
            // 
            // btnOpenJob
            // 
            this.btnOpenJob.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnOpenJob, "btnOpenJob");
            this.btnOpenJob.Name = "btnOpenJob";
            this.btnOpenJob.Click += new System.EventHandler(this.btnOpenJob_Click);
            // 
            // JobSubJobsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnOpenJob);
            this.Controls.Add(this.lvJobSubJobs);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.contextButtons);
            this.Controls.Add(this.cmbSubJobJob);
            this.Name = "JobSubJobsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons contextButtons;
        private System.Windows.Forms.Label label8;
        private ContextButton btnMoveDown;
        private ContextButton btnMoveUp;
        private System.Windows.Forms.ComboBox cmbSubJobJob;
        private ListView lvJobSubJobs;
        private LSOne.Controls.Columns.Column colDescription;
        private LSOne.Controls.Columns.Column colMethod;
        private LSOne.Controls.Columns.Column colTableProcedure;
        private LSOne.Controls.Columns.Column colDesign;
        private LSOne.Controls.Columns.Column colEnabled;
        private LSOne.Controls.Columns.Column colMoveActions;
        private ContextButton btnOpenJob;
    }
}
