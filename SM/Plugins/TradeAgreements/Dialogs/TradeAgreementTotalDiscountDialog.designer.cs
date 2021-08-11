using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    partial class TradeAgreementTotalDiscountDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeAgreementTotalDiscountDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ntbPercent1 = new LSOne.Controls.NumericTextBox();
            this.ntbDiscount = new LSOne.Controls.NumericTextBox();
            this.ntbAmount = new LSOne.Controls.NumericTextBox();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbPercent2 = new LSOne.Controls.NumericTextBox();
            this.btnAddCurrency = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // dtpToDate
            // 
            this.dtpToDate.Checked = false;
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.ShowCheckBox = true;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // ntbPercent1
            // 
            this.ntbPercent1.AllowDecimal = true;
            this.ntbPercent1.AllowNegative = false;
            this.ntbPercent1.CultureInfo = null;
            this.ntbPercent1.DecimalLetters = 2;
            this.ntbPercent1.ForeColor = System.Drawing.Color.Black;
            this.ntbPercent1.HasMinValue = false;
            resources.ApplyResources(this.ntbPercent1, "ntbPercent1");
            this.ntbPercent1.MaxValue = 100D;
            this.ntbPercent1.MinValue = 0D;
            this.ntbPercent1.Name = "ntbPercent1";
            this.ntbPercent1.Value = 0D;
            this.ntbPercent1.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbDiscount
            // 
            this.ntbDiscount.AllowDecimal = true;
            this.ntbDiscount.AllowNegative = false;
            this.ntbDiscount.CultureInfo = null;
            this.ntbDiscount.DecimalLetters = 2;
            this.ntbDiscount.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscount.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscount, "ntbDiscount");
            this.ntbDiscount.MaxValue = 999999999999D;
            this.ntbDiscount.MinValue = 0D;
            this.ntbDiscount.Name = "ntbDiscount";
            this.ntbDiscount.Value = 0D;
            this.ntbDiscount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = true;
            this.ntbAmount.AllowNegative = false;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.MaxValue = 999999999999D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.AddList = null;
            this.cmbCurrency.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCurrency, "cmbCurrency");
            this.cmbCurrency.MaxLength = 32767;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.NoChangeAllowed = false;
            this.cmbCurrency.OnlyDisplayID = false;
            this.cmbCurrency.RemoveList = null;
            this.cmbCurrency.RowHeight = ((short)(22));
            this.cmbCurrency.SecondaryData = null;
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SelectedDataID = null;
            this.cmbCurrency.SelectionList = null;
            this.cmbCurrency.SkipIDColumn = true;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbPercent2
            // 
            this.ntbPercent2.AllowDecimal = true;
            this.ntbPercent2.AllowNegative = false;
            this.ntbPercent2.CultureInfo = null;
            this.ntbPercent2.DecimalLetters = 2;
            this.ntbPercent2.ForeColor = System.Drawing.Color.Black;
            this.ntbPercent2.HasMinValue = false;
            resources.ApplyResources(this.ntbPercent2, "ntbPercent2");
            this.ntbPercent2.MaxValue = 100D;
            this.ntbPercent2.MinValue = 0D;
            this.ntbPercent2.Name = "ntbPercent2";
            this.ntbPercent2.Value = 0D;
            this.ntbPercent2.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddCurrency
            // 
            this.btnAddCurrency.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCurrency.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCurrency, "btnAddCurrency");
            this.btnAddCurrency.Name = "btnAddCurrency";
            this.btnAddCurrency.Click += new System.EventHandler(this.btnAddCurrency_Click);
            // 
            // TradeAgreementTotalDiscountDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddCurrency);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ntbPercent2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.ntbPercent1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ntbDiscount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "TradeAgreementTotalDiscountDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbCurrency, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.dtpFromDate, 0);
            this.Controls.SetChildIndex(this.dtpToDate, 0);
            this.Controls.SetChildIndex(this.ntbAmount, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.ntbDiscount, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.ntbPercent1, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.ntbPercent2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnAddCurrency, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private NumericTextBox ntbDiscount;
        private System.Windows.Forms.Label label9;
        private NumericTextBox ntbAmount;
        private System.Windows.Forms.Label label13;
        private NumericTextBox ntbPercent1;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbPercent2;
        private ContextButton btnAddCurrency;
    }
}