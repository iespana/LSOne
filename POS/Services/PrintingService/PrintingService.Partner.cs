using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class PrintingService
	{
		[LSOneUsage(CodeUsage.Partner)]
		public virtual bool PrintPartnerReceipt(IConnectionManager entry, IEFTInfo eftInfo, FormSystemType formType)
		{
			return false;
		}

		[LSOneUsage(CodeUsage.Partner)]
		public virtual void PrintCustomReceipt(IConnectionManager entry, Transaction transaction, string customText,OperationInfo operationInfo)
		{
			string comment = "This operation has not been implemented."                
					+ "\r\n"
					+ "Parameter received: " + operationInfo.Parameter;

			Interfaces.Services.DialogService(entry).ShowMessage(comment, MessageBoxButtons.OK, MessageDialogType.Generic);

		}

	}
}