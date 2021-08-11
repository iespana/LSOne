using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class LabelService
	{
		[LSOneUsage(CodeUsage.Partner)]
		private RecordIdentifier GetEntityID(IDataEntity dataEntity)
		{
			return null;
		}
	}
}