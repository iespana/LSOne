namespace LSOne.DataLayer.BusinessObjects.Infocodes
{
    public enum UsageCategoriesEnum
    {
        None = 0,
        CrossSelling = 1,
        ItemModifier = 2
    }

    public enum OKPressedActions 
    { 
        JumpToNextUnDisplayed = 0, 
        JumpToNextRequierd = 1
    }

    public enum PriceHandlings
    {
        None = 0,
        AlwaysCharge = 1,
        NoCharge = 2
    }

    public enum TriggerFunctions
    {
        None = 0,
        Item = 1,
        DiscountGroup = 2,
        Customer = 3,
        VAT = 4,
        Infocode = 5,
        TaxGroup = 6
    }

    public enum PriceTypes
    {
        None = 0,
        FromItem = 1,
        Price = 2,
        Percent = 3
    }

    public enum TriggeringEnum
    {
        Automatic = 0,
        Manual = 1
    }

    public enum RefTableEnum
    {
        All = 0,
        /// <summary>
        /// InventTable = 1
        /// </summary>
        Item = 1,
        /// <summary>
        /// CustomerTable = 2
        /// </summary>
        Customer = 2,
        /// <summary>
        /// RboStoreTenderTypeTable = 3
        /// </summary>
        Tender = 3,
        /// <summary>
        /// RboStoreTenderTypeCardTable = 4
        /// </summary>
        CreditCard = 4,
        /// <summary>
        /// RboIncomeExpenseAccountTable = 5
        /// </summary>
        IncomeExpense = 5,
        /// <summary>
        /// RboInventItemDepartment = 6
        /// </summary>
        RetailDepartment = 6,
        /// <summary>
        /// RboInventItemRetailGroup = 7
        /// </summary>
        RetailGroup = 7,
        /// <summary>
        /// PosFunctionalityProfile = 8
        /// </summary>
        FunctionalityProfile = 8,
        /// <summary>
        /// PosFunctionalityProfile = 9
        /// </summary>
        PreItem = 9

    }

    public enum InputTypesEnum
    {
        General = 0,
        /// <summary>
        /// If a subcode is connected to the infocode that will be presented as a list
        /// </summary>
        SubCodeList = 1,
        /// <summary>
        /// If the input is date.
        /// </summary>
        Date = 2,
        /// <summary>
        /// If the input is numeric.
        /// </summary>
        Numeric = 3,
        /// <summary>
        /// If the input is an item.
        /// </summary>
        Item = 4,
        /// <summary>
        /// If the input is a customer.
        /// </summary>
        Customer = 5,
        /// <summary>
        /// If the input is employee.
        /// </summary>
        Operator = 6,
        /// <summary>
        /// Will not be implemented in LS Pos
        /// </summary>
        CreateDataEntry = 7,    //??
        /// <summary>
        /// Will not be implemented in LS Pos
        /// </summary>
        ApplyToEntry = 8,       //??
        /// <summary>
        /// If the input is a text.
        /// </summary>
        Text = 9,
        /// <summary>
        /// If a subcode is connected to the infocode that will be presented as buttons
        /// </summary>
        SubCodeButtons = 10,
        /// <summary>
        /// Determines what is the minimum age that the customer needs to have.
        /// </summary>
        AgeLimit = 11,
        /// <summary>
        /// Infocode that groups other infocodes together. The selection codes of the infocode are the infocodes themselves.
        /// </summary>
        Group = 12
    }

    public enum DisplayOptions
    {
        None = 0,
        Lookup = 1,
        PopUp = 2
    }
}
