using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class MenuTypeSelectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuTypeSelectionDialog));
            this.dialogHeader = new LSOne.Controls.TouchDialogBanner();
            this.buttonPanelMenuTypes = new LSOne.Controls.TouchScrollButtonPanel();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.btnPrintAll = new LSOne.Controls.TouchButton();
            this.btnClear = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
            // 
            // dialogHeader
            // 
            resources.ApplyResources(this.dialogHeader, "dialogHeader");
            this.dialogHeader.BackColor = System.Drawing.Color.White;
            this.dialogHeader.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.dialogHeader.Name = "dialogHeader";
            this.dialogHeader.TabStop = false;
            // 
            // buttonPanelMenuTypes
            // 
            resources.ApplyResources(this.buttonPanelMenuTypes, "buttonPanelMenuTypes");
            this.buttonPanelMenuTypes.BackColor = System.Drawing.Color.White;
            this.buttonPanelMenuTypes.ButtonHeight = 50;
            this.buttonPanelMenuTypes.ColumnCount = 2;
            this.buttonPanelMenuTypes.KeySpacing = 5;
            this.buttonPanelMenuTypes.Name = "buttonPanelMenuTypes";
            this.buttonPanelMenuTypes.Click += new LSOne.Controls.ScrollButtonEventHandler(this.buttonPanelMenuTypes_Click);
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
            // 
            // btnPrintAll
            // 
            resources.ApplyResources(this.btnPrintAll, "btnPrintAll");
            this.btnPrintAll.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnPrintAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnPrintAll.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Action;
            this.btnPrintAll.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPrintAll.ForeColor = System.Drawing.Color.White;
            this.btnPrintAll.Name = "btnPrintAll";
            this.btnPrintAll.UseVisualStyleBackColor = false;
            this.btnPrintAll.Click += new System.EventHandler(this.btnPrintAll_Click);
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
            // MenuTypeSelectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnPrintAll);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.buttonPanelMenuTypes);
            this.Controls.Add(this.dialogHeader);
            this.Name = "MenuTypeSelectionDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner dialogHeader;
        private TouchScrollButtonPanel buttonPanelMenuTypes;
        private LSOne.Controls.TouchButton btnCancel;
        private TouchButton btnOK;
        private TouchButton btnPrintAll;
        private TouchButton btnClear;
    }
}