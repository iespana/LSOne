using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{

    public interface IBlankOperationsService : IService
    {
        /// <summary>
        /// Implemented by \Services\BlankOperations\BlankOperations.cs.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="operationInfo">Information about the current operation - <see cref="OperationInfo"/></param>
        /// <param name="posTransaction">The current posTransaction.</param>
        IPosTransaction BlankOperation(IConnectionManager entry, ISession session, OperationInfo operationInfo, IPosTransaction posTransaction);
    }
}
