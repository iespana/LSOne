using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class RemoveDiscountsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveDiscountsDialog));
            this.tdbBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnOkYesRetry = new LSOne.Controls.TouchButton();
            this.btnCancelAbort = new LSOne.Controls.TouchButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tdbBanner
            // 
            this.tdbBanner.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdbBanner, "tdbBanner");
            this.tdbBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbBanner.Name = "tdbBanner";
            this.tdbBanner.TabStop = false;
            // 
            // btnOkYesRetry
            // 
            resources.ApplyResources(this.btnOkYesRetry, "btnOkYesRetry");
            this.btnOkYesRetry.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOkYesRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnOkYesRetry.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOkYesRetry.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkYesRetry.ForeColor = System.Drawing.Color.White;
            this.btnOkYesRetry.Name = "btnOkYesRetry";
            this.btnOkYesRetry.UseVisualStyleBackColor = true;
            // 
            // btnCancelAbort
            // 
            resources.ApplyResources(this.btnCancelAbort, "btnCancelAbort");
            this.btnCancelAbort.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancelAbort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancelAbort.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancelAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelAbort.ForeColor = System.Drawing.Color.White;
            this.btnCancelAbort.Name = "btnCancelAbort";
            this.btnCancelAbort.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblMessage.Name = "lblMessage";
            // 
            // RemoveDiscountsDialog
            // 
            this.AcceptButton = this.btnOkYesRetry;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnCancelAbort);
            this.Controls.Add(this.btnOkYesRetry);
            this.Controls.Add(this.tdbBanner);
            this.Name = "RemoveDiscountsDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchDialogBanner tdbBanner;
        private TouchButton btnOkYesRetry;
        private TouchButton btnCancelAbort;
        private System.Windows.Forms.Label lblMessage;
    }
}