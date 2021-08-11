namespace LSOne.ViewPlugins.LSCommerce.ViewPages
{
    partial class InventoryTemplateGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplateGeneralPage));
            this.chkAllowAddNewLine = new System.Windows.Forms.CheckBox();
            this.lblAddNewLine = new System.Windows.Forms.Label();
            this.chkUseBarcodeUom = new System.Windows.Forms.CheckBox();
            this.lblBarcodeUom = new System.Windows.Forms.Label();
            this.cmbEnteringType = new System.Windows.Forms.ComboBox();
            this.lblTypeOfEntering = new System.Windows.Forms.Label();
            this.lblDefaultQty = new System.Windows.Forms.Label();
            this.ntDefaultQty = new LSOne.Controls.NumericTextBox();
            this.cmbQuantityMethod = new System.Windows.Forms.ComboBox();
            this.lblQtyMethod = new System.Windows.Forms.Label();
            this.linkFields2 = new LSOne.Controls.LinkFields();
            this.chkAllowImageImport = new System.Windows.Forms.CheckBox();
            this.lblAllowImageImport = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkAllowAddNewLine
            // 
            resources.ApplyResources(this.chkAllowAddNewLine, "chkAllowAddNewLine");
            this.chkAllowAddNewLine.Checked = true;
            this.chkAllowAddNewLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowAddNewLine.Name = "chkAllowAddNewLine";
            this.chkAllowAddNewLine.UseVisualStyleBackColor = true;
            // 
            // lblAddNewLine
            // 
            this.lblAddNewLine.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAddNewLine, "lblAddNewLine");
            this.lblAddNewLine.Name = "lblAddNewLine";
            // 
            // chkUseBarcodeUom
            // 
            resources.ApplyResources(this.chkUseBarcodeUom, "chkUseBarcodeUom");
            this.chkUseBarcodeUom.Checked = true;
            this.chkUseBarcodeUom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseBarcodeUom.Name = "chkUseBarcodeUom";
            this.chkUseBarcodeUom.UseVisualStyleBackColor = true;
            // 
            // lblBarcodeUom
            // 
            this.lblBarcodeUom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcodeUom, "lblBarcodeUom");
            this.lblBarcodeUom.Name = "lblBarcodeUom";
            // 
            // cmbEnteringType
            // 
            this.cmbEnteringType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEnteringType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbEnteringType, "cmbEnteringType");
            this.cmbEnteringType.Name = "cmbEnteringType";
            // 
            // lblTypeOfEntering
            // 
            this.lblTypeOfEntering.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTypeOfEntering, "lblTypeOfEntering");
            this.lblTypeOfEntering.Name = "lblTypeOfEntering";
            // 
            // lblDefaultQty
            // 
            this.lblDefaultQty.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDefaultQty, "lblDefaultQty");
            this.lblDefaultQty.Name = "lblDefaultQty";
            // 
            // ntDefaultQty
            // 
            this.ntDefaultQty.AllowDecimal = false;
            this.ntDefaultQty.AllowNegative = false;
            this.ntDefaultQty.CultureInfo = null;
            this.ntDefaultQty.DecimalLetters = 2;
            this.ntDefaultQty.ForeColor = System.Drawing.Color.Black;
            this.ntDefaultQty.HasMinValue = true;
            resources.ApplyResources(this.ntDefaultQty, "ntDefaultQty");
            this.ntDefaultQty.MaxValue = 999999999999D;
            this.ntDefaultQty.MinValue = 0D;
            this.ntDefaultQty.Name = "ntDefaultQty";
            this.ntDefaultQty.Value = 1D;
            // 
            // cmbQuantityMethod
            // 
            this.cmbQuantityMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuantityMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbQuantityMethod, "cmbQuantityMethod");
            this.cmbQuantityMethod.Name = "cmbQuantityMethod";
            this.cmbQuantityMethod.SelectedIndexChanged += new System.EventHandler(this.cmbQuantityMethod_SelectedIndexChanged);
            // 
            // lblQtyMethod
            // 
            this.lblQtyMethod.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblQtyMethod, "lblQtyMethod");
            this.lblQtyMethod.Name = "lblQtyMethod";
            // 
            // linkFields2
            // 
            this.linkFields2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields2, "linkFields2");
            this.linkFields2.Name = "linkFields2";
            this.linkFields2.TabStop = false;
            // 
            // chkAllowImageImport
            // 
            resources.ApplyResources(this.chkAllowImageImport, "chkAllowImageImport");
            this.chkAllowImageImport.Name = "chkAllowImageImport";
            this.chkAllowImageImport.UseVisualStyleBackColor = true;
            // 
            // lblAllowImageImport
            // 
            this.lblAllowImageImport.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAllowImageImport, "lblAllowImageImport");
            this.lblAllowImageImport.Name = "lblAllowImageImport";
            // 
            // InventoryTemplateGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkAllowImageImport);
            this.Controls.Add(this.lblAllowImageImport);
            this.Controls.Add(this.linkFields2);
            this.Controls.Add(this.cmbEnteringType);
            this.Controls.Add(this.lblTypeOfEntering);
            this.Controls.Add(this.lblDefaultQty);
            this.Controls.Add(this.ntDefaultQty);
            this.Controls.Add(this.cmbQuantityMethod);
            this.Controls.Add(this.lblQtyMethod);
            this.Controls.Add(this.chkAllowAddNewLine);
            this.Controls.Add(this.lblAddNewLine);
            this.Controls.Add(this.chkUseBarcodeUom);
            this.Controls.Add(this.lblBarcodeUom);
            this.DoubleBuffered = true;
            this.Name = "InventoryTemplateGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAllowAddNewLine;
        private System.Windows.Forms.Label lblAddNewLine;
        private System.Windows.Forms.CheckBox chkUseBarcodeUom;
        private System.Windows.Forms.Label lblBarcodeUom;
        private System.Windows.Forms.ComboBox cmbEnteringType;
        private System.Windows.Forms.Label lblTypeOfEntering;
        private System.Windows.Forms.Label lblDefaultQty;
        private Controls.NumericTextBox ntDefaultQty;
        private System.Windows.Forms.ComboBox cmbQuantityMethod;
        private System.Windows.Forms.Label lblQtyMethod;
        private Controls.LinkFields linkFields2;
        private System.Windows.Forms.CheckBox chkAllowImageImport;
        private System.Windows.Forms.Label lblAllowImageImport;
    }
}
