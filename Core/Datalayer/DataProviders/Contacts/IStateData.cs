using System.Collections.Generic;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Contacts
{
	public interface IStateData : IDataProviderBase<DataEntity>
	{
		List<DataEntity> GetList(IConnectionManager entry, Address.AddressFormatEnum formatterID);
	}
}