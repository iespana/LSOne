using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class FuncProfileInfocodeTabPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuncProfileInfocodeTabPage));
            this.lvInfocodesImages = new System.Windows.Forms.ImageList(this.components);
            this.cmbFuncTenderDecl = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFuncItemNotFound = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFuncItemDisc = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbFuncTotalDisc = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFuncPriceOverride = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbFuncReturnItem = new LSOne.Controls.DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbFuncReturnTrans = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbFuncVoidItem = new LSOne.Controls.DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbFuncVoidPayment = new LSOne.Controls.DualDataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbFuncVoidTransaction = new LSOne.Controls.DualDataComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbFuncAddSalesp = new LSOne.Controls.DualDataComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkTenderDecl = new System.Windows.Forms.CheckBox();
            this.chkItemNotFound = new System.Windows.Forms.CheckBox();
            this.chkItemDisc = new System.Windows.Forms.CheckBox();
            this.chkTotalDisc = new System.Windows.Forms.CheckBox();
            this.chkPriceOver = new System.Windows.Forms.CheckBox();
            this.chkReturnItem = new System.Windows.Forms.CheckBox();
            this.chkReturnTrans = new System.Windows.Forms.CheckBox();
            this.chkVoidItem = new System.Windows.Forms.CheckBox();
            this.chkVoidPayment = new System.Windows.Forms.CheckBox();
            this.chkVoidTrans = new System.Windows.Forms.CheckBox();
            this.chkAddSalesPerson = new System.Windows.Forms.CheckBox();
            this.chkStartTrans = new System.Windows.Forms.CheckBox();
            this.cmbFuncStartTrans = new LSOne.Controls.DualDataComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkEndTrans = new System.Windows.Forms.CheckBox();
            this.cmbFuncEndTrans = new LSOne.Controls.DualDataComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.chkOpenDrawer = new System.Windows.Forms.CheckBox();
            this.cmbFuncOpenDrawer = new LSOne.Controls.DualDataComboBox();
            this.cmbStartOfTransactionType = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvInfocodesImages
            // 
            this.lvInfocodesImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.lvInfocodesImages, "lvInfocodesImages");
            this.lvInfocodesImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmbFuncTenderDecl
            // 
            this.cmbFuncTenderDecl.AddList = null;
            this.cmbFuncTenderDecl.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncTenderDecl, "cmbFuncTenderDecl");
            this.cmbFuncTenderDecl.MaxLength = 32767;
            this.cmbFuncTenderDecl.Name = "cmbFuncTenderDecl";
            this.cmbFuncTenderDecl.NoChangeAllowed = false;
            this.cmbFuncTenderDecl.OnlyDisplayID = false;
            this.cmbFuncTenderDecl.RemoveList = null;
            this.cmbFuncTenderDecl.RowHeight = ((short)(22));
            this.cmbFuncTenderDecl.SecondaryData = null;
            this.cmbFuncTenderDecl.SelectedData = null;
            this.cmbFuncTenderDecl.SelectedDataID = null;
            this.cmbFuncTenderDecl.SelectionList = null;
            this.cmbFuncTenderDecl.SkipIDColumn = true;
            this.cmbFuncTenderDecl.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbFuncItemNotFound
            // 
            this.cmbFuncItemNotFound.AddList = null;
            this.cmbFuncItemNotFound.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncItemNotFound, "cmbFuncItemNotFound");
            this.cmbFuncItemNotFound.MaxLength = 32767;
            this.cmbFuncItemNotFound.Name = "cmbFuncItemNotFound";
            this.cmbFuncItemNotFound.NoChangeAllowed = false;
            this.cmbFuncItemNotFound.OnlyDisplayID = false;
            this.cmbFuncItemNotFound.RemoveList = null;
            this.cmbFuncItemNotFound.RowHeight = ((short)(22));
            this.cmbFuncItemNotFound.SecondaryData = null;
            this.cmbFuncItemNotFound.SelectedData = null;
            this.cmbFuncItemNotFound.SelectedDataID = null;
            this.cmbFuncItemNotFound.SelectionList = null;
            this.cmbFuncItemNotFound.SkipIDColumn = true;
            this.cmbFuncItemNotFound.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbFuncItemDisc
            // 
            this.cmbFuncItemDisc.AddList = null;
            this.cmbFuncItemDisc.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncItemDisc, "cmbFuncItemDisc");
            this.cmbFuncItemDisc.MaxLength = 32767;
            this.cmbFuncItemDisc.Name = "cmbFuncItemDisc";
            this.cmbFuncItemDisc.NoChangeAllowed = false;
            this.cmbFuncItemDisc.OnlyDisplayID = false;
            this.cmbFuncItemDisc.RemoveList = null;
            this.cmbFuncItemDisc.RowHeight = ((short)(22));
            this.cmbFuncItemDisc.SecondaryData = null;
            this.cmbFuncItemDisc.SelectedData = null;
            this.cmbFuncItemDisc.SelectedDataID = null;
            this.cmbFuncItemDisc.SelectionList = null;
            this.cmbFuncItemDisc.SkipIDColumn = true;
            this.cmbFuncItemDisc.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbFuncTotalDisc
            // 
            this.cmbFuncTotalDisc.AddList = null;
            this.cmbFuncTotalDisc.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncTotalDisc, "cmbFuncTotalDisc");
            this.cmbFuncTotalDisc.MaxLength = 32767;
            this.cmbFuncTotalDisc.Name = "cmbFuncTotalDisc";
            this.cmbFuncTotalDisc.NoChangeAllowed = false;
            this.cmbFuncTotalDisc.OnlyDisplayID = false;
            this.cmbFuncTotalDisc.RemoveList = null;
            this.cmbFuncTotalDisc.RowHeight = ((short)(22));
            this.cmbFuncTotalDisc.SecondaryData = null;
            this.cmbFuncTotalDisc.SelectedData = null;
            this.cmbFuncTotalDisc.SelectedDataID = null;
            this.cmbFuncTotalDisc.SelectionList = null;
            this.cmbFuncTotalDisc.SkipIDColumn = true;
            this.cmbFuncTotalDisc.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbFuncPriceOverride
            // 
            this.cmbFuncPriceOverride.AddList = null;
            this.cmbFuncPriceOverride.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncPriceOverride, "cmbFuncPriceOverride");
            this.cmbFuncPriceOverride.MaxLength = 32767;
            this.cmbFuncPriceOverride.Name = "cmbFuncPriceOverride";
            this.cmbFuncPriceOverride.NoChangeAllowed = false;
            this.cmbFuncPriceOverride.OnlyDisplayID = false;
            this.cmbFuncPriceOverride.RemoveList = null;
            this.cmbFuncPriceOverride.RowHeight = ((short)(22));
            this.cmbFuncPriceOverride.SecondaryData = null;
            this.cmbFuncPriceOverride.SelectedData = null;
            this.cmbFuncPriceOverride.SelectedDataID = null;
            this.cmbFuncPriceOverride.SelectionList = null;
            this.cmbFuncPriceOverride.SkipIDColumn = true;
            this.cmbFuncPriceOverride.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbFuncReturnItem
            // 
            this.cmbFuncReturnItem.AddList = null;
            this.cmbFuncReturnItem.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncReturnItem, "cmbFuncReturnItem");
            this.cmbFuncReturnItem.MaxLength = 32767;
            this.cmbFuncReturnItem.Name = "cmbFuncReturnItem";
            this.cmbFuncReturnItem.NoChangeAllowed = false;
            this.cmbFuncReturnItem.OnlyDisplayID = false;
            this.cmbFuncReturnItem.RemoveList = null;
            this.cmbFuncReturnItem.RowHeight = ((short)(22));
            this.cmbFuncReturnItem.SecondaryData = null;
            this.cmbFuncReturnItem.SelectedData = null;
            this.cmbFuncReturnItem.SelectedDataID = null;
            this.cmbFuncReturnItem.SelectionList = null;
            this.cmbFuncReturnItem.SkipIDColumn = true;
            this.cmbFuncReturnItem.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbFuncReturnTrans
            // 
            this.cmbFuncReturnTrans.AddList = null;
            this.cmbFuncReturnTrans.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncReturnTrans, "cmbFuncReturnTrans");
            this.cmbFuncReturnTrans.MaxLength = 32767;
            this.cmbFuncReturnTrans.Name = "cmbFuncReturnTrans";
            this.cmbFuncReturnTrans.NoChangeAllowed = false;
            this.cmbFuncReturnTrans.OnlyDisplayID = false;
            this.cmbFuncReturnTrans.RemoveList = null;
            this.cmbFuncReturnTrans.RowHeight = ((short)(22));
            this.cmbFuncReturnTrans.SecondaryData = null;
            this.cmbFuncReturnTrans.SelectedData = null;
            this.cmbFuncReturnTrans.SelectedDataID = null;
            this.cmbFuncReturnTrans.SelectionList = null;
            this.cmbFuncReturnTrans.SkipIDColumn = true;
            this.cmbFuncReturnTrans.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbFuncVoidItem
            // 
            this.cmbFuncVoidItem.AddList = null;
            this.cmbFuncVoidItem.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncVoidItem, "cmbFuncVoidItem");
            this.cmbFuncVoidItem.MaxLength = 32767;
            this.cmbFuncVoidItem.Name = "cmbFuncVoidItem";
            this.cmbFuncVoidItem.NoChangeAllowed = false;
            this.cmbFuncVoidItem.OnlyDisplayID = false;
            this.cmbFuncVoidItem.RemoveList = null;
            this.cmbFuncVoidItem.RowHeight = ((short)(22));
            this.cmbFuncVoidItem.SecondaryData = null;
            this.cmbFuncVoidItem.SelectedData = null;
            this.cmbFuncVoidItem.SelectedDataID = null;
            this.cmbFuncVoidItem.SelectionList = null;
            this.cmbFuncVoidItem.SkipIDColumn = true;
            this.cmbFuncVoidItem.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbFuncVoidPayment
            // 
            this.cmbFuncVoidPayment.AddList = null;
            this.cmbFuncVoidPayment.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncVoidPayment, "cmbFuncVoidPayment");
            this.cmbFuncVoidPayment.MaxLength = 32767;
            this.cmbFuncVoidPayment.Name = "cmbFuncVoidPayment";
            this.cmbFuncVoidPayment.NoChangeAllowed = false;
            this.cmbFuncVoidPayment.OnlyDisplayID = false;
            this.cmbFuncVoidPayment.RemoveList = null;
            this.cmbFuncVoidPayment.RowHeight = ((short)(22));
            this.cmbFuncVoidPayment.SecondaryData = null;
            this.cmbFuncVoidPayment.SelectedData = null;
            this.cmbFuncVoidPayment.SelectedDataID = null;
            this.cmbFuncVoidPayment.SelectionList = null;
            this.cmbFuncVoidPayment.SkipIDColumn = true;
            this.cmbFuncVoidPayment.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbFuncVoidTransaction
            // 
            this.cmbFuncVoidTransaction.AddList = null;
            this.cmbFuncVoidTransaction.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncVoidTransaction, "cmbFuncVoidTransaction");
            this.cmbFuncVoidTransaction.MaxLength = 32767;
            this.cmbFuncVoidTransaction.Name = "cmbFuncVoidTransaction";
            this.cmbFuncVoidTransaction.NoChangeAllowed = false;
            this.cmbFuncVoidTransaction.OnlyDisplayID = false;
            this.cmbFuncVoidTransaction.RemoveList = null;
            this.cmbFuncVoidTransaction.RowHeight = ((short)(22));
            this.cmbFuncVoidTransaction.SecondaryData = null;
            this.cmbFuncVoidTransaction.SelectedData = null;
            this.cmbFuncVoidTransaction.SelectedDataID = null;
            this.cmbFuncVoidTransaction.SelectionList = null;
            this.cmbFuncVoidTransaction.SkipIDColumn = true;
            this.cmbFuncVoidTransaction.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbFuncAddSalesp
            // 
            this.cmbFuncAddSalesp.AddList = null;
            this.cmbFuncAddSalesp.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncAddSalesp, "cmbFuncAddSalesp");
            this.cmbFuncAddSalesp.MaxLength = 32767;
            this.cmbFuncAddSalesp.Name = "cmbFuncAddSalesp";
            this.cmbFuncAddSalesp.NoChangeAllowed = false;
            this.cmbFuncAddSalesp.OnlyDisplayID = false;
            this.cmbFuncAddSalesp.RemoveList = null;
            this.cmbFuncAddSalesp.RowHeight = ((short)(22));
            this.cmbFuncAddSalesp.SecondaryData = null;
            this.cmbFuncAddSalesp.SelectedData = null;
            this.cmbFuncAddSalesp.SelectedDataID = null;
            this.cmbFuncAddSalesp.SelectionList = null;
            this.cmbFuncAddSalesp.SkipIDColumn = true;
            this.cmbFuncAddSalesp.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // chkTenderDecl
            // 
            resources.ApplyResources(this.chkTenderDecl, "chkTenderDecl");
            this.chkTenderDecl.Name = "chkTenderDecl";
            this.chkTenderDecl.UseVisualStyleBackColor = true;
            this.chkTenderDecl.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkItemNotFound
            // 
            resources.ApplyResources(this.chkItemNotFound, "chkItemNotFound");
            this.chkItemNotFound.Name = "chkItemNotFound";
            this.chkItemNotFound.UseVisualStyleBackColor = true;
            this.chkItemNotFound.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkItemDisc
            // 
            resources.ApplyResources(this.chkItemDisc, "chkItemDisc");
            this.chkItemDisc.Name = "chkItemDisc";
            this.chkItemDisc.UseVisualStyleBackColor = true;
            this.chkItemDisc.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkTotalDisc
            // 
            resources.ApplyResources(this.chkTotalDisc, "chkTotalDisc");
            this.chkTotalDisc.Name = "chkTotalDisc";
            this.chkTotalDisc.UseVisualStyleBackColor = true;
            this.chkTotalDisc.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkPriceOver
            // 
            resources.ApplyResources(this.chkPriceOver, "chkPriceOver");
            this.chkPriceOver.Name = "chkPriceOver";
            this.chkPriceOver.UseVisualStyleBackColor = true;
            this.chkPriceOver.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkReturnItem
            // 
            resources.ApplyResources(this.chkReturnItem, "chkReturnItem");
            this.chkReturnItem.Name = "chkReturnItem";
            this.chkReturnItem.UseVisualStyleBackColor = true;
            this.chkReturnItem.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkReturnTrans
            // 
            resources.ApplyResources(this.chkReturnTrans, "chkReturnTrans");
            this.chkReturnTrans.Name = "chkReturnTrans";
            this.chkReturnTrans.UseVisualStyleBackColor = true;
            this.chkReturnTrans.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkVoidItem
            // 
            resources.ApplyResources(this.chkVoidItem, "chkVoidItem");
            this.chkVoidItem.Name = "chkVoidItem";
            this.chkVoidItem.UseVisualStyleBackColor = true;
            this.chkVoidItem.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkVoidPayment
            // 
            resources.ApplyResources(this.chkVoidPayment, "chkVoidPayment");
            this.chkVoidPayment.Name = "chkVoidPayment";
            this.chkVoidPayment.UseVisualStyleBackColor = true;
            this.chkVoidPayment.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkVoidTrans
            // 
            resources.ApplyResources(this.chkVoidTrans, "chkVoidTrans");
            this.chkVoidTrans.Name = "chkVoidTrans";
            this.chkVoidTrans.UseVisualStyleBackColor = true;
            this.chkVoidTrans.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkAddSalesPerson
            // 
            resources.ApplyResources(this.chkAddSalesPerson, "chkAddSalesPerson");
            this.chkAddSalesPerson.Name = "chkAddSalesPerson";
            this.chkAddSalesPerson.UseVisualStyleBackColor = true;
            this.chkAddSalesPerson.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // chkStartTrans
            // 
            resources.ApplyResources(this.chkStartTrans, "chkStartTrans");
            this.chkStartTrans.Name = "chkStartTrans";
            this.chkStartTrans.UseVisualStyleBackColor = true;
            this.chkStartTrans.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // cmbFuncStartTrans
            // 
            this.cmbFuncStartTrans.AddList = null;
            this.cmbFuncStartTrans.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncStartTrans, "cmbFuncStartTrans");
            this.cmbFuncStartTrans.MaxLength = 32767;
            this.cmbFuncStartTrans.Name = "cmbFuncStartTrans";
            this.cmbFuncStartTrans.NoChangeAllowed = false;
            this.cmbFuncStartTrans.OnlyDisplayID = false;
            this.cmbFuncStartTrans.RemoveList = null;
            this.cmbFuncStartTrans.RowHeight = ((short)(22));
            this.cmbFuncStartTrans.SecondaryData = null;
            this.cmbFuncStartTrans.SelectedData = null;
            this.cmbFuncStartTrans.SelectedDataID = null;
            this.cmbFuncStartTrans.SelectionList = null;
            this.cmbFuncStartTrans.SkipIDColumn = true;
            this.cmbFuncStartTrans.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // chkEndTrans
            // 
            resources.ApplyResources(this.chkEndTrans, "chkEndTrans");
            this.chkEndTrans.Name = "chkEndTrans";
            this.chkEndTrans.UseVisualStyleBackColor = true;
            this.chkEndTrans.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // cmbFuncEndTrans
            // 
            this.cmbFuncEndTrans.AddList = null;
            this.cmbFuncEndTrans.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncEndTrans, "cmbFuncEndTrans");
            this.cmbFuncEndTrans.MaxLength = 32767;
            this.cmbFuncEndTrans.Name = "cmbFuncEndTrans";
            this.cmbFuncEndTrans.NoChangeAllowed = false;
            this.cmbFuncEndTrans.OnlyDisplayID = false;
            this.cmbFuncEndTrans.RemoveList = null;
            this.cmbFuncEndTrans.RowHeight = ((short)(22));
            this.cmbFuncEndTrans.SecondaryData = null;
            this.cmbFuncEndTrans.SelectedData = null;
            this.cmbFuncEndTrans.SelectedDataID = null;
            this.cmbFuncEndTrans.SelectionList = null;
            this.cmbFuncEndTrans.SkipIDColumn = true;
            this.cmbFuncEndTrans.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestData);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.chkOpenDrawer);
            this.groupBox1.Controls.Add(this.cmbFuncOpenDrawer);
            this.groupBox1.Controls.Add(this.cmbStartOfTransactionType);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.chkEndTrans);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbFuncEndTrans);
            this.groupBox1.Controls.Add(this.cmbFuncTenderDecl);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkStartTrans);
            this.groupBox1.Controls.Add(this.cmbFuncItemNotFound);
            this.groupBox1.Controls.Add(this.cmbFuncStartTrans);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbFuncItemDisc);
            this.groupBox1.Controls.Add(this.chkAddSalesPerson);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkVoidTrans);
            this.groupBox1.Controls.Add(this.cmbFuncTotalDisc);
            this.groupBox1.Controls.Add(this.chkVoidPayment);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkVoidItem);
            this.groupBox1.Controls.Add(this.cmbFuncPriceOverride);
            this.groupBox1.Controls.Add(this.chkReturnTrans);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.chkReturnItem);
            this.groupBox1.Controls.Add(this.cmbFuncReturnItem);
            this.groupBox1.Controls.Add(this.chkPriceOver);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkTotalDisc);
            this.groupBox1.Controls.Add(this.cmbFuncReturnTrans);
            this.groupBox1.Controls.Add(this.chkItemDisc);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chkItemNotFound);
            this.groupBox1.Controls.Add(this.cmbFuncVoidItem);
            this.groupBox1.Controls.Add(this.chkTenderDecl);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmbFuncAddSalesp);
            this.groupBox1.Controls.Add(this.cmbFuncVoidPayment);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbFuncVoidTransaction);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // chkOpenDrawer
            // 
            resources.ApplyResources(this.chkOpenDrawer, "chkOpenDrawer");
            this.chkOpenDrawer.Name = "chkOpenDrawer";
            this.chkOpenDrawer.UseVisualStyleBackColor = true;
            this.chkOpenDrawer.CheckedChanged += new System.EventHandler(this.chkTenderDecl_CheckedChanged);
            // 
            // cmbFuncOpenDrawer
            // 
            this.cmbFuncOpenDrawer.AddList = null;
            this.cmbFuncOpenDrawer.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFuncOpenDrawer, "cmbFuncOpenDrawer");
            this.cmbFuncOpenDrawer.MaxLength = 32767;
            this.cmbFuncOpenDrawer.Name = "cmbFuncOpenDrawer";
            this.cmbFuncOpenDrawer.NoChangeAllowed = false;
            this.cmbFuncOpenDrawer.OnlyDisplayID = false;
            this.cmbFuncOpenDrawer.RemoveList = null;
            this.cmbFuncOpenDrawer.RowHeight = ((short)(22));
            this.cmbFuncOpenDrawer.SecondaryData = null;
            this.cmbFuncOpenDrawer.SelectedData = null;
            this.cmbFuncOpenDrawer.SelectedDataID = null;
            this.cmbFuncOpenDrawer.SelectionList = null;
            this.cmbFuncOpenDrawer.SkipIDColumn = true;
            this.cmbFuncOpenDrawer.RequestData += new System.EventHandler(this.cmbFuncTenderDecl_RequestDateExcludingItemAndCust);
            // 
            // cmbStartOfTransactionType
            // 
            this.cmbStartOfTransactionType.FormattingEnabled = true;
            this.cmbStartOfTransactionType.Items.AddRange(new object[] {
            resources.GetString("cmbStartOfTransactionType.Items"),
            resources.GetString("cmbStartOfTransactionType.Items1")});
            resources.ApplyResources(this.cmbStartOfTransactionType, "cmbStartOfTransactionType");
            this.cmbStartOfTransactionType.Name = "cmbStartOfTransactionType";
            // 
            // FuncProfileInfocodeTabPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "FuncProfileInfocodeTabPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList lvInfocodesImages;
        private DualDataComboBox cmbFuncTenderDecl;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbFuncItemNotFound;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbFuncItemDisc;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbFuncTotalDisc;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbFuncPriceOverride;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbFuncReturnItem;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbFuncReturnTrans;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbFuncVoidItem;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbFuncVoidPayment;
        private System.Windows.Forms.Label label9;
        private DualDataComboBox cmbFuncVoidTransaction;
        private System.Windows.Forms.Label label10;
        private DualDataComboBox cmbFuncAddSalesp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkTenderDecl;
        private System.Windows.Forms.CheckBox chkItemNotFound;
        private System.Windows.Forms.CheckBox chkItemDisc;
        private System.Windows.Forms.CheckBox chkTotalDisc;
        private System.Windows.Forms.CheckBox chkPriceOver;
        private System.Windows.Forms.CheckBox chkReturnItem;
        private System.Windows.Forms.CheckBox chkReturnTrans;
        private System.Windows.Forms.CheckBox chkVoidItem;
        private System.Windows.Forms.CheckBox chkVoidPayment;
        private System.Windows.Forms.CheckBox chkVoidTrans;
        private System.Windows.Forms.CheckBox chkAddSalesPerson;
        private System.Windows.Forms.CheckBox chkStartTrans;
        private DualDataComboBox cmbFuncStartTrans;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkEndTrans;
        private DualDataComboBox cmbFuncEndTrans;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbStartOfTransactionType;
        private System.Windows.Forms.CheckBox chkOpenDrawer;
        private DualDataComboBox cmbFuncOpenDrawer;
        private System.Windows.Forms.Label label14;
    }
}
