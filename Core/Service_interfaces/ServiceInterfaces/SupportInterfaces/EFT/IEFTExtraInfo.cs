using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.ErrorHandling;
using System.Xml.Linq;

namespace LSOne.Services.Interfaces.SupportInterfaces.EFT
{
    /// <summary>
    /// Defines methods to add additional information to EFTInfo that need to be stored during a transactions lifetime
    /// </summary>
    public interface IEFTExtraInfo
    {
        /// <summary>
        /// Serializes the contents of this instance to XML and returns it as a <see cref="XElement"/>
        /// </summary>
        /// <param name="errorLogger">The logger instance</param>
        /// <returns></returns>
        XElement ToXml(IErrorLog errorLogger = null);

        /// <summary>
        /// Deserializes <paramref name="xmlExtraInfo"/> and populates this instance with the information
        /// </summary>
        /// <param name="xmlExtraInfo">The <see cref="XElement"/> that contains the serialized information</param>
        /// <param name="errorLogger">The logger instance</param>
        /// <returns></returns>
        void ToClass(XElement xmlExtraInfo, IErrorLog errorLogger = null);

        /// <summary>
        /// Saves any relevant information to the database. This is called when the transaction is saved into RBOTRANSACTIONTABLE
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="retailTransaction">The transaction that was just saved to the database</param>
        void Insert(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Called when the transaction has been rebuilt from data from the database. F.ex when returning a transaction or when it's being
        /// viewed from the journal. This will rebuild this instance from data from the posted transaction and any other data required from the transaction tables.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="retailTransaction">The transaction that was rebuilt from the database</param>
        void Rebuild(IConnectionManager entry, IRetailTransaction retailTransaction);
    }
}
