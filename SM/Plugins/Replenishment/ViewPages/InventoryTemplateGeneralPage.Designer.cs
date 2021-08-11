namespace LSOne.ViewPlugins.Replenishment.ViewPages
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
            this.chkChangeUomInLine = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkChangeVendorInLine = new System.Windows.Forms.CheckBox();
            this.lblChangeVendorInLine = new System.Windows.Forms.Label();
            this.chkDisplayBarcode = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblTypeValue = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chkChangeUomInLine
            // 
            resources.ApplyResources(this.chkChangeUomInLine, "chkChangeUomInLine");
            this.chkChangeUomInLine.Checked = true;
            this.chkChangeUomInLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeUomInLine.Name = "chkChangeUomInLine";
            this.chkChangeUomInLine.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkChangeVendorInLine
            // 
            resources.ApplyResources(this.chkChangeVendorInLine, "chkChangeVendorInLine");
            this.chkChangeVendorInLine.Checked = true;
            this.chkChangeVendorInLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeVendorInLine.Name = "chkChangeVendorInLine";
            this.chkChangeVendorInLine.UseVisualStyleBackColor = true;
            // 
            // lblChangeVendorInLine
            // 
            this.lblChangeVendorInLine.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblChangeVendorInLine, "lblChangeVendorInLine");
            this.lblChangeVendorInLine.Name = "lblChangeVendorInLine";
            // 
            // chkDisplayBarcode
            // 
            resources.ApplyResources(this.chkDisplayBarcode, "chkDisplayBarcode");
            this.chkDisplayBarcode.Name = "chkDisplayBarcode";
            this.chkDisplayBarcode.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblType
            // 
            this.lblType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // lblTypeValue
            // 
            this.lblTypeValue.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTypeValue, "lblTypeValue");
            this.lblTypeValue.Name = "lblTypeValue";
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
            // 
            // cmbUnit
            // 
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.FormattingEnabled = true;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.Name = "cmbUnit";
            // 
            // InventoryTemplateGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.lblTypeValue);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.chkDisplayBarcode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkChangeUomInLine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkChangeVendorInLine);
            this.Controls.Add(this.lblChangeVendorInLine);
            this.DoubleBuffered = true;
            this.Name = "InventoryTemplateGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkChangeUomInLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkChangeVendorInLine;
        private System.Windows.Forms.Label lblChangeVendorInLine;
        private System.Windows.Forms.CheckBox chkDisplayBarcode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblTypeValue;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ComboBox cmbUnit;
    }
}
