using System;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class HospitalitySetupData : SqlServerDataProviderBase, IHospitalitySetupData
    {
        private static void PopulateHospitalitySetup(IDataReader dr, HospitalitySetup setup)
        {
            setup.ID = (string)dr["SETUP"];

            setup.DeliverySalesType = (string)dr["DELIVERYSALESTYPE"];
            setup.DineInSalesType = (string)dr["DINEINSALESTYPE"];

            setup.OrderProcessTimeMin = (int)dr["ORDERPROCESSTIMEMIN"];

            setup.TableFreeColorB = (int)dr["TABLEFREECOLORB"];
            setup.TableNotAvailColorB = (int)dr["TABLENOTAVAILCOLORB"];
            setup.TableLockedColorB = (int)dr["TABLELOCKEDCOLORB"];
            setup.OrderNotPrintedColorB = (int)dr["ORDERNOTPRINTEDCOLORB"];
            setup.OrderPrintedColorB = (int)dr["ORDERPRINTEDCOLORB"];
            setup.OrderStartedColorB = (int)dr["ORDERSTARTEDCOLORB"];
            setup.OrderFinishedColorB = (int)dr["ORDERFINISHEDCOLORB"];
            setup.OrderConfirmedColorB = (int)dr["ORDERCONFIRMEDCOLORB"];

            setup.TableFreeColorF = (string)dr["TABLEFREECOLORF"];
            setup.TableNotAvailColorF = (string)dr["TABLENOTAVAILCOLORF"];
            setup.TableLockedColorF = (string)dr["TABLELOCKEDCOLORF"];
            setup.OrderNotPrintedColorF = (string)dr["ORDERNOTPRINTEDCOLORF"];
            setup.OrderPrintedColorF = (string)dr["ORDERPRINTEDCOLORF"];
            setup.OrderStartedColorF = (string)dr["ORDERSTARTEDCOLORF"];
            setup.OrderFinishedColorF = (string)dr["ORDERFINISHEDCOLORF"];
            setup.OrderConfirmedColorF = (string)dr["ORDERCONFIRMEDCOLORF"];

            setup.ConfirmStationPrinting = ((byte)dr["CONFIRMSTATIONPRINTING"] != 0);
            setup.RequestNoOfGuests = ((byte)dr["REQUESTNOOFGUESTS"] != 0);

            setup.NoOfDineInTablesCol = (int)dr["NOOFDINEINTABLESCOL"];
            setup.NoOfDineInTablesRows = (int)dr["NOOFDINEINTABLESROWS"];
            setup.StationPrinting = (HospitalitySetup.SetupStationPrinting)dr["STATIONPRINTING"];
            setup.DineInTableLocking = (HospitalitySetup.SetupDineInTableLocking)dr["DINEINTABLELOCKING"];
            setup.DineInTableSelection = (HospitalitySetup.SetupDineInTableSelection)dr["DINEINTABLESELECTION"];

            setup.Period1TimeFrom = GetTimeStamp(dr, "PERIOD1TIMEFROM");
            setup.Period1TimeTo = GetTimeStamp(dr, "PERIOD1TIMETO");
            setup.Period2TimeFrom = GetTimeStamp(dr, "PERIOD2TIMEFROM");
            setup.Period2TimeTo = GetTimeStamp(dr, "PERIOD2TIMETO");
            setup.Period3TimeFrom = GetTimeStamp(dr, "PERIOD3TIMEFROM");
            setup.Period3TimeTo = GetTimeStamp(dr, "PERIOD3TIMETO");
            setup.Period4TimeFrom = GetTimeStamp(dr, "PERIOD4TIMEFROM");
            setup.Period4TimeTo = GetTimeStamp(dr, "PERIOD4TIMETO");

            setup.AutoLogoffAtPOSExit = ((byte)dr["AUTOLOGOFFATPOSEXIT"] != 0);
            setup.TakeOutSalesType = (string)dr["TAKEOUTSALESTYPE"];
            setup.PreOrderSalesType = (string)dr["PREORDERSALESTYPE"];
            setup.LogStationPrinting = ((byte)dr["LOGSTATIONPRINTING"] != 0);
            setup.PopulateDeliveryInfocodes = ((byte)dr["POPULATEDELIVERYINFOCODES"] != 0);
            setup.AllowPreOrders = ((byte)dr["ALLOWPREORDERS"] != 0);
            setup.TakeoutNoNameNo = (string)dr["TAKEOUTNONAMENO"];
            setup.AdvPreOrdPrintMin = (int)dr["ADVPREORDPRINTMIN"];
            setup.CloseTripOnDepart = ((byte)dr["CLOSETRIPONDEPART"] != 0);
            setup.DelProgressStatusInUse = ((byte)dr["DELPROGRESSSTATUSINUSE"] != 0);

            setup.DaysBOMPrintExist = (int)dr["DAYSBOMPRINTEXIST"];
            setup.DaysBOMMonitorExist = (int)dr["DAYSBOMMONITOREXIST"];
            setup.DaysDriverTripsExist = (int)dr["DAYSDRIVERTRIPSEXIST"];

            setup.PosTerminalPrintPreOrders = (string)dr["POSTERMINALPRINTPREORDERS"];
            setup.DisplayTimeAtOrderTaking = ((byte)dr["DISPLAYTIMEATORDERTAKING"] != 0);
            setup.NormalPOSSalesType = (string)dr["NORMALPOSSALESTYPE"];
            setup.OrdListScrollPageSize = (int)dr["ORDLISTSCROLLPAGESIZE"];
            setup.TableUpdateTimerInterval = (int)dr["TABLEUPDATETIMERINTERVAL"];
        }

        private static TimeSpan GetTimeStamp(IDataReader dr, string field)
        {
            var value = dr.GetOrdinal(field);
            if (dr.IsDBNull(value))
                return new TimeSpan();

            return ((SqlDataReader) dr).GetTimeSpan(dr.GetOrdinal(field));
        }

        public virtual HospitalitySetup Get(IConnectionManager entry)
        {
            // Start checking if the table is empty.
            // If this is the first time we are opening the hospitality setup sheet, an empty 
            // record needs to be created. This is similar to the standard NAV functionality.
            if (!Exists(entry, new RecordIdentifier("1")))
            {
                var statement = new SqlServerStatement("HOSPITALITYSETUP") {StatementType = StatementType.Insert};

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("SETUP", "1");

                entry.Connection.ExecuteStatement(statement);
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "select SETUP," +
                    "ISNULL(DELIVERYSALESTYPE,'') as DELIVERYSALESTYPE," +
                    "ISNULL(DINEINSALESTYPE,'') as DINEINSALESTYPE," +
                    "ISNULL(ORDERPROCESSTIMEMIN,0) as ORDERPROCESSTIMEMIN," +
                    "ISNULL(TABLEFREECOLORB,0) as TABLEFREECOLORB," +
                    "ISNULL(TABLENOTAVAILCOLORB,0) as TABLENOTAVAILCOLORB," +
                    "ISNULL(TABLELOCKEDCOLORB,0) as TABLELOCKEDCOLORB," +
                    "ISNULL(ORDERNOTPRINTEDCOLORB,0) as ORDERNOTPRINTEDCOLORB," +
                    "ISNULL(ORDERPRINTEDCOLORB,0) as ORDERPRINTEDCOLORB," +
                    "ISNULL(ORDERSTARTEDCOLORB,0) as ORDERSTARTEDCOLORB," +
                    "ISNULL(ORDERFINISHEDCOLORB,0) as ORDERFINISHEDCOLORB," +
                    "ISNULL(ORDERCONFIRMEDCOLORB,0) as ORDERCONFIRMEDCOLORB," +
                    "ISNULL(TABLEFREECOLORF,'') as TABLEFREECOLORF," +
                    "ISNULL(TABLENOTAVAILCOLORF,'') as TABLENOTAVAILCOLORF," +
                    "ISNULL(TABLELOCKEDCOLORF,'') as TABLELOCKEDCOLORF," +
                    "ISNULL(ORDERNOTPRINTEDCOLORF,'') as ORDERNOTPRINTEDCOLORF," +
                    "ISNULL(ORDERPRINTEDCOLORF,'') as ORDERPRINTEDCOLORF," +
                    "ISNULL(ORDERSTARTEDCOLORF,'') as ORDERSTARTEDCOLORF," +
                    "ISNULL(ORDERFINISHEDCOLORF,'') as ORDERFINISHEDCOLORF," +
                    "ISNULL(ORDERCONFIRMEDCOLORF,'') as ORDERCONFIRMEDCOLORF," +
                    "ISNULL(CONFIRMSTATIONPRINTING,0) as CONFIRMSTATIONPRINTING," +
                    "ISNULL(REQUESTNOOFGUESTS,0) as REQUESTNOOFGUESTS," +
                    "ISNULL(NOOFDINEINTABLESCOL,0) as NOOFDINEINTABLESCOL," +
                    "ISNULL(NOOFDINEINTABLESROWS,0) as NOOFDINEINTABLESROWS," +
                    "ISNULL(STATIONPRINTING,0) as STATIONPRINTING," +
                    "ISNULL(DINEINTABLELOCKING,0) as DINEINTABLELOCKING," +
                    "ISNULL(DINEINTABLESELECTION,0) as DINEINTABLESELECTION," +
                    "ISNULL(PERIOD1TIMEFROM,'00:00:00') as PERIOD1TIMEFROM," +
                    "ISNULL(PERIOD1TIMETO,'00:00:00') as PERIOD1TIMETO," +
                    "ISNULL(PERIOD2TIMEFROM,'00:00:00') as PERIOD2TIMEFROM," +
                    "ISNULL(PERIOD2TIMETO,'00:00:00') as PERIOD2TIMETO," +
                    "ISNULL(PERIOD3TIMEFROM,'00:00:00') as PERIOD3TIMEFROM," +
                    "ISNULL(PERIOD3TIMETO,'00:00:00') as PERIOD3TIMETO," +
                    "ISNULL(PERIOD4TIMEFROM,'00:00:00') as PERIOD4TIMEFROM," +
                    "ISNULL(PERIOD4TIMETO,'00:00:00') as PERIOD4TIMETO," +
                    "ISNULL(AUTOLOGOFFATPOSEXIT,0) as AUTOLOGOFFATPOSEXIT," +
                    "ISNULL(TAKEOUTSALESTYPE,'') as TAKEOUTSALESTYPE," +
                    "ISNULL(PREORDERSALESTYPE,'') as PREORDERSALESTYPE," +
                    "ISNULL(LOGSTATIONPRINTING,0) as LOGSTATIONPRINTING," +
                    "ISNULL(POPULATEDELIVERYINFOCODES,0) as POPULATEDELIVERYINFOCODES," +
                    "ISNULL(ALLOWPREORDERS,0) as ALLOWPREORDERS," +
                    "ISNULL(TAKEOUTNONAMENO,'') as TAKEOUTNONAMENO," +
                    "ISNULL(ADVPREORDPRINTMIN,0) as ADVPREORDPRINTMIN," +
                    "ISNULL(CLOSETRIPONDEPART,0) as CLOSETRIPONDEPART," +
                    "ISNULL(DELPROGRESSSTATUSINUSE,0) as DELPROGRESSSTATUSINUSE," +
                    "ISNULL(DAYSBOMPRINTEXIST,0) as DAYSBOMPRINTEXIST," +
                    "ISNULL(DAYSBOMMONITOREXIST,0) as DAYSBOMMONITOREXIST," +
                    "ISNULL(DAYSDRIVERTRIPSEXIST,0) as DAYSDRIVERTRIPSEXIST," +
                    "ISNULL(POSTERMINALPRINTPREORDERS,'') as POSTERMINALPRINTPREORDERS," +
                    "ISNULL(DISPLAYTIMEATORDERTAKING,0) as DISPLAYTIMEATORDERTAKING," +
                    "ISNULL(NORMALPOSSALESTYPE,'') as NORMALPOSSALESTYPE," +
                    "ISNULL(TABLEUPDATETIMERINTERVAL,30) as TABLEUPDATETIMERINTERVAL," +
                    "ISNULL(ORDLISTSCROLLPAGESIZE,0) as ORDLISTSCROLLPAGESIZE " +
                    "from HOSPITALITYSETUP " +
                    "where DATAAREAID = @dataAreaId and SETUP = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", "1");

                return Execute<HospitalitySetup>(entry, cmd, CommandType.Text, PopulateHospitalitySetup)[0];
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "HOSPITALITYSETUP", "SETUP", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "HOSPITALITYSETUP", "SETUP", id, BusinessObjects.Permission.ManageHospitalitySetup);
        }

        public virtual void Save(IConnectionManager entry, HospitalitySetup setup)
        {
            var statement = new SqlServerStatement("HOSPITALITYSETUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageHospitalitySetup);

            if (!Exists(entry, setup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("SETUP", (string)setup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("SETUP", (string)setup.ID);
            }

            statement.AddField("DELIVERYSALESTYPE", (string)setup.DeliverySalesType);
            statement.AddField("DINEINSALESTYPE", (string)setup.DineInSalesType);
            statement.AddField("ORDERPROCESSTIMEMIN", setup.OrderProcessTimeMin, SqlDbType.Int);

            statement.AddField("TABLEFREECOLORB", setup.TableFreeColorB, SqlDbType.Int);
            statement.AddField("TABLENOTAVAILCOLORB", setup.TableNotAvailColorB, SqlDbType.Int);
            statement.AddField("TABLELOCKEDCOLORB", setup.TableLockedColorB, SqlDbType.Int);
            statement.AddField("ORDERNOTPRINTEDCOLORB", setup.OrderNotPrintedColorB, SqlDbType.Int);
            statement.AddField("ORDERPRINTEDCOLORB", setup.OrderPrintedColorB, SqlDbType.Int);
            statement.AddField("ORDERSTARTEDCOLORB", setup.OrderStartedColorB, SqlDbType.Int);
            statement.AddField("ORDERFINISHEDCOLORB", setup.OrderFinishedColorB, SqlDbType.Int);
            statement.AddField("ORDERCONFIRMEDCOLORB", setup.OrderConfirmedColorB, SqlDbType.Int);

            statement.AddField("TABLEFREECOLORF", setup.TableFreeColorF);
            statement.AddField("TABLENOTAVAILCOLORF", setup.TableNotAvailColorF);
            statement.AddField("TABLELOCKEDCOLORF", setup.TableLockedColorF);
            statement.AddField("ORDERNOTPRINTEDCOLORF", setup.OrderNotPrintedColorF);
            statement.AddField("ORDERPRINTEDCOLORF", setup.OrderPrintedColorF);
            statement.AddField("ORDERSTARTEDCOLORF", setup.OrderStartedColorF);
            statement.AddField("ORDERFINISHEDCOLORF", setup.OrderFinishedColorF);
            statement.AddField("ORDERCONFIRMEDCOLORF", setup.OrderConfirmedColorF);

            statement.AddField("CONFIRMSTATIONPRINTING", setup.ConfirmStationPrinting ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REQUESTNOOFGUESTS", setup.RequestNoOfGuests ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("NOOFDINEINTABLESCOL", setup.NoOfDineInTablesCol, SqlDbType.Int);
            statement.AddField("NOOFDINEINTABLESROWS", setup.NoOfDineInTablesRows, SqlDbType.Int);
            statement.AddField("STATIONPRINTING", setup.StationPrinting, SqlDbType.Int);
            statement.AddField("DINEINTABLELOCKING", setup.DineInTableLocking, SqlDbType.Int);
            statement.AddField("DINEINTABLESELECTION", setup.DineInTableSelection, SqlDbType.Int);

            statement.AddField("PERIOD1TIMEFROM", setup.Period1TimeFrom, SqlDbType.Time);
            statement.AddField("PERIOD1TIMETO", setup.Period1TimeTo, SqlDbType.Time);
            statement.AddField("PERIOD2TIMEFROM", setup.Period2TimeFrom, SqlDbType.Time);
            statement.AddField("PERIOD2TIMETO", setup.Period2TimeTo, SqlDbType.Time);
            statement.AddField("PERIOD3TIMEFROM", setup.Period3TimeFrom, SqlDbType.Time);
            statement.AddField("PERIOD3TIMETO", setup.Period3TimeTo, SqlDbType.Time);
            statement.AddField("PERIOD4TIMEFROM", setup.Period4TimeFrom, SqlDbType.Time);
            statement.AddField("PERIOD4TIMETO", setup.Period4TimeTo, SqlDbType.Time);

            statement.AddField("AUTOLOGOFFATPOSEXIT", setup.AutoLogoffAtPOSExit ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TAKEOUTSALESTYPE", (string)setup.TakeOutSalesType);
            statement.AddField("PREORDERSALESTYPE", (string)setup.PreOrderSalesType);
            statement.AddField("LOGSTATIONPRINTING", setup.LogStationPrinting ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("POPULATEDELIVERYINFOCODES", setup.PopulateDeliveryInfocodes ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWPREORDERS", setup.AllowPreOrders ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TAKEOUTNONAMENO", setup.TakeoutNoNameNo);
            statement.AddField("ADVPREORDPRINTMIN", setup.AdvPreOrdPrintMin, SqlDbType.Int);
            statement.AddField("CLOSETRIPONDEPART", setup.CloseTripOnDepart ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DELPROGRESSSTATUSINUSE", setup.DelProgressStatusInUse ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DAYSBOMPRINTEXIST", setup.DaysBOMPrintExist, SqlDbType.Int);
            statement.AddField("DAYSBOMMONITOREXIST", setup.DaysBOMMonitorExist, SqlDbType.Int);
            statement.AddField("DAYSDRIVERTRIPSEXIST", setup.DaysDriverTripsExist, SqlDbType.Int);
            statement.AddField("POSTERMINALPRINTPREORDERS", setup.PosTerminalPrintPreOrders);
            statement.AddField("DISPLAYTIMEATORDERTAKING", setup.DisplayTimeAtOrderTaking ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("NORMALPOSSALESTYPE", (string)setup.NormalPOSSalesType);
            statement.AddField("ORDLISTSCROLLPAGESIZE", setup.OrdListScrollPageSize, SqlDbType.Int);
            statement.AddField("TABLEUPDATETIMERINTERVAL", setup.TableUpdateTimerInterval, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
