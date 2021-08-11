using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreSettingsPage));
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDefaultCustomer = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.cmbPriceTaxSetting = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbKeyboard = new System.Windows.Forms.ComboBox();
            this.cmbFunctionalProfile = new LSOne.Controls.DualDataComboBox();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.cmbCurrencies = new LSOne.Controls.DualDataComboBox();
            this.cmbTouchLayout = new LSOne.Controls.DualDataComboBox();
            this.cmbOperationAuditing = new System.Windows.Forms.ComboBox();
            this.lblAuditOperations = new System.Windows.Forms.Label();
            this.lblStartAmount = new System.Windows.Forms.Label();
            this.addressField = new LSOne.Controls.AddressControl();
            this.cmbRegion = new LSOne.Controls.DualDataComboBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.btnAddRegion = new LSOne.Controls.ContextButton();
            this.btnEditRegion = new LSOne.Controls.ContextButton();
            this.ntbStartAmount = new LSOne.Controls.NumericTextBox();
            this.btnAddCustomer = new LSOne.Controls.ContextButton();
            this.btnEditFunctionalProfile = new LSOne.Controls.ContextButton();
            this.btnEditSalesTaxGroup = new LSOne.Controls.ContextButton();
            this.btnCurrenciesEdit = new LSOne.Controls.ContextButton();
            this.btnEditTouchButtons = new LSOne.Controls.ContextButton();
            this.btnEditCustomer = new LSOne.Controls.ContextButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            resources.GetString("cmbLanguage.Items"),
            resources.GetString("cmbLanguage.Items1"),
            resources.GetString("cmbLanguage.Items2"),
            resources.GetString("cmbLanguage.Items3"),
            resources.GetString("cmbLanguage.Items4"),
            resources.GetString("cmbLanguage.Items5"),
            resources.GetString("cmbLanguage.Items6"),
            resources.GetString("cmbLanguage.Items7"),
            resources.GetString("cmbLanguage.Items8"),
            resources.GetString("cmbLanguage.Items9"),
            resources.GetString("cmbLanguage.Items10"),
            resources.GetString("cmbLanguage.Items11"),
            resources.GetString("cmbLanguage.Items12"),
            resources.GetString("cmbLanguage.Items13"),
            resources.GetString("cmbLanguage.Items14"),
            resources.GetString("cmbLanguage.Items15"),
            resources.GetString("cmbLanguage.Items16"),
            resources.GetString("cmbLanguage.Items17"),
            resources.GetString("cmbLanguage.Items18"),
            resources.GetString("cmbLanguage.Items19"),
            resources.GetString("cmbLanguage.Items20"),
            resources.GetString("cmbLanguage.Items21"),
            resources.GetString("cmbLanguage.Items22"),
            resources.GetString("cmbLanguage.Items23"),
            resources.GetString("cmbLanguage.Items24"),
            resources.GetString("cmbLanguage.Items25"),
            resources.GetString("cmbLanguage.Items26"),
            resources.GetString("cmbLanguage.Items27"),
            resources.GetString("cmbLanguage.Items28"),
            resources.GetString("cmbLanguage.Items29"),
            resources.GetString("cmbLanguage.Items30"),
            resources.GetString("cmbLanguage.Items31"),
            resources.GetString("cmbLanguage.Items32"),
            resources.GetString("cmbLanguage.Items33"),
            resources.GetString("cmbLanguage.Items34"),
            resources.GetString("cmbLanguage.Items35"),
            resources.GetString("cmbLanguage.Items36"),
            resources.GetString("cmbLanguage.Items37"),
            resources.GetString("cmbLanguage.Items38"),
            resources.GetString("cmbLanguage.Items39"),
            resources.GetString("cmbLanguage.Items40"),
            resources.GetString("cmbLanguage.Items41"),
            resources.GetString("cmbLanguage.Items42"),
            resources.GetString("cmbLanguage.Items43"),
            resources.GetString("cmbLanguage.Items44"),
            resources.GetString("cmbLanguage.Items45"),
            resources.GetString("cmbLanguage.Items46"),
            resources.GetString("cmbLanguage.Items47"),
            resources.GetString("cmbLanguage.Items48"),
            resources.GetString("cmbLanguage.Items49"),
            resources.GetString("cmbLanguage.Items50"),
            resources.GetString("cmbLanguage.Items51"),
            resources.GetString("cmbLanguage.Items52"),
            resources.GetString("cmbLanguage.Items53"),
            resources.GetString("cmbLanguage.Items54"),
            resources.GetString("cmbLanguage.Items55"),
            resources.GetString("cmbLanguage.Items56"),
            resources.GetString("cmbLanguage.Items57"),
            resources.GetString("cmbLanguage.Items58"),
            resources.GetString("cmbLanguage.Items59"),
            resources.GetString("cmbLanguage.Items60"),
            resources.GetString("cmbLanguage.Items61"),
            resources.GetString("cmbLanguage.Items62"),
            resources.GetString("cmbLanguage.Items63"),
            resources.GetString("cmbLanguage.Items64"),
            resources.GetString("cmbLanguage.Items65"),
            resources.GetString("cmbLanguage.Items66"),
            resources.GetString("cmbLanguage.Items67"),
            resources.GetString("cmbLanguage.Items68"),
            resources.GetString("cmbLanguage.Items69"),
            resources.GetString("cmbLanguage.Items70"),
            resources.GetString("cmbLanguage.Items71"),
            resources.GetString("cmbLanguage.Items72"),
            resources.GetString("cmbLanguage.Items73"),
            resources.GetString("cmbLanguage.Items74"),
            resources.GetString("cmbLanguage.Items75"),
            resources.GetString("cmbLanguage.Items76"),
            resources.GetString("cmbLanguage.Items77"),
            resources.GetString("cmbLanguage.Items78"),
            resources.GetString("cmbLanguage.Items79"),
            resources.GetString("cmbLanguage.Items80"),
            resources.GetString("cmbLanguage.Items81"),
            resources.GetString("cmbLanguage.Items82"),
            resources.GetString("cmbLanguage.Items83"),
            resources.GetString("cmbLanguage.Items84"),
            resources.GetString("cmbLanguage.Items85"),
            resources.GetString("cmbLanguage.Items86"),
            resources.GetString("cmbLanguage.Items87"),
            resources.GetString("cmbLanguage.Items88"),
            resources.GetString("cmbLanguage.Items89"),
            resources.GetString("cmbLanguage.Items90"),
            resources.GetString("cmbLanguage.Items91"),
            resources.GetString("cmbLanguage.Items92"),
            resources.GetString("cmbLanguage.Items93"),
            resources.GetString("cmbLanguage.Items94"),
            resources.GetString("cmbLanguage.Items95"),
            resources.GetString("cmbLanguage.Items96"),
            resources.GetString("cmbLanguage.Items97"),
            resources.GetString("cmbLanguage.Items98"),
            resources.GetString("cmbLanguage.Items99"),
            resources.GetString("cmbLanguage.Items100"),
            resources.GetString("cmbLanguage.Items101"),
            resources.GetString("cmbLanguage.Items102"),
            resources.GetString("cmbLanguage.Items103"),
            resources.GetString("cmbLanguage.Items104"),
            resources.GetString("cmbLanguage.Items105"),
            resources.GetString("cmbLanguage.Items106"),
            resources.GetString("cmbLanguage.Items107"),
            resources.GetString("cmbLanguage.Items108"),
            resources.GetString("cmbLanguage.Items109"),
            resources.GetString("cmbLanguage.Items110"),
            resources.GetString("cmbLanguage.Items111"),
            resources.GetString("cmbLanguage.Items112"),
            resources.GetString("cmbLanguage.Items113"),
            resources.GetString("cmbLanguage.Items114"),
            resources.GetString("cmbLanguage.Items115"),
            resources.GetString("cmbLanguage.Items116"),
            resources.GetString("cmbLanguage.Items117"),
            resources.GetString("cmbLanguage.Items118"),
            resources.GetString("cmbLanguage.Items119"),
            resources.GetString("cmbLanguage.Items120"),
            resources.GetString("cmbLanguage.Items121"),
            resources.GetString("cmbLanguage.Items122"),
            resources.GetString("cmbLanguage.Items123"),
            resources.GetString("cmbLanguage.Items124"),
            resources.GetString("cmbLanguage.Items125"),
            resources.GetString("cmbLanguage.Items126"),
            resources.GetString("cmbLanguage.Items127"),
            resources.GetString("cmbLanguage.Items128"),
            resources.GetString("cmbLanguage.Items129"),
            resources.GetString("cmbLanguage.Items130"),
            resources.GetString("cmbLanguage.Items131"),
            resources.GetString("cmbLanguage.Items132"),
            resources.GetString("cmbLanguage.Items133"),
            resources.GetString("cmbLanguage.Items134"),
            resources.GetString("cmbLanguage.Items135"),
            resources.GetString("cmbLanguage.Items136"),
            resources.GetString("cmbLanguage.Items137"),
            resources.GetString("cmbLanguage.Items138"),
            resources.GetString("cmbLanguage.Items139"),
            resources.GetString("cmbLanguage.Items140"),
            resources.GetString("cmbLanguage.Items141"),
            resources.GetString("cmbLanguage.Items142"),
            resources.GetString("cmbLanguage.Items143"),
            resources.GetString("cmbLanguage.Items144"),
            resources.GetString("cmbLanguage.Items145"),
            resources.GetString("cmbLanguage.Items146"),
            resources.GetString("cmbLanguage.Items147"),
            resources.GetString("cmbLanguage.Items148"),
            resources.GetString("cmbLanguage.Items149"),
            resources.GetString("cmbLanguage.Items150"),
            resources.GetString("cmbLanguage.Items151"),
            resources.GetString("cmbLanguage.Items152"),
            resources.GetString("cmbLanguage.Items153"),
            resources.GetString("cmbLanguage.Items154"),
            resources.GetString("cmbLanguage.Items155"),
            resources.GetString("cmbLanguage.Items156"),
            resources.GetString("cmbLanguage.Items157"),
            resources.GetString("cmbLanguage.Items158"),
            resources.GetString("cmbLanguage.Items159"),
            resources.GetString("cmbLanguage.Items160"),
            resources.GetString("cmbLanguage.Items161"),
            resources.GetString("cmbLanguage.Items162"),
            resources.GetString("cmbLanguage.Items163"),
            resources.GetString("cmbLanguage.Items164"),
            resources.GetString("cmbLanguage.Items165"),
            resources.GetString("cmbLanguage.Items166"),
            resources.GetString("cmbLanguage.Items167"),
            resources.GetString("cmbLanguage.Items168"),
            resources.GetString("cmbLanguage.Items169"),
            resources.GetString("cmbLanguage.Items170"),
            resources.GetString("cmbLanguage.Items171"),
            resources.GetString("cmbLanguage.Items172"),
            resources.GetString("cmbLanguage.Items173"),
            resources.GetString("cmbLanguage.Items174"),
            resources.GetString("cmbLanguage.Items175"),
            resources.GetString("cmbLanguage.Items176"),
            resources.GetString("cmbLanguage.Items177"),
            resources.GetString("cmbLanguage.Items178"),
            resources.GetString("cmbLanguage.Items179"),
            resources.GetString("cmbLanguage.Items180"),
            resources.GetString("cmbLanguage.Items181"),
            resources.GetString("cmbLanguage.Items182"),
            resources.GetString("cmbLanguage.Items183"),
            resources.GetString("cmbLanguage.Items184"),
            resources.GetString("cmbLanguage.Items185"),
            resources.GetString("cmbLanguage.Items186"),
            resources.GetString("cmbLanguage.Items187"),
            resources.GetString("cmbLanguage.Items188"),
            resources.GetString("cmbLanguage.Items189"),
            resources.GetString("cmbLanguage.Items190"),
            resources.GetString("cmbLanguage.Items191"),
            resources.GetString("cmbLanguage.Items192"),
            resources.GetString("cmbLanguage.Items193"),
            resources.GetString("cmbLanguage.Items194"),
            resources.GetString("cmbLanguage.Items195"),
            resources.GetString("cmbLanguage.Items196"),
            resources.GetString("cmbLanguage.Items197"),
            resources.GetString("cmbLanguage.Items198"),
            resources.GetString("cmbLanguage.Items199"),
            resources.GetString("cmbLanguage.Items200"),
            resources.GetString("cmbLanguage.Items201")});
            resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.Name = "cmbLanguage";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbDefaultCustomer
            // 
            this.cmbDefaultCustomer.AddList = null;
            this.cmbDefaultCustomer.EnableTextBox = true;
            resources.ApplyResources(this.cmbDefaultCustomer, "cmbDefaultCustomer");
            this.cmbDefaultCustomer.MaxLength = 32767;
            this.cmbDefaultCustomer.Name = "cmbDefaultCustomer";
            this.cmbDefaultCustomer.NoChangeAllowed = false;
            this.cmbDefaultCustomer.RemoveList = null;
            this.cmbDefaultCustomer.RowHeight = ((short)(22));
            this.cmbDefaultCustomer.SecondaryData = null;
            this.cmbDefaultCustomer.SelectedData = null;
            this.cmbDefaultCustomer.SelectionList = null;
            this.cmbDefaultCustomer.ShowDropDownOnTyping = true;
            this.cmbDefaultCustomer.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbDefaultCustomer_DropDown);
            this.cmbDefaultCustomer.SelectedDataChanged += new System.EventHandler(this.cmbDefaultCustomer_SelectedDataChanged);
            this.cmbDefaultCustomer.RequestClear += new System.EventHandler(this.cmbDefaultCustomer_RequestClear);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbPriceTaxSetting
            // 
            this.cmbPriceTaxSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriceTaxSetting.FormattingEnabled = true;
            this.cmbPriceTaxSetting.Items.AddRange(new object[] {
            resources.GetString("cmbPriceTaxSetting.Items"),
            resources.GetString("cmbPriceTaxSetting.Items1")});
            resources.ApplyResources(this.cmbPriceTaxSetting, "cmbPriceTaxSetting");
            this.cmbPriceTaxSetting.Name = "cmbPriceTaxSetting";
            this.cmbPriceTaxSetting.SelectedIndexChanged += new System.EventHandler(this.cmbPriceTaxSetting_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbKeyboard
            // 
            this.cmbKeyboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyboard.FormattingEnabled = true;
            resources.ApplyResources(this.cmbKeyboard, "cmbKeyboard");
            this.cmbKeyboard.Name = "cmbKeyboard";
            // 
            // cmbFunctionalProfile
            // 
            this.cmbFunctionalProfile.AddList = null;
            this.cmbFunctionalProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFunctionalProfile, "cmbFunctionalProfile");
            this.cmbFunctionalProfile.MaxLength = 32767;
            this.cmbFunctionalProfile.Name = "cmbFunctionalProfile";
            this.cmbFunctionalProfile.NoChangeAllowed = false;
            this.cmbFunctionalProfile.OnlyDisplayID = false;
            this.cmbFunctionalProfile.RemoveList = null;
            this.cmbFunctionalProfile.RowHeight = ((short)(22));
            this.cmbFunctionalProfile.SecondaryData = null;
            this.cmbFunctionalProfile.SelectedData = null;
            this.cmbFunctionalProfile.SelectedDataID = null;
            this.cmbFunctionalProfile.SelectionList = null;
            this.cmbFunctionalProfile.SkipIDColumn = true;
            this.cmbFunctionalProfile.RequestData += new System.EventHandler(this.cmbFunctionalProfile_RequestData);
            // 
            // cmbSalesTaxGroup
            // 
            this.cmbSalesTaxGroup.AddList = null;
            this.cmbSalesTaxGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesTaxGroup, "cmbSalesTaxGroup");
            this.cmbSalesTaxGroup.MaxLength = 32767;
            this.cmbSalesTaxGroup.Name = "cmbSalesTaxGroup";
            this.cmbSalesTaxGroup.NoChangeAllowed = false;
            this.cmbSalesTaxGroup.OnlyDisplayID = false;
            this.cmbSalesTaxGroup.RemoveList = null;
            this.cmbSalesTaxGroup.RowHeight = ((short)(22));
            this.cmbSalesTaxGroup.SecondaryData = null;
            this.cmbSalesTaxGroup.SelectedData = null;
            this.cmbSalesTaxGroup.SelectedDataID = null;
            this.cmbSalesTaxGroup.SelectionList = null;
            this.cmbSalesTaxGroup.SkipIDColumn = true;
            this.cmbSalesTaxGroup.RequestData += new System.EventHandler(this.cmbSalesTaxGroup_RequestData);
            this.cmbSalesTaxGroup.SelectedDataChanged += new System.EventHandler(this.cmbSalesTaxGroup_SelectedDataChanged);
            this.cmbSalesTaxGroup.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbCurrencies
            // 
            this.cmbCurrencies.AddList = null;
            this.cmbCurrencies.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCurrencies, "cmbCurrencies");
            this.cmbCurrencies.MaxLength = 32767;
            this.cmbCurrencies.Name = "cmbCurrencies";
            this.cmbCurrencies.NoChangeAllowed = false;
            this.cmbCurrencies.OnlyDisplayID = false;
            this.cmbCurrencies.RemoveList = null;
            this.cmbCurrencies.RowHeight = ((short)(22));
            this.cmbCurrencies.SecondaryData = null;
            this.cmbCurrencies.SelectedData = null;
            this.cmbCurrencies.SelectedDataID = null;
            this.cmbCurrencies.SelectionList = null;
            this.cmbCurrencies.SkipIDColumn = true;
            this.cmbCurrencies.RequestData += new System.EventHandler(this.cmbCurrencies_RequestData);
            this.cmbCurrencies.SelectedDataChanged += new System.EventHandler(this.cmbCurrencies_SelectedDataChanged);
            this.cmbCurrencies.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbTouchLayout
            // 
            this.cmbTouchLayout.AddList = null;
            this.cmbTouchLayout.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTouchLayout, "cmbTouchLayout");
            this.cmbTouchLayout.MaxLength = 32767;
            this.cmbTouchLayout.Name = "cmbTouchLayout";
            this.cmbTouchLayout.NoChangeAllowed = false;
            this.cmbTouchLayout.OnlyDisplayID = false;
            this.cmbTouchLayout.RemoveList = null;
            this.cmbTouchLayout.RowHeight = ((short)(22));
            this.cmbTouchLayout.SecondaryData = null;
            this.cmbTouchLayout.SelectedData = null;
            this.cmbTouchLayout.SelectedDataID = null;
            this.cmbTouchLayout.SelectionList = null;
            this.cmbTouchLayout.SkipIDColumn = true;
            this.cmbTouchLayout.RequestData += new System.EventHandler(this.cmbTouchLayout_RequestData);
            // 
            // cmbOperationAuditing
            // 
            this.cmbOperationAuditing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperationAuditing.FormattingEnabled = true;
            this.cmbOperationAuditing.Items.AddRange(new object[] {
            resources.GetString("cmbOperationAuditing.Items"),
            resources.GetString("cmbOperationAuditing.Items1"),
            resources.GetString("cmbOperationAuditing.Items2")});
            resources.ApplyResources(this.cmbOperationAuditing, "cmbOperationAuditing");
            this.cmbOperationAuditing.Name = "cmbOperationAuditing";
            // 
            // lblAuditOperations
            // 
            this.lblAuditOperations.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAuditOperations, "lblAuditOperations");
            this.lblAuditOperations.Name = "lblAuditOperations";
            // 
            // lblStartAmount
            // 
            this.lblStartAmount.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStartAmount, "lblStartAmount");
            this.lblStartAmount.Name = "lblStartAmount";
            // 
            // addressField
            // 
            this.addressField.BackColor = System.Drawing.Color.Transparent;
            this.addressField.DataModel = null;
            resources.ApplyResources(this.addressField, "addressField");
            this.addressField.Name = "addressField";
            // 
            // cmbRegion
            // 
            this.cmbRegion.AddList = null;
            this.cmbRegion.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRegion, "cmbRegion");
            this.cmbRegion.MaxLength = 32767;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.NoChangeAllowed = false;
            this.cmbRegion.OnlyDisplayID = false;
            this.cmbRegion.RemoveList = null;
            this.cmbRegion.RowHeight = ((short)(22));
            this.cmbRegion.SecondaryData = null;
            this.cmbRegion.SelectedData = null;
            this.cmbRegion.SelectedDataID = null;
            this.cmbRegion.SelectionList = null;
            this.cmbRegion.SkipIDColumn = true;
            this.cmbRegion.RequestData += new System.EventHandler(this.cmbRegion_RequestData);
            this.cmbRegion.SelectedDataChanged += new System.EventHandler(this.cmbRegion_SelectedDataChanged);
            this.cmbRegion.RequestClear += new System.EventHandler(this.cmbRegion_RequestClear);
            // 
            // lblRegion
            // 
            this.lblRegion.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRegion, "lblRegion");
            this.lblRegion.Name = "lblRegion";
            // 
            // btnAddRegion
            // 
            this.btnAddRegion.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRegion.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddRegion, "btnAddRegion");
            this.btnAddRegion.Name = "btnAddRegion";
            this.btnAddRegion.Click += new System.EventHandler(this.btnAddRegion_Click);
            // 
            // btnEditRegion
            // 
            this.btnEditRegion.BackColor = System.Drawing.Color.Transparent;
            this.btnEditRegion.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditRegion, "btnEditRegion");
            this.btnEditRegion.Name = "btnEditRegion";
            this.btnEditRegion.Click += new System.EventHandler(this.btnEditRegion_Click);
            // 
            // ntbStartAmount
            // 
            this.ntbStartAmount.AllowDecimal = false;
            this.ntbStartAmount.AllowNegative = false;
            this.ntbStartAmount.CultureInfo = null;
            this.ntbStartAmount.DecimalLetters = 2;
            this.ntbStartAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbStartAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbStartAmount, "ntbStartAmount");
            this.ntbStartAmount.MaxValue = 0D;
            this.ntbStartAmount.MinValue = 0D;
            this.ntbStartAmount.Name = "ntbStartAmount";
            this.ntbStartAmount.Value = 0D;
            // 
            // btnAddCustomer
            // 
            this.btnAddCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCustomer.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCustomer, "btnAddCustomer");
            this.btnAddCustomer.Name = "btnAddCustomer";
            this.btnAddCustomer.Click += new System.EventHandler(this.btnAddCustomer_Click);
            // 
            // btnEditFunctionalProfile
            // 
            this.btnEditFunctionalProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFunctionalProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditFunctionalProfile, "btnEditFunctionalProfile");
            this.btnEditFunctionalProfile.Name = "btnEditFunctionalProfile";
            this.btnEditFunctionalProfile.Click += new System.EventHandler(this.btnEditFunctionalProfile_Click);
            // 
            // btnEditSalesTaxGroup
            // 
            this.btnEditSalesTaxGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnEditSalesTaxGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditSalesTaxGroup, "btnEditSalesTaxGroup");
            this.btnEditSalesTaxGroup.Name = "btnEditSalesTaxGroup";
            this.btnEditSalesTaxGroup.Click += new System.EventHandler(this.btnEditItemSalesTaxGroup_Click);
            // 
            // btnCurrenciesEdit
            // 
            this.btnCurrenciesEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnCurrenciesEdit.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnCurrenciesEdit, "btnCurrenciesEdit");
            this.btnCurrenciesEdit.Name = "btnCurrenciesEdit";
            this.btnCurrenciesEdit.Click += new System.EventHandler(this.btnCurrenciesEdit_Click);
            // 
            // btnEditTouchButtons
            // 
            this.btnEditTouchButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnEditTouchButtons.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditTouchButtons, "btnEditTouchButtons");
            this.btnEditTouchButtons.Name = "btnEditTouchButtons";
            this.btnEditTouchButtons.Click += new System.EventHandler(this.btnEditTouchButtons_Click);
            // 
            // btnEditCustomer
            // 
            this.btnEditCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnEditCustomer.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditCustomer, "btnEditCustomer");
            this.btnEditCustomer.Name = "btnEditCustomer";
            this.btnEditCustomer.Click += new System.EventHandler(this.btnEditCustomer_Click);
            // 
            // StoreSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnAddRegion);
            this.Controls.Add(this.btnEditRegion);
            this.Controls.Add(this.cmbRegion);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.ntbStartAmount);
            this.Controls.Add(this.lblStartAmount);
            this.Controls.Add(this.cmbOperationAuditing);
            this.Controls.Add(this.lblAuditOperations);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbKeyboard);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbPriceTaxSetting);
            this.Controls.Add(this.btnEditCustomer);
            this.Controls.Add(this.btnEditTouchButtons);
            this.Controls.Add(this.btnCurrenciesEdit);
            this.Controls.Add(this.btnEditSalesTaxGroup);
            this.Controls.Add(this.btnEditFunctionalProfile);
            this.Controls.Add(this.btnAddCustomer);
            this.Controls.Add(this.cmbTouchLayout);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCurrencies);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.cmbFunctionalProfile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbDefaultCustomer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.addressField);
            this.DoubleBuffered = true;
            this.Name = "StoreSettingsPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private DropDownFormComboBox cmbDefaultCustomer;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbFunctionalProfile;
        private DualDataComboBox cmbSalesTaxGroup;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbCurrencies;
        private DualDataComboBox cmbTouchLayout;
        private System.Windows.Forms.Label label1;
        private ContextButton btnAddCustomer;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private ContextButton btnEditCustomer;
        private ContextButton btnEditTouchButtons;
        private ContextButton btnCurrenciesEdit;
        private ContextButton btnEditSalesTaxGroup;
        private ContextButton btnEditFunctionalProfile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbPriceTaxSetting;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbKeyboard;
        private System.Windows.Forms.ComboBox cmbOperationAuditing;
        private System.Windows.Forms.Label lblAuditOperations;
        private NumericTextBox ntbStartAmount;
        private System.Windows.Forms.Label lblStartAmount;
        private AddressControl addressField;
        private ContextButton btnAddRegion;
        private ContextButton btnEditRegion;
        private DualDataComboBox cmbRegion;
        private System.Windows.Forms.Label lblRegion;
    }
}
