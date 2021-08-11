using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services
{
    partial class ReceiptPreviewDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiptPreviewDialog));
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnPrint = new LSOne.Controls.TouchButton();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.textViewer = new LSOne.Controls.TextViewer();
            this.SuspendLayout();
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
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnPrint.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // textViewer
            // 
            resources.ApplyResources(this.textViewer, "textViewer");
            this.textViewer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.textViewer.Name = "textViewer";
            // 
            // ReceiptPreviewDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.textViewer);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Name = "ReceiptPreviewDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        private TouchButton btnCancel;
        private TouchButton btnPrint;
        private TouchDialogBanner touchDialogBanner1;
        private TextViewer textViewer;
    }
}