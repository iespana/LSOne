namespace LSOne.Services
{
    partial class frmTenderRestriction
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
            this.lblContinue = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnNo = new DevExpress.XtraEditors.SimpleButton();
            this.btnYes = new DevExpress.XtraEditors.SimpleButton();
            this.memMessage = new DevExpress.XtraEditors.MemoEdit();
            this.lstExcluded = new DevExpress.XtraEditors.ListBoxControl();
            this.lblExcluded = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memMessage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstExcluded)).BeginInit();
            this.SuspendLayout();
            // 
            // lblContinue
            // 
            this.lblContinue.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.lblContinue.Location = new System.Drawing.Point(5, 330);
            this.lblContinue.Name = "lblContinue";
            this.lblContinue.Size = new System.Drawing.Size(345, 21);
            this.lblContinue.TabIndex = 5;
            this.lblContinue.Tag = "Normal";
            this.lblContinue.Text = "Do you want to continue?";
            this.lblContinue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.btnNo);
            this.panelControl1.Controls.Add(this.btnYes);
            this.panelControl1.Controls.Add(this.memMessage);
            this.panelControl1.Controls.Add(this.lblContinue);
            this.panelControl1.Controls.Add(this.lstExcluded);
            this.panelControl1.Controls.Add(this.lblExcluded);
            this.panelControl1.Location = new System.Drawing.Point(2, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(355, 418);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // btnNo
            // 
            this.btnNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(191, 363);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(107, 38);
            this.btnNo.TabIndex = 2;
            this.btnNo.Tag = "BtnNormal";
            this.btnNo.Text = "No";
            // 
            // btnYes
            // 
            this.btnYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(46, 363);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(108, 38);
            this.btnYes.TabIndex = 1;
            this.btnYes.Tag = "BtnNormal";
            this.btnYes.Text = "Yes";
            // 
            // memMessage
            // 
            this.memMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.memMessage.Location = new System.Drawing.Point(4, 248);
            this.memMessage.Name = "memMessage";
            this.memMessage.Properties.ReadOnly = true;
            this.memMessage.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memMessage.Size = new System.Drawing.Size(346, 72);
            this.memMessage.TabIndex = 4;
            this.memMessage.Tag = "";
            // 
            // lstExcluded
            // 
            this.lstExcluded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstExcluded.Location = new System.Drawing.Point(4, 28);
            this.lstExcluded.Name = "lstExcluded";
            this.lstExcluded.Size = new System.Drawing.Size(346, 214);
            this.lstExcluded.TabIndex = 3;
            this.lstExcluded.Tag = "";
            // 
            // lblExcluded
            // 
            this.lblExcluded.AutoSize = true;
            this.lblExcluded.Font = new System.Drawing.Font("Tahoma", 10.75F, System.Drawing.FontStyle.Bold);
            this.lblExcluded.Location = new System.Drawing.Point(5, 2);
            this.lblExcluded.Name = "lblExcluded";
            this.lblExcluded.Size = new System.Drawing.Size(234, 18);
            this.lblExcluded.TabIndex = 6;
            this.lblExcluded.Text = "Items excluded from payment:";
            // 
            // frmTenderRestriction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 418);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTenderRestriction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmTenderRestriction";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memMessage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstExcluded)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblContinue;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.MemoEdit memMessage;
        private DevExpress.XtraEditors.ListBoxControl lstExcluded;
        private System.Windows.Forms.Label lblExcluded;
        private DevExpress.XtraEditors.SimpleButton btnNo;
        private DevExpress.XtraEditors.SimpleButton btnYes;
    }
}