using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    partial class TerminalSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalSettingsPage));
            this.label7 = new System.Windows.Forms.Label();
            this.chkAutomaticLogout = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkOpenDrawer = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkExitAfterTransaction = new System.Windows.Forms.CheckBox();
            this.ntbAutomaticLogoutTime = new LSOne.Controls.NumericTextBox();
            this.ntbAutomaticLockTime = new LSOne.Controls.NumericTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkAutomaticLock = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbReceiptNumberSeries = new LSOne.Controls.DualDataComboBox();
            this.lblFormInfo1 = new System.Windows.Forms.Label();
            this.txtFormInfo1 = new System.Windows.Forms.TextBox();
            this.txtFormInfo2 = new System.Windows.Forms.TextBox();
            this.lblFormInfo2 = new System.Windows.Forms.Label();
            this.txtFormInfo3 = new System.Windows.Forms.TextBox();
            this.lblFormInfo3 = new System.Windows.Forms.Label();
            this.txtFormInfo4 = new System.Windows.Forms.TextBox();
            this.lblFormInfo4 = new System.Windows.Forms.Label();
            this.dtLastActivated = new System.Windows.Forms.DateTimePicker();
            this.lblLastActivated = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkIsActivated = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkAutomaticLogout
            // 
            resources.ApplyResources(this.chkAutomaticLogout, "chkAutomaticLogout");
            this.chkAutomaticLogout.Name = "chkAutomaticLogout";
            this.chkAutomaticLogout.UseVisualStyleBackColor = true;
            this.chkAutomaticLogout.CheckedChanged += new System.EventHandler(this.chkAutomaticLogout_CheckedChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkOpenDrawer
            // 
            resources.ApplyResources(this.chkOpenDrawer, "chkOpenDrawer");
            this.chkOpenDrawer.Name = "chkOpenDrawer";
            this.chkOpenDrawer.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkExitAfterTransaction
            // 
            resources.ApplyResources(this.chkExitAfterTransaction, "chkExitAfterTransaction");
            this.chkExitAfterTransaction.Name = "chkExitAfterTransaction";
            this.chkExitAfterTransaction.UseVisualStyleBackColor = true;
            // 
            // ntbAutomaticLogoutTime
            // 
            this.ntbAutomaticLogoutTime.AllowDecimal = false;
            this.ntbAutomaticLogoutTime.AllowNegative = false;
            this.ntbAutomaticLogoutTime.CultureInfo = null;
            this.ntbAutomaticLogoutTime.DecimalLetters = 2;
            this.ntbAutomaticLogoutTime.ForeColor = System.Drawing.Color.Black;
            this.ntbAutomaticLogoutTime.HasMinValue = false;
            resources.ApplyResources(this.ntbAutomaticLogoutTime, "ntbAutomaticLogoutTime");
            this.ntbAutomaticLogoutTime.MaxValue = 1440D;
            this.ntbAutomaticLogoutTime.MinValue = 0D;
            this.ntbAutomaticLogoutTime.Name = "ntbAutomaticLogoutTime";
            this.ntbAutomaticLogoutTime.Value = 0D;
            // 
            // ntbAutomaticLockTime
            // 
            this.ntbAutomaticLockTime.AllowDecimal = false;
            this.ntbAutomaticLockTime.AllowNegative = false;
            this.ntbAutomaticLockTime.CultureInfo = null;
            this.ntbAutomaticLockTime.DecimalLetters = 2;
            this.ntbAutomaticLockTime.ForeColor = System.Drawing.Color.Black;
            this.ntbAutomaticLockTime.HasMinValue = false;
            resources.ApplyResources(this.ntbAutomaticLockTime, "ntbAutomaticLockTime");
            this.ntbAutomaticLockTime.MaxValue = 1440D;
            this.ntbAutomaticLockTime.MinValue = 0D;
            this.ntbAutomaticLockTime.Name = "ntbAutomaticLockTime";
            this.ntbAutomaticLockTime.Value = 0D;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkAutomaticLock
            // 
            resources.ApplyResources(this.chkAutomaticLock, "chkAutomaticLock");
            this.chkAutomaticLock.Name = "chkAutomaticLock";
            this.chkAutomaticLock.UseVisualStyleBackColor = true;
            this.chkAutomaticLock.CheckedChanged += new System.EventHandler(this.chkAutomaticLock_CheckedChanged);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmbReceiptNumberSeries
            // 
            this.cmbReceiptNumberSeries.AddList = null;
            this.cmbReceiptNumberSeries.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbReceiptNumberSeries, "cmbReceiptNumberSeries");
            this.cmbReceiptNumberSeries.MaxLength = 32767;
            this.cmbReceiptNumberSeries.Name = "cmbReceiptNumberSeries";
            this.cmbReceiptNumberSeries.NoChangeAllowed = false;
            this.cmbReceiptNumberSeries.OnlyDisplayID = false;
            this.cmbReceiptNumberSeries.RemoveList = null;
            this.cmbReceiptNumberSeries.RowHeight = ((short)(22));
            this.cmbReceiptNumberSeries.SecondaryData = null;
            this.cmbReceiptNumberSeries.SelectedData = null;
            this.cmbReceiptNumberSeries.SelectedDataID = null;
            this.cmbReceiptNumberSeries.SelectionList = null;
            this.cmbReceiptNumberSeries.SkipIDColumn = true;
            this.cmbReceiptNumberSeries.RequestData += new System.EventHandler(this.cmbReceiptNumberSeries_RequestData);
            // 
            // lblFormInfo1
            // 
            this.lblFormInfo1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo1, "lblFormInfo1");
            this.lblFormInfo1.Name = "lblFormInfo1";
            // 
            // txtFormInfo1
            // 
            resources.ApplyResources(this.txtFormInfo1, "txtFormInfo1");
            this.txtFormInfo1.Name = "txtFormInfo1";
            // 
            // txtFormInfo2
            // 
            resources.ApplyResources(this.txtFormInfo2, "txtFormInfo2");
            this.txtFormInfo2.Name = "txtFormInfo2";
            // 
            // lblFormInfo2
            // 
            this.lblFormInfo2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo2, "lblFormInfo2");
            this.lblFormInfo2.Name = "lblFormInfo2";
            // 
            // txtFormInfo3
            // 
            resources.ApplyResources(this.txtFormInfo3, "txtFormInfo3");
            this.txtFormInfo3.Name = "txtFormInfo3";
            // 
            // lblFormInfo3
            // 
            this.lblFormInfo3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo3, "lblFormInfo3");
            this.lblFormInfo3.Name = "lblFormInfo3";
            // 
            // txtFormInfo4
            // 
            resources.ApplyResources(this.txtFormInfo4, "txtFormInfo4");
            this.txtFormInfo4.Name = "txtFormInfo4";
            // 
            // lblFormInfo4
            // 
            this.lblFormInfo4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo4, "lblFormInfo4");
            this.lblFormInfo4.Name = "lblFormInfo4";
            // 
            // dtLastActivated
            // 
            this.dtLastActivated.Checked = false;
            resources.ApplyResources(this.dtLastActivated, "dtLastActivated");
            this.dtLastActivated.Name = "dtLastActivated";
            this.dtLastActivated.ShowCheckBox = true;
            this.dtLastActivated.TabStop = false;
            // 
            // lblLastActivated
            // 
            this.lblLastActivated.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLastActivated, "lblLastActivated");
            this.lblLastActivated.Name = "lblLastActivated";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkIsActivated
            // 
            resources.ApplyResources(this.chkIsActivated, "chkIsActivated");
            this.chkIsActivated.Name = "chkIsActivated";
            this.chkIsActivated.UseVisualStyleBackColor = true;
            this.chkIsActivated.CheckedChanged += new System.EventHandler(this.chkIsActivated_CheckedChanged);
            // 
            // TerminalSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkIsActivated);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtLastActivated);
            this.Controls.Add(this.lblLastActivated);
            this.Controls.Add(this.txtFormInfo4);
            this.Controls.Add(this.lblFormInfo4);
            this.Controls.Add(this.txtFormInfo3);
            this.Controls.Add(this.lblFormInfo3);
            this.Controls.Add(this.txtFormInfo2);
            this.Controls.Add(this.lblFormInfo2);
            this.Controls.Add(this.txtFormInfo1);
            this.Controls.Add(this.lblFormInfo1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbReceiptNumberSeries);
            this.Controls.Add(this.ntbAutomaticLockTime);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chkAutomaticLock);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ntbAutomaticLogoutTime);
            this.Controls.Add(this.chkExitAfterTransaction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkOpenDrawer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkAutomaticLogout);
            this.Controls.Add(this.label4);
            this.DoubleBuffered = true;
            this.Name = "TerminalSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkAutomaticLogout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkOpenDrawer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkExitAfterTransaction;
        private NumericTextBox ntbAutomaticLogoutTime;
        private NumericTextBox ntbAutomaticLockTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkAutomaticLock;
        private System.Windows.Forms.Label label11;
        private DualDataComboBox cmbReceiptNumberSeries;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblFormInfo1;
        private System.Windows.Forms.TextBox txtFormInfo1;
        private System.Windows.Forms.TextBox txtFormInfo2;
        private System.Windows.Forms.Label lblFormInfo2;
        private System.Windows.Forms.TextBox txtFormInfo3;
        private System.Windows.Forms.Label lblFormInfo3;
        private System.Windows.Forms.TextBox txtFormInfo4;
        private System.Windows.Forms.Label lblFormInfo4;
        private System.Windows.Forms.DateTimePicker dtLastActivated;
        private System.Windows.Forms.Label lblLastActivated;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkIsActivated;
    }
}
