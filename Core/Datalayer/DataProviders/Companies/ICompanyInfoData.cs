using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Companies
{
    public interface ICompanyInfoData : IDataProvider<CompanyInfo>
    {
        CompanyInfo Get(IConnectionManager entry, bool includeReportFormatting);

        bool HasCompanyCurrency(IConnectionManager entry);

        string CompanyCurrencyCode(IConnectionManager entry);
    }
}