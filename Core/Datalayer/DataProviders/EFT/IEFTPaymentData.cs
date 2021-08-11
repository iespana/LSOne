using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.EFT
{
    public interface IEFTPaymentData : IDataProvider<EFTPayment>, ISequenceable
    {
    }
}
