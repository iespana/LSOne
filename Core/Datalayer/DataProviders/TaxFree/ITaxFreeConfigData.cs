using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.TaxFree
{
    public interface ITaxFreeConfigData : IDataProvider<TaxFreeConfig>
    {
        TaxFreeConfig Get(IConnectionManager entry);
        bool HasEntries(IConnectionManager entry);
    }
}
