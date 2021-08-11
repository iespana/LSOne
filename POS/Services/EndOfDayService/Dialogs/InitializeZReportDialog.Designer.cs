using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class InitializeZReportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializeZReportDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.ntbGrossAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.ntbNetAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.touchKeyboard1 = new LSOne.Controls.TouchKeyboard();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnClear = new LSOne.Controls.TouchButton();
            this.tbNewZReportIDPreview = new LSOne.Controls.ShadeTextBoxTouch();
            this.ntbNewZReportID = new LSOne.Controls.ShadeNumericTextBox();
            this.SuspendLayout();
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // ntbGrossAmount
            // 
            this.ntbGrossAmount.AllowDecimal = true;
            this.ntbGrossAmount.AllowNegative = false;
            resources.ApplyResources(this.ntbGrossAmount, "ntbGrossAmount");
            this.ntbGrossAmount.BackColor = System.Drawing.Color.White;
            this.ntbGrossAmount.CultureInfo = null;
            this.ntbGrossAmount.DecimalLetters = 2;
            this.ntbGrossAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbGrossAmount.HasMinValue = false;
            this.ntbGrossAmount.MaxLength = 15;
            this.ntbGrossAmount.MaxValue = 0D;
            this.ntbGrossAmount.MinValue = 0D;
            this.ntbGrossAmount.Name = "ntbGrossAmount";
            this.ntbGrossAmount.Value = 0D;
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.Name = "lblAmount";
            // 
            // ntbNetAmount
            // 
            this.ntbNetAmount.AllowDecimal = true;
            this.ntbNetAmount.AllowNegative = false;
            resources.ApplyResources(this.ntbNetAmount, "ntbNetAmount");
            this.ntbNetAmount.BackColor = System.Drawing.Color.White;
            this.ntbNetAmount.CultureInfo = null;
            this.ntbNetAmount.DecimalLetters = 2;
            this.ntbNetAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbNetAmount.HasMinValue = false;
            this.ntbNetAmount.MaxLength = 15;
            this.ntbNetAmount.MaxValue = 0D;
            this.ntbNetAmount.MinValue = 0D;
            this.ntbNetAmount.Name = "ntbNetAmount";
            this.ntbNetAmount.Value = 0D;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.KeystrokeMode = true;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnClear.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbNewZReportIDPreview
            // 
            resources.ApplyResources(this.tbNewZReportIDPreview, "tbNewZReportIDPreview");
            this.tbNewZReportIDPreview.BackColor = System.Drawing.Color.White;
            this.tbNewZReportIDPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbNewZReportIDPreview.MaxLength = 60;
            this.tbNewZReportIDPreview.Name = "tbNewZReportIDPreview";
            this.tbNewZReportIDPreview.ReadOnly = true;
            // 
            // ntbNewZReportID
            // 
            this.ntbNewZReportID.AllowDecimal = false;
            this.ntbNewZReportID.AllowNegative = false;
            this.ntbNewZReportID.BackColor = System.Drawing.Color.White;
            this.ntbNewZReportID.CultureInfo = null;
            this.ntbNewZReportID.DecimalLetters = 2;
            resources.ApplyResources(this.ntbNewZReportID, "ntbNewZReportID");
            this.ntbNewZReportID.ForeColor = System.Drawing.Color.Black;
            this.ntbNewZReportID.HasMinValue = false;
            this.ntbNewZReportID.MaxLength = 32767;
            this.ntbNewZReportID.MaxValue = 99999999D;
            this.ntbNewZReportID.MinValue = 1D;
            this.ntbNewZReportID.Name = "ntbNewZReportID";
            this.ntbNewZReportID.ShowToolTip = false;
            this.ntbNewZReportID.Value = 1D;
            this.ntbNewZReportID.ValueChanged += new System.EventHandler(this.ntbNewZReportID_ValueChanged);
            // 
            // InitializeZReportDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ntbNewZReportID);
            this.Controls.Add(this.tbNewZReportIDPreview);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.touchKeyboard1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbNetAmount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ntbGrossAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "InitializeZReportDialog";
            this.Load += new System.EventHandler(this.InitializeZReportDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.ShadeNumericTextBox ntbGrossAmount;
        private System.Windows.Forms.Label lblAmount;
        private Controls.ShadeNumericTextBox ntbNetAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.TouchKeyboard touchKeyboard1;
        private Controls.TouchButton btnOk;
        private Controls.TouchButton btnCancel;
        private Controls.TouchButton btnClear;
        private Controls.ShadeTextBoxTouch tbNewZReportIDPreview;
        private Controls.ShadeNumericTextBox ntbNewZReportID;
    }
}