using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.CustomerLoyalty.ViewPages
{
	partial class CustomerLoyaltyPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLoyaltyPage));
            this.groupPanel1 = new GroupPanel();
            this.laPointStatus = new System.Windows.Forms.Label();
            this.tbPointStatus = new System.Windows.Forms.TextBox();
            this.laExpired = new System.Windows.Forms.Label();
            this.tbExpired = new System.Windows.Forms.TextBox();
            this.laUsed = new System.Windows.Forms.Label();
            this.tbUsed = new System.Windows.Forms.TextBox();
            this.laIssued = new System.Windows.Forms.Label();
            this.tbIssued = new System.Windows.Forms.TextBox();
            this.gridControl1 = new ListView();
            this.columnCardNumber = new Column();
            this.columnType = new Column();
            this.columnScheme = new Column();
            this.columnCurrentValue = new Column();
            this.columnIssued = new Column();
            this.columnUsed = new Column();
            this.columnExpired = new Column();
            this.columnStatus = new Column();
            this.btnsContextButtons = new ContextButtons();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.laPointStatus);
            this.groupPanel1.Controls.Add(this.tbPointStatus);
            this.groupPanel1.Controls.Add(this.laExpired);
            this.groupPanel1.Controls.Add(this.tbExpired);
            this.groupPanel1.Controls.Add(this.laUsed);
            this.groupPanel1.Controls.Add(this.tbUsed);
            this.groupPanel1.Controls.Add(this.laIssued);
            this.groupPanel1.Controls.Add(this.tbIssued);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // laPointStatus
            // 
            this.laPointStatus.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laPointStatus, "laPointStatus");
            this.laPointStatus.Name = "laPointStatus";
            // 
            // tbPointStatus
            // 
            resources.ApplyResources(this.tbPointStatus, "tbPointStatus");
            this.tbPointStatus.Name = "tbPointStatus";
            this.tbPointStatus.ReadOnly = true;
            // 
            // laExpired
            // 
            this.laExpired.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laExpired, "laExpired");
            this.laExpired.Name = "laExpired";
            // 
            // tbExpired
            // 
            resources.ApplyResources(this.tbExpired, "tbExpired");
            this.tbExpired.Name = "tbExpired";
            this.tbExpired.ReadOnly = true;
            // 
            // laUsed
            // 
            this.laUsed.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laUsed, "laUsed");
            this.laUsed.Name = "laUsed";
            // 
            // tbUsed
            // 
            resources.ApplyResources(this.tbUsed, "tbUsed");
            this.tbUsed.Name = "tbUsed";
            this.tbUsed.ReadOnly = true;
            // 
            // laIssued
            // 
            this.laIssued.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laIssued, "laIssued");
            this.laIssued.Name = "laIssued";
            // 
            // tbIssued
            // 
            resources.ApplyResources(this.tbIssued, "tbIssued");
            this.tbIssued.Name = "tbIssued";
            this.tbIssued.ReadOnly = true;
            // 
            // gridControl1
            // 
            resources.ApplyResources(this.gridControl1, "gridControl1");
            this.gridControl1.BackColor = System.Drawing.Color.White;
            this.gridControl1.BuddyControl = null;
            this.gridControl1.Columns.Add(this.columnCardNumber);
            this.gridControl1.Columns.Add(this.columnType);
            this.gridControl1.Columns.Add(this.columnScheme);
            this.gridControl1.Columns.Add(this.columnCurrentValue);
            this.gridControl1.Columns.Add(this.columnIssued);
            this.gridControl1.Columns.Add(this.columnUsed);
            this.gridControl1.Columns.Add(this.columnExpired);
            this.gridControl1.Columns.Add(this.columnStatus);
            this.gridControl1.ContentBackColor = System.Drawing.Color.White;
            this.gridControl1.DefaultRowHeight = ((short)(22));
            this.gridControl1.DimSelectionWhenDisabled = true;
            this.gridControl1.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.gridControl1.ForeColor = System.Drawing.Color.Black;
            this.gridControl1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.gridControl1.HeaderHeight = ((short)(25));
            this.gridControl1.HorizontalScrollbar = true;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.OddRowColor = System.Drawing.Color.Transparent;
            this.gridControl1.RowLineColor = System.Drawing.Color.LightGray;
            this.gridControl1.SelectionModel = ListView.SelectionModelEnum.FullRowMultiSelection;
            this.gridControl1.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.gridControl1.SortSetting = "0:1";
            this.gridControl1.HeaderClicked += new HeaderDelegate(this.gridControl1_HeaderClicked);
            this.gridControl1.SelectionChanged += new System.EventHandler(this.gridControl1_SelectionChanged);
            this.gridControl1.RowDoubleClick += new RowClickDelegate(this.gridControl1_RowDoubleClick);
            // 
            // columnCardNumber
            // 
            this.columnCardNumber.AutoSize = true;
            this.columnCardNumber.Clickable = true;
            this.columnCardNumber.DefaultStyle = null;
            resources.ApplyResources(this.columnCardNumber, "columnCardNumber");
            this.columnCardNumber.MaximumWidth = ((short)(0));
            this.columnCardNumber.MinimumWidth = ((short)(90));
            this.columnCardNumber.Sizable = true;
            this.columnCardNumber.Tag = null;
            this.columnCardNumber.Width = ((short)(100));
            // 
            // columnType
            // 
            this.columnType.AutoSize = true;
            this.columnType.Clickable = true;
            this.columnType.DefaultStyle = null;
            resources.ApplyResources(this.columnType, "columnType");
            this.columnType.MaximumWidth = ((short)(0));
            this.columnType.MinimumWidth = ((short)(60));
            this.columnType.Sizable = true;
            this.columnType.Tag = null;
            this.columnType.Width = ((short)(60));
            // 
            // columnScheme
            // 
            this.columnScheme.AutoSize = true;
            this.columnScheme.Clickable = false;
            this.columnScheme.DefaultStyle = null;
            resources.ApplyResources(this.columnScheme, "columnScheme");
            this.columnScheme.MaximumWidth = ((short)(0));
            this.columnScheme.MinimumWidth = ((short)(60));
            this.columnScheme.Sizable = true;
            this.columnScheme.Tag = null;
            this.columnScheme.Width = ((short)(60));
            // 
            // columnCurrentValue
            // 
            this.columnCurrentValue.AutoSize = true;
            this.columnCurrentValue.Clickable = false;
            this.columnCurrentValue.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnCurrentValue.DefaultStyle = null;
            resources.ApplyResources(this.columnCurrentValue, "columnCurrentValue");
            this.columnCurrentValue.MaximumWidth = ((short)(0));
            this.columnCurrentValue.MinimumWidth = ((short)(10));
            this.columnCurrentValue.Sizable = true;
            this.columnCurrentValue.Tag = null;
            this.columnCurrentValue.Width = ((short)(80));
            // 
            // columnIssued
            // 
            this.columnIssued.AutoSize = true;
            this.columnIssued.Clickable = false;
            this.columnIssued.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnIssued.DefaultStyle = null;
            resources.ApplyResources(this.columnIssued, "columnIssued");
            this.columnIssued.MaximumWidth = ((short)(0));
            this.columnIssued.MinimumWidth = ((short)(10));
            this.columnIssued.Sizable = true;
            this.columnIssued.Tag = null;
            this.columnIssued.Width = ((short)(50));
            // 
            // columnUsed
            // 
            this.columnUsed.AutoSize = true;
            this.columnUsed.Clickable = false;
            this.columnUsed.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnUsed.DefaultStyle = null;
            resources.ApplyResources(this.columnUsed, "columnUsed");
            this.columnUsed.MaximumWidth = ((short)(0));
            this.columnUsed.MinimumWidth = ((short)(10));
            this.columnUsed.Sizable = true;
            this.columnUsed.Tag = null;
            this.columnUsed.Width = ((short)(50));
            // 
            // columnExpired
            // 
            this.columnExpired.AutoSize = true;
            this.columnExpired.Clickable = false;
            this.columnExpired.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnExpired.DefaultStyle = null;
            resources.ApplyResources(this.columnExpired, "columnExpired");
            this.columnExpired.MaximumWidth = ((short)(0));
            this.columnExpired.MinimumWidth = ((short)(10));
            this.columnExpired.Sizable = true;
            this.columnExpired.Tag = null;
            this.columnExpired.Width = ((short)(50));
            // 
            // columnStatus
            // 
            this.columnStatus.AutoSize = true;
            this.columnStatus.Clickable = false;
            this.columnStatus.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnStatus.DefaultStyle = null;
            resources.ApplyResources(this.columnStatus, "columnStatus");
            this.columnStatus.MaximumWidth = ((short)(0));
            this.columnStatus.MinimumWidth = ((short)(10));
            this.columnStatus.Sizable = true;
            this.columnStatus.Tag = null;
            this.columnStatus.Width = ((short)(50));
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAdd;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            // 
            // CustomerLoyaltyPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.groupPanel1);
            this.Name = "CustomerLoyaltyPage";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private GroupPanel groupPanel1;
		private System.Windows.Forms.Label laIssued;
		private System.Windows.Forms.TextBox tbIssued;
		private ListView gridControl1;
		private System.Windows.Forms.Label laPointStatus;
		private System.Windows.Forms.TextBox tbPointStatus;
		private System.Windows.Forms.Label laExpired;
		private System.Windows.Forms.TextBox tbExpired;
		private System.Windows.Forms.Label laUsed;
        private System.Windows.Forms.TextBox tbUsed;
		private Column columnCardNumber;
		private Column columnType;
		private Column columnScheme;
		private Column columnCurrentValue;
		private Column columnIssued;
		private Column columnUsed;
		private Column columnExpired;
		private Column columnStatus;
        private ContextButtons btnsContextButtons;


	}
}
