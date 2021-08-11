using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class SalesOrderDialog
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
            LSOne.Controls.TouchDialogBanner tdbHeader;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesOrderDialog));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.tbSalesOrderId = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbCustomerId = new LSOne.Controls.ShadeTextBoxTouch();
            this.btnSearchSalesOrder = new LSOne.Controls.TouchButton();
            this.btnSearchCustomer = new LSOne.Controls.TouchButton();
            this.lblSalesOrder = new System.Windows.Forms.Label();
            this.lblForPrepayment = new LSOne.Controls.DoubleLabel();
            this.btnPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblPrepaid = new LSOne.Controls.DoubleLabel();
            this.lblCreated = new LSOne.Controls.DoubleLabel();
            this.lblTotal = new LSOne.Controls.DoubleLabel();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            tdbHeader = new LSOne.Controls.TouchDialogBanner();
            this.SuspendLayout();
            // 
            // tdbHeader
            // 
            resources.ApplyResources(tdbHeader, "tdbHeader");
            tdbHeader.BackColor = System.Drawing.Color.White;
            tdbHeader.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            tdbHeader.Name = "tdbHeader";
            tdbHeader.TabStop = false;
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.SendEnterAsKeyStroke = true;
            this.touchKeyboard.TabStop = false;
            // 
            // tbSalesOrderId
            // 
            this.tbSalesOrderId.BackColor = System.Drawing.Color.White;
            this.tbSalesOrderId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbSalesOrderId, "tbSalesOrderId");
            this.tbSalesOrderId.MaxLength = 60;
            this.tbSalesOrderId.Name = "tbSalesOrderId";
            this.tbSalesOrderId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSalesOrderId_KeyPress);
            // 
            // tbCustomerId
            // 
            this.tbCustomerId.BackColor = System.Drawing.Color.White;
            this.tbCustomerId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbCustomerId, "tbCustomerId");
            this.tbCustomerId.MaxLength = 60;
            this.tbCustomerId.Name = "tbCustomerId";
            this.tbCustomerId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCustomerId_KeyPress);
            // 
            // btnSearchSalesOrder
            // 
            this.btnSearchSalesOrder.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSearchSalesOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnSearchSalesOrder.BackgroundImage = global::LSOne.Services.Properties.Resources.Checkmark_white_32px;
            resources.ApplyResources(this.btnSearchSalesOrder, "btnSearchSalesOrder");
            this.btnSearchSalesOrder.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnSearchSalesOrder.DrawBorder = false;
            this.btnSearchSalesOrder.ForeColor = System.Drawing.Color.White;
            this.btnSearchSalesOrder.Name = "btnSearchSalesOrder";
            this.btnSearchSalesOrder.UseVisualStyleBackColor = false;
            this.btnSearchSalesOrder.Click += new System.EventHandler(this.btnSearchSalesOrder_Click);
            // 
            // btnSearchCustomer
            // 
            this.btnSearchCustomer.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSearchCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnSearchCustomer.BackgroundImage = global::LSOne.Services.Properties.Resources.Checkmark_white_32px;
            resources.ApplyResources(this.btnSearchCustomer, "btnSearchCustomer");
            this.btnSearchCustomer.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnSearchCustomer.DrawBorder = false;
            this.btnSearchCustomer.ForeColor = System.Drawing.Color.White;
            this.btnSearchCustomer.Name = "btnSearchCustomer";
            this.btnSearchCustomer.UseVisualStyleBackColor = false;
            this.btnSearchCustomer.Click += new System.EventHandler(this.btnSearchCustomer_Click);
            // 
            // lblSalesOrder
            // 
            resources.ApplyResources(this.lblSalesOrder, "lblSalesOrder");
            this.lblSalesOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblSalesOrder.Name = "lblSalesOrder";
            // 
            // lblForPrepayment
            // 
            this.lblForPrepayment.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblForPrepayment, "lblForPrepayment");
            this.lblForPrepayment.HeaderText = "For prepayment";
            this.lblForPrepayment.Name = "lblForPrepayment";
            // 
            // btnPanel
            // 
            resources.ApplyResources(this.btnPanel, "btnPanel");
            this.btnPanel.BackColor = System.Drawing.Color.White;
            this.btnPanel.ButtonHeight = 50;
            this.btnPanel.HorizontalMaxButtonWidth = 150;
            this.btnPanel.HorizontalMinButtonWidth = 150;
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.btnPanel_Click);
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCustomer.Name = "lblCustomer";
            // 
            // lblPrepaid
            // 
            this.lblPrepaid.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblPrepaid, "lblPrepaid");
            this.lblPrepaid.HeaderText = "Prepaid";
            this.lblPrepaid.Name = "lblPrepaid";
            // 
            // lblCreated
            // 
            this.lblCreated.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCreated, "lblCreated");
            this.lblCreated.HeaderText = "Created";
            this.lblCreated.Name = "lblCreated";
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblTotal, "lblTotal");
            this.lblTotal.HeaderText = "Total";
            this.lblTotal.Name = "lblTotal";
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.HeaderText = "Balance";
            this.lblBalance.Name = "lblBalance";
            // 
            // SalesOrderDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.lblPrepaid);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.btnSearchCustomer);
            this.Controls.Add(this.btnSearchSalesOrder);
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.lblForPrepayment);
            this.Controls.Add(this.lblSalesOrder);
            this.Controls.Add(this.tbCustomerId);
            this.Controls.Add(tdbHeader);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.tbSalesOrderId);
            this.Name = "SalesOrderDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchKeyboard touchKeyboard;
        private LSOne.Controls.ShadeTextBoxTouch tbSalesOrderId;
        private LSOne.Controls.ShadeTextBoxTouch tbCustomerId;
        private LSOne.Controls.TouchButton btnSearchSalesOrder;
        private LSOne.Controls.TouchButton btnSearchCustomer;
        private System.Windows.Forms.Label lblSalesOrder;
        private Controls.DoubleLabel lblForPrepayment;
        private Controls.TouchScrollButtonPanel btnPanel;
        private System.Windows.Forms.Label lblCustomer;
        private Controls.DoubleLabel lblPrepaid;
        private Controls.DoubleLabel lblCreated;
        private Controls.DoubleLabel lblTotal;
        private Controls.DoubleLabel lblBalance;
    }
}