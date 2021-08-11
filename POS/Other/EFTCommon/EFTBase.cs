using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System.Linq;

namespace LSOne.Services.EFT.Common
{
    public class EFTBase
    {
        public EFTBase()
        {
        }

        protected void Init(IConnectionManager dataModel,ISettings settings)
        {
            DLLEntry.DataModel = dataModel;
            DLLEntry.Settings = settings;
        }

        protected decimal GetPayableAmountWithLimitation(IPosTransaction posTransaction, PaymentInfo paymentInfo, StorePaymentMethod storePaymentMethod)
        {
            decimal amountToPay;
            if (paymentInfo.RestrictedAmount == 0)
            {
                amountToPay = paymentInfo.BalanceAmount;
            }
            else
            {
                bool hasPaymentsWithoutLimitations =
                        posTransaction.ITenderLines
                        .Select(x => Providers.StorePaymentMethodData.Get(DLLEntry.DataModel, new RecordIdentifier(DLLEntry.DataModel.CurrentStoreID, new RecordIdentifier(x.TenderTypeId)), CacheType.CacheTypeApplicationLifeTime))
                        .Any(x => !x.PaymentLimitations.Any());
                if (storePaymentMethod.PaymentLimitations.Any() &&
                    posTransaction is RetailTransaction retailTransaction &&
                    !retailTransaction.IsReturnTransaction &&
                    retailTransaction.CustomerOrder.Empty() &&
                    !hasPaymentsWithoutLimitations)
                {
                    amountToPay = paymentInfo.RestrictedAmount;
                }
                else
                {
                    amountToPay = POS.Processes.Helpers.PaymentLimitationHelper.ChooseTheProperValue(paymentInfo.RestrictedAmount, paymentInfo.BalanceAmount);
                }
            }

            return amountToPay;
        }

        protected virtual void ProcessInfoCode(IConnectionManager entry, ISession session, IPosTransaction posTransaction, string storeId, TenderLineItem tenderLineItem)
        {
            //Get infocode information if needed.
            //RefRelation3 is empty for all tender type infocodes except the Card types - then it needs to be the CardTypeId
            string refRelation3 = "";
            if (tenderLineItem is CardTenderLineItem && ((CardTenderLineItem)tenderLineItem).CardTypeID != null)
            {
                refRelation3 = ((CardTenderLineItem)tenderLineItem).CardTypeID;
            }

            Interfaces.Services.InfocodesService(DLLEntry.DataModel).ProcessInfocode(DLLEntry.DataModel, session, posTransaction, 0, tenderLineItem.Amount, storeId, tenderLineItem.TenderTypeId, refRelation3, InfoCodeLineItem.TableRefId.Tender, "", null, InfoCodeLineItem.InfocodeType.Payment, true);
            Interfaces.Services.InfocodesService(DLLEntry.DataModel).ProcessLinkedInfocodes(DLLEntry.DataModel, session, posTransaction, tenderLineItem, storeId, InfoCodeLineItem.TableRefId.Tender, InfoCodeLineItem.InfocodeType.Payment);
        }
    }
}