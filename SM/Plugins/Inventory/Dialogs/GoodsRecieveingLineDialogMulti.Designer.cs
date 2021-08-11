using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class GoodsRecieveingLineDialogMulti
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodsRecieveingLineDialogMulti));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.ntbReceivedQuantity = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnableQuantity = new LSOne.Controls.TouchCheckBox();
            this.chkEnableDate = new LSOne.Controls.TouchCheckBox();
            this.dtpReceivedDate = new System.Windows.Forms.DateTimePicker();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbReceivedQuantity
            // 
            this.ntbReceivedQuantity.AllowDecimal = false;
            this.ntbReceivedQuantity.AllowNegative = false;
            this.ntbReceivedQuantity.CultureInfo = null;
            this.ntbReceivedQuantity.DecimalLetters = 2;
            resources.ApplyResources(this.ntbReceivedQuantity, "ntbReceivedQuantity");
            this.ntbReceivedQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbReceivedQuantity.HasMinValue = false;
            this.ntbReceivedQuantity.MaxValue = 0D;
            this.ntbReceivedQuantity.MinValue = 0D;
            this.ntbReceivedQuantity.Name = "ntbReceivedQuantity";
            this.ntbReceivedQuantity.Value = 0D;
            this.ntbReceivedQuantity.ValueChanged += new System.EventHandler(this.CheckEnabled);
            this.ntbReceivedQuantity.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkEnableQuantity
            // 
            resources.ApplyResources(this.chkEnableQuantity, "chkEnableQuantity");
            this.chkEnableQuantity.Name = "chkEnableQuantity";
            this.chkEnableQuantity.CheckedChanged += new System.EventHandler(this.chkEnableQuantity_CheckedChanged);
            // 
            // chkEnableDate
            // 
            resources.ApplyResources(this.chkEnableDate, "chkEnableDate");
            this.chkEnableDate.Name = "chkEnableDate";
            this.chkEnableDate.CheckedChanged += new System.EventHandler(this.chkEnableDate_CheckedChanged);
            // 
            // dtpReceivedDate
            // 
            resources.ApplyResources(this.dtpReceivedDate, "dtpReceivedDate");
            this.dtpReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpReceivedDate.Name = "dtpReceivedDate";
            // 
            // GoodsRecieveingLineDialogMulti
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dtpReceivedDate);
            this.Controls.Add(this.chkEnableDate);
            this.Controls.Add(this.chkEnableQuantity);
            this.Controls.Add(this.ntbReceivedQuantity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.HelpButton = true;
            this.Name = "GoodsRecieveingLineDialogMulti";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.ntbReceivedQuantity, 0);
            this.Controls.SetChildIndex(this.chkEnableQuantity, 0);
            this.Controls.SetChildIndex(this.chkEnableDate, 0);
            this.Controls.SetChildIndex(this.dtpReceivedDate, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbReceivedQuantity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private TouchCheckBox chkEnableDate;
        private TouchCheckBox chkEnableQuantity;
        private System.Windows.Forms.DateTimePicker dtpReceivedDate;
    }
}