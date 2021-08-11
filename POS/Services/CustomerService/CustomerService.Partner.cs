using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class CustomerService
	{
		/// <summary>
		/// Gives you a chance to add to a newly created customer.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="customer">The newly created customer</param>
		/// <returns>If the customer needs to be saved again</returns>
		[LSOneUsage(CodeUsage.Partner)]
		public virtual bool AddCustomerExtra(IConnectionManager entry, Customer customer)
		{
			return false;
		}
	}
}