namespace LSOne.Controls.Dialogs
{
    partial class PayCheckDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayCheckDialog));
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.touchScrollButtonPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.lblCheckID = new System.Windows.Forms.Label();
            this.tbCheckID = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.lblAmount = new System.Windows.Forms.Label();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = true;
            this.ntbAmount.AllowNegative = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.BackColor = System.Drawing.Color.White;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbAmount.HasMinValue = false;
            this.ntbAmount.MaxLength = 32767;
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.ValueChanged += new System.EventHandler(this.ntbAmount_ValueChanged);
            this.ntbAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ntbAmount_KeyDown);
            this.ntbAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ntbAmount_KeyPress);
            // 
            // touchScrollButtonPanel
            // 
            resources.ApplyResources(this.touchScrollButtonPanel, "touchScrollButtonPanel");
            this.touchScrollButtonPanel.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel.ButtonHeight = 50;
            this.touchScrollButtonPanel.IsHorizontal = true;
            this.touchScrollButtonPanel.Name = "touchScrollButtonPanel";
            this.touchScrollButtonPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel_Click);
            // 
            // lblCheckID
            // 
            resources.ApplyResources(this.lblCheckID, "lblCheckID");
            this.lblCheckID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCheckID.Name = "lblCheckID";
            // 
            // tbCheckID
            // 
            resources.ApplyResources(this.tbCheckID, "tbCheckID");
            this.tbCheckID.BackColor = System.Drawing.Color.White;
            this.tbCheckID.EndCharacter = null;
            this.tbCheckID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCheckID.LastTrack = null;
            this.tbCheckID.ManualEntryOfTrack = true;
            this.tbCheckID.MaxLength = 60;
            this.tbCheckID.Name = "tbCheckID";
            this.tbCheckID.NumericOnly = false;
            this.tbCheckID.Seperator = null;
            this.tbCheckID.StartCharacter = null;
            this.tbCheckID.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAmount.Name = "lblAmount";
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
            this.btnOk.Click += new System.EventHandler(this.PayButtonPressed);
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PayCheckDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.lblCheckID);
            this.Controls.Add(this.touchScrollButtonPanel);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.tbCheckID);
            this.Name = "PayCheckDialog";
            this.Load += new System.EventHandler(this.PayCheckDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner;
        private ShadeNumericTextBox ntbAmount;
        private TouchScrollButtonPanel touchScrollButtonPanel;
        private System.Windows.Forms.Label lblCheckID;
        private MSRTextBoxTouch tbCheckID;
        private TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Label lblAmount;
        private TouchButton btnOk;
        private TouchButton btnCancel;
    }
}