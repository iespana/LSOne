namespace LSOne.ViewPlugins.ExcelFiles.Dialogs
{
    partial class ImportExcelDocumentDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportExcelDocumentDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRetailGroups = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbRetailItems = new System.Windows.Forms.ComboBox();
            this.cmbCustomers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbVendors = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbRetailDepartments = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblProfitMargin = new System.Windows.Forms.Label();
            this.chkProfitMargin = new System.Windows.Forms.CheckBox();
            this.lblDimAttr = new System.Windows.Forms.Label();
            this.txtDimAttrSeparator = new System.Windows.Forms.TextBox();
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbRetailGroups
            // 
            this.cmbRetailGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRetailGroups.FormattingEnabled = true;
            this.cmbRetailGroups.Items.AddRange(new object[] {
            resources.GetString("cmbRetailGroups.Items"),
            resources.GetString("cmbRetailGroups.Items1"),
            resources.GetString("cmbRetailGroups.Items2"),
            resources.GetString("cmbRetailGroups.Items3")});
            resources.ApplyResources(this.cmbRetailGroups, "cmbRetailGroups");
            this.cmbRetailGroups.Name = "cmbRetailGroups";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbRetailItems
            // 
            this.cmbRetailItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRetailItems.FormattingEnabled = true;
            this.cmbRetailItems.Items.AddRange(new object[] {
            resources.GetString("cmbRetailItems.Items"),
            resources.GetString("cmbRetailItems.Items1"),
            resources.GetString("cmbRetailItems.Items2"),
            resources.GetString("cmbRetailItems.Items3")});
            resources.ApplyResources(this.cmbRetailItems, "cmbRetailItems");
            this.cmbRetailItems.Name = "cmbRetailItems";
            this.cmbRetailItems.SelectedIndexChanged += new System.EventHandler(this.cmbRetailItems_SelectedIndexChanged);
            // 
            // cmbCustomers
            // 
            this.cmbCustomers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomers.FormattingEnabled = true;
            this.cmbCustomers.Items.AddRange(new object[] {
            resources.GetString("cmbCustomers.Items"),
            resources.GetString("cmbCustomers.Items1"),
            resources.GetString("cmbCustomers.Items2"),
            resources.GetString("cmbCustomers.Items3")});
            resources.ApplyResources(this.cmbCustomers, "cmbCustomers");
            this.cmbCustomers.Name = "cmbCustomers";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbVendors
            // 
            this.cmbVendors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendors.FormattingEnabled = true;
            this.cmbVendors.Items.AddRange(new object[] {
            resources.GetString("cmbVendors.Items"),
            resources.GetString("cmbVendors.Items1"),
            resources.GetString("cmbVendors.Items2"),
            resources.GetString("cmbVendors.Items3")});
            resources.ApplyResources(this.cmbVendors, "cmbVendors");
            this.cmbVendors.Name = "cmbVendors";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbRetailDepartments
            // 
            this.cmbRetailDepartments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRetailDepartments.FormattingEnabled = true;
            this.cmbRetailDepartments.Items.AddRange(new object[] {
            resources.GetString("cmbRetailDepartments.Items"),
            resources.GetString("cmbRetailDepartments.Items1"),
            resources.GetString("cmbRetailDepartments.Items2"),
            resources.GetString("cmbRetailDepartments.Items3")});
            resources.ApplyResources(this.cmbRetailDepartments, "cmbRetailDepartments");
            this.cmbRetailDepartments.Name = "cmbRetailDepartments";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblProfitMargin
            // 
            resources.ApplyResources(this.lblProfitMargin, "lblProfitMargin");
            this.lblProfitMargin.Name = "lblProfitMargin";
            // 
            // chkProfitMargin
            // 
            resources.ApplyResources(this.chkProfitMargin, "chkProfitMargin");
            this.chkProfitMargin.Checked = true;
            this.chkProfitMargin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProfitMargin.Name = "chkProfitMargin";
            this.chkProfitMargin.UseVisualStyleBackColor = true;
            // 
            // lblDimAttr
            // 
            resources.ApplyResources(this.lblDimAttr, "lblDimAttr");
            this.lblDimAttr.Name = "lblDimAttr";
            // 
            // txtDimAttrSeparator
            // 
            resources.ApplyResources(this.txtDimAttrSeparator, "txtDimAttrSeparator");
            this.txtDimAttrSeparator.Name = "txtDimAttrSeparator";
            // 
            // ImportExcelDocumentDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.txtDimAttrSeparator);
            this.Controls.Add(this.lblDimAttr);
            this.Controls.Add(this.chkProfitMargin);
            this.Controls.Add(this.lblProfitMargin);
            this.Controls.Add(this.cmbRetailDepartments);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbVendors);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbCustomers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbRetailItems);
            this.Controls.Add(this.cmbRetailGroups);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ImportExcelDocumentDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbRetailGroups, 0);
            this.Controls.SetChildIndex(this.cmbRetailItems, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbCustomers, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbVendors, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.cmbRetailDepartments, 0);
            this.Controls.SetChildIndex(this.lblProfitMargin, 0);
            this.Controls.SetChildIndex(this.chkProfitMargin, 0);
            this.Controls.SetChildIndex(this.lblDimAttr, 0);
            this.Controls.SetChildIndex(this.txtDimAttrSeparator, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRetailGroups;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cmbCustomers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbRetailItems;
        private System.Windows.Forms.ComboBox cmbVendors;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbRetailDepartments;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkProfitMargin;
        private System.Windows.Forms.Label lblProfitMargin;
        private System.Windows.Forms.TextBox txtDimAttrSeparator;
        private System.Windows.Forms.Label lblDimAttr;
    }
}