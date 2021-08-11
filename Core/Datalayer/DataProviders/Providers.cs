using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.DataLayer.DataProviders.CustomerOrders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.DataLayer.DataProviders.TimeKeeper;
using LSOne.DataLayer.KDSBusinessObjects;

namespace LSOne.DataLayer.DataProviders
{
    public static class Providers
    {
        static public Profiles.IFunctionalityProfileData FunctionalityProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IFunctionalityProfileData, BusinessObjects.Profiles.FunctionalityProfile>();} }
        static public Profiles.IVisualProfileData VisualProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IVisualProfileData, BusinessObjects.Profiles.VisualProfile>();} }
        static public Profiles.IUserProfileData UserProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IUserProfileData, BusinessObjects.Profiles.UserProfile>(); } }
        static public Profiles.IHardwareProfileData HardwareProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IHardwareProfileData, BusinessObjects.Profiles.HardwareProfile>();} }
        static public Profiles.IWindowsPrinterConfigurationData WindowsPrinterConfigurationData { get { return DataProviderFactory.Instance.Get<Profiles.IWindowsPrinterConfigurationData, BusinessObjects.Profiles.WindowsPrinterConfiguration>();} }
        static public Companies.IParameterData ParameterData { get { return DataProviderFactory.Instance.Get<Companies.IParameterData, BusinessObjects.Companies.Parameters>();} }
        static public Companies.ICompanyInfoData CompanyInfoData { get { return DataProviderFactory.Instance.Get<Companies.ICompanyInfoData, BusinessObjects.Companies.CompanyInfo>();} }
        static public StoreManagement.IStoreData StoreData { get { return DataProviderFactory.Instance.Get<StoreManagement.IStoreData, BusinessObjects.StoreManagement.Store>();} }
        static public StoreManagement.IRegionData RegionData { get { return DataProviderFactory.Instance.Get<StoreManagement.IRegionData, BusinessObjects.StoreManagement.Region>();} }
        static public PricesAndDiscounts.IPriceDiscountGroupData PriceDiscountGroupData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IPriceDiscountGroupData, BusinessObjects.PricesAndDiscounts.PriceDiscountGroup>();} }
        static public StoreManagement.ITerminalData TerminalData { get { return DataProviderFactory.Instance.Get<StoreManagement.ITerminalData, BusinessObjects.StoreManagement.Terminal>();} }
        static public UserManagement.IPOSUserData POSUserData { get { return DataProviderFactory.Instance.Get<UserManagement.IPOSUserData, BusinessObjects.UserManagement.POSUser>();} }
        static public PricesAndDiscounts.IDiscountCalculationData DiscountCalculationData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IDiscountCalculationData, BusinessObjects.PricesAndDiscounts.DiscountCalculation>(); } }
        static public PricesAndDiscounts.IDiscountParametersData DiscountParametersData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IDiscountParametersData, BusinessObjects.PricesAndDiscounts.DiscountParameters>(); } }
        static public Profiles.ISiteServiceProfileData SiteServiceProfileData { get { return DataProviderFactory.Instance.Get<Profiles.ISiteServiceProfileData, BusinessObjects.Profiles.SiteServiceProfile>();} }
        static public IntegrationFramework.IAccessTokenData AccessTokenData { get { return DataProviderFactory.Instance.Get<IntegrationFramework.IAccessTokenData, BusinessObjects.IntegrationFramework.AccessToken>();} }
        static public LookupValues.IKeyboardMappingData KeyboardMappingData { get { return DataProviderFactory.Instance.Get<LookupValues.IKeyboardMappingData, BusinessObjects.LookupValues.KeyboardMapping>();} }
        static public Currencies.IExchangeRatesData ExchangeRatesData { get { return DataProviderFactory.Instance.Get<Currencies.IExchangeRatesData, BusinessObjects.Currencies.ExchangeRate>();} }
        static public TouchButtons.IPosOperationData PosOperationData { get { return DataProviderFactory.Instance.Get<TouchButtons.IPosOperationData, BusinessObjects.TouchButtons.PosOperation>();} }
        static public UserManagement.IUserData UserData { get { return DataProviderFactory.Instance.Get<UserManagement.IUserData, BusinessObjects.UserManagement.User>();} }
        static public Contacts.IContactData ContactData { get { return DataProviderFactory.Instance.Get<Contacts.IContactData, BusinessObjects.Contact>();} }
        static public LookupValues.IPaymentMethodData PaymentMethodData { get { return DataProviderFactory.Instance.Get<LookupValues.IPaymentMethodData, BusinessObjects.LookupValues.PaymentMethod>();} }
        static public StoreManagement.IStorePaymentMethodData StorePaymentMethodData { get { return DataProviderFactory.Instance.Get<StoreManagement.IStorePaymentMethodData, BusinessObjects.StoreManagement.StorePaymentMethod>();} }
        static public StoreManagement.IStorePaymentLimitationData StorePaymentLimitationData { get { return DataProviderFactory.Instance.Get<StoreManagement.IStorePaymentLimitationData, BusinessObjects.StoreManagement.StorePaymentLimitation>();} }
        static public Currencies.ICurrencyData CurrencyData { get { return DataProviderFactory.Instance.Get<Currencies.ICurrencyData, BusinessObjects.Currencies.Currency>();} }
        static public Forms.IFormData FormData { get { return DataProviderFactory.Instance.Get<Forms.IFormData, BusinessObjects.Forms.Form>();} }
        static public Profiles.IFormProfileLineData FormProfileLineData { get { return DataProviderFactory.Instance.Get<Profiles.IFormProfileLineData, BusinessObjects.Profiles.FormProfileLine>();} }
        static public ItemMaster.IRetailGroupData RetailGroupData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailGroupData, BusinessObjects.ItemMaster.RetailGroup>();} }
        static public ItemMaster.IRetailItemData RetailItemData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailItemData, BusinessObjects.ItemMaster.RetailItem>(); } }
        static public ItemMaster.IRetailItemAssemblyData RetailItemAssemblyData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailItemAssemblyData, BusinessObjects.ItemMaster.RetailItemAssembly>(); } }
        static public ItemMaster.IRetailItemAssemblyComponentData RetailItemAssemblyComponentData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailItemAssemblyComponentData, BusinessObjects.ItemMaster.RetailItemAssemblyComponent>(); } }
        static public ItemMaster.IRetailItemCostData RetailItemCostData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailItemCostData, BusinessObjects.ItemMaster.RetailItemCost>(); } }
        static public ItemMaster.IRetailItemDataOld RetailItemDataOld { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailItemDataOld, BusinessObjects.ItemMaster.RetailItemOld>(); } }

        static public ItemMaster.MultiEditing.IRetailItemMultiEditData RetailItemMultiEditData { get { return DataProviderFactory.Instance.Get<ItemMaster.MultiEditing.IRetailItemMultiEditData, BusinessObjects.ItemMaster.MultiEditing.RetailItemMultiEdit>(); } }

        static public Tax.ISalesTaxGroupData SalesTaxGroupData { get { return DataProviderFactory.Instance.Get<Tax.ISalesTaxGroupData, BusinessObjects.Tax.SalesTaxGroup>(); } }
        static public Tax.IItemSalesTaxGroupData ItemSalesTaxGroupData { get { return DataProviderFactory.Instance.Get<Tax.IItemSalesTaxGroupData, BusinessObjects.Tax.ItemSalesTaxGroup>();} }
        static public Units.IUnitData UnitData { get { return DataProviderFactory.Instance.Get<Units.IUnitData, BusinessObjects.Units.Unit>();} }
        static public Tax.ITaxCodeData TaxCodeData { get { return DataProviderFactory.Instance.Get<Tax.ITaxCodeData, BusinessObjects.Tax.TaxCode>();} }
        static public Tax.ITaxCodeValueData TaxCodeValueData { get { return DataProviderFactory.Instance.Get<Tax.ITaxCodeValueData, BusinessObjects.Tax.TaxCodeValue>();} }
        static public TaxFree.ITaxFreeConfigData TaxFreeConfigData { get { return DataProviderFactory.Instance.Get<TaxFree.ITaxFreeConfigData, BusinessObjects.TaxFree.TaxFreeConfig>(); } }
        static public TaxFree.ITaxRefundRangeData TaxRefundRangeData { get { return DataProviderFactory.Instance.Get<TaxFree.ITaxRefundRangeData, BusinessObjects.TaxFree.TaxRefundRange>(); } }
        static public TaxFree.ITaxRefundData TaxRefundData { get { return DataProviderFactory.Instance.Get<TaxFree.ITaxRefundData, BusinessObjects.TaxFree.TaxRefund>(); } }
        static public TaxFree.ITaxRefundTransactionData TaxRefundTransactionData { get { return DataProviderFactory.Instance.Get<TaxFree.ITaxRefundTransactionData, BusinessObjects.TaxFree.TaxRefundTransaction>(); } }
        static public TaxFree.ITouristData TouristData { get { return DataProviderFactory.Instance.Get<TaxFree.ITouristData, BusinessObjects.TaxFree.Tourist>(); } }
        static public TouchButtons.IPosStyleData PosStyleData { get { return DataProviderFactory.Instance.Get<TouchButtons.IPosStyleData, BusinessObjects.TouchButtons.PosStyle>();} }
        static public TouchButtons.IPosMenuHeaderData PosMenuHeaderData { get { return DataProviderFactory.Instance.Get<TouchButtons.IPosMenuHeaderData, BusinessObjects.TouchButtons.PosMenuHeader>();} }
        static public TouchButtons.IPosMenuLineData PosMenuLineData { get { return DataProviderFactory.Instance.Get<TouchButtons.IPosMenuLineData, BusinessObjects.TouchButtons.PosMenuLine>();} }
        static public TouchButtons.ITouchLayoutData TouchLayoutData { get { return DataProviderFactory.Instance.Get<TouchButtons.ITouchLayoutData, BusinessObjects.TouchButtons.TouchLayout>();} }
        static public UserManagement.IUserGroupData UserGroupData { get { return DataProviderFactory.Instance.Get<UserManagement.IUserGroupData, BusinessObjects.UserManagement.UserGroup>();} }
        static public Hospitality.IHospitalityTypeData HospitalityTypeData { get { return DataProviderFactory.Instance.Get<Hospitality.IHospitalityTypeData, BusinessObjects.Hospitality.HospitalityType>();} }
        static public Images.IImageData ImageData { get { return DataProviderFactory.Instance.Get<Images.IImageData, BusinessObjects.Images.Image>(); } }
        static public Info.IPosisInfoData PosisInfoData { get { return DataProviderFactory.Instance.Get<Info.IPosisInfoData, BusinessObjects.DataEntity>();} }
        static public TouchButtons.IButtonGridData ButtonGridData { get { return DataProviderFactory.Instance.Get<TouchButtons.IButtonGridData, BusinessObjects.TouchButtons.ButtonGrid>();} }
        static public TouchButtons.IButtonGridButtonsData ButtonGridButtonsData { get { return DataProviderFactory.Instance.Get<TouchButtons.IButtonGridButtonsData, BusinessObjects.TouchButtons.ButtonGridButton>();} }
        static public Operations.IBlankOperationData BlankOperationData { get { return DataProviderFactory.Instance.Get<Operations.IBlankOperationData, BusinessObjects.StoreManagement.BlankOperation>();} }
        static public Infocodes.IInfocodeData InfocodeData { get { return DataProviderFactory.Instance.Get<Infocodes.IInfocodeData, BusinessObjects.Infocodes.Infocode>();} }
        static public Transactions.ISuspendedTransactionTypeData SuspendedTransactionTypeData { get { return DataProviderFactory.Instance.Get<Transactions.ISuspendedTransactionTypeData, BusinessObjects.Transactions.SuspendedTransactionType>();} }
        static public Financials.IIncomeExpenseAccountData IncomeExpenseAccountData { get { return DataProviderFactory.Instance.Get<Financials.IIncomeExpenseAccountData, BusinessObjects.IncomeExpenseAccount>();} }
        static public Currencies.ICashDenominatorData CashDenominatorData { get { return DataProviderFactory.Instance.Get<Currencies.ICashDenominatorData, BusinessObjects.Currencies.CashDenominator>();} }
        static public PricesAndDiscounts.IDiscountOfferData DiscountOfferData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IDiscountOfferData, BusinessObjects.PricesAndDiscounts.DiscountOffer>(); } }
        static public PricesAndDiscounts.ICouponData CouponData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.ICouponData, BusinessObjects.PricesAndDiscounts.Coupon>(); } }
        static public BarCodes.IBarCodeData BarCodeData { get { return DataProviderFactory.Instance.Get<BarCodes.IBarCodeData, BusinessObjects.BarCodes.BarCode>();} }
        static public Contacts.IStateData StateData { get { return DataProviderFactory.Instance.Get<Contacts.IStateData, BusinessObjects.DataEntity>();} }
        static public ItemMaster.ISpecialGroupData SpecialGroupData { get { return DataProviderFactory.Instance.Get<ItemMaster.ISpecialGroupData, BusinessObjects.DataEntity>();} }
        static public ItemMaster.IRetailDepartmentData RetailDepartmentData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailDepartmentData, BusinessObjects.ItemMaster.RetailDepartment>(); } }
        static public ItemMaster.Dimensions.IRetailItemDimensionData RetailItemDimensionData { get { return DataProviderFactory.Instance.Get<ItemMaster.Dimensions.IRetailItemDimensionData, BusinessObjects.ItemMaster.Dimensions.RetailItemDimension>(); } }
        static public ItemMaster.Dimensions.IDimensionTemplateData DimensionTemplateData { get { return DataProviderFactory.Instance.Get<ItemMaster.Dimensions.IDimensionTemplateData, BusinessObjects.ItemMaster.Dimensions.DimensionTemplate>(); } }
        static public ItemMaster.Dimensions.IDimensionAttributeData DimensionAttributeData { get { return DataProviderFactory.Instance.Get<ItemMaster.Dimensions.IDimensionAttributeData, BusinessObjects.ItemMaster.Dimensions.DimensionAttribute>(); } }
        static public Customers.ICustomerData CustomerData { get { return DataProviderFactory.Instance.Get<Customers.ICustomerData, BusinessObjects.Customers.Customer>();} }
        static public Inventory.IVendorItemData VendorItemData { get { return DataProviderFactory.Instance.Get<Inventory.IVendorItemData, BusinessObjects.Inventory.VendorItem>();} }
        static public Inventory.IPurchaseOrderData PurchaseOrderData { get { return DataProviderFactory.Instance.Get<Inventory.IPurchaseOrderData, BusinessObjects.Inventory.PurchaseOrder>();} }
        static public ItemMaster.IRetailDivisionData RetailDivisionData { get { return DataProviderFactory.Instance.Get<ItemMaster.IRetailDivisionData, BusinessObjects.ItemMaster.RetailDivision>();} }
        static public Loyalty.ILoyaltyMSRCardData LoyaltyMSRCardData { get { return DataProviderFactory.Instance.Get<Loyalty.ILoyaltyMSRCardData, BusinessObjects.Loyalty.LoyaltyMSRCard>();} }
        static public Customers.ICustomerAddressData CustomerAddressData { get { return DataProviderFactory.Instance.Get<Customers.ICustomerAddressData, BusinessObjects.Customers.CustomerAddress>();} }
        static public Hospitality.IHospitalitySetupData HospitalitySetupData { get { return DataProviderFactory.Instance.Get<Hospitality.IHospitalitySetupData, BusinessObjects.Hospitality.HospitalitySetup>();} }
        static public Inventory.IGoodsReceivingDocumentLineData GoodsReceivingDocumentLineData { get { return DataProviderFactory.Instance.Get<Inventory.IGoodsReceivingDocumentLineData, BusinessObjects.Inventory.GoodsReceivingDocumentLine>();} }
        static public Inventory.IPurchaseOrderLineData PurchaseOrderLineData { get { return DataProviderFactory.Instance.Get<Inventory.IPurchaseOrderLineData, BusinessObjects.Inventory.PurchaseOrderLine>();} }
        static public Inventory.IInventoryTransactionData InventoryTransactionData { get { return DataProviderFactory.Instance.Get<Inventory.IInventoryTransactionData, BusinessObjects.Inventory.InventoryTransaction>();} }
        static public Inventory.IInventoryAdjustmentData InventoryAdjustmentData { get { return DataProviderFactory.Instance.Get<Inventory.IInventoryAdjustmentData, BusinessObjects.Inventory.InventoryAdjustment>();} }
        static public Units.IUnitConversionData UnitConversionData { get { return DataProviderFactory.Instance.Get<Units.IUnitConversionData, BusinessObjects.Units.UnitConversion>();} }
        static public Transactions.ISalesTransactionData SalesTransactionData { get { return DataProviderFactory.Instance.Get<Transactions.ISalesTransactionData, BusinessObjects.Transactions.SalesTransaction>();} }
        static public Inventory.IGoodsReceivingDocumentData GoodsReceivingDocumentData { get { return DataProviderFactory.Instance.Get<Inventory.IGoodsReceivingDocumentData, BusinessObjects.Inventory.GoodsReceivingDocument>();} }
        static public Loyalty.ILoyaltyMSRCardTransData LoyaltyMSRCardTransData { get { return DataProviderFactory.Instance.Get<Loyalty.ILoyaltyMSRCardTransData, BusinessObjects.Loyalty.LoyaltyMSRCardTrans>();} }
        static public Loyalty.ILoyaltySchemesData LoyaltySchemesData { get { return DataProviderFactory.Instance.Get<Loyalty.ILoyaltySchemesData, BusinessObjects.Loyalty.LoyaltySchemes>();} }
        static public Omni.IOmniAppLicenseData OmniAppLicenseData { get { return DataProviderFactory.Instance.Get<Omni.IOmniAppLicenseData, BusinessObjects.Omni.OmniLicense>();} }
        static public Omni.IOmniJournalData OmniJournalData { get { return DataProviderFactory.Instance.Get<Omni.IOmniJournalData, BusinessObjects.Omni.OmniJournal>(); } }
        static public PricesAndDiscounts.IDiscountPeriodData DiscountPeriodData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IDiscountPeriodData, BusinessObjects.PricesAndDiscounts.DiscountPeriod>();} }
        static public Profiles.IPosContextData PosContextData { get { return DataProviderFactory.Instance.Get<Profiles.IPosContextData, BusinessObjects.Profiles.PosContext>();} }
        static public Profiles.IPosStyleProfileLineData PosStyleProfileLineData { get { return DataProviderFactory.Instance.Get<Profiles.IPosStyleProfileLineData, BusinessObjects.Profiles.PosStyleProfileLine>();} }
        static public Reports.IReportData ReportData { get { return DataProviderFactory.Instance.Get<Reports.IReportData, BusinessObjects.Reports.ReportListItem>();} }
        static public Statements.IStatementLineData StatementLineData { get { return DataProviderFactory.Instance.Get<Statements.IStatementLineData, BusinessObjects.StatementLine>();} }
        static public StoreManagement.IPaymentTypeCardTypesData PaymentTypeCardTypesData { get { return DataProviderFactory.Instance.Get<StoreManagement.IPaymentTypeCardTypesData, BusinessObjects.StoreManagement.StoreCardType>();} }
        static public Hospitality.ISalesTypeData SalesTypeData { get { return DataProviderFactory.Instance.Get<Hospitality.ISalesTypeData, BusinessObjects.Hospitality.SalesType>();} }
        static public TenderDeclaration.ITenderDeclarationData TenderDeclarationData { get { return DataProviderFactory.Instance.Get<TenderDeclaration.ITenderDeclarationData, BusinessObjects.Tenderdeclaration>(); } }
        static public TenderDeclaration.ITenderDeclarationLineData TenderDeclarationLineData { get { return DataProviderFactory.Instance.Get<TenderDeclaration.ITenderDeclarationLineData, BusinessObjects.TenderdeclarationLine>(); } }
        static public UserManagement.IPermissionData PermissionData { get { return DataProviderFactory.Instance.Get<UserManagement.IPermissionData, BusinessObjects.UserManagement.ListItems.PermissionsAssignmentResult>();} }
        static public Vouchers.ICreditVoucherLineData CreditVoucherLineData { get { return DataProviderFactory.Instance.Get<Vouchers.ICreditVoucherLineData, BusinessObjects.Vouchers.CreditVoucherLine>();} }
        static public Vouchers.IGiftCardLineData GiftCardLineData { get { return DataProviderFactory.Instance.Get<Vouchers.IGiftCardLineData, BusinessObjects.Vouchers.GiftCardLine>();} }
        static public Transactions.ITransactionData TransactionData { get { return DataProviderFactory.Instance.Get<Transactions.ITransactionData, BusinessObjects.Transactions.Transaction>();} }
        static public Terminals.ITerminalGroupConnectionData TerminalGroupConnectionData { get { return DataProviderFactory.Instance.Get<Terminals.ITerminalGroupConnectionData, BusinessObjects.Terminals.TerminalGroupConnection>();} }
        static public BarCodes.IBarcodeMaskData BarcodeMaskData { get { return DataProviderFactory.Instance.Get<BarCodes.IBarcodeMaskData, BusinessObjects.BarCodes.BarcodeMask>(); } }
        static public BarCodes.IBarCodeMaskSegmentData BarcodeMaskSegmentData { get { return DataProviderFactory.Instance.Get<BarCodes.IBarCodeMaskSegmentData, BusinessObjects.BarCodes.BarcodeMaskSegment>(); } }
        static public Card.ICardInfoData CardInfoData { get { return DataProviderFactory.Instance.Get<Card.ICardInfoData, BusinessObjects.Card.CardInfo>();} }
        static public Card.ICardToTenderMappingData CardToTenderMappingData { get { return DataProviderFactory.Instance.Get<Card.ICardToTenderMappingData, BusinessObjects.Card.CardToTenderMapping>(); } }
        static public Transactions.ISuspendedTransactionData SuspendedTransactionData { get { return DataProviderFactory.Instance.Get<Transactions.ISuspendedTransactionData, BusinessObjects.Transactions.SuspendedTransaction>();} }
        static public Transactions.ISuspensionTypeAdditionalInfoData SuspensionTypeAdditionalInfoData { get { return DataProviderFactory.Instance.Get<Transactions.ISuspensionTypeAdditionalInfoData, BusinessObjects.Transactions.SuspensionTypeAdditionalInfo>();} }
        static public Transactions.ISuspendedTransactionAnswerData SuspendedTransactionAnswerData { get { return DataProviderFactory.Instance.Get<Transactions.ISuspendedTransactionAnswerData, BusinessObjects.Transactions.SuspendedTransactionAnswer>();} }
        static public Transactions.IPaymentTransactionData PaymentTransactionData { get { return DataProviderFactory.Instance.Get<Transactions.IPaymentTransactionData, BusinessObjects.Transactions.PaymentTransaction>();} }
        static public ItemMaster.IItemTranslationData ItemTranslationData { get { return DataProviderFactory.Instance.Get<ItemMaster.IItemTranslationData, BusinessObjects.ItemMaster.ItemTranslation>();} }
        static public UserManagement.IAuthenticationTokenData AuthenticationTokenData { get { return DataProviderFactory.Instance.Get<UserManagement.IAuthenticationTokenData, BusinessObjects.UserManagement.AuthenticationToken>();} }
        static public PricesAndDiscounts.IDiscountAndPriceActivationData DiscountAndPriceActivationData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IDiscountAndPriceActivationData, BusinessObjects.PricesAndDiscounts.DiscountAndPriceActivation>();} }
        static public PricesAndDiscounts.IMultibuyDiscountLineData MultibuyDiscountLineData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IMultibuyDiscountLineData, BusinessObjects.PricesAndDiscounts.MultibuyDiscountLine>();} }
        static public PricesAndDiscounts.IMixAndMatchLineGroupData MixAndMatchLineGroupData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IMixAndMatchLineGroupData, BusinessObjects.PricesAndDiscounts.MixAndMatchLineGroup>();} }
        static public Card.ICardTypeData CardTypeData { get { return DataProviderFactory.Instance.Get<Card.ICardTypeData, BusinessObjects.Card.CardType>();} }
        static public StoreManagement.IStoreCardTypesData StoreCardTypesData { get { return DataProviderFactory.Instance.Get<StoreManagement.IStoreCardTypesData, BusinessObjects.StoreManagement.StorePaymentTypeCardType>();} }
        static public EFT.IEFTMappingData EFTMappingData { get { return DataProviderFactory.Instance.Get<EFT.IEFTMappingData, BusinessObjects.EFT.EFTMapping>();} }
        static public EFT.IEFTPaymentData EFTPaymentData { get { return DataProviderFactory.Instance.Get<EFT.IEFTPaymentData, BusinessObjects.EFT.EFTPayment>();} }
        static public Statements.IStatementInfoData StatementInfoData { get { return DataProviderFactory.Instance.Get<Statements.IStatementInfoData, BusinessObjects.StatementInfo>();} }
        static public EOD.IZReportData ZReportData { get { return DataProviderFactory.Instance.Get<EOD.IZReportData, BusinessObjects.EOD.ZReport>();} }
        static public Sequences.INumberSequenceData NumberSequenceData { get { return DataProviderFactory.Instance.Get<Sequences.INumberSequenceData, BusinessObjects.Sequencable.NumberSequence>();} }
        static public Hospitality.IPrintingStationData PrintingStationData { get { return DataProviderFactory.Instance.Get<Hospitality.IPrintingStationData, BusinessObjects.Hospitality.PrintingStation>();} }
        static public Hospitality.IStationSelectionData StationSelectionData { get { return DataProviderFactory.Instance.Get<Hospitality.IStationSelectionData, BusinessObjects.Hospitality.StationSelection>();} }
        static public Hospitality.IRestaurantMenuTypeData RestaurantMenuTypeData { get { return DataProviderFactory.Instance.Get<Hospitality.IRestaurantMenuTypeData, BusinessObjects.Hospitality.RestaurantMenuType>();} }
        static public Hospitality.IDiningTableLayoutData DiningTableLayoutData { get { return DataProviderFactory.Instance.Get<Hospitality.IDiningTableLayoutData, BusinessObjects.Hospitality.DiningTableLayout>();} }
        static public Hospitality.IRestaurantDiningTableData RestaurantDiningTableData { get { return DataProviderFactory.Instance.Get<Hospitality.IRestaurantDiningTableData, BusinessObjects.Hospitality.RestaurantDiningTable>();} }
        static public Hospitality.IPosLookupData PosLookupData { get { return DataProviderFactory.Instance.Get<Hospitality.IPosLookupData, BusinessObjects.Hospitality.PosLookup>();} }
        static public Hospitality.IStationPrintingHostData StationPrintingHostData { get { return DataProviderFactory.Instance.Get<Hospitality.IStationPrintingHostData, BusinessObjects.Hospitality.StationPrintingHost>();} }
        static public Transactions.IInfoCodeLineData InfoCodeLineData { get { return DataProviderFactory.Instance.Get<Transactions.IInfoCodeLineData, BusinessObjects.Transactions.Line.InfoCodeLineItem>();} }
        static public Infocodes.IInfocodeSubcodeData InfocodeSubcodeData { get { return DataProviderFactory.Instance.Get<Infocodes.IInfocodeSubcodeData, BusinessObjects.Infocodes.InfocodeSubcode>();} }
        static public Inventory.IInventoryTransferData InventoryTransferData { get { return DataProviderFactory.Instance.Get<IInventoryTransferData, DataEntity>();} }
        static public Inventory.IInventoryTransferOrderData InventoryTransferOrderData { get { return DataProviderFactory.Instance.Get<IInventoryTransferOrderData, InventoryTransferOrder>();} }
        static public Inventory.IInventoryTransferOrderLineData InventoryTransferOrderLineData { get { return DataProviderFactory.Instance.Get<IInventoryTransferOrderLineData, InventoryTransferOrderLine>();} }
        static public Inventory.IInventoryTransferRequestData InventoryTransferRequestData { get { return DataProviderFactory.Instance.Get<IInventoryTransferRequestData, InventoryTransferRequest>();} }
        static public Inventory.IInventoryTransferRequestLineData InventoryTransferRequestLineData { get { return DataProviderFactory.Instance.Get<IInventoryTransferRequestLineData, InventoryTransferRequestLine>();} }
        static public Replenishment.IPurchaseWorksheetLineData PurchaseWorksheetLineData { get { return DataProviderFactory.Instance.Get<Replenishment.IPurchaseWorksheetLineData, BusinessObjects.Replenishment.PurchaseWorksheetLine>();} }
        static public Labels.ILabelQueueData LabelQueueData { get { return DataProviderFactory.Instance.Get<Labels.ILabelQueueData, BusinessObjects.Labels.LabelQueue>();} }
        static public Labels.ILabelTemplateData LabelTemplateData { get { return DataProviderFactory.Instance.Get<Labels.ILabelTemplateData, BusinessObjects.Labels.LabelTemplate>();} }
        static public Loyalty.ILoyaltyPointsData LoyaltyPointsData { get { return DataProviderFactory.Instance.Get<Loyalty.ILoyaltyPointsData, BusinessObjects.Loyalty.LoyaltyPoints>();} }
        static public Loyalty.ILoyaltyCustomerParamsData LoyaltyCustomerParamsData { get { return DataProviderFactory.Instance.Get<Loyalty.ILoyaltyCustomerParamsData, BusinessObjects.Loyalty.LoyaltyCustomerParams>();} }
        static public LookupValues.IMsrCardLinkData MsrCardLinkData { get { return DataProviderFactory.Instance.Get<LookupValues.IMsrCardLinkData, BusinessObjects.LookupValues.MsrCardLink>();} }
        static public ItemMaster.ILinkedItemData LinkedItemData { get { return DataProviderFactory.Instance.Get<ItemMaster.ILinkedItemData, BusinessObjects.ItemMaster.LinkedItem>();} }
        static public POS.IPOSKeyboardButtonControlData POSKeyboardButtonControlData { get { return DataProviderFactory.Instance.Get<POS.IPOSKeyboardButtonControlData, BusinessObjects.POS.POSKeyboardButtonControl>();} }
        static public PricesAndDiscounts.ITradeAgreementData TradeAgreementData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.ITradeAgreementData, BusinessObjects.PricesAndDiscounts.TradeAgreementEntry>();} }
        static public Settings.IDecimalSettingsData DecimalSettingsData { get { return DataProviderFactory.Instance.Get<Settings.IDecimalSettingsData, BusinessObjects.DecimalSetting>();} }
        static public Ledger.ICustomerLedgerEntriesData CustomerLedgerEntriesData { get { return DataProviderFactory.Instance.Get<Ledger.ICustomerLedgerEntriesData, BusinessObjects.Ledger.CustomerLedgerEntries>();} }
        static public Loyalty.ILoyaltyCustomerData LoyaltyCustomerData { get { return DataProviderFactory.Instance.Get<Loyalty.ILoyaltyCustomerData, BusinessObjects.Loyalty.LoyaltyCustomer>();} }
        static public Tax.ITaxItemData TaxItemData { get { return DataProviderFactory.Instance.Get<Tax.ITaxItemData, BusinessObjects.Tax.TaxItem>();} }
        static public Inventory.IVendorData VendorData { get { return DataProviderFactory.Instance.Get<Inventory.IVendorData, BusinessObjects.Inventory.Vendor>();} }
        static public LookupValues.IPaymentLimitationsData PaymentLimitationsData { get { return DataProviderFactory.Instance.Get<LookupValues.IPaymentLimitationsData, BusinessObjects.LookupValues.PaymentMethodLimitation>();} }
        static public Vouchers.ICreditVoucherData CreditVoucherData { get { return DataProviderFactory.Instance.Get<Vouchers.ICreditVoucherData, BusinessObjects.Vouchers.CreditVoucher>();} }
        static public EMails.IEMailSettingData EMailSettingData { get { return DataProviderFactory.Instance.Get<EMails.IEMailSettingData, BusinessObjects.EMails.EMailSetting>();} }
        static public EMails.IEMailQueueEntryData EMailQueueEntryData { get { return DataProviderFactory.Instance.Get<EMails.IEMailQueueEntryData, BusinessObjects.EMails.EMailQueueEntry>();} }
        static public EMails.IEMailQueueAttachmentData EMailQueueAttachmentData { get { return DataProviderFactory.Instance.Get<EMails.IEMailQueueAttachmentData, BusinessObjects.EMails.EMailQueueAttachment>();} }
        static public Vouchers.IGiftCardData GiftCardData { get { return DataProviderFactory.Instance.Get<Vouchers.IGiftCardData, BusinessObjects.Vouchers.GiftCard>();} }
        static public Hospitality.ITableInfoData TableInfoData { get { return DataProviderFactory.Instance.Get<Hospitality.ITableInfoData, BusinessObjects.Hospitality.TableInfo>();} }
        static public Inventory.IInventoryData InventoryData { get { return DataProviderFactory.Instance.Get<Inventory.IInventoryData, BusinessObjects.Inventory.InventoryTransaction>();} }
        static public BarCodes.IBarCodeSetupData BarCodeSetupData { get { return DataProviderFactory.Instance.Get<BarCodes.IBarCodeSetupData, BusinessObjects.BarCodes.BarCodeSetup>();} }
        static public ConfigurationWizard.IWizardTemplateViewData WizardTemplateViewData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.IWizardTemplateViewData, BusinessObjects.ConfigurationWizard.WizardTemplateView>();} }
        static public ConfigurationWizard.IStoreSettingsData StoreSettingsData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.IStoreSettingsData, BusinessObjects.ConfigurationWizard.StoreSettings>();} }
        static public ConfigurationWizard.IPaymentsAndCurrencyData PaymentsAndCurrencyData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.IPaymentsAndCurrencyData, BusinessObjects.ConfigurationWizard.PaymentsAndCurrency>();} }
        static public ConfigurationWizard.IPeripheralsData PeripheralsData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.IPeripheralsData, BusinessObjects.ConfigurationWizard.Peripherals>();} }
        static public ConfigurationWizard.ITouchButtonLayoutData TouchButtonLayoutData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.ITouchButtonLayoutData, BusinessObjects.ConfigurationWizard.TouchButtonLayout>();} }
        static public ConfigurationWizard.IReceiptsData ReceiptsData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.IReceiptsData, BusinessObjects.Forms.Form>();} }
        static public ConfigurationWizard.IRetailGroupsData RetailGroupsData { get { return DataProviderFactory.Instance.Get<ConfigurationWizard.IRetailGroupsData, BusinessObjects.ConfigurationWizard.RetailGroups>();} }
        static public Customers.IGroupCategoryData GroupCategoryData { get { return DataProviderFactory.Instance.Get<Customers.IGroupCategoryData, BusinessObjects.Customers.GroupCategory>();} }
        static public Customers.ICustomerGroupData CustomerGroupData { get { return DataProviderFactory.Instance.Get<Customers.ICustomerGroupData, BusinessObjects.Customers.CustomerGroup>();} }
        static public Customers.ICustomersInGroupData CustomersInGroupData { get { return DataProviderFactory.Instance.Get<Customers.ICustomersInGroupData, BusinessObjects.Customers.CustomerInGroup>(); } }
        static public Financials.IFinancialReportData FinancialReportData { get { return DataProviderFactory.Instance.Get<Financials.IFinancialReportData, BusinessObjects.FinancialReportTaxGroupLine>();} }
        static public Replenishment.IItemReplenishmentSettingData ItemReplenishmentSettingData { get { return DataProviderFactory.Instance.Get<Replenishment.IItemReplenishmentSettingData, BusinessObjects.Replenishment.ItemReplenishmentSetting>();} }
        static public Contacts.ICountryData CountryData { get { return DataProviderFactory.Instance.Get<Contacts.ICountryData, BusinessObjects.DataEntity>();} }
        static public Forms.IFormTypeData FormTypeData { get { return DataProviderFactory.Instance.Get<Forms.IFormTypeData, BusinessObjects.Forms.FormType>();} }
        static public Profiles.IFormProfileData FormProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IFormProfileData, BusinessObjects.Profiles.FormProfile>();} }
        static public Hospitality.IDiningTableLayoutScreenData DiningTableLayoutScreenData { get { return DataProviderFactory.Instance.Get<Hospitality.IDiningTableLayoutScreenData, BusinessObjects.Hospitality.DiningTableLayoutScreen>();} }
        static public Hospitality.IPosColorData PosColorData { get { return DataProviderFactory.Instance.Get<Hospitality.IPosColorData, BusinessObjects.Hospitality.PosColor>();} }
        static public Hospitality.IHospitalityOperationData HospitalityOperationData { get { return DataProviderFactory.Instance.Get<Hospitality.IHospitalityOperationData, BusinessObjects.Hospitality.HospitalityOperation>();} }
        static public Hospitality.IPosParameterSetupData PosParameterSetupData { get { return DataProviderFactory.Instance.Get<Hospitality.IPosParameterSetupData, BusinessObjects.Hospitality.PosParameterSetup>();} }
        static public Hospitality.IFloorLayoutData FloorLayoutData { get { return DataProviderFactory.Instance.Get<Hospitality.IFloorLayoutData, BusinessObjects.Hospitality.FloorLayout>(); } }
        static public Infocodes.IInfocodeSpecificData InfocodeSpecificData { get { return DataProviderFactory.Instance.Get<Infocodes.IInfocodeSpecificData, BusinessObjects.Infocodes.InfocodeSpecific>();} }
        static public Inventory.IPurchaseOrderMiscChargesData PurchaseOrderMiscChargesData { get { return DataProviderFactory.Instance.Get<Inventory.IPurchaseOrderMiscChargesData, BusinessObjects.Inventory.PurchaseOrderMiscCharges>();} }
        static public Inventory.IInventoryJournalTransactionData InventoryJournalTransactionData { get { return DataProviderFactory.Instance.Get<Inventory.IInventoryJournalTransactionData, BusinessObjects.Inventory.InventoryJournalTransaction>();} }
        static public IReasonCodeData ReasonCodeData { get { return DataProviderFactory.Instance.Get<IReasonCodeData, ReasonCode>(); } }
        static public StoreManagement.ICardNumberSerieData CardNumberSerieData { get { return DataProviderFactory.Instance.Get<StoreManagement.ICardNumberSerieData, BusinessObjects.StoreManagement.CardNumberSerie>();} }
        static public LookupValues.IRemoteHostData RemoteHostData { get { return DataProviderFactory.Instance.Get<LookupValues.IRemoteHostData, BusinessObjects.LookupValues.RemoteHost>();} }
        static public PricesAndDiscounts.IDiscountOfferLineData DiscountOfferLineData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IDiscountOfferLineData, BusinessObjects.PricesAndDiscounts.DiscountOfferLine>();} }
        static public Profiles.IStyleProfileData StyleProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IStyleProfileData, BusinessObjects.Profiles.StyleProfile>();} }
        static public Transactions.IReceiptData ReceiptData { get { return DataProviderFactory.Instance.Get<Transactions.IReceiptData, BusinessObjects.Transactions.ReceiptListItem>();} }
        static public Replenishment.IInventoryAreaData InventoryAreaData { get { return DataProviderFactory.Instance.Get<Replenishment.IInventoryAreaData, BusinessObjects.Replenishment.InventoryArea>();} }
        static public Replenishment.IInventoryTemplateData InventoryTemplateData { get { return DataProviderFactory.Instance.Get<Replenishment.IInventoryTemplateData, BusinessObjects.Replenishment.InventoryTemplate>();} }
        static public Replenishment.IInventoryTemplateStoreConnectionData InventoryTemplateStoreConnectionData { get { return DataProviderFactory.Instance.Get<Replenishment.IInventoryTemplateStoreConnectionData, BusinessObjects.Replenishment.InventoryTemplateStoreConnection>();} }
        static public Replenishment.IPurchaseWorksheetData PurchaseWorksheetData { get { return DataProviderFactory.Instance.Get<Replenishment.IPurchaseWorksheetData, BusinessObjects.Replenishment.PurchaseWorksheet>();} }
        static public Replenishment.IInventoryTemplateSectionData InventoryTemplateSectionData { get { return DataProviderFactory.Instance.Get<Replenishment.IInventoryTemplateSectionData, BusinessObjects.Replenishment.InventoryTemplateSection>();} }
        static public Replenishment.IInventoryTemplateSectionSelectionData InventoryTemplateSectionSelectionData { get { return DataProviderFactory.Instance.Get<Replenishment.IInventoryTemplateSectionSelectionData, BusinessObjects.Replenishment.InventoryTemplateSectionSelection>();} }
        static public Replenishment.IInventoryTemplateItemFilterData InventoryTemplateItemFilterData { get { return DataProviderFactory.Instance.Get<Replenishment.IInventoryTemplateItemFilterData, BusinessObjects.Replenishment.ListItems.InventoryTemplateFilterListItem>();} }
        static public Reports.IReportEnumValueData ReportEnumValueData { get { return DataProviderFactory.Instance.Get<Reports.IReportEnumValueData, BusinessObjects.Reports.ReportEnumValue>();} }
        static public Reports.IReportContextData ReportContextData { get { return DataProviderFactory.Instance.Get<Reports.IReportContextData, BusinessObjects.Reports.ReportContext>();} }
        static public Reports.IProcedureParameterData ProcedureParameterData { get { return DataProviderFactory.Instance.Get<Reports.IProcedureParameterData, BusinessObjects.Reports.ProcedureParameter>();} }
        static public IItemLedgerData ItemLedgerData { get { return DataProviderFactory.Instance.Get<IItemLedgerData, ItemLedger>();} }
        static public Terminals.ITerminalGroupData TerminalGroupData { get { return DataProviderFactory.Instance.Get<Terminals.ITerminalGroupData, BusinessObjects.Terminals.TerminalGroup>();} }
        static public PricesAndDiscounts.IItemInPriceDiscountGroupData ItemInPriceDiscountGroupData { get { return DataProviderFactory.Instance.Get<PricesAndDiscounts.IItemInPriceDiscountGroupData, BusinessObjects.PricesAndDiscounts.ItemInPriceDiscountGroup>();} }
        static public UserManagement.IUserMigrationCommands UserMigrationCommands { get { return DataProviderFactory.Instance.Get<UserManagement.IUserMigrationCommands, BusinessObjects.UserManagement.POSUser>();} }
        static public UserManagement.IPermissionGroupData PermissionGroupData { get { return DataProviderFactory.Instance.Get<UserManagement.IPermissionGroupData, DataEntity>();} }
        static public UserInterface.IUIStyleData UIStyleData { get { return DataProviderFactory.Instance.Get<UserInterface.IUIStyleData, BusinessObjects.UserInterface.UIStyle>();} }
        static public Integrations.IIntegrationLogData IntegrationLogData { get { return DataProviderFactory.Instance.Get<Integrations.IIntegrationLogData, BusinessObjects.Integrations.IntegrationLog>(); } }
        static public Integrations.IIntegrationMappingData IntegrationMappingData { get { return DataProviderFactory.Instance.Get<Integrations.IIntegrationMappingData, BusinessObjects.Integrations.IntegrationMapping>(); } }
        static public Fiscal.IFiscalData FiscalData { get { return DataProviderFactory.Instance.Get<Fiscal.IFiscalData, BusinessObjects.Fiscal.FiscalTrans>(); } }
        static public Fiscal.IFiscalLogData FiscalLogData { get { return DataProviderFactory.Instance.Get<Fiscal.IFiscalLogData, BusinessObjects.Fiscal.FiscalLogEntity>(); } }
        static public Log.IClearPosisLog ClearPosisLogData { get { return DataProviderFactory.Instance.Get<Log.IClearPosisLog, DataEntity>(); } }
        static public CustomerOrders.ICustomerOrderSettings CustomerOrderSettingsData { get { return DataProviderFactory.Instance.Get<CustomerOrders.ICustomerOrderSettings, CustomerOrderSettings>(); } }
        static public CustomerOrders.ICustomerOrderAdditionalConfigurationData CustomerOrderAdditionalConfigData { get { return DataProviderFactory.Instance.Get<CustomerOrders.ICustomerOrderAdditionalConfigurationData, CustomerOrderAdditionalConfigurations>(); } }
        static public CustomerOrders.ICustomerOrderData CustomerOrderData { get { return DataProviderFactory.Instance.Get<CustomerOrders.ICustomerOrderData, CustomerOrder>(); } }
        static public Profiles.IImportProfileData ImportProfileData { get { return DataProviderFactory.Instance.Get<Profiles.IImportProfileData, BusinessObjects.Profiles.ImportProfile>(); } }
        static public Profiles.IImportProfileLineData ImportProfileLineData { get { return DataProviderFactory.Instance.Get<Profiles.IImportProfileLineData, BusinessObjects.Profiles.ImportProfileLine>(); } }
        static public SerialNumbers.ISerialNumberData SerialNumberData { get { return DataProviderFactory.Instance.Get<SerialNumbers.ISerialNumberData, BusinessObjects.SerialNumbers.SerialNumber>(); } }
        static public Profiles.IShorthandItemData ShortHandItemData { get { return DataProviderFactory.Instance.Get<Profiles.IShorthandItemData, BusinessObjects.Profiles.ShorthandItem>(); } }        

        static public KitchenDisplaySystem.IKitchenDisplayTransactionProfileData KitchenDisplayTransactionProfileData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayTransactionProfileData, BusinessObjects.Profiles.KitchenServiceProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayVisualProfileData KitchenDisplayVisualProfileData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayVisualProfileData, KDSBusinessObjects.KitchenDisplayVisualProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayTimeStyleData KitchenDisplayTimeStyleData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayTimeStyleData, KDSBusinessObjects.KitchenDisplayTimeStyle>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayTerminalRoutingConnectionData KitchenDisplayTerminalRoutingConnectionData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayTerminalRoutingConnectionData, LSOneKitchenDisplayTerminalRoutingConnection>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayStyleProfileData KitchenDisplayStyleProfileData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayStyleProfileData, KDSBusinessObjects.LSOneKitchenDisplayStyleProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayStationData KitchenDisplayStationData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayStationData, KDSBusinessObjects.KitchenDisplayStation>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayProfileData KitchenDisplayProfileData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayProfileData, KDSBusinessObjects.KitchenDisplayProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayPrinterData KitchenDisplayPrinterData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayPrinterData, KDSBusinessObjects.KitchenDisplayPrinter>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayLineData KitchenDisplayLineData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayLineData, KDSBusinessObjects.KitchenDisplayLine>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayLineColumnData KitchenDisplayLineColumnData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayLineColumnData, KDSBusinessObjects.KitchenDisplayLineColumn>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayItemRoutingConnectionData KitchenDisplayItemRoutingConnectionData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayItemRoutingConnectionData, LSOneKitchenDisplayItemRoutingConnection>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayHospitalityTypeRoutingConnectionData KitchenDisplayHospitalityTypeRoutingConnectionData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayHospitalityTypeRoutingConnectionData, LSOneKitchenDisplayHospitalityTypeRoutingConnection>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayFunctionalProfileData KitchenDisplayFunctionalProfileData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayFunctionalProfileData, KDSBusinessObjects.KitchenDisplayFunctionalProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayProductionSectionData KitchenDisplayProductionSectionData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayProductionSectionData, KitchenDisplayProductionSection>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayItemSectionRoutingData KitchenDisplayItemSectionRoutingData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayItemSectionRoutingData, KitchenDisplayItemSectionRouting>(); } }
        static public KitchenDisplaySystem.IKitchenDisplaySectionStationRoutingData KitchenDisplaySectionStationRoutingData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplaySectionStationRoutingData, KitchenDisplaySectionStationRouting>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayHeaderPaneData KitchenDisplayHeaderPaneData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayHeaderPaneData, KDSBusinessObjects.HeaderPaneProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayHeaderPaneLineData KitchenDisplayHeaderPaneLineData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayHeaderPaneLineData, KDSBusinessObjects.HeaderPaneLine>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayHeaderPaneLineColumnData KitchenDisplayHeaderPaneLineColumnData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayHeaderPaneLineColumnData, LSOneHeaderPaneLineColumn>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayAggregateProfileData KitchenDisplayAggregateProfileData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayAggregateProfileData, KDSBusinessObjects.AggregateProfile>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayAggregateProfileGroupData KitchenDisplayAggregateProfileGroupData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayAggregateProfileGroupData, KDSBusinessObjects.AggregateProfileGroup>(); } }
        static public KitchenDisplaySystem.IKitchenDisplayAggregateGroupItemData KitchenDisplayAggregateGroupItemData { get { return DataProviderFactory.Instance.Get<KitchenDisplaySystem.IKitchenDisplayAggregateGroupItemData, BusinessObjects.KitchenDisplaySystem.AggregateGroupItem>(); } }

        static public ITimeKeeperData TimeKeeperData { get { return DataProviderFactory.Instance.Get<ITimeKeeperData, TimeKept>(); } }
    }
}
