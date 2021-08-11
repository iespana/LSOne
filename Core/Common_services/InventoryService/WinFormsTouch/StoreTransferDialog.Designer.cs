namespace LSOne.Services.WinFormsTouch
{
    partial class StoreTransferDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTransferDialog));
            this.td_Banner = new LSOne.Controls.TouchDialogBanner();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // td_Banner
            // 
            resources.ApplyResources(this.td_Banner, "td_Banner");
            this.td_Banner.BackColor = System.Drawing.Color.White;
            this.td_Banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.td_Banner.Name = "td_Banner";
            this.td_Banner.TabStop = false;
            // 
            // pnlControls
            // 
            resources.ApplyResources(this.pnlControls, "pnlControls");
            this.pnlControls.Name = "pnlControls";
            // 
            // StoreTransferDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.td_Banner);
            this.Controls.Add(this.pnlControls);
            this.Name = "StoreTransferDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StoreTransferDialog_FormClosed);
            this.Load += new System.EventHandler(this.StoreTransferDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner td_Banner;
        private System.Windows.Forms.Panel pnlControls;
    }
}