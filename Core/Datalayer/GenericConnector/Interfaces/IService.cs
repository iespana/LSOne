using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
    public interface IService
    {
        IErrorLog ErrorLog
        {
            set;
        }

        void Init(IConnectionManager entry);
    }
}
