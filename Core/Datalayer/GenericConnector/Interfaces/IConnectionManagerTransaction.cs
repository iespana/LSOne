using System.Data;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
    public interface IConnectionManagerTransaction : IConnectionManager
    {
        void Commit();
        void Rollback();

        //TO Be deleted DO NOT USE
        IDbTransaction TempGetTransaction();
    }
}
