using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IHospitalitySetupData : IDataProvider<HospitalitySetup>
    {
        HospitalitySetup Get(IConnectionManager entry);
    }
}