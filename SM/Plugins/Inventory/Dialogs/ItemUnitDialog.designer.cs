using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class ItemUnitDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemUnitDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnAddUnit1 = new ContextButton();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSalesUnitNew = new DualDataComboBox();
            this.tbItem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSalesUnitCurrent = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbInventoryUnitCurrent = new System.Windows.Forms.TextBox();
            this.cmbInventoryUnitNew = new DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddUnit2 = new ContextButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbPurchaseUnitCurrent = new System.Windows.Forms.TextBox();
            this.cmbPurchaseUnitNew = new DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAddUnit3 = new ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // btnAddUnit1
            // 
            this.btnAddUnit1.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit1.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit1, "btnAddUnit1");
            this.btnAddUnit1.Name = "btnAddUnit1";
            this.btnAddUnit1.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbSalesUnitNew
            // 
            this.cmbSalesUnitNew.AddList = null;
            this.cmbSalesUnitNew.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesUnitNew, "cmbSalesUnitNew");
            this.cmbSalesUnitNew.MaxLength = 32767;
            this.cmbSalesUnitNew.Name = "cmbSalesUnitNew";
            this.cmbSalesUnitNew.OnlyDisplayID = false;
            this.cmbSalesUnitNew.RemoveList = null;
            this.cmbSalesUnitNew.RowHeight = ((short)(22));
            this.cmbSalesUnitNew.SelectedData = null;
            this.cmbSalesUnitNew.SelectedDataID = null;
            this.cmbSalesUnitNew.SelectionList = null;
            this.cmbSalesUnitNew.SkipIDColumn = true;
            this.cmbSalesUnitNew.RequestData += new System.EventHandler(this.cmbSalesUnitNew_RequestData);
            this.cmbSalesUnitNew.SelectedDataChanged += new System.EventHandler(this.CheckChanged);
            // 
            // tbItem
            // 
            resources.ApplyResources(this.tbItem, "tbItem");
            this.tbItem.Name = "tbItem";
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
            // tbSalesUnitCurrent
            // 
            resources.ApplyResources(this.tbSalesUnitCurrent, "tbSalesUnitCurrent");
            this.tbSalesUnitCurrent.Name = "tbSalesUnitCurrent";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.tbSalesUnitCurrent);
            this.groupBox4.Controls.Add(this.cmbSalesUnitNew);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.btnAddUnit1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tbInventoryUnitCurrent);
            this.groupBox1.Controls.Add(this.cmbInventoryUnitNew);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnAddUnit2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tbInventoryUnitCurrent
            // 
            resources.ApplyResources(this.tbInventoryUnitCurrent, "tbInventoryUnitCurrent");
            this.tbInventoryUnitCurrent.Name = "tbInventoryUnitCurrent";
            // 
            // cmbInventoryUnitNew
            // 
            this.cmbInventoryUnitNew.AddList = null;
            this.cmbInventoryUnitNew.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbInventoryUnitNew, "cmbInventoryUnitNew");
            this.cmbInventoryUnitNew.MaxLength = 32767;
            this.cmbInventoryUnitNew.Name = "cmbInventoryUnitNew";
            this.cmbInventoryUnitNew.OnlyDisplayID = false;
            this.cmbInventoryUnitNew.RemoveList = null;
            this.cmbInventoryUnitNew.RowHeight = ((short)(22));
            this.cmbInventoryUnitNew.SelectedData = null;
            this.cmbInventoryUnitNew.SelectedDataID = null;
            this.cmbInventoryUnitNew.SelectionList = null;
            this.cmbInventoryUnitNew.SkipIDColumn = true;
            this.cmbInventoryUnitNew.RequestData += new System.EventHandler(this.cmbInventoryUnitNew_RequestData);
            this.cmbInventoryUnitNew.SelectedDataChanged += new System.EventHandler(this.CheckChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // btnAddUnit2
            // 
            this.btnAddUnit2.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit2.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit2, "btnAddUnit2");
            this.btnAddUnit2.Name = "btnAddUnit2";
            this.btnAddUnit2.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.tbPurchaseUnitCurrent);
            this.groupBox2.Controls.Add(this.cmbPurchaseUnitNew);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnAddUnit3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tbPurchaseUnitCurrent
            // 
            resources.ApplyResources(this.tbPurchaseUnitCurrent, "tbPurchaseUnitCurrent");
            this.tbPurchaseUnitCurrent.Name = "tbPurchaseUnitCurrent";
            // 
            // cmbPurchaseUnitNew
            // 
            this.cmbPurchaseUnitNew.AddList = null;
            this.cmbPurchaseUnitNew.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPurchaseUnitNew, "cmbPurchaseUnitNew");
            this.cmbPurchaseUnitNew.MaxLength = 32767;
            this.cmbPurchaseUnitNew.Name = "cmbPurchaseUnitNew";
            this.cmbPurchaseUnitNew.OnlyDisplayID = false;
            this.cmbPurchaseUnitNew.RemoveList = null;
            this.cmbPurchaseUnitNew.RowHeight = ((short)(22));
            this.cmbPurchaseUnitNew.SelectedData = null;
            this.cmbPurchaseUnitNew.SelectedDataID = null;
            this.cmbPurchaseUnitNew.SelectionList = null;
            this.cmbPurchaseUnitNew.SkipIDColumn = true;
            this.cmbPurchaseUnitNew.RequestData += new System.EventHandler(this.cmbPurchaseUnitNew_RequestData);
            this.cmbPurchaseUnitNew.SelectedDataChanged += new System.EventHandler(this.CheckChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // btnAddUnit3
            // 
            this.btnAddUnit3.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit3.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit3, "btnAddUnit3");
            this.btnAddUnit3.Name = "btnAddUnit3";
            this.btnAddUnit3.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // ItemUnitDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.tbItem);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ItemUnitDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbItem, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButton btnAddUnit1;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbSalesUnitNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSalesUnitCurrent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbInventoryUnitCurrent;
        private DualDataComboBox cmbInventoryUnitNew;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ContextButton btnAddUnit2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbPurchaseUnitCurrent;
        private DualDataComboBox cmbPurchaseUnitNew;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private ContextButton btnAddUnit3;
    }
}