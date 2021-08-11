using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemPOSSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemPOSSettingsPage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new LSOne.Controls.DoubleBufferGroupBox();
            this.dtpDateToBeBlocked = new LSOne.Controls.MultiDateControl();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new LSOne.Controls.DoubleBufferGroupBox();
            this.dtpIssueDate = new LSOne.Controls.MultiDateControl();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new LSOne.Controls.DoubleBufferGroupBox();
            this.chkCanBeSold = new LSOne.Controls.DoubleBufferedCheckbox();
            this.lblCanBeSold = new System.Windows.Forms.Label();
            this.ntbTareWeight = new LSOne.Controls.NumericTextBox();
            this.lblTareWeight = new System.Windows.Forms.Label();
            this.cmbKeyingInSerialNumber = new System.Windows.Forms.ComboBox();
            this.lblKeyInSerialNumber = new System.Windows.Forms.Label();
            this.chkReturnable = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkMustSelectUOM = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnEditValidationPeriod = new LSOne.Controls.ContextButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbValPeriod = new LSOne.Controls.DualDataComboBox();
            this.cmbKeyingInQuantity = new System.Windows.Forms.ComboBox();
            this.chkScaleItem = new LSOne.Controls.DoubleBufferedCheckbox();
            this.cmbKeyingInPrice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkMustKeyInComment = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkZeroPriceValid = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkQtBecomesNegative = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkNoDiscount = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.dtpDateToBeBlocked);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // dtpDateToBeBlocked
            // 
            this.dtpDateToBeBlocked.Checked = false;
            this.dtpDateToBeBlocked.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDateToBeBlocked, "dtpDateToBeBlocked");
            this.dtpDateToBeBlocked.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDateToBeBlocked.MinDate = new System.DateTime(1900, 1, 2, 0, 0, 0, 0);
            this.dtpDateToBeBlocked.Name = "dtpDateToBeBlocked";
            this.dtpDateToBeBlocked.ShowCheckBox = true;
            this.dtpDateToBeBlocked.Value = new System.DateTime(2015, 11, 17, 15, 9, 27, 849);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.dtpIssueDate);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // dtpIssueDate
            // 
            this.dtpIssueDate.Checked = false;
            this.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpIssueDate, "dtpIssueDate");
            this.dtpIssueDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpIssueDate.MinDate = new System.DateTime(1900, 1, 2, 0, 0, 0, 0);
            this.dtpIssueDate.Name = "dtpIssueDate";
            this.dtpIssueDate.ShowCheckBox = true;
            this.dtpIssueDate.Value = new System.DateTime(2015, 11, 17, 15, 9, 27, 849);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.chkCanBeSold);
            this.groupBox1.Controls.Add(this.lblCanBeSold);
            this.groupBox1.Controls.Add(this.ntbTareWeight);
            this.groupBox1.Controls.Add(this.lblTareWeight);
            this.groupBox1.Controls.Add(this.cmbKeyingInSerialNumber);
            this.groupBox1.Controls.Add(this.lblKeyInSerialNumber);
            this.groupBox1.Controls.Add(this.chkReturnable);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkMustSelectUOM);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnEditValidationPeriod);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbValPeriod);
            this.groupBox1.Controls.Add(this.cmbKeyingInQuantity);
            this.groupBox1.Controls.Add(this.chkScaleItem);
            this.groupBox1.Controls.Add(this.cmbKeyingInPrice);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkMustKeyInComment);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkZeroPriceValid);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkQtBecomesNegative);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkNoDiscount);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkCanBeSold
            // 
            resources.ApplyResources(this.chkCanBeSold, "chkCanBeSold");
            this.chkCanBeSold.Name = "chkCanBeSold";
            this.chkCanBeSold.UseVisualStyleBackColor = true;
            // 
            // lblCanBeSold
            // 
            this.lblCanBeSold.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCanBeSold, "lblCanBeSold");
            this.lblCanBeSold.Name = "lblCanBeSold";
            // 
            // ntbTareWeight
            // 
            this.ntbTareWeight.AllowDecimal = false;
            this.ntbTareWeight.AllowNegative = false;
            this.ntbTareWeight.CultureInfo = null;
            this.ntbTareWeight.DecimalLetters = 2;
            this.ntbTareWeight.ForeColor = System.Drawing.Color.Black;
            this.ntbTareWeight.HasMinValue = false;
            resources.ApplyResources(this.ntbTareWeight, "ntbTareWeight");
            this.ntbTareWeight.MaxValue = 99999D;
            this.ntbTareWeight.MinValue = 0D;
            this.ntbTareWeight.Name = "ntbTareWeight";
            this.ntbTareWeight.Value = 0D;
            // 
            // lblTareWeight
            // 
            this.lblTareWeight.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTareWeight, "lblTareWeight");
            this.lblTareWeight.Name = "lblTareWeight";
            // 
            // cmbKeyingInSerialNumber
            // 
            this.cmbKeyingInSerialNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyingInSerialNumber.FormattingEnabled = true;
            this.cmbKeyingInSerialNumber.Items.AddRange(new object[] {
            resources.GetString("cmbKeyingInSerialNumber.Items"),
            resources.GetString("cmbKeyingInSerialNumber.Items1"),
            resources.GetString("cmbKeyingInSerialNumber.Items2")});
            resources.ApplyResources(this.cmbKeyingInSerialNumber, "cmbKeyingInSerialNumber");
            this.cmbKeyingInSerialNumber.Name = "cmbKeyingInSerialNumber";
            // 
            // lblKeyInSerialNumber
            // 
            this.lblKeyInSerialNumber.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblKeyInSerialNumber, "lblKeyInSerialNumber");
            this.lblKeyInSerialNumber.Name = "lblKeyInSerialNumber";
            // 
            // chkReturnable
            // 
            resources.ApplyResources(this.chkReturnable, "chkReturnable");
            this.chkReturnable.Name = "chkReturnable";
            this.chkReturnable.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkMustSelectUOM
            // 
            resources.ApplyResources(this.chkMustSelectUOM, "chkMustSelectUOM");
            this.chkMustSelectUOM.Name = "chkMustSelectUOM";
            this.chkMustSelectUOM.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // btnEditValidationPeriod
            // 
            this.btnEditValidationPeriod.BackColor = System.Drawing.Color.Transparent;
            this.btnEditValidationPeriod.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditValidationPeriod, "btnEditValidationPeriod");
            this.btnEditValidationPeriod.Name = "btnEditValidationPeriod";
            this.btnEditValidationPeriod.Click += new System.EventHandler(this.btnEditValidationPeriod_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbValPeriod
            // 
            this.cmbValPeriod.AddList = null;
            this.cmbValPeriod.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbValPeriod, "cmbValPeriod");
            this.cmbValPeriod.MaxLength = 32767;
            this.cmbValPeriod.Name = "cmbValPeriod";
            this.cmbValPeriod.NoChangeAllowed = false;
            this.cmbValPeriod.OnlyDisplayID = false;
            this.cmbValPeriod.RemoveList = null;
            this.cmbValPeriod.RowHeight = ((short)(22));
            this.cmbValPeriod.SecondaryData = null;
            this.cmbValPeriod.SelectedData = null;
            this.cmbValPeriod.SelectedDataID = null;
            this.cmbValPeriod.SelectionList = null;
            this.cmbValPeriod.SkipIDColumn = true;
            this.cmbValPeriod.RequestData += new System.EventHandler(this.cmbValPeriod_RequestData);
            this.cmbValPeriod.RequestClear += new System.EventHandler(this.cmbValPeriod_RequestClear);
            // 
            // cmbKeyingInQuantity
            // 
            this.cmbKeyingInQuantity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyingInQuantity.FormattingEnabled = true;
            this.cmbKeyingInQuantity.Items.AddRange(new object[] {
            resources.GetString("cmbKeyingInQuantity.Items"),
            resources.GetString("cmbKeyingInQuantity.Items1"),
            resources.GetString("cmbKeyingInQuantity.Items2")});
            resources.ApplyResources(this.cmbKeyingInQuantity, "cmbKeyingInQuantity");
            this.cmbKeyingInQuantity.Name = "cmbKeyingInQuantity";
            // 
            // chkScaleItem
            // 
            resources.ApplyResources(this.chkScaleItem, "chkScaleItem");
            this.chkScaleItem.Name = "chkScaleItem";
            this.chkScaleItem.UseVisualStyleBackColor = true;
            this.chkScaleItem.CheckedChanged += new System.EventHandler(this.chkScaleItem_CheckedChanged);
            // 
            // cmbKeyingInPrice
            // 
            this.cmbKeyingInPrice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyingInPrice.FormattingEnabled = true;
            this.cmbKeyingInPrice.Items.AddRange(new object[] {
            resources.GetString("cmbKeyingInPrice.Items"),
            resources.GetString("cmbKeyingInPrice.Items1"),
            resources.GetString("cmbKeyingInPrice.Items2"),
            resources.GetString("cmbKeyingInPrice.Items3"),
            resources.GetString("cmbKeyingInPrice.Items4")});
            resources.ApplyResources(this.cmbKeyingInPrice, "cmbKeyingInPrice");
            this.cmbKeyingInPrice.Name = "cmbKeyingInPrice";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkMustKeyInComment
            // 
            resources.ApplyResources(this.chkMustKeyInComment, "chkMustKeyInComment");
            this.chkMustKeyInComment.Name = "chkMustKeyInComment";
            this.chkMustKeyInComment.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkZeroPriceValid
            // 
            resources.ApplyResources(this.chkZeroPriceValid, "chkZeroPriceValid");
            this.chkZeroPriceValid.Name = "chkZeroPriceValid";
            this.chkZeroPriceValid.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkQtBecomesNegative
            // 
            resources.ApplyResources(this.chkQtBecomesNegative, "chkQtBecomesNegative");
            this.chkQtBecomesNegative.Name = "chkQtBecomesNegative";
            this.chkQtBecomesNegative.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // chkNoDiscount
            // 
            resources.ApplyResources(this.chkNoDiscount, "chkNoDiscount");
            this.chkNoDiscount.Name = "chkNoDiscount";
            this.chkNoDiscount.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // ItemPOSSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "ItemPOSSettingsPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private DoubleBufferedCheckbox chkScaleItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private DoubleBufferedCheckbox chkNoDiscount;
        private DoubleBufferedCheckbox chkQtBecomesNegative;
        private DoubleBufferedCheckbox chkZeroPriceValid;
        private DoubleBufferedCheckbox chkMustKeyInComment;
        private System.Windows.Forms.ComboBox cmbKeyingInPrice;
        private System.Windows.Forms.ComboBox cmbKeyingInQuantity;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DoubleBufferGroupBox groupBox1;
        private DoubleBufferGroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private DoubleBufferGroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        //private System.Windows.Forms.ComboBox cmbValPeriod;
        private DualDataComboBox cmbValPeriod;
        private ContextButton btnEditValidationPeriod;
        private System.Windows.Forms.Label label13;
        private DoubleBufferedCheckbox chkMustSelectUOM;
        private System.Windows.Forms.Label label12;
        private MultiDateControl dtpIssueDate;
        private MultiDateControl dtpDateToBeBlocked;
        private DoubleBufferedCheckbox chkReturnable;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbKeyingInSerialNumber;
        private System.Windows.Forms.Label lblKeyInSerialNumber;
        private NumericTextBox ntbTareWeight;
        private System.Windows.Forms.Label lblTareWeight;
        private DoubleBufferedCheckbox chkCanBeSold;
        private System.Windows.Forms.Label lblCanBeSold;
    }
}
