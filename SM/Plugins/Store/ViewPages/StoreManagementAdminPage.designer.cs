using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreManagementAdminPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreManagementAdminPage));
            this.label3 = new System.Windows.Forms.Label();
            this.btnEditStore = new LSOne.Controls.ContextButton();
            this.btnRecalculatePrices = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkManuallyEnterGiftCardID = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkManuallyEnterTaxGroupIDs = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkManuallyEnterTaxCodeIDs = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkManuallyEnterUnitIDs = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkManuallyEnterTerminalIDs = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkManuallyEnterStoreIDs = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkManuallyEnterVendorIDs = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkManuallyEnterCustomerIDs = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkManuallyEnterItemIDs = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbTaxExemptTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnEditTaxGroup = new LSOne.Controls.ContextButton();
            this.cmbLocalStore = new LSOne.Controls.DualDataComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnEditStore
            // 
            this.btnEditStore.BackColor = System.Drawing.Color.Transparent;
            this.btnEditStore.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditStore, "btnEditStore");
            this.btnEditStore.Name = "btnEditStore";
            this.btnEditStore.Click += new System.EventHandler(this.btnEditStore_Click);
            // 
            // btnRecalculatePrices
            // 
            resources.ApplyResources(this.btnRecalculatePrices, "btnRecalculatePrices");
            this.btnRecalculatePrices.Name = "btnRecalculatePrices";
            this.btnRecalculatePrices.UseVisualStyleBackColor = true;
            this.btnRecalculatePrices.Click += new System.EventHandler(this.btnRecalculatePrices_Click);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.btnRecalculatePrices);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.chkManuallyEnterGiftCardID);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkManuallyEnterTaxGroupIDs);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chkManuallyEnterTaxCodeIDs);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.chkManuallyEnterUnitIDs);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkManuallyEnterTerminalIDs);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.chkManuallyEnterStoreIDs);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkManuallyEnterVendorIDs);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkManuallyEnterCustomerIDs);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkManuallyEnterItemIDs);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkManuallyEnterGiftCardID
            // 
            resources.ApplyResources(this.chkManuallyEnterGiftCardID, "chkManuallyEnterGiftCardID");
            this.chkManuallyEnterGiftCardID.Name = "chkManuallyEnterGiftCardID";
            this.chkManuallyEnterGiftCardID.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkManuallyEnterTaxGroupIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterTaxGroupIDs, "chkManuallyEnterTaxGroupIDs");
            this.chkManuallyEnterTaxGroupIDs.Name = "chkManuallyEnterTaxGroupIDs";
            this.chkManuallyEnterTaxGroupIDs.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // chkManuallyEnterTaxCodeIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterTaxCodeIDs, "chkManuallyEnterTaxCodeIDs");
            this.chkManuallyEnterTaxCodeIDs.Name = "chkManuallyEnterTaxCodeIDs";
            this.chkManuallyEnterTaxCodeIDs.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // chkManuallyEnterUnitIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterUnitIDs, "chkManuallyEnterUnitIDs");
            this.chkManuallyEnterUnitIDs.Name = "chkManuallyEnterUnitIDs";
            this.chkManuallyEnterUnitIDs.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkManuallyEnterTerminalIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterTerminalIDs, "chkManuallyEnterTerminalIDs");
            this.chkManuallyEnterTerminalIDs.Name = "chkManuallyEnterTerminalIDs";
            this.chkManuallyEnterTerminalIDs.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkManuallyEnterStoreIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterStoreIDs, "chkManuallyEnterStoreIDs");
            this.chkManuallyEnterStoreIDs.Name = "chkManuallyEnterStoreIDs";
            this.chkManuallyEnterStoreIDs.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // chkManuallyEnterVendorIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterVendorIDs, "chkManuallyEnterVendorIDs");
            this.chkManuallyEnterVendorIDs.Name = "chkManuallyEnterVendorIDs";
            this.chkManuallyEnterVendorIDs.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkManuallyEnterCustomerIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterCustomerIDs, "chkManuallyEnterCustomerIDs");
            this.chkManuallyEnterCustomerIDs.Name = "chkManuallyEnterCustomerIDs";
            this.chkManuallyEnterCustomerIDs.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkManuallyEnterItemIDs
            // 
            resources.ApplyResources(this.chkManuallyEnterItemIDs, "chkManuallyEnterItemIDs");
            this.chkManuallyEnterItemIDs.Name = "chkManuallyEnterItemIDs";
            this.chkManuallyEnterItemIDs.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.cmbTaxExemptTaxGroup);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.btnEditTaxGroup);
            this.groupBox2.Controls.Add(this.cmbLocalStore);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnEditStore);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cmbTaxExemptTaxGroup
            // 
            this.cmbTaxExemptTaxGroup.AddList = null;
            this.cmbTaxExemptTaxGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTaxExemptTaxGroup, "cmbTaxExemptTaxGroup");
            this.cmbTaxExemptTaxGroup.MaxLength = 32767;
            this.cmbTaxExemptTaxGroup.Name = "cmbTaxExemptTaxGroup";
            this.cmbTaxExemptTaxGroup.NoChangeAllowed = false;
            this.cmbTaxExemptTaxGroup.OnlyDisplayID = false;
            this.cmbTaxExemptTaxGroup.RemoveList = null;
            this.cmbTaxExemptTaxGroup.RowHeight = ((short)(22));
            this.cmbTaxExemptTaxGroup.SecondaryData = null;
            this.cmbTaxExemptTaxGroup.SelectedData = null;
            this.cmbTaxExemptTaxGroup.SelectedDataID = null;
            this.cmbTaxExemptTaxGroup.SelectionList = null;
            this.cmbTaxExemptTaxGroup.SkipIDColumn = false;
            this.cmbTaxExemptTaxGroup.RequestData += new System.EventHandler(this.cmbTaxExemptTaxGroup_RequestData);
            this.cmbTaxExemptTaxGroup.SelectedDataChanged += new System.EventHandler(this.cmbTaxExemptTaxGroup_SelectedDataChanged);
            this.cmbTaxExemptTaxGroup.RequestClear += new System.EventHandler(this.cmbTaxExemptTaxGroup_RequestClear);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // btnEditTaxGroup
            // 
            this.btnEditTaxGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnEditTaxGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditTaxGroup, "btnEditTaxGroup");
            this.btnEditTaxGroup.Name = "btnEditTaxGroup";
            this.btnEditTaxGroup.Click += new System.EventHandler(this.btnNewTaxGroup_Click);
            // 
            // cmbLocalStore
            // 
            this.cmbLocalStore.AddList = null;
            this.cmbLocalStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLocalStore, "cmbLocalStore");
            this.cmbLocalStore.MaxLength = 32767;
            this.cmbLocalStore.Name = "cmbLocalStore";
            this.cmbLocalStore.NoChangeAllowed = false;
            this.cmbLocalStore.OnlyDisplayID = false;
            this.cmbLocalStore.RemoveList = null;
            this.cmbLocalStore.RowHeight = ((short)(22));
            this.cmbLocalStore.SecondaryData = null;
            this.cmbLocalStore.SelectedData = null;
            this.cmbLocalStore.SelectedDataID = null;
            this.cmbLocalStore.SelectionList = null;
            this.cmbLocalStore.SkipIDColumn = false;
            this.cmbLocalStore.RequestData += new System.EventHandler(this.cmbLocalStore_RequestData);
            this.cmbLocalStore.SelectedDataChanged += new System.EventHandler(this.DataChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // StoreManagementAdminPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.DoubleBuffered = true;
            this.Name = "StoreManagementAdminPage";
            this.Load += new System.EventHandler(this.StoreManagementAdminPage_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DualDataComboBox cmbLocalStore;
        private System.Windows.Forms.Label label3;
        private ContextButton btnEditStore;
        private System.Windows.Forms.Button btnRecalculatePrices;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkManuallyEnterCustomerIDs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkManuallyEnterItemIDs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkManuallyEnterVendorIDs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkManuallyEnterTerminalIDs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkManuallyEnterStoreIDs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkManuallyEnterUnitIDs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkManuallyEnterTaxGroupIDs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkManuallyEnterTaxCodeIDs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkManuallyEnterGiftCardID;
        private System.Windows.Forms.Label label10;
        private DualDataComboBox cmbTaxExemptTaxGroup;
        private System.Windows.Forms.Label label11;
        private ContextButton btnEditTaxGroup;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
