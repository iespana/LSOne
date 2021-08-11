﻿using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    partial class PayCurrencyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayCurrencyDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.touchNumPad1 = new LSOne.Controls.TouchNumPad();
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.touchScrollButtonPanel1 = new LSOne.Controls.TouchScrollButtonPanel();
            this.lblCurrency = new LSOne.Controls.DoubleLabel();
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
            // touchNumPad1
            // 
            resources.ApplyResources(this.touchNumPad1, "touchNumPad1");
            this.touchNumPad1.BackColor = System.Drawing.Color.Transparent;
            this.touchNumPad1.KeystrokeMode = true;
            this.touchNumPad1.MultiplyButtonIsZeroZero = true;
            this.touchNumPad1.Name = "touchNumPad1";
            this.touchNumPad1.TabStop = false;
            this.touchNumPad1.EnterPressed += new System.EventHandler(this.touchNumPad1_EnterPressed);
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = true;
            this.ntbAmount.AllowNegative = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.BackColor = System.Drawing.Color.White;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbAmount.HasMinValue = false;
            this.ntbAmount.MaxLength = 32767;
            this.ntbAmount.MaxValue = 999999999D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbAmount.Value = 0D;
            this.ntbAmount.ValueChanged += new System.EventHandler(this.ntbAmount_ValueChanged);
            this.ntbAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericTextBox1_KeyDown);
            this.ntbAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTextBox1_KeyPress);
            // 
            // touchScrollButtonPanel1
            // 
            resources.ApplyResources(this.touchScrollButtonPanel1, "touchScrollButtonPanel1");
            this.touchScrollButtonPanel1.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel1.ButtonHeight = 50;
            this.touchScrollButtonPanel1.Name = "touchScrollButtonPanel1";
            this.touchScrollButtonPanel1.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel1_Click);
            // 
            // lblCurrency
            // 
            this.lblCurrency.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCurrency, "lblCurrency");
            this.lblCurrency.HeaderText = "Amount in";
            this.lblCurrency.Name = "lblCurrency";
            // 
            // PayCurrencyDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblCurrency);
            this.Controls.Add(this.touchScrollButtonPanel1);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.touchNumPad1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "PayCurrencyDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner1;
        private TouchNumPad touchNumPad1;
        private ShadeNumericTextBox ntbAmount;
        private TouchScrollButtonPanel touchScrollButtonPanel1;
        private DoubleLabel lblCurrency;
    }
}