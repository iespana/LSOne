using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Administration.Views
{
    partial class AuditLogViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditLogViewer));
            this.lblUser = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tbUser = new System.Windows.Forms.TextBox();
            this.lvAuditLogs = new ListView();
            this.column1 = new Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvAuditLogs);
            this.pnlBottom.Controls.Add(this.tbUser);
            this.pnlBottom.Controls.Add(this.lblTo);
            this.pnlBottom.Controls.Add(this.dtpTo);
            this.pnlBottom.Controls.Add(this.dtpFrom);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.lblUser);
            // 
            // lblUser
            // 
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.Name = "lblUser";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Checked = false;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFrom, "dtpFrom");
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.Checked = false;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tbUser
            // 
            resources.ApplyResources(this.tbUser, "tbUser");
            this.tbUser.Name = "tbUser";
            this.tbUser.TextChanged += new System.EventHandler(this.tbUser_TextChanged);
            // 
            // lvAuditLogs
            // 
            resources.ApplyResources(this.lvAuditLogs, "lvAuditLogs");
            this.lvAuditLogs.BuddyControl = null;
            this.lvAuditLogs.Columns.Add(this.column1);
            this.lvAuditLogs.ContentBackColor = System.Drawing.Color.White;
            this.lvAuditLogs.DefaultRowHeight = ((short)(22));
            this.lvAuditLogs.DimSelectionWhenDisabled = true;
            this.lvAuditLogs.EvenRowColor = System.Drawing.Color.White;
            this.lvAuditLogs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAuditLogs.HeaderHeight = ((short)(1));
            this.lvAuditLogs.HorizontalScrollbar = true;
            this.lvAuditLogs.Name = "lvAuditLogs";
            this.lvAuditLogs.OddRowColor = System.Drawing.Color.White;
            this.lvAuditLogs.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAuditLogs.RowLines = true;
            this.lvAuditLogs.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvAuditLogs.RowDoubleClick += new RowClickDelegate(this.lvAuditLogs_RowDoubleClick);
            this.lvAuditLogs.RowExpanded += new RowClickDelegate(this.lvAuditLogs_RowExpanded);
            this.lvAuditLogs.RowCollapsed += new RowClickDelegate(this.lvAuditLogs_RowCollapsed);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // AuditLogViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 70;
            this.Name = "AuditLogViewer";
            this.Load += new System.EventHandler(this.AuditLogViewer_Load);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox tbUser;
        private ListView lvAuditLogs;
        private Column column1;
    }
}
