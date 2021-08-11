using LSOne.Controls;

namespace LSOne.ViewPlugins.LabelPrinting.Dialogs
{
    partial class LabelPrintingDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelPrintingDialog));
            this.lblPrinter = new System.Windows.Forms.Label();
            this.cmbLabels = new System.Windows.Forms.ComboBox();
            this.btnEditLabels = new ContextButton();
            this.lblLabel = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.numQuantity = new NumericTextBox();
            this.cmbPrinters = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPrinter
            // 
            resources.ApplyResources(this.lblPrinter, "lblPrinter");
            this.lblPrinter.Name = "lblPrinter";
            // 
            // cmbLabels
            // 
            this.cmbLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLabels.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLabels, "cmbLabels");
            this.cmbLabels.Name = "cmbLabels";
            this.cmbLabels.Sorted = true;
            // 
            // btnEditLabels
            // 
            this.btnEditLabels.BackColor = System.Drawing.Color.Transparent;
            this.btnEditLabels.Context = ButtonType.Edit;
            this.btnEditLabels.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnEditLabels, "btnEditLabels");
            this.btnEditLabels.Name = "btnEditLabels";
            this.btnEditLabels.Click += new System.EventHandler(this.OnEditLabelsClick);
            // 
            // lblLabel
            // 
            resources.ApplyResources(this.lblLabel, "lblLabel");
            this.lblLabel.Name = "lblLabel";
            // 
            // lblQty
            // 
            resources.ApplyResources(this.lblQty, "lblQty");
            this.lblQty.Name = "lblQty";
            // 
            // numQuantity
            // 
            this.numQuantity.AllowDecimal = false;
            this.numQuantity.AllowNegative = false;
            this.numQuantity.CultureInfo = null;
            this.numQuantity.DecimalLetters = 2;
            this.numQuantity.HasMinValue = false;
            resources.ApplyResources(this.numQuantity, "numQuantity");
            this.numQuantity.MaxValue = 1000D;
            this.numQuantity.MinValue = 1D;
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Value = 0D;
            // 
            // cmbPrinters
            // 
            this.cmbPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinters.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPrinters, "cmbPrinters");
            this.cmbPrinters.Name = "cmbPrinters";
            this.cmbPrinters.Sorted = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnOKClick);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
            // 
            // LabelPrintingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnEditLabels);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.cmbLabels);
            this.Controls.Add(this.lblPrinter);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.cmbPrinters);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "LabelPrintingDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblLabel, 0);
            this.Controls.SetChildIndex(this.cmbPrinters, 0);
            this.Controls.SetChildIndex(this.lblQty, 0);
            this.Controls.SetChildIndex(this.lblPrinter, 0);
            this.Controls.SetChildIndex(this.cmbLabels, 0);
            this.Controls.SetChildIndex(this.numQuantity, 0);
            this.Controls.SetChildIndex(this.btnEditLabels, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrinter;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.ComboBox cmbLabels;
        private System.Windows.Forms.ComboBox cmbPrinters;
        private NumericTextBox numQuantity;
        private ContextButton btnEditLabels;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
    }
}