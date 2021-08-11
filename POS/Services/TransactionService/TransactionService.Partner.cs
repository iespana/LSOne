using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class TransactionService
	{
		[LSOneUsage(CodeUsage.Partner)]
		private int CalculatePartnerTotalNumberOfTransactionLines(IConnectionManager entry, IPosTransaction transaction)
		{
			return 0;
		}
	}
}