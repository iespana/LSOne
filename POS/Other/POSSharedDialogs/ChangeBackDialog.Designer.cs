using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    partial class ChangeBackDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeBackDialog));
            this.drawerTimer = new System.Windows.Forms.Timer(this.components);
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.btnClose = new LSOne.Controls.TouchButton();
            this.lblSale = new LSOne.Controls.DoubleLabel();
            this.lblPaid = new LSOne.Controls.DoubleLabel();
            this.lblRounded = new LSOne.Controls.DoubleLabel();
            this.lblMoneyBackValue = new System.Windows.Forms.Label();
            this.lblMoneyBack = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // drawerTimer
            // 
            this.drawerTimer.Enabled = true;
            this.drawerTimer.Interval = 350;
            this.drawerTimer.Tick += new System.EventHandler(this.timerTick);
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnClose.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // lblSale
            // 
            this.lblSale.BackColor = System.Drawing.Color.White;
            this.lblSale.HeaderText = "Sale";
            resources.ApplyResources(this.lblSale, "lblSale");
            this.lblSale.Name = "lblSale";
            // 
            // lblPaid
            // 
            this.lblPaid.BackColor = System.Drawing.Color.White;
            this.lblPaid.HeaderText = "Paid";
            resources.ApplyResources(this.lblPaid, "lblPaid");
            this.lblPaid.Name = "lblPaid";
            // 
            // lblRounded
            // 
            this.lblRounded.BackColor = System.Drawing.Color.White;
            this.lblRounded.HeaderText = "Rounded";
            resources.ApplyResources(this.lblRounded, "lblRounded");
            this.lblRounded.Name = "lblRounded";
            // 
            // lblMoneyBackValue
            // 
            resources.ApplyResources(this.lblMoneyBackValue, "lblMoneyBackValue");
            this.lblMoneyBackValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblMoneyBackValue.Name = "lblMoneyBackValue";
            // 
            // lblMoneyBack
            // 
            resources.ApplyResources(this.lblMoneyBack, "lblMoneyBack");
            this.lblMoneyBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.lblMoneyBack.Name = "lblMoneyBack";
            // 
            // ChangeBackDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMoneyBackValue);
            this.Controls.Add(this.lblMoneyBack);
            this.Controls.Add(this.lblRounded);
            this.Controls.Add(this.lblPaid);
            this.Controls.Add(this.lblSale);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "ChangeBackDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChangeBackDialog_FormClosing);
            this.Load += new System.EventHandler(this.ChangeBackDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer drawerTimer;
        private TouchDialogBanner touchDialogBanner1;
        private TouchButton btnClose;
        private DoubleLabel lblSale;
        private DoubleLabel lblPaid;
        private DoubleLabel lblRounded;
        private System.Windows.Forms.Label lblMoneyBackValue;
        private System.Windows.Forms.Label lblMoneyBack;
    }
}