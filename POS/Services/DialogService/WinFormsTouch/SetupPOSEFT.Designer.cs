using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    partial class SetupPOSEFT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupPOSEFT));
            this.touchKeyboard1 = new LSOne.Controls.TouchKeyboard();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.lblCreditMemoAmount = new System.Windows.Forms.Label();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.tbStoreID = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbTerminalID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCustomField1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCustomField2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.KeystrokeMode = true;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            this.touchKeyboard1.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.SteelBlue;
            this.touchDialogBanner1.Icon = ((System.Drawing.Image)(resources.GetObject("touchDialogBanner1.Icon")));
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // lblCreditMemoAmount
            // 
            resources.ApplyResources(this.lblCreditMemoAmount, "lblCreditMemoAmount");
            this.lblCreditMemoAmount.Name = "lblCreditMemoAmount";
            // 
            // lblIPAddress
            // 
            resources.ApplyResources(this.lblIPAddress, "lblIPAddress");
            this.lblIPAddress.Name = "lblIPAddress";
            // 
            // tbStoreID
            // 
            resources.ApplyResources(this.tbStoreID, "tbStoreID");
            this.tbStoreID.Name = "tbStoreID";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbTerminalID
            // 
            resources.ApplyResources(this.tbTerminalID, "tbTerminalID");
            this.tbTerminalID.Name = "tbTerminalID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbCustomField1
            // 
            resources.ApplyResources(this.tbCustomField1, "tbCustomField1");
            this.tbCustomField1.Name = "tbCustomField1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbCustomField2
            // 
            resources.ApplyResources(this.tbCustomField2, "tbCustomField2");
            this.tbCustomField2.Name = "tbCustomField2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbIPAddress
            // 
            resources.ApplyResources(this.tbIPAddress, "tbIPAddress");
            this.tbIPAddress.Name = "tbIPAddress";
            // 
            // SetupPOSEFT
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbIPAddress);
            this.Controls.Add(this.tbCustomField2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbCustomField1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTerminalID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbStoreID);
            this.Controls.Add(this.lblCreditMemoAmount);
            this.Controls.Add(this.lblIPAddress);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.touchKeyboard1);
            this.Name = "SetupPOSEFT";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchKeyboard touchKeyboard1;
        private TouchDialogBanner touchDialogBanner1;
        private System.Windows.Forms.Label lblCreditMemoAmount;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.TextBox tbStoreID;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbTerminalID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCustomField1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCustomField2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbIPAddress;
    }
}