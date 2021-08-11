using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders
{
    /// <summary>
    /// This interface must be implemented by classes that know how to register data providers with LS One
    /// </summary>
    public interface IRegistrar
    {
        /// <summary>
        /// Register data providers and return the number of data providers registered
        /// </summary>
        /// <returns></returns>
        int Register();
    }
}
