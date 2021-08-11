using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services
{
    partial class InformationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationDialog));
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.lblDriverID = new System.Windows.Forms.Label();
            this.tbDriverID = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbVehicleId = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblVehicleID = new System.Windows.Forms.Label();
            this.tbOdometer = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblOdometer = new System.Windows.Forms.Label();
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
            // 
            // btnOk
            // 
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // touchKeyboard
            // 
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // lblDriverID
            // 
            resources.ApplyResources(this.lblDriverID, "lblDriverID");
            this.lblDriverID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDriverID.Name = "lblDriverID";
            // 
            // tbDriverID
            // 
            this.tbDriverID.BackColor = System.Drawing.Color.White;
            this.tbDriverID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbDriverID, "tbDriverID");
            this.tbDriverID.Name = "tbDriverID";
            this.tbDriverID.Enter += new System.EventHandler(this.tbDriverID_Enter);
            this.tbDriverID.Leave += new System.EventHandler(this.tbDriverID_Leave);
            // 
            // tbVehicleId
            // 
            this.tbVehicleId.BackColor = System.Drawing.Color.White;
            this.tbVehicleId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbVehicleId, "tbVehicleId");
            this.tbVehicleId.Name = "tbVehicleId";
            this.tbVehicleId.Enter += new System.EventHandler(this.tbVehicleId_Enter);
            this.tbVehicleId.Leave += new System.EventHandler(this.tbVehicleId_Leave);
            // 
            // lblVehicleID
            // 
            resources.ApplyResources(this.lblVehicleID, "lblVehicleID");
            this.lblVehicleID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblVehicleID.Name = "lblVehicleID";
            // 
            // tbOdometer
            // 
            this.tbOdometer.BackColor = System.Drawing.Color.White;
            this.tbOdometer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbOdometer, "tbOdometer");
            this.tbOdometer.Name = "tbOdometer";
            this.tbOdometer.Enter += new System.EventHandler(this.tbOdometer_Enter);
            this.tbOdometer.Leave += new System.EventHandler(this.tbOdometer_Leave);
            // 
            // lblOdometer
            // 
            resources.ApplyResources(this.lblOdometer, "lblOdometer");
            this.lblOdometer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblOdometer.Name = "lblOdometer";
            // 
            // InformationDialog
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.lblOdometer);
            this.Controls.Add(this.tbOdometer);
            this.Controls.Add(this.lblVehicleID);
            this.Controls.Add(this.tbVehicleId);
            this.Controls.Add(this.lblDriverID);
            this.Controls.Add(this.tbDriverID);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.touchDialogBanner);
            this.Name = "InformationDialog";
            this.ResumeLayout(false);

        }

        #endregion


        private TouchDialogBanner touchDialogBanner;
        private TouchButton btnCancel;
        private TouchButton btnOk;
        private TouchKeyboard touchKeyboard;
        private Label lblDriverID;
        private ShadeTextBoxTouch tbDriverID;
        private ShadeTextBoxTouch tbVehicleId;
        private Label lblVehicleID;
        private ShadeTextBoxTouch tbOdometer;
        private Label lblOdometer;
    }
}