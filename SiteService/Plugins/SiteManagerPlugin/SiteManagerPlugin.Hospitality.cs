using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual List<TableInfo> LoadHospitalityTableState(DiningTableLayout tableLayout, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.TableInfoData.GetList(dataModel, tableLayout);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        public virtual TableInfo SaveHospitalityTableState(TableInfo table, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
				Utils.Log(this, Utils.Starting);

                if(table.TerminalID != "") //We want to lock to a terminal
                {
                    TableInfo currentTableInfo = Providers.TableInfoData.RefreshTableInfo(dataModel, table);

                    if(currentTableInfo != null && currentTableInfo.TerminalID != "" && currentTableInfo.TerminalID != table.TerminalID)
                    {
                        //Locked by another terminal
                        return currentTableInfo;
                    }
                }

                Providers.TableInfoData.Save(dataModel, table);
                return table;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        public virtual void SaveUnlockedTransaction(Guid transactionID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.TableInfoData.SaveUnlockedTransaction(dataModel, transactionID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual bool ExistsUnlockedTransaction(Guid transactionID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.TableInfoData.ExistsUnlockedTransaction(dataModel, transactionID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return false;
        }

        public virtual TableInfo LoadSpecificTableState(TableInfo table, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.TableInfoData.RefreshTableInfo(dataModel, table);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        public virtual void ClearTerminalLocks(string terminalID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(terminalID)}: {terminalID}");
                List<TableInfo> tables = Providers.TableInfoData.GetListForTerminal(dataModel, terminalID, dataModel.CurrentStoreID);
                if (tables != null && tables.Any())
                {
                    foreach (TableInfo tableInfo in tables)
                    {
                        tableInfo.TerminalID = "";
                        Providers.TableInfoData.Save(dataModel, tableInfo);
                    }
                }
                else
                {
                    Utils.Log(this, "No tables found");
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}