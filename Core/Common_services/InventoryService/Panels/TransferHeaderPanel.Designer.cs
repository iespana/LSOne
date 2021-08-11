namespace LSOne.Services.Panels
{
    partial class TransferHeaderPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferHeaderPanel));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.lblCreateOptions = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.cmbCreateOptions = new LSOne.Controls.DualDataComboBox();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblStore = new System.Windows.Forms.Label();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.btnNext = new LSOne.Controls.TouchButton();
            this.btnSave = new LSOne.Controls.TouchButton();
            this.btnClose = new LSOne.Controls.TouchButton();
            this.dtExpectedDelivery = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.touchErrorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
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
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // lblCreateOptions
            // 
            resources.ApplyResources(this.lblCreateOptions, "lblCreateOptions");
            this.lblCreateOptions.Name = "lblCreateOptions";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.EnableTextBox = true;
            this.cmbCopyFrom.IsPOSControl = true;
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.ShowDropDownOnTyping = true;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.Touch = true;
            this.cmbCopyFrom.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCopyFrom_DropDown);
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            // 
            // cmbCreateOptions
            // 
            this.cmbCreateOptions.AddList = null;
            this.cmbCreateOptions.AllowKeyboardSelection = false;
            this.cmbCreateOptions.EnableTextBox = true;
            resources.ApplyResources(this.cmbCreateOptions, "cmbCreateOptions");
            this.cmbCreateOptions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbCreateOptions.IsPOSControl = true;
            this.cmbCreateOptions.MaxLength = 32767;
            this.cmbCreateOptions.Name = "cmbCreateOptions";
            this.cmbCreateOptions.NoChangeAllowed = false;
            this.cmbCreateOptions.OnlyDisplayID = false;
            this.cmbCreateOptions.ReadOnly = true;
            this.cmbCreateOptions.RemoveList = null;
            this.cmbCreateOptions.RowHeight = ((short)(22));
            this.cmbCreateOptions.SecondaryData = null;
            this.cmbCreateOptions.SelectedData = null;
            this.cmbCreateOptions.SelectedDataID = null;
            this.cmbCreateOptions.SelectionList = null;
            this.cmbCreateOptions.ShowDropDownOnTyping = true;
            this.cmbCreateOptions.SkipIDColumn = true;
            this.cmbCreateOptions.Touch = true;
            this.cmbCreateOptions.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCreateOptions_DropDown);
            this.cmbCreateOptions.SelectedDataChanged += new System.EventHandler(this.cmbCreateOptions_SelectedDataChanged);
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            this.cmbStore.EnableTextBox = true;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.IsPOSControl = true;
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.ReadOnly = true;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.ShowDropDownOnTyping = true;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.Touch = true;
            this.cmbStore.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStore_DropDown);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            // 
            // lblCopyFrom
            // 
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.BackColor = System.Drawing.Color.White;
            this.tbDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbDescription.MaxLength = 60;
            this.tbDescription.Name = "tbDescription";
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.Name = "lblStore";
            // 
            // lblDueDate
            // 
            resources.ApplyResources(this.lblDueDate, "lblDueDate");
            this.lblDueDate.Name = "lblDueDate";
            // 
            // btnNext
            // 
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnNext.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Name = "btnNext";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnSave.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnClose.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dtExpectedDelivery
            // 
            resources.ApplyResources(this.dtExpectedDelivery, "dtExpectedDelivery");
            this.dtExpectedDelivery.BackColor = System.Drawing.Color.White;
            this.dtExpectedDelivery.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtExpectedDelivery.MinDate = new System.DateTime(((long)(0)));
            this.dtExpectedDelivery.Name = "dtExpectedDelivery";
            // 
            // touchErrorProvider
            // 
            resources.ApplyResources(this.touchErrorProvider, "touchErrorProvider");
            this.touchErrorProvider.BackColor = System.Drawing.Color.White;
            this.touchErrorProvider.Name = "touchErrorProvider";
            // 
            // TransferHeaderPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.touchErrorProvider);
            this.Controls.Add(this.dtExpectedDelivery);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblDueDate);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblCopyFrom);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.cmbCreateOptions);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.lblCreateOptions);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "TransferHeaderPanel";
            this.Load += new System.EventHandler(this.TransferHeaderPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Label lblCreateOptions;
        private Controls.DualDataComboBox cmbCopyFrom;
        private LSOne.Controls.DualDataComboBox cmbCreateOptions;
        private Controls.DualDataComboBox cmbStore;
        private System.Windows.Forms.Label lblCopyFrom;
        private System.Windows.Forms.Label lblDescription;
        private LSOne.Controls.ShadeTextBoxTouch tbDescription;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.Label lblDueDate;
        private Controls.TouchButton btnClose;
        private Controls.TouchButton btnSave;
        private Controls.TouchButton btnNext;
        private Controls.Dialogs.DatePickerTouch dtExpectedDelivery;
        private Controls.Dialogs.TouchErrorProvider touchErrorProvider;
    }
}
