using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    partial class CustomerTradeAgreementDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerTradeAgreementDialog));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLineDiscount = new WizardOptionButton();
            this.btnMultilineDiscount = new WizardOptionButton();
            this.btnTotalDiscount = new WizardOptionButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLineDiscount
            // 
            resources.ApplyResources(this.btnLineDiscount, "btnLineDiscount");
            this.btnLineDiscount.Name = "btnLineDiscount";
            this.btnLineDiscount.Click += new System.EventHandler(this.btnLineDiscount_Click);
            // 
            // btnMultilineDiscount
            // 
            resources.ApplyResources(this.btnMultilineDiscount, "btnMultilineDiscount");
            this.btnMultilineDiscount.Name = "btnMultilineDiscount";
            this.btnMultilineDiscount.Click += new System.EventHandler(this.btnMultilineDiscount_Click);
            // 
            // btnTotalDiscount
            // 
            resources.ApplyResources(this.btnTotalDiscount, "btnTotalDiscount");
            this.btnTotalDiscount.Name = "btnTotalDiscount";
            this.btnTotalDiscount.Click += new System.EventHandler(this.btnTotalDiscount_Click);
            // 
            // CustomerTradeAgreementDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnTotalDiscount);
            this.Controls.Add(this.btnMultilineDiscount);
            this.Controls.Add(this.btnLineDiscount);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "CustomerTradeAgreementDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnLineDiscount, 0);
            this.Controls.SetChildIndex(this.btnMultilineDiscount, 0);
            this.Controls.SetChildIndex(this.btnTotalDiscount, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private WizardOptionButton btnLineDiscount;
        private WizardOptionButton btnTotalDiscount;
        private WizardOptionButton btnMultilineDiscount;
        private System.Windows.Forms.Button btnCancel;
    }
}