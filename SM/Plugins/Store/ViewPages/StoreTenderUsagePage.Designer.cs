using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreTenderUsagePage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTenderUsagePage));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkAllowSafeDrop = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkAllowBankDrop = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkAllowFloat = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkCountingRequired = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.gbLimits = new System.Windows.Forms.GroupBox();
            this.ntbMaximumAmount = new LSOne.Controls.NumericTextBox();
            this.lblMaximumAmount = new System.Windows.Forms.Label();
            this.chkAmountInPOSLimiting = new System.Windows.Forms.CheckBox();
            this.lblLimitEnabled = new System.Windows.Forms.Label();
            this.chkAllowNegativePaymentAmounts = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkPaymentTypeCanBeVoided = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.gbLimits.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Coins.png");
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.chkAllowSafeDrop);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.chkAllowBankDrop);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.chkAllowFloat);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.chkCountingRequired);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // chkAllowSafeDrop
            // 
            resources.ApplyResources(this.chkAllowSafeDrop, "chkAllowSafeDrop");
            this.chkAllowSafeDrop.Name = "chkAllowSafeDrop";
            this.chkAllowSafeDrop.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // chkAllowBankDrop
            // 
            resources.ApplyResources(this.chkAllowBankDrop, "chkAllowBankDrop");
            this.chkAllowBankDrop.Name = "chkAllowBankDrop";
            this.chkAllowBankDrop.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // chkAllowFloat
            // 
            resources.ApplyResources(this.chkAllowFloat, "chkAllowFloat");
            this.chkAllowFloat.Name = "chkAllowFloat";
            this.chkAllowFloat.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkCountingRequired
            // 
            resources.ApplyResources(this.chkCountingRequired, "chkCountingRequired");
            this.chkCountingRequired.Name = "chkCountingRequired";
            this.chkCountingRequired.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // gbLimits
            // 
            resources.ApplyResources(this.gbLimits, "gbLimits");
            this.gbLimits.Controls.Add(this.chkAllowNegativePaymentAmounts);
            this.gbLimits.Controls.Add(this.label6);
            this.gbLimits.Controls.Add(this.chkPaymentTypeCanBeVoided);
            this.gbLimits.Controls.Add(this.label5);
            this.gbLimits.Controls.Add(this.ntbMaximumAmount);
            this.gbLimits.Controls.Add(this.lblMaximumAmount);
            this.gbLimits.Controls.Add(this.chkAmountInPOSLimiting);
            this.gbLimits.Controls.Add(this.lblLimitEnabled);
            this.gbLimits.Name = "gbLimits";
            this.gbLimits.TabStop = false;
            // 
            // ntbMaximumAmount
            // 
            this.ntbMaximumAmount.AllowDecimal = false;
            this.ntbMaximumAmount.AllowNegative = false;
            this.ntbMaximumAmount.CultureInfo = null;
            this.ntbMaximumAmount.DecimalLetters = 2;
            this.ntbMaximumAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbMaximumAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumAmount, "ntbMaximumAmount");
            this.ntbMaximumAmount.MaxValue = 0D;
            this.ntbMaximumAmount.MinValue = 0D;
            this.ntbMaximumAmount.Name = "ntbMaximumAmount";
            this.ntbMaximumAmount.Value = 0D;
            // 
            // lblMaximumAmount
            // 
            this.lblMaximumAmount.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaximumAmount, "lblMaximumAmount");
            this.lblMaximumAmount.Name = "lblMaximumAmount";
            // 
            // chkAmountInPOSLimiting
            // 
            resources.ApplyResources(this.chkAmountInPOSLimiting, "chkAmountInPOSLimiting");
            this.chkAmountInPOSLimiting.Name = "chkAmountInPOSLimiting";
            this.chkAmountInPOSLimiting.UseVisualStyleBackColor = true;
            this.chkAmountInPOSLimiting.CheckedChanged += new System.EventHandler(this.chkAmountInPOSLimiting_CheckedChanged);
            // 
            // lblLimitEnabled
            // 
            this.lblLimitEnabled.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLimitEnabled, "lblLimitEnabled");
            this.lblLimitEnabled.Name = "lblLimitEnabled";
            // 
            // chkAllowNegativePaymentAmounts
            // 
            resources.ApplyResources(this.chkAllowNegativePaymentAmounts, "chkAllowNegativePaymentAmounts");
            this.chkAllowNegativePaymentAmounts.Name = "chkAllowNegativePaymentAmounts";
            this.chkAllowNegativePaymentAmounts.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkPaymentTypeCanBeVoided
            // 
            resources.ApplyResources(this.chkPaymentTypeCanBeVoided, "chkPaymentTypeCanBeVoided");
            this.chkPaymentTypeCanBeVoided.Name = "chkPaymentTypeCanBeVoided";
            this.chkPaymentTypeCanBeVoided.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // StoreTenderUsagePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gbLimits);
            this.Controls.Add(this.groupBox4);
            this.DoubleBuffered = true;
            this.Name = "StoreTenderUsagePage";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gbLimits.ResumeLayout(false);
            this.gbLimits.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkAllowSafeDrop;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkAllowBankDrop;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkAllowFloat;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkCountingRequired;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox gbLimits;
        private System.Windows.Forms.CheckBox chkAmountInPOSLimiting;
        private System.Windows.Forms.Label lblLimitEnabled;
        private System.Windows.Forms.Label lblMaximumAmount;
        private NumericTextBox ntbMaximumAmount;
        private System.Windows.Forms.CheckBox chkAllowNegativePaymentAmounts;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkPaymentTypeCanBeVoided;
        private System.Windows.Forms.Label label5;
    }
}
