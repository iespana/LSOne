using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    partial class NumpadAmountQtyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NumpadAmountQtyDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.touchNumPad = new LSOne.Controls.TouchNumPad();
            this.ntbValue = new LSOne.Controls.ShadeNumericTextBox();
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
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // touchNumPad
            // 
            resources.ApplyResources(this.touchNumPad, "touchNumPad");
            this.touchNumPad.BackColor = System.Drawing.Color.Transparent;
            this.touchNumPad.KeystrokeMode = true;
            this.touchNumPad.MultiplyButtonIsZeroZero = true;
            this.touchNumPad.Name = "touchNumPad";
            this.touchNumPad.TabStop = false;
            this.touchNumPad.EnterPressed += new System.EventHandler(this.touchNumPad_EnterPressed);
            this.touchNumPad.ClearPressed += new System.EventHandler(this.touchNumPad_ClearPressed);
            // 
            // ntbValue
            // 
            this.ntbValue.AllowDecimal = true;
            this.ntbValue.AllowNegative = false;
            resources.ApplyResources(this.ntbValue, "ntbValue");
            this.ntbValue.BackColor = System.Drawing.Color.White;
            this.ntbValue.CultureInfo = null;
            this.ntbValue.DecimalLetters = 2;
            this.ntbValue.ForeColor = System.Drawing.Color.Black;
            this.ntbValue.HasMinValue = false;
            this.ntbValue.MaxLength = 32767;
            this.ntbValue.MaxValue = 2100000000D;
            this.ntbValue.MinValue = 0D;
            this.ntbValue.Name = "ntbValue";
            this.ntbValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbValue.Value = 0D;
            this.ntbValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ntbValue_KeyDown);
            // 
            // NumpadAmountQtyDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ntbValue);
            this.Controls.Add(this.touchNumPad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "NumpadAmountQtyDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner1;
        private LSOne.Controls.TouchButton btnCancel;
        private LSOne.Controls.TouchButton btnOk;
        private TouchNumPad touchNumPad;
        private ShadeNumericTextBox ntbValue;
    }
}