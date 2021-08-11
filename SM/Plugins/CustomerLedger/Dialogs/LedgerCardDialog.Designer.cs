using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLedger.Dialogs
{
    partial class LedgerCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LedgerCardDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.tbDocumentNo = new System.Windows.Forms.TextBox();
            this.lblDocumentNo = new System.Windows.Forms.Label();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.lblCurrencyAmount = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.cmbCurrency = new DualDataComboBox();
            this.ntbCurrencyAmount = new NumericTextBox();
            this.ntbAmount = new NumericTextBox();
            this.ntbDiscountAmount = new NumericTextBox();
            this.linkFields1 = new LinkFields();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // dtpDate
            // 
            this.dtpDate.Checked = false;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDate, "dtpDate");
            this.dtpDate.Name = "dtpDate";
            // 
            // lblDate
            // 
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.Name = "lblDate";
            // 
            // tbDocumentNo
            // 
            resources.ApplyResources(this.tbDocumentNo, "tbDocumentNo");
            this.tbDocumentNo.Name = "tbDocumentNo";
            // 
            // lblDocumentNo
            // 
            resources.ApplyResources(this.lblDocumentNo, "lblDocumentNo");
            this.lblDocumentNo.Name = "lblDocumentNo";
            // 
            // lblCurrency
            // 
            resources.ApplyResources(this.lblCurrency, "lblCurrency");
            this.lblCurrency.Name = "lblCurrency";
            // 
            // lblCurrencyAmount
            // 
            resources.ApplyResources(this.lblCurrencyAmount, "lblCurrencyAmount");
            this.lblCurrencyAmount.Name = "lblCurrencyAmount";
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.Name = "lblAmount";
            // 
            // lblDiscountAmount
            // 
            resources.ApplyResources(this.lblDiscountAmount, "lblDiscountAmount");
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.AddList = null;
            this.cmbCurrency.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCurrency, "cmbCurrency");
            this.cmbCurrency.MaxLength = 32767;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.RemoveList = null;
            this.cmbCurrency.RowHeight = ((short)(22));
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SelectionList = null;
            this.cmbCurrency.SkipIDColumn = false;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.cmbCurrency_SelectedDataChanged);
            this.cmbCurrency.RequestClear += new System.EventHandler(this.cmbCurrency_RequestClear);
            // 
            // ntbCurrencyAmount
            // 
            this.ntbCurrencyAmount.AllowDecimal = true;
            this.ntbCurrencyAmount.AllowNegative = true;
            this.ntbCurrencyAmount.CultureInfo = null;
            this.ntbCurrencyAmount.DecimalLetters = 2;
            this.ntbCurrencyAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbCurrencyAmount, "ntbCurrencyAmount");
            this.ntbCurrencyAmount.MaxValue = 99999999999999D;
            this.ntbCurrencyAmount.MinValue = 0D;
            this.ntbCurrencyAmount.Name = "ntbCurrencyAmount";
            this.ntbCurrencyAmount.Value = 0D;
            this.ntbCurrencyAmount.TextChanged += new System.EventHandler(this.ntbCurrencyAmount_TextChanged);
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = true;
            this.ntbAmount.AllowNegative = true;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.MaxValue = 999999999999999D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbDiscountAmount
            // 
            this.ntbDiscountAmount.AllowDecimal = true;
            this.ntbDiscountAmount.AllowNegative = false;
            this.ntbDiscountAmount.CultureInfo = null;
            this.ntbDiscountAmount.DecimalLetters = 2;
            this.ntbDiscountAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountAmount, "ntbDiscountAmount");
            this.ntbDiscountAmount.MaxValue = 999999999999999D;
            this.ntbDiscountAmount.MinValue = 0D;
            this.ntbDiscountAmount.Name = "ntbDiscountAmount";
            this.ntbDiscountAmount.Value = 0D;
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // LedgerCardDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.ntbDiscountAmount);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.ntbCurrencyAmount);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblDiscountAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblCurrencyAmount);
            this.Controls.Add(this.lblCurrency);
            this.Controls.Add(this.tbDocumentNo);
            this.Controls.Add(this.lblDocumentNo);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "LedgerCardDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.dtpDate, 0);
            this.Controls.SetChildIndex(this.lblDate, 0);
            this.Controls.SetChildIndex(this.lblDocumentNo, 0);
            this.Controls.SetChildIndex(this.tbDocumentNo, 0);
            this.Controls.SetChildIndex(this.lblCurrency, 0);
            this.Controls.SetChildIndex(this.lblCurrencyAmount, 0);
            this.Controls.SetChildIndex(this.lblAmount, 0);
            this.Controls.SetChildIndex(this.lblDiscountAmount, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.cmbCurrency, 0);
            this.Controls.SetChildIndex(this.ntbCurrencyAmount, 0);
            this.Controls.SetChildIndex(this.ntbAmount, 0);
            this.Controls.SetChildIndex(this.ntbDiscountAmount, 0);
            this.Controls.SetChildIndex(this.linkFields1, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblDiscountAmount;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblCurrencyAmount;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.TextBox tbDocumentNo;
        private System.Windows.Forms.Label lblDocumentNo;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private DualDataComboBox cmbCurrency;
        private NumericTextBox ntbCurrencyAmount;
        private NumericTextBox ntbAmount;
        private NumericTextBox ntbDiscountAmount;
        private LinkFields linkFields1;
    }
}