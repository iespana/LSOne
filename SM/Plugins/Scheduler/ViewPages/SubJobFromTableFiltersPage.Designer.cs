using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class SubJobFromTableFiltersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobFromTableFiltersPage));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.lvFilters = new LSOne.Controls.ListView();
            this.colField = new LSOne.Controls.Columns.Column();
            this.colFilterType = new LSOne.Controls.Columns.Column();
            this.colValue1 = new LSOne.Controls.Columns.Column();
            this.colValue2 = new LSOne.Controls.Columns.Column();
            this.colApply = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
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
            // lvFilters
            // 
            resources.ApplyResources(this.lvFilters, "lvFilters");
            this.lvFilters.BuddyControl = null;
            this.lvFilters.Columns.Add(this.colField);
            this.lvFilters.Columns.Add(this.colFilterType);
            this.lvFilters.Columns.Add(this.colValue1);
            this.lvFilters.Columns.Add(this.colValue2);
            this.lvFilters.Columns.Add(this.colApply);
            this.lvFilters.ContentBackColor = System.Drawing.Color.White;
            this.lvFilters.ContextMenuStrip = this.contextMenuStrip;
            this.lvFilters.DefaultRowHeight = ((short)(22));
            this.lvFilters.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvFilters.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFilters.HeaderHeight = ((short)(25));
            this.lvFilters.Name = "lvFilters";
            this.lvFilters.OddRowColor = System.Drawing.Color.White;
            this.lvFilters.RowLineColor = System.Drawing.Color.LightGray;
            this.lvFilters.SecondarySortColumn = ((short)(-1));
            this.lvFilters.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvFilters.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvFilters.SortSetting = "0:1";
            this.lvFilters.SelectionChanged += new System.EventHandler(this.lvFilters_SelectionChanged);
            this.lvFilters.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvFilters_RowDoubleClick);
            // 
            // colField
            // 
            this.colField.AutoSize = true;
            this.colField.DefaultStyle = null;
            resources.ApplyResources(this.colField, "colField");
            this.colField.InternalSort = true;
            this.colField.MaximumWidth = ((short)(0));
            this.colField.MinimumWidth = ((short)(10));
            this.colField.Tag = null;
            this.colField.Width = ((short)(50));
            // 
            // colFilterType
            // 
            this.colFilterType.AutoSize = true;
            this.colFilterType.DefaultStyle = null;
            resources.ApplyResources(this.colFilterType, "colFilterType");
            this.colFilterType.InternalSort = true;
            this.colFilterType.MaximumWidth = ((short)(0));
            this.colFilterType.MinimumWidth = ((short)(10));
            this.colFilterType.Tag = null;
            this.colFilterType.Width = ((short)(50));
            // 
            // colValue1
            // 
            this.colValue1.AutoSize = true;
            this.colValue1.DefaultStyle = null;
            resources.ApplyResources(this.colValue1, "colValue1");
            this.colValue1.InternalSort = true;
            this.colValue1.MaximumWidth = ((short)(0));
            this.colValue1.MinimumWidth = ((short)(10));
            this.colValue1.Tag = null;
            this.colValue1.Width = ((short)(50));
            // 
            // colValue2
            // 
            this.colValue2.AutoSize = true;
            this.colValue2.DefaultStyle = null;
            resources.ApplyResources(this.colValue2, "colValue2");
            this.colValue2.InternalSort = true;
            this.colValue2.MaximumWidth = ((short)(0));
            this.colValue2.MinimumWidth = ((short)(10));
            this.colValue2.Tag = null;
            this.colValue2.Width = ((short)(50));
            // 
            // colApply
            // 
            this.colApply.AutoSize = true;
            this.colApply.DefaultStyle = null;
            resources.ApplyResources(this.colApply, "colApply");
            this.colApply.InternalSort = true;
            this.colApply.MaximumWidth = ((short)(0));
            this.colApply.MinimumWidth = ((short)(10));
            this.colApply.Tag = null;
            this.colApply.Width = ((short)(50));
            // 
            // SubJobFromTableFiltersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvFilters);
            this.Controls.Add(this.contextButtons);
            this.Name = "SubJobFromTableFiltersPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons contextButtons;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private ListView lvFilters;
        private LSOne.Controls.Columns.Column colField;
        private LSOne.Controls.Columns.Column colFilterType;
        private LSOne.Controls.Columns.Column colValue1;
        private LSOne.Controls.Columns.Column colValue2;
        private LSOne.Controls.Columns.Column colApply;
    }
}
