using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    /// <summary>
    /// A dialog that is used when creating a new customer order/quote and also to display the current configurations for a customer order/quote
    /// </summary>
    partial class CustomerOrderDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerOrderDetails));
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.SuspendLayout();
            // 
            // banner
            // 
            resources.ApplyResources(this.banner, "banner");
            this.banner.BackColor = System.Drawing.Color.White;
            this.banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.banner.Name = "banner";
            this.banner.TabStop = false;
            // 
            // CustomerOrderDetails
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.banner);
            this.Name = "CustomerOrderDetails";
            this.Load += new System.EventHandler(this.CustomerOrderDetails_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private LSOne.Controls.TouchDialogBanner banner;
    }
}