using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.GiftCards.Views
{
    partial class GiftCardView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GiftCardView));
			this.lvUsageLog = new LSOne.Controls.ExtendedListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ntbBallance = new LSOne.Controls.NumericTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.chkActive = new System.Windows.Forms.CheckBox();
			this.tbCurrency = new System.Windows.Forms.TextBox();
			this.groupPanel1 = new LSOne.Controls.GroupPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.chkRefillable = new System.Windows.Forms.CheckBox();
			this.btnViewUser = new System.Windows.Forms.Button();
			this.btnViewTerminal = new System.Windows.Forms.Button();
			this.btnViewStore = new System.Windows.Forms.Button();
			this.pnlBottom.SuspendLayout();
			this.groupPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.btnViewUser);
			this.pnlBottom.Controls.Add(this.btnViewTerminal);
			this.pnlBottom.Controls.Add(this.btnViewStore);
			this.pnlBottom.Controls.Add(this.chkRefillable);
			this.pnlBottom.Controls.Add(this.label6);
			this.pnlBottom.Controls.Add(this.groupPanel1);
			this.pnlBottom.Controls.Add(this.tbCurrency);
			this.pnlBottom.Controls.Add(this.chkActive);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.ntbBallance);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
			// 
			// lvUsageLog
			// 
			resources.ApplyResources(this.lvUsageLog, "lvUsageLog");
			this.lvUsageLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
			this.lvUsageLog.FullRowSelect = true;
			this.lvUsageLog.HideSelection = false;
			this.lvUsageLog.LockDrawing = false;
			this.lvUsageLog.MultiSelect = false;
			this.lvUsageLog.Name = "lvUsageLog";
			this.lvUsageLog.SortColumn = 4;
			this.lvUsageLog.SortedBackwards = false;
			this.lvUsageLog.UseCompatibleStateImageBehavior = false;
			this.lvUsageLog.UseEveryOtherRowColoring = true;
			this.lvUsageLog.View = System.Windows.Forms.View.Details;
			this.lvUsageLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsageLog_ColumnClick);
			this.lvUsageLog.SelectedIndexChanged += new System.EventHandler(this.lvUsageLog_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			resources.ApplyResources(this.columnHeader1, "columnHeader1");
			// 
			// columnHeader2
			// 
			resources.ApplyResources(this.columnHeader2, "columnHeader2");
			// 
			// columnHeader3
			// 
			resources.ApplyResources(this.columnHeader3, "columnHeader3");
			// 
			// columnHeader4
			// 
			resources.ApplyResources(this.columnHeader4, "columnHeader4");
			// 
			// columnHeader5
			// 
			resources.ApplyResources(this.columnHeader5, "columnHeader5");
			// 
			// columnHeader6
			// 
			resources.ApplyResources(this.columnHeader6, "columnHeader6");
			// 
			// columnHeader7
			// 
			resources.ApplyResources(this.columnHeader7, "columnHeader7");
			// 
			// tbID
			// 
			this.tbID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// ntbBallance
			// 
			this.ntbBallance.AllowDecimal = true;
			this.ntbBallance.AllowNegative = false;
			this.ntbBallance.CultureInfo = null;
			this.ntbBallance.DecimalLetters = 2;
			resources.ApplyResources(this.ntbBallance, "ntbBallance");
			this.ntbBallance.ForeColor = System.Drawing.Color.Black;
			this.ntbBallance.HasMinValue = false;
			this.ntbBallance.MaxValue = 0D;
			this.ntbBallance.MinValue = 0D;
			this.ntbBallance.Name = "ntbBallance";
			this.ntbBallance.Value = 0D;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// chkActive
			// 
			resources.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.Name = "chkActive";
			this.chkActive.UseVisualStyleBackColor = true;
			// 
			// tbCurrency
			// 
			resources.ApplyResources(this.tbCurrency, "tbCurrency");
			this.tbCurrency.Name = "tbCurrency";
			// 
			// groupPanel1
			// 
			resources.ApplyResources(this.groupPanel1, "groupPanel1");
			this.groupPanel1.Controls.Add(this.label3);
			this.groupPanel1.Controls.Add(this.lvUsageLog);
			this.groupPanel1.Name = "groupPanel1";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// chkRefillable
			// 
			resources.ApplyResources(this.chkRefillable, "chkRefillable");
			this.chkRefillable.Name = "chkRefillable";
			this.chkRefillable.UseVisualStyleBackColor = true;
			// 
			// btnViewUser
			// 
			resources.ApplyResources(this.btnViewUser, "btnViewUser");
			this.btnViewUser.Name = "btnViewUser";
			this.btnViewUser.UseVisualStyleBackColor = true;
			this.btnViewUser.Click += new System.EventHandler(this.ViewUser);
			// 
			// btnViewTerminal
			// 
			resources.ApplyResources(this.btnViewTerminal, "btnViewTerminal");
			this.btnViewTerminal.Name = "btnViewTerminal";
			this.btnViewTerminal.UseVisualStyleBackColor = true;
			this.btnViewTerminal.Click += new System.EventHandler(this.ViewTerminal);
			// 
			// btnViewStore
			// 
			resources.ApplyResources(this.btnViewStore, "btnViewStore");
			this.btnViewStore.Name = "btnViewStore";
			this.btnViewStore.UseVisualStyleBackColor = true;
			this.btnViewStore.Click += new System.EventHandler(this.ViewStore);
			// 
			// GiftCardView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 50;
			this.Name = "GiftCardView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.groupPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvUsageLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbBallance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCurrency;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRefillable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnViewUser;
        private System.Windows.Forms.Button btnViewTerminal;
        private System.Windows.Forms.Button btnViewStore;
    }
}
