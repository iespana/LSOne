using LSOne.DataLayer.DataProviders;

namespace LSOne.DataLayer.SqlDDDataProviders
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
