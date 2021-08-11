using LSOne.Controls;

namespace LSOne.ViewPlugins.LSCommerce.ViewPages
{
    partial class FunctionalityProfilePage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalityProfilePage));
			this.lblSuspensionType = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblMainMenu = new System.Windows.Forms.Label();
			this.btnEditImageItemLookupGroup = new LSOne.Controls.ContextButton();
			this.btnAddSuspensionType = new LSOne.Controls.ContextButton();
			this.btnEditPrintingStation = new LSOne.Controls.ContextButton();
			this.cmbMainMenu = new LSOne.Controls.DualDataComboBox();
			this.cmbSuspensionTypes = new LSOne.Controls.DualDataComboBox();
			this.cmbPrintingStation = new LSOne.Controls.DualDataComboBox();
			this.cmbItemImageLookupGroup = new LSOne.Controls.DualDataComboBox();
			this.btnEditMainMenu = new LSOne.Controls.ContextButton();
			this.grpMobileInventory = new System.Windows.Forms.GroupBox();
			this.linkFields2 = new LSOne.Controls.LinkFields();
			this.cmbEnteringType = new System.Windows.Forms.ComboBox();
			this.lblTypeOfEntering = new System.Windows.Forms.Label();
			this.lblDefaultQty = new System.Windows.Forms.Label();
			this.ntDefaultQty = new LSOne.Controls.NumericTextBox();
			this.cmbQuantityMethod = new System.Windows.Forms.ComboBox();
			this.lblQtyMethod = new System.Windows.Forms.Label();
			this.grpMobilePOS = new System.Windows.Forms.GroupBox();
			this.chkShowLSCommerceInventory = new System.Windows.Forms.CheckBox();
			this.lblShowMobileInventory = new System.Windows.Forms.Label();
			this.chkAllowOfflineTrans = new System.Windows.Forms.CheckBox();
			this.lblAllowOfflineTrans = new System.Windows.Forms.Label();
			this.grpMobileInventory.SuspendLayout();
			this.grpMobilePOS.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblSuspensionType
			// 
			this.lblSuspensionType.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblSuspensionType, "lblSuspensionType");
			this.lblSuspensionType.Name = "lblSuspensionType";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// lblMainMenu
			// 
			this.lblMainMenu.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblMainMenu, "lblMainMenu");
			this.lblMainMenu.Name = "lblMainMenu";
			// 
			// btnEditImageItemLookupGroup
			// 
			this.btnEditImageItemLookupGroup.BackColor = System.Drawing.Color.Transparent;
			this.btnEditImageItemLookupGroup.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditImageItemLookupGroup, "btnEditImageItemLookupGroup");
			this.btnEditImageItemLookupGroup.Name = "btnEditImageItemLookupGroup";
			this.btnEditImageItemLookupGroup.Click += new System.EventHandler(this.btnEditImageItemLookupGroup_Click);
			// 
			// btnAddSuspensionType
			// 
			this.btnAddSuspensionType.BackColor = System.Drawing.Color.Transparent;
			this.btnAddSuspensionType.Context = LSOne.Controls.ButtonType.Add;
			resources.ApplyResources(this.btnAddSuspensionType, "btnAddSuspensionType");
			this.btnAddSuspensionType.Name = "btnAddSuspensionType";
			this.btnAddSuspensionType.Click += new System.EventHandler(this.btnAddSuspensionType_Click);
			// 
			// btnEditPrintingStation
			// 
			this.btnEditPrintingStation.BackColor = System.Drawing.Color.Transparent;
			this.btnEditPrintingStation.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditPrintingStation, "btnEditPrintingStation");
			this.btnEditPrintingStation.Name = "btnEditPrintingStation";
			this.btnEditPrintingStation.Click += new System.EventHandler(this.btnEditPrintingStation_Click);
			// 
			// cmbMainMenu
			// 
			this.cmbMainMenu.AddList = null;
			this.cmbMainMenu.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbMainMenu, "cmbMainMenu");
			this.cmbMainMenu.MaxLength = 32767;
			this.cmbMainMenu.Name = "cmbMainMenu";
			this.cmbMainMenu.NoChangeAllowed = false;
			this.cmbMainMenu.OnlyDisplayID = false;
			this.cmbMainMenu.RemoveList = null;
			this.cmbMainMenu.RowHeight = ((short)(22));
			this.cmbMainMenu.SecondaryData = null;
			this.cmbMainMenu.SelectedData = null;
			this.cmbMainMenu.SelectedDataID = null;
			this.cmbMainMenu.SelectionList = null;
			this.cmbMainMenu.SkipIDColumn = true;
			this.cmbMainMenu.UsesMasterDataEntity = true;
			this.cmbMainMenu.RequestData += new System.EventHandler(this.cmbMainMenu_RequestData);
			// 
			// cmbSuspensionTypes
			// 
			this.cmbSuspensionTypes.AddList = null;
			this.cmbSuspensionTypes.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbSuspensionTypes, "cmbSuspensionTypes");
			this.cmbSuspensionTypes.MaxLength = 32767;
			this.cmbSuspensionTypes.Name = "cmbSuspensionTypes";
			this.cmbSuspensionTypes.NoChangeAllowed = false;
			this.cmbSuspensionTypes.OnlyDisplayID = false;
			this.cmbSuspensionTypes.RemoveList = null;
			this.cmbSuspensionTypes.RowHeight = ((short)(22));
			this.cmbSuspensionTypes.SecondaryData = null;
			this.cmbSuspensionTypes.SelectedData = null;
			this.cmbSuspensionTypes.SelectedDataID = null;
			this.cmbSuspensionTypes.SelectionList = null;
			this.cmbSuspensionTypes.SkipIDColumn = true;
			this.cmbSuspensionTypes.RequestData += new System.EventHandler(this.cmbSuspensionTypes_RequestData);
			// 
			// cmbPrintingStation
			// 
			this.cmbPrintingStation.AddList = null;
			this.cmbPrintingStation.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbPrintingStation, "cmbPrintingStation");
			this.cmbPrintingStation.MaxLength = 32767;
			this.cmbPrintingStation.Name = "cmbPrintingStation";
			this.cmbPrintingStation.NoChangeAllowed = false;
			this.cmbPrintingStation.OnlyDisplayID = false;
			this.cmbPrintingStation.RemoveList = null;
			this.cmbPrintingStation.RowHeight = ((short)(22));
			this.cmbPrintingStation.SecondaryData = null;
			this.cmbPrintingStation.SelectedData = null;
			this.cmbPrintingStation.SelectedDataID = null;
			this.cmbPrintingStation.SelectionList = null;
			this.cmbPrintingStation.SkipIDColumn = true;
			this.cmbPrintingStation.RequestData += new System.EventHandler(this.cmbPrintingStation_RequestData);
			// 
			// cmbItemImageLookupGroup
			// 
			this.cmbItemImageLookupGroup.AddList = null;
			this.cmbItemImageLookupGroup.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbItemImageLookupGroup, "cmbItemImageLookupGroup");
			this.cmbItemImageLookupGroup.MaxLength = 32767;
			this.cmbItemImageLookupGroup.Name = "cmbItemImageLookupGroup";
			this.cmbItemImageLookupGroup.NoChangeAllowed = false;
			this.cmbItemImageLookupGroup.OnlyDisplayID = false;
			this.cmbItemImageLookupGroup.RemoveList = null;
			this.cmbItemImageLookupGroup.RowHeight = ((short)(22));
			this.cmbItemImageLookupGroup.SecondaryData = null;
			this.cmbItemImageLookupGroup.SelectedData = null;
			this.cmbItemImageLookupGroup.SelectedDataID = null;
			this.cmbItemImageLookupGroup.SelectionList = null;
			this.cmbItemImageLookupGroup.SkipIDColumn = true;
			this.cmbItemImageLookupGroup.UsesMasterDataEntity = true;
			this.cmbItemImageLookupGroup.RequestData += new System.EventHandler(this.cmbItemImageLookupGroup_RequestData);
			this.cmbItemImageLookupGroup.RequestClear += new System.EventHandler(this.cmbItemImageLookupGroup_RequestClear);
			// 
			// btnEditMainMenu
			// 
			this.btnEditMainMenu.BackColor = System.Drawing.Color.Transparent;
			this.btnEditMainMenu.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditMainMenu, "btnEditMainMenu");
			this.btnEditMainMenu.Name = "btnEditMainMenu";
			this.btnEditMainMenu.Click += new System.EventHandler(this.btnEditMainMenu_Click);
			// 
			// grpMobileInventory
			// 
			resources.ApplyResources(this.grpMobileInventory, "grpMobileInventory");
			this.grpMobileInventory.Controls.Add(this.linkFields2);
			this.grpMobileInventory.Controls.Add(this.cmbEnteringType);
			this.grpMobileInventory.Controls.Add(this.lblTypeOfEntering);
			this.grpMobileInventory.Controls.Add(this.lblDefaultQty);
			this.grpMobileInventory.Controls.Add(this.ntDefaultQty);
			this.grpMobileInventory.Controls.Add(this.cmbQuantityMethod);
			this.grpMobileInventory.Controls.Add(this.lblQtyMethod);
			this.grpMobileInventory.Controls.Add(this.btnEditMainMenu);
			this.grpMobileInventory.Controls.Add(this.lblMainMenu);
			this.grpMobileInventory.Controls.Add(this.cmbMainMenu);
			this.grpMobileInventory.Name = "grpMobileInventory";
			this.grpMobileInventory.TabStop = false;
			// 
			// linkFields2
			// 
			this.linkFields2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.linkFields2, "linkFields2");
			this.linkFields2.Name = "linkFields2";
			this.linkFields2.TabStop = false;
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
			// 
			// lblQtyMethod
			// 
			this.lblQtyMethod.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblQtyMethod, "lblQtyMethod");
			this.lblQtyMethod.Name = "lblQtyMethod";
			// 
			// grpMobilePOS
			// 
			resources.ApplyResources(this.grpMobilePOS, "grpMobilePOS");
			this.grpMobilePOS.Controls.Add(this.chkShowLSCommerceInventory);
			this.grpMobilePOS.Controls.Add(this.lblShowMobileInventory);
			this.grpMobilePOS.Controls.Add(this.btnEditImageItemLookupGroup);
			this.grpMobilePOS.Controls.Add(this.chkAllowOfflineTrans);
			this.grpMobilePOS.Controls.Add(this.cmbItemImageLookupGroup);
			this.grpMobilePOS.Controls.Add(this.lblAllowOfflineTrans);
			this.grpMobilePOS.Controls.Add(this.label2);
			this.grpMobilePOS.Controls.Add(this.cmbPrintingStation);
			this.grpMobilePOS.Controls.Add(this.btnEditPrintingStation);
			this.grpMobilePOS.Controls.Add(this.lblSuspensionType);
			this.grpMobilePOS.Controls.Add(this.cmbSuspensionTypes);
			this.grpMobilePOS.Controls.Add(this.label1);
			this.grpMobilePOS.Controls.Add(this.btnAddSuspensionType);
			this.grpMobilePOS.Name = "grpMobilePOS";
			this.grpMobilePOS.TabStop = false;
			// 
			// chkShowLSCommerceInventory
			// 
			resources.ApplyResources(this.chkShowLSCommerceInventory, "chkShowLSCommerceInventory");
			this.chkShowLSCommerceInventory.Name = "chkShowLSCommerceInventory";
			this.chkShowLSCommerceInventory.UseVisualStyleBackColor = true;
			// 
			// lblShowMobileInventory
			// 
			this.lblShowMobileInventory.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblShowMobileInventory, "lblShowMobileInventory");
			this.lblShowMobileInventory.Name = "lblShowMobileInventory";
			// 
			// chkAllowOfflineTrans
			// 
			resources.ApplyResources(this.chkAllowOfflineTrans, "chkAllowOfflineTrans");
			this.chkAllowOfflineTrans.Name = "chkAllowOfflineTrans";
			this.chkAllowOfflineTrans.UseVisualStyleBackColor = true;
			// 
			// lblAllowOfflineTrans
			// 
			this.lblAllowOfflineTrans.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblAllowOfflineTrans, "lblAllowOfflineTrans");
			this.lblAllowOfflineTrans.Name = "lblAllowOfflineTrans";
			// 
			// FunctionalityProfilePage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.grpMobilePOS);
			this.Controls.Add(this.grpMobileInventory);
			this.DoubleBuffered = true;
			this.Name = "FunctionalityProfilePage";
			this.grpMobileInventory.ResumeLayout(false);
			this.grpMobileInventory.PerformLayout();
			this.grpMobilePOS.ResumeLayout(false);
			this.grpMobilePOS.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private DualDataComboBox cmbSuspensionTypes;
        private System.Windows.Forms.Label lblSuspensionType;
        private ContextButton btnAddSuspensionType;
        private ContextButton btnEditPrintingStation;
        private DualDataComboBox cmbPrintingStation;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbItemImageLookupGroup;
        private System.Windows.Forms.Label label2;
        private ContextButton btnEditImageItemLookupGroup;
        private DualDataComboBox cmbMainMenu;
        private System.Windows.Forms.Label lblMainMenu;
        private ContextButton btnEditMainMenu;
        private System.Windows.Forms.GroupBox grpMobileInventory;
        private LinkFields linkFields2;
        private System.Windows.Forms.ComboBox cmbEnteringType;
        private System.Windows.Forms.Label lblTypeOfEntering;
        private System.Windows.Forms.Label lblDefaultQty;
        private NumericTextBox ntDefaultQty;
        private System.Windows.Forms.ComboBox cmbQuantityMethod;
        private System.Windows.Forms.Label lblQtyMethod;
		private System.Windows.Forms.GroupBox grpMobilePOS;
		private System.Windows.Forms.CheckBox chkAllowOfflineTrans;
		private System.Windows.Forms.Label lblAllowOfflineTrans;
		private System.Windows.Forms.CheckBox chkShowLSCommerceInventory;
		private System.Windows.Forms.Label lblShowMobileInventory;
	}
}
