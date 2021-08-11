
namespace LSOne.Services.SplitBill
{
    partial class SplitBillDialog
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
        private void InitializeComponent()
        {
            this.tlpForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlpGrid1 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpWindow = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSplitLines = new System.Windows.Forms.TableLayoutPanel();
            this.tlpGuestLines = new System.Windows.Forms.TableLayoutPanel();
            this.lblTableBalance = new System.Windows.Forms.Label();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.lblTableBalanceValue = new System.Windows.Forms.Label();
            this.tlpForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpForm
            // 
            this.tlpForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpForm.ColumnCount = 5;
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpForm.Controls.Add(this.tlpGrid1, 1, 1);
            this.tlpForm.Controls.Add(this.tlpWindow, 3, 1);
            this.tlpForm.Controls.Add(this.tlpSplitLines, 2, 0);
            this.tlpForm.Controls.Add(this.tlpGuestLines, 4, 0);
            this.tlpForm.Location = new System.Drawing.Point(1, 51);
            this.tlpForm.Margin = new System.Windows.Forms.Padding(0);
            this.tlpForm.Name = "tlpForm";
            this.tlpForm.RowCount = 3;
            this.tlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpForm.Size = new System.Drawing.Size(1022, 577);
            this.tlpForm.TabIndex = 0;
            // 
            // tlpGrid1
            // 
            this.tlpGrid1.ColumnCount = 1;
            this.tlpGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGrid1.Location = new System.Drawing.Point(10, 458);
            this.tlpGrid1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.tlpGrid1.Name = "tlpGrid1";
            this.tlpGrid1.RowCount = 1;
            this.tlpForm.SetRowSpan(this.tlpGrid1, 2);
            this.tlpGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpGrid1.Size = new System.Drawing.Size(349, 109);
            this.tlpGrid1.TabIndex = 3;
            // 
            // tlpWindow
            // 
            this.tlpWindow.ColumnCount = 1;
            this.tlpWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWindow.Location = new System.Drawing.Point(517, 458);
            this.tlpWindow.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.tlpWindow.Name = "tlpWindow";
            this.tlpWindow.RowCount = 1;
            this.tlpForm.SetRowSpan(this.tlpWindow, 2);
            this.tlpWindow.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWindow.Size = new System.Drawing.Size(349, 109);
            this.tlpWindow.TabIndex = 4;
            // 
            // tlpSplitLines
            // 
            this.tlpSplitLines.ColumnCount = 1;
            this.tlpSplitLines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSplitLines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSplitLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSplitLines.Location = new System.Drawing.Point(364, 10);
            this.tlpSplitLines.Margin = new System.Windows.Forms.Padding(2, 10, 2, 10);
            this.tlpSplitLines.Name = "tlpSplitLines";
            this.tlpSplitLines.RowCount = 1;
            this.tlpForm.SetRowSpan(this.tlpSplitLines, 3);
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpSplitLines.Size = new System.Drawing.Size(148, 557);
            this.tlpSplitLines.TabIndex = 5;
            // 
            // tlpGuestLines
            // 
            this.tlpGuestLines.ColumnCount = 1;
            this.tlpGuestLines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGuestLines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpGuestLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGuestLines.Location = new System.Drawing.Point(871, 10);
            this.tlpGuestLines.Margin = new System.Windows.Forms.Padding(2, 10, 10, 10);
            this.tlpGuestLines.Name = "tlpGuestLines";
            this.tlpGuestLines.RowCount = 1;
            this.tlpForm.SetRowSpan(this.tlpGuestLines, 3);
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 572F));
            this.tlpGuestLines.Size = new System.Drawing.Size(141, 557);
            this.tlpGuestLines.TabIndex = 6;
            // 
            // lblTableBalance
            // 
            this.lblTableBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTableBalance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTableBalance.Location = new System.Drawing.Point(786, 5);
            this.lblTableBalance.Margin = new System.Windows.Forms.Padding(0);
            this.lblTableBalance.Name = "lblTableBalance";
            this.lblTableBalance.Size = new System.Drawing.Size(237, 20);
            this.lblTableBalance.TabIndex = 1;
            this.lblTableBalance.Text = "Table balance";
            this.lblTableBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // touchDialogBanner
            // 
            this.touchDialogBanner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.BannerText = "Split bill";
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Location = new System.Drawing.Point(1, 1);
            this.touchDialogBanner.Margin = new System.Windows.Forms.Padding(0);
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.Size = new System.Drawing.Size(1022, 50);
            this.touchDialogBanner.TabIndex = 1;
            this.touchDialogBanner.TabStop = false;
            // 
            // lblTableBalanceValue
            // 
            this.lblTableBalanceValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTableBalanceValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableBalanceValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTableBalanceValue.Location = new System.Drawing.Point(786, 25);
            this.lblTableBalanceValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblTableBalanceValue.Name = "lblTableBalanceValue";
            this.lblTableBalanceValue.Size = new System.Drawing.Size(237, 20);
            this.lblTableBalanceValue.TabIndex = 2;
            this.lblTableBalanceValue.Text = "-";
            this.lblTableBalanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SplitBillDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 629);
            this.Controls.Add(this.lblTableBalanceValue);
            this.Controls.Add(this.lblTableBalance);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.tlpForm);
            this.Name = "SplitBillDialog";
            this.Text = "SplitBillDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplitBillDialog_FormClosing);
            this.Load += new System.EventHandler(this.SplitBillDialog_Load);
            this.tlpForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpForm;
        private System.Windows.Forms.Label lblTableBalance;
        private System.Windows.Forms.TableLayoutPanel tlpGrid1;
        private System.Windows.Forms.TableLayoutPanel tlpWindow;
        private System.Windows.Forms.TableLayoutPanel tlpSplitLines;
        private System.Windows.Forms.TableLayoutPanel tlpGuestLines;
        private Controls.TouchDialogBanner touchDialogBanner;
        private System.Windows.Forms.Label lblTableBalanceValue;
    }
}