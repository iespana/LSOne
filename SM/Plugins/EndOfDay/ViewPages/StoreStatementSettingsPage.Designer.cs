using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    partial class StoreStatementSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreStatementSettingsPage));
            this.lblAvgProdTiming = new System.Windows.Forms.Label();
            this.cmbTenderDeclarationCalculation = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ntbPerStatementLine = new LSOne.Controls.NumericTextBox();
            this.ntbTotal = new LSOne.Controls.NumericTextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAvgProdTiming
            // 
            resources.ApplyResources(this.lblAvgProdTiming, "lblAvgProdTiming");
            this.lblAvgProdTiming.Name = "lblAvgProdTiming";
            // 
            // cmbTenderDeclarationCalculation
            // 
            this.cmbTenderDeclarationCalculation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTenderDeclarationCalculation.FormattingEnabled = true;
            this.cmbTenderDeclarationCalculation.Items.AddRange(new object[] {
            resources.GetString("cmbTenderDeclarationCalculation.Items"),
            resources.GetString("cmbTenderDeclarationCalculation.Items1")});
            resources.ApplyResources(this.cmbTenderDeclarationCalculation, "cmbTenderDeclarationCalculation");
            this.cmbTenderDeclarationCalculation.Name = "cmbTenderDeclarationCalculation";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.ntbPerStatementLine);
            this.groupBox4.Controls.Add(this.ntbTotal);
            this.groupBox4.Controls.Add(this.lblAddress);
            this.groupBox4.Controls.Add(this.lblName);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // ntbPerStatementLine
            // 
            this.ntbPerStatementLine.AllowDecimal = false;
            this.ntbPerStatementLine.AllowNegative = false;
            this.ntbPerStatementLine.CultureInfo = null;
            this.ntbPerStatementLine.DecimalLetters = 2;
            this.ntbPerStatementLine.HasMinValue = false;
            resources.ApplyResources(this.ntbPerStatementLine, "ntbPerStatementLine");
            this.ntbPerStatementLine.MaxValue = 1000000D;
            this.ntbPerStatementLine.MinValue = 0D;
            this.ntbPerStatementLine.Name = "ntbPerStatementLine";
            this.ntbPerStatementLine.Value = 0D;
            // 
            // ntbTotal
            // 
            this.ntbTotal.AllowDecimal = false;
            this.ntbTotal.AllowNegative = false;
            this.ntbTotal.CultureInfo = null;
            this.ntbTotal.DecimalLetters = 2;
            this.ntbTotal.HasMinValue = false;
            resources.ApplyResources(this.ntbTotal, "ntbTotal");
            this.ntbTotal.MaxValue = 1000000D;
            this.ntbTotal.MinValue = 0D;
            this.ntbTotal.Name = "ntbTotal";
            this.ntbTotal.Value = 0D;
            // 
            // lblAddress
            // 
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.Name = "lblAddress";
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // StoreStatementSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cmbTenderDeclarationCalculation);
            this.Controls.Add(this.lblAvgProdTiming);
            this.DoubleBuffered = true;
            this.Name = "StoreStatementSettingsPage";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAvgProdTiming;
        private System.Windows.Forms.ComboBox cmbTenderDeclarationCalculation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblName;
        private NumericTextBox ntbPerStatementLine;
        private NumericTextBox ntbTotal;

    }
}
