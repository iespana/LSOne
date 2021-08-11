using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.EFT.Common.Touch
{
    partial class PayCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayCardDialog));
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.touchScrollButtonPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.lblExpireDate = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.tbCardNumber = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbExpireDate = new LSOne.Controls.ShadeTextBoxTouch();
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.touchNumPad = new LSOne.Controls.TouchNumPad();
            this.tbCVV = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblCVV = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // touchScrollButtonPanel
            // 
            resources.ApplyResources(this.touchScrollButtonPanel, "touchScrollButtonPanel");
            this.touchScrollButtonPanel.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel.ButtonHeight = 50;
            this.touchScrollButtonPanel.Name = "touchScrollButtonPanel";
            this.touchScrollButtonPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel1_Click);
            // 
            // lblExpireDate
            // 
            resources.ApplyResources(this.lblExpireDate, "lblExpireDate");
            this.lblExpireDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblExpireDate.Name = "lblExpireDate";
            // 
            // lblCardNumber
            // 
            resources.ApplyResources(this.lblCardNumber, "lblCardNumber");
            this.lblCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCardNumber.Name = "lblCardNumber";
            // 
            // tbCardNumber
            // 
            this.tbCardNumber.BackColor = System.Drawing.Color.White;
            this.tbCardNumber.EndCharacter = null;
            this.tbCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCardNumber.LastTrack = null;
            resources.ApplyResources(this.tbCardNumber, "tbCardNumber");
            this.tbCardNumber.ManualEntryOfTrack = true;
            this.tbCardNumber.MaxLength = 32767;
            this.tbCardNumber.Name = "tbCardNumber";
            this.tbCardNumber.NumericOnly = false;
            this.tbCardNumber.Seperator = null;
            this.tbCardNumber.StartCharacter = null;
            this.tbCardNumber.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbCardNumber.TextChanged += new System.EventHandler(this.tbCardNumber_TextChanged);
            // 
            // tbExpireDate
            // 
            this.tbExpireDate.BackColor = System.Drawing.Color.White;
            this.tbExpireDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbExpireDate, "tbExpireDate");
            this.tbExpireDate.MaxLength = 5;
            this.tbExpireDate.Name = "tbExpireDate";
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = false;
            this.ntbAmount.AllowNegative = false;
            this.ntbAmount.BackColor = System.Drawing.SystemColors.Window;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.MaxLength = 32767;
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbAmount.Value = 0D;
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAmount.Name = "lblAmount";
            // 
            // touchNumPad
            // 
            resources.ApplyResources(this.touchNumPad, "touchNumPad");
            this.touchNumPad.BackColor = System.Drawing.Color.Transparent;
            this.touchNumPad.KeystrokeMode = true;
            this.touchNumPad.MultiplyButtonIsZeroZero = true;
            this.touchNumPad.Name = "touchNumPad";
            this.touchNumPad.TabStop = false;
            this.touchNumPad.EnterPressed += new System.EventHandler(this.touchNumPad1_EnterPressed);
            this.touchNumPad.ClearPressed += new System.EventHandler(this.touchNumPad1_ClearPressed);
            this.touchNumPad.TouchKeyPressed += new LSOne.Controls.TouchKeyEventHandler(this.touchNumPad1_TouchKeyPressed);
            this.touchNumPad.PlusMinusPressed += new System.EventHandler(this.touchNumPad1_PlusMinusPressed);
            // 
            // tbCVV
            // 
            this.tbCVV.BackColor = System.Drawing.Color.White;
            this.tbCVV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbCVV, "tbCVV");
            this.tbCVV.MaxLength = 4;
            this.tbCVV.Name = "tbCVV";
            // 
            // lblCVV
            // 
            resources.ApplyResources(this.lblCVV, "lblCVV");
            this.lblCVV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCVV.Name = "lblCVV";
            // 
            // PayCardDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbCVV);
            this.Controls.Add(this.lblCVV);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.tbExpireDate);
            this.Controls.Add(this.tbCardNumber);
            this.Controls.Add(this.lblExpireDate);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.touchScrollButtonPanel);
            this.Controls.Add(this.touchNumPad);
            this.Controls.Add(this.touchDialogBanner);
            this.Name = "PayCardDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PayCardDialog_FormClosed);
            this.Load += new System.EventHandler(this.PayCardDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner;
        private TouchScrollButtonPanel touchScrollButtonPanel;
        private System.Windows.Forms.Label lblExpireDate;
        private System.Windows.Forms.Label lblCardNumber;
        private MSRTextBoxTouch tbCardNumber;
        private LSOne.Controls.ShadeTextBoxTouch tbExpireDate;
        private ShadeNumericTextBox ntbAmount;
        private System.Windows.Forms.Label lblAmount;
        private TouchNumPad touchNumPad;
        private LSOne.Controls.ShadeTextBoxTouch tbCVV;
        private System.Windows.Forms.Label lblCVV;
    }
}