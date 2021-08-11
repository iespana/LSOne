using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using SalesOrderInterface;
using System.Collections.Generic;

namespace LSOne.Services.Interfaces
{
    public class Services
    {
        public static IHospitalityService HospitalityService(IConnectionManager entry)
        {
            return (IHospitalityService)entry.Service(ServiceType.HospitalityService);
        }

        public static IInfocodesService InfocodesService(IConnectionManager entry)
        {
            return (IInfocodesService)entry.Service(ServiceType.InfocodesService);
        }

        public static IEndOfDayService EndOfDayPOSService(IConnectionManager entry)
        {
            return (IEndOfDayService)entry.Service(ServiceType.EndOfDayService);
        }

        public static ICurrencyService CurrencyService(IConnectionManager entry)
        {
            return (ICurrencyService)entry.Service(ServiceType.CurrencyService);
        }

        public static IRFIDService RFIDService(IConnectionManager entry)
        {
            return (IRFIDService)entry.Service(ServiceType.RFIDService);
        }

        public static IGiftCardService GiftCardService(IConnectionManager entry)
        {
            return (IGiftCardService)entry.Service(ServiceType.GiftCardService);
        }

        public static IForecourtService ForecourtService(IConnectionManager entry)
        {
            return (IForecourtService)entry.Service(ServiceType.ForecourtService);
        }

        public static IPrintingService PrintingService(IConnectionManager entry)
        {
            return (IPrintingService)entry.Service(ServiceType.PrintingService);
        }

        public static IEFTService EFTService(IConnectionManager entry)
        {
            return (IEFTService)entry.Service(ServiceType.EFTService);
        }

        public static IDiscountService DiscountService(IConnectionManager entry)
        {
            return (IDiscountService)entry.Service(ServiceType.DiscountService);
        }

        public static IDimensionService DimensionService(IConnectionManager entry)
        {
            return (IDimensionService)entry.Service(ServiceType.DimensionService);
        }

        public static ICustomerService CustomerService(IConnectionManager entry)
        {
            return (ICustomerService)entry.Service(ServiceType.CustomerService);
        }

        public static ICreditMemoService CreditMemoService(IConnectionManager entry)
        {
            return (ICreditMemoService)entry.Service(ServiceType.CreditMemoService);
        }

        public static ICorporateCardService CorporateCardService(IConnectionManager entry)
        {
            return (ICorporateCardService)entry.Service(ServiceType.CorporateCardService);
        }

        public static ICouponService CouponService(IConnectionManager entry)
        {
            return (ICouponService) entry.Service(ServiceType.CouponService);
        }

        public static ICCTVService CCTVService(IConnectionManager entry)
        {
            return (ICCTVService)entry.Service(ServiceType.CCTVService);
        }

        public static ICashChangerService CashChangerService(IConnectionManager entry)
        {
            return (ICashChangerService)entry.Service(ServiceType.CashChangerService);
        }

        public static ICashManagementService CashManagementService(IConnectionManager entry)
        {
            return (ICashManagementService)entry.Service(ServiceType.CashManagementService);
        }

        public static IBlankOperationsService BlankOperationsService(IConnectionManager entry)
        {
            return (IBlankOperationsService)entry.Service(ServiceType.BlankOperationsService);
        }

        public static IRoundingService RoundingService(IConnectionManager entry)
        {
            return (IRoundingService)entry.Service(ServiceType.RoundingService);
        }

        public static IExcelService ExcelService(IConnectionManager entry)
        {
            return (IExcelService)entry.Service(ServiceType.ExcelService);
        }

        public static ITaxService TaxService(IConnectionManager entry)
        {
            return (ITaxService)entry.Service(ServiceType.TaxService);
        }

        public static ICalculationService CalculationService(IConnectionManager entry)
        {
            return (ICalculationService)entry.Service(ServiceType.CalculationService);
        }

        public static ISalesInvoiceService SalesInvoiceService(IConnectionManager entry)
        {
            return (ISalesInvoiceService)entry.Service(ServiceType.SalesInvoiceService);
        }

        public static ILoyaltyService LoyaltyService(IConnectionManager entry)
        {
            return (ILoyaltyService)entry.Service(ServiceType.LoyaltyService);
        }

        public static ICardService CardService(IConnectionManager entry)
        {
            return (ICardService)entry.Service(ServiceType.CardService);
        }

        public static ITenderRestrictionService TenderRestrictionService(IConnectionManager entry)
        {
            return (ITenderRestrictionService)entry.Service(ServiceType.TenderRestrictionService);
        }

        public static ISiteServiceService SiteServiceService(IConnectionManager entry)
        {
            return (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
        }

        public static IEndOfDayBackOfficeService EndOfDayBOService(IConnectionManager entry)
        {
            return (IEndOfDayBackOfficeService)entry.Service(ServiceType.EndOfDayBackOfficeService);
        }

        public static ILoginPanelService LoginPanelService(IConnectionManager entry)
        {
            return (ILoginPanelService)entry.Service(ServiceType.LoginPanelService);
        }

        public static IApplicationService ApplicationService(IConnectionManager entry)
        {
            return (IApplicationService)entry.Service(ServiceType.ApplicationService);
        }

        public static IBarcodeService BarcodeService(IConnectionManager entry)
        {
            return (IBarcodeService)entry.Service(ServiceType.BarcodeService);
        }

        public static IAddressLookupService AddressLookupService(IConnectionManager entry)
        {
            return (IAddressLookupService)entry.Service(ServiceType.AddressLookupService);
        }

        public static IPharmacyService PharmacyService(IConnectionManager entry)
        {
            return (IPharmacyService)entry.Service(ServiceType.PharmacyService);
        }

        public static IDialogService DialogService(IConnectionManager entry)
        {
            return (IDialogService)entry.Service(ServiceType.DialogService);
        }

        public static IDualDisplayService DualDisplayService(IConnectionManager entry)
        {
            return (IDualDisplayService) entry.Service(ServiceType.DualDisplayService);
        }

        public static ITransactionService TransactionService(IConnectionManager entry)
        {
            return (ITransactionService) entry.Service(ServiceType.TransactionService);
        }

        public static IInventoryService InventoryService(IConnectionManager entry)
        {
            return (IInventoryService)entry.Service(ServiceType.InventoryService);
        }

        public static ILabelService LabelService(IConnectionManager entry)
        {
            return (ILabelService) entry.Service(ServiceType.LabelService);
        }

        public static ILicenseService LicenseService(IConnectionManager entry)
        {
            return (ILicenseService) entry.Service(ServiceType.LicenseService);
        }

        public static ITaxFreeService TaxFreeService(IConnectionManager entry)
        {
            return (ITaxFreeService) entry.Service(ServiceType.TaxFreeService);
        }

        public static IEventService EventService(IConnectionManager entry)
        {
            return (IEventService)entry.Service(ServiceType.EventService);
        }

        public static IItemService ItemService(IConnectionManager entry)
        {
            return (IItemService)entry.Service(ServiceType.ItemService);
        }

        public static IPriceService PriceService(IConnectionManager entry)
        {
            return (IPriceService)entry.Service(ServiceType.PriceService);
        }

        public static IStartOfDayService StartOfDayService(IConnectionManager entry)
        {
            return (IStartOfDayService) entry.Service(ServiceType.StartOfDayService);
        }

        public static IScaleService ScaleService(IConnectionManager entry)
        {
            return (IScaleService)entry.Service(ServiceType.ScaleService);
        }

        public static IDDService DDService(IConnectionManager entry)
        {
            return (IDDService)entry.Service(ServiceType.DDService);
        }

        public static ISalesOrderService SalesOrderService(IConnectionManager entry)
        {
            return (ISalesOrderService)entry.Service(ServiceType.SalesOrderService);
        }

        public static IFiscalService FiscalService(IConnectionManager entry)
        {
            return (IFiscalService)entry.Service(ServiceType.FiscalService);
        }

        public static IMigrationService MigrationService(IConnectionManager entry)
        {
            return (IMigrationService)entry.Service(ServiceType.MigrationService);
        }
        public static IBackupService BackupService(IConnectionManager entry)
        {
            return (IBackupService)entry.Service(ServiceType.BackupService);
        }
        public static ITenderService TenderService(IConnectionManager entry)
        {
            return (ITenderService)entry.Service(ServiceType.TenderService);
        }

        public static ICustomerOrderService CustomerOrderService(IConnectionManager entry)
        {
            return (ICustomerOrderService)entry.Service(ServiceType.CustomerOrderService);
        }

        public static List<string> GetServiceOverloadNames(IConnectionManager entry,ServiceType serviceType)
        {
            return ((ConnectionManagerBase)entry).GetServiceOverloadNames(serviceType);
        }

        public static string GetServiceShortName(IConnectionManager entry,ServiceType serviceType)
        {
            return ((ConnectionManagerBase)entry).GetServiceShortName(serviceType);
        }

        public static void UnloadService(IConnectionManager entry, ServiceType serviceType)
        {
            ((ConnectionManagerBase)entry).UnloadService(serviceType);
        }

        public static List<KeyValuePair<ServiceType,string>> GetOverloadServiesWithMask(IConnectionManager entry,string endsWithMask, ServiceType? exclude)
        {
            return ((ConnectionManagerBase)entry).GetOverloadServiesWithMask(endsWithMask, exclude);
        }

        public static bool HasService(IConnectionManager entry,ServiceType serviceType)
        {
            return ((ConnectionManagerBase)entry).HasService(serviceType);
        }


        public static ITimeKeeperService TimeKeeperService(IConnectionManager entry)

        {
            return (ITimeKeeperService)entry.Service(ServiceType.TimeKeeperService);
        }

        public static ICentralSuspensionService CentralSuspensionService(IConnectionManager entry)
        {
            return (ICentralSuspensionService)entry.Service(ServiceType.CentralSuspensionService);
        }
    }
}
