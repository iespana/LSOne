using System;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public class PosOperation : DataEntity
    {
        public PosOperation()
        {
            MasterID = Guid.Empty;
            Type = OperationTypeEnum.All;
            LookupType = LookupTypeEnum.None;
            Audit = OperationAuditEnum.Never;
            OperationDBName = "";
        }

        public override string Text
        {
            get
            {
                int operationID = 0;
                int.TryParse(ID.StringValue, out operationID);

                POSOperations operationType = (POSOperations)operationID;

                switch (operationType)
                {
                    //Operations that the user can never put on a button
                    case POSOperations.SetCostPrice:
                    case POSOperations.ProcessInput:
                    case POSOperations.SalespersonBarcode:
                    case POSOperations.SalespersonCard:
                    case POSOperations.ItemSaleMulti:
                    case POSOperations.RFIDSale:
                    case POSOperations.PayTransaction:
                    case POSOperations.SalesHistory:
                        return OperationDBName;

                    //Operations that can be put on a button
                    case POSOperations.NoOperation:
                        return Properties.Resources.NoOperation;
                    case POSOperations.ItemSale:
                        return Properties.Resources.ItemSale;
                    case POSOperations.PriceCheck:
                        return Properties.Resources.PriceCheck;
                    case POSOperations.VoidItem:
                        return Properties.Resources.VoidItem;
                    case POSOperations.ItemComment:
                        return Properties.Resources.ItemComment;
                    case POSOperations.PriceOverride:
                        return Properties.Resources.PriceOverride;
                    case POSOperations.SetQty:
                        return Properties.Resources.SetQty;
                    case POSOperations.ClearQty:
                        return Properties.Resources.ClearQty;
                    case POSOperations.ClearPriceOverride:
                        return Properties.Resources.ClearPriceOverride;
                    case POSOperations.ItemSearch:
                        return Properties.Resources.ItemSearch;
                    case POSOperations.ReturnItem:
                        return Properties.Resources.ReturnItem;
                    case POSOperations.WeighItem:
                        return Properties.Resources.WeighItem;
                    case POSOperations.FuelItemSale:
                        return Properties.Resources.FuelItemSale;
                    case POSOperations.LinkedItemsAdd:
                        return Properties.Resources.LinkedItemsAdd;
                    case POSOperations.SetDimensions:
                        return Properties.Resources.SetDimensions;
                    case POSOperations.ReturnTransaction:
                        return Properties.Resources.ReturnTransaction;
                    case POSOperations.ShowJournal:
                        return Properties.Resources.ShowJournal;
                    case POSOperations.LoyaltyRequest:
                        return Properties.Resources.LoyaltyRequest;
                    case POSOperations.SalespersonClear:
                        return Properties.Resources.ClearSalesPerson;
                    case POSOperations.InvoiceComment:
                        return Properties.Resources.InvoiceComment;
                    case POSOperations.ChangeUnitOfMeasure:
                        return Properties.Resources.ChangeUOM;
                    case POSOperations.DiscountVoucherAdd:
                        return Properties.Resources.DiscountVoucherAdd;
                    case POSOperations.DiscountVoucherRemove:
                        return Properties.Resources.DiscountVoucherRemove;
                    case POSOperations.InfocodesOnRequest:
                        return Properties.Resources.InfocodeOnRequest;
                    case POSOperations.ClearItemComments:
                        return Properties.Resources.ClearItemComments;
                    case POSOperations.ChangeItemComments:
                        return Properties.Resources.ChangeItemComments;
                    case POSOperations.GetLoyaltycardInfo:
                        return Properties.Resources.LoyaltyCardInformation;
                    case POSOperations.LoyaltyPointDiscount:
                        return Properties.Resources.LoyaltyPointsDiscount;
                    case POSOperations.JournalLogExport:
                        return Properties.Resources.JournalLogExport;
                    case POSOperations.PayCash:
                        return Properties.Resources.PayCash;
                    case POSOperations.PayCard:
                        return Properties.Resources.PayCard;
                    case POSOperations.PayCustomerAccount:
                        return Properties.Resources.PayCustomerAccount;
                    case POSOperations.PayCurrency:
                        return Properties.Resources.PayCurrency;
                    case POSOperations.PayCheque:
                        return Properties.Resources.PayCheck;
                    case POSOperations.PayCashQuick:
                        return Properties.Resources.PayCashQuick;
                    case POSOperations.PayLoyalty:
                        return Properties.Resources.PayLoyalty;
                    case POSOperations.PayCorporateCard:
                        return Properties.Resources.PayCorporateCard;
                    case POSOperations.ChangeBack:
                        return Properties.Resources.ChangeBack;
                    case POSOperations.VoidPayment:
                        return Properties.Resources.VoidPayment;
                    case POSOperations.FleetCardInfo:
                        return Properties.Resources.GetFleetCardInformation;
                    case POSOperations.PayCreditMemo:
                        return Properties.Resources.PayCreditMemo;
                    case POSOperations.PayGiftCertificate:
                        return Properties.Resources.PayGiftCard;
                    case POSOperations.AuthorizeCard:
                        return Properties.Resources.AuthorizeCard;
                    case POSOperations.AuthorizeCardQuick:
                        return Properties.Resources.AuthorizeCardQuick;
                    case POSOperations.LineDiscountAmount:
                        return Properties.Resources.LineDiscountAmount;
                    case POSOperations.LineDiscountPercent:
                        return Properties.Resources.LineDiscountPercent;
                    case POSOperations.TotalDiscountAmount:
                        return Properties.Resources.TotalDiscountAmount;
                    case POSOperations.TotalDiscountPercent:
                        return Properties.Resources.TotalDiscountPercent;
                    case POSOperations.ManuallyTriggerPeriodicDisount:
                        return Properties.Resources.ManuallyTriggerPerDiscAmt;
                    case POSOperations.ClearManuallyTriggeredPeriodicDiscount:
                        return Properties.Resources.ClearManuallyTriggeredPeriodicDiscount;
                    case POSOperations.ClearCoupon:
                        return Properties.Resources.ClearCoupon;
                    case POSOperations.ClearLineDiscount:
                        return Properties.Resources.ClearLineDiscount;
                    case POSOperations.ClearAllDiscounts:
                        return Properties.Resources.ClearAllDiscounts;
                    case POSOperations.ClearTotalDiscount:
                        return Properties.Resources.ClearTotalDiscount;
                    case POSOperations.VoidTransaction:
                        return Properties.Resources.VoidTransaction;
                    case POSOperations.TransactionComment:
                        return Properties.Resources.TransactionComment;
                    case POSOperations.SalesPerson:
                        return Properties.Resources.SalesPerson;
                    case POSOperations.SuspendTransaction:
                        return Properties.Resources.SuspendTransaction;
                    case POSOperations.RecallTransaction:
                        return Properties.Resources.RecallTransaction;
                    case POSOperations.RecallUnconcludedTransaction:
                        return Properties.Resources.RecallUnconcludedTransaction;
                    case POSOperations.CardSwipe:
                        return Properties.Resources.CardSwipe;
                    case POSOperations.PharmacyPrescriptionCancel:
                        return Properties.Resources.PharmPrescriptionCancel;
                    case POSOperations.PharmacyPrescriptions:
                        return Properties.Resources.PharmPrescriptions;
                    case POSOperations.IssueCreditMemo:
                        return Properties.Resources.IssueCreditMemo;
                    case POSOperations.IssueGiftCertificate:
                        return Properties.Resources.IssueGiftCard;
                    case POSOperations.DisplayTotal:
                        return Properties.Resources.DisplayTotal;
                    case POSOperations.SalesOrder:
                        return Properties.Resources.SalesOrder;
                    case POSOperations.SalesInvoice:
                        return Properties.Resources.SalesInvoice;
                    case POSOperations.IncomeAccounts:
                        return Properties.Resources.IncomeAccount;
                    case POSOperations.ExpenseAccounts:
                        return Properties.Resources.ExpenseAccount;
                    case POSOperations.ReturnIncomeAccounts:
                        return Properties.Resources.ReturnIncomeAccounts;
                    case POSOperations.ReturnExpenseAccounts:
                        return Properties.Resources.ReturnExpenseAccounts;
                    case POSOperations.GetGiftCardBalance:
                        return Properties.Resources.GetGiftCardBalance;
                    case POSOperations.Customer:
                        return Properties.Resources.CustomerOperationName;
                    case POSOperations.CustomerSearch:
                        return Properties.Resources.CustomerSearch;
                    case POSOperations.CustomerClear:
                        return Properties.Resources.CustomerClear;                    
                    case POSOperations.CustomerTransactions:
                        return Properties.Resources.CustomerTransactions;
                    case POSOperations.CustomerTransactionsReport:
                        return Properties.Resources.CustomerTransactionsReport;
                    case POSOperations.CustomerBalanceReport:
                        return Properties.Resources.CustomerBalanceReport;
                    case POSOperations.CustomerAdd:
                        return Properties.Resources.CustomerAdd;
                    case POSOperations.CustomerOrders:
                        return Properties.Resources.CustomerOrder;
                    case POSOperations.Quotes:
                        return Properties.Resources.Quote;
                    case POSOperations.RecallCustomerOrders:
                        return Properties.Resources.RecallCustomerOrders;
                    case POSOperations.RecallQuotes:
                        return Properties.Resources.RecallQuotes;
                    case POSOperations.LogOn:
                        return Properties.Resources.Logon;
                    case POSOperations.LogOff:
                        return Properties.Resources.LogOff;
                    case POSOperations.ChangeUser:
                        return Properties.Resources.ChangeUser;
                    case POSOperations.LockTerminal:
                        return Properties.Resources.LockTerminal;
                    case POSOperations.LogOffForce:
                        return Properties.Resources.LogOffForce;
                    case POSOperations.NegativeAdjustment:
                        return Properties.Resources.NegativeAdjustment;
                    case POSOperations.InventoryLookup:
                        return Properties.Resources.InventoryLookup;
                    case POSOperations.ApplicationExit:
                        return Properties.Resources.ApplicationExit;
                    case POSOperations.InitializeZReport:
                        return Properties.Resources.InitializeZReport;
                    case POSOperations.PrintX:
                        return Properties.Resources.PrintX;
                    case POSOperations.PrintZ:
                        return Properties.Resources.PrintZ;
                    case POSOperations.PrintTaxFree:
                        return Properties.Resources.PrintTaxFree;
                    case POSOperations.PrintPreviousSlip:
                        return Properties.Resources.PrintPreviousSlip;
                    case POSOperations.PrintPreviousInvoice:
                        return Properties.Resources.PrintPreviousInvoice;
                    case POSOperations.UploadPrinterLogo:
                        return Properties.Resources.UploadPrinterLogo;
                    case POSOperations.RestartComputer:
                        return Properties.Resources.RestartComputer;
                    case POSOperations.ShutDownComputer:
                        return Properties.Resources.ShutDownComputer;
                    case POSOperations.DesignModeEnable:
                        return Properties.Resources.DesignModeEnable;
                    case POSOperations.DesignModeDisable:
                        return Properties.Resources.DesignModeDisable;
                    case POSOperations.MinimizePOSWindow:
                        return Properties.Resources.MinimizePOSWindow;
                    case POSOperations.BlankOperation:
                        return Properties.Resources.BlankOperation;
                    case POSOperations.RunExternalCommand:
                        return Properties.Resources.RunExternalCommand;
                    case POSOperations.ExecutePOSPlugin:
                        return Properties.Resources.ExecutePOSPlugin;
                    case POSOperations.PrintItemSaleReport:
                        return Properties.Resources.PrintItemSalesReport;
                    case POSOperations.PrintFiscalInfoSlip:
                        return Properties.Resources.PrintFiscalInfoSlip;
                    case POSOperations.OpenDrawer:
                        return Properties.Resources.OpenDrawer;
                    case POSOperations.EndOfDay:
                        return Properties.Resources.EndOfDay;
                    case POSOperations.EndOfShift:
                        return Properties.Resources.EndOfShift;
                    case POSOperations.TenderDeclaration:
                        return Properties.Resources.TenderDeclaration;
                    case POSOperations.CustomerAccountDeposit:
                        return Properties.Resources.CustomerAccountDeposit;
                    case POSOperations.AddCustomerToLoyaltyCard:
                        return Properties.Resources.AddCustomerToLoyaltyCard;
                    case POSOperations.DeclareStartAmount:
                        return Properties.Resources.DeclareStartAmount;
                    case POSOperations.FloatEntry:
                        return Properties.Resources.FloatEntryOperation;
                    case POSOperations.TenderRemoval:
                        return Properties.Resources.TenderRemoval;
                    case POSOperations.SafeDrop:
                        return Properties.Resources.SafeDropOperation;
                    case POSOperations.BankDrop:
                        return Properties.Resources.BankDropOperation;
                    case POSOperations.SafeDropReversal:
                        return Properties.Resources.SafeDropReversalOperation;
                    case POSOperations.BankDropReversal:
                        return Properties.Resources.BankDropReversalOperation;
                    case POSOperations.SplitBill:
                        return Properties.Resources.SplitBill;
                    case POSOperations.ShowHospitality:
                        return Properties.Resources.ExitHospitalityPOS;
                    case POSOperations.PrintHospitalityMenuType:
                        return Properties.Resources.PrintHospitalityMenuType;
                    case POSOperations.SetHospitalityMenuType:
                        return Properties.Resources.SetHospitlaityMenuType;
                    case POSOperations.ChangeHospitalityMenuType:
                        return Properties.Resources.ChangeHospitalityMenuType;
                    case POSOperations.BumpOrder:
                        return Properties.Resources.BumpOrder;
                    case POSOperations.TaxExemptTransaction:
                        return Properties.Resources.TaxExemptTransaction;
                    case POSOperations.ClearTransactionTaxExemption:
                        return Properties.Resources.ClearTransactionTaxExemption;
                    case POSOperations.InfocodeTaxGroupChange:
                        return Properties.Resources.InfocodeTaxGroupChange;
                    case POSOperations.OpenMenu:
                        return Properties.Resources.OpenMenu;
                    case POSOperations.SubMenu:
                        return Properties.Resources.SubMenu;
                    case POSOperations.PopupMenu:
                        return Properties.Resources.PopupMenu;
                    case POSOperations.RefreshDisplayStations:
                        return Properties.Resources.RefreshKitchenDisplays;
                    case POSOperations.TaxRefund:
                        return Properties.Resources.TaxRefund;
                    case POSOperations.StartOfDay:
                        return Properties.Resources.StartOfDay;
                    case POSOperations.ScanQR:
                        return Properties.Resources.ReprintReceipt;
                    case POSOperations.RunJob:
                        return Properties.Resources.RunJob;
                    case POSOperations.ReprintReceipt:
                        return Properties.Resources.ReprintReceipt;
                    case POSOperations.SetReasonCode:
                        return Properties.Resources.SetReasonCode;
                    case POSOperations.EmailReceipt:
                        return Properties.Resources.SendAnEmailReceipt;
                    case POSOperations.CustomerEdit:
                        return Properties.Resources.EditCustomer;
                    default:
                        return OperationDBName;
                }
            }
            set { OperationDBName = value; } // Fallback to support some legacy ways.
        }

        public Guid MasterID { get; set; }

        /// <summary>
        /// The original operation name from the database
        /// </summary>
        public string OperationDBName { get; set; }

        public OperationTypeEnum Type { get; set; }

        public LookupTypeEnum LookupType { get; set; }

        public OperationAuditEnum Audit { get; set; }
    }
}
