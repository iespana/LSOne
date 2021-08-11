using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class RecallCustomerOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecallCustomerOrder));
            this.panelButtons = new LSOne.Controls.TouchScrollButtonPanel();
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.ButtonHeight = 50;
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panelButtons_Click);
            // 
            // banner
            // 
            resources.ApplyResources(this.banner, "banner");
            this.banner.BackColor = System.Drawing.Color.White;
            this.banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.banner.Name = "banner";
            this.banner.TabStop = false;
            // 
            // RecallCustomerOrder
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.banner);
            this.Controls.Add(this.panelButtons);
            this.Name = "RecallCustomerOrder";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchScrollButtonPanel panelButtons;
        private LSOne.Controls.TouchDialogBanner banner;
    }
}