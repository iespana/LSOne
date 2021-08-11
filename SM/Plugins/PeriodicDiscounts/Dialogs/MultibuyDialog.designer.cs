using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class MultibuyDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultibuyDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbPriority = new LSOne.Controls.NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupPanel2 = new LSOne.Controls.GroupPanel();
            this.btnsDiscountPeriod = new LSOne.Controls.ContextButtons();
            this.tbEndingDate = new System.Windows.Forms.TextBox();
            this.tbStartingDate = new System.Windows.Forms.TextBox();
            this.cmbDiscountPeriodNumber = new LSOne.Controls.DualDataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupPanel3 = new LSOne.Controls.GroupPanel();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.cmbTriggering = new System.Windows.Forms.ComboBox();
            this.lblTriggering = new System.Windows.Forms.Label();
            this.cmbAccountSelection = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAccountCode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddPriceGroup = new LSOne.Controls.ContextButton();
            this.cmbPriceGroup = new LSOne.Controls.DualDataComboBox();
            this.cmbDiscountType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvValues = new LSOne.Controls.ExtendedListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ntbPriority
            // 
            this.ntbPriority.AllowDecimal = false;
            this.ntbPriority.AllowNegative = false;
            this.ntbPriority.CultureInfo = null;
            this.ntbPriority.DecimalLetters = 2;
            this.ntbPriority.ForeColor = System.Drawing.Color.Black;
            this.ntbPriority.HasMinValue = false;
            resources.ApplyResources(this.ntbPriority, "ntbPriority");
            this.ntbPriority.MaxValue = 0D;
            this.ntbPriority.MinValue = 0D;
            this.ntbPriority.Name = "ntbPriority";
            this.ntbPriority.Value = 0D;
            this.ntbPriority.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label8.Name = "label8";
            // 
            // groupPanel2
            // 
            this.groupPanel2.Controls.Add(this.btnsDiscountPeriod);
            this.groupPanel2.Controls.Add(this.tbEndingDate);
            this.groupPanel2.Controls.Add(this.tbStartingDate);
            this.groupPanel2.Controls.Add(this.cmbDiscountPeriodNumber);
            this.groupPanel2.Controls.Add(this.label9);
            this.groupPanel2.Controls.Add(this.label10);
            this.groupPanel2.Controls.Add(this.label12);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // btnsDiscountPeriod
            // 
            this.btnsDiscountPeriod.AddButtonEnabled = true;
            this.btnsDiscountPeriod.BackColor = System.Drawing.Color.Transparent;
            this.btnsDiscountPeriod.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsDiscountPeriod.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsDiscountPeriod, "btnsDiscountPeriod");
            this.btnsDiscountPeriod.Name = "btnsDiscountPeriod";
            this.btnsDiscountPeriod.RemoveButtonEnabled = false;
            this.btnsDiscountPeriod.EditButtonClicked += new System.EventHandler(this.btnsDiscountPeriod_EditButtonClicked);
            this.btnsDiscountPeriod.AddButtonClicked += new System.EventHandler(this.btnAddDiscountPeriod_Click);
            // 
            // tbEndingDate
            // 
            resources.ApplyResources(this.tbEndingDate, "tbEndingDate");
            this.tbEndingDate.Name = "tbEndingDate";
            this.tbEndingDate.TabStop = false;
            // 
            // tbStartingDate
            // 
            resources.ApplyResources(this.tbStartingDate, "tbStartingDate");
            this.tbStartingDate.Name = "tbStartingDate";
            this.tbStartingDate.TabStop = false;
            // 
            // cmbDiscountPeriodNumber
            // 
            this.cmbDiscountPeriodNumber.AddList = null;
            this.cmbDiscountPeriodNumber.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDiscountPeriodNumber, "cmbDiscountPeriodNumber");
            this.cmbDiscountPeriodNumber.MaxLength = 32767;
            this.cmbDiscountPeriodNumber.Name = "cmbDiscountPeriodNumber";
            this.cmbDiscountPeriodNumber.NoChangeAllowed = false;
            this.cmbDiscountPeriodNumber.OnlyDisplayID = false;
            this.cmbDiscountPeriodNumber.RemoveList = null;
            this.cmbDiscountPeriodNumber.RowHeight = ((short)(22));
            this.cmbDiscountPeriodNumber.SecondaryData = null;
            this.cmbDiscountPeriodNumber.SelectedData = null;
            this.cmbDiscountPeriodNumber.SelectedDataID = null;
            this.cmbDiscountPeriodNumber.SelectionList = null;
            this.cmbDiscountPeriodNumber.SkipIDColumn = true;
            this.cmbDiscountPeriodNumber.RequestData += new System.EventHandler(this.cmbDiscountPeriodNumber_RequestData);
            this.cmbDiscountPeriodNumber.SelectedDataChanged += new System.EventHandler(this.cmbDiscountPeriodNumber_SelectedDataChanged);
            this.cmbDiscountPeriodNumber.RequestClear += new System.EventHandler(this.cmbDiscountPeriodNumber_RequestClear);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // groupPanel3
            // 
            this.groupPanel3.Controls.Add(this.lblBarcode);
            this.groupPanel3.Controls.Add(this.tbBarcode);
            this.groupPanel3.Controls.Add(this.cmbTriggering);
            this.groupPanel3.Controls.Add(this.lblTriggering);
            this.groupPanel3.Controls.Add(this.cmbAccountSelection);
            this.groupPanel3.Controls.Add(this.label2);
            this.groupPanel3.Controls.Add(this.cmbAccountCode);
            this.groupPanel3.Controls.Add(this.label5);
            this.groupPanel3.Controls.Add(this.label4);
            this.groupPanel3.Controls.Add(this.btnAddPriceGroup);
            this.groupPanel3.Controls.Add(this.cmbPriceGroup);
            this.groupPanel3.Controls.Add(this.cmbDiscountType);
            this.groupPanel3.Controls.Add(this.ntbPriority);
            this.groupPanel3.Controls.Add(this.label7);
            this.groupPanel3.Controls.Add(this.label6);
            this.groupPanel3.Controls.Add(this.tbDescription);
            this.groupPanel3.Controls.Add(this.label3);
            resources.ApplyResources(this.groupPanel3, "groupPanel3");
            this.groupPanel3.Name = "groupPanel3";
            // 
            // lblBarcode
            // 
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcode, "lblBarcode");
            this.lblBarcode.Name = "lblBarcode";
            // 
            // tbBarcode
            // 
            resources.ApplyResources(this.tbBarcode, "tbBarcode");
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.TextChanged += new System.EventHandler(this.tbBarcode_TextChanged);
            // 
            // cmbTriggering
            // 
            this.cmbTriggering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTriggering.FormattingEnabled = true;
            this.cmbTriggering.Items.AddRange(new object[] {
            resources.GetString("cmbTriggering.Items"),
            resources.GetString("cmbTriggering.Items1")});
            resources.ApplyResources(this.cmbTriggering, "cmbTriggering");
            this.cmbTriggering.Name = "cmbTriggering";
            this.cmbTriggering.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblTriggering
            // 
            this.lblTriggering.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTriggering, "lblTriggering");
            this.lblTriggering.Name = "lblTriggering";
            // 
            // cmbAccountSelection
            // 
            this.cmbAccountSelection.AddList = null;
            this.cmbAccountSelection.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAccountSelection, "cmbAccountSelection");
            this.cmbAccountSelection.EnableTextBox = true;
            this.cmbAccountSelection.MaxLength = 32767;
            this.cmbAccountSelection.Name = "cmbAccountSelection";
            this.cmbAccountSelection.NoChangeAllowed = false;
            this.cmbAccountSelection.OnlyDisplayID = false;
            this.cmbAccountSelection.RemoveList = null;
            this.cmbAccountSelection.RowHeight = ((short)(22));
            this.cmbAccountSelection.SecondaryData = null;
            this.cmbAccountSelection.SelectedData = null;
            this.cmbAccountSelection.SelectedDataID = null;
            this.cmbAccountSelection.SelectionList = null;
            this.cmbAccountSelection.ShowDropDownOnTyping = true;
            this.cmbAccountSelection.SkipIDColumn = true;
            this.cmbAccountSelection.RequestData += new System.EventHandler(this.cmbAccountSelection_RequestData);
            this.cmbAccountSelection.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbAccountSelection_DropDown);
            this.cmbAccountSelection.SelectedDataChanged += new System.EventHandler(this.cmbAccountSelection_SelectedDataChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbAccountCode
            // 
            this.cmbAccountCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountCode.FormattingEnabled = true;
            this.cmbAccountCode.Items.AddRange(new object[] {
            resources.GetString("cmbAccountCode.Items"),
            resources.GetString("cmbAccountCode.Items1"),
            resources.GetString("cmbAccountCode.Items2")});
            resources.ApplyResources(this.cmbAccountCode, "cmbAccountCode");
            this.cmbAccountCode.Name = "cmbAccountCode";
            this.cmbAccountCode.SelectedIndexChanged += new System.EventHandler(this.cmbAccountCode_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnAddPriceGroup
            // 
            this.btnAddPriceGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnAddPriceGroup.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddPriceGroup, "btnAddPriceGroup");
            this.btnAddPriceGroup.Name = "btnAddPriceGroup";
            this.btnAddPriceGroup.Click += new System.EventHandler(this.btnAddPriceGroup_Click);
            // 
            // cmbPriceGroup
            // 
            this.cmbPriceGroup.AddList = null;
            this.cmbPriceGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPriceGroup, "cmbPriceGroup");
            this.cmbPriceGroup.MaxLength = 32767;
            this.cmbPriceGroup.Name = "cmbPriceGroup";
            this.cmbPriceGroup.NoChangeAllowed = false;
            this.cmbPriceGroup.OnlyDisplayID = false;
            this.cmbPriceGroup.RemoveList = null;
            this.cmbPriceGroup.RowHeight = ((short)(22));
            this.cmbPriceGroup.SecondaryData = null;
            this.cmbPriceGroup.SelectedData = null;
            this.cmbPriceGroup.SelectedDataID = null;
            this.cmbPriceGroup.SelectionList = null;
            this.cmbPriceGroup.SkipIDColumn = true;
            this.cmbPriceGroup.RequestData += new System.EventHandler(this.cmbPriceGroup_RequestData);
            this.cmbPriceGroup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbPriceGroup.RequestClear += new System.EventHandler(this.cmbPriceGroup_RequestClear);
            // 
            // cmbDiscountType
            // 
            this.cmbDiscountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiscountType.FormattingEnabled = true;
            this.cmbDiscountType.Items.AddRange(new object[] {
            resources.GetString("cmbDiscountType.Items"),
            resources.GetString("cmbDiscountType.Items1")});
            resources.ApplyResources(this.cmbDiscountType, "cmbDiscountType");
            this.cmbDiscountType.Name = "cmbDiscountType";
            this.cmbDiscountType.SelectedIndexChanged += new System.EventHandler(this.cmbDiscountType_SelectedIndexChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label13.Name = "label13";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.btnsContextButtons);
            this.groupPanel1.Controls.Add(this.lvValues);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = true;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = true;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEditValue_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAddValue_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemoveValue_Click);
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.lvValues.FullRowSelect = true;
            this.lvValues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvValues.HideSelection = false;
            this.lvValues.LockDrawing = false;
            this.lvValues.MultiSelect = false;
            this.lvValues.Name = "lvValues";
            this.lvValues.SortColumn = -1;
            this.lvValues.SortedBackwards = false;
            this.lvValues.UseCompatibleStateImageBehavior = false;
            this.lvValues.UseEveryOtherRowColoring = true;
            this.lvValues.View = System.Windows.Forms.View.Details;
            this.lvValues.SelectedIndexChanged += new System.EventHandler(this.lvValues_SelectedIndexChanged);
            this.lvValues.DoubleClick += new System.EventHandler(this.lvValues_DoubleClick);
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // MultibuyDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "MultibuyDialog";
            this.Load += new System.EventHandler(this.MultibuyDialog_Load);
            this.Controls.SetChildIndex(this.groupPanel3, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.groupPanel2, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbPriority;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private GroupPanel groupPanel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DualDataComboBox cmbDiscountPeriodNumber;
        private System.Windows.Forms.Label label13;
        private GroupPanel groupPanel3;
        private System.Windows.Forms.TextBox tbEndingDate;
        private System.Windows.Forms.TextBox tbStartingDate;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label1;
        private ExtendedListView lvValues;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label label4;
        private ContextButton btnAddPriceGroup;
        private DualDataComboBox cmbPriceGroup;
        private ContextButtons btnsDiscountPeriod;
        private ContextButtons btnsContextButtons;
        private DualDataComboBox cmbAccountSelection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAccountCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ComboBox cmbTriggering;
        private System.Windows.Forms.Label lblTriggering;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox tbBarcode;
    }
}