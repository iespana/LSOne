using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class ExchangeRateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExchangeRateDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbFromCurrency = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblSize = new System.Windows.Forms.Label();
            this.tbToCurrency = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbExchangeRate = new LSOne.Controls.NumericTextBox();
            this.ntbPOSExchangeRate = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // tbFromCurrency
            // 
            resources.ApplyResources(this.tbFromCurrency, "tbFromCurrency");
            this.tbFromCurrency.Name = "tbFromCurrency";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblSize
            // 
            resources.ApplyResources(this.lblSize, "lblSize");
            this.lblSize.Name = "lblSize";
            // 
            // tbToCurrency
            // 
            resources.ApplyResources(this.tbToCurrency, "tbToCurrency");
            this.tbToCurrency.Name = "tbToCurrency";
            this.tbToCurrency.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbExchangeRate
            // 
            this.ntbExchangeRate.AllowDecimal = true;
            this.ntbExchangeRate.AllowNegative = false;
            this.ntbExchangeRate.CultureInfo = null;
            this.ntbExchangeRate.DecimalLetters = 4;
            this.ntbExchangeRate.ForeColor = System.Drawing.Color.Black;
            this.ntbExchangeRate.HasMinValue = false;
            resources.ApplyResources(this.ntbExchangeRate, "ntbExchangeRate");
            this.ntbExchangeRate.MaxValue = 10000000D;
            this.ntbExchangeRate.MinValue = 0D;
            this.ntbExchangeRate.Name = "ntbExchangeRate";
            this.ntbExchangeRate.Value = 0D;
            this.ntbExchangeRate.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbPOSExchangeRate
            // 
            this.ntbPOSExchangeRate.AllowDecimal = true;
            this.ntbPOSExchangeRate.AllowNegative = false;
            this.ntbPOSExchangeRate.CultureInfo = null;
            this.ntbPOSExchangeRate.DecimalLetters = 4;
            this.ntbPOSExchangeRate.ForeColor = System.Drawing.Color.Black;
            this.ntbPOSExchangeRate.HasMinValue = false;
            resources.ApplyResources(this.ntbPOSExchangeRate, "ntbPOSExchangeRate");
            this.ntbPOSExchangeRate.MaxValue = 10000000D;
            this.ntbPOSExchangeRate.MinValue = 0D;
            this.ntbPOSExchangeRate.Name = "ntbPOSExchangeRate";
            this.ntbPOSExchangeRate.Value = 0D;
            this.ntbPOSExchangeRate.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Name = "dtpStartDate";
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // ExchangeRateDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ntbPOSExchangeRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbExchangeRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbToCurrency);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.tbFromCurrency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ExchangeRateDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbFromCurrency, 0);
            this.Controls.SetChildIndex(this.lblSize, 0);
            this.Controls.SetChildIndex(this.tbToCurrency, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ntbExchangeRate, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.ntbPOSExchangeRate, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dtpStartDate, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbFromCurrency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.TextBox tbToCurrency;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbExchangeRate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbPOSExchangeRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider2;
    }
}