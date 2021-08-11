namespace LSOne.ViewPlugins.Customer.Dialogs
{
    partial class NewCustomerGroupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewCustomerGroupDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.cmbCategories = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkExclusive = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddCategory = new LSOne.Controls.ContextButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbMaxAmount = new LSOne.Controls.NumericTextBox();
            this.cmbPurchasePeriod = new System.Windows.Forms.ComboBox();
            this.lblMaxAmount = new System.Windows.Forms.Label();
            this.lblPurchasePeriod = new System.Windows.Forms.Label();
            this.chkLimitDiscount = new System.Windows.Forms.CheckBox();
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
            // cmbCategories
            // 
            this.cmbCategories.AddList = null;
            this.cmbCategories.AllowKeyboardSelection = false;
            this.cmbCategories.EnableTextBox = true;
            resources.ApplyResources(this.cmbCategories, "cmbCategories");
            this.cmbCategories.MaxLength = 32767;
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.OnlyDisplayID = false;
            this.cmbCategories.RemoveList = null;
            this.cmbCategories.RowHeight = ((short)(22));
            this.cmbCategories.SecondaryData = null;
            this.cmbCategories.SelectedData = null;
            this.cmbCategories.SelectedDataID = null;
            this.cmbCategories.SelectionList = null;
            this.cmbCategories.ShowDropDownOnTyping = true;
            this.cmbCategories.SkipIDColumn = true;
            this.cmbCategories.RequestData += new System.EventHandler(this.cmbCategories_RequestData);
            this.cmbCategories.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbCategories.RequestClear += new System.EventHandler(this.cmbCategories_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkExclusive
            // 
            resources.ApplyResources(this.chkExclusive, "chkExclusive");
            this.chkExclusive.Name = "chkExclusive";
            this.chkExclusive.UseVisualStyleBackColor = true;
            this.chkExclusive.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCategory.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCategory, "btnAddCategory");
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ntbMaxAmount
            // 
            this.ntbMaxAmount.AllowDecimal = false;
            this.ntbMaxAmount.AllowNegative = false;
            this.ntbMaxAmount.CultureInfo = null;
            this.ntbMaxAmount.DecimalLetters = 0;
            this.ntbMaxAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxAmount, "ntbMaxAmount");
            this.ntbMaxAmount.MaxValue = 999999999D;
            this.ntbMaxAmount.MinValue = 0D;
            this.ntbMaxAmount.Name = "ntbMaxAmount";
            this.ntbMaxAmount.Value = 0D;
            this.ntbMaxAmount.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbPurchasePeriod
            // 
            this.cmbPurchasePeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchasePeriod.FormattingEnabled = true;
            this.cmbPurchasePeriod.Items.AddRange(new object[] {
            resources.GetString("cmbPurchasePeriod.Items"),
            resources.GetString("cmbPurchasePeriod.Items1"),
            resources.GetString("cmbPurchasePeriod.Items2"),
            resources.GetString("cmbPurchasePeriod.Items3")});
            resources.ApplyResources(this.cmbPurchasePeriod, "cmbPurchasePeriod");
            this.cmbPurchasePeriod.Name = "cmbPurchasePeriod";
            this.cmbPurchasePeriod.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblMaxAmount
            // 
            resources.ApplyResources(this.lblMaxAmount, "lblMaxAmount");
            this.lblMaxAmount.Name = "lblMaxAmount";
            // 
            // lblPurchasePeriod
            // 
            resources.ApplyResources(this.lblPurchasePeriod, "lblPurchasePeriod");
            this.lblPurchasePeriod.Name = "lblPurchasePeriod";
            // 
            // chkLimitDiscount
            // 
            resources.ApplyResources(this.chkLimitDiscount, "chkLimitDiscount");
            this.chkLimitDiscount.Name = "chkLimitDiscount";
            this.chkLimitDiscount.UseVisualStyleBackColor = true;
            this.chkLimitDiscount.CheckedChanged += new System.EventHandler(this.chkLimitDiscount_CheckedChanged);
            // 
            // NewCustomerGroupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbPurchasePeriod);
            this.Controls.Add(this.lblMaxAmount);
            this.Controls.Add(this.lblPurchasePeriod);
            this.Controls.Add(this.chkLimitDiscount);
            this.Controls.Add(this.ntbMaxAmount);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkExclusive);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewCustomerGroupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.cmbCategories, 0);
            this.Controls.SetChildIndex(this.chkExclusive, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnAddCategory, 0);
            this.Controls.SetChildIndex(this.ntbMaxAmount, 0);
            this.Controls.SetChildIndex(this.chkLimitDiscount, 0);
            this.Controls.SetChildIndex(this.lblPurchasePeriod, 0);
            this.Controls.SetChildIndex(this.lblMaxAmount, 0);
            this.Controls.SetChildIndex(this.cmbPurchasePeriod, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescription;
        private Controls.DualDataComboBox cmbCategories;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkExclusive;
        private System.Windows.Forms.Label label1;
        private Controls.ContextButton btnAddCategory;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Controls.NumericTextBox ntbMaxAmount;
        private System.Windows.Forms.ComboBox cmbPurchasePeriod;
        private System.Windows.Forms.Label lblMaxAmount;
        private System.Windows.Forms.Label lblPurchasePeriod;
        private System.Windows.Forms.CheckBox chkLimitDiscount;
    }
}