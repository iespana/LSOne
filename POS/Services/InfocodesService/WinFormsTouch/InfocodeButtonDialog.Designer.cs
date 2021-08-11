using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class InfocodeButtonDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfocodeButtonDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.tspInfocodes = new LSOne.Controls.TouchScrollButtonPanel();
            this.btnCancel = new LSOne.Controls.TouchButton();
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
            // tspInfocodes
            // 
            resources.ApplyResources(this.tspInfocodes, "tspInfocodes");
            this.tspInfocodes.BackColor = System.Drawing.Color.White;
            this.tspInfocodes.ButtonHeight = 50;
            this.tspInfocodes.ColumnCount = 2;
            this.tspInfocodes.KeySpacing = 5;
            this.tspInfocodes.Name = "tspInfocodes";
            this.tspInfocodes.Click += new LSOne.Controls.ScrollButtonEventHandler(this.tspInfocodes_Click);
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
            // InfocodeButtonDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tspInfocodes);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "InfocodeButtonDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner1;
        private TouchScrollButtonPanel tspInfocodes;
        private LSOne.Controls.TouchButton btnCancel;
    }
}