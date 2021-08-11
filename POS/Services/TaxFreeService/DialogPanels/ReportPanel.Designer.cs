using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.DialogPanels
{
    partial class ReportPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportPanel));
            this.tdbHeader = new LSOne.Controls.TouchDialogBanner();
            this.tbBooking = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblBooking = new System.Windows.Forms.Label();
            this.lblRunning = new System.Windows.Forms.Label();
            this.tbRunning = new LSOne.Controls.ShadeTextBoxTouch();
            this.errorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // tdbHeader
            // 
            this.tdbHeader.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdbHeader, "tdbHeader");
            this.tdbHeader.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbHeader.Name = "tdbHeader";
            this.tdbHeader.TabStop = false;
            // 
            // tbBooking
            // 
            this.tbBooking.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbBooking, "tbBooking");
            this.tbBooking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbBooking.MaxLength = 60;
            this.tbBooking.Name = "tbBooking";
            // 
            // lblBooking
            // 
            resources.ApplyResources(this.lblBooking, "lblBooking");
            this.lblBooking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblBooking.Name = "lblBooking";
            // 
            // lblRunning
            // 
            resources.ApplyResources(this.lblRunning, "lblRunning");
            this.lblRunning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblRunning.Name = "lblRunning";
            // 
            // tbRunning
            // 
            this.tbRunning.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbRunning, "tbRunning");
            this.tbRunning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbRunning.MaxLength = 60;
            this.tbRunning.Name = "tbRunning";
            // 
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // ReportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblBooking);
            this.Controls.Add(this.lblRunning);
            this.Controls.Add(this.tbRunning);
            this.Controls.Add(this.tbBooking);
            this.Controls.Add(this.tdbHeader);
            this.Controls.Add(this.errorProvider);
            this.Name = "ReportPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner tdbHeader;
        private LSOne.Controls.ShadeTextBoxTouch tbBooking;
        private System.Windows.Forms.Label lblBooking;
        private System.Windows.Forms.Label lblRunning;
        private LSOne.Controls.ShadeTextBoxTouch tbRunning;
        private TouchErrorProvider errorProvider;
    }
}
