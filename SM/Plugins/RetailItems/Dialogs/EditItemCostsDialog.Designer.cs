namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class EditItemCostsDialog
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStores = new LSOne.Controls.DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.ntbCostPrice = new LSOne.Controls.NumericTextBox();
            this.lblCostPrice = new System.Windows.Forms.Label();
            this.txtReason = new LSOne.Controls.ShadeTextBox();
            this.lblReason = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(-4, 199);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(447, 46);
            this.panel2.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(259, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(340, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbStores
            // 
            this.cmbStores.AddList = null;
            this.cmbStores.AllowKeyboardSelection = false;
            this.cmbStores.Location = new System.Drawing.Point(131, 92);
            this.cmbStores.MaxLength = 32767;
            this.cmbStores.Name = "cmbStores";
            this.cmbStores.NoChangeAllowed = false;
            this.cmbStores.OnlyDisplayID = false;
            this.cmbStores.RemoveList = null;
            this.cmbStores.RowHeight = ((short)(22));
            this.cmbStores.SecondaryData = null;
            this.cmbStores.SelectedData = null;
            this.cmbStores.SelectedDataID = null;
            this.cmbStores.SelectionList = null;
            this.cmbStores.Size = new System.Drawing.Size(239, 21);
            this.cmbStores.SkipIDColumn = true;
            this.cmbStores.TabIndex = 2;
            this.cmbStores.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStores_DropDown);
            this.cmbStores.SelectedDataChanged += new System.EventHandler(this.cmbStores_SelectedDataChanged);
            // 
            // lblStore
            // 
            this.lblStore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblStore.Location = new System.Drawing.Point(3, 95);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(122, 18);
            this.lblStore.TabIndex = 1;
            this.lblStore.Text = "Store:";
            this.lblStore.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ntbCostPrice
            // 
            this.ntbCostPrice.AllowDecimal = true;
            this.ntbCostPrice.AllowNegative = false;
            this.ntbCostPrice.CultureInfo = null;
            this.ntbCostPrice.DecimalLetters = 2;
            this.ntbCostPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbCostPrice.HasMinValue = false;
            this.ntbCostPrice.Location = new System.Drawing.Point(131, 119);
            this.ntbCostPrice.MaxValue = 9999999999999D;
            this.ntbCostPrice.MinValue = 0D;
            this.ntbCostPrice.Name = "ntbCostPrice";
            this.ntbCostPrice.Size = new System.Drawing.Size(239, 20);
            this.ntbCostPrice.TabIndex = 4;
            this.ntbCostPrice.Text = "0.00";
            this.ntbCostPrice.Value = 0D;
            this.ntbCostPrice.ValueChanged += new System.EventHandler(this.ntbCostPrice_ValueChanged);
            // 
            // lblCostPrice
            // 
            this.lblCostPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblCostPrice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCostPrice.Location = new System.Drawing.Point(0, 122);
            this.lblCostPrice.Name = "lblCostPrice";
            this.lblCostPrice.Size = new System.Drawing.Size(125, 19);
            this.lblCostPrice.TabIndex = 3;
            this.lblCostPrice.Text = "Cost price:";
            this.lblCostPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtReason
            // 
            this.txtReason.GhostText = "Manually edited";
            this.txtReason.Location = new System.Drawing.Point(131, 145);
            this.txtReason.MaxLength = 45;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(239, 20);
            this.txtReason.TabIndex = 6;
            // 
            // lblReason
            // 
            this.lblReason.BackColor = System.Drawing.Color.Transparent;
            this.lblReason.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblReason.Location = new System.Drawing.Point(0, 148);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(125, 19);
            this.lblReason.TabIndex = 5;
            this.lblReason.Text = "Reason:";
            this.lblReason.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // EditItemCostsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(421, 241);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.ntbCostPrice);
            this.Controls.Add(this.lblCostPrice);
            this.Controls.Add(this.cmbStores);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Header = "Edit item cost for the selected stores";
            this.Name = "EditItemCostsDialog";
            this.Text = "Edit item costs";
            this.Load += new System.EventHandler(this.EditItemCostsDialog_Load);
            this.Shown += new System.EventHandler(this.EditItemCostsDialog_Shown);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblStore, 0);
            this.Controls.SetChildIndex(this.cmbStores, 0);
            this.Controls.SetChildIndex(this.lblCostPrice, 0);
            this.Controls.SetChildIndex(this.ntbCostPrice, 0);
            this.Controls.SetChildIndex(this.txtReason, 0);
            this.Controls.SetChildIndex(this.lblReason, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.DualDataComboBox cmbStores;
        private System.Windows.Forms.Label lblStore;
        private Controls.NumericTextBox ntbCostPrice;
        private System.Windows.Forms.Label lblCostPrice;
        private Controls.ShadeTextBox txtReason;
        private System.Windows.Forms.Label lblReason;
    }
}