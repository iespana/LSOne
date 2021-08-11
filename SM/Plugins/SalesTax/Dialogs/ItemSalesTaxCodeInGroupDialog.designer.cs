using LSOne.Controls;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    partial class ItemSalesTaxCodeInGroupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSalesTaxCodeInGroupDialog));
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkAddAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbTaxCode = new LSOne.Controls.DualDataComboBox();
            this.btnAddSalesTax = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkAddAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkAddAnother
            // 
            resources.ApplyResources(this.chkAddAnother, "chkAddAnother");
            this.chkAddAnother.Name = "chkAddAnother";
            this.chkAddAnother.UseVisualStyleBackColor = true;
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
            // cmbTaxCode
            // 
            this.cmbTaxCode.AddList = null;
            this.cmbTaxCode.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTaxCode, "cmbTaxCode");
            this.cmbTaxCode.MaxLength = 32767;
            this.cmbTaxCode.Name = "cmbTaxCode";
            this.cmbTaxCode.NoChangeAllowed = false;
            this.cmbTaxCode.OnlyDisplayID = false;
            this.cmbTaxCode.RemoveList = null;
            this.cmbTaxCode.RowHeight = ((short)(22));
            this.cmbTaxCode.SecondaryData = null;
            this.cmbTaxCode.SelectedData = null;
            this.cmbTaxCode.SelectedDataID = null;
            this.cmbTaxCode.SelectionList = null;
            this.cmbTaxCode.SkipIDColumn = false;
            this.cmbTaxCode.RequestData += new System.EventHandler(this.cmbTaxCode_RequestData);
            this.cmbTaxCode.SelectedDataChanged += new System.EventHandler(this.cmbTaxCode_SelectedDataChanged);
            // 
            // btnAddSalesTax
            // 
            resources.ApplyResources(this.btnAddSalesTax, "btnAddSalesTax");
            this.btnAddSalesTax.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSalesTax.Context = LSOne.Controls.ButtonType.Add;
            this.btnAddSalesTax.Name = "btnAddSalesTax";
            this.btnAddSalesTax.Click += new System.EventHandler(this.btnAddSalesTaxCode_Click);
            // 
            // ItemSalesTaxCodeInGroupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddSalesTax);
            this.Controls.Add(this.cmbTaxCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ItemSalesTaxCodeInGroupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbTaxCode, 0);
            this.Controls.SetChildIndex(this.btnAddSalesTax, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbTaxCode;
        private ContextButton btnAddSalesTax;
        private System.Windows.Forms.CheckBox chkAddAnother;
    }
}