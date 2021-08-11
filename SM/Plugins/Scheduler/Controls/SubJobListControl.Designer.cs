using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class SubJobListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobListControl));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnAddMultipleSubjobs = new System.Windows.Forms.Button();
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.lvSubJobs = new LSOne.Controls.ListView();
            this.colImage = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colMethod = new LSOne.Controls.Columns.Column();
            this.colTableProc = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnAddMultipleSubjobs);
            this.pnlBottom.Controls.Add(this.contextButtons);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Name = "pnlBottom";
            // 
            // btnAddMultipleSubjobs
            // 
            resources.ApplyResources(this.btnAddMultipleSubjobs, "btnAddMultipleSubjobs");
            this.btnAddMultipleSubjobs.Name = "btnAddMultipleSubjobs";
            this.btnAddMultipleSubjobs.UseVisualStyleBackColor = true;
            this.btnAddMultipleSubjobs.Click += new System.EventHandler(this.btnAddMultiple_Click);
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
            // lvSubJobs
            // 
            resources.ApplyResources(this.lvSubJobs, "lvSubJobs");
            this.lvSubJobs.BuddyControl = null;
            this.lvSubJobs.Columns.Add(this.colImage);
            this.lvSubJobs.Columns.Add(this.colDescription);
            this.lvSubJobs.Columns.Add(this.colMethod);
            this.lvSubJobs.Columns.Add(this.colTableProc);
            this.lvSubJobs.ContentBackColor = System.Drawing.Color.White;
            this.lvSubJobs.DefaultRowHeight = ((short)(18));
            this.lvSubJobs.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvSubJobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSubJobs.HeaderHeight = ((short)(20));
            this.lvSubJobs.Name = "lvSubJobs";
            this.lvSubJobs.OddRowColor = System.Drawing.Color.White;
            this.lvSubJobs.RowLineColor = System.Drawing.Color.LightGray;
            this.lvSubJobs.SecondarySortColumn = ((short)(-1));
            this.lvSubJobs.SortSetting = "0:1";
            this.lvSubJobs.SelectionChanged += new System.EventHandler(this.lvSubJobs_SelectionChanged);
            this.lvSubJobs.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvSubJobs_RowDoubleClick);
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
            // colMethod
            // 
            this.colMethod.AutoSize = true;
            this.colMethod.DefaultStyle = null;
            resources.ApplyResources(this.colMethod, "colMethod");
            this.colMethod.InternalSort = true;
            this.colMethod.MaximumWidth = ((short)(0));
            this.colMethod.MinimumWidth = ((short)(10));
            this.colMethod.SecondarySortColumn = ((short)(-1));
            this.colMethod.Tag = null;
            this.colMethod.Width = ((short)(50));
            // 
            // colTableProc
            // 
            this.colTableProc.AutoSize = true;
            this.colTableProc.DefaultStyle = null;
            resources.ApplyResources(this.colTableProc, "colTableProc");
            this.colTableProc.InternalSort = true;
            this.colTableProc.MaximumWidth = ((short)(0));
            this.colTableProc.MinimumWidth = ((short)(10));
            this.colTableProc.SecondarySortColumn = ((short)(-1));
            this.colTableProc.Tag = null;
            this.colTableProc.Width = ((short)(50));
            // 
            // SubJobListControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvSubJobs);
            this.Controls.Add(this.pnlBottom);
            this.Name = "SubJobListControl";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private ContextButtons contextButtons;
        private ListView lvSubJobs;
        private LSOne.Controls.Columns.Column colImage;
        private LSOne.Controls.Columns.Column colDescription;
        private LSOne.Controls.Columns.Column colMethod;
        private LSOne.Controls.Columns.Column colTableProc;
        private System.Windows.Forms.Button btnAddMultipleSubjobs;
    }
}
