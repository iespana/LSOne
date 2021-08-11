using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Core;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Helper;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class TransactionImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        private string RMSConnectionString { get; set; }
        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            RecordIdentifier currentStore = PluginEntry.DataModel.CurrentStoreID;
            RecordIdentifier currentTerminal = PluginEntry.DataModel.CurrentTerminalID;

            PluginEntry.DataModel.DisableReplicationActionCreation = true;
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            try
            {

                RecordIdentifier companyCurrency = (RecordIdentifier)Providers.CompanyInfoData.CompanyCurrencyCode(PluginEntry.DataModel);

                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                DataTable transactionHeaders = entry.Connection.ExecuteReader(Constants.GET_ALL_TRANSACTIONS).ToDataTable();
                DataTable transactionLines = entry.Connection.ExecuteReader(Constants.GET_ALL_TRANSACTION_LINES).ToDataTable();
                DataTable transactionPayments = entry.Connection.ExecuteReader(Constants.GET_ALL_TRANSACTION_PAYMENTS).ToDataTable();
                DataTable taxSystem = entry.Connection.ExecuteReader(Constants.GET_ALL_SYSTEM_TAX).ToDataTable();

                DataTable defaultSystemTax = entry.Connection.ExecuteReader(Constants.GET_SYSTEM_TAX).ToDataTable();
                if (defaultSystemTax.Rows.Count > 0 && defaultSystemTax.Columns.Count > 0)
                {
                    lookupManager.SystemTax = defaultSystemTax.Rows[0][0].ToString().ToInt();
                }

                Dictionary<int, int> taxSystemLookup = new Dictionary<int, int>();
                foreach (DataRow dr in taxSystem.Rows)
                {
                    if (!taxSystemLookup.ContainsKey(dr["StoreID"].ToString().ToInt()))
                    {
                        taxSystemLookup.Add(dr["StoreID"].ToString().ToInt(), dr["TaxSystem"].ToString().ToInt());
                    }
                }

                SetProgressSize(transactionHeaders.Rows.Count);
                foreach (DataRow th in transactionHeaders.Rows)
                {
                    int rmsStoreID = th.Field<int>("RMS_StoreID");

                    if (!lookupManager.StoreLookup.ContainsKey(rmsStoreID))
                    {
                        continue;
                    }
                    try
                    {
                        string transactionNumber = th.Field<string>("TransactionNumber");
                        string storeID = lookupManager.StoreLookup[rmsStoreID].ToString();
                        RetailTransaction transactionHeader = new RetailTransaction(storeID, companyCurrency.ToString(), true);
                        transactionHeader.EntryStatus = DataLayer.BusinessObjects.Enums.TransactionStatus.Normal;
                        transactionHeader.TerminalId = lookupManager.TerminalLookup[th.Field<int>("RMS_TerminalID")].ToString();
                        transactionHeader.BeginDateTime = th.Field<DateTime>("RMSPostingDate");
                        transactionHeader.GrossAmountWithTax = Math.Abs(th.Field<decimal>("TransactionAmount"));
                        transactionHeader.TaxIncludedInPrice = true;
                        transactionHeader.TaxAmount = th.Field<decimal>("VATAmount");
                        transactionHeader.NetAmount = Math.Abs(transactionHeader.GrossAmountWithTax - transactionHeader.TaxAmount);
                        transactionHeader.NetAmountWithTax = transactionHeader.GrossAmountWithTax;
                        transactionHeader.Payment = 0;

                        int? customerID = th.Field<int?>("RMS_CustomerID");
                        if (customerID.HasValue && lookupManager.CustomerLookup.ContainsKey(customerID.Value))
                        {
                            transactionHeader.Customer.ID = lookupManager.CustomerLookup[th.Field<int>("RMS_CustomerID")];
                        }

                        PluginEntry.DataModel.CurrentTerminalID = transactionHeader.TerminalId;
                        PluginEntry.DataModel.CurrentStoreID = transactionHeader.StoreId;

                        Settings newSettings = new Settings();
                        newSettings.Load(PluginEntry.DataModel);
                        newSettings.LoadProfiles(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStaffID);

                        transactionHeader.Cashier.ID = PluginEntry.DataModel.CurrentUser.UserName;
                        transactionHeader.Cashier.Name = PluginEntry.DataModel.CurrentUser.UserName;
                        transactionHeader.Cashier.NameOnReceipt = PluginEntry.DataModel.CurrentUser.UserName;

                        transactionHeader.SaleItems = new LinkedList<ISaleLineItem>();

                        List<DataRow> linkedTransactionLines = transactionLines.AsEnumerable()
                                                           .Where(row => row.Field<string>("TransactionNumber") == transactionNumber && row.Field<int>("RMS_StoreID") == rmsStoreID).ToList();

                        foreach (DataRow item in linkedTransactionLines)
                        {
                            SaleLineItem ti = new SaleLineItem(transactionHeader);

                            ti.LineId = (int)item.Field<long>("LineNumber");
                            ti.DateToActivateItem = new Date(DateTime.MinValue);
                            ti.Found = true;
                            ti.ReturnTerminalId = transactionHeader.TerminalId;
                            ti.ReturnStoreId = transactionHeader.StoreId;
                            ti.Quantity = Math.Abs(item.Field<decimal>("Quantity"));
                            ti.UnitQuantity = ti.Quantity;
                            ti.TaxAmount = item.Field<decimal>("VATAmount");

                            int ts = lookupManager.SystemTax;
                            if (taxSystemLookup.ContainsKey(rmsStoreID))
                            {
                                ts = taxSystemLookup[rmsStoreID];
                            }

                            if (ts == 1)
                            {
                                ti.Price = item.Field<decimal>("UnitPrice");
                                ti.NetAmount = Math.Abs(item.Field<decimal>("Amount"));
                                ti.NetAmountWithTax = ti.NetAmount + ti.TaxAmount;
                                ti.PriceWithTax = ti.Price + (ti.TaxAmount / ti.Quantity);
                                ti.TaxIncludedInItemPrice = false;
                            }
                            else
                            {
                                ti.PriceWithTax = item.Field<decimal>("UnitPrice");
                                ti.NetAmountWithTax = Math.Abs(item.Field<decimal>("Amount"));
                                ti.TaxIncludedInItemPrice = true;
                            }
                            ti.LineDiscount = Math.Abs(item.Field<decimal>("LineDiscountAmount"));

                            ti.Returnable = false;
                            ti.TaxExempt = false;
                            transactionHeader.Payment += ti.NetAmountWithTax;

                            int? rmsItemID = item.Field<int?>("RMS_ItemID");
                            if (rmsItemID.HasValue)
                            {
                                if (lookupManager.StandardItemsLookup.ContainsKey(rmsItemID.Value))
                                {
                                    ti.ItemId = lookupManager.StandardItemsLookup[rmsItemID.Value].Item1.ToString();
                                }
                                if (lookupManager.VariantItemsLookup.ContainsKey(rmsItemID.Value))
                                {
                                    ti.ItemId = lookupManager.VariantItemsLookup[rmsItemID.Value].Item1.ToString();
                                }
                            }
                            if (!string.IsNullOrEmpty(item.Field<string>("UnitOfMeasure")) && lookupManager.UnitOfMeasure.ContainsKey(item.Field<string>("UnitOfMeasure")))
                            {
                                ti.SalesOrderUnitOfMeasure = lookupManager.UnitOfMeasure[item.Field<string>("UnitOfMeasure")].ToString();
                            }
                            else
                            {
                                ti.SalesOrderUnitOfMeasure = lookupManager.DefaultUnitOfMeasureID.ToString();
                            }

                            transactionHeader.SaleItems.AddLast(ti);
                        }

                        transactionHeader.TenderLines = new List<ITenderLineItem>();
                        List<DataRow> linkedTransactionPayments = transactionPayments.AsEnumerable()
                                                           .Where(row => row.Field<string>("TransactionNumber") == transactionNumber && row.Field<int>("RMS_StoreID") == rmsStoreID).ToList();

                        foreach (DataRow item in linkedTransactionPayments)
                        {
                            var tenderType = item.Field<int>("PaymentMethodCode");
                            ITenderLineItem tl = ((TenderTypeEnum)tenderType).CreateTender();

                            string rmsPaymentMethod = item.Field<string>("PaymentMethod");
                            if (!lookupManager.PaymentMethodLookup.ContainsKey(rmsPaymentMethod))
                            {
                                PaymentMethod paymentMethod = new PaymentMethod();
                                paymentMethod.Text = rmsPaymentMethod;
                                paymentMethod.DefaultFunction = 0;
                                Providers.PaymentMethodData.Save(PluginEntry.DataModel, paymentMethod);
                                lookupManager.PaymentMethodLookup.Add(paymentMethod.Text, paymentMethod.ID);
                            }
                            tl.TenderTypeId = lookupManager.PaymentMethodLookup[rmsPaymentMethod].ToString();
                            tl.Transaction = transactionHeader;
                            tl.LineId = (int)item.Field<long>("LineNumber");
                            tl.BeginDateTime = item.Field<DateTime>("RMS_PostingDate");
                            tl.Amount = item.Field<decimal>("Amount");
                            tl.CompanyCurrencyAmount = item.Field<decimal>("AmountSystemCurrency");
                            tl.OpenDrawer = false;
                            tl.ExchrateMST = 100;

                            transactionHeader.TenderLines.Add(tl);
                        }

                        TransactionProviders.PosTransactionData.Save(PluginEntry.DataModel, newSettings, transactionHeader);

                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingTransaction, th.Field<string>("RMS_ID")) + " " + ex.Message });
                        ReportProgress?.Invoke();
                    }
                };
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = ex.Message });
                return logItems;
            }
            finally
            {
                PluginEntry.DataModel.CurrentStoreID = currentStore;
                PluginEntry.DataModel.CurrentTerminalID = currentTerminal;
            }
            return logItems;
        }
    }
}
