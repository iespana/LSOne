using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class EmailAddressDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailAddressDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.label1 = new System.Windows.Forms.Label();
            this.tbEmailAddress = new LSOne.Controls.ShadeTextBoxTouch();
            this.touchKeyboard1 = new LSOne.Controls.TouchKeyboard();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.touchShorthandButtons = new LSOne.Controls.TouchScrollButtonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label1.Name = "label1";
            // 
            // tbEmailAddress
            // 
            this.tbEmailAddress.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbEmailAddress, "tbEmailAddress");
            this.tbEmailAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbEmailAddress.MaxLength = 500;
            this.tbEmailAddress.Name = "tbEmailAddress";
            this.tbEmailAddress.Enter += new System.EventHandler(this.tbEmailAddress_Enter);
            this.tbEmailAddress.Leave += new System.EventHandler(this.tbEmailAddress_Leave);
            this.tbEmailAddress.TextChanged += new System.EventHandler(this.tbEmailAddress_TextChanged);
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            this.touchKeyboard1.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // touchShorthandButtons
            // 
            this.touchShorthandButtons.BackColor = System.Drawing.Color.White;
            this.touchShorthandButtons.ButtonHeight = 50;
            resources.ApplyResources(this.touchShorthandButtons, "touchShorthandButtons");
            this.touchShorthandButtons.IsHorizontal = true;
            this.touchShorthandButtons.Name = "touchShorthandButtons";
            this.touchShorthandButtons.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchShorthandButtons_Click);
            // 
            // EmailAddressDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.touchShorthandButtons);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.touchKeyboard1);
            this.Controls.Add(this.tbEmailAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "EmailAddressDialog";
            this.Shown += new System.EventHandler(this.ReceiptEmail_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchDialogBanner touchDialogBanner1;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.ShadeTextBoxTouch tbEmailAddress;
        private LSOne.Controls.TouchKeyboard touchKeyboard1;
        private LSOne.Controls.TouchButton btnOK;
        private LSOne.Controls.TouchButton btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private LSOne.Controls.TouchScrollButtonPanel touchShorthandButtons;
    }
}