using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    public partial class PrintingService
    {

        protected virtual FormInfo CreateCodedVoidedTransactionForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings)
        {
            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            StringBuilder reportLayout = new StringBuilder();

            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.VoidedTransactionReceipt);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(singleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.VoidedTransactionID + posTransaction.TransactionId);

            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            formInfo.Header = reportLayout.ToString();

            return formInfo;
        }

        protected virtual StringBuilder CreateCodedSuspendTransactionPrintPrefix(IConnectionManager entry, RetailTransaction posTransaction)
        {
            var reportLayout = new StringBuilder();

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            reportLayout.AppendLine();
            reportLayout.AppendLine("<TB: " + posTransaction.TransactionId + ">" + "\r\n");
            reportLayout.AppendLine(Properties.Resources.SuspendedTransaction);
            reportLayout.AppendLine(Properties.Resources.SuspendDestination + ": " + posTransaction.SuspendDestination);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            if (posTransaction.SuspendTransactionAnswers.Count > 0)
            {
                reportLayout.AppendLine(Properties.Resources.AdditionalInformation);
                reportLayout.AppendLine();

                if (entry.Settings.LocalizationContext.CultureInfo == null)
                {
                    entry.Settings.LocalizationContext.CultureInfo = settings.CultureInfo;
                }

                foreach (SuspendedTransactionAnswer ans in posTransaction.SuspendTransactionAnswers)
                {
                    string descr = ans.ToString(entry.Settings.LocalizationContext);
                    if (ans.InformationType == SuspensionTypeAdditionalInfo.InfoTypeEnum.Date)
                    {
                        descr = Convert.ToDateTime(descr, settings.CultureInfo.DateTimeFormat).ToShortDateString();
                    }

                    string str = ans.Prompt + ": " + descr;
                    do
                    {
                        if (str.Length < 55)
                        {
                            reportLayout.AppendLine(str);
                            str = "";
                            reportLayout.AppendLine();
                        }
                        else
                        {
                            string substr = str.Substring(0, 55);
                            reportLayout.AppendLine(substr);
                            str = str.Substring(55, str.Length - 55);
                        }
                    }
                    while (str != "");
                }
                reportLayout.AppendLine();
                reportLayout.AppendLine(doubleLine);
            }

            reportLayout.AppendLine();

            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();

            return reportLayout;
        }

        protected virtual FormInfo CreatedCodedSafeDropForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings)
        {
            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            //string tenderTypeId = "";
            string tenderName = "";
            string amount = "";
            string currencyCode = "";
            string foreignCurrAmount = "";

            var reportLayout = new StringBuilder();
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.SafeDropReceipt);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(Properties.Resources.TerminalIDWithPostFix + posTransaction.TerminalId.PadLeft(7, ' '));
            reportLayout.AppendLine(singleLine);
            reportLayout.AppendLine();

            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (TenderLineItem tenderLine in ((SafeDropTransaction)posTransaction).TenderLines)
            {
                if (tenderLine.CurrencyCode == posTransaction.StoreCurrencyCode)
                {
                    // Tenders in the store currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true,
                        CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString("", tenderCol2));
                    reportLayout.Append(FormatString("", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
                else
                {
                    // Foreign currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true,
                        CacheType.CacheTypeTransactionLifeTime);

                    foreignCurrAmount = rounding.RoundString(entry, tenderLine.ForeignCurrencyAmount,
                        tenderLine.CurrencyCode, true, CacheType.CacheTypeTransactionLifeTime);
                    currencyCode = tenderLine.CurrencyCode;

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString(currencyCode, tenderCol2));
                    reportLayout.Append(FormatStringLEFT(foreignCurrAmount + "; ", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
            }

            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            formInfo.Header = reportLayout.ToString();

            return formInfo;
        }

        protected virtual FormInfo CreateCodedGiftCertificateForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings, IGiftCertificateItem giftCertificateItem, bool copyReceipt)
        {
            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            var reportLayout = new StringBuilder();

            reportLayout.AppendLine();

            if (copyReceipt)
            {
                reportLayout.AppendLine(Properties.Resources.Copy);
                reportLayout.AppendLine();
            }

            reportLayout.AppendLine(Properties.Resources.GiftCard);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(singleLine);
            reportLayout.AppendLine();

            reportLayout.AppendLine(Properties.Resources.GiftCardNo + "  " + giftCertificateItem.SerialNumber);

            reportLayout.AppendLine(Properties.Resources.GiftCardAmount + "  " + rounding.RoundForDisplay(
                entry,
                giftCertificateItem.Amount,
                true,
                true,
                settings.Store.Currency,
                CacheType.CacheTypeTransactionLifeTime));

            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);

            formInfo.Header = reportLayout.ToString();

            return formInfo;
        }


        protected virtual FormInfo CreateSaveDropReversalForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings)
        {

            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            //string tenderTypeId = "";
            string tenderName = "";
            string amount = "";
            string currencyCode = "";
            string foreignCurrAmount = "";

            StringBuilder reportLayout = new StringBuilder();
            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.SafeDropReversalReceipt);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(Properties.Resources.TerminalIDWithPostFix + posTransaction.TerminalId.PadLeft(7, ' '));
            reportLayout.AppendLine(singleLine);
            reportLayout.AppendLine();

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (TenderLineItem tenderLine in ((SafeDropReversalTransaction)posTransaction).TenderLines)
            {
                if (tenderLine.CurrencyCode == posTransaction.StoreCurrencyCode)
                {
                    // Tenders in the store currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true,
                        CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString("", tenderCol2));
                    reportLayout.Append(FormatString("", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
                else
                {
                    // Foreign currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
                    foreignCurrAmount = rounding.RoundString(entry, tenderLine.ForeignCurrencyAmount, tenderLine.CurrencyCode, true, CacheType.CacheTypeTransactionLifeTime);
                    currencyCode = tenderLine.CurrencyCode;

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString(currencyCode, tenderCol2));
                    reportLayout.Append(FormatStringLEFT(foreignCurrAmount + "; ", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
            }

            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            formInfo.Header = reportLayout.ToString();

            return formInfo;
        }

        protected virtual FormInfo CreateCodedBankDropForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings)
        {
            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            StringBuilder reportLayout = new StringBuilder();

            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.BankDropReceipt);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(Properties.Resources.TerminalIDWithPostFix + posTransaction.TerminalId.PadLeft(7, ' '));
            reportLayout.Append(Properties.Resources.BagID + "  " + ((BankDropTransaction)posTransaction).BankBagNo);
            reportLayout.AppendLine();
            reportLayout.Append(singleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            //string tenderTypeId = "";
            string tenderName = "";
            string amount = "";
            string currencyCode = "";
            string foreignCurrAmount = "";

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (TenderLineItem tenderLine in ((BankDropTransaction)posTransaction).TenderLines)
            {
                if (tenderLine.CurrencyCode == posTransaction.StoreCurrencyCode)
                {
                    // Tenders in the store currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString("", tenderCol2));
                    reportLayout.Append(FormatString("", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
                else
                {
                    // Foreign currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
                    currencyCode = tenderLine.CurrencyCode;
                    foreignCurrAmount = rounding.RoundString(entry, tenderLine.ForeignCurrencyAmount, tenderLine.CurrencyCode, true, CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString(currencyCode, tenderCol2));
                    reportLayout.Append(FormatStringLEFT(foreignCurrAmount + "; ", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
            }

            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            formInfo.Header = reportLayout.ToString();

            return formInfo;
        }

        protected virtual FormInfo CreateCodedBankDropReversalForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings)
        {
            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            StringBuilder reportLayout = new StringBuilder();

            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.BankDropReversalReceipt);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(Properties.Resources.TerminalIDWithPostFix + posTransaction.TerminalId.PadLeft(7, ' '));
            reportLayout.Append(Properties.Resources.BagID + "  " + ((BankDropReversalTransaction)posTransaction).BankBagNo);
            reportLayout.AppendLine();
            reportLayout.Append(singleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            //string tenderTypeId = "";
            string tenderName = "";
            string amount = "";
            string currencyCode = "";
            string foreignCurrAmount = "";

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (TenderLineItem tenderLine in ((BankDropReversalTransaction)posTransaction).TenderLines)
            {
                if (tenderLine.CurrencyCode == posTransaction.StoreCurrencyCode)
                {
                    // Tenders in the store currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString("", tenderCol2));
                    reportLayout.Append(FormatString("", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
                else
                {
                    // Foreign currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
                    currencyCode = tenderLine.CurrencyCode;
                    foreignCurrAmount = rounding.RoundString(entry, tenderLine.ForeignCurrencyAmount, tenderLine.CurrencyCode, true, CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString(currencyCode, tenderCol2));
                    reportLayout.Append(FormatStringLEFT(foreignCurrAmount + "; ", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
            }

            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            formInfo.Header = reportLayout.ToString();

            return formInfo;
        }

        protected virtual FormInfo CreateCodedTenderDeclarationForm(IConnectionManager entry, IPosTransaction posTransaction, ISettings settings)
        {
            FormInfo formInfo = CreateFormInfo(entry, settings);

            string doubleLine = "";
            doubleLine = doubleLine.PadLeft(55, '=');
            string singleLine = "";
            singleLine = singleLine.PadLeft(55, '-');

            StringBuilder reportLayout = new StringBuilder();

            reportLayout.AppendLine();
            reportLayout.AppendLine(Properties.Resources.TenderDeclarationReceipt);
            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);

            reportLayout.Append(Properties.Resources.OperatorIDWithPostFix + ((string)posTransaction.Cashier.Login).PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.DateWithPostFix + posTransaction.EndDateTime.ToShortDateString().PadLeft(10, ' '));
            reportLayout.Append(Properties.Resources.StoreIDWithPostFix + posTransaction.StoreId.PadLeft(7, ' '));
            reportLayout.Append(spaceString);
            reportLayout.AppendLine(Properties.Resources.TimeWithPostFix + posTransaction.EndDateTime.ToShortTimeString().PadLeft(10, ' '));
            reportLayout.AppendLine(singleLine);
            reportLayout.AppendLine();

            //string tenderTypeId = "";
            string tenderName = "";
            string amount = "";
            string currencyCode = "";
            string foreignCurrAmount = "";

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (TenderLineItem tenderLine in ((TenderDeclarationTransaction)posTransaction).TenderLines)
            {
                if (tenderLine.CurrencyCode == posTransaction.StoreCurrencyCode)
                {
                    // Tenders in the store currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString("", tenderCol2));
                    reportLayout.Append(FormatString("", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
                else
                {
                    // Foreign currency
                    //tenderTypeId = tenderLine.TenderTypeId;
                    tenderName = tenderLine.Description;
                    amount = rounding.RoundString(entry, tenderLine.Amount, settings.Store.Currency, true);
                    foreignCurrAmount = rounding.RoundString(entry, tenderLine.ForeignCurrencyAmount, tenderLine.CurrencyCode, true, CacheType.CacheTypeTransactionLifeTime);
                    currencyCode = tenderLine.CurrencyCode;

                    reportLayout.Append(FormatString(tenderName + ":", tenderCol1));
                    reportLayout.Append(FormatString(currencyCode, tenderCol2));
                    reportLayout.Append(FormatStringLEFT(foreignCurrAmount + "; ", tenderCol3));
                    reportLayout.Append(FormatString(storeCurrencyCode, tenderCol4));
                    reportLayout.AppendLine(FormatStringLEFT(amount, tenderCol5));
                }
            }

            reportLayout.AppendLine();
            reportLayout.AppendLine(doubleLine);
            reportLayout.AppendLine();
            reportLayout.AppendLine();

            formInfo.Header = reportLayout.ToString();
            return formInfo;
        }

        protected virtual FormInfo CreateFormInfo(IConnectionManager entry, ISettings settings)
        {
            return new FormInfo(settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.Windows,
                                settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.Windows ? settings.HardwareProfile.WindowsPrinterConfiguration.PrinterDeviceName : "",
                                false,
                                PrintBehaviors.AlwaysPrint,
                                56);
        }
    }
}
