using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.CustomerOrders
{
    public interface ICustomerOrderSettings : IDataProvider<CustomerOrderSettings>
    {
        List<CustomerOrderSettings> GetList(IConnectionManager entry);
        CustomerOrderSettings Get(IConnectionManager entry, RecordIdentifier ID);

        CustomerOrderSettings Get(IConnectionManager entry, CustomerOrderType orderType);
    }
}
