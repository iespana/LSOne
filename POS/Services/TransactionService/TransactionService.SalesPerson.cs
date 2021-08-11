using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services
{
    public partial class TransactionService
    {
        public virtual void SetSalesPerson(IConnectionManager entry, ISession session, IPosTransaction transaction, OperationInfo operationInfo, RecordIdentifier salesPersonID, bool hereAfter)
        {
            Employee salesPerson = new Employee();
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!(transaction is RetailTransaction))
            {
                POSFormsManager.ShowPOSMessageDialog(Properties.Resources.OperationInvalidForTransaction);
                return;
            }

            if (salesPersonID != RecordIdentifier.Empty)
            {
                POSUser user = Providers.POSUserData.Get(entry, salesPersonID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
                if (user != null)
                {
                    salesPerson.ID = user.ID;
                    salesPerson.Login = user.Login;
                    salesPerson.Name = entry.Settings.NameFormatter.Format(user.Name);
                    salesPerson.NameOnReceipt = user.NameOnReceipt;
                }
            }

            //If a specific sales person was not selected as a parameter
            if (salesPerson.Exists == false)
            {
                DialogResult dlgResult = ((IDialogService)entry.Service(ServiceType.DialogService)).ShowSalesPersonDialog(entry, ref salesPerson);

                if (dlgResult != DialogResult.OK)
                {
                    return;
                }
            }

            if (hereAfter)
            {
                ((RetailTransaction)transaction).SalesPerson = (Employee)salesPerson.Clone();
                settings.Terminal.SalesPersonID = ((RetailTransaction)transaction).SalesPerson.ID;
            }
            else
            {
                ((RetailTransaction)transaction).SalesPerson.Clear();
                settings.Terminal.SalesPersonID = RecordIdentifier.Empty;
            }

            if (((string)settings.FunctionalityProfile.InfocodeAddSalesPerson).Trim() != "")
            {
                Interfaces.Services.InfocodesService(entry).ProcessInfocode(entry, session, transaction, 0, 0, (string)settings.FunctionalityProfile.ID,
                    ((string)settings.FunctionalityProfile.InfocodeAddSalesPerson).Trim(), "", InfoCodeLineItem.TableRefId.FunctionalityProfile, "", null,
                    InfoCodeLineItem.InfocodeType.Header, true);

                Interfaces.Services.InfocodesService(entry)
                    .ProcessLinkedInfocodes(entry, session, transaction,
                        InfoCodeLineItem.TableRefId.FunctionalityProfile, InfoCodeLineItem.InfocodeType.Header);
            }

            if (((RetailTransaction)transaction).SaleItems.Count > 0)
            {
                ISaleLineItem sli = ((RetailTransaction)transaction).GetItem(operationInfo.ItemLineId);
                if (sli != null)
                {
                    sli.SalesPerson = (Employee)salesPerson.Clone();
                }
            }

            POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.SalesPerson + " " + (salesPerson).Name + " " + Properties.Resources.AddedEnd);
        }

        public virtual bool CanRunSalesPersonOperation(ISettings settings, IPosTransaction transaction, POSOperations operationID, LinkedList<ISaleLineItem> saleItemsBeforeOperation)
        {
            return transaction is RetailTransaction
                && operationID != POSOperations.RecallCustomerOrders
                && operationID != POSOperations.RecallQuotes
                && operationID != POSOperations.RecallTransaction
                && operationID != POSOperations.RecallUnconcludedTransaction
                && operationID != POSOperations.ReturnTransaction
                && operationID != POSOperations.ShowJournal
                && (((RetailTransaction)transaction).SalesPerson == null || RecordIdentifier.IsEmptyOrNull(((RetailTransaction)transaction).SalesPerson.ID))
                && settings.FunctionalityProfile.SalesPersonPrompt != SalesPersonPrompt.None
                && saleItemsBeforeOperation.Where(x => x.GetType() == typeof(SaleLineItem)).Count() == 0
                && ((RetailTransaction)transaction).SaleItems.Where(x => x.GetType() == typeof(SaleLineItem)).Count() > 0
                && transaction.EntryStatus != TransactionStatus.Concluded
                && transaction.EntryStatus != TransactionStatus.Cancelled;
        }
    }
}
