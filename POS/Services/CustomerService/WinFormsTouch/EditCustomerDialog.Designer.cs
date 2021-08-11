namespace LSOne.Services.WinFormsTouch
{
    partial class EditCustomerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCustomerDialog));
            this.btnPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.SuspendLayout();
            // 
            // btnPanel
            // 
            resources.ApplyResources(this.btnPanel, "btnPanel");
            this.btnPanel.BackColor = System.Drawing.Color.Transparent;
            this.btnPanel.ButtonHeight = 50;
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.TabStop = false;
            this.btnPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.btnPanel_Click);
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // pnlControls
            // 
            resources.ApplyResources(this.pnlControls, "pnlControls");
            this.pnlControls.Name = "pnlControls";
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // EditCustomerDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.btnPanel);
            this.Name = "EditCustomerDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchScrollButtonPanel btnPanel;
        private Controls.TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Panel pnlControls;
        private Controls.TouchDialogBanner touchDialogBanner1;
    }
}