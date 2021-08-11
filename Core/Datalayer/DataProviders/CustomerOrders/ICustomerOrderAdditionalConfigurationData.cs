using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.CustomerOrders
{
    public interface ICustomerOrderAdditionalConfigurationData : IDataProvider<CustomerOrderAdditionalConfigurations>
    {
        List<CustomerOrderAdditionalConfigurations> GetList(IConnectionManager entry);

        List<DataEntity> GetList(IConnectionManager entry, ConfigurationType type);

        CustomerOrderAdditionalConfigurations Get(IConnectionManager entry, RecordIdentifier ID);

        bool ConfigIsInUse(IConnectionManager entry, CustomerOrderAdditionalConfigurations config);
    }
}
