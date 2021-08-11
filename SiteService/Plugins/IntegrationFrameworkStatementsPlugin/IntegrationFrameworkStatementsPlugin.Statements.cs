using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkStatementsPlugin
{
    public partial class IntegrationFrameworkStatementsPlugin
    {
        private SiteServiceProfile siteServiceProfile;

        public virtual List<StatementInfo> CreateStatements(bool allowPostingWithoutEodTransaction)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    dataModel.ServiceFactory = new DataLayer.GenericConnector.ServiceFactory();

                    List<StatementInfo> results = new List<StatementInfo>();
                    List<Store> stores = Providers.StoreData.GetStores(dataModel);

                    foreach (Store s in stores)
                    {
                        StatementInfo statement = CreateStatementForStore(dataModel, s.ID,
                            allowPostingWithoutEodTransaction);

                        if (statement != null)
                        {
                            results.Add(statement);
                        }
                    }

                    return results;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual StatementInfo CreateStatement(RecordIdentifier storeID, bool allowPostingWithoutEodTransaction)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    dataModel.ServiceFactory = new DataLayer.GenericConnector.ServiceFactory();

                    return Providers.StoreData.Exists(dataModel, storeID)
                        ? CreateStatementForStore(dataModel, storeID, allowPostingWithoutEodTransaction)
                        : null;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual List<StatementInfo> GetStatements(DateTime date)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    return Providers.StatementInfoData.GetPostedStatementsAfterDate(dataModel, date);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual List<StatementInfo> GetStatementsForStore(DateTime date,
            RecordIdentifier storeID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.StatementInfoData.GetPostedStatementsAfterDate(dataModel, date, storeID);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        protected virtual StatementInfo CreateStatementForStore(IConnectionManager entry, RecordIdentifier storeID, bool allowPostingWithoutEodTransaction)
        {
           
            StatementInfo statement = new StatementInfo
            {
                ID = RecordIdentifier.Empty,
                StoreID = storeID,
                EndingTime = DateTime.Now,
                PostingDate = Date.Now,
                FromPeriodType = StatementPeriodTypeEnum.LastStatementEnd,
                ToPeriodType = StatementPeriodTypeEnum.EndOfDay,
                FromType = StatementPeriodFormEnum.Period,
                ToType = StatementPeriodFormEnum.Manual,
                CalculatedTime = DateTime.Now
            };

            StatementInfo lastStatement = Providers.StatementInfoData.GetLastStatement(entry, storeID);
            if(lastStatement != null)
            {
                statement.StartingTime = lastStatement.EndingTime;
            }
            else
            {
                statement.FromPeriodType = StatementPeriodTypeEnum.EndOfDay;
                List<Transaction> transactions = Providers.TransactionData.GetTransactionsFromType(entry, storeID, TypeOfTransaction.EndOfDay, true);

                if(transactions.Count > 0)
                {
                    statement.StartingTime = transactions[0].TransactionDate.DateTime;
                }
                else
                {
                    //If we get here we don't have any statement or transaction from which 
                    // to get the starting time and we cannot continue the process
                    return null;
                }
            }

            IEndOfDayBackOfficeService service = Services.Interfaces.Services.EndOfDayBOService(entry);
            if (service == null) return null;

            Providers.StatementInfoData.Save(entry, statement);

            try
            {
                service.CalculateStatement(entry, statement.ID, statement.StoreID, statement.StartingTime, statement.EndingTime);

                List<AllowEODEnums> statementFlags = service.AllowedToPostStatement(entry, statement);

                if (!statementFlags.Contains(AllowEODEnums.DisallowEodMarkMissingOnTerminal) || allowPostingWithoutEodTransaction)
                {
                    service.PostStatement(entry, statement.ID, statement.StoreID, DateTime.Now.Date);
                    UpdateCustomerLedger(entry, statement.ID);
                    StatementInfo createdStatement = Providers.StatementInfoData.Get(entry, statement.ID);
                    return createdStatement;
                }
                else
                {
                    Providers.StatementInfoData.Delete(entry, statement.ID, storeID);
                    return null;
                }
            }
            catch
            {
                Providers.StatementInfoData.Delete(entry, statement.ID, storeID);
                return null;
            }
        }

        protected virtual void UpdateCustomerLedger(IConnectionManager entry, RecordIdentifier statementID)
        {
            if(siteServiceProfile == null)
            {
                Parameters paramsData = Providers.ParameterData.Get(entry);
                siteServiceProfile = Providers.SiteServiceProfileData.Get(entry, paramsData.SiteServiceProfile);
            }

            Services.Interfaces.Services.SiteServiceService(entry).UpdateCustomerLedgerAtEOD(entry, siteServiceProfile, statementID, (siteServiceProfile == null || siteServiceProfile.CheckCustomer));
        }
    }
}
