using LSOne.Controls;

namespace LSOne.ViewPlugins.GiftCards.Dialogs
{
    partial class NewGiftCardManualIDDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewGiftCardManualIDDialog));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnAddCurrency = new LSOne.Controls.ContextButton();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.ntbAmount = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkFillable = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbGiftCardNumber = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkCreateAnother
            // 
            resources.ApplyResources(this.chkCreateAnother, "chkCreateAnother");
            this.chkCreateAnother.Checked = true;
            this.chkCreateAnother.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateAnother.Name = "chkCreateAnother";
            this.chkCreateAnother.UseVisualStyleBackColor = true;
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
            // btnAddCurrency
            // 
            this.btnAddCurrency.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCurrency.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCurrency, "btnAddCurrency");
            this.btnAddCurrency.Name = "btnAddCurrency";
            this.btnAddCurrency.Click += new System.EventHandler(this.btnAddCurrency_Click);
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
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.cmbCurrency_SelectedDataChanged);
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
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkFillable
            // 
            resources.ApplyResources(this.chkFillable, "chkFillable");
            this.chkFillable.Name = "chkFillable";
            this.chkFillable.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbGiftCardNumber
            // 
            resources.ApplyResources(this.tbGiftCardNumber, "tbGiftCardNumber");
            this.tbGiftCardNumber.Name = "tbGiftCardNumber";
            // 
            // NewGiftCardManualIDDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbGiftCardNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkFillable);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.btnAddCurrency);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewGiftCardManualIDDialog";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnAddCurrency, 0);
            this.Controls.SetChildIndex(this.cmbCurrency, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.ntbAmount, 0);
            this.Controls.SetChildIndex(this.chkFillable, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.tbGiftCardNumber, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButton btnAddCurrency;
        private DualDataComboBox cmbCurrency;
        private NumericTextBox ntbAmount;
        private System.Windows.Forms.CheckBox chkFillable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbGiftCardNumber;
        private System.Windows.Forms.CheckBox chkCreateAnother;
    }
}