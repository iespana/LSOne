using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class EditPriorityDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditPriorityDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbOfferNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.ntbPriority = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbTriggering = new System.Windows.Forms.ComboBox();
            this.lblTriggering = new System.Windows.Forms.Label();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.tbBarcode = new System.Windows.Forms.TextBox();
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
            // 
            // tbOfferNumber
            // 
            resources.ApplyResources(this.tbOfferNumber, "tbOfferNumber");
            this.tbOfferNumber.Name = "tbOfferNumber";
            this.tbOfferNumber.ReadOnly = true;
            this.tbOfferNumber.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            this.tbDescription.TabStop = false;
            // 
            // ntbPriority
            // 
            this.ntbPriority.AllowDecimal = false;
            this.ntbPriority.AllowNegative = false;
            this.ntbPriority.CultureInfo = null;
            this.ntbPriority.DecimalLetters = 2;
            this.ntbPriority.ForeColor = System.Drawing.Color.Black;
            this.ntbPriority.HasMinValue = false;
            resources.ApplyResources(this.ntbPriority, "ntbPriority");
            this.ntbPriority.MaxValue = 0D;
            this.ntbPriority.MinValue = 0D;
            this.ntbPriority.Name = "ntbPriority";
            this.ntbPriority.Value = 0D;
            this.ntbPriority.TextChanged += new System.EventHandler(this.ntbPriority_TextChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cmbTriggering
            // 
            this.cmbTriggering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTriggering.FormattingEnabled = true;
            this.cmbTriggering.Items.AddRange(new object[] {
            resources.GetString("cmbTriggering.Items"),
            resources.GetString("cmbTriggering.Items1")});
            resources.ApplyResources(this.cmbTriggering, "cmbTriggering");
            this.cmbTriggering.Name = "cmbTriggering";
            this.cmbTriggering.SelectedIndexChanged += new System.EventHandler(this.cmbTriggering_SelectedIndexChanged);
            // 
            // lblTriggering
            // 
            this.lblTriggering.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTriggering, "lblTriggering");
            this.lblTriggering.Name = "lblTriggering";
            // 
            // lblBarcode
            // 
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcode, "lblBarcode");
            this.lblBarcode.Name = "lblBarcode";
            // 
            // tbBarcode
            // 
            resources.ApplyResources(this.tbBarcode, "tbBarcode");
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.TextChanged += new System.EventHandler(this.tbBarcode_TextChanged);
            // 
            // EditPriorityDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.cmbTriggering);
            this.Controls.Add(this.lblTriggering);
            this.Controls.Add(this.ntbPriority);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbOfferNumber);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "EditPriorityDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbOfferNumber, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.ntbPriority, 0);
            this.Controls.SetChildIndex(this.lblTriggering, 0);
            this.Controls.SetChildIndex(this.cmbTriggering, 0);
            this.Controls.SetChildIndex(this.tbBarcode, 0);
            this.Controls.SetChildIndex(this.lblBarcode, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbOfferNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private NumericTextBox ntbPriority;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cmbTriggering;
        private System.Windows.Forms.Label lblTriggering;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox tbBarcode;
    }
}