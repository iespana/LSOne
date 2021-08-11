namespace LSOne.DataLayer.BusinessObjects
{
	public class Permission
	{
		public const string SecurityCreateNewUsers = "290af560-fa2c-11da-974d-0800200c9a66";
		public const string SecurityEditUser = "e5971c50-554d-11db-b0de-0800200c9a66";
		public const string SecurityViewUsers = "A1D66FBE-0CF6-4075-92D0-967772E50E48";
		public const string SecurityDeleteUser = "95d51e30-face-11da-974d-0800200c9a66";
		public const string SecurityManageUserPermissions = "6dfbc670-5eb5-11db-b0de-0800200c9a66";
		public const string SecurityEnableDisableUser = "ed7b4700-6049-11db-b0de-0800200c9a66";
		public const string SecurityResetPassword = "2defc640-fa05-11da-974d-0800200c9a66";

		public const string SecurityDeleteUserGroups = "17d4aae0-5d03-11db-b0de-0800200c9a66";
		public const string SecurityCreateUserGroups = "a56c7d70-5d02-11db-b0de-0800200c9a66";
		public const string SecurityEditUserGroups = "032cbbf0-5d03-11db-b0de-0800200c9a66";
		public const string SecurityManageGroupPermissions = "6d5650e0-5d03-11db-b0de-0800200c9a66";
		public const string SecurityAssignUserToGroup = "30bbcd90-fac0-11da-974d-0800200c9a66";

		public const string SecurityGrantPermissions = "3eb1f3a0-fac2-11da-974d-0800200c9a66";
		public const string SecurityGrantHigherPermissions = "f4c88680-fac3-11da-974d-0800200c9a66";

		public const string SecurityManageAuthenticationTokens = "92d1c390-6edb-11e2-bcfd-0800200c9a66";

		public const string StoreView = "59188a60-b72f-11de-8a39-0800200c9a66";
		public const string StoreEdit = "60f70180-b72f-11de-8a39-0800200c9a66";
		public const string TerminalView = "edf40550-ba3c-11de-8a39-0800200c9a66";
		public const string TerminalEdit = "f33e8b20-ba3c-11de-8a39-0800200c9a66";
		public const string CurrencyView = "a330ca10-89af-11df-a4ee-0800200c9a66";
		public const string CurrencyEdit = "1dd55800-89ae-11df-a4ee-0800200c9a66";

		public const string IncomeExpenseView = "71d5edaa-3743-43b3-9c8c-d3cb8d8fd708";

		public const string ColorSizeStyleEdit = "d27f0360-04e7-11df-8a39-0800200c9a66";

		public const string ItemsEdit = "9c29d3a0-09bd-11df-8a39-0800200c9a66";
		public const string ItemsView = "8f623090-09bd-11df-8a39-0800200c9a66";
		public const string MultiEditItems = "E1839574-3AD1-4D57-BEC1-BB30E5D78ACA";
		public const string ManageItemTypes = "2a84f88d-85da-4828-a9e0-9d97db6eba69";
		public const string ManageItemCostPrice = "f6962a7c-38ef-463b-87a8-1dbf172033cd";

        public const string ManageRetailDepartments = "cf194f40-9a39-11df-981c-0800200c9a66";

		public const string ManageDiscounts = "c2417850-9643-11df-981c-0800200c9a66";
		public const string ManageTradeAgreementPrices = "b37bc520-2080-11df-8a39-0800200c9a66";

		public const string ManageRetailDivisions = "c18bea2d-7b24-41a2-8db5-025aa3178424";

		public const string ManageRetailGroups = "22e640f0-a6de-11df-981c-0800200c9a66";

		public const string ManageItemDimensions = "1bc710f0-0bf8-11df-8a39-0800200c9a66";

		public const string ManageSerialNumbers = "cc6ed4ca-19ae-4f1b-9021-8b0a54ff2bbe";

		public const string ManageVendorsOnItems = "50bd6e9e-716f-4264-b8bc-cf790221e1d8";

		public const string ViewSalesTaxSetup = "82cb0090-7890-11df-93f2-0800200c9a66";
		public const string EditSalesTaxSetup = "880d9720-7890-11df-93f2-0800200c9a66";

		public const string ManageSpecialGroups = "79c2f690-ae93-11df-94e2-0800200c9a66";

		public const string CustomerView = "ad69d4d0-d9ac-11de-8a39-0800200c9a66";
		public const string CustomerEdit = "b545daf0-d9ac-11de-8a39-0800200c9a66";

		public const string CategoriesView = "4DD9F547-BD6C-44B6-9947-31F91A017DD1";
		public const string CategoriesEdit = "6A4C3850-4779-46BA-9F05-C77FDD1A2267";

		public const string CustomerGroupsView = "A33B4F8D-7A01-467B-85AD-BBB996F3EC08";
		public const string CustomerGroupsEdit = "0C57FE60-A545-487F-9713-FF5730E73F58";

		public const string InfocodeView = "fe0375e0-b4ee-11df-8d81-0800200c9a66";
		public const string InfocodeEdit = "dd56aac0-b506-11df-8d81-0800200c9a66";

		public const string EFTMappingView = "6c41bac6-871e-4b1f-80cc-5349902d5181";
		public const string EFTMappingEdit = "ca74d61c-7894-4364-bd91-55a3f98e6a0f";

		public const string ConfigurationWizardView = "2D06DC46-AA16-42E4-BA46-1730B1D060D6";
		public const string ConfigurationWizardEdit = "FFE6DA02-0098-4481-AA64-F4C7F6EBD676";

		public const string ManageUnits = "4c385c10-bc03-11df-851a-0800200c9a66";

		public const string ManageLSLicense = "d09c0340-85e3-11df-a4ee-0800200c9a66";

		public const string ViewPriceGroups = "4c66df90-c0b8-11df-851a-0800200c9a66";
		public const string EditPriceGroups = "F4F59C82-14AE-44DA-B307-63563581D1F2";

		public const string ViewItemLedger = "54da9e65-e48d-4cfc-9807-248699d9e82a";

		public const string ViewIntegrationMappings = "ecc57e15-3ea1-48e1-854e-b45379222b19";
		public const string EditIntegrationMapping = "9790919b-e1be-4e88-97db-bcac2c5b2343";
		public const string ViewIntegrationLog = "1B27F8A0-EAE3-4211-AA10-D930F62671BC";

		public const string ManageUIStyleSetup = "0339DB04-6381-4F08-96B6-F8D36BE33908";

		public const string ViewTaxFree = "EF573EF1-6543-40B5-A61D-C813FDD4F64D";

		// Hospitality permissions
		// ===
		public const string ManageHospitalitySetup = "c8a068f0-a3b8-11df-981c-0800200c9a66";
		public const string ManageSalesTypes = "dcd1c900-dac6-11df-937b-0800200c9a66";
		public const string ManageHospitalityTypes = "070c7f80-e8d2-11df-9492-0800200c9a66";
		public const string ManageDiningTableLayouts = "5c024140-f3eb-11df-98cf-0800200c9a66";
		public const string ManageRestaurantMenuTypes = "bf5dc860-02b7-11e0-a976-0800200c9a66";
		public const string ManageStationPrinting = "c8039500-03a5-11e0-a976-0800200c9a66";
		public const string EditPosMenus = "19f52730-0c3d-11e0-81e0-0800200c9a66";
		public const string ViewPosMenus = "1265ec10-0c3e-11e0-81e0-0800200c9a66";
		public const string ManageMenuGroups = "7cf60320-19b4-11e0-ac64-0800200c9a66";
		public const string ManageKitchenServiceProfiles = "B02E9A4C-F932-4528-97B3-0703ED352471";
		public const string ManageKitchenDisplayStations = "1EBF0956-B3A6-4AFA-8149-E257E1EE7BE3";
		public const string ManageKitchenDisplayProfiles = "2E353342-0FF3-4342-8386-CF35154E95B3";
		public const string ManageKitchenPrinters = "211a5a6e-3654-4490-bb5c-63b89e26f102";
		public const string HospitalityUnlockTable = "19CF937E-5570-41FD-8ABE-BDFF5A9C68D5";
		// ===

		public const string EditCustomerDiscGroups = "98daf440-6cb9-11df-be2b-0800200c9a66";
		public const string ViewCustomerDiscGroups = "94780f50-6cb9-11df-be2b-0800200c9a66";
		public const string ViewItemDiscGroups = "c7070150-7241-11df-93f2-0800200c9a66";
		public const string EditItemDiscGroups = "cef38230-7241-11df-93f2-0800200c9a66";

		public const string RunEndOfDay = "b4284d70-89aa-11df-a4ee-0800200c9a66";

		public const string ManageItemBarcodes = "f9ff4cd0-0bf7-11df-8a39-0800200c9a66";
		public const string ManageBarcodesMasks = "0fa596b0-118f-11df-8a39-0800200c9a66";
		public const string ManageBarcodeSetup = "162480B4-0D65-4127-A798-A047B7319A56";

		public const string VisualProfileView = "51d29820-b35d-11de-8a39-0800200c9a66";
		public const string VisualProfileEdit = "691974e0-b35d-11de-8a39-0800200c9a66";

		public const string FunctionalProfileView = "a8d6e570-db3c-11de-8a39-0800200c9a66";
		public const string FunctionalProfileEdit = "b1190060-db3c-11de-8a39-0800200c9a66";

		public const string TransactionServiceProfileView = "4d101050-dffa-11de-8a39-0800200c9a66";
		public const string TransactionServiceProfileEdit = "5ddbaf70-dffa-11de-8a39-0800200c9a66";

		public const string HardwareProfileView = "acca1c80-e3eb-11de-8a39-0800200c9a66";
		public const string HardwareProfileEdit = "b556adf0-e3eb-11de-8a39-0800200c9a66";

		public const string StyleProfileView = "91395ae0-bb92-11e1-afa7-0800200c9a66";
		public const string StyleProfileEdit = "a0953d60-bb92-11e1-afa7-0800200c9a66";

		public const string FormProfileView = "aa918970-d1a1-11e1-9b23-0800200c9a66";
		public const string FormProfileEdit = "b5e4dc00-d1a1-11e1-9b23-0800200c9a66";

		public const string ManageUserProfiles = "5F770172-1438-4C09-853C-9775D1560AC6";
		public const string ManageImportProfiles = "ae391482-815c-4187-9b2b-4bb3ba454063";

		public const string CalculateStatement = "c1fc2460-f0b8-11df-98cf-0800200c9a66";
		public const string PostStatement = "d1408740-f0b8-11df-98cf-0800200c9a66";

		// Inventory permissions
		public const string ManagePurchaseOrders = "3fbc4b20-f3e1-11df-98cf-0800200c9a66";
		public const string ManageGoodsReceivingDocuments = "ea89d350-0d0a-11e0-81e0-0800200c9a66";
		public const string VendorView = "2802ec60-f624-11df-98cf-0800200c9a66";
		public const string VendorEdit = "76fb7210-f624-11df-98cf-0800200c9a66";
		public const string ManageInventoryAdjustmentsForAllStores = "85851D77-483F-435E-92D3-DAD722A8512F";
		public const string ViewInventoryAdjustments = "db2cf220-f6df-11df-98cf-0800200c9a66";
		public const string EditInventoryAdjustments = "69bf30a0-f6e7-11df-98cf-0800200c9a66";
		public const string StockCounting = "c59f3020-09f0-11e0-81e0-0800200c9a66";
		public const string PostAndReceivePO = "63FCEB22-13D6-4AC2-BF96-E1C3BEA1B83F";
		public const string AutoPopulateGRDLines = "48F6E912-B38F-4CA5-83BF-B78DED9B4066";

		public const string ManageRemoteHosts = "74c27750-27cf-11e0-91fa-0800200c9a66";
		public const string AdministrationMaintainSettings = "d3bed760-a229-11db-8ab9-0800200c9a66";
		public const string AdministrationEditNumberSequences = "59fddf70-c72d-11df-bd3b-0800200c9a66";
		public const string ImportDataPackages = "0843b114-359f-4f56-8f62-b7c310c0615f";
		public const string ResetJournalProcessingStatus = "9cf869ac-84d7-4532-87a9-33c22f18ae33";

		public const string ManageTouchButtonLayout = "5569a210-d44d-11de-8a39-0800200c9a66";
		public const string ManageStyleSetup = "0339DB04-6381-4F08-96B6-F8D36BE33908";
		public const string ManageImageBank = "59ABC187-00A0-450A-B3A0-674BBCC779DA";

		public const string ContextView = "0A456F9A-342C-4B21-8EBC-C19FDDB699B3";
		public const string ContextEdit = "4C2AF7AC-5D98-497C-9323-9A749908334E";

		public const string ManageInventoryTemplates = "6593B5D3-1464-4B86-9559-E131947C7859";
		public const string ManageReplenishment = "4B538D9F-E793-4463-84D6-FCC1405E560C";
		public const string ManageParkedInventory = "8F61C556-EBF2-47EA-9A58-F9174070515A";
		public const string ManageInventoryAdjustments = "9F77B529-30EA-4244-9C7C-D501FBA36F8C";
		public const string ManageStockReservations = "5E54791E-05FF-45C8-BE61-B150B2484FC6";
		public const string ManageStockReservationsForAllStores = "6159A8F8-EE17-464F-9CB9-42D915B985EB";
		public const string ManageParkedInventoryForAllStores = "4C09DBFD-D5E9-45C3-A7EF-0C4E32D87D64";
		public const string ViewInventoryForAllStores = "7AB1D100-589F-49E1-8851-7444670AFF36";
		public const string ManagePurchaseOrdersForAllStores = "7AB1D100-589F-49E1-8851-7444670AFF37";
		public const string ManageGoodsReceivingForAllStores = "7AB1D100-589F-49E1-8851-7444670AFF38";
		public const string CreateStoreTransfersForAllStores = "7AB1D100-589F-49E1-8851-7444670AFF39";
		public const string ManageTransfersForAllStores = "7AB1D100-589F-49E1-8851-7444670AFF40";

		public const string ViewOnHandInventory = "8D3CD897-E735-471E-A49D-E0D814834F44";

		//TenderDeclaration is declared below
		public const string TenderDeclaration = "975f9ad0-643e-11e0-ae3e-0800200c9a66";

		public const string CardTypesView = "118f9e90-c53f-11de-8a39-0800200c9a66";
		public const string CardTypesEdit = "1b777a40-c53f-11de-8a39-0800200c9a66";

		public const string PaymentMethodsView = "e3dbbea0-c941-11de-8a39-0800200c9a66";
		public const string PaymentMethodsEdit = "e8aa0f90-c941-11de-8a39-0800200c9a66";

		public const string ManageGiftCards = "ba50d4d0-a239-11e0-8264-0800200c9a66";
		public const string ManageCreditVouchers = "1d4e8dc0-d232-11e0-9572-0800200c9a66";
		public const string ManageCentralSuspensions = "20a29c60-e078-11e0-9572-0800200c9a66";
		public const string ManageSuspensionSettings = "0d25e7df-949a-4155-8161-90b959242412";
		public const string ManagePriceSettings = "E02B5156-4232-40CA-B44B-36BB974E1119";
		public const string ManageFuelSettings = "466A97BB-87B9-46A9-8E22-B173D948E4D6";
		public const string ManageAllowedPaymentSettings = "91A1596A-2BE4-4558-8481-D0D9EDB8A02C";
		public const string ManageCustomerGroups = "5C458EB3-2B84-4566-868E-492AD0F0B612";
        public const string ManageFiscalisationSettings = "40F486EA-D891-4ECD-AB72-E69BAE0B4653";

        public const string ManageLinkedItems = "88d35290-a232-11e0-8264-0800200c9a66";

		public const string ManageMsrCardLinks = "03bdf970-f660-11e0-be50-0800200c9a66";

		public const string FormsView = "0a709660-e95b-11de-8a39-0800200c9a66";
		public const string FormsEdit = "103de0c0-e95b-11de-8a39-0800200c9a66";

		public const string EditActionPermissions = "f0f5bfc0-cf89-11de-8a39-0800200c9a66";


		// Pos Permissions 

		public const string ItemSale = "cb2af180-a9a6-11e1-afa6-0800200c9a66";
		public const string PriceCheck = "cb2af181-a9a6-11e1-afa6-0800200c9a66";
		public const string VoidItem = "cb2af182-a9a6-11e1-afa6-0800200c9a66";
		public const string ItemComment = "cb2af183-a9a6-11e1-afa6-0800200c9a66";
		public const string PriceOverride = "cb2af184-a9a6-11e1-afa6-0800200c9a66";
		public const string ClearPriceOverride = "88A26FAB-AF3D-4F81-AE37-74ECD0518A1D";
		public const string SetQty = "cb2af185-a9a6-11e1-afa6-0800200c9a66";
		public const string ClearQty = "cb2af186-a9a6-11e1-afa6-0800200c9a66";
		public const string ItemSearch = "cb2af187-a9a6-11e1-afa6-0800200c9a66";
		public const string ReturnItem = "cb2af188-a9a6-11e1-afa6-0800200c9a66";
		public const string ReturnTransaction = "cb2af189-a9a6-11e1-afa6-0800200c9a66";
		public const string SetReasonCode = "35287470-9B5F-4C09-95D9-1CC64104CFFD";
		public const string ShowJournal = "cb2af18a-a9a6-11e1-afa6-0800200c9a66";
		public const string LoyaltyRequest = "cb2af18b-a9a6-11e1-afa6-0800200c9a66";
		public const string ClearSalesperson = "cb2af18c-a9a6-11e1-afa6-0800200c9a66";
		public const string InvoiceComment = "cb2af18d-a9a6-11e1-afa6-0800200c9a66";
		public const string ChangeUnitofMeasure = "cb2af18e-a9a6-11e1-afa6-0800200c9a66";
		public const string InfocodeOnRequest = "cb2af191-a9a6-11e1-afa6-0800200c9a66";
		public const string ChangeItemComments = "cb2af193-a9a6-11e1-afa6-0800200c9a66";
		public const string LoyaltyPointDiscount = "89161D7F-43A2-4D9B-9758-B26072BD65D5";

		public const string PayCash = "cb2af194-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCard = "cb2af195-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCustomerAccount = "cb2af196-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCurrency = "cb2af197-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCheck = "cb2af198-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCashQuick = "cb2af199-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCorporateCard = "cb2af19a-a9a6-11e1-afa6-0800200c9a66";
		public const string VoidPayment = "cb2af19b-a9a6-11e1-afa6-0800200c9a66";
		public const string PayCreditMemo = "cb2af19c-a9a6-11e1-afa6-0800200c9a66";
		public const string PayGiftcard = "cb2af19d-a9a6-11e1-afa6-0800200c9a66";
		public const string LineDiscountAmount = "cb2af19e-a9a6-11e1-afa6-0800200c9a66";
		public const string LineDiscountPercent = "cb2af19f-a9a6-11e1-afa6-0800200c9a66";
		public const string TotalDiscountAmount = "cb2af1a0-a9a6-11e1-afa6-0800200c9a66";
		public const string TotalDiscountPercent = "cb2af1a1-a9a6-11e1-afa6-0800200c9a66";
		public const string ClearDiscounts = "A263AC10-7D96-49FD-A761-94F35407C187";
		public const string ManuallyTriggerPeriodicDiscount = "7CC8499D-0AA8-497E-99BA-A8A208EF87EB";
		public const string VoidTransaction = "cb2b1892-a9a6-11e1-afa6-0800200c9a66";
		public const string TransactionComment = "cb2b1893-a9a6-11e1-afa6-0800200c9a66";
		public const string SalesPerson = "cb2b1894-a9a6-11e1-afa6-0800200c9a66";
		public const string SuspendTransaction = "cb2b1895-a9a6-11e1-afa6-0800200c9a66";
		public const string RecallTransaction = "cb2b1896-a9a6-11e1-afa6-0800200c9a66";
		public const string PharmacyPrescriptionCancel = "cb2b1897-a9a6-11e1-afa6-0800200c9a66";
		public const string PharmacyPrescriptions = "cb2b1898-a9a6-11e1-afa6-0800200c9a66";
		public const string IssueCreditMemo = "cb2b1899-a9a6-11e1-afa6-0800200c9a66";
		public const string IssueGiftCertificate = "cb2b189a-a9a6-11e1-afa6-0800200c9a66";
		public const string DisplayTotal = "cb2b189b-a9a6-11e1-afa6-0800200c9a66";
		public const string SalesOrder = "cb2b189c-a9a6-11e1-afa6-0800200c9a66";
		public const string SalesInvoice = "cb2b189d-a9a6-11e1-afa6-0800200c9a66";
		public const string IncomeAccount = "cb2b189e-a9a6-11e1-afa6-0800200c9a66";
		public const string ExpenseAccount = "cb2b189f-a9a6-11e1-afa6-0800200c9a66";
		public const string ReturnIncomeAccounts = "cb2b18a0-a9a6-11e1-afa6-0800200c9a66";
		public const string ReturnExpenseAccounts = "cb2b18a1-a9a6-11e1-afa6-0800200c9a66";
		public const string ManagePOSCustomerBlocking = "6F39CFF7-EC68-4BBD-826E-3020E23376C9";
		public const string CustomerSearch = "fbccb030-a9a1-11e1-afa6-0800200c9a66";
		public const string CustomerClear = "3fc739d0-a9a3-11e1-afa6-0800200c9a66";
		public const string CustomerTransactions = "aec82af0-a9a4-11e1-afa6-0800200c9a66";
		public const string CustomerTransactionsReport = "145d6ab0-a9a5-11e1-afa6-0800200c9a66";
		public const string LogOff = "801eb6a0-a9a5-11e1-afa6-0800200c9a66";
		public const string LockTerminal = "04d68b20-a9a6-11e1-afa6-0800200c9a66";
		public const string LogOffForce = "88632610-a9a6-11e1-afa6-0800200c9a66";
		public const string InventoryLookup = "eb1da780-a9a6-11e1-afa6-0800200c9a66";
		public const string InitializeReport = "5a022c70-a9a7-11e1-afa6-0800200c9a66";
		public const string PrintX = "a896e740-a9a7-11e1-afa6-0800200c9a66";
		public const string PrintZ = "ecfcf960-a9a7-11e1-afa6-0800200c9a66";
		public const string PrintItemSaleReport = "1BA39C5E-BFFB-41BF-AA25-5848F2685F23";
		public const string PrintFiscalInfoSlip = "68F21087-4A8B-4494-97B3-B7305CDDA9C7";
		public const string EditPOSLayout = "7c504220-a9a8-11e1-afa6-0800200c9a66";
		public const string MinimizePOSWindow = "dd590460-aa36-11e1-afa6-0800200c9a66";
		public const string BlankOperation = "31601850-aa37-11e1-afa6-0800200c9a66";
		public const string RunExternalCommand = "6bf47f9e-9d35-4aa0-91a8-d190d4236838";
		public const string ExecutePOSPlugin = "a58bdec5-86b1-495f-af96-8da24012b126";
		public const string OpenDrawer = "92ab7960-aa37-11e1-afa6-0800200c9a66";
		public const string EndOfDay = "f8c72730-aa37-11e1-afa6-0800200c9a66";
		public const string EndOfShift = "4e3f3040-aa38-11e1-afa6-0800200c9a66";
		public const string POSTenderDeclaration = "9e65aea0-aa38-11e1-afa6-0800200c9a66";
		public const string CustomerAccountDeposit = "f4572d70-aa38-11e1-afa6-0800200c9a66";
		public const string DeclareStartAmount = "4402af70-aa39-11e1-afa6-0800200c9a66";
		public const string FloatEntry = "8ba268c0-aa39-11e1-afa6-0800200c9a66";
		public const string TenderRemoval = "eba34870-aa39-11e1-afa6-0800200c9a66";
		public const string SafeDrop = "58de1500-aa3a-11e1-afa6-0800200c9a66";
		public const string BankDrop = "a3fdcd50-aa3a-11e1-afa6-0800200c9a66";
		public const string SafeDropReversal = "78bd4d90-aa3b-11e1-afa6-0800200c9a66";
		public const string BankDropReversal = "cfd7bc00-aa3b-11e1-afa6-0800200c9a66";
		public const string SplitBill = "0e6475d0-aa3c-11e1-afa6-0800200c9a66";
		public const string ExitHospitalityPOS = "7f68f490-aa3c-11e1-afa6-0800200c9a66";
		public const string PrintHospitalityMenuType = "e45dd5a0-aa3c-11e1-afa6-0800200c9a66";
		public const string SetHospitalityMenuType = "25f13fc0-aa3d-11e1-afa6-0800200c9a66";
		public const string ChangeHospitalityMenuType = "5cc33710-aa3d-11e1-afa6-0800200c9a66";
		public const string TaxExemptTransaction = "9ba84a10-aa3d-11e1-afa6-0800200c9a66";
		public const string ClearTransactionTaxExemption = "de11a1d0-aa3d-11e1-afa6-0800200c9a66";
		public const string InfocodeTaxGroupChange = "08d5f740-aa3e-11e1-afa6-0800200c9a66";
		public const string CustomerAdd = "4692F4C7-8491-4943-A092-335904F96D60";
		public const string BumpOrder = "33A883D8-1557-4D22-939D-4B92A9B43BBB";
		public const string PayLoyalty = "7911D1D2-3567-4E15-A35C-B7917067D4DE";
		public const string AddCustomerToLoyaltyCard = "851FDAD8-281C-4B30-B3FD-EFE30B3E2FAB";
		public const string ViewInventoryTransferRequests = "FF61FC67-7A75-4A5F-AE31-06655E7C628F";
		public const string EditInventoryTransferRequests = "63A12986-450F-4583-B886-38BEFBA64019";
		public const string ViewInventoryTransferOrders = "73747D3D-4AB7-4B5C-B77C-C01E5B9CDDA4";
		public const string EditInventoryTransfersOrders = "99ADF603-3981-46AD-899F-343A3C2F201E";
		public const string AutoSetQuantityOnTransferOrder = "C8B4DA03-399C-41C2-BE32-CFFB8968D71E";
		public const string SchemesEdit = "92df57e5-a6ab-4b3e-ad44-022911465763";
		public const string SchemesView = "a1a62675-b17f-4283-89dc-7c2f84689983";
		public const string CardsEdit = "87034c77-0ada-4c76-9102-d3e818f0e64f";
		public const string LoyaltyTransactions = "31862587-7C64-433A-8E76-E3E022A3BA51";
		public const string CardsView = "b58125d3-7eb1-4de4-9cd0-06346603ee12";
		public const string CustomerLedgerEntriesView = "B4B7457A-09F3-41EF-B469-8B96F2A36C19";
		public const string CustomerLedgerEntriesEdit = "9FF97831-DFA2-4792-A0EE-FAC964A64C90";
		public const string GetGiftCardBalance = "54142856-C298-4DB0-A9DF-F02995EA24AC";
		public const string StartOfDay = "88562A82-AD6D-4934-B189-CE6D0CA3EA4C";

		public const string LabelTemplatesView = "B623624E-C77D-4649-83C7-57206423FAA1";
		public const string LabelTemplatesEdit = "DD995493-97DF-4203-8A73-5E852409220F";
		public const string LabelTemplatesPrint = "499B4FFB-9415-4514-816C-AB3D46C8997C";

		public const string ManageCustomerOrderSettings = "AB8469A3-B82B-412D-9095-4087CF2C66F3";
		public const string ManageCustomerOrders = "C679394A-A0E8-4469-B422-484BB8055A29";

		//Reports
		public const string ManageReports = "f3791dd0-9842-11e1-a8b0-0800200c9a66";
		public const string ViewReports = "41dc8eb3-ab1c-4f09-802a-48b45ca1c02c";
		public const string ViewFinancialReports = "478302f2-bb44-491f-b28d-f9149e6e62b8";
		public const string ViewInventoryReports = "39be03a0-7fe4-11e0-b278-0800200c9a66";
		public const string ViewSalesReports = "6c7c97b7-28a7-4777-944c-c42e3f69c322";
		public const string ViewItemReports = "e2fa3f6e-eac2-4ae0-9f1e-cdfa5b92a1c5";
		public const string ViewCustomerReports = "c13a8edf-89db-4c48-be83-22b5e14d6093";
		public const string ViewUsersReports = "24acec23-06b6-4a58-9fcb-75d99ce11693";
		public const string ViewHospitalityReports = "7e3f23f4-26a3-48e5-846a-034421c13c77";
		public const string ViewSetupReports = "a8396bcc-3db3-466b-ac7f-618ed4828572";


		public const string ManagePosHardwareProfile = "AABAD5CD-2726-4084-8759-DF9C5E6C71E2";
		public const string BackupDatabase = "D5917F72-A9DB-450B-AE6B-771F8985A361";
		public const string ReprintReceipt = "040ABAC9-7E46-4B74-BD57-1F7BEFEF12E3";
		public const string SendEmailReceipt = "";

		public const string ViewTerminalOperationsAudit = "5c8f1749-a107-43b3-9d02-3fa7d910da72";

		public const string ExitPOSApplication = "5B124A59-1EC8-4D5A-BB97-B2F47EA0C668";
		public const string RestartComputer = "C9E23EF2-9FF0-4232-96B6-657302870EFD";
		public const string ShutdownComputer = "2EE61F04-C62B-4B30-8680-6636D655800D";

		public const string ActivateTrainingMode = "0C5E5E9D-E0AD-41B8-86D1-24B439E4CE63";

		//Customer orders
		public const string CustomerOrders = "5CBB9D93-64B0-4842-B300-39BB17B0EBFB";
		public const string Quotes = "5ECA0247-BD40-46BB-AB07-950851B5CB93";
		public const string CustomerOrdersManage = "6C4AB8C9-CEC0-42EE-A320-4097710FCAFF";
		public const string OverrideMinDeposit = "CDA8A454-DADB-4B94-BB4C-097CB21241CD";

		public const string KeepTime = "8F046DDF-842A-4A48-A2A1-388F72983122";

		//Omni 
		/// <summary>
		/// A user having this permission will have full access to all functionalities in the mobile POS.
		/// </summary>
		public const string ManageMobilePOS = "BF462D83-B316-460C-9215-F5115F3D5141";

	}
}
