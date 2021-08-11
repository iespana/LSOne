using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    partial class RetailItemReplenishmentPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailItemReplenishmentPage));
            this.groupPanel3 = new LSOne.Controls.GroupPanel();
            this.linkFields2 = new LSOne.Controls.LinkFields();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.lblMaximumInventoryUnit = new System.Windows.Forms.Label();
            this.lblReorderPointUnit = new System.Windows.Forms.Label();
            this.cmbReplenishmentMethod = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpBlockedDate = new LSOne.Controls.MultiDateControl();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBlocked = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbPurchaseOrderMultiple = new LSOne.Controls.NumericTextBox();
            this.cmbPurchaseOrderMultipleRounding = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ntbMaximumInventory = new LSOne.Controls.NumericTextBox();
            this.ntbReorderPoint = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblReorderPoint = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lvStoreSettings = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.label4 = new System.Windows.Forms.Label();
            this.groupPanel3.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel3
            // 
            resources.ApplyResources(this.groupPanel3, "groupPanel3");
            this.groupPanel3.Controls.Add(this.linkFields2);
            this.groupPanel3.Controls.Add(this.linkFields1);
            this.groupPanel3.Controls.Add(this.lblMaximumInventoryUnit);
            this.groupPanel3.Controls.Add(this.lblReorderPointUnit);
            this.groupPanel3.Controls.Add(this.cmbReplenishmentMethod);
            this.groupPanel3.Controls.Add(this.label5);
            this.groupPanel3.Controls.Add(this.dtpBlockedDate);
            this.groupPanel3.Controls.Add(this.label2);
            this.groupPanel3.Controls.Add(this.cmbBlocked);
            this.groupPanel3.Controls.Add(this.label1);
            this.groupPanel3.Controls.Add(this.ntbPurchaseOrderMultiple);
            this.groupPanel3.Controls.Add(this.cmbPurchaseOrderMultipleRounding);
            this.groupPanel3.Controls.Add(this.label9);
            this.groupPanel3.Controls.Add(this.ntbMaximumInventory);
            this.groupPanel3.Controls.Add(this.ntbReorderPoint);
            this.groupPanel3.Controls.Add(this.label6);
            this.groupPanel3.Controls.Add(this.lblReorderPoint);
            this.groupPanel3.Name = "groupPanel3";
            // 
            // linkFields2
            // 
            this.linkFields2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields2, "linkFields2");
            this.linkFields2.Name = "linkFields2";
            this.linkFields2.TabStop = false;
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Mode = LSOne.Controls.LinkFields.ModeEnum.Tripple;
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // lblMaximumInventoryUnit
            // 
            this.lblMaximumInventoryUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaximumInventoryUnit, "lblMaximumInventoryUnit");
            this.lblMaximumInventoryUnit.Name = "lblMaximumInventoryUnit";
            // 
            // lblReorderPointUnit
            // 
            this.lblReorderPointUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReorderPointUnit, "lblReorderPointUnit");
            this.lblReorderPointUnit.Name = "lblReorderPointUnit";
            // 
            // cmbReplenishmentMethod
            // 
            this.cmbReplenishmentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReplenishmentMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbReplenishmentMethod, "cmbReplenishmentMethod");
            this.cmbReplenishmentMethod.Name = "cmbReplenishmentMethod";
            this.cmbReplenishmentMethod.SelectedIndexChanged += new System.EventHandler(this.cmbReplenishmentMethod_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtpBlockedDate
            // 
            resources.ApplyResources(this.dtpBlockedDate, "dtpBlockedDate");
            this.dtpBlockedDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpBlockedDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpBlockedDate.Name = "dtpBlockedDate";
            this.dtpBlockedDate.Value = new System.DateTime(2016, 3, 14, 15, 28, 27, 104);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbBlocked
            // 
            this.cmbBlocked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlocked.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBlocked, "cmbBlocked");
            this.cmbBlocked.Name = "cmbBlocked";
            this.cmbBlocked.SelectedIndexChanged += new System.EventHandler(this.cmbBlocked_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbPurchaseOrderMultiple
            // 
            this.ntbPurchaseOrderMultiple.AllowDecimal = false;
            this.ntbPurchaseOrderMultiple.AllowNegative = false;
            this.ntbPurchaseOrderMultiple.CultureInfo = null;
            this.ntbPurchaseOrderMultiple.DecimalLetters = 2;
            this.ntbPurchaseOrderMultiple.ForeColor = System.Drawing.Color.Black;
            this.ntbPurchaseOrderMultiple.HasMinValue = false;
            resources.ApplyResources(this.ntbPurchaseOrderMultiple, "ntbPurchaseOrderMultiple");
            this.ntbPurchaseOrderMultiple.MaxValue = 99999999D;
            this.ntbPurchaseOrderMultiple.MinValue = 0D;
            this.ntbPurchaseOrderMultiple.Name = "ntbPurchaseOrderMultiple";
            this.ntbPurchaseOrderMultiple.Value = 0D;
            // 
            // cmbPurchaseOrderMultipleRounding
            // 
            this.cmbPurchaseOrderMultipleRounding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchaseOrderMultipleRounding.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPurchaseOrderMultipleRounding, "cmbPurchaseOrderMultipleRounding");
            this.cmbPurchaseOrderMultipleRounding.Name = "cmbPurchaseOrderMultipleRounding";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // ntbMaximumInventory
            // 
            this.ntbMaximumInventory.AllowDecimal = true;
            this.ntbMaximumInventory.AllowNegative = false;
            this.ntbMaximumInventory.CultureInfo = null;
            this.ntbMaximumInventory.DecimalLetters = 2;
            this.ntbMaximumInventory.ForeColor = System.Drawing.Color.Black;
            this.ntbMaximumInventory.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumInventory, "ntbMaximumInventory");
            this.ntbMaximumInventory.MaxValue = 999999999D;
            this.ntbMaximumInventory.MinValue = 0D;
            this.ntbMaximumInventory.Name = "ntbMaximumInventory";
            this.ntbMaximumInventory.Value = 0D;
            this.ntbMaximumInventory.TextChanged += new System.EventHandler(this.ntbMaximumInventory_TextChanged);
            // 
            // ntbReorderPoint
            // 
            this.ntbReorderPoint.AllowDecimal = true;
            this.ntbReorderPoint.AllowNegative = false;
            this.ntbReorderPoint.CultureInfo = null;
            this.ntbReorderPoint.DecimalLetters = 2;
            this.ntbReorderPoint.ForeColor = System.Drawing.Color.Black;
            this.ntbReorderPoint.HasMinValue = false;
            resources.ApplyResources(this.ntbReorderPoint, "ntbReorderPoint");
            this.ntbReorderPoint.MaxValue = 999999999D;
            this.ntbReorderPoint.MinValue = 0D;
            this.ntbReorderPoint.Name = "ntbReorderPoint";
            this.ntbReorderPoint.Value = 0D;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblReorderPoint
            // 
            this.lblReorderPoint.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReorderPoint, "lblReorderPoint");
            this.lblReorderPoint.Name = "lblReorderPoint";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label13.Name = "label13";
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
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lvStoreSettings);
            this.groupPanel1.Controls.Add(this.btnsEditAddRemove);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvStoreSettings
            // 
            resources.ApplyResources(this.lvStoreSettings, "lvStoreSettings");
            this.lvStoreSettings.BuddyControl = null;
            this.lvStoreSettings.Columns.Add(this.column1);
            this.lvStoreSettings.Columns.Add(this.column2);
            this.lvStoreSettings.Columns.Add(this.column3);
            this.lvStoreSettings.Columns.Add(this.column4);
            this.lvStoreSettings.Columns.Add(this.column5);
            this.lvStoreSettings.Columns.Add(this.column6);
            this.lvStoreSettings.Columns.Add(this.column7);
            this.lvStoreSettings.Columns.Add(this.column8);
            this.lvStoreSettings.ContentBackColor = System.Drawing.Color.White;
            this.lvStoreSettings.DefaultRowHeight = ((short)(22));
            this.lvStoreSettings.DimSelectionWhenDisabled = true;
            this.lvStoreSettings.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvStoreSettings.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvStoreSettings.HeaderHeight = ((short)(25));
            this.lvStoreSettings.HorizontalScrollbar = true;
            this.lvStoreSettings.Name = "lvStoreSettings";
            this.lvStoreSettings.OddRowColor = System.Drawing.Color.White;
            this.lvStoreSettings.RowLineColor = System.Drawing.Color.LightGray;
            this.lvStoreSettings.SecondarySortColumn = ((short)(-1));
            this.lvStoreSettings.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvStoreSettings.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvStoreSettings.SortSetting = "0:1";
            this.lvStoreSettings.SelectionChanged += new System.EventHandler(this.lvStoreSettings_SelectionChanged);
            this.lvStoreSettings.CellAction += new LSOne.Controls.CellActionDelegate(this.lvStoreSettings_CellAction);
            this.lvStoreSettings.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvStoreSettings_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(15));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.InternalSort = true;
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.InternalSort = true;
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label4.Name = "label4";
            // 
            // RetailItemReplenishmentPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupPanel3);
            this.DoubleBuffered = true;
            this.Name = "RetailItemReplenishmentPage";
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel3;
        private NumericTextBox ntbMaximumInventory;
        private NumericTextBox ntbReorderPoint;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblReorderPoint;
        private System.Windows.Forms.Label label13;
        private NumericTextBox ntbPurchaseOrderMultiple;
        private System.Windows.Forms.ComboBox cmbPurchaseOrderMultipleRounding;
        private System.Windows.Forms.Label label9;
        private MultiDateControl dtpBlockedDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBlocked;
        private System.Windows.Forms.Label label1;
        private ContextButtons btnsEditAddRemove;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbReplenishmentMethod;
        private System.Windows.Forms.Label label5;
        private ListView lvStoreSettings;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private Column column5;
        private Column column6;
        private Column column7;
        private Column column8;
        private System.Windows.Forms.Label lblMaximumInventoryUnit;
        private System.Windows.Forms.Label lblReorderPointUnit;
        private LinkFields linkFields1;
        private LinkFields linkFields2;
    }
}
