namespace LSOne.ViewPlugins.TaxRefund.Dialogs
{
    partial class EditTaxRefundRangeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTaxRefundRangeDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ntbFrom = new LSOne.Controls.NumericTextBox();
            this.ntbTo = new LSOne.Controls.NumericTextBox();
            this.ntbTaxRefundRange = new LSOne.Controls.NumericTextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblTaxReturn = new System.Windows.Forms.Label();
            this.lblTaxReturnPercentage = new System.Windows.Forms.Label();
            this.ntbTaxRefundPct = new LSOne.Controls.NumericTextBox();
            this.panel2.SuspendLayout();
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
            // 
            // ntbFrom
            // 
            this.ntbFrom.AllowDecimal = true;
            this.ntbFrom.AllowNegative = false;
            this.ntbFrom.CultureInfo = null;
            this.ntbFrom.DecimalLetters = 5;
            this.ntbFrom.HasMinValue = false;
            resources.ApplyResources(this.ntbFrom, "ntbFrom");
            this.ntbFrom.MaxValue = 999999999999D;
            this.ntbFrom.MinValue = 0D;
            this.ntbFrom.Name = "ntbFrom";
            this.ntbFrom.Value = 0D;
            this.ntbFrom.TextChanged += new System.EventHandler(this.ntb_TextChanged);
            // 
            // ntbTo
            // 
            this.ntbTo.AllowDecimal = true;
            this.ntbTo.AllowNegative = false;
            this.ntbTo.CultureInfo = null;
            this.ntbTo.DecimalLetters = 5;
            this.ntbTo.HasMinValue = false;
            resources.ApplyResources(this.ntbTo, "ntbTo");
            this.ntbTo.MaxValue = 999999999999D;
            this.ntbTo.MinValue = 0D;
            this.ntbTo.Name = "ntbTo";
            this.ntbTo.Value = 0D;
            this.ntbTo.TextChanged += new System.EventHandler(this.ntb_TextChanged);
            // 
            // ntbTaxRefundRange
            // 
            this.ntbTaxRefundRange.AllowDecimal = true;
            this.ntbTaxRefundRange.AllowNegative = false;
            this.ntbTaxRefundRange.CultureInfo = null;
            this.ntbTaxRefundRange.DecimalLetters = 5;
            this.ntbTaxRefundRange.HasMinValue = false;
            resources.ApplyResources(this.ntbTaxRefundRange, "ntbTaxRefundRange");
            this.ntbTaxRefundRange.MaxValue = 999999999999D;
            this.ntbTaxRefundRange.MinValue = 0D;
            this.ntbTaxRefundRange.Name = "ntbTaxRefundRange";
            this.ntbTaxRefundRange.Value = 0D;
            // 
            // lblFrom
            // 
            this.lblFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // lblTo
            // 
            this.lblTo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // lblTaxReturn
            // 
            this.lblTaxReturn.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTaxReturn, "lblTaxReturn");
            this.lblTaxReturn.Name = "lblTaxReturn";
            // 
            // lblTaxReturnPercentage
            // 
            this.lblTaxReturnPercentage.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTaxReturnPercentage, "lblTaxReturnPercentage");
            this.lblTaxReturnPercentage.Name = "lblTaxReturnPercentage";
            // 
            // ntbTaxRefundPct
            // 
            this.ntbTaxRefundPct.AllowDecimal = true;
            this.ntbTaxRefundPct.AllowNegative = false;
            this.ntbTaxRefundPct.CultureInfo = null;
            this.ntbTaxRefundPct.DecimalLetters = 5;
            this.ntbTaxRefundPct.HasMinValue = false;
            resources.ApplyResources(this.ntbTaxRefundPct, "ntbTaxRefundPct");
            this.ntbTaxRefundPct.MaxValue = 100D;
            this.ntbTaxRefundPct.MinValue = 0D;
            this.ntbTaxRefundPct.Name = "ntbTaxRefundPct";
            this.ntbTaxRefundPct.Value = 0D;
            this.ntbTaxRefundPct.TextChanged += new System.EventHandler(this.OnTaxPercentageChanged);
            // 
            // EditTaxRefundRangeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTaxReturnPercentage);
            this.Controls.Add(this.ntbTaxRefundPct);
            this.Controls.Add(this.lblTaxReturn);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.ntbTaxRefundRange);
            this.Controls.Add(this.ntbTo);
            this.Controls.Add(this.ntbFrom);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "EditTaxRefundRangeDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.ntbFrom, 0);
            this.Controls.SetChildIndex(this.ntbTo, 0);
            this.Controls.SetChildIndex(this.ntbTaxRefundRange, 0);
            this.Controls.SetChildIndex(this.lblFrom, 0);
            this.Controls.SetChildIndex(this.lblTo, 0);
            this.Controls.SetChildIndex(this.lblTaxReturn, 0);
            this.Controls.SetChildIndex(this.ntbTaxRefundPct, 0);
            this.Controls.SetChildIndex(this.lblTaxReturnPercentage, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.NumericTextBox ntbFrom;
        private Controls.NumericTextBox ntbTo;
        private Controls.NumericTextBox ntbTaxRefundRange;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblTaxReturn;
        private System.Windows.Forms.Label lblTaxReturnPercentage;
        private Controls.NumericTextBox ntbTaxRefundPct;
    }
}
