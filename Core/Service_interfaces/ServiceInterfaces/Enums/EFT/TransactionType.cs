using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.Enums.EFT
{
	/// <summary>
	/// 
	/// </summary>
	public enum TransactionType
	{
		/// <summary>
		/// 1
		/// </summary>
		NormalSale = 1,
		/// <summary>
		/// 2
		/// </summary>
		VoidTransaction = 2,
		/// <summary>
		/// 3
		/// </summary>
		Refund = 3,
		/// <summary>
		/// 4
		/// </summary>
		Mailorder = 4,
		/// <summary>
		/// 5
		/// </summary>
		OfflineTransaction = 5,
		/// <summary>
		/// 6
		/// </summary>
		PreAuth = 6,
		/// <summary>
		/// 7
		/// </summary>
		ManualCallForAuthorisation = 7,
		/// <summary>
		/// 8
		/// </summary>
		SerialPayment = 8,
		/// <summary>
		/// 9
		/// </summary>
		ConfirmationForVendingMachines = 9,
		/// <summary>
		/// 10
		/// </summary>
		ReservedForPOINT = 10,
		/// <summary>
		/// 11
		/// </summary>
		SerialPaymentContract = 11,
		/// <summary>
		/// 19
		/// </summary>
		ValidateCard = 19,
		/// <summary>
		/// 99
		/// </summary>
		AuthorisationConfirmation = 99,
		/// <summary>
		/// 30
		/// </summary>
		BatchAmount = 30,
		/// <summary>
		/// 31
		/// </summary>
		CurrentBatchNumber = 31,
		/// <summary>
		/// 32
		/// </summary>
		IncreaseBatchNumber = 32
	}
}
