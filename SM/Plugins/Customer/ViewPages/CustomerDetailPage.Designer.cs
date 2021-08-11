using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    partial class CustomerDetailPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerDetailPage));
            this.label4 = new System.Windows.Forms.Label();
            this.cmbReceiptOption = new System.Windows.Forms.ComboBox();
            this.tbReceiptEmail = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.tbIdentificationNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSearchAlias = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditSalesTaxGroup = new LSOne.Controls.ContextButton();
            this.btnCurrenciesEdit = new LSOne.Controls.ContextButton();
            this.chkPricesIncludeSalesTax = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVATnumber = new System.Windows.Forms.TextBox();
            this.chkIsCashCustomer = new System.Windows.Forms.CheckBox();
            this.lblUserIsDisabled = new System.Windows.Forms.Label();
            this.lblDomainUser = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbCust = new LSOne.Controls.DualDataComboBox();
            this.cmbCurrencies = new LSOne.Controls.DualDataComboBox();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.cmbBlocking = new LSOne.Controls.DataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dtDateOfBirth = new System.Windows.Forms.DateTimePicker();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbTaxExempt = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbReceiptOption
            // 
            this.cmbReceiptOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReceiptOption.FormattingEnabled = true;
            this.cmbReceiptOption.Items.AddRange(new object[] {
            resources.GetString("cmbReceiptOption.Items"),
            resources.GetString("cmbReceiptOption.Items1"),
            resources.GetString("cmbReceiptOption.Items2")});
            resources.ApplyResources(this.cmbReceiptOption, "cmbReceiptOption");
            this.cmbReceiptOption.Name = "cmbReceiptOption";
            // 
            // tbReceiptEmail
            // 
            resources.ApplyResources(this.tbReceiptEmail, "tbReceiptEmail");
            this.tbReceiptEmail.Name = "tbReceiptEmail";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
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
            // tbIdentificationNumber
            // 
            resources.ApplyResources(this.tbIdentificationNumber, "tbIdentificationNumber");
            this.tbIdentificationNumber.Name = "tbIdentificationNumber";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // tbSearchAlias
            // 
            resources.ApplyResources(this.tbSearchAlias, "tbSearchAlias");
            this.tbSearchAlias.Name = "tbSearchAlias";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // chkPricesIncludeSalesTax
            // 
            this.chkPricesIncludeSalesTax.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.chkPricesIncludeSalesTax, "chkPricesIncludeSalesTax");
            this.chkPricesIncludeSalesTax.Name = "chkPricesIncludeSalesTax";
            this.chkPricesIncludeSalesTax.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbVATnumber
            // 
            resources.ApplyResources(this.tbVATnumber, "tbVATnumber");
            this.tbVATnumber.Name = "tbVATnumber";
            // 
            // chkIsCashCustomer
            // 
            this.chkIsCashCustomer.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.chkIsCashCustomer, "chkIsCashCustomer");
            this.chkIsCashCustomer.Name = "chkIsCashCustomer";
            this.chkIsCashCustomer.UseVisualStyleBackColor = false;
            // 
            // lblUserIsDisabled
            // 
            this.lblUserIsDisabled.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUserIsDisabled, "lblUserIsDisabled");
            this.lblUserIsDisabled.Name = "lblUserIsDisabled";
            // 
            // lblDomainUser
            // 
            this.lblDomainUser.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDomainUser, "lblDomainUser");
            this.lblDomainUser.Name = "lblDomainUser";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmbCust
            // 
            this.cmbCust.AddList = null;
            this.cmbCust.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCust, "cmbCust");
            this.cmbCust.MaxLength = 32767;
            this.cmbCust.Name = "cmbCust";
            this.cmbCust.NoChangeAllowed = false;
            this.cmbCust.OnlyDisplayID = false;
            this.cmbCust.RemoveList = null;
            this.cmbCust.RowHeight = ((short)(22));
            this.cmbCust.SecondaryData = null;
            this.cmbCust.SelectedData = null;
            this.cmbCust.SelectedDataID = null;
            this.cmbCust.SelectionList = null;
            this.cmbCust.SkipIDColumn = true;
            this.cmbCust.RequestData += new System.EventHandler(this.cmbCust_RequestData);
            this.cmbCust.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCust_DropDown);
            this.cmbCust.RequestClear += new System.EventHandler(this.cmbCust_RequestClear);
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
            this.cmbCurrencies.RequestClear += new System.EventHandler(this.cmbCurrencies_RequestClear);
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
            this.cmbSalesTaxGroup.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbBlocking
            // 
            this.cmbBlocking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlocking.FormattingEnabled = true;
            this.cmbBlocking.Items.AddRange(new object[] {
            resources.GetString("cmbBlocking.Items"),
            resources.GetString("cmbBlocking.Items1"),
            resources.GetString("cmbBlocking.Items2")});
            resources.ApplyResources(this.cmbBlocking, "cmbBlocking");
            this.cmbBlocking.Name = "cmbBlocking";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // dtDateOfBirth
            // 
            this.dtDateOfBirth.Checked = false;
            resources.ApplyResources(this.dtDateOfBirth, "dtDateOfBirth");
            this.dtDateOfBirth.Name = "dtDateOfBirth";
            this.dtDateOfBirth.ShowCheckBox = true;
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            resources.GetString("cmbGender.Items"),
            resources.GetString("cmbGender.Items1"),
            resources.GetString("cmbGender.Items2")});
            resources.ApplyResources(this.cmbGender, "cmbGender");
            this.cmbGender.Name = "cmbGender";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbTaxExempt
            // 
            this.cmbTaxExempt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaxExempt.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTaxExempt, "cmbTaxExempt");
            this.cmbTaxExempt.Name = "cmbTaxExempt";
            this.cmbTaxExempt.SelectedIndexChanged += new System.EventHandler(this.cmbTaxExempt_SelectedIndexChanged);
            // 
            // CustomerDetailPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbTaxExempt);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.dtDateOfBirth);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbReceiptOption);
            this.Controls.Add(this.tbReceiptEmail);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.cmbCust);
            this.Controls.Add(this.tbIdentificationNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbSearchAlias);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEditSalesTaxGroup);
            this.Controls.Add(this.btnCurrenciesEdit);
            this.Controls.Add(this.chkPricesIncludeSalesTax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbVATnumber);
            this.Controls.Add(this.cmbCurrencies);
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.cmbBlocking);
            this.Controls.Add(this.chkIsCashCustomer);
            this.Controls.Add(this.lblUserIsDisabled);
            this.Controls.Add(this.lblDomainUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Name = "CustomerDetailPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbReceiptOption;
        private System.Windows.Forms.TextBox tbReceiptEmail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private DualDataComboBox cmbCust;
        private System.Windows.Forms.TextBox tbIdentificationNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbSearchAlias;
        private System.Windows.Forms.Label label1;
        private ContextButton btnEditSalesTaxGroup;
        private ContextButton btnCurrenciesEdit;
        private System.Windows.Forms.CheckBox chkPricesIncludeSalesTax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbVATnumber;
        private DualDataComboBox cmbCurrencies;
        private DualDataComboBox cmbSalesTaxGroup;
        private DataComboBox cmbBlocking;
        private System.Windows.Forms.CheckBox chkIsCashCustomer;
        private System.Windows.Forms.Label lblUserIsDisabled;
        private System.Windows.Forms.Label lblDomainUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtDateOfBirth;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label label13;
        private LinkFields linkFields1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cmbTaxExempt;
    }
}
