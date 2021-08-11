using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Peripherals.Dialogs
{
    partial class HardwareConfigurationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareConfigurationDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.pnlPages = new LSOne.Controls.DoubleBufferedPanel();
            this.buttonPanelSteps = new LSOne.Controls.TouchScrollButtonPanel();
            this.cmbHardwareProfile = new LSOne.Controls.DualDataComboBox();
            this.lblStoreID = new System.Windows.Forms.Label();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.buttonPanelActions = new LSOne.Controls.TouchScrollButtonPanel();
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
            // pnlPages
            // 
            resources.ApplyResources(this.pnlPages, "pnlPages");
            this.pnlPages.Name = "pnlPages";
            // 
            // buttonPanelSteps
            // 
            resources.ApplyResources(this.buttonPanelSteps, "buttonPanelSteps");
            this.buttonPanelSteps.BackColor = System.Drawing.Color.White;
            this.buttonPanelSteps.ButtonHeight = 50;
            this.buttonPanelSteps.HorizontalMaxButtonWidth = 200;
            this.buttonPanelSteps.Name = "buttonPanelSteps";
            this.buttonPanelSteps.TabStop = false;
            this.buttonPanelSteps.ToggleState = LSOne.Controls.TouchScrollButtonPanel.ToggleStateEnum.Single;
            this.buttonPanelSteps.Click += new LSOne.Controls.ScrollButtonEventHandler(this.buttonPanelCategories_Click);
            // 
            // cmbHardwareProfile
            // 
            this.cmbHardwareProfile.AddList = null;
            this.cmbHardwareProfile.AllowKeyboardSelection = false;
            this.cmbHardwareProfile.EnableTextBox = true;
            resources.ApplyResources(this.cmbHardwareProfile, "cmbHardwareProfile");
            this.cmbHardwareProfile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbHardwareProfile.IsPOSControl = true;
            this.cmbHardwareProfile.MaxLength = 32767;
            this.cmbHardwareProfile.Name = "cmbHardwareProfile";
            this.cmbHardwareProfile.NoChangeAllowed = false;
            this.cmbHardwareProfile.OnlyDisplayID = false;
            this.cmbHardwareProfile.ReadOnly = true;
            this.cmbHardwareProfile.RemoveList = null;
            this.cmbHardwareProfile.RowHeight = ((short)(22));
            this.cmbHardwareProfile.SecondaryData = null;
            this.cmbHardwareProfile.SelectedData = null;
            this.cmbHardwareProfile.SelectedDataID = null;
            this.cmbHardwareProfile.SelectionList = null;
            this.cmbHardwareProfile.ShowDropDownOnTyping = true;
            this.cmbHardwareProfile.SkipIDColumn = true;
            this.cmbHardwareProfile.Touch = true;
            this.cmbHardwareProfile.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbHardwareProfile_DropDown);
            this.cmbHardwareProfile.SelectedDataChanged += new System.EventHandler(this.cmbHardwareProfile_SelectedDataChanged);
            // 
            // lblStoreID
            // 
            resources.ApplyResources(this.lblStoreID, "lblStoreID");
            this.lblStoreID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStoreID.Name = "lblStoreID";
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
            // buttonPanelActions
            // 
            resources.ApplyResources(this.buttonPanelActions, "buttonPanelActions");
            this.buttonPanelActions.BackColor = System.Drawing.Color.White;
            this.buttonPanelActions.ButtonHeight = 50;
            this.buttonPanelActions.HorizontalMaxButtonWidth = 150;
            this.buttonPanelActions.HorizontalMinButtonWidth = 150;
            this.buttonPanelActions.IsHorizontal = true;
            this.buttonPanelActions.Name = "buttonPanelActions";
            this.buttonPanelActions.ToggleState = LSOne.Controls.TouchScrollButtonPanel.ToggleStateEnum.Single;
            this.buttonPanelActions.Click += new LSOne.Controls.ScrollButtonEventHandler(this.buttonPanelActions_Click);
            // 
            // HardwareConfigurationDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonPanelActions);
            this.Controls.Add(this.lblStoreID);
            this.Controls.Add(this.cmbHardwareProfile);
            this.Controls.Add(this.buttonPanelSteps);
            this.Controls.Add(this.pnlPages);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "HardwareConfigurationDialog";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HardwareConfigurationDialog_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private LSOne.Controls.DualDataComboBox cmbHardwareProfile;
        private System.Windows.Forms.Label lblStoreID;
        private TouchScrollButtonPanel buttonPanelSteps;
        private TouchScrollButtonPanel buttonPanelActions;
        private LSOne.Controls.DoubleBufferedPanel pnlPages;
        private TouchDialogBanner touchDialogBanner1;
    }
}