using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    partial class FunctionalProfileTerminalPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalProfileTerminalPage));
            this.cmbEventLog = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.chkKeepDailyJournalOpenAfterPrintingReceipt = new System.Windows.Forms.CheckBox();
            this.chkCustomerRequiredOnReturn = new System.Windows.Forms.CheckBox();
            this.chkAllowSaleAndReturn = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkUsePolling = new System.Windows.Forms.CheckBox();
            this.lblPollingInterval = new System.Windows.Forms.Label();
            this.ntbPollingInterval = new LSOne.Controls.NumericTextBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.chkTDUsesDenomination = new System.Windows.Forms.CheckBox();
            this.chkBDRevUsesDenomination = new System.Windows.Forms.CheckBox();
            this.chkBDUsesDenomination = new System.Windows.Forms.CheckBox();
            this.chkSDRevUsesDenomination = new System.Windows.Forms.CheckBox();
            this.chkSDUsesDenomination = new System.Windows.Forms.CheckBox();
            this.chkAllowTransactionsWithOpenDrawer = new System.Windows.Forms.CheckBox();
            this.chkDisplayVoidedPayments = new System.Windows.Forms.CheckBox();
            this.ntbClearAfter = new LSOne.Controls.NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ntbDecimalsInNumpad = new LSOne.Controls.NumericTextBox();
            this.ntbMaxQuantity = new LSOne.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ntbMaxPrice = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbClearSettings = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbEventLog
            // 
            this.cmbEventLog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEventLog.FormattingEnabled = true;
            this.cmbEventLog.Items.AddRange(new object[] {
            resources.GetString("cmbEventLog.Items"),
            resources.GetString("cmbEventLog.Items1"),
            resources.GetString("cmbEventLog.Items2"),
            resources.GetString("cmbEventLog.Items3")});
            resources.ApplyResources(this.cmbEventLog, "cmbEventLog");
            this.cmbEventLog.Name = "cmbEventLog";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.chkKeepDailyJournalOpenAfterPrintingReceipt);
            this.groupPanel1.Controls.Add(this.chkCustomerRequiredOnReturn);
            this.groupPanel1.Controls.Add(this.chkAllowSaleAndReturn);
            this.groupPanel1.Controls.Add(this.tableLayoutPanel1);
            this.groupPanel1.Controls.Add(this.chkTDUsesDenomination);
            this.groupPanel1.Controls.Add(this.chkBDRevUsesDenomination);
            this.groupPanel1.Controls.Add(this.chkBDUsesDenomination);
            this.groupPanel1.Controls.Add(this.chkSDRevUsesDenomination);
            this.groupPanel1.Controls.Add(this.chkSDUsesDenomination);
            this.groupPanel1.Controls.Add(this.chkAllowTransactionsWithOpenDrawer);
            this.groupPanel1.Controls.Add(this.chkDisplayVoidedPayments);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // chkKeepDailyJournalOpenAfterPrintingReceipt
            // 
            resources.ApplyResources(this.chkKeepDailyJournalOpenAfterPrintingReceipt, "chkKeepDailyJournalOpenAfterPrintingReceipt");
            this.chkKeepDailyJournalOpenAfterPrintingReceipt.BackColor = System.Drawing.Color.Transparent;
            this.chkKeepDailyJournalOpenAfterPrintingReceipt.Name = "chkKeepDailyJournalOpenAfterPrintingReceipt";
            this.chkKeepDailyJournalOpenAfterPrintingReceipt.UseVisualStyleBackColor = false;
            // 
            // chkCustomerRequiredOnReturn
            // 
            resources.ApplyResources(this.chkCustomerRequiredOnReturn, "chkCustomerRequiredOnReturn");
            this.chkCustomerRequiredOnReturn.BackColor = System.Drawing.Color.Transparent;
            this.chkCustomerRequiredOnReturn.Name = "chkCustomerRequiredOnReturn";
            this.chkCustomerRequiredOnReturn.UseVisualStyleBackColor = false;
            // 
            // chkAllowSaleAndReturn
            // 
            resources.ApplyResources(this.chkAllowSaleAndReturn, "chkAllowSaleAndReturn");
            this.chkAllowSaleAndReturn.BackColor = System.Drawing.Color.Transparent;
            this.chkAllowSaleAndReturn.Name = "chkAllowSaleAndReturn";
            this.chkAllowSaleAndReturn.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.chkUsePolling, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPollingInterval, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.ntbPollingInterval, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSeconds, 5, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // chkUsePolling
            // 
            resources.ApplyResources(this.chkUsePolling, "chkUsePolling");
            this.chkUsePolling.BackColor = System.Drawing.Color.Transparent;
            this.chkUsePolling.Name = "chkUsePolling";
            this.chkUsePolling.UseVisualStyleBackColor = false;
            this.chkUsePolling.CheckedChanged += new System.EventHandler(this.chkUsePolling_CheckedChanged);
            // 
            // lblPollingInterval
            // 
            resources.ApplyResources(this.lblPollingInterval, "lblPollingInterval");
            this.lblPollingInterval.Name = "lblPollingInterval";
            // 
            // ntbPollingInterval
            // 
            this.ntbPollingInterval.AllowDecimal = true;
            this.ntbPollingInterval.AllowNegative = false;
            this.ntbPollingInterval.CultureInfo = null;
            this.ntbPollingInterval.DecimalLetters = 0;
            this.ntbPollingInterval.ForeColor = System.Drawing.Color.Black;
            this.ntbPollingInterval.HasMinValue = true;
            resources.ApplyResources(this.ntbPollingInterval, "ntbPollingInterval");
            this.ntbPollingInterval.MaxValue = 0D;
            this.ntbPollingInterval.MinValue = 1D;
            this.ntbPollingInterval.Name = "ntbPollingInterval";
            this.ntbPollingInterval.Value = 0D;
            // 
            // lblSeconds
            // 
            resources.ApplyResources(this.lblSeconds, "lblSeconds");
            this.lblSeconds.Name = "lblSeconds";
            // 
            // chkTDUsesDenomination
            // 
            resources.ApplyResources(this.chkTDUsesDenomination, "chkTDUsesDenomination");
            this.chkTDUsesDenomination.BackColor = System.Drawing.Color.Transparent;
            this.chkTDUsesDenomination.Name = "chkTDUsesDenomination";
            this.chkTDUsesDenomination.UseVisualStyleBackColor = false;
            // 
            // chkBDRevUsesDenomination
            // 
            resources.ApplyResources(this.chkBDRevUsesDenomination, "chkBDRevUsesDenomination");
            this.chkBDRevUsesDenomination.BackColor = System.Drawing.Color.Transparent;
            this.chkBDRevUsesDenomination.Name = "chkBDRevUsesDenomination";
            this.chkBDRevUsesDenomination.UseVisualStyleBackColor = false;
            // 
            // chkBDUsesDenomination
            // 
            resources.ApplyResources(this.chkBDUsesDenomination, "chkBDUsesDenomination");
            this.chkBDUsesDenomination.BackColor = System.Drawing.Color.Transparent;
            this.chkBDUsesDenomination.Name = "chkBDUsesDenomination";
            this.chkBDUsesDenomination.UseVisualStyleBackColor = false;
            // 
            // chkSDRevUsesDenomination
            // 
            resources.ApplyResources(this.chkSDRevUsesDenomination, "chkSDRevUsesDenomination");
            this.chkSDRevUsesDenomination.BackColor = System.Drawing.Color.Transparent;
            this.chkSDRevUsesDenomination.Name = "chkSDRevUsesDenomination";
            this.chkSDRevUsesDenomination.UseVisualStyleBackColor = false;
            // 
            // chkSDUsesDenomination
            // 
            resources.ApplyResources(this.chkSDUsesDenomination, "chkSDUsesDenomination");
            this.chkSDUsesDenomination.BackColor = System.Drawing.Color.Transparent;
            this.chkSDUsesDenomination.Name = "chkSDUsesDenomination";
            this.chkSDUsesDenomination.UseVisualStyleBackColor = false;
            // 
            // chkAllowTransactionsWithOpenDrawer
            // 
            resources.ApplyResources(this.chkAllowTransactionsWithOpenDrawer, "chkAllowTransactionsWithOpenDrawer");
            this.chkAllowTransactionsWithOpenDrawer.BackColor = System.Drawing.Color.Transparent;
            this.chkAllowTransactionsWithOpenDrawer.Name = "chkAllowTransactionsWithOpenDrawer";
            this.chkAllowTransactionsWithOpenDrawer.UseVisualStyleBackColor = false;
            // 
            // chkDisplayVoidedPayments
            // 
            resources.ApplyResources(this.chkDisplayVoidedPayments, "chkDisplayVoidedPayments");
            this.chkDisplayVoidedPayments.BackColor = System.Drawing.Color.Transparent;
            this.chkDisplayVoidedPayments.Name = "chkDisplayVoidedPayments";
            this.chkDisplayVoidedPayments.UseVisualStyleBackColor = false;
            // 
            // ntbClearAfter
            // 
            this.ntbClearAfter.AllowDecimal = true;
            this.ntbClearAfter.AllowNegative = false;
            this.ntbClearAfter.CultureInfo = null;
            this.ntbClearAfter.DecimalLetters = 0;
            this.ntbClearAfter.ForeColor = System.Drawing.Color.Black;
            this.ntbClearAfter.HasMinValue = true;
            resources.ApplyResources(this.ntbClearAfter, "ntbClearAfter");
            this.ntbClearAfter.MaxValue = 0D;
            this.ntbClearAfter.MinValue = 1D;
            this.ntbClearAfter.Name = "ntbClearAfter";
            this.ntbClearAfter.Value = 0D;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ntbDecimalsInNumpad
            // 
            this.ntbDecimalsInNumpad.AllowDecimal = false;
            this.ntbDecimalsInNumpad.AllowNegative = false;
            this.ntbDecimalsInNumpad.CultureInfo = null;
            this.ntbDecimalsInNumpad.DecimalLetters = 0;
            this.ntbDecimalsInNumpad.ForeColor = System.Drawing.Color.Black;
            this.ntbDecimalsInNumpad.HasMinValue = false;
            resources.ApplyResources(this.ntbDecimalsInNumpad, "ntbDecimalsInNumpad");
            this.ntbDecimalsInNumpad.MaxValue = 6D;
            this.ntbDecimalsInNumpad.MinValue = 0D;
            this.ntbDecimalsInNumpad.Name = "ntbDecimalsInNumpad";
            this.ntbDecimalsInNumpad.Value = 2D;
            // 
            // ntbMaxQuantity
            // 
            this.ntbMaxQuantity.AllowDecimal = false;
            this.ntbMaxQuantity.AllowNegative = false;
            this.ntbMaxQuantity.CultureInfo = null;
            this.ntbMaxQuantity.DecimalLetters = 0;
            this.ntbMaxQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxQuantity, "ntbMaxQuantity");
            this.ntbMaxQuantity.MaxValue = 999999999999D;
            this.ntbMaxQuantity.MinValue = 0D;
            this.ntbMaxQuantity.Name = "ntbMaxQuantity";
            this.ntbMaxQuantity.Value = 0D;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ntbMaxPrice
            // 
            this.ntbMaxPrice.AllowDecimal = false;
            this.ntbMaxPrice.AllowNegative = false;
            this.ntbMaxPrice.CultureInfo = null;
            this.ntbMaxPrice.DecimalLetters = 0;
            this.ntbMaxPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxPrice, "ntbMaxPrice");
            this.ntbMaxPrice.MaxValue = 999999999999D;
            this.ntbMaxPrice.MinValue = 0D;
            this.ntbMaxPrice.Name = "ntbMaxPrice";
            this.ntbMaxPrice.Value = 0D;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lblMinutes
            // 
            resources.ApplyResources(this.lblMinutes, "lblMinutes");
            this.lblMinutes.Name = "lblMinutes";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.lblMinutes, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.ntbClearAfter, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbClearSettings, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // cmbClearSettings
            // 
            this.cmbClearSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClearSettings.FormattingEnabled = true;
            this.cmbClearSettings.Items.AddRange(new object[] {
            resources.GetString("cmbClearSettings.Items"),
            resources.GetString("cmbClearSettings.Items1"),
            resources.GetString("cmbClearSettings.Items2")});
            resources.ApplyResources(this.cmbClearSettings, "cmbClearSettings");
            this.cmbClearSettings.Name = "cmbClearSettings";
            this.cmbClearSettings.SelectedIndexChanged += new System.EventHandler(this.cmbClearSettings_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // FunctionalProfileTerminalPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.ntbMaxPrice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ntbMaxQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ntbDecimalsInNumpad);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbEventLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label7);
            this.DoubleBuffered = true;
            this.Name = "FunctionalProfileTerminalPage";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEventLog;
        private System.Windows.Forms.Label label1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.CheckBox chkAllowTransactionsWithOpenDrawer;
        private System.Windows.Forms.CheckBox chkDisplayVoidedPayments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbDecimalsInNumpad;
        private System.Windows.Forms.CheckBox chkTDUsesDenomination;
        private System.Windows.Forms.CheckBox chkBDRevUsesDenomination;
        private System.Windows.Forms.CheckBox chkBDUsesDenomination;
        private System.Windows.Forms.CheckBox chkSDRevUsesDenomination;
        private System.Windows.Forms.CheckBox chkSDUsesDenomination;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chkUsePolling;
        private System.Windows.Forms.Label lblPollingInterval;
        private NumericTextBox ntbPollingInterval;
        private System.Windows.Forms.Label lblSeconds;
        private NumericTextBox ntbMaxQuantity;
        private System.Windows.Forms.Label label3;
        private NumericTextBox ntbMaxPrice;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbClearAfter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cmbClearSettings;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkAllowSaleAndReturn;
        private System.Windows.Forms.CheckBox chkCustomerRequiredOnReturn;
        private System.Windows.Forms.CheckBox chkKeepDailyJournalOpenAfterPrintingReceipt;
    }
}
