using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class TableInfoData : SqlServerDataProviderBase, ITableInfoData
    {
        protected string BaseSQL
        {
            get
            {
                return @"SELECT RESTAURANTID
                        ,SALESTYPE
                        ,DINEINTABLENO
                        ,SEQUENCE
                        ,DININGTABLELAYOUTID
                        ,ISNULL(DESCRIPTION, '') AS DESCRIPTION
                        ,ISNULL(NOOFGUESTS, 0) AS NOOFGUESTS
                        ,ISNULL(STAFFID, '') AS STAFFID
                        ,ISNULL(STATUS, 0) AS STATUS
                        ,ISNULL(TRANSACTIONXML, '') AS TRANSACTIONXML
                        ,DATAAREAID
                        ,ISNULL(TERMINALID, '') AS TERMINALID
                        ,ISNULL(CUSTOMERID, '') AS CUSTOMERID
                        ,KITCHENSTATUS
                        FROM POSISHOSPITALITYDININGTABLES ";
            }
        }

        public virtual List<TableInfo> GetListForTerminal(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText = BaseSQL +
                                    @" WHERE TERMINALID = @TERMINALID
                                       AND DATAAREAID = @DATAAREAID
                                       AND RESTAURANTID = @STOREID";

                MakeParam(cmd, "TERMINALID", (string)terminalID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeID);

                return Execute<TableInfo>(entry, cmd, CommandType.Text, PopulateDineInTable);
            }
        }

        public virtual List<TableInfo> GetList(IConnectionManager entry, DiningTableLayout tableLayout)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                    @" WHERE RESTAURANTID = @RESTAURANTID 
                                       AND SALESTYPE = @SALESTYPE 
                                       AND SEQUENCE = @SEQUENCE
                                       AND DININGTABLELAYOUTID = @DININGTABLELAYOUTID 
                                       AND DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "RESTAURANTID", (string)tableLayout.RestaurantID);
                MakeParam(cmd, "SALESTYPE", (string)tableLayout.SalesType);
                MakeParam(cmd, "SEQUENCE", (int)tableLayout.Sequence, SqlDbType.Int);
                MakeParam(cmd, "DININGTABLELAYOUTID", (string)tableLayout.LayoutID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<TableInfo>(entry, cmd, CommandType.Text, PopulateDineInTable);
            }
        }

        public virtual TableInfo RefreshTableInfo(IConnectionManager entry, TableInfo table)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                    @"WHERE RESTAURANTID=@RESTAURANTID 
                                    AND SALESTYPE=@SALESTYPE 
                                    AND SEQUENCE=@SEQUENCE
                                    AND DININGTABLELAYOUTID=@DININGTABLELAYOUTID 
                                    AND DINEINTABLENO=@DINEINTABLENO
                                    AND DATAAREAID=@DATAAREAID ";

                MakeParam(cmd, "RESTAURANTID", table.StoreID);
                MakeParam(cmd, "SALESTYPE", table.SalesType);
                MakeParam(cmd, "SEQUENCE", table.Sequence, SqlDbType.Int);
                MakeParam(cmd, "DININGTABLELAYOUTID", table.DiningTableLayoutID);
                MakeParam(cmd, "DINEINTABLENO", table.TableID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                var list = Execute<TableInfo>(entry, cmd, CommandType.Text, PopulateDineInTable);

                return list.Count > 0 ? list[0] : null;
            }
        }

        private static void PopulateDineInTable(IDataReader reader, TableInfo dineInTable)
        {
            dineInTable.StoreID = (string)reader["RESTAURANTID"];
            dineInTable.SalesType = (string)reader["SALESTYPE"];
            dineInTable.Sequence = (int)reader["SEQUENCE"];
            dineInTable.TableID = (int)reader["DINEINTABLENO"];
            dineInTable.DiningTableLayoutID = (string)reader["DININGTABLELAYOUTID"];
            dineInTable.Description = (string)reader["DESCRIPTION"];
            dineInTable.CustomerID = (string)reader["CUSTOMERID"];

            if (reader["NOOFGUESTS"] == null || reader["NOOFGUESTS"] == DBNull.Value)
            {
                dineInTable.NumberOfGuests = 0;
            }
            else
            {
                dineInTable.NumberOfGuests = (int)reader["NOOFGUESTS"];
            }

            dineInTable.DiningTableStatus = (DiningTableStatus)(int)reader["STATUS"];
            dineInTable.KitchenStatus = (KitchenOrderStatusEnum)(byte)reader["KITCHENSTATUS"];

            if (reader["TERMINALID"] == null || reader["TERMINALID"] == DBNull.Value)
            {
                dineInTable.TerminalID = "";
            }
            else
            {
                dineInTable.TerminalID = (string)reader["TERMINALID"];
            }

            if (reader["STAFFID"] == null || reader["STAFFID"] == DBNull.Value)
            {
                dineInTable.StaffID = "";
            }
            else
            {
                dineInTable.StaffID = (string)reader["STAFFID"];
            }

            if (reader["TRANSACTIONXML"] == null || reader["TRANSACTIONXML"] == DBNull.Value)
            {
                dineInTable.TransactionXML = "";
            }
            else
            {
                dineInTable.TransactionXML = (string)reader["TRANSACTIONXML"];
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier resturantID, RecordIdentifier salesType, RecordIdentifier sequence, int dineInTableNumber, RecordIdentifier dineInTableLayoutID)
        {
            return RecordExists(entry, "POSISHOSPITALITYDININGTABLES", new[] { "RESTAURANTID", "SALESTYPE", "SEQUENCE", "DINEINTABLENO","DININGTABLELAYOUTID" }, new RecordIdentifier(resturantID,new RecordIdentifier(salesType, new RecordIdentifier(sequence, new RecordIdentifier(dineInTableNumber,dineInTableLayoutID)))));
        }

        public virtual void Save(IConnectionManager entry, TableInfo diningTable)
        {
            var statement = new SqlServerStatement("POSISHOSPITALITYDININGTABLES");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiningTableLayouts);

            if (
                !Exists(entry, diningTable.StoreID, diningTable.SalesType, diningTable.Sequence, diningTable.TableID,
                        diningTable.DiningTableLayoutID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RESTAURANTID", diningTable.StoreID);
                statement.AddKey("SALESTYPE", diningTable.SalesType);
                statement.AddKey("DINEINTABLENO", diningTable.TableID, SqlDbType.Int);
                statement.AddKey("SEQUENCE", diningTable.Sequence, SqlDbType.Int);
                statement.AddKey("DININGTABLELAYOUTID", diningTable.DiningTableLayoutID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("RESTAURANTID", diningTable.StoreID);
                statement.AddCondition("SALESTYPE", diningTable.SalesType);
                statement.AddCondition("DINEINTABLENO", diningTable.TableID, SqlDbType.Int);
                statement.AddCondition("SEQUENCE", diningTable.Sequence, SqlDbType.Int);
                statement.AddCondition("DININGTABLELAYOUTID", diningTable.DiningTableLayoutID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            if (diningTable.Description != null)
            {
                statement.AddField("DESCRIPTION", diningTable.Description);
            }
            else
            {
                statement.AddField("DESCRIPTION", DBNull.Value, SqlDbType.NVarChar);
            }
            statement.AddField("NOOFGUESTS", diningTable.NumberOfGuests, SqlDbType.Int);
            statement.AddField("CUSTOMERID", (string)diningTable.CustomerID);

            if (diningTable.StaffID != null)
            {
                statement.AddField("STAFFID", diningTable.StaffID);
            }
            else
            {
                statement.AddField("STAFFID", DBNull.Value, SqlDbType.NVarChar);
            }

            if (diningTable.TerminalID != null)
            {
                statement.AddField("TERMINALID", diningTable.TerminalID);
            }
            else
            {
                statement.AddField("TERMINALID", DBNull.Value, SqlDbType.NVarChar);
            }

            statement.AddField("STATUS", diningTable.DiningTableStatus, SqlDbType.Int);
            statement.AddField("KITCHENSTATUS", diningTable.KitchenStatus, SqlDbType.TinyInt);

            if (diningTable.TransactionXML != null)
            {
                statement.AddField("TRANSACTIONXML", diningTable.TransactionXML, SqlDbType.Xml);
            }
            else
            {
                statement.AddField("TRANSACTIONXML", DBNull.Value, SqlDbType.Xml);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool ExistsUnlockedTransaction(IConnectionManager entry, Guid transactionID)
        {
            return RecordExists(entry, "POSTEDUNLOCKEDTRANSACTIONS", "ID", transactionID, false);
        }

        public virtual void SaveUnlockedTransaction(IConnectionManager entry, Guid transactionID)
        {
            var statement = new SqlServerStatement("POSTEDUNLOCKEDTRANSACTIONS");

            ValidateSecurity(entry, BusinessObjects.Permission.HospitalityUnlockTable);

            if (!ExistsUnlockedTransaction(entry, transactionID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", transactionID, SqlDbType.UniqueIdentifier);
            }

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
