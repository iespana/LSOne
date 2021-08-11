using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.SaleByPaymentMediaReport.Datalayer.DataEntities;

namespace LSOne.ViewPlugins.SaleByPaymentMediaReport
{
    partial class PluginOperations
    {
        public static void AddDataToReport(
            string storeId,
            DateTime periodFrom,
            DateTime periodTo,
            List<TerminalPaymentMethods> terminalPaymentBreakdown,
            List<PaymentMethodAmount> paymentMethodBreakdown)
        {

        }

        public static IEnumerable<PaymentTransaction> ResolveCardTenderID(StorePaymentTypeCardType card, RecordIdentifier paymentTypeID, IEnumerable<PaymentTransaction> cardTransactions)
        {
            return null;
        }
    }
}
