using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class TableDesignPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableDesignPage));
            this.grpHeader = new LSOne.Controls.GroupPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.tbTableName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lvFields = new LSOne.Controls.ListView();
            this.colName = new LSOne.Controls.Columns.Column();
            this.colDataType = new LSOne.Controls.Columns.Column();
            this.colLength = new LSOne.Controls.Columns.Column();
            this.colEnabled = new LSOne.Controls.Columns.Column();
            this.grpHeader.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHeader
            // 
            this.grpHeader.Controls.Add(this.lblTitle);
            resources.ApplyResources(this.grpHeader, "grpHeader");
            this.grpHeader.Name = "grpHeader";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Name = "lblTitle";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.BackColor = System.Drawing.Color.Transparent;
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = false;
            // 
            // tbTableName
            // 
            resources.ApplyResources(this.tbTableName, "tbTableName");
            this.tbTableName.Name = "tbTableName";
            this.tbTableName.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.tbTableName);
            this.pnlTop.Controls.Add(this.chkEnabled);
            this.pnlTop.Controls.Add(this.grpHeader);
            this.pnlTop.Controls.Add(this.label1);
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.Name = "pnlTop";
            // 
            // lvFields
            // 
            resources.ApplyResources(this.lvFields, "lvFields");
            this.lvFields.BuddyControl = null;
            this.lvFields.Columns.Add(this.colName);
            this.lvFields.Columns.Add(this.colDataType);
            this.lvFields.Columns.Add(this.colLength);
            this.lvFields.Columns.Add(this.colEnabled);
            this.lvFields.ContentBackColor = System.Drawing.Color.White;
            this.lvFields.DefaultRowHeight = ((short)(22));
            this.lvFields.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvFields.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFields.HeaderHeight = ((short)(25));
            this.lvFields.Name = "lvFields";
            this.lvFields.OddRowColor = System.Drawing.Color.White;
            this.lvFields.RowLineColor = System.Drawing.Color.LightGray;
            this.lvFields.SecondarySortColumn = ((short)(-1));
            this.lvFields.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvFields.SortSetting = "0:1";
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.InternalSort = true;
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.SecondarySortColumn = ((short)(-1));
            this.colName.Sizable = false;
            this.colName.Tag = null;
            this.colName.Width = ((short)(50));
            // 
            // colDataType
            // 
            this.colDataType.AutoSize = true;
            this.colDataType.DefaultStyle = null;
            resources.ApplyResources(this.colDataType, "colDataType");
            this.colDataType.InternalSort = true;
            this.colDataType.MaximumWidth = ((short)(0));
            this.colDataType.MinimumWidth = ((short)(10));
            this.colDataType.SecondarySortColumn = ((short)(-1));
            this.colDataType.Sizable = false;
            this.colDataType.Tag = null;
            this.colDataType.Width = ((short)(50));
            // 
            // colLength
            // 
            this.colLength.AutoSize = true;
            this.colLength.DefaultStyle = null;
            resources.ApplyResources(this.colLength, "colLength");
            this.colLength.InternalSort = true;
            this.colLength.MaximumWidth = ((short)(0));
            this.colLength.MinimumWidth = ((short)(10));
            this.colLength.SecondarySortColumn = ((short)(-1));
            this.colLength.Sizable = false;
            this.colLength.Tag = null;
            this.colLength.Width = ((short)(50));
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
            this.colEnabled.Sizable = false;
            this.colEnabled.Tag = null;
            this.colEnabled.Width = ((short)(50));
            // 
            // TableDesignPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvFields);
            this.Controls.Add(this.pnlTop);
            this.Name = "TableDesignPage";
            this.grpHeader.ResumeLayout(false);
            this.grpHeader.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel grpHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.TextBox tbTableName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlTop;
        private ListView lvFields;
        private LSOne.Controls.Columns.Column colName;
        private LSOne.Controls.Columns.Column colDataType;
        private LSOne.Controls.Columns.Column colLength;
        private LSOne.Controls.Columns.Column colEnabled;
    }
}
