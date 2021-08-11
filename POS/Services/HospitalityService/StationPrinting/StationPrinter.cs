using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSRetail.PrintingStationClient;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.BusinessObjects.Profiles;

namespace LSOne.Services.StationPrinting
{
    public class StationPrinter
    {
        public PrintingStation RestaurantStation 
        {
            get { return restaurantStation; }
            set
            {
                restaurantStation = value;
                remoteHost = Providers.StationPrintingHostData.Get(dataModel, value.StationPrintingHostID);
                if (remoteHost != null)
                {
                    stPrintClient = new PrintingStationCli(remoteHost.Address, remoteHost.Port);
                }
            }
        }
        public List<SaleLineItem> ItemsToPrint { get; set; }

        private PrintingStationCli stPrintClient;
        private StationPrintingHost remoteHost;
        private PrintingStation restaurantStation;
        private IConnectionManager dataModel;
        private ISettings settings;

        public StationPrinter(IConnectionManager entry)
        {
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            dataModel = entry;
            RestaurantStation = new PrintingStation();
            ItemsToPrint = new List<SaleLineItem>();            
        }

        /// <summary>
        /// Prints a slip showing which items are being transferred
        /// </summary>
        /// <param name="retailTransaction">The transaction containing the items being transferred.</param>
        /// <param name="fromTableID">The table the items are being transferred from</param>
        /// <param name="fromHospitalityType"></param>
        public void PrintTransferSlip(IConnectionManager entry, IRetailTransaction retailTransaction, int fromTableID, HospitalityType fromHospitalityType)
        {
            string transferSlipString = GetTransferTableString(retailTransaction, fromTableID, fromHospitalityType);

            SendToPrinter(entry, transferSlipString);
        }

        public bool Print(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //Update restaurant station to properly print on receipt
            retailTransaction.Hospitality.RestaurantStation = RestaurantStation.Text;

            //Create transaction only with the selected items (kitchen slip can print partial items from a transaction based on menu types, retail groups etc.)
            IRetailTransaction partialTransaction = (IRetailTransaction)retailTransaction.Clone();
            partialTransaction.SaleItems.Clear();
            ItemsToPrint.ForEach(x => partialTransaction.Add((ISaleLineItem)x.Clone()));

            string textToPrint = Interfaces.Services.PrintingService(entry).GetOPOSPrintString(entry, FormSystemType.KitchenSlip, partialTransaction);
            return SendToPrinter(entry, textToPrint);
        }

        private bool SendToPrinter(IConnectionManager entry, string textToPrint)
        {
            if (ItemsToPrint.Count == 0)
                return true;

            if (RestaurantStation.StationType == PrintingStation.StationTypeEnum.WindowsPrinter && !RecordIdentifier.IsEmptyOrNull(RestaurantStation.WindowsPrinterConfigurationID))
            {
                return WindowsPrint(textToPrint);
            }
            else if (RestaurantStation.StationType == PrintingStation.StationTypeEnum.OPOSPrinter || RestaurantStation.StationType == PrintingStation.StationTypeEnum.HardwareProfilePrinter)
            {
                if (RestaurantStation.StationType == PrintingStation.StationTypeEnum.OPOSPrinter && !string.IsNullOrEmpty(RestaurantStation.StationPrintingHostID.ToString()))
                {
                    return OPOSPrint(textToPrint);
                }
                else
                {
                    return HardwareProfileOPOSPrint(entry, textToPrint);
                }
            }

            return true;
        }

        private bool WindowsPrint(string textToPrint)
        {
            WindowsPrinterConfiguration windowsPrinterConfiguration = Providers.WindowsPrinterConfigurationData.Get(dataModel, RestaurantStation.WindowsPrinterConfigurationID, CacheType.CacheTypeApplicationLifeTime);
            Printer.WindowsPrinting(dataModel, textToPrint, windowsPrinterConfiguration);
            return true;
        }

        private bool HardwareProfileOPOSPrint(IConnectionManager entry, string textToPrint)
        {
            int result = 0;

            Interfaces.Services.DialogService(entry).ShowStatusDialog(Properties.Resources.SendingToKitchenPrinter);
            result = Printer.PrintReceipt(entry,textToPrint, null);
            Interfaces.Services.DialogService(entry).CloseStatusDialog();

            return result == 0;
            
        }

        private bool OPOSPrint(string textToPrint)
        {
            // Connect to the LS Station Printing service running on the remote host
            try
            {

                bool testPreview = remoteHost.Address.ToUpperInvariant() == "PRINTPREVIEW";

                Interfaces.Services.DialogService(dataModel).ShowStatusDialog(Resources.SendingToKitchenPrinter);

                if (testPreview)
                {
                    Interfaces.Services.PrintingService(dataModel).ShowPrintPreview(dataModel, textToPrint, false);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(RestaurantStation.PrinterDeviceName) == false)
                    {
                        if (stPrintClient.StationPrintEx(RestaurantStation.PrinterDeviceName, textToPrint) == false)
                        {
                            Interfaces.Services.DialogService(dataModel).ShowErrorMessage(Resources.PrintError, stPrintClient.GetLastPrintMessage());
                            return false;
                        }

                        stPrintClient.Close();
                    }
                }
            }
            finally
            {
                Interfaces.Services.DialogService(dataModel).CloseStatusDialog();
            }
            
            return true;
           
        }

        private string GetTransferTableString(IRetailTransaction retailTransaction, int fromTableID, HospitalityType fromHospitalityType)
        {
            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');
            const int columnLength = 55;
            int maxChars = 20;

            StringBuilder printLayout = new StringBuilder();

            HospitalityType activeHospitalityType = Providers.HospitalityTypeData.Get(dataModel,
                                                                            dataModel.CurrentStoreID,
                                                                            retailTransaction.Hospitality.ActiveHospitalitySalesType);

            string fromHospiTalityTypeNameSuffix = fromHospitalityType != null ? " - " + fromHospitalityType.Text : "";
            string toHospiTalityTypeNameSuffix = activeHospitalityType != null ? " - " + activeHospitalityType.Text : "";

            printLayout.AppendLine();
            printLayout.AppendLine(Properties.Resources.TransferTableSlip);
            printLayout.AppendLine(doubleLine);

            printLayout.AppendLine(Properties.Resources.FromTable.PadRight(maxChars, '.') + ": " + fromTableID + fromHospiTalityTypeNameSuffix);
            printLayout.AppendLine(Properties.Resources.ToTable.PadRight(maxChars, '.') + ": " + retailTransaction.Hospitality.TableInformation.TableID + toHospiTalityTypeNameSuffix);
            printLayout.AppendLine();

            string staff = (string)retailTransaction.Cashier.Login;
            
            if ((activeHospitalityType != null) &&
                (activeHospitalityType.TableButtonStaffDescription == HospitalityType.TableButtonStaffDescriptionEnum.ReceiptName))
            {                
                staff = retailTransaction.Cashier.NameOnReceipt;
            }
            
            printLayout.AppendLine(Properties.Resources.OperatorID.PadRight(maxChars, '.') + ": " + staff);            
            printLayout.AppendLine(Properties.Resources.Date.PadRight(maxChars, '.') + ": " + retailTransaction.BeginDateTime.ToShortDateString());
            printLayout.AppendLine(Properties.Resources.Time.PadRight(maxChars, '.') + ": " + DateTime.Now.ToShortTimeString());
            printLayout.AppendLine();

            List<string> MenuTypes = new List<string>();
            string toDisplay = "";

            foreach (SaleLineItem line in ItemsToPrint.Where(w => w.MenuTypeItem.Description != ""))
            {
                int cnt = MenuTypes.Count(f => f == line.MenuTypeItem.Description);
                if (cnt != 0)
                    continue;

                MenuTypes.Add(line.MenuTypeItem.Description);
                if (toDisplay != "")
                {
                    toDisplay += ", ";
                }
                toDisplay += line.MenuTypeItem.Description;
            }

            if (ItemsToPrint.Count(f => f.MenuTypeItem.Description.Trim() == "") > 0)
            {
                if (toDisplay != "")
                {
                    toDisplay += ", ";
                }
                toDisplay += Resources.NoMenuTypes;
            }

            if (toDisplay != "")
            {
                printLayout.AppendLine(Properties.Resources.MenuTypes.PadRight(maxChars, '.') + ": " + toDisplay);
            }

            printLayout.AppendLine();

            string tempStr = Resources.Items;
            string tempStr2 = Resources.Qty;
            tempStr = tempStr.PadRight(columnLength - tempStr2.Length, ' ');
            tempStr += tempStr2;
            printLayout.AppendLine(tempStr);
            printLayout.AppendLine(singleLine);

            IRoundingService rounding = (IRoundingService)dataModel.Service(ServiceType.RoundingService);

            foreach (SaleLineItem line in ItemsToPrint)
            {

                tempStr = line.Description;

                if (line.Voided == false && retailTransaction.EntryStatus != TransactionStatus.Voided)
                {
                    tempStr += (line.ChangedForPreparation) ? "  " + Resources.ChangedItem : "";
                    tempStr2 = rounding.RoundQuantity(
                        dataModel,
                        line.Quantity,
                        line.SalesOrderUnitOfMeasure,
                        ((ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency,
                        CacheType.CacheTypeTransactionLifeTime);
                }
                else
                {
                    tempStr2 = Resources.Voided;
                }

                if (line.IsInfoCodeItem || line.IsLinkedItem)
                {
                    tempStr = " " + Resources.PrefixModifier + " " + tempStr;
                }

                printLayout.AppendLine(tempStr.PadRight(columnLength - tempStr2.Length, ' ') + tempStr2);

                if (!string.IsNullOrEmpty(line.Comment))
                {
                    string separator = "\r\n";
                    string[] comments = line.Comment.Split(separator.ToCharArray());
                    foreach (string comment in comments)
                    {
                        if (!string.IsNullOrEmpty(comment))
                            printLayout.AppendLine(Properties.Resources.PrefixComments + " " + comment);
                    }
                }

                if (line.Dimension.Exists())
                {
                    printLayout.AppendLine(Properties.Resources.PrefixComments + " " + line.Dimension.Description);
                }
            }

            printLayout.AppendLine();
            printLayout.AppendLine(doubleLine);
            printLayout.AppendLine();
            printLayout.AppendLine();
            printLayout.AppendLine();

            return printLayout.ToString();
        }
    }
}
