using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class CustomerOrderActions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerOrderActions));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.panelOptions = new LSOne.Controls.TouchScrollButtonPanel();
            this.SuspendLayout();
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // panelOptions
            // 
            this.panelOptions.BackColor = System.Drawing.Color.White;
            this.panelOptions.ButtonHeight = 50;
            resources.ApplyResources(this.panelOptions, "panelOptions");
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panelClick);
            // 
            // CustomerOrderActions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "CustomerOrderActions";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchScrollButtonPanel panelOptions;
    }
}