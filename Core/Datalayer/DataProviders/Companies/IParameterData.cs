using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Companies
{
    public interface IParameterData : IDataProvider<Parameters>
    {
        Parameters Get(IConnectionManager entry);
    }
}