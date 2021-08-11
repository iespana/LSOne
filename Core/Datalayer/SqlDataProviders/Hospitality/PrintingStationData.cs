using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class PrintingStationData : SqlServerDataProviderBase, IPrintingStationData
    {
        private static string ResolveSort(PrintingStationSorting sort, bool backwards)
        {
            switch (sort)
            {
                case PrintingStationSorting.PrintingStationId:
                    return backwards ? "rs.ID DESC" : "ID ASC";

                case PrintingStationSorting.PrintingtStationName:
                    return backwards ? "rs.STATIONNAME DESC" : "rs.STATIONNAME ASC";
            }

            return "";
        }

        private static string BaseSelectString
        {
            get
            {
                return @"select
                    rs.ID,
                    ISNULL(rs.STATIONNAME,'') as STATIONNAME,
                    ISNULL(rs.WINDOWSPRINTER,'') as WINDOWSPRINTER,
                    ISNULL(rs.OUTPUTLINES,0) as OUTPUTLINES, 
                    ISNULL(rs.CHECKPRINTING,0) as CHECKPRINTING, 
                    ISNULL(rs.POSEXTERNALPRINTERID,'') as POSEXTERNALPRINTERID, 
                    ISNULL(rs.PRINTING,0) as PRINTING, 
                    ISNULL(rs.STATIONFILTER,'') as STATIONFILTER, 
                    ISNULL(rs.STATIONCHECKMINUTES,0) as STATIONCHECKMINUTES, 
                    ISNULL(rs.COMPRESSBOMRECEIPT,0) as COMPRESSBOMRECEIPT, 
                    ISNULL(rs.EXCLUDEFROMCOMPRESSION,'') as EXCLUDEFROMCOMPRESSION,
                    ISNULL(rs.STATIONTYPE,0) as STATIONTYPE, 
                    ISNULL(rs.PRINTINGPRIORITY,0) as PRINTINGPRIORITY, 
                    ISNULL(rs.ENDTURNSREDAFTERMIN,0) as ENDTURNSREDAFTERMIN, 
                    ISNULL(rs.REMOTEHOSTID,'') as STATIONPRINTINGHOSTID, 
                    ISNULL(rh.ADDRESS,'') as STATIONPRINTINGHOSTADDRESS, 
                    ISNULL(rh.PORT,'') as STATIONPRINTINGHOSTPORT,
                    ISNULL(rs.PRINTERDEVICENAME, '') as  PRINTERDEVICENAME, 
                    ISNULL(rs.WINDOWSPRINTERCONFIGURATIONID, '') as  WINDOWSPRINTERCONFIGURATIONID, 
                    ISNULL(rs.OPOSFONTSIZEV, 1) as OPOSFONTSIZEV,
                    ISNULL(rs.OPOSFONTSIZEH, 2) as OPOSFONTSIZEH,
                    ISNULL(rs.OPOSMAXCHARS, 28) as OPOSMAXCHARS,
                    ISNULL(rs.KITCHENDISPLAYPROFILEID, '') as KITCHENDISPLAYPROFILEID
                    from RESTAURANTSTATION rs 
                    left outer join STATIONPRINTINGHOSTS rh on rh.ID = rs.REMOTEHOSTID and rh.DATAAREAID = rs.DATAAREAID ";
            }
        }

        private static void PopulatePrintingStation(IDataReader dr, PrintingStation printingStation)
        {
            printingStation.ID = (string)dr["ID"];
            printingStation.Text = (string)dr["STATIONNAME"];
            printingStation.WindowsPrinter = (string)dr["WINDOWSPRINTER"];
            printingStation.OutputLines = (PrintingStation.OutputLinesEnum)((int)dr["OUTPUTLINES"]);
            printingStation.CheckPrinting = (PrintingStation.CheckPrintingEnum)((int)dr["CHECKPRINTING"]);
            printingStation.POSExternalPrinterID = (string)dr["POSEXTERNALPRINTERID"];
            printingStation.Printing = (PrintingStation.PrintingEnum)((int)dr["PRINTING"]);
            printingStation.StationFilter = (string)dr["STATIONFILTER"];
            printingStation.StationCheckMinutes = (int)dr["STATIONCHECKMINUTES"];
            printingStation.CompressBOMReceipt = (PrintingStation.CompressBOMReceiptEnum)((int)dr["COMPRESSBOMRECEIPT"]);
            printingStation.ExcludeFromCompression = (string)dr["EXCLUDEFROMCOMPRESSION"];
            printingStation.StationType = (PrintingStation.StationTypeEnum)((int)dr["STATIONTYPE"]);
            printingStation.PrintingPriority = (int)dr["PRINTINGPRIORITY"];
            printingStation.EndTurnsRedAfterMin = (int)dr["ENDTURNSREDAFTERMIN"];
            printingStation.StationPrintingHostID = (string)dr["STATIONPRINTINGHOSTID"];
            printingStation.StationPrintingHostAddress = (string)dr["STATIONPRINTINGHOSTADDRESS"];
            printingStation.StationPrintingHostPort = (int)dr["STATIONPRINTINGHOSTPORT"];
            printingStation.PrinterDeviceName = (string)dr["PRINTERDEVICENAME"];
            printingStation.WindowsPrinterConfigurationID = (string)dr["WINDOWSPRINTERCONFIGURATIONID"];
            printingStation.OPOSFontSizeV = (int) dr["OPOSFONTSIZEV"];
            printingStation.OPOSFontSizeH = (int)dr["OPOSFONTSIZEH"];
            printingStation.OPOSMaxChars = (int)dr["OPOSMAXCHARS"];
        }

        /// <summary>
        /// Gets all PrintingStation stations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PrintingStation objects containing all printing station</returns>
        public virtual List<PrintingStation> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where rs.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PrintingStation>(entry, cmd, CommandType.Text, PopulatePrintingStation);
            }
        }

        /// <summary>
        /// Gets all printing stations ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all printing stations, ordered by a chosen field</returns>
        public virtual List<PrintingStation> GetList(IConnectionManager entry, PrintingStationSorting sortBy, bool sortBackwards)
        {
            if (sortBy != PrintingStationSorting.PrintingStationId && sortBy != PrintingStationSorting.PrintingtStationName)
            {
                throw new NotSupportedException();
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where rs.DATAAREAID = @dataAreaId " +
                    " order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PrintingStation>(entry, cmd, CommandType.Text, PopulatePrintingStation);
            }
        }

        /// <summary>
        /// Gets a data entity version of the printing station with the give ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Id of the printing station to get data entity for</param>
        public virtual DataEntity GetDataEntity(IConnectionManager entry, RecordIdentifier id)
        {
            return GetDataEntity<DataEntity>(entry, "RESTAURANTSTATION", "STATIONNAME", "ID", id);
        }

        /// <summary>
        /// Gets a printing station with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the printing station to get</param>
        /// <returns>A PrintingStation object containing the printing station with the given ID</returns>
        public virtual PrintingStation Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where rs.DATAAREAID = @dataAreaId and rs.ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) id);

                var result = Execute<PrintingStation>(entry, cmd, CommandType.Text, PopulatePrintingStation);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a printing station with the given id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the printing station to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RESTAURANTSTATION", "ID", id);
        }

        /// <summary>
        /// Deletes the printing station with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the printing station to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RESTAURANTSTATION", "ID", id, BusinessObjects.Permission.ManageStationPrinting);

            // Remove the printing station from the kds section station routing table
            Providers.KitchenDisplaySectionStationRoutingData.RemoveStation(entry, id);
        }

        /// <summary>
        /// Saves a printing station into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="printingStation">The printing station object to save</param>
        public virtual void Save(IConnectionManager entry, PrintingStation printingStation)
        {
            var statement = new SqlServerStatement("RESTAURANTSTATION");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageStationPrinting);

            bool isNew = false;
            if (printingStation.ID.IsEmpty)
            {
                isNew = true;
                printingStation.ID = DataProviderFactory.Instance.GenerateNumber<IPrintingStationData, PrintingStation>(entry);
            }

            if (isNew || !Exists(entry, printingStation.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (string)printingStation.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)printingStation.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("STATIONNAME", printingStation.Text);
            statement.AddField("WINDOWSPRINTER", printingStation.WindowsPrinter);
            statement.AddField("OUTPUTLINES", (int)printingStation.OutputLines, SqlDbType.Int);
            statement.AddField("CHECKPRINTING", (int)printingStation.CheckPrinting, SqlDbType.Int);
            statement.AddField("POSEXTERNALPRINTERID", (string)printingStation.POSExternalPrinterID);
            statement.AddField("PRINTING", (int)printingStation.Printing, SqlDbType.Int);
            statement.AddField("STATIONFILTER", printingStation.StationFilter);
            statement.AddField("STATIONCHECKMINUTES", printingStation.StationCheckMinutes, SqlDbType.Int);
            statement.AddField("COMPRESSBOMRECEIPT", (int)printingStation.CompressBOMReceipt, SqlDbType.Int);
            statement.AddField("EXCLUDEFROMCOMPRESSION", printingStation.ExcludeFromCompression);
            statement.AddField("STATIONTYPE", (int)printingStation.StationType, SqlDbType.Int);
            statement.AddField("PRINTINGPRIORITY", printingStation.PrintingPriority, SqlDbType.Int);
            statement.AddField("ENDTURNSREDAFTERMIN", printingStation.EndTurnsRedAfterMin, SqlDbType.Int);
            statement.AddField("REMOTEHOSTID", (string)printingStation.StationPrintingHostID);
            statement.AddField("PRINTERDEVICENAME", printingStation.PrinterDeviceName);
            statement.AddField("WINDOWSPRINTERCONFIGURATIONID", (string)printingStation.WindowsPrinterConfigurationID);
            statement.AddField("OPOSMAXCHARS", printingStation.OPOSMaxChars, SqlDbType.Int);
            statement.AddField("OPOSFONTSIZEV", printingStation.OPOSFontSizeV, SqlDbType.Int);
            statement.AddField("OPOSFONTSIZEH", printingStation.OPOSFontSizeH, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "RestaurantStation"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RESTAURANTSTATION", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
