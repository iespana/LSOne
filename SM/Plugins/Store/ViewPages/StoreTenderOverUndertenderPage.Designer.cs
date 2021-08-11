using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreTenderOverUndertenderPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTenderOverUndertenderPage));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkPaymentTypeCanBePartOfSplitPayment = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupPanel2 = new GroupPanel();
            this.ntbUndertenderAmount = new NumericTextBox();
            this.lblUndertenderAmount = new System.Windows.Forms.Label();
            this.chkUnderTenderAllowed = new System.Windows.Forms.CheckBox();
            this.groupPanel1 = new GroupPanel();
            this.ntbMaxOverTender = new NumericTextBox();
            this.lblMaxOverTender = new System.Windows.Forms.Label();
            this.chkOverTenderAllowed = new System.Windows.Forms.CheckBox();
            this.groupPanel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkPaymentTypeCanBePartOfSplitPayment
            // 
            resources.ApplyResources(this.chkPaymentTypeCanBePartOfSplitPayment, "chkPaymentTypeCanBePartOfSplitPayment");
            this.chkPaymentTypeCanBePartOfSplitPayment.Name = "chkPaymentTypeCanBePartOfSplitPayment";
            this.chkPaymentTypeCanBePartOfSplitPayment.UseVisualStyleBackColor = true;
            this.chkPaymentTypeCanBePartOfSplitPayment.CheckedChanged += new System.EventHandler(this.chkPaymentTypeCanBePartOfSplitPayment_CheckedChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupPanel2
            // 
            this.groupPanel2.Controls.Add(this.ntbUndertenderAmount);
            this.groupPanel2.Controls.Add(this.lblUndertenderAmount);
            this.groupPanel2.Controls.Add(this.chkUnderTenderAllowed);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // ntbUndertenderAmount
            // 
            this.ntbUndertenderAmount.AllowDecimal = false;
            this.ntbUndertenderAmount.AllowNegative = false;
            this.ntbUndertenderAmount.CultureInfo = null;
            this.ntbUndertenderAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbUndertenderAmount, "ntbUndertenderAmount");
            this.ntbUndertenderAmount.HasMinValue = false;
            this.ntbUndertenderAmount.MaxValue = 9999999999999D;
            this.ntbUndertenderAmount.MinValue = 0D;
            this.ntbUndertenderAmount.Name = "ntbUndertenderAmount";
            this.ntbUndertenderAmount.Value = 0D;
            // 
            // lblUndertenderAmount
            // 
            this.lblUndertenderAmount.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUndertenderAmount, "lblUndertenderAmount");
            this.lblUndertenderAmount.Name = "lblUndertenderAmount";
            // 
            // chkUnderTenderAllowed
            // 
            resources.ApplyResources(this.chkUnderTenderAllowed, "chkUnderTenderAllowed");
            this.chkUnderTenderAllowed.Name = "chkUnderTenderAllowed";
            this.chkUnderTenderAllowed.UseVisualStyleBackColor = true;
            this.chkUnderTenderAllowed.CheckedChanged += new System.EventHandler(this.chkUnderTenderAllowed_CheckedChanged);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.ntbMaxOverTender);
            this.groupPanel1.Controls.Add(this.lblMaxOverTender);
            this.groupPanel1.Controls.Add(this.chkOverTenderAllowed);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // ntbMaxOverTender
            // 
            this.ntbMaxOverTender.AllowDecimal = false;
            this.ntbMaxOverTender.AllowNegative = false;
            this.ntbMaxOverTender.CultureInfo = null;
            this.ntbMaxOverTender.DecimalLetters = 2;
            resources.ApplyResources(this.ntbMaxOverTender, "ntbMaxOverTender");
            this.ntbMaxOverTender.HasMinValue = false;
            this.ntbMaxOverTender.MaxValue = 9999999999999D;
            this.ntbMaxOverTender.MinValue = 0D;
            this.ntbMaxOverTender.Name = "ntbMaxOverTender";
            this.ntbMaxOverTender.Value = 0D;
            // 
            // lblMaxOverTender
            // 
            this.lblMaxOverTender.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaxOverTender, "lblMaxOverTender");
            this.lblMaxOverTender.Name = "lblMaxOverTender";
            // 
            // chkOverTenderAllowed
            // 
            resources.ApplyResources(this.chkOverTenderAllowed, "chkOverTenderAllowed");
            this.chkOverTenderAllowed.Name = "chkOverTenderAllowed";
            this.chkOverTenderAllowed.UseVisualStyleBackColor = true;
            this.chkOverTenderAllowed.CheckedChanged += new System.EventHandler(this.chkOverTenderAllowed_CheckedChanged);
            // 
            // StoreTenderOverUndertenderPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkPaymentTypeCanBePartOfSplitPayment);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "StoreTenderOverUndertenderPage";
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblMaxOverTender;
        private System.Windows.Forms.CheckBox chkOverTenderAllowed;
        private System.Windows.Forms.Label label3;
        private NumericTextBox ntbMaxOverTender;
        private GroupPanel groupPanel2;
        private NumericTextBox ntbUndertenderAmount;
        private System.Windows.Forms.Label lblUndertenderAmount;
        private System.Windows.Forms.CheckBox chkUnderTenderAllowed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkPaymentTypeCanBePartOfSplitPayment;
        private System.Windows.Forms.Label label6;
    }
}
