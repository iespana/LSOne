using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    partial class DiningTableLayoutView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiningTableLayoutView));
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbLayoutID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ntbNoOfScreens = new LSOne.Controls.NumericTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.ntbNoOfDiningTables = new LSOne.Controls.NumericTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ntbStartnigTableNo = new LSOne.Controls.NumericTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ntbEndingTableNo = new LSOne.Controls.NumericTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ntbDiningTableColumns = new LSOne.Controls.NumericTextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.ntbDiningTableRows = new LSOne.Controls.NumericTextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.groupPanel1 = new LSOne.Controls.GroupPanel();
			this.btnsEditAddRemoveDiningTables = new LSOne.Controls.ContextButtons();
			this.lvRestaurantDiningTables = new LSOne.Controls.ExtendedListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblGroupHeader = new System.Windows.Forms.Label();
			this.pnlBottom.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.groupPanel1);
			this.pnlBottom.Controls.Add(this.groupBox1);
			this.pnlBottom.Controls.Add(this.ntbEndingTableNo);
			this.pnlBottom.Controls.Add(this.label6);
			this.pnlBottom.Controls.Add(this.ntbStartnigTableNo);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.ntbNoOfDiningTables);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbLayoutID);
			this.pnlBottom.Controls.Add(this.ntbNoOfScreens);
			this.pnlBottom.Controls.Add(this.label2);
			this.pnlBottom.Controls.Add(this.label4);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbLayoutID
			// 
			this.tbLayoutID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbLayoutID, "tbLayoutID");
			this.tbLayoutID.Name = "tbLayoutID";
			this.tbLayoutID.ReadOnly = true;
			this.tbLayoutID.BackColor = ColorPalette.BackgroundColor;
			this.tbLayoutID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// ntbNoOfScreens
			// 
			this.ntbNoOfScreens.AllowDecimal = false;
			this.ntbNoOfScreens.AllowNegative = false;
			this.ntbNoOfScreens.CultureInfo = null;
			this.ntbNoOfScreens.DecimalLetters = 2;
			resources.ApplyResources(this.ntbNoOfScreens, "ntbNoOfScreens");
			this.ntbNoOfScreens.ForeColor = System.Drawing.Color.Black;
			this.ntbNoOfScreens.HasMinValue = false;
			this.ntbNoOfScreens.MaxValue = 0D;
			this.ntbNoOfScreens.MinValue = 0D;
			this.ntbNoOfScreens.Name = "ntbNoOfScreens";
			this.ntbNoOfScreens.ReadOnly = true;
			this.ntbNoOfScreens.Value = 0D;
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// ntbNoOfDiningTables
			// 
			this.ntbNoOfDiningTables.AllowDecimal = false;
			this.ntbNoOfDiningTables.AllowNegative = false;
			this.ntbNoOfDiningTables.CultureInfo = null;
			this.ntbNoOfDiningTables.DecimalLetters = 2;
			this.ntbNoOfDiningTables.ForeColor = System.Drawing.Color.Black;
			this.ntbNoOfDiningTables.HasMinValue = false;
			resources.ApplyResources(this.ntbNoOfDiningTables, "ntbNoOfDiningTables");
			this.ntbNoOfDiningTables.MaxValue = 0D;
			this.ntbNoOfDiningTables.MinValue = 0D;
			this.ntbNoOfDiningTables.Name = "ntbNoOfDiningTables";
			this.ntbNoOfDiningTables.ReadOnly = true;
			this.ntbNoOfDiningTables.Value = 0D;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// ntbStartnigTableNo
			// 
			this.ntbStartnigTableNo.AllowDecimal = false;
			this.ntbStartnigTableNo.AllowNegative = false;
			this.ntbStartnigTableNo.CultureInfo = null;
			this.ntbStartnigTableNo.DecimalLetters = 2;
			this.ntbStartnigTableNo.ForeColor = System.Drawing.Color.Black;
			this.ntbStartnigTableNo.HasMinValue = false;
			resources.ApplyResources(this.ntbStartnigTableNo, "ntbStartnigTableNo");
			this.ntbStartnigTableNo.MaxValue = 0D;
			this.ntbStartnigTableNo.MinValue = 0D;
			this.ntbStartnigTableNo.Name = "ntbStartnigTableNo";
			this.ntbStartnigTableNo.ReadOnly = true;
			this.ntbStartnigTableNo.Value = 0D;
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// ntbEndingTableNo
			// 
			this.ntbEndingTableNo.AllowDecimal = false;
			this.ntbEndingTableNo.AllowNegative = false;
			this.ntbEndingTableNo.CultureInfo = null;
			this.ntbEndingTableNo.DecimalLetters = 2;
			this.ntbEndingTableNo.ForeColor = System.Drawing.Color.Black;
			this.ntbEndingTableNo.HasMinValue = false;
			resources.ApplyResources(this.ntbEndingTableNo, "ntbEndingTableNo");
			this.ntbEndingTableNo.MaxValue = 0D;
			this.ntbEndingTableNo.MinValue = 0D;
			this.ntbEndingTableNo.Name = "ntbEndingTableNo";
			this.ntbEndingTableNo.ReadOnly = true;
			this.ntbEndingTableNo.Value = 0D;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ntbDiningTableColumns);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.ntbDiningTableRows);
			this.groupBox1.Controls.Add(this.label7);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// ntbDiningTableColumns
			// 
			this.ntbDiningTableColumns.AllowDecimal = false;
			this.ntbDiningTableColumns.AllowNegative = false;
			this.ntbDiningTableColumns.CultureInfo = null;
			this.ntbDiningTableColumns.DecimalLetters = 2;
			this.ntbDiningTableColumns.ForeColor = System.Drawing.Color.Black;
			this.ntbDiningTableColumns.HasMinValue = false;
			resources.ApplyResources(this.ntbDiningTableColumns, "ntbDiningTableColumns");
			this.ntbDiningTableColumns.MaxValue = 0D;
			this.ntbDiningTableColumns.MinValue = 0D;
			this.ntbDiningTableColumns.Name = "ntbDiningTableColumns";
			this.ntbDiningTableColumns.Value = 0D;
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// ntbDiningTableRows
			// 
			this.ntbDiningTableRows.AllowDecimal = false;
			this.ntbDiningTableRows.AllowNegative = false;
			this.ntbDiningTableRows.CultureInfo = null;
			this.ntbDiningTableRows.DecimalLetters = 2;
			this.ntbDiningTableRows.ForeColor = System.Drawing.Color.Black;
			this.ntbDiningTableRows.HasMinValue = false;
			resources.ApplyResources(this.ntbDiningTableRows, "ntbDiningTableRows");
			this.ntbDiningTableRows.MaxValue = 0D;
			this.ntbDiningTableRows.MinValue = 0D;
			this.ntbDiningTableRows.Name = "ntbDiningTableRows";
			this.ntbDiningTableRows.Value = 0D;
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// groupPanel1
			// 
			resources.ApplyResources(this.groupPanel1, "groupPanel1");
			this.groupPanel1.Controls.Add(this.btnsEditAddRemoveDiningTables);
			this.groupPanel1.Controls.Add(this.lvRestaurantDiningTables);
			this.groupPanel1.Controls.Add(this.lblGroupHeader);
			this.groupPanel1.Name = "groupPanel1";
			// 
			// btnsEditAddRemoveDiningTables
			// 
			this.btnsEditAddRemoveDiningTables.AddButtonEnabled = true;
			resources.ApplyResources(this.btnsEditAddRemoveDiningTables, "btnsEditAddRemoveDiningTables");
			this.btnsEditAddRemoveDiningTables.BackColor = System.Drawing.Color.Transparent;
			this.btnsEditAddRemoveDiningTables.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
			this.btnsEditAddRemoveDiningTables.EditButtonEnabled = true;
			this.btnsEditAddRemoveDiningTables.Name = "btnsEditAddRemoveDiningTables";
			this.btnsEditAddRemoveDiningTables.RemoveButtonEnabled = true;
			this.btnsEditAddRemoveDiningTables.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDiningTables_EditButtonClicked);
			this.btnsEditAddRemoveDiningTables.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDiningTables_AddButtonClicked);
			this.btnsEditAddRemoveDiningTables.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDiningTables_RemoveButtonClicked);
			// 
			// lvRestaurantDiningTables
			// 
			resources.ApplyResources(this.lvRestaurantDiningTables, "lvRestaurantDiningTables");
			this.lvRestaurantDiningTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.lvRestaurantDiningTables.FullRowSelect = true;
			this.lvRestaurantDiningTables.HideSelection = false;
			this.lvRestaurantDiningTables.LockDrawing = false;
			this.lvRestaurantDiningTables.MultiSelect = false;
			this.lvRestaurantDiningTables.Name = "lvRestaurantDiningTables";
			this.lvRestaurantDiningTables.SortColumn = -1;
			this.lvRestaurantDiningTables.SortedBackwards = false;
			this.lvRestaurantDiningTables.UseCompatibleStateImageBehavior = false;
			this.lvRestaurantDiningTables.UseEveryOtherRowColoring = true;
			this.lvRestaurantDiningTables.View = System.Windows.Forms.View.Details;
			this.lvRestaurantDiningTables.SelectedIndexChanged += new System.EventHandler(this.lvRestaurantDiningTables_SelectedIndexChanged);
			this.lvRestaurantDiningTables.DoubleClick += new System.EventHandler(this.lvRestaurantDiningTables_DoubleClick);
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
			// lblGroupHeader
			// 
			resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
			this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
			this.lblGroupHeader.Name = "lblGroupHeader";
			// 
			// DiningTableLayoutView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 70;
			this.Name = "DiningTableLayoutView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupPanel1.ResumeLayout(false);
			this.groupPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLayoutID;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbNoOfScreens;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbNoOfDiningTables;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbEndingTableNo;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbStartnigTableNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private NumericTextBox ntbDiningTableColumns;
        private System.Windows.Forms.Label label8;
        private NumericTextBox ntbDiningTableRows;
        private System.Windows.Forms.Label label7;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private ExtendedListView lvRestaurantDiningTables;
        private ContextButtons btnsEditAddRemoveDiningTables;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}
