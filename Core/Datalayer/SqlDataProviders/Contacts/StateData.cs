using System.Collections.Generic;
using System.Data;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Contacts
{
	public class StateData : SqlServerDataProviderBase, IStateData
	{
		public virtual List<DataEntity> GetList(IConnectionManager entry, Address.AddressFormatEnum formatterID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = "Select ABBREVIATEDNAME, NAME from STATES where FORMATTERID = @formatterID and DATAAREAID = @dataAreaId order by NAME";

				MakeParam(cmd, "formatterID", (int)formatterID);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "ABBREVIATEDNAME");
			}
		}
	}
}
