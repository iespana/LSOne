using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.DialogPanels
{
    partial class PassportPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PassportPanel));
            this.tdbHeader = new LSOne.Controls.TouchDialogBanner();
            this.tbPassportNumber = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblPassportNumber = new System.Windows.Forms.Label();
            this.lblPassportIssuedBy = new System.Windows.Forms.Label();
            this.tbPassportIssuedBy = new LSOne.Controls.ShadeTextBoxTouch();
            this.dtIssueDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // tdbHeader
            // 
            this.tdbHeader.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdbHeader, "tdbHeader");
            this.tdbHeader.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbHeader.Name = "tdbHeader";
            this.tdbHeader.TabStop = false;
            // 
            // tbPassportNumber
            // 
            this.tbPassportNumber.BackColor = System.Drawing.Color.White;
            this.tbPassportNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbPassportNumber, "tbPassportNumber");
            this.tbPassportNumber.MaxLength = 40;
            this.tbPassportNumber.Name = "tbPassportNumber";
            // 
            // lblPassportNumber
            // 
            resources.ApplyResources(this.lblPassportNumber, "lblPassportNumber");
            this.lblPassportNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPassportNumber.Name = "lblPassportNumber";
            // 
            // lblPassportIssuedBy
            // 
            resources.ApplyResources(this.lblPassportIssuedBy, "lblPassportIssuedBy");
            this.lblPassportIssuedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPassportIssuedBy.Name = "lblPassportIssuedBy";
            // 
            // tbPassportIssuedBy
            // 
            this.tbPassportIssuedBy.BackColor = System.Drawing.Color.White;
            this.tbPassportIssuedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbPassportIssuedBy, "tbPassportIssuedBy");
            this.tbPassportIssuedBy.MaxLength = 60;
            this.tbPassportIssuedBy.Name = "tbPassportIssuedBy";
            // 
            // dtIssueDate
            // 
            this.dtIssueDate.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dtIssueDate, "dtIssueDate");
            this.dtIssueDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.dtIssueDate.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtIssueDate.MinDate = new System.DateTime(((long)(0)));
            this.dtIssueDate.Name = "dtIssueDate";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label1.Name = "label1";
            // 
            // btnSearch
            // 
            this.btnSearch.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnSearch.BackgroundImage = global::LSOne.Services.Properties.Resources.Whitesearch32px;
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnSearch.DrawBorder = false;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.TabStop = false;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // PassportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblPassportNumber);
            this.Controls.Add(this.dtIssueDate);
            this.Controls.Add(this.lblPassportIssuedBy);
            this.Controls.Add(this.tbPassportIssuedBy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPassportNumber);
            this.Controls.Add(this.tdbHeader);
            this.Controls.Add(this.errorProvider);
            this.Name = "PassportPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner tdbHeader;
        private ShadeTextBoxTouch tbPassportNumber;
        private System.Windows.Forms.Label lblPassportNumber;
        private System.Windows.Forms.Label lblPassportIssuedBy;
        private ShadeTextBoxTouch tbPassportIssuedBy;
        private DatePickerTouch dtIssueDate;
        private System.Windows.Forms.Label label1;
        private TouchButton btnSearch;
        private TouchErrorProvider errorProvider;
    }
}
