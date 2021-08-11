using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using LSOne.Utilities.ColorPalette;
using LSOne.Services.Properties;
using LSOne.DataLayer.KDSBusinessObjects.Enums;

namespace LSOne.Services
{
    public class DiningTable
    {
        private IConnectionManager dataModel;

        public TableInfo Details { get; set; }
        public IPosTransaction Transaction { get; set; }
        public MenuButton TableButton { get; set; }
        public Customer Customer { get; set; }
        public string TableDescription { get; set; }

        public DiningTable(IConnectionManager entry)
        {
            Details = new TableInfo();
            Customer = new Customer();
            dataModel = entry;
        }

        public DiningTable(IConnectionManager entry, DiningTable table) : this(entry)
        {
            Transaction = table.Transaction;

            Details.TableID = table.Details.TableID;
            Details.Description = table.Details.Description;
            Details.DiningTableStatus = table.Details.DiningTableStatus;
            Details.NumberOfGuests = table.Details.NumberOfGuests;
            Details.StaffID = table.Details.StaffID;
            Details.TerminalID = table.Details.TerminalID;
            Details.StoreID = table.Details.StoreID;
            Details.SalesType = table.Details.SalesType;
            Details.Sequence = table.Details.Sequence;
            Details.DiningTableLayoutID = table.Details.DiningTableLayoutID;
            Details.Locked = table.Details.Locked;
            Details.CustomerID = table.Details.CustomerID;
            Details.KitchenStatus = table.Details.KitchenStatus;

            TableDescription = table.TableDescription;
            Customer = table.Customer;
        }
        private LogonInfo CreateLogonInfo(IConnectionManager entry)
        {
            var logonInfo = new LogonInfo
            {
                UserID = entry.CurrentUser.ID,
                storeId = (string)entry.CurrentStoreID,
                StaffID = entry.CurrentStaffID == RecordIdentifier.Empty ? "" : (string)entry.CurrentStaffID,
                terminalId = entry.CurrentTerminalID == RecordIdentifier.Empty ? "" : (string)entry.CurrentTerminalID
            };

            return logonInfo;
        }
        /// <summary>
        /// Clears Customer, Staff and number of guests on the table as well as resetting the description of the table button to it's default
        /// </summary>
        public void Clear()
        {
            ClearCustomer();
            ClearStaff();
            Details.Description = TableDescription;
            Details.NumberOfGuests = 0;
            Details.StaffID = "";
        }

        public void ClearCustomer()
        {
            Customer = new Customer();
            Details.CustomerID = RecordIdentifier.Empty;

            if (Transaction != null)
            {
                var customer = new Customer
                {
                    PriceGroupID = ((ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).StorePriceGroup
                };
                ((RetailTransaction)Transaction).Add(customer);
                ((RetailTransaction)Transaction).AddInvoicedCustomer(customer);
            }
        }

        public void ClearStaff()
        {
            if (Transaction != null)
            {
                Transaction.Cashier.Clear();
            }
        }

        public Color Color1()
        {
            Color color;

            if (this.Details.Locked == true)
            {
                color = ColorPalette.HospitalityTableStatusLocked;   
                return color;
            }

            switch (Details.DiningTableStatus)
            {
                case DiningTableStatus.Available:            
                    color = ColorPalette.HospitalityTableStatusAvailable;
                    break;
                case DiningTableStatus.GuestsSeated:         
                    color = ColorPalette.HospitalityTableStatusSeated;
                    break;
                case DiningTableStatus.OrderNotPrinted:      
                    color = ColorPalette.HospitalityTableStatusNotPrinted;
                    break;
                case DiningTableStatus.OrderPartiallyPrinted:
                    color = ColorPalette.HospitalityTableStatusNotPrinted;
                    break;
                case DiningTableStatus.OrderPrinted:
                    color = ColorPalette.HospitalityTableStatusPrinted;
                    break;
                case DiningTableStatus.OrderStarted:         
                    color = ColorPalette.HospitalityTableStatusStarted;
                    break;
                case DiningTableStatus.OrderFinished:        
                    color = ColorPalette.HospitalityTableStatusOrderFinished;
                    break;
                case DiningTableStatus.OrderConfirmed:       
                    color = ColorPalette.HospitalityTableStatusOrderFinished;
                    break;
                case DiningTableStatus.Locked:              
                    color = ColorPalette.HospitalityTableStatusLocked;
                    break;
                case DiningTableStatus.Unavailable:         
                    color = ColorPalette.HospitalityTableUnavailable;  
                    break;
                case DiningTableStatus.AlertNotServed:     
                    color = ColorPalette.HospitalityTableStatusAlertNotServed;
                    break;
                case DiningTableStatus.OrderSent:
                    color = ColorPalette.HospitalityTableStatusSent;
                    break;
                case DiningTableStatus.OrderPartiallySent:
                    if (Details.KitchenStatus == KitchenOrderStatusEnum.Done)
                    {
                        color = ColorPalette.HospitalityTableStatusPartiallySent;
                    }
                    else
                    {
                        color = ColorPalette.HospitalityTableStatusSent;
                    }
                    break;
                case DiningTableStatus.OrderNotSent:
                    color = ColorPalette.HospitalityTableStatusNotSent;
                    break;
                default:
                    color = ColorPalette.HospitalityTableUnavailable;           
                    break;
            }
            return color;
        }

        public Color Color2()
        {
            return Color1();           
        }

        public Color Forecolor()
        {
            Color forecolor;

            if (this.Details.Locked == true)
            {
                forecolor = ColorPalette.POSBackgroundColor;   
                return forecolor;
            }

            switch (Details.DiningTableStatus)
            {
                case DiningTableStatus.Available:            
                    forecolor = ColorPalette.POSTextColor;
                    break;
                case DiningTableStatus.GuestsSeated:        
                    forecolor = ColorPalette.POSTextColor;
                    break;
                case DiningTableStatus.OrderNotPrinted:      
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.OrderPartiallyPrinted:
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.OrderPrinted:         
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.OrderStarted:         
                    forecolor = ColorPalette.POSTextColor;
                    break;
                case DiningTableStatus.OrderFinished:       
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.OrderConfirmed:       
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.Locked:               
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.Unavailable:         
                    forecolor = ColorPalette.POSControlBorderColor;
                    break;
                case DiningTableStatus.AlertNotServed:       
                    forecolor = ColorPalette.POSWhite;
                    break;
                case DiningTableStatus.OrderSent:
                    forecolor = ColorPalette.POSTextColor;
                    break;
                case DiningTableStatus.OrderPartiallySent:
                    if (Details.KitchenStatus == KitchenOrderStatusEnum.Done)
                    {
                        forecolor = ColorPalette.POSWhite;
                    }
                    else
                    {
                        forecolor = ColorPalette.POSTextColor;
                    }
                    break;
                case DiningTableStatus.OrderNotSent:
                    forecolor = ColorPalette.POSWhite;
                    break;
                default:
                    forecolor = ColorPalette.POSTextColor;        
                    break;
            }
            return forecolor;
        }

        public Image Image()
        {
            switch (Details.DiningTableStatus)
            {
                case DiningTableStatus.OrderSent:
                    return Resources.OutlineGrey;
                case DiningTableStatus.OrderStarted:
                    return Resources.HalfGrey;
                case DiningTableStatus.OrderPartiallySent:
                    if (Details.KitchenStatus == KitchenOrderStatusEnum.None)
                    {
                        return Resources.OutlineGrey;
                    }
                    else if (Details.KitchenStatus == KitchenOrderStatusEnum.Started)
                    {
                        return Resources.HalfGrey;
                    }
                    return null;
                default:
                    return null;
            }
        }

        public void Load(TableInfo table)
        {
            if (table == null)
            {
                Clear();
                return;
            }

            Details.NumberOfGuests = table.NumberOfGuests;
            Details.DiningTableStatus = table.DiningTableStatus;
            Details.KitchenStatus = table.KitchenStatus;
            Details.TerminalID = table.TerminalID;
            Details.StaffID = table.StaffID;
            if (Details.CustomerID != table.CustomerID)
            {
                Customer = new Customer();
            }
                
            Details.CustomerID = (string)table.CustomerID == "" ? RecordIdentifier.Empty : table.CustomerID;

            if (string.IsNullOrEmpty(table.TransactionXML))
            {
                Transaction = null;
            }
            else
            {
                Transaction = PosTransaction.CreateTransFromXML(table.TransactionXML, (PosTransaction)Transaction,
                                                                Interfaces.Services.ApplicationService(dataModel)
                                                                        .PartnerObject);
                if ((Transaction is RetailTransaction) &&
                    (((RetailTransaction)Transaction).Hospitality.ActiveHospitalitySalesType == null))
                    ((RetailTransaction)Transaction).Hospitality.ActiveHospitalitySalesType = RecordIdentifier.Empty;
            }
            
        }

        public void Load(List<TableInfo> dineInTableList)
        {
            try
            {
                TableInfo table =
                    dineInTableList.FirstOrDefault(
                        f =>
                        f.StoreID == Details.StoreID && 
                        f.SalesType == Details.SalesType && 
                        f.Sequence == Details.Sequence &&
                        f.TableID == Details.TableID && 
                        f.DiningTableLayoutID == Details.DiningTableLayoutID);

                Load(table);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public TableInfo Save(IConnectionManager entry, ISiteServiceService siteService, string lockToTerminal, string stringlockToStaff)
        {
            Details.TerminalID = lockToTerminal;
            Details.StaffID = stringlockToStaff;

            if (Transaction != null)
            {
                Details.TransactionXML = Transaction.CreateXmlTransaction().ToString();
            }
            else
            {
                Details.TransactionXML = null;
            }

            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            return siteService.SaveHospitalityTableState(entry, settings.HospitalitySiteServiceProfile, this.Details, false);
        }

        public void SaveUnlockedTransaction(IConnectionManager entry, ISiteServiceService siteService, Guid transactionID)
        {
            try
			{
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                siteService.SaveUnlockedTransaction(entry, settings.HospitalitySiteServiceProfile, transactionID, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ExistsUnlockedTransaction(IConnectionManager entry, ISiteServiceService siteService, Guid transactionID)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                siteService.Connect(entry, settings.HospitalitySiteServiceProfile);
                return siteService.ExistsUnlockedTransaction(entry, settings.HospitalitySiteServiceProfile, transactionID, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IPosTransaction GetTransaction()
        {
            if (Transaction == null)
            {
                Transaction = new RetailTransaction(
                    (string)dataModel.CurrentStoreID, 
                    ((ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency, 
                    ((ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).TaxIncludedInPrice);

                Interfaces.Services.TransactionService(dataModel).LoadTransactionStatus(dataModel, Transaction, false, true);
            }

            return Transaction;
        }


    }
}
