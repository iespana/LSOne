using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class ContextsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextsView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvContexts = new LSOne.Controls.ListView();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colMenuRequired = new LSOne.Controls.Columns.Column();
            this.colUsedInStyle = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvContexts);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvContexts
            // 
            resources.ApplyResources(this.lvContexts, "lvContexts");
            this.lvContexts.BuddyControl = null;
            this.lvContexts.Columns.Add(this.colDescription);
            this.lvContexts.Columns.Add(this.colMenuRequired);
            this.lvContexts.Columns.Add(this.colUsedInStyle);
            this.lvContexts.ContentBackColor = System.Drawing.Color.White;
            this.lvContexts.DefaultRowHeight = ((short)(22));
            this.lvContexts.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvContexts.HeaderFont = null;
            this.lvContexts.HeaderHeight = ((short)(25));
            this.lvContexts.Name = "lvContexts";
            this.lvContexts.OddRowColor = System.Drawing.Color.White;
            this.lvContexts.RowLineColor = System.Drawing.Color.LightGray;
            this.lvContexts.SecondarySortColumn = ((short)(-1));
            this.lvContexts.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvContexts.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvContexts.SortSetting = "-1:1";
            this.lvContexts.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvContexts_HeaderClicked);
            this.lvContexts.RowClick += new LSOne.Controls.RowClickDelegate(this.lvContexts_RowClick);
            this.lvContexts.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvContexts_RowDoubleClick);
            // 
            // colDescription
            // 
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(250));
            // 
            // colMenuRequired
            // 
            this.colMenuRequired.DefaultStyle = null;
            resources.ApplyResources(this.colMenuRequired, "colMenuRequired");
            this.colMenuRequired.MaximumWidth = ((short)(0));
            this.colMenuRequired.MinimumWidth = ((short)(10));
            this.colMenuRequired.Tag = null;
            this.colMenuRequired.Width = ((short)(100));
            // 
            // colUsedInStyle
            // 
            this.colUsedInStyle.DefaultStyle = null;
            resources.ApplyResources(this.colUsedInStyle, "colUsedInStyle");
            this.colUsedInStyle.MaximumWidth = ((short)(0));
            this.colUsedInStyle.MinimumWidth = ((short)(10));
            this.colUsedInStyle.Tag = null;
            this.colUsedInStyle.Width = ((short)(100));
            // 
            // ContextsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ContextsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddRemove;
        private ListView lvContexts;
        private Column colDescription;
        private Column colMenuRequired;
        private Column colUsedInStyle;
    }
}
