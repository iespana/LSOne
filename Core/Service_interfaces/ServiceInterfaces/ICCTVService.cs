using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface ICCTVService : IService
    {

        /// <summary>
        /// Sends an output to a CCTV system
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The transaction object containing all information about items, payments, discounts, etc...</param>
        /// <param name="operationId">The id of the operation currently being run</param>
        /// <param name="mainOperation">If the currently run operation is a main operation or a support operation</param>
        /// <param name="operationInfo">Extra information about the operation currently being run</param>
        /// <param name="text">Optional text supplied by LS POS</param>
        void CCTVOutput(IConnectionManager entry, IPosTransaction posTransaction, POSOperations operationId, bool mainOperation, OperationInfo operationInfo, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="text"></param>
        void CCTVMessageOutput(IConnectionManager entry, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="text"></param>
        void CCTVErrorOutput(IConnectionManager entry, string text);
    }
}
