using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders
{
    public class Registrar : IRegistrar
    {
        /// <summary>
        /// Register all data providers in this assembly with the data provider factory
        /// </summary>
        public virtual int Register()
        {
            return DataProviderFactory.Instance.RegisterProviders(typeof (Registrar).Assembly);
        }
    }
}
