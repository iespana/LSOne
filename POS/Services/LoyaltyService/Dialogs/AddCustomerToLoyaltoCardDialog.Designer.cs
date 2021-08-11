using LSOne.Controls;

namespace LSOne.Services.Dialogs
{
    partial class AddCustomerToLoyaltoCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomerToLoyaltoCardDialog));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnGet = new LSOne.Controls.TouchButton();
            this.tbLoyaltyCardNumber = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.btnCustomerSearch = new LSOne.Controls.TouchButton();
            this.tbCustomer = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblCardScheme = new LSOne.Controls.DoubleLabel();
            this.lblPointUseLimit = new LSOne.Controls.DoubleLabel();
            this.lblValue = new LSOne.Controls.DoubleLabel();
            this.lblStartingPoints = new LSOne.Controls.DoubleLabel();
            this.touchErrorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.pnlInfo.SuspendLayout();
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
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
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
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // btnGet
            // 
            resources.ApplyResources(this.btnGet, "btnGet");
            this.btnGet.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnGet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnGet.BackgroundImage = global::LSOne.Services.Properties.Resources.Checkmark_white_32px;
            this.btnGet.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnGet.DrawBorder = false;
            this.btnGet.ForeColor = System.Drawing.Color.White;
            this.btnGet.Name = "btnGet";
            this.btnGet.UseVisualStyleBackColor = false;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // tbLoyaltyCardNumber
            // 
            this.tbLoyaltyCardNumber.BackColor = System.Drawing.Color.White;
            this.tbLoyaltyCardNumber.EndCharacter = null;
            resources.ApplyResources(this.tbLoyaltyCardNumber, "tbLoyaltyCardNumber");
            this.tbLoyaltyCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbLoyaltyCardNumber.LastTrack = null;
            this.tbLoyaltyCardNumber.ManualEntryOfTrack = true;
            this.tbLoyaltyCardNumber.MaxLength = 32767;
            this.tbLoyaltyCardNumber.Name = "tbLoyaltyCardNumber";
            this.tbLoyaltyCardNumber.NumericOnly = false;
            this.tbLoyaltyCardNumber.Seperator = null;
            this.tbLoyaltyCardNumber.StartCharacter = null;
            this.tbLoyaltyCardNumber.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbLoyaltyCardNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLoyaltyCardNumber_KeyDown);
            this.tbLoyaltyCardNumber.TextChanged += new System.EventHandler(this.tbLoyaltyCardNumber_TextChanged);
            this.tbLoyaltyCardNumber.LostFocus += new System.EventHandler(this.tbLoyaltyCardNumber_LostFocus);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label3.Name = "label3";
            // 
            // btnCustomerSearch
            // 
            resources.ApplyResources(this.btnCustomerSearch, "btnCustomerSearch");
            this.btnCustomerSearch.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCustomerSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnCustomerSearch.BackgroundImage = global::LSOne.Services.Properties.Resources.Whitesearch32px;
            this.btnCustomerSearch.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnCustomerSearch.DrawBorder = false;
            this.btnCustomerSearch.ForeColor = System.Drawing.Color.White;
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.UseVisualStyleBackColor = false;
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // tbCustomer
            // 
            this.tbCustomer.BackColor = System.Drawing.Color.White;
            this.tbCustomer.EndCharacter = null;
            this.tbCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbCustomer, "tbCustomer");
            this.tbCustomer.LastTrack = null;
            this.tbCustomer.ManualEntryOfTrack = true;
            this.tbCustomer.MaxLength = 32767;
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.NumericOnly = false;
            this.tbCustomer.Seperator = null;
            this.tbCustomer.StartCharacter = null;
            this.tbCustomer.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCustomer_KeyDown);
            this.tbCustomer.TextChanged += new System.EventHandler(this.tbCustomer_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label4.Name = "label4";
            // 
            // pnlInfo
            // 
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.Controls.Add(this.lblCardScheme);
            this.pnlInfo.Controls.Add(this.lblPointUseLimit);
            this.pnlInfo.Controls.Add(this.lblValue);
            this.pnlInfo.Controls.Add(this.lblStartingPoints);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInfo_Paint);
            // 
            // lblCardScheme
            // 
            this.lblCardScheme.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCardScheme, "lblCardScheme");
            this.lblCardScheme.HeaderText = "Card scheme";
            this.lblCardScheme.Name = "lblCardScheme";
            // 
            // lblPointUseLimit
            // 
            this.lblPointUseLimit.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblPointUseLimit, "lblPointUseLimit");
            this.lblPointUseLimit.HeaderText = "Point use limit %";
            this.lblPointUseLimit.Name = "lblPointUseLimit";
            // 
            // lblValue
            // 
            this.lblValue.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblValue, "lblValue");
            this.lblValue.HeaderText = "Value";
            this.lblValue.Name = "lblValue";
            // 
            // lblStartingPoints
            // 
            this.lblStartingPoints.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblStartingPoints, "lblStartingPoints");
            this.lblStartingPoints.HeaderText = "Starting points";
            this.lblStartingPoints.Name = "lblStartingPoints";
            // 
            // touchErrorProvider
            // 
            this.touchErrorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchErrorProvider, "touchErrorProvider");
            this.touchErrorProvider.Name = "touchErrorProvider";
            // 
            // AddCustomerToLoyaltoCardDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.touchErrorProvider);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.btnCustomerSearch);
            this.Controls.Add(this.tbCustomer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.tbLoyaltyCardNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "AddCustomerToLoyaltoCardDialog";
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private LSOne.Controls.TouchButton btnCancel;
        private LSOne.Controls.TouchButton btnOk;
        private TouchDialogBanner touchDialogBanner;
        private TouchButton btnGet;
        private MSRTextBoxTouch tbLoyaltyCardNumber;
        private System.Windows.Forms.Label label3;
        private TouchButton btnCustomerSearch;
        private MSRTextBoxTouch tbCustomer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlInfo;
        private DoubleLabel lblCardScheme;
        private DoubleLabel lblPointUseLimit;
        private DoubleLabel lblValue;
        private DoubleLabel lblStartingPoints;
        private Controls.Dialogs.TouchErrorProvider touchErrorProvider;
    }
}