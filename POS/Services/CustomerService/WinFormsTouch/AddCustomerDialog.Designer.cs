using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    partial class AddCustomerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomerDialog));
            this.btnNext = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnBack = new LSOne.Controls.TouchButton();
            this.milestone1 = new LSOne.Controls.Progress.Milestone();
            this.milestone2 = new LSOne.Controls.Progress.Milestone();
            this.milestone3 = new LSOne.Controls.Progress.Milestone();
            this.activePageIndicator = new LSOne.Controls.ActivePageIndicator();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnNext.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Name = "btnNext";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // touchKeyboard
            // 
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // btnBack
            // 
            this.btnBack.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnBack.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // milestone1
            // 
            resources.ApplyResources(this.milestone1, "milestone1");
            // 
            // milestone2
            // 
            resources.ApplyResources(this.milestone2, "milestone2");
            // 
            // milestone3
            // 
            resources.ApplyResources(this.milestone3, "milestone3");
            // 
            // activePageIndicator
            // 
            this.activePageIndicator.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.activePageIndicator, "activePageIndicator");
            this.activePageIndicator.Name = "activePageIndicator";
            this.activePageIndicator.Pages.Add("Details");
            this.activePageIndicator.Pages.Add("Address");
            this.activePageIndicator.Pages.Add("Other");
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // AddCustomerDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.activePageIndicator);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnCancel);
            this.Name = "AddCustomerDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchButton btnNext;
        private LSOne.Controls.TouchButton btnCancel;
        private TouchKeyboard touchKeyboard;
        private LSOne.Controls.TouchButton btnBack;
        private Controls.Progress.Milestone milestone1;
        private Controls.Progress.Milestone milestone2;
        private Controls.Progress.Milestone milestone3;
        private ActivePageIndicator activePageIndicator;
        private TouchDialogBanner touchDialogBanner1;
        private System.Windows.Forms.Panel panel1;
    }
}