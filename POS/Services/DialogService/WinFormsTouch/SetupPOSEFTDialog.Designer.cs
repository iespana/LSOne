using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class SetupPOSEFTDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupPOSEFTDialog));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.lblStoreID = new System.Windows.Forms.Label();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.tbStoreID = new LSOne.Controls.ShadeTextBoxTouch();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.tbTerminalID = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblTerminalID = new System.Windows.Forms.Label();
            this.tbCustomField1 = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblCustomField1 = new System.Windows.Forms.Label();
            this.tbCustomField2 = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblCustomField2 = new System.Windows.Forms.Label();
            this.tbIPAddress = new LSOne.Controls.ShadeTextBoxTouch();
            this.SuspendLayout();
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // lblStoreID
            // 
            resources.ApplyResources(this.lblStoreID, "lblStoreID");
            this.lblStoreID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStoreID.Name = "lblStoreID";
            // 
            // lblIPAddress
            // 
            resources.ApplyResources(this.lblIPAddress, "lblIPAddress");
            this.lblIPAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblIPAddress.Name = "lblIPAddress";
            // 
            // tbStoreID
            // 
            resources.ApplyResources(this.tbStoreID, "tbStoreID");
            this.tbStoreID.BackColor = System.Drawing.Color.White;
            this.tbStoreID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbStoreID.MaxLength = 60;
            this.tbStoreID.Name = "tbStoreID";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            // tbTerminalID
            // 
            resources.ApplyResources(this.tbTerminalID, "tbTerminalID");
            this.tbTerminalID.BackColor = System.Drawing.Color.White;
            this.tbTerminalID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbTerminalID.MaxLength = 60;
            this.tbTerminalID.Name = "tbTerminalID";
            // 
            // lblTerminalID
            // 
            resources.ApplyResources(this.lblTerminalID, "lblTerminalID");
            this.lblTerminalID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTerminalID.Name = "lblTerminalID";
            // 
            // tbCustomField1
            // 
            resources.ApplyResources(this.tbCustomField1, "tbCustomField1");
            this.tbCustomField1.BackColor = System.Drawing.Color.White;
            this.tbCustomField1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCustomField1.MaxLength = 60;
            this.tbCustomField1.Name = "tbCustomField1";
            // 
            // lblCustomField1
            // 
            resources.ApplyResources(this.lblCustomField1, "lblCustomField1");
            this.lblCustomField1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCustomField1.Name = "lblCustomField1";
            // 
            // tbCustomField2
            // 
            resources.ApplyResources(this.tbCustomField2, "tbCustomField2");
            this.tbCustomField2.BackColor = System.Drawing.Color.White;
            this.tbCustomField2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCustomField2.MaxLength = 60;
            this.tbCustomField2.Name = "tbCustomField2";
            // 
            // lblCustomField2
            // 
            resources.ApplyResources(this.lblCustomField2, "lblCustomField2");
            this.lblCustomField2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCustomField2.Name = "lblCustomField2";
            // 
            // tbIPAddress
            // 
            resources.ApplyResources(this.tbIPAddress, "tbIPAddress");
            this.tbIPAddress.BackColor = System.Drawing.Color.White;
            this.tbIPAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbIPAddress.MaxLength = 60;
            this.tbIPAddress.Name = "tbIPAddress";
            // 
            // SetupPOSEFTDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbIPAddress);
            this.Controls.Add(this.tbCustomField2);
            this.Controls.Add(this.lblCustomField2);
            this.Controls.Add(this.tbCustomField1);
            this.Controls.Add(this.lblCustomField1);
            this.Controls.Add(this.tbTerminalID);
            this.Controls.Add(this.lblTerminalID);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbStoreID);
            this.Controls.Add(this.lblStoreID);
            this.Controls.Add(this.lblIPAddress);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "SetupPOSEFTDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private TouchDialogBanner touchDialogBanner;
        private System.Windows.Forms.Label lblStoreID;
        private System.Windows.Forms.Label lblIPAddress;
        private LSOne.Controls.ShadeTextBoxTouch tbStoreID;
        private LSOne.Controls.ShadeTextBoxTouch tbTerminalID;
        private System.Windows.Forms.Label lblTerminalID;
        private LSOne.Controls.ShadeTextBoxTouch tbCustomField1;
        private System.Windows.Forms.Label lblCustomField1;
        private LSOne.Controls.ShadeTextBoxTouch tbCustomField2;
        private System.Windows.Forms.Label lblCustomField2;
        private LSOne.Controls.ShadeTextBoxTouch tbIPAddress;
        private TouchButton btnOk;
        private TouchButton btnCancel;
    }
}