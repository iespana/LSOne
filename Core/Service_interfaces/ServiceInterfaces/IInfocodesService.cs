using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInfocodesService : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <param name="refRelation"></param>
        /// <param name="refRelation2"></param>
        /// <param name="refRelation3"></param>
        /// <param name="tableRefId"></param>
        /// <param name="linkedInfoCodeId"></param>
        /// <param name="orgInfocode"></param>
        /// <param name="infocodeType"></param>
        /// <param name="automaticTriggering"></param>
        /// <returns></returns>
        bool ProcessInfocode(IConnectionManager entry, ISession session,IPosTransaction posTransaction, decimal quantity, decimal amount, string refRelation, string refRelation2, string refRelation3, InfoCodeLineItem.TableRefId tableRefId, string linkedInfoCodeId, InfoCodeLineItem orgInfocode, InfoCodeLineItem.InfocodeType infocodeType, bool automaticTriggering);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <param name="refRelation"></param>
        /// <param name="refRelation2"></param>
        /// <param name="refRelation3"></param>
        /// <param name="tableRefId"></param>
        /// <param name="linkedInfoCodeId"></param>
        /// <param name="orgInfocode"></param>
        /// <param name="infocodeType"></param>
        /// <param name="saleLineItem"></param>
        /// <param name="automaticTriggering"></param>
        /// <param name="minimumValue"></param>
        /// <returns></returns>
        bool ProcessInfocode(IConnectionManager entry,
            ISession session,
            IPosTransaction posTransaction,
            decimal quantity,
            decimal amount,
            string refRelation,
            string refRelation2,
            string refRelation3,
            InfoCodeLineItem.TableRefId tableRefId,
            string linkedInfoCodeId,
            InfoCodeLineItem orgInfocode,
            InfoCodeLineItem.InfocodeType infocodeType,
            ref ISaleLineItem saleLineItem,
            bool automaticTriggering,
            decimal minimumValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <param name="refRelation"></param>
        /// <param name="refRelation2"></param>
        /// <param name="refRelation3"></param>
        /// <param name="tableRefId"></param>
        /// <param name="linkedInfoCodeId"></param>
        /// <param name="orgInfocode"></param>
        /// <param name="infocodeType"></param>
        /// <param name="saleLineItem"></param>
        /// <param name="automaticTriggering"></param>
        /// <returns></returns>
        bool ProcessInfocode(IConnectionManager entry,ISession session, IPosTransaction posTransaction, decimal quantity, decimal amount, string refRelation, string refRelation2, string refRelation3, InfoCodeLineItem.TableRefId tableRefId, string linkedInfoCodeId, InfoCodeLineItem orgInfocode, InfoCodeLineItem.InfocodeType infocodeType, ref ISaleLineItem saleLineItem, bool automaticTriggering);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction"></param>
        /// <param name="saleLineItem"></param>
        /// <param name="tableRefId"></param>
        /// <param name="infocodeType"></param>
        /// <returns></returns>
        bool ProcessLinkedInfocodes(IConnectionManager entry, ISession session, IPosTransaction posTransaction, ISaleLineItem saleLineItem, InfoCodeLineItem.TableRefId tableRefId, InfoCodeLineItem.InfocodeType infocodeType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction"></param>
        /// <param name="tenderLineItem"></param>
        /// <param name="storeId"></param>
        /// <param name="tableRefId"></param>
        /// <param name="infocodeType"></param>
        /// <returns></returns>
        bool ProcessLinkedInfocodes(IConnectionManager entry, ISession session, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, string storeId, InfoCodeLineItem.TableRefId tableRefId, InfoCodeLineItem.InfocodeType infocodeType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction"></param>
        /// <param name="tableRefId"></param>
        /// <param name="infocodeType"></param>
        /// <returns></returns>
        bool ProcessLinkedInfocodes(IConnectionManager entry, ISession session, IPosTransaction posTransaction, InfoCodeLineItem.TableRefId tableRefId, InfoCodeLineItem.InfocodeType infocodeType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="retailTransaction"></param>
        /// <param name="infocodeId"></param>
        /// <param name="inputRequired"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        bool ProcessCentralSuspensionInfocode(IConnectionManager entry, ISession session, IRetailTransaction retailTransaction, string infocodeId, bool inputRequired, string prompt);

        /// <summary>
        /// Displays all subcodes with trigger function TaxGroup for the given infocodeID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="infocodeID">The ID of the infocode to display subcodes for</param>
        /// <returns>True if the user chose to change the current tax group, false otherwise</returns>
        bool ChangeTaxGroup(IConnectionManager entry, IPosTransaction posTransaction, string infocodeID);
    }
}
