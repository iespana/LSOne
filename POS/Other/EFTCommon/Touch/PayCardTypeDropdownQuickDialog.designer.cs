using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.EFT.Common.Touch
{
    partial class PayCardTypeDropdownQuickDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayCardTypeDropdownQuickDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.touchScrollButtonPanel1 = new LSOne.Controls.TouchScrollButtonPanel();
            this.lblCardType = new System.Windows.Forms.Label();
            this.tbCardNumber = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.comboBox1 = new LSOne.Controls.DualDataComboBox();
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
            // touchScrollButtonPanel1
            // 
            resources.ApplyResources(this.touchScrollButtonPanel1, "touchScrollButtonPanel1");
            this.touchScrollButtonPanel1.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel1.ButtonHeight = 50;
            this.touchScrollButtonPanel1.Name = "touchScrollButtonPanel1";
            this.touchScrollButtonPanel1.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel1_Click);
            // 
            // lblCardType
            // 
            resources.ApplyResources(this.lblCardType, "lblCardType");
            this.lblCardType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCardType.Name = "lblCardType";
            // 
            // tbCardNumber
            // 
            resources.ApplyResources(this.tbCardNumber, "tbCardNumber");
            this.tbCardNumber.BackColor = System.Drawing.Color.White;
            this.tbCardNumber.EndCharacter = null;
            this.tbCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCardNumber.LastTrack = null;
            this.tbCardNumber.ManualEntryOfTrack = true;
            this.tbCardNumber.MaxLength = 32767;
            this.tbCardNumber.Name = "tbCardNumber";
            this.tbCardNumber.NumericOnly = false;
            this.tbCardNumber.Seperator = null;
            this.tbCardNumber.StartCharacter = null;
            this.tbCardNumber.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // lblCardNumber
            // 
            resources.ApplyResources(this.lblCardNumber, "lblCardNumber");
            this.lblCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCardNumber.Name = "lblCardNumber";
            // 
            // comboBox1
            // 
            this.comboBox1.AddList = null;
            this.comboBox1.AllowKeyboardSelection = false;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.EnableTextBox = true;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.comboBox1.IsPOSControl = true;
            this.comboBox1.MaxLength = 32767;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.NoChangeAllowed = false;
            this.comboBox1.OnlyDisplayID = false;
            this.comboBox1.ReadOnly = true;
            this.comboBox1.RemoveList = null;
            this.comboBox1.RowHeight = ((short)(22));
            this.comboBox1.SecondaryData = null;
            this.comboBox1.SelectedData = null;
            this.comboBox1.SelectedDataID = null;
            this.comboBox1.SelectionList = null;
            this.comboBox1.ShowDropDownOnTyping = true;
            this.comboBox1.SkipIDColumn = true;
            this.comboBox1.Touch = true;
            this.comboBox1.DropDown += new LSOne.Controls.DropDownEventHandler(this.comboBox1_DropDown);
            this.comboBox1.SelectedDataChanged += new System.EventHandler(this.comboBox1_SelectedDataChanged);
            // 
            // PayCardTypeDropdownQuickDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbCardNumber);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblCardType);
            this.Controls.Add(this.touchScrollButtonPanel1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "PayCardTypeDropdownQuickDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PayCardDialog_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner1;
        private TouchScrollButtonPanel touchScrollButtonPanel1;
        private System.Windows.Forms.Label lblCardType;
        private LSOne.Controls.DualDataComboBox comboBox1;
        private MSRTextBoxTouch tbCardNumber;
        private System.Windows.Forms.Label lblCardNumber;
    }
}