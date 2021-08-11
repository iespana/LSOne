using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class MultibuyConfigurationDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultibuyConfigurationDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblPriceDiscount = new System.Windows.Forms.Label();
            this.ntbMinQty = new LSOne.Controls.NumericTextBox();
            this.ntbPriceDiscount = new LSOne.Controls.NumericTextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblPriceDiscount
            // 
            this.lblPriceDiscount.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPriceDiscount, "lblPriceDiscount");
            this.lblPriceDiscount.Name = "lblPriceDiscount";
            // 
            // ntbMinQty
            // 
            this.ntbMinQty.AllowDecimal = true;
            this.ntbMinQty.AllowNegative = false;
            this.ntbMinQty.CultureInfo = null;
            this.ntbMinQty.DecimalLetters = 2;
            this.ntbMinQty.ForeColor = System.Drawing.Color.Black;
            this.ntbMinQty.HasMinValue = false;
            resources.ApplyResources(this.ntbMinQty, "ntbMinQty");
            this.ntbMinQty.MaxValue = 0D;
            this.ntbMinQty.MinValue = 0D;
            this.ntbMinQty.Name = "ntbMinQty";
            this.ntbMinQty.Value = 0D;
            this.ntbMinQty.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbPriceDiscount
            // 
            this.ntbPriceDiscount.AllowDecimal = true;
            this.ntbPriceDiscount.AllowNegative = false;
            this.ntbPriceDiscount.CultureInfo = null;
            this.ntbPriceDiscount.DecimalLetters = 2;
            this.ntbPriceDiscount.ForeColor = System.Drawing.Color.Black;
            this.ntbPriceDiscount.HasMinValue = false;
            resources.ApplyResources(this.ntbPriceDiscount, "ntbPriceDiscount");
            this.ntbPriceDiscount.MaxValue = 0D;
            this.ntbPriceDiscount.MinValue = 0D;
            this.ntbPriceDiscount.Name = "ntbPriceDiscount";
            this.ntbPriceDiscount.Value = 0D;
            this.ntbPriceDiscount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // MultibuyConfigurationDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbPriceDiscount);
            this.Controls.Add(this.ntbMinQty);
            this.Controls.Add(this.lblPriceDiscount);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "MultibuyConfigurationDialog";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblPriceDiscount, 0);
            this.Controls.SetChildIndex(this.ntbMinQty, 0);
            this.Controls.SetChildIndex(this.ntbPriceDiscount, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblPriceDiscount;
        private NumericTextBox ntbMinQty;
        private NumericTextBox ntbPriceDiscount;
    }
}