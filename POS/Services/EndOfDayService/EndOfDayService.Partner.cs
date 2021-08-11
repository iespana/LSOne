using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class EndOfDayService
	{
		[LSOneUsage(CodeUsage.Partner)]
		public virtual void PartnerEndOfDay(IConnectionManager entry, IEndOfDayTransaction transaction)
		{
			
		}

		[LSOneUsage(CodeUsage.Partner)]
		public virtual void GetExtraHeaderText(EODInfo info)
		{
			
		}

		[LSOneUsage(CodeUsage.Partner)]
		public virtual ReportLogic GetReportLogic(IConnectionManager entry)
		{
			ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
			return new ReportLogic(settings.FunctionalityProfile.ZReportConfig.ReportWidth, settings.FunctionalityProfile.ZReportConfig.DefaultPadding);
		}
	}
}
