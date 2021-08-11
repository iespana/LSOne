using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class ReceiptListDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiptListDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.panel = new LSOne.Controls.TouchScrollButtonPanel();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.lblReceiptID = new LSOne.Controls.DoubleLabel();
            this.lblStore = new LSOne.Controls.DoubleLabel();
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
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.ButtonHeight = 50;
            this.panel.Name = "panel";
            this.panel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panel_Click);
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
            // lblReceiptID
            // 
            this.lblReceiptID.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblReceiptID, "lblReceiptID");
            this.lblReceiptID.HeaderText = "Receipt ID";
            this.lblReceiptID.Name = "lblReceiptID";
            // 
            // lblStore
            // 
            this.lblStore.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.HeaderText = "Store";
            this.lblStore.Name = "lblStore";
            // 
            // ReceiptListDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.lblReceiptID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "ReceiptListDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchScrollButtonPanel panel;
        private LSOne.Controls.TouchButton btnCancel;
        private Controls.DoubleLabel lblReceiptID;
        private Controls.DoubleLabel lblStore;
    }
}