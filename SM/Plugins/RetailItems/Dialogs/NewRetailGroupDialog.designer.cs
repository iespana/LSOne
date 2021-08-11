using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class NewRetailGroupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRetailGroupDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbRetailDepartment = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbItemSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.btnAddRetailDepartment = new LSOne.Controls.ContextButton();
            this.btnAddItemSalesTaxGroup = new LSOne.Controls.ContextButton();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbRetailDepartment
            // 
            this.cmbRetailDepartment.AddList = null;
            this.cmbRetailDepartment.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRetailDepartment, "cmbRetailDepartment");
            this.cmbRetailDepartment.MaxLength = 32767;
            this.cmbRetailDepartment.Name = "cmbRetailDepartment";
            this.cmbRetailDepartment.NoChangeAllowed = false;
            this.cmbRetailDepartment.OnlyDisplayID = false;
            this.cmbRetailDepartment.RemoveList = null;
            this.cmbRetailDepartment.RowHeight = ((short)(22));
            this.cmbRetailDepartment.SecondaryData = null;
            this.cmbRetailDepartment.SelectedData = null;
            this.cmbRetailDepartment.SelectedDataID = null;
            this.cmbRetailDepartment.SelectionList = null;
            this.cmbRetailDepartment.SkipIDColumn = true;
            this.cmbRetailDepartment.RequestData += new System.EventHandler(this.cmbRetailDepartment_RequestData);
            this.cmbRetailDepartment.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbItemSalesTaxGroup
            // 
            this.cmbItemSalesTaxGroup.AddList = null;
            this.cmbItemSalesTaxGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbItemSalesTaxGroup, "cmbItemSalesTaxGroup");
            this.cmbItemSalesTaxGroup.MaxLength = 32767;
            this.cmbItemSalesTaxGroup.Name = "cmbItemSalesTaxGroup";
            this.cmbItemSalesTaxGroup.NoChangeAllowed = false;
            this.cmbItemSalesTaxGroup.OnlyDisplayID = false;
            this.cmbItemSalesTaxGroup.RemoveList = null;
            this.cmbItemSalesTaxGroup.RowHeight = ((short)(22));
            this.cmbItemSalesTaxGroup.SecondaryData = null;
            this.cmbItemSalesTaxGroup.SelectedData = null;
            this.cmbItemSalesTaxGroup.SelectedDataID = null;
            this.cmbItemSalesTaxGroup.SelectionList = null;
            this.cmbItemSalesTaxGroup.SkipIDColumn = true;
            this.cmbItemSalesTaxGroup.RequestData += new System.EventHandler(this.cmbSalesOrder_RequestData);
            this.cmbItemSalesTaxGroup.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddRetailDepartment
            // 
            this.btnAddRetailDepartment.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRetailDepartment.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddRetailDepartment, "btnAddRetailDepartment");
            this.btnAddRetailDepartment.Name = "btnAddRetailDepartment";
            this.btnAddRetailDepartment.Click += new System.EventHandler(this.btnAddRetailDepartment_Click);
            // 
            // btnAddItemSalesTaxGroup
            // 
            this.btnAddItemSalesTaxGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItemSalesTaxGroup.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddItemSalesTaxGroup, "btnAddItemSalesTaxGroup");
            this.btnAddItemSalesTaxGroup.Name = "btnAddItemSalesTaxGroup";
            this.btnAddItemSalesTaxGroup.Click += new System.EventHandler(this.btnAddSalesOrder_Click);
            // 
            // NewRetailGroupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddItemSalesTaxGroup);
            this.Controls.Add(this.btnAddRetailDepartment);
            this.Controls.Add(this.cmbRetailDepartment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbItemSalesTaxGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewRetailGroupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.cmbItemSalesTaxGroup, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbRetailDepartment, 0);
            this.Controls.SetChildIndex(this.btnAddRetailDepartment, 0);
            this.Controls.SetChildIndex(this.btnAddItemSalesTaxGroup, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbRetailDepartment;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbItemSalesTaxGroup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescription;
        private ContextButton btnAddItemSalesTaxGroup;
        private ContextButton btnAddRetailDepartment;
    }
}