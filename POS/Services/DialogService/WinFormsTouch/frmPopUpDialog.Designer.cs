using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.Enums;

namespace LSOne.Services.WinFormsTouch
{
    partial class frmPopUpDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPopUpDialog));
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.tableLayoutPanelItems = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelGroups = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnClearSelection = new LSOne.Controls.TouchButton();
            this.btnClearQty = new LSOne.Controls.TouchButton();
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanelItems
            // 
            resources.ApplyResources(this.tableLayoutPanelItems, "tableLayoutPanelItems");
            this.tableLayoutPanelItems.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanelItems.Name = "tableLayoutPanelItems";
            // 
            // tableLayoutPanelGroups
            // 
            resources.ApplyResources(this.tableLayoutPanelGroups, "tableLayoutPanelGroups");
            this.tableLayoutPanelGroups.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanelGroups.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanelGroups.Name = "tableLayoutPanelGroups";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblInfo
            // 
            resources.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.BackColor = System.Drawing.Color.White;
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblInfo.Name = "lblInfo";
            // 
            // btnClearSelection
            // 
            resources.ApplyResources(this.btnClearSelection, "btnClearSelection");
            this.btnClearSelection.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClearSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnClearSelection.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Action;
            this.btnClearSelection.ForeColor = System.Drawing.Color.White;
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.UseVisualStyleBackColor = false;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // btnClearQty
            // 
            resources.ApplyResources(this.btnClearQty, "btnClearQty");
            this.btnClearQty.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClearQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnClearQty.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Action;
            this.btnClearQty.ForeColor = System.Drawing.Color.White;
            this.btnClearQty.Name = "btnClearQty";
            this.btnClearQty.UseVisualStyleBackColor = false;
            this.btnClearQty.Click += new System.EventHandler(this.btnClearQty_Click);
            // 
            // banner
            // 
            resources.ApplyResources(this.banner, "banner");
            this.banner.BackColor = System.Drawing.Color.White;
            this.banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.banner.Name = "banner";
            this.banner.TabStop = false;
            // 
            // frmPopUpDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelItems);
            this.Controls.Add(this.tableLayoutPanelGroups);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnClearQty);
            this.Controls.Add(this.btnClearSelection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.banner);
            this.Name = "frmPopUpDialog";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelItems;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGroups;
        private LSOne.Controls.TouchButton btnOK;
        private System.Windows.Forms.Label lblInfo;
        private LSOne.Controls.TouchButton btnCancel;
        private LSOne.Controls.TouchButton btnClearSelection;
        private LSOne.Controls.TouchButton btnClearQty;
        private TouchDialogBanner banner;
    }
}