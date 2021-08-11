using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class Registrar : IRegistrar
    {
        /// <summary>
        /// Register all data providers in this assembly with the data provider factory
        /// </summary>
        public int Register()
        {
            return DataProviderFactory.Instance.RegisterProviders(typeof (Registrar).Assembly);
        }
    }
}
