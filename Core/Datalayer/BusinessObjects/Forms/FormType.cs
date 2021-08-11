using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Forms
{
    public class FormType : DataEntity
    {
        public FormType()
            : base()
        {
            ID = RecordIdentifier.Empty;
            SystemType = 0;
           
        }

        public int SystemType { get; set; }

        public string SystemTypeText
        {
            get
            {
                switch ((FormSystemType) SystemType)
                {
                    case FormSystemType.Receipt:
                        return Properties.Resources.Receipt;
                    case FormSystemType.CardReceiptForShop:
                        return Properties.Resources.CardReceiptForShop;
                    case FormSystemType.CardReceiptForCust:
                        return Properties.Resources.CardReceiptForCust;
                    case FormSystemType.CardReceiptForShopReturn:
                        return Properties.Resources.CardReceiptForShopReturn;
                    case FormSystemType.CardReceiptForCustReturn:
                        return Properties.Resources.CardReceiptForCustReturn;
                    case FormSystemType.CardInfo:
                        return Properties.Resources.CardInfo;
                    case FormSystemType.CustAcntReceiptForCust:
                        return Properties.Resources.CustAcntReceiptForCust;
                    case FormSystemType.CustAcntReceiptForShop:
                        return Properties.Resources.CustAcntReceiptForShop;
                    case FormSystemType.CustAcntReceiptForCustReturn:
                        return Properties.Resources.CustAcntReceiptForCustReturn;
                    case FormSystemType.CustAcntReceiptForShopReturn:
                        return Properties.Resources.CustAcntReceiptForShopReturn;
                    case FormSystemType.TenderDeclaration:
                        return Properties.Resources.TenderDeclaration;
                    case FormSystemType.RemoveTender:
                        return Properties.Resources.RemoveTender;
                    case FormSystemType.CustomerAccountDeposit:
                        return Properties.Resources.CustomerAccountDeposit;
                    case FormSystemType.CreditMemo:
                        return Properties.Resources.CreditVoucher;
                    case FormSystemType.CreditMemoBalance:
                        return Properties.Resources.CreditVoucherBalance;
                    case FormSystemType.FloatEntry:
                        return Properties.Resources.FloatEntry;
                    case FormSystemType.SalesOrderReceipt:
                        return Properties.Resources.SalesOrderReceipt;
                    case FormSystemType.SalesInvoiceReceipt:
                        return Properties.Resources.SalesInvoiceReceipt;
                    case FormSystemType.GiftCertificate:
                        return Properties.Resources.GiftCard;
                    case FormSystemType.EFTMessage:
                        return Properties.Resources.EftMessage;
                    case FormSystemType.SuspendedTransaction:
                        return Properties.Resources.SuspendedTransaction;
                    case FormSystemType.SuspendedTransactionPrefix:
                        return Properties.Resources.SuspendedTransactionPrefix;
                    case FormSystemType.VoidedTransaction:
                        return Properties.Resources.VoidedTransaction;
                    case FormSystemType.Invoice:
                        return Properties.Resources.Invoice;
                    case FormSystemType.SafeDrop:
                        return Properties.Resources.SafeDrop;
                    case FormSystemType.BankDrop:
                        return Properties.Resources.BankDrop;
                    case FormSystemType.SafeDropReversal:
                        return Properties.Resources.SafeDropReversal;
                    case FormSystemType.BankDropReversal:
                        return Properties.Resources.BankDropReversal;
                    case FormSystemType.LoyaltyPaymentReceipt:
                        return Properties.Resources.LoyaltyPaymentReceipt;
                    case FormSystemType.UserDefinedType:
                        return Properties.Resources.UserDefinedType;
                    case FormSystemType.GiftReceipt:
                        return Properties.Resources.GiftReceipt;
                    case FormSystemType.CustomerOrderDeposit:
                        return Properties.Resources.CustomerOrderDeposit;
                    case FormSystemType.CustomerOrderInformation:
                        return Properties.Resources.CustomerOrderInformation;
                    case FormSystemType.CustomerOrderPickingList:
                        return Properties.Resources.CustomerOrderPickingList;
                    case FormSystemType.ReceiptEmailSubject:
                        return Properties.Resources.ReceiptEmailSubject;
                    case FormSystemType.ReceiptEmailBody:
                        return Properties.Resources.ReceiptEmailBody;
                    case FormSystemType.QuoteInformation:
                        return Properties.Resources.QuoteInformation;
                    case FormSystemType.OpenDrawer:
                        return Properties.Resources.OpenDrawer;
                    case FormSystemType.GiftCardBalance:
                        return Properties.Resources.GiftCardBalance;
                    case FormSystemType.KitchenSlip:
                        return Properties.Resources.KitchenSlip;
                    case FormSystemType.FiscalInfoSlip:
                        return Properties.Resources.FiscalInfo;
                    default:
                        return "";
                }
            }
        }
    }
}
