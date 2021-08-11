using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class CashDenominatorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashDenominatorDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbCurrency = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbAmount = new LSOne.Controls.NumericTextBox();
            this.AAmount = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDenomination = new System.Windows.Forms.TextBox();
            this.lblDenomination = new System.Windows.Forms.Label();
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
            // tbCurrency
            // 
            resources.ApplyResources(this.tbCurrency, "tbCurrency");
            this.tbCurrency.Name = "tbCurrency";
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
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = true;
            this.ntbAmount.AllowNegative = false;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.MaxValue = 10000000D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // AAmount
            // 
            resources.ApplyResources(this.AAmount, "AAmount");
            this.AAmount.Name = "AAmount";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbDenomination
            // 
            resources.ApplyResources(this.tbDenomination, "tbDenomination");
            this.tbDenomination.Name = "tbDenomination";
            this.tbDenomination.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDenomination
            // 
            resources.ApplyResources(this.lblDenomination, "lblDenomination");
            this.lblDenomination.Name = "lblDenomination";
            // 
            // CashDenominatorDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbDenomination);
            this.Controls.Add(this.lblDenomination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.AAmount);
            this.Controls.Add(this.tbCurrency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "CashDenominatorDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbCurrency, 0);
            this.Controls.SetChildIndex(this.AAmount, 0);
            this.Controls.SetChildIndex(this.ntbAmount, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblDenomination, 0);
            this.Controls.SetChildIndex(this.tbDenomination, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbCurrency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbAmount;
        private System.Windows.Forms.Label AAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.TextBox tbDenomination;
        private System.Windows.Forms.Label lblDenomination;
    }
}