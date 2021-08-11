using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class ErrorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorDialog));
            this.tdbBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnClose = new LSOne.Controls.TouchButton();
            this.btnViewDetails = new LSOne.Controls.TouchButton();
            this.btnCopyToClipboard = new LSOne.Controls.TouchButton();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.txtDetails = new LSOne.Controls.TextViewer();
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
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnClose.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnViewDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnViewDetails.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnViewDetails.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnViewDetails, "btnViewDetails");
            this.btnViewDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.UseVisualStyleBackColor = false;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // btnCopyToClipboard
            // 
            resources.ApplyResources(this.btnCopyToClipboard, "btnCopyToClipboard");
            this.btnCopyToClipboard.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCopyToClipboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnCopyToClipboard.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnCopyToClipboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = false;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtMessage, "txtMessage");
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            // 
            // txtDetails
            // 
            resources.ApplyResources(this.txtDetails, "txtDetails");
            this.txtDetails.BackColor = System.Drawing.Color.White;
            this.txtDetails.Name = "txtDetails";
            // 
            // ErrorDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tdbBanner);
            this.Name = "ErrorDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner tdbBanner;
        private LSOne.Controls.TouchButton btnClose;
        private LSOne.Controls.TouchButton btnViewDetails;
        private LSOne.Controls.TouchButton btnCopyToClipboard;
        private System.Windows.Forms.RichTextBox txtMessage;
        private LSOne.Controls.TextViewer txtDetails;
    }
}