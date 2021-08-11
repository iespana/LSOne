using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    partial class OverrideMessageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverrideMessageDialog));
            this.tdbBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnOverride = new LSOne.Controls.TouchButton();
            this.btnKeepCurrent = new LSOne.Controls.TouchButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tdbBanner
            // 
            resources.ApplyResources(this.tdbBanner, "tdbBanner");
            this.tdbBanner.BackColor = System.Drawing.Color.White;
            this.tdbBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Attention;
            this.tdbBanner.Name = "tdbBanner";
            this.tdbBanner.TabStop = false;
            // 
            // btnOverride
            // 
            resources.ApplyResources(this.btnOverride, "btnOverride");
            this.btnOverride.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOverride.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOverride.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOverride.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOverride.ForeColor = System.Drawing.Color.White;
            this.btnOverride.Name = "btnOverride";
            this.btnOverride.UseVisualStyleBackColor = false;
            // 
            // btnKeepCurrent
            // 
            resources.ApplyResources(this.btnKeepCurrent, "btnKeepCurrent");
            this.btnKeepCurrent.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnKeepCurrent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnKeepCurrent.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnKeepCurrent.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnKeepCurrent.ForeColor = System.Drawing.Color.White;
            this.btnKeepCurrent.Name = "btnKeepCurrent";
            this.btnKeepCurrent.UseVisualStyleBackColor = false;
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblMessage.Name = "lblMessage";
            // 
            // OverrideMessageDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnKeepCurrent);
            this.Controls.Add(this.btnOverride);
            this.Controls.Add(this.tdbBanner);
            this.Name = "OverrideMessageDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private Services.DialogPanels.DialogPageBase dialogPageBase1;
        private TouchDialogBanner tdbBanner;
        private TouchButton btnOverride;
        private TouchButton btnKeepCurrent;
        private System.Windows.Forms.Label lblMessage;
    }
}