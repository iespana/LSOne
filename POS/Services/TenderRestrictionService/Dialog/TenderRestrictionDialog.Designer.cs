namespace LSOne.Services
{
    partial class TenderRestrictionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TenderRestrictionDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.buttonPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblSelectOtherPayment = new System.Windows.Forms.Label();
            this.pnlReceipt = new LSOne.Controls.DoubleBufferedPanel();
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
            // buttonPanel
            // 
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            this.buttonPanel.BackColor = System.Drawing.Color.White;
            this.buttonPanel.ButtonHeight = 50;
            this.buttonPanel.HorizontalMaxButtonWidth = 150;
            this.buttonPanel.HorizontalMinButtonWidth = 150;
            this.buttonPanel.IsHorizontal = true;
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.buttonPanel_Click);
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.Name = "lblMessage";
            // 
            // lblSelectOtherPayment
            // 
            resources.ApplyResources(this.lblSelectOtherPayment, "lblSelectOtherPayment");
            this.lblSelectOtherPayment.Name = "lblSelectOtherPayment";
            // 
            // pnlReceipt
            // 
            resources.ApplyResources(this.pnlReceipt, "pnlReceipt");
            this.pnlReceipt.Name = "pnlReceipt";
            // 
            // TenderRestrictionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReceipt);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblSelectOtherPayment);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "TenderRestrictionDialog";
            this.Load += new System.EventHandler(this.TenderRestrictionDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchScrollButtonPanel buttonPanel;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblSelectOtherPayment;
        private Controls.DoubleBufferedPanel pnlReceipt;
    }
}