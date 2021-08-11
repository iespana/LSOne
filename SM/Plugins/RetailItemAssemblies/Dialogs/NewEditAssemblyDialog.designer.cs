using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItemAssemblies.Dialogs
{
    partial class NewEditAssemblyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEditAssemblyDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.chkShowComponentsOnPos = new System.Windows.Forms.CheckBox();
            this.chkShowComponentsOnReceipt = new System.Windows.Forms.CheckBox();
            this.lblShowComponents = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.dtpStartingDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartingDate = new System.Windows.Forms.Label();
            this.lblMargin = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.ntbTotalCost = new LSOne.Controls.NumericTextBox();
            this.ntbPrice = new LSOne.Controls.NumericTextBox();
            this.ntbMargin = new LSOne.Controls.NumericTextBox();
            this.chkAllStores = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCalcPriceFromComps = new System.Windows.Forms.Label();
            this.chkCalculatePriceFromComponents = new System.Windows.Forms.CheckBox();
            this.lblSendComponentsToKds = new System.Windows.Forms.Label();
            this.cmbSendComponentsToKds = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.cbCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // cbCreateAnother
            // 
            resources.ApplyResources(this.cbCreateAnother, "cbCreateAnother");
            this.cbCreateAnother.Name = "cbCreateAnother";
            this.cbCreateAnother.UseVisualStyleBackColor = true;
            this.cbCreateAnother.CheckedChanged += new System.EventHandler(this.cbCreateAnother_CheckedChanged);
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
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            this.cmbCopyFrom.EnableTextBox = true;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.ShowDropDownOnTyping = true;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            this.cmbCopyFrom.RequestClear += new System.EventHandler(this.cmbCopyFrom_ClearData);
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTotalCost, "lblTotalCost");
            this.lblTotalCost.Name = "lblTotalCost";
            // 
            // chkShowComponentsOnPos
            // 
            resources.ApplyResources(this.chkShowComponentsOnPos, "chkShowComponentsOnPos");
            this.chkShowComponentsOnPos.Name = "chkShowComponentsOnPos";
            this.chkShowComponentsOnPos.UseVisualStyleBackColor = true;
            this.chkShowComponentsOnPos.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // chkShowComponentsOnReceipt
            // 
            resources.ApplyResources(this.chkShowComponentsOnReceipt, "chkShowComponentsOnReceipt");
            this.chkShowComponentsOnReceipt.Name = "chkShowComponentsOnReceipt";
            this.chkShowComponentsOnReceipt.UseVisualStyleBackColor = true;
            this.chkShowComponentsOnReceipt.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblShowComponents
            // 
            this.lblShowComponents.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblShowComponents, "lblShowComponents");
            this.lblShowComponents.Name = "lblShowComponents";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            this.cmbStore.EnableTextBox = true;
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
            this.cmbStore.ShowDropDownOnTyping = true;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblStore
            // 
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.Name = "lblStore";
            // 
            // dtpStartingDate
            // 
            this.dtpStartingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpStartingDate, "dtpStartingDate");
            this.dtpStartingDate.Name = "dtpStartingDate";
            this.dtpStartingDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblStartingDate
            // 
            this.lblStartingDate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStartingDate, "lblStartingDate");
            this.lblStartingDate.Name = "lblStartingDate";
            // 
            // lblMargin
            // 
            this.lblMargin.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMargin, "lblMargin");
            this.lblMargin.Name = "lblMargin";
            // 
            // lblPrice
            // 
            this.lblPrice.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPrice, "lblPrice");
            this.lblPrice.Name = "lblPrice";
            // 
            // ntbTotalCost
            // 
            this.ntbTotalCost.AllowDecimal = true;
            this.ntbTotalCost.AllowNegative = false;
            this.ntbTotalCost.CultureInfo = null;
            this.ntbTotalCost.DecimalLetters = 2;
            resources.ApplyResources(this.ntbTotalCost, "ntbTotalCost");
            this.ntbTotalCost.ForeColor = System.Drawing.Color.Black;
            this.ntbTotalCost.HasMinValue = false;
            this.ntbTotalCost.MaxValue = 0D;
            this.ntbTotalCost.MinValue = 0D;
            this.ntbTotalCost.Name = "ntbTotalCost";
            this.ntbTotalCost.Value = 0D;
            // 
            // ntbPrice
            // 
            this.ntbPrice.AllowDecimal = true;
            this.ntbPrice.AllowNegative = false;
            this.ntbPrice.CultureInfo = null;
            this.ntbPrice.DecimalLetters = 2;
            this.ntbPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbPrice, "ntbPrice");
            this.ntbPrice.MaxValue = 9999999D;
            this.ntbPrice.MinValue = 0D;
            this.ntbPrice.Name = "ntbPrice";
            this.ntbPrice.Value = 0D;
            this.ntbPrice.TextChanged += new System.EventHandler(this.ntbPrice_TextChanged);
            // 
            // ntbMargin
            // 
            this.ntbMargin.AllowDecimal = true;
            this.ntbMargin.AllowNegative = false;
            this.ntbMargin.CultureInfo = null;
            this.ntbMargin.DecimalLetters = 2;
            this.ntbMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbMargin.HasMinValue = false;
            resources.ApplyResources(this.ntbMargin, "ntbMargin");
            this.ntbMargin.MaxValue = 100D;
            this.ntbMargin.MinValue = 0D;
            this.ntbMargin.Name = "ntbMargin";
            this.ntbMargin.Value = 0D;
            this.ntbMargin.TextChanged += new System.EventHandler(this.ntbMargin_TextChanged);
            // 
            // chkAllStores
            // 
            resources.ApplyResources(this.chkAllStores, "chkAllStores");
            this.chkAllStores.Name = "chkAllStores";
            this.chkAllStores.UseVisualStyleBackColor = true;
            this.chkAllStores.CheckedChanged += new System.EventHandler(this.chkAllStores_CheckedChanged);
            // 
            // txtDescription
            // 
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblCalcPriceFromComps
            // 
            this.lblCalcPriceFromComps.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCalcPriceFromComps, "lblCalcPriceFromComps");
            this.lblCalcPriceFromComps.Name = "lblCalcPriceFromComps";
            // 
            // chkCalculatePriceFromComponents
            // 
            resources.ApplyResources(this.chkCalculatePriceFromComponents, "chkCalculatePriceFromComponents");
            this.chkCalculatePriceFromComponents.Name = "chkCalculatePriceFromComponents";
            this.chkCalculatePriceFromComponents.UseVisualStyleBackColor = true;
            this.chkCalculatePriceFromComponents.CheckedChanged += new System.EventHandler(this.chkCalculatePriceFromComponents_CheckedChanged);
            // 
            // lblSendComponentsToKds
            // 
            this.lblSendComponentsToKds.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSendComponentsToKds, "lblSendComponentsToKds");
            this.lblSendComponentsToKds.Name = "lblSendComponentsToKds";
            // 
            // cmbSendComponentsToKds
            // 
            this.cmbSendComponentsToKds.AddList = null;
            this.cmbSendComponentsToKds.AllowKeyboardSelection = false;
            this.cmbSendComponentsToKds.EnableTextBox = true;
            resources.ApplyResources(this.cmbSendComponentsToKds, "cmbSendComponentsToKds");
            this.cmbSendComponentsToKds.MaxLength = 32767;
            this.cmbSendComponentsToKds.Name = "cmbSendComponentsToKds";
            this.cmbSendComponentsToKds.NoChangeAllowed = false;
            this.cmbSendComponentsToKds.OnlyDisplayID = false;
            this.cmbSendComponentsToKds.RemoveList = null;
            this.cmbSendComponentsToKds.RowHeight = ((short)(22));
            this.cmbSendComponentsToKds.SecondaryData = null;
            this.cmbSendComponentsToKds.SelectedData = null;
            this.cmbSendComponentsToKds.SelectedDataID = null;
            this.cmbSendComponentsToKds.SelectionList = null;
            this.cmbSendComponentsToKds.ShowDropDownOnTyping = true;
            this.cmbSendComponentsToKds.SkipIDColumn = true;
            this.cmbSendComponentsToKds.RequestData += new System.EventHandler(this.cmbShowComponentsOnKds_RequestData);
            this.cmbSendComponentsToKds.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // NewEditAssemblyDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbSendComponentsToKds);
            this.Controls.Add(this.lblSendComponentsToKds);
            this.Controls.Add(this.chkCalculatePriceFromComponents);
            this.Controls.Add(this.lblCalcPriceFromComps);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.chkAllStores);
            this.Controls.Add(this.ntbMargin);
            this.Controls.Add(this.ntbPrice);
            this.Controls.Add(this.ntbTotalCost);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblMargin);
            this.Controls.Add(this.dtpStartingDate);
            this.Controls.Add(this.lblStartingDate);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.lblShowComponents);
            this.Controls.Add(this.chkShowComponentsOnPos);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.lblCopyFrom);
            this.Controls.Add(this.lblTotalCost);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.chkShowComponentsOnReceipt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewEditAssemblyDialog";
            this.Controls.SetChildIndex(this.chkShowComponentsOnReceipt, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblTotalCost, 0);
            this.Controls.SetChildIndex(this.lblCopyFrom, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.chkShowComponentsOnPos, 0);
            this.Controls.SetChildIndex(this.lblShowComponents, 0);
            this.Controls.SetChildIndex(this.lblStore, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.Controls.SetChildIndex(this.lblStartingDate, 0);
            this.Controls.SetChildIndex(this.dtpStartingDate, 0);
            this.Controls.SetChildIndex(this.lblMargin, 0);
            this.Controls.SetChildIndex(this.lblPrice, 0);
            this.Controls.SetChildIndex(this.ntbTotalCost, 0);
            this.Controls.SetChildIndex(this.ntbPrice, 0);
            this.Controls.SetChildIndex(this.ntbMargin, 0);
            this.Controls.SetChildIndex(this.chkAllStores, 0);
            this.Controls.SetChildIndex(this.txtDescription, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.lblCalcPriceFromComps, 0);
            this.Controls.SetChildIndex(this.chkCalculatePriceFromComponents, 0);
            this.Controls.SetChildIndex(this.lblSendComponentsToKds, 0);
            this.Controls.SetChildIndex(this.cmbSendComponentsToKds, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label lblCopyFrom;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.CheckBox chkShowComponentsOnPos;
        private System.Windows.Forms.CheckBox chkShowComponentsOnReceipt;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.Label lblShowComponents;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblMargin;
        private System.Windows.Forms.DateTimePicker dtpStartingDate;
        private System.Windows.Forms.Label lblStartingDate;
        private System.Windows.Forms.CheckBox cbCreateAnother;
        private NumericTextBox ntbMargin;
        private NumericTextBox ntbPrice;
        private NumericTextBox ntbTotalCost;
        private System.Windows.Forms.CheckBox chkAllStores;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkCalculatePriceFromComponents;
        private System.Windows.Forms.Label lblCalcPriceFromComps;
        private System.Windows.Forms.Label lblSendComponentsToKds;
        private DualDataComboBox cmbSendComponentsToKds;
    }
}