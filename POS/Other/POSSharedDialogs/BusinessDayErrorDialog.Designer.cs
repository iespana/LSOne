using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    partial class BusinessDayErrorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BusinessDayErrorDialog));
            this.tdbBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnNo = new LSOne.Controls.TouchButton();
            this.btnYes = new LSOne.Controls.TouchButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblErrorText = new System.Windows.Forms.Label();
            this.lblContinueQuestion = new System.Windows.Forms.Label();
            this.lblBusinessDay = new System.Windows.Forms.Label();
            this.lblSystemDay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tdbBanner
            // 
            resources.ApplyResources(this.tdbBanner, "tdbBanner");
            this.tdbBanner.BackColor = System.Drawing.Color.White;
            this.tdbBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.ErrorWarning;
            this.tdbBanner.Name = "tdbBanner";
            this.tdbBanner.TabStop = false;
            // 
            // btnNo
            // 
            resources.ApplyResources(this.btnNo, "btnNo");
            this.btnNo.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnNo.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Name = "btnNo";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // btnYes
            // 
            resources.ApplyResources(this.btnYes, "btnYes");
            this.btnYes.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnYes.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Name = "btnYes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.Name = "lblMessage";
            // 
            // lblErrorText
            // 
            resources.ApplyResources(this.lblErrorText, "lblErrorText");
            this.lblErrorText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblErrorText.Name = "lblErrorText";
            // 
            // lblContinueQuestion
            // 
            resources.ApplyResources(this.lblContinueQuestion, "lblContinueQuestion");
            this.lblContinueQuestion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblContinueQuestion.Name = "lblContinueQuestion";
            // 
            // lblBusinessDay
            // 
            resources.ApplyResources(this.lblBusinessDay, "lblBusinessDay");
            this.lblBusinessDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblBusinessDay.Name = "lblBusinessDay";
            // 
            // lblSystemDay
            // 
            resources.ApplyResources(this.lblSystemDay, "lblSystemDay");
            this.lblSystemDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblSystemDay.Name = "lblSystemDay";
            // 
            // BusinessDayErrorDialog
            // 
            this.AcceptButton = this.btnYes;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSystemDay);
            this.Controls.Add(this.lblBusinessDay);
            this.Controls.Add(this.lblContinueQuestion);
            this.Controls.Add(this.lblErrorText);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.tdbBanner);
            this.Name = "BusinessDayErrorDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchDialogBanner tdbBanner;
        private TouchButton btnNo;
        private TouchButton btnYes;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblErrorText;
        private System.Windows.Forms.Label lblContinueQuestion;
        private System.Windows.Forms.Label lblBusinessDay;
        private System.Windows.Forms.Label lblSystemDay;
    }
}