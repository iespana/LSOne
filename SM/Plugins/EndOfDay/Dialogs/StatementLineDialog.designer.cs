using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.Dialogs
{
    partial class StatementLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatementLineDialog));
            this.tbStatementNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStore = new DualDataComboBox();
            this.tbTerminalID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbStaffID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTender = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ntbTransactionAmount = new NumericTextBox();
            this.ntbBankedAmount = new NumericTextBox();
            this.ntbCountedAmount = new NumericTextBox();
            this.ntbSafeAmount = new NumericTextBox();
            this.ntbDifference = new NumericTextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbStatementNumber
            // 
            resources.ApplyResources(this.tbStatementNumber, "tbStatementNumber");
            this.tbStatementNumber.Name = "tbStatementNumber";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbStore
            // 
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.SelectedData = null;
            // 
            // tbTerminalID
            // 
            resources.ApplyResources(this.tbTerminalID, "tbTerminalID");
            this.tbTerminalID.Name = "tbTerminalID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbStaffID
            // 
            resources.ApplyResources(this.tbStaffID, "tbStaffID");
            this.tbStaffID.Name = "tbStaffID";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbTender
            // 
            resources.ApplyResources(this.tbTender, "tbTender");
            this.tbTender.Name = "tbTender";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
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
            // ntbTransactionAmount
            // 
            this.ntbTransactionAmount.AllowDecimal = true;
            this.ntbTransactionAmount.AllowNegative = true;
            this.ntbTransactionAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbTransactionAmount, "ntbTransactionAmount");
            this.ntbTransactionAmount.MaxValue = 0D;
            this.ntbTransactionAmount.Name = "ntbTransactionAmount";
            this.ntbTransactionAmount.Value = 0D;
            // 
            // ntbBankedAmount
            // 
            this.ntbBankedAmount.AllowDecimal = true;
            this.ntbBankedAmount.AllowNegative = true;
            this.ntbBankedAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbBankedAmount, "ntbBankedAmount");
            this.ntbBankedAmount.MaxValue = 0D;
            this.ntbBankedAmount.Name = "ntbBankedAmount";
            this.ntbBankedAmount.Value = 0D;
            // 
            // ntbCountedAmount
            // 
            this.ntbCountedAmount.AllowDecimal = true;
            this.ntbCountedAmount.AllowNegative = true;
            this.ntbCountedAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbCountedAmount, "ntbCountedAmount");
            this.ntbCountedAmount.MaxValue = 0D;
            this.ntbCountedAmount.Name = "ntbCountedAmount";
            this.ntbCountedAmount.Value = 0D;
            // 
            // ntbSafeAmount
            // 
            this.ntbSafeAmount.AllowDecimal = true;
            this.ntbSafeAmount.AllowNegative = true;
            this.ntbSafeAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbSafeAmount, "ntbSafeAmount");
            this.ntbSafeAmount.MaxValue = 0D;
            this.ntbSafeAmount.Name = "ntbSafeAmount";
            this.ntbSafeAmount.Value = 0D;
            // 
            // ntbDifference
            // 
            this.ntbDifference.AllowDecimal = true;
            this.ntbDifference.AllowNegative = true;
            this.ntbDifference.DecimalLetters = 2;
            resources.ApplyResources(this.ntbDifference, "ntbDifference");
            this.ntbDifference.MaxValue = 0D;
            this.ntbDifference.Name = "ntbDifference";
            this.ntbDifference.Value = 0D;
            // 
            // StatementLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbDifference);
            this.Controls.Add(this.ntbCountedAmount);
            this.Controls.Add(this.ntbSafeAmount);
            this.Controls.Add(this.ntbBankedAmount);
            this.Controls.Add(this.ntbTransactionAmount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbTender);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbStaffID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbTerminalID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbStatementNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "StatementLineDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbStatementNumber, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tbTerminalID, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbStaffID, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbTender, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.ntbTransactionAmount, 0);
            this.Controls.SetChildIndex(this.ntbBankedAmount, 0);
            this.Controls.SetChildIndex(this.ntbSafeAmount, 0);
            this.Controls.SetChildIndex(this.ntbCountedAmount, 0);
            this.Controls.SetChildIndex(this.ntbDifference, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbStatementNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbTender;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStaffID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTerminalID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private NumericTextBox ntbDifference;
        private NumericTextBox ntbCountedAmount;
        private NumericTextBox ntbSafeAmount;
        private NumericTextBox ntbBankedAmount;
        private NumericTextBox ntbTransactionAmount;
    }
}