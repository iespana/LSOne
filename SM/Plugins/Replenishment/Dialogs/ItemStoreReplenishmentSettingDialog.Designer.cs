using LSOne.Controls;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    partial class ItemStoreReplenishmentSettingDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemStoreReplenishmentSettingDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbReplenishmentMethod = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ntbMaximumInventory = new LSOne.Controls.NumericTextBox();
            this.ntbReorderPoint = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.dtpBlockedDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBlocked = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ntbPurchaseOrderMultiple = new LSOne.Controls.NumericTextBox();
            this.cmbPurchaseOrderMultipleRounding = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.linkFields2 = new LSOne.Controls.LinkFields();
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cmbReplenishmentMethod
            // 
            this.cmbReplenishmentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReplenishmentMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbReplenishmentMethod, "cmbReplenishmentMethod");
            this.cmbReplenishmentMethod.Name = "cmbReplenishmentMethod";
            this.cmbReplenishmentMethod.SelectedIndexChanged += new System.EventHandler(this.cmbReplenishmentMethod_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // ntbMaximumInventory
            // 
            this.ntbMaximumInventory.AllowDecimal = true;
            this.ntbMaximumInventory.AllowNegative = false;
            this.ntbMaximumInventory.CultureInfo = null;
            this.ntbMaximumInventory.DecimalLetters = 2;
            this.ntbMaximumInventory.ForeColor = System.Drawing.Color.Black;
            this.ntbMaximumInventory.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumInventory, "ntbMaximumInventory");
            this.ntbMaximumInventory.MaxValue = 999999999999D;
            this.ntbMaximumInventory.MinValue = 0D;
            this.ntbMaximumInventory.Name = "ntbMaximumInventory";
            this.ntbMaximumInventory.Value = 0D;
            this.ntbMaximumInventory.TextChanged += new System.EventHandler(this.ntbMaximumInventory_TextChanged);
            // 
            // ntbReorderPoint
            // 
            this.ntbReorderPoint.AllowDecimal = true;
            this.ntbReorderPoint.AllowNegative = false;
            this.ntbReorderPoint.CultureInfo = null;
            this.ntbReorderPoint.DecimalLetters = 2;
            this.ntbReorderPoint.ForeColor = System.Drawing.Color.Black;
            this.ntbReorderPoint.HasMinValue = false;
            resources.ApplyResources(this.ntbReorderPoint, "ntbReorderPoint");
            this.ntbReorderPoint.MaxValue = 999999999999D;
            this.ntbReorderPoint.MinValue = 0D;
            this.ntbReorderPoint.Name = "ntbReorderPoint";
            this.ntbReorderPoint.Value = 0D;
            this.ntbReorderPoint.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // dtpBlockedDate
            // 
            resources.ApplyResources(this.dtpBlockedDate, "dtpBlockedDate");
            this.dtpBlockedDate.Name = "dtpBlockedDate";
            this.dtpBlockedDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbBlocked
            // 
            this.cmbBlocked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlocked.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBlocked, "cmbBlocked");
            this.cmbBlocked.Name = "cmbBlocked";
            this.cmbBlocked.SelectedIndexChanged += new System.EventHandler(this.cmbBlocked_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // ntbPurchaseOrderMultiple
            // 
            this.ntbPurchaseOrderMultiple.AllowDecimal = false;
            this.ntbPurchaseOrderMultiple.AllowNegative = false;
            this.ntbPurchaseOrderMultiple.CultureInfo = null;
            this.ntbPurchaseOrderMultiple.DecimalLetters = 2;
            this.ntbPurchaseOrderMultiple.ForeColor = System.Drawing.Color.Black;
            this.ntbPurchaseOrderMultiple.HasMinValue = false;
            resources.ApplyResources(this.ntbPurchaseOrderMultiple, "ntbPurchaseOrderMultiple");
            this.ntbPurchaseOrderMultiple.MaxValue = 99999999D;
            this.ntbPurchaseOrderMultiple.MinValue = 0D;
            this.ntbPurchaseOrderMultiple.Name = "ntbPurchaseOrderMultiple";
            this.ntbPurchaseOrderMultiple.Value = 0D;
            this.ntbPurchaseOrderMultiple.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbPurchaseOrderMultipleRounding
            // 
            this.cmbPurchaseOrderMultipleRounding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchaseOrderMultipleRounding.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPurchaseOrderMultipleRounding, "cmbPurchaseOrderMultipleRounding");
            this.cmbPurchaseOrderMultipleRounding.Name = "cmbPurchaseOrderMultipleRounding";
            this.cmbPurchaseOrderMultipleRounding.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Mode = LSOne.Controls.LinkFields.ModeEnum.Tripple;
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // linkFields2
            // 
            this.linkFields2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields2, "linkFields2");
            this.linkFields2.Name = "linkFields2";
            this.linkFields2.TabStop = false;
            // 
            // ItemStoreReplenishmentSettingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.linkFields2);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.dtpBlockedDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBlocked);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ntbPurchaseOrderMultiple);
            this.Controls.Add(this.cmbPurchaseOrderMultipleRounding);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbReplenishmentMethod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ntbMaximumInventory);
            this.Controls.Add(this.ntbReorderPoint);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "ItemStoreReplenishmentSettingDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.ntbReorderPoint, 0);
            this.Controls.SetChildIndex(this.ntbMaximumInventory, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbReplenishmentMethod, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.cmbPurchaseOrderMultipleRounding, 0);
            this.Controls.SetChildIndex(this.ntbPurchaseOrderMultiple, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbBlocked, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dtpBlockedDate, 0);
            this.Controls.SetChildIndex(this.linkFields1, 0);
            this.Controls.SetChildIndex(this.linkFields2, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cmbReplenishmentMethod;
        private System.Windows.Forms.Label label5;
        private NumericTextBox ntbMaximumInventory;
        private NumericTextBox ntbReorderPoint;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.DateTimePicker dtpBlockedDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBlocked;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbPurchaseOrderMultiple;
        private System.Windows.Forms.ComboBox cmbPurchaseOrderMultipleRounding;
        private System.Windows.Forms.Label label9;
        private LinkFields linkFields1;
        private LinkFields linkFields2;
    }
}