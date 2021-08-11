namespace LSOne.Services.WinFormsTouch
{
    partial class NameDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NameDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnClear = new LSOne.Controls.TouchButton();
            this.tbLastName = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbFirstName = new LSOne.Controls.ShadeTextBoxTouch();
            this.cmbTitle = new LSOne.Controls.DualDataComboBox();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
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
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnClear.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbLastName
            // 
            this.tbLastName.BackColor = System.Drawing.Color.White;
            this.tbLastName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbLastName, "tbLastName");
            this.tbLastName.MaxLength = 60;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbFirstName
            // 
            this.tbFirstName.BackColor = System.Drawing.Color.White;
            this.tbFirstName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbFirstName, "tbFirstName");
            this.tbFirstName.MaxLength = 60;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbTitle
            // 
            this.cmbTitle.AddList = null;
            this.cmbTitle.AllowKeyboardSelection = false;
            this.cmbTitle.EnableTextBox = true;
            resources.ApplyResources(this.cmbTitle, "cmbTitle");
            this.cmbTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbTitle.IsPOSControl = true;
            this.cmbTitle.MaxLength = 32767;
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.NoChangeAllowed = false;
            this.cmbTitle.OnlyDisplayID = false;
            this.cmbTitle.ReadOnly = true;
            this.cmbTitle.RemoveList = null;
            this.cmbTitle.RowHeight = ((short)(22));
            this.cmbTitle.SecondaryData = null;
            this.cmbTitle.SelectedData = null;
            this.cmbTitle.SelectedDataID = null;
            this.cmbTitle.SelectionList = null;
            this.cmbTitle.ShowDropDownOnTyping = true;
            this.cmbTitle.SkipIDColumn = true;
            this.cmbTitle.Touch = true;
            this.cmbTitle.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTitle_DropDown);
            this.cmbTitle.RequestClear += new System.EventHandler(this.cmbTitle_RequestClear);
            // 
            // lblPrefix
            // 
            resources.ApplyResources(this.lblPrefix, "lblPrefix");
            this.lblPrefix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPrefix.Name = "lblPrefix";
            // 
            // lblFirstName
            // 
            resources.ApplyResources(this.lblFirstName, "lblFirstName");
            this.lblFirstName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFirstName.Name = "lblFirstName";
            // 
            // lblLastName
            // 
            resources.ApplyResources(this.lblLastName, "lblLastName");
            this.lblLastName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblLastName.Name = "lblLastName";
            // 
            // NameDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblPrefix);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.cmbTitle);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "NameDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchButton btnCancel;
        private Controls.TouchButton btnOK;
        private Controls.TouchKeyboard touchKeyboard;
        private Controls.TouchButton btnClear;
        private Controls.ShadeTextBoxTouch tbLastName;
        private Controls.ShadeTextBoxTouch tbFirstName;
        private Controls.DualDataComboBox cmbTitle;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
    }
}