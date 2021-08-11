using System;
using System.Data;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.Enums;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;

namespace LSRetail.SiteService.SiteServiceInterface
{
	public class SiteServiceConstants
	{
		public const string EndPointName = "LSOneSiteService";
	}

	[ServiceContract(SessionMode = SessionMode.Required)]
	public partial interface ISiteService
	{
		#region Customer Orders

		//All code moved to ISiteService.CustomerOrders.cs

		#endregion

		#region Reserve stock

		//All code moved to ISiteService.Inventory.InventoryAdjustment.cs

		#endregion

		#region Connection link verification and configuration

		[OperationContract]
		ConnectionEnum TestConnection(LogonInfo logonInfo);

		/// <summary>
		/// Validates an administrative password by returning an encrypted UNIX timestamp.
		/// </summary>
		/// <param name="administrativePassword"></param>
		/// <returns></returns>
		[OperationContract]
		string AdministrativeLogin(string administrativePassword);

		/// <summary>
		/// Returns the Site Service configurations from config file.
		/// </summary>
		/// <param name="administrativePassword">Authorization password set at install time for retrieving the settings.</param>
		/// <returns></returns>
		[OperationContract]
		Dictionary<string, string> LoadConfiguration(string administrativePassword);

		/// <summary>
		/// Updates Site Service configuratiosn from config file.
		/// </summary>
		/// <param name="administrativePassword">Authorization password set at install time for saving the passed settings.</param>
		/// <param name="fileConfigurations">List of settings to be saved in Site Service config file.</param>
		[OperationContract]
		void SendConfiguration(string administrativePassword, Dictionary<string, string> fileConfigurations);
		
		#endregion

		#region Hospitality

		[OperationContract]
		List<TableInfo> LoadHospitalityTableState(DiningTableLayout tableLayout, LogonInfo logonInfo);
		[OperationContract]
        TableInfo SaveHospitalityTableState(TableInfo table, LogonInfo logonInfo);
		[OperationContract]
		void SaveUnlockedTransaction(Guid transactionID, LogonInfo logonInfo);
		[OperationContract]
		bool ExistsUnlockedTransaction(Guid transactionID, LogonInfo logonInfo);

		[OperationContract]
		TableInfo LoadSpecificTableState(TableInfo table, LogonInfo logonInfo);

		[OperationContract]
		void ClearTerminalLocks(string terminalID, LogonInfo logonInfo);

		#endregion
		
		#region Suspension of transactions

		[OperationContract]
		SuspendedTransaction GetSuspendedTransaction(LogonInfo logonInfo, RecordIdentifier suspendedTransactionID);

		[OperationContract]
		List<SuspendedTransaction> GetAllSuspendedTransactions(LogonInfo logonInfo);

		[OperationContract]
		RecordIdentifier SuspendTransaction(
			RecordIdentifier suspendedTransactionId,
			LogonInfo logonInfo, 
			RecordIdentifier transactionTypeID, 
			string xmlTransaction,
			decimal balance,
			decimal balanceWithTax,
			List<SuspendedTransactionAnswer> answers);
		
		[OperationContract]
		List<SuspendedTransaction> GetSuspendedTransactionList(
			LogonInfo logonInfo,
			RecordIdentifier suspensionTransactionTypeID, 
			RecordIdentifier storeID,
			RecordIdentifier terminalId,
			Date dateFrom,
			Date dateTo,
			SuspendedTransaction.SortEnum sortEnum,
			bool sortBackwards);

		[OperationContract]
		List<SuspendedTransaction> GetSuspendedTransactionListForStore(
			LogonInfo logonInfo,
			RecordIdentifier suspensionTransactionTypeID,
			RecordIdentifier storeID);

		[OperationContract]
		List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswers(RecordIdentifier transactionID, LogonInfo logonInfo);

		[OperationContract]
		string RecallSuspendedTransaction(RecordIdentifier transactionID, LogonInfo logonInfo);

		[OperationContract]
		bool DeleteSuspendedTransaction(RecordIdentifier transactionID, LogonInfo logonInfo);

		[OperationContract]
		int GetSuspendedTransCount(RecordIdentifier storeId, RecordIdentifier terminalId, RecordIdentifier suspensionTransactionTypeID, RetrieveSuspendedTransactions whatToRetreive, LogonInfo logonInfo);

		[OperationContract]
		List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswersByType(RecordIdentifier suspensionTypeID, LogonInfo logonInfo);

		#endregion

		#region Inventory operations

		//Note -> most of the code has been moved to ISiteService.Inventory.PurchaseOrders.cs

		#endregion

		#region Retail items functions

		//Code moved to ISiteService.RetailItem.cs

		#endregion

		#region Customer functions

		/// <summary>
		/// Checks if a customer with the given ID exists
		/// </summary>
		/// <param name="customerID">The customer ID to check for</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>True if the customer exists</returns>
		[OperationContract]
		bool CustomerExists(RecordIdentifier customerID, LogonInfo logonInfo);

		void ValidateCustomerStatus(ref CustomerStatusValidationEnum retVal, ref string comment, string customerId, string amount, string currencyCode, LogonInfo logonInfo);

		[OperationContract]
		List<CustomerListItem> GetCustomers(string searchString, bool beginsWith, CustomerSorting sortOrder, bool sortBackwards, LogonInfo logonInfo);

		[OperationContract]
		Customer GetCustomer(RecordIdentifier customerID, LogonInfo logonInfo);

		/// <summary>
		/// Save customer
		/// </summary>
		/// <param name="customer">The customer to be saved</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Returns the customer being saved. This is useful when another the integration service provides more data than the one used to create the customer</returns>
		[OperationContract]
		Customer SaveCustomer(Customer customer, LogonInfo logonInfo);

		[OperationContract]
		void DeleteCustomer(Customer customer, LogonInfo logonInfo);

		/// <summary>
		/// Set the credit limit of a customer
		/// </summary>
		/// <param name="customerID">The customer ID for which to update the credit limit</param>
		/// <param name="creditLimit">The credit limit to be set on the customer</param>
		/// <param name="logonInfo">Login credentials</param>
		[OperationContract]
		void SetCustomerCreditLimit(RecordIdentifier customerID, decimal creditLimit, LogonInfo logonInfo);

		[OperationContract]
		void CustomersDiscountedPurchasesStatus(
			string customerID, LogonInfo logonInfo,
			out decimal maxDiscountedPurchases,
			out decimal currentPeriodDiscountedPurchases);

		[OperationContract]
		PurchaseOrderLinesDeleteResult DeletePurchaseOrder(
			LogonInfo logonInfo,
			RecordIdentifier purchaseOrderLineID);


        /// <summary>
        /// Get all information for a customer to be displayed in the customer panel of the POS
        /// </summary>
		/// <param name="logonInfo">Login credentials</param>
        /// <param name="customerID">ID of the customer</param>
        /// <returns></returns>
		[OperationContract]
        CustomerPanelInformation GetCustomerPanelInformation(LogonInfo logonInfo, RecordIdentifier customerID);

        #endregion

        #region Staff functions

        [OperationContract]
		void StaffLogOn(ref bool retVal, ref string comment, string staffId, string storeId, string terminalId, string password, LogonInfo logonInfo);
		[OperationContract]
		void StaffLogOff(ref bool retVal, string staffId, string storeId, string terminalId, LogonInfo logonInfo);

		/// <summary>
		/// Changes password for given user
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		/// <param name="newPasswordHash">The hash of the new password</param>
		/// <param name="lastChangeTime">Sets the last change time for the password</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="needPasswordChange">Sets wether the user needs to change  password</param>
		/// <param name="expiresDate">Sets the expire date for the password</param>
		[OperationContract]
		bool ChangePasswordForUser(RecordIdentifier userID, string newPasswordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, LogonInfo logonInfo);

		/// <summary>
		/// Indicates wether the given user needs to change his password
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns></returns>
		[OperationContract]
		bool UserNeedsToChangePassword(RecordIdentifier userID, LogonInfo logonInfo);

		/// <summary>
		/// Locks the user
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		/// <param name="logonInfo">Login credentials</param>
		[OperationContract]
		void LockUser(RecordIdentifier userID, LogonInfo logonInfo);

		/// <summary>
		/// Gets the information about the current password satus for the given user
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		/// <param name="passwordHash">The hash of the current password</param>
		/// <param name="expiresDate">The date of the password expiration</param>
		/// <param name="lastChangeTime">The date when the password was last changed</param>
		/// <param name="logonInfo">Login credentials</param>
		[OperationContract]
		void GetUserPasswordChangeInfo(RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime, LogonInfo logonInfo);

		#endregion

		#region Loyalty

		[OperationContract]
		LoyaltySchemes GetLoyaltyScheme(RecordIdentifier schemeID, LogonInfo logonInfo);

		[OperationContract]
		LoyaltySchemes SaveLoyaltyScheme(LoyaltySchemes scheme, RecordIdentifier copyRulesFrom, LogonInfo logonInfo);

		[OperationContract]
		void SaveLoyaltySchemeRule(LoyaltyPoints schemeRule, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyMSRCardTrans GetLoyaltyMSRCardTrans(RecordIdentifier loyMsrCardTransID, LogonInfo logonInfo);

		[OperationContract]
		List<LoyaltyMSRCardTrans> GetLoyaltyMSRCardTransList(
			string StoreFilter,
			string TerminalFilter,
			string MSRCardFilter,
			string SchemeFilter,
			int TypeFilter,
			int OpenFilter,
			int EntryTypeFilter,
			string CustomerFilter,
			string receiptID,
			Date dateFrom,
			Date dateTo,
			Date expiredateFrom,
			Date expiredateTo,
			int rowFrom, int rowTo, LogonInfo logonInfo, bool backwards = false);

		[OperationContract]
		void SaveLoyaltyMSRCardTrans(LoyaltyMSRCardTrans loyaltyTrans, LogonInfo logonInfo);

		[OperationContract]
		int GetLoyaltyMSRCardTransCount(RecordIdentifier cardNumber, LogonInfo logonInfo);

		[OperationContract]
		int GetCustomerLoyaltyMSRCardTransCount(RecordIdentifier cardNumber, LogonInfo logonInfo);
		 
		[OperationContract]
		LoyaltyMSRCard GetLoyaltyMSRCard(RecordIdentifier cardNumber, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyMSRCard.TenderTypeEnum? GetLoyaltyCardType(RecordIdentifier cardNumber, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyCustomer.ErrorCodes UpdateRemainingPoints(RecordIdentifier cardNumber, RecordIdentifier customerID, decimal loyaltyPoints, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyCustomer.ErrorCodes? SetExpirePoints(RecordIdentifier cardNumber, RecordIdentifier customerID, DateTime transDate, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyPointStatus GetLoyaltyPointsStatus(RecordIdentifier customerId, LoyaltyPointStatus pointStatus, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyCustomer GetLoyaltyCustomer(RecordIdentifier customerId, LogonInfo logonInfo);

		[OperationContract]
		RecordIdentifier SaveLoyaltyMSRCard(LoyaltyMSRCard loyaltyCard, LogonInfo logonInfo);

		[OperationContract]
		void DeleteLoyaltyMSRCard(RecordIdentifier cardID, LogonInfo logonInfo);

		[OperationContract]
		decimal GetMaxLoyaltyMSRCardTransLnNum(string cardNumber, LogonInfo logonInfo);

		[OperationContract]
		List<LoyaltySchemes> GetLoyaltySchemes( LogonInfo logonInfo);

		[OperationContract]
		List<LoyaltyPoints> GetLoyaltySchemeRules(RecordIdentifier loyaltySchemeId, LogonInfo logonInfo);

		[OperationContract]
		LoyaltyPoints GetLoyaltySchemeRule(RecordIdentifier loyaltySchemeRuleId, LogonInfo logonInfo);

		[OperationContract]
		void DeleteLoyaltyScheme(RecordIdentifier loyaltySchemeId, LogonInfo logonInfo);

		[OperationContract]
		void DeleteLoyaltySchemeRule(RecordIdentifier loyaltySchemeRuleId, LogonInfo logonInfo);

		[OperationContract]
		List<LoyaltyMSRCard> GetCustomerMSRCards(
			List<DataEntity> customers, 
			List<DataEntity> schemas,
			RecordIdentifier cardID, 
			bool? hasCustomer, 
			int tenderType,
			double? status, 
			int statusInequality, 
			int fromRow, 
			int toRow,
			LoyaltyMSRCardSorting sortBy, 
			bool backwards, 
			LogonInfo logonInfo);

		[OperationContract]
		List<LoyaltyMSRCardTrans> GetLoyaltyTrans(
			string StoreFilter,
			string TerminalFilter,
			string MSRCardFilter,
			string SchemeFilter,
			int TypeFilter,
			int OpenFilter,
			int EntryTypeFilter,
			string CustomerFilter,
			string receiptID,
			Date dateFrom,
			Date dateTo,
			Date expiredateFrom,
			Date expiredateTo,
			int rowFrom,
			int rowTo, 
			LogonInfo logonInfo,
			bool backwards = false);

		[OperationContract]
		LoyaltyPoints GetPointsExchangeRate(RecordIdentifier schemeID, LogonInfo logonInfo);

		[OperationContract]
		void UpdateIssuedLoyaltyPointsForCustomer(RecordIdentifier loyalityCardId, RecordIdentifier customerId, LogonInfo logonInfo);

		[OperationContract]
		void UpdateCouponCustomerLink(RecordIdentifier couponID, RecordIdentifier customerID, LogonInfo logonInfo);

		[OperationContract]
		bool LoyaltyCardExistsForLoyaltyScheme( RecordIdentifier loyaltySchemeID, LogonInfo logonInfo);
		#endregion

		#region Gift Certificates



		// StoreController with the scenario HO and multiple stores management functions.
		// ----------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Searches for gift cards
		/// </summary>
		/// <param name="logonInfo"></param>
        /// <param name="filter">Search filter</param>
		/// <param name="itemCount">Number of items found</param>
		/// <returns>List of found gift cards</returns>
		[OperationContract]
		List<GiftCard> SearchGiftCards(LogonInfo logonInfo, GiftCardFilter filter, out int itemCount);

		/// <summary>
		/// Fetches a gift card by a given ID
		/// </summary>
		/// <param name="giftCardID">The id of the gift card to be fetched</param>
		/// <param name="logonInfo"></param>
		/// <returns>The requested gift card or null if card with the given ID was not found</returns>
		[OperationContract]
		GiftCard GetGiftCard(RecordIdentifier giftCardID, LogonInfo logonInfo);

		/// <summary>
		/// Fetches gift card lines log for a gift card with a given ID
		/// </summary>
		/// <param name="giftCardID">ID of the gift card</param>
		/// <param name="logonInfo"></param>
		/// <returns>Lines for the requested gift card or empty list if none were found</returns>
		[OperationContract]
		List<GiftCardLine> GetGiftCardLines(RecordIdentifier giftCardID, LogonInfo logonInfo);

		/// <summary>
		/// Deletes a gift card by a given ID
		/// </summary>
		/// <param name="giftCardID">The ID of the gift card to be deleted</param>
		/// <param name="logonInfo">Login credentials</param>
		[OperationContract]
		void DeleteGiftCertificate(RecordIdentifier giftCardID, LogonInfo logonInfo);

		// General functions
		// ----------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Adds a amount to a gift card with a given ID
		/// </summary>
		/// <param name="giftCardID">The gift card to add to</param>
		/// <param name="amount">The amount to add to the gift card</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>New ballance on the gift card after the amount has been added to it</returns>
		[OperationContract]
		decimal AddToGiftCertificate(RecordIdentifier giftCardID, decimal amount, LogonInfo logonInfo);

		/// <summary>
		/// Activates a giftcard with a given ID
		/// </summary>
		/// <param name="id">ID of the gift card to be activated</param>
		/// <param name="transactionID">ID of the transaction</param>
		/// /// <param name="receiptID">The receipt id of the transaction</param>
		/// <param name="logonInfo">Logon credidentials</param>
		/// <param name="comment">A comment that is passed back if there is exception</param>
		/// <returns>True if the gift card was activated, false if it was allready activated or did not exist</returns>
		[OperationContract]
		bool ActivateGiftCertificate(RecordIdentifier id, RecordIdentifier transactionID, RecordIdentifier receiptID, LogonInfo logonInfo, ref string comment);


		[OperationContract]
		bool MarkGiftCertificateIssued(RecordIdentifier id, LogonInfo logonInfo);


		/// <summary>
		/// Deactivates a giftcard with a given ID
		/// </summary>
		/// <param name="id">ID of the gift card to be deactivated</param>
		/// <param name="transactionID">ID of the transaction</param>
		/// <param name="logonInfo">Logon credentials</param>
		/// <param name="comment">A comment that is passed back if there is exception</param>
		/// <returns>True if the gift card was deactivated, false if it was allready deactivated or did not exist</returns>
		[OperationContract]
		bool DeactivateGiftCertificate(RecordIdentifier id, RecordIdentifier transactionID,LogonInfo logonInfo, ref string comment);


		/// <summary>
		/// Adds a new gift card
		/// </summary>
		/// <param name="logonInfo">Logon info block that identifies the user and the location of the caller</param>
		/// <param name="giftCard">The gift card to add</param>
		/// <param name="prefix">Prefix for the ID added for barcode usage</param>
		/// <param name="numberSequenceLowest">The point where you want your number sequence start</param>
		/// <returns>The ID of the gift card if a automatic sequence was used.</returns>
		[OperationContract]
		RecordIdentifier AddNewGiftCertificate(LogonInfo logonInfo, GiftCard giftCard, string prefix, int? numberSequenceLowest = null);

		/// <summary>
		/// Validates gift certificate for a specific amount without taking anything from it
		/// </summary>
		/// <param name="amount">The amount to check for. Upon return then this by byref value will contain the actual amount on the giftcard</param>
		/// <param name="giftCardID">The ID of the gift card to check</param>
		/// <param name="logonInfo">Logon credentials</param>
		/// <returns>A enum that tells if the gift card was valid for at least the requested amount</returns>
		[OperationContract]
		GiftCardValidationEnum ValidateGiftCertificate(ref decimal amount, RecordIdentifier giftCardID, LogonInfo logonInfo);

		/// <summary>
		/// Uses a part of a gift card or all of it.
		/// </summary>
		/// <param name="amount">The amount to subtract from the gift card. Upon return then this byref value will contain remaining ballance on the gift card</param>
		/// <param name="giftCardID">The ID of the gift card to subtract from</param>
		/// <param name="transactionId">ID of the transaction</param>
		/// <param name="receiptId">ID of the receipt</param>
		/// <param name="logonInfo">Logon credentials</param>
		/// <returns>A enum that tells if the operation was successful. If it was successful then the requested amount was subtracted from the gift card</returns>
		[OperationContract]
		GiftCardValidationEnum UseGiftCertificate(ref decimal amount, RecordIdentifier giftCardID, RecordIdentifier transactionId, RecordIdentifier receiptId, LogonInfo logonInfo);

		/// <summary>
		/// Updates a gift card payment with the receipt ID which is created after the gift card payment
		/// </summary>
		/// <param name="giftCardID">The ID of the gift card to update</param>
		/// <param name="transactionID">The current transaction ID</param>
		/// <param name="receiptID">The current receipt ID</param>
		/// <param name="storeID">The current store ID</param>
		/// <param name="terminalID">The current terminal ID</param>
		/// <param name="logonInfo">Logon credentials</param>
		/// <returns></returns>
		[OperationContract]
		GiftCardValidationEnum UpdateGiftCardPaymentReceipt(RecordIdentifier giftCardID, RecordIdentifier transactionID,
															RecordIdentifier receiptID, RecordIdentifier storeID,
															RecordIdentifier terminalID, LogonInfo logonInfo);

		#endregion

		#region Credit vouchers

		/// <summary>
		/// Searches for credit vouchers that start with a given ID, and or can search by empty state.
		/// </summary>
		/// <param name="logonInfo">Logon credentials</param>
        /// <param name="filter">Search filter</param>
		/// <param name="itemCount">Out parameter. Returns the total number of returned items</param>
		/// <returns>List of found gift cards</returns>
		[OperationContract]
		List<CreditVoucher> SearchCreditVouchers(LogonInfo logonInfo, CreditVoucherFilter filter, out int itemCount);

		/// <summary>
		/// Fetches a credit voucher by a given ID
		/// </summary>
		/// <param name="creditvoucherID">The id of the credit voucher with the given ID was not found</param>
		/// <param name="logonInfo"></param>
		[OperationContract]
		CreditVoucher GetCreditVoucher(RecordIdentifier creditvoucherID, LogonInfo logonInfo);

		/// <summary>
		/// Fetches credit voucher lines log for a credit voucher with a given ID
		/// </summary>
		/// <param name="creditvoucherID">ID of the credit voucher</param>
		/// <param name="logonInfo"></param>
		/// <returns>Lines for the requested credit voucher or empty list if none were found</returns>
		[OperationContract]
		List<CreditVoucherLine> GetCreditVoucherLines(RecordIdentifier creditvoucherID, LogonInfo logonInfo);

		/// <summary>
		/// Deletes a credit voucher by a given ID
		/// </summary>
		/// <param name="creditvoucherID">The ID of the gift card to be deleted</param>
		/// <param name="logonInfo">Login credentials</param>
		[OperationContract]
		void DeleteCreditVoucher(RecordIdentifier creditvoucherID, LogonInfo logonInfo);

		/// <summary>
		/// Issues a credit voucher
		/// </summary>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="voucher">The voucher to issue</param>
		/// <param name="transactionId">Th ID of the retail transaction</param>
		/// <param name="receiptId">The ID of the receipt</param>
		/// <returns>ID of the newly created credit voucher</returns>
		[OperationContract]
		RecordIdentifier IssueCreditVoucher(LogonInfo logonInfo, CreditVoucher voucher, RecordIdentifier transactionId, RecordIdentifier receiptId);

		/// <summary>
		/// Validates credit voucher for a specific amount without taking anything from it
		/// </summary>
		/// <param name="amount">The amount to check for. Upon return then this by byref value will contain the actual amount on the credit voucher</param>
		/// <param name="creditVoucherID">The ID of the credit voucher to check</param>
		/// <param name="logonInfo">Logon credentials</param>
		/// <returns>A enum that tells if the credit voucher was valid for at least the requested amount</returns>
		[OperationContract]
		CreditVoucherValidationEnum ValidateCreditVoucher(ref decimal amount, RecordIdentifier creditVoucherID, LogonInfo logonInfo);

		/// <summary>
		/// Uses a part of a credit voucher or all of it.
		/// </summary>
		/// <param name="amount">The amount to subtract from the credit voucher. Upon return then this byref value will contain remaining ballance on the credit voucher</param>
		/// <param name="creditVoucherID">The ID of the credit voucher to subtract from</param>
		/// <param name="transactionId">ID of the transaction</param>
		/// <param name="receiptId">ID of the receipt</param>
		/// <param name="logonInfo">Logon credentials</param>
		/// <returns>A enum that tells if the operation was successful. If it was successful then the requested amount was subtracted from the credit voucher</returns>
		[OperationContract]
		CreditVoucherValidationEnum UseCreditVoucher(ref decimal amount, RecordIdentifier creditVoucherID, RecordIdentifier transactionId, RecordIdentifier receiptId, LogonInfo logonInfo);

		/// <summary>
		/// Adds a amount to a credit voucher with a given ID
		/// </summary>
		/// <param name="creditVoucherID">The credit voucher to add to</param>
		/// <param name="amount">The amount to add to the credit voucher</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>New balance on the credit voucher after the amount has been added to it</returns>
		[OperationContract]
		decimal AddToCreditVoucher(RecordIdentifier creditVoucherID, decimal amount, LogonInfo logonInfo);

		//TODO Old functions to be evaluated and or phased out
		//void IssueGiftCertificate(ref bool retVal, ref string comment, ref string id, string storeId, string terminalId, string staffId, string transactionId, string receiptId, string lineNum, decimal amount, DateTime date, LogonInfo logonInfo);
		//void UpdateGiftCertificate(ref bool retVal, ref string comment, string id, string storeId, string terminalId, string staffId, string transactionId, string receiptId, string lineNum, decimal amount, DateTime date, LogonInfo logonInfo);
		//void VoidGiftCertificatePayment(ref bool retVal, ref string comment, string id, string storeId, string terminalId, LogonInfo logonInfo);
	   
		// Use gift card
		//void VoidGiftCertificate(ref bool retVal, ref string comment, string id, LogonInfo logonInfo);


		//void IssueCreditMemo(ref bool retVal, ref string comment, ref string id, string storeId, string terminalId, string staffId, string transactionId, string receiptId, string lineNum, decimal amount, DateTime date, LogonInfo logonInfo);
		//void ValidateCreditMemo(ref bool retVal, ref string comment, ref decimal amount, string id, string storeId, string terminalId, LogonInfo logonInfo);
		//void UpdateCreditMemo(ref bool retVal, ref string comment, string id, string storeId, string terminalId, string staffId, string transactionId, string receiptId, string lineNum, decimal amount, DateTime date, LogonInfo logonInfo);
		//void VoidCreditMemoPayment(ref bool retVal, ref string comment, string id, string storeId, string terminalId, LogonInfo logonInfo);

		#endregion

		#region Sales Orders
		//Moved to ISiteService.SalesOrders.cs and ISiteService.SalesInvoices.cs
		#endregion

		#region Returns

		void GetTransaction(ref bool retVal, ref string comment, ref bool uniqueReceiptId, ref DataTable transHeader, ref DataTable transItems, ref DataTable transPayments, string receiptId, string storeId, string terminalId, LogonInfo logonInfo);
		//void MarkItemsReturned(ref bool retVal, ref string comment, LinkedList<SaleLineItem> returnedItems, LogonInfo logonInfo);

		#endregion

		#region Customer ledger

		[OperationContract]
		List<CustomerLedgerEntries> GetCustomerLedgerEntriesList(
			RecordIdentifier customerId, 
			out int totalRecords,
			LogonInfo logonInfo,
			CustomerLedgerFilter filter);

		[OperationContract]
		decimal GetCustomerBalance(RecordIdentifier customerId, LogonInfo logonInfo);

		[OperationContract]
		decimal GetCustomerTotalSales(RecordIdentifier customerId, LogonInfo logonInfo);

		[OperationContract]
		void ValidateCustomerStatus(ref int valid, ref string comment, RecordIdentifier customerId, LogonInfo logonInfo);

		[OperationContract]
		void UpdateCustomerLedgerAtEOD(ref int valid, ref string comment, RecordIdentifier statementID, LogonInfo logonInfo);

		[OperationContract]
		void SaveCustomerLedgerEntries(CustomerLedgerEntries custLedgerEntries, LogonInfo logonInfo);

		[OperationContract]
		void DeleteCustomerLedgerEntry(RecordIdentifier ledgerEntryNo, LogonInfo logonInfo);

		[OperationContract]
		bool UpdateRemainingAmount(RecordIdentifier customerId, LogonInfo logonInfo);

		[OperationContract]
		XElement GetCustomerTransactionXML(LogonInfo logonInfo, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier storeCurrency, bool taxIncludedInPrice);
		#endregion

		#region Email functionality
		[OperationContract]
		bool IsEMailSetupForStore(RecordIdentifier storeID, LogonInfo logonInfo);

		[OperationContract]
		EMailSetting GetEMailSetupForStore(RecordIdentifier storeID, LogonInfo logonInfo);

		[OperationContract]
		void SaveEMailSetupForStore(EMailSetting setting, LogonInfo logonInfo);

		[OperationContract]
		void QueueEMailEntry(EMailQueueEntry entry, List<EMailQueueAttachment> attachments, LogonInfo logonInfo);

		[OperationContract]
		void SendQueuedEMailEntries(int maximumEntries, int maximumAttempts);

		[OperationContract]
		int GetEMailCount(bool unsentOnly, LogonInfo logonInfo);

		[OperationContract]
		List<EMailQueueEntry> GetEMails(bool unsentOnly, int index, int maxEntries, EMailSortEnum sort, LogonInfo logonInfo);

		[OperationContract]
		EMailQueueEntry GetEMail(int ID, LogonInfo logonInfo);

		[OperationContract]
		void TruncateEMailQueue(DateTime createdBefore, LogonInfo logonInfo);
		#endregion

		#region Central Returns

		[OperationContract]
		List<ReceiptListItem> GetTransactionListForReceiptID(LogonInfo logonInfo, RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID);
		[OperationContract]
		System.Xml.Linq.XElement GetTransactionXML(LogonInfo logonInfo, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier storeCurrency, bool taxIncludedInPrice);

		[OperationContract]
		bool MarkItemsAsReturned(LogonInfo logonInfo, List<ReturnItemInfo> returnedItems);

		#endregion

		#region Tax Refund

		[OperationContract]
		void SaveTaxRefund(TaxRefund refund, LogonInfo logonInfo);

		[OperationContract]
		TaxRefund GetTaxRefund(RecordIdentifier id, LogonInfo logonInfo);

		#endregion

		#region Cloud functionality

		[OperationContract]
		Guid RegisterClient(string userName, byte[] hash, string dbname, Guid clientID, string passCode, long tick);

		[OperationContract]
		void SetHardwareProfile(RecordIdentifier terminalID, RecordIdentifier storeID, HardwareProfile profile, LogonInfo logonInfo);

		[OperationContract]
		ActivationResultEnum MarkAsActivated(RecordIdentifier terminalID, RecordIdentifier storeID, LogonInfo logonInfo);

		[OperationContract]
		void SetEFTForTerminal(RecordIdentifier terminalID, RecordIdentifier storeID, string ipAddress,
			string eftStoreID, string eftTerminalID, string customField1, string customField2, LogonInfo logonInfo);

		[OperationContract]
		DateTime GetServerUTCDate();

		[OperationContract]
		Guid CreateDatabase(string databaseName, long ticks, byte[] authenticationHash, 
			string databaseUserPassword, string databaseUserName, string cloudPassword);

		[OperationContract]
		bool CheckDatabaseAvailability(string databaseName);

		[OperationContract]
		bool VerifyLogin(string userName, string cloudPassword, Guid userID);

		[OperationContract]
		List<ScriptInfo> GetDemoDataTypes();

		[OperationContract]
		Guid RunDemoDataUnsecureRemoveWhenNotNeeded(ScriptInfo demoDataType, long ticks, byte[] authenticationHash, string databaseName, Guid userGuid);

		[OperationContract]
		Guid RunDemoData(ScriptInfo demoDataType, LogonInfo logonInfo);

		[OperationContract]
		bool IsTaskActive(Guid taskGuid);

		#endregion

		[OperationContract]
		void NotifyPlugin(MessageEventArgs e);
	}
}
