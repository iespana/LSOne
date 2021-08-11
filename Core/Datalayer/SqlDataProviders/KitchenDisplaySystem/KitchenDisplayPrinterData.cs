using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayPrinterData : SqlServerDataProviderBase, IKitchenDisplayPrinterData
    {
       
        private static string ResolveSort(KitchenPrinterSortingEnum sort, bool backwards)
        {
            switch (sort)
            {
                case KitchenPrinterSortingEnum.Id:
                    return backwards ? "host.ID DESC" : "host.ID ASC";

                case KitchenPrinterSortingEnum.Host:
                    return backwards ? "host.DESCRIPTION DESC" : "host.DESCRIPTION ASC";

                case KitchenPrinterSortingEnum.PrinterName:
                    return backwards ? "printer.PRINTERNAME DESC" : "printer.PRINTERNAME ASC";
            }

            return "";
        }

        private static string BaseSelectString
        {
            get
            {
                return @"
                        SELECT 
                        printer.ID as PRINTERID,
                        printer.DESCRIPTION as DESCRIPTION,
                        printer.PRINTERNAME as PRINTERNAME,
                        printer.USEEXTERNALROUTING as USEEXTERNALROUTING,
                        printer.VISUALPROFILEID as VISUALPROFILEID,
                        visualProfile.NAME as VISUALPROFILENAME,
                        host.ID as HOSTID,
                        host.DESCRIPTION as HOSTDESCRIPTION,
                        printer.CUTPAPER as CUTPAPER,
                        ISNULL(printer.OPOSMAXCHARS, 55) as OPOSMAXCHARS,
                        ISNULL(printer.OPOSFONTSIZEV, 0) as OPOSFONTSIZEV,
                        ISNULL(printer.OPOSFONTSIZEH, 0) as OPOSFONTSIZEH,
                        ISNULL(printer.OPOSLINESPACING, 30) as OPOSLINESPACING,
                        ISNULL(printer.STATIONTYPE, 0) as STATIONTYPE,
                        ISNULL(printer.LAYOUTTYPE, 0) as LAYOUTTYPE,
                        ISNULL(printer.TOPSPACES, 0) as TOPSPACES,
                        ISNULL(printer.BOTTOMSPACES, 5) as BOTTOMSPACES,
                        printer.DIRECTTOLOGFILE as DIRECTTOLOGFILE
                        FROM KITCHENDISPLAYPRINTER " +
                        " as printer JOIN STATIONPRINTINGHOSTS as host on printer.PRINTERHOSTID = host.ID and printer.DATAAREAID = host.DATAAREAID JOIN KITCHENDISPLAYVISUALPROFILE as visualProfile on printer.VISUALPROFILEID = visualProfile.ID and printer.DATAAREAID = visualProfile.DATAAREAID ";
            }
        }

        private static void PopulateKitchenPrinter(IDataReader dr, KitchenDisplayPrinter printer)
        {
            printer.ID = (string)dr["PRINTERID"];
            printer.Text = (string)dr["DESCRIPTION"];
            printer.PrinterName = (string)dr["PRINTERNAME"];
            printer.UseExternalRouting = AsBool(dr["USEEXTERNALROUTING"]);
            printer.VisualProfileId = (Guid)dr["VISUALPROFILEID"];
            printer.VisualProfileDescription = (string)dr["VISUALPROFILENAME"];
            printer.HostId = (string)dr["HOSTID"];
            printer.HostDescription = (string)dr["HOSTDESCRIPTION"];
            if (dr["CUTPAPER"] == DBNull.Value)
                printer.CutPaper = true;
            else
                printer.CutPaper = AsBool(dr["CUTPAPER"]);
            printer.OPosMaxChars = (int)dr["OPOSMAXCHARS"];
            printer.OPosFontSizeV = (int)dr["OPOSFONTSIZEV"];
            printer.OPosFontSizeH = (int)dr["OPOSFONTSIZEH"];
            printer.OPosLineSpacing = (int)dr["OPOSLINESPACING"];
            printer.TopSpaces = (int)dr["TOPSPACES"];
            printer.BottomSpaces = (int)dr["BOTTOMSPACES"];
            if (dr["DIRECTTOLOGFILE"] == DBNull.Value)
                printer.DirectOutputToLog = false;
            else
                printer.DirectOutputToLog = AsBool(dr["DIRECTTOLOGFILE"]);

          
            printer.StationType = (KitchenDisplayStation.StationTypeEnum)((byte)dr["STATIONTYPE"]);
            printer.LayoutType = (KitchenDisplayStation.ChitLayoutEnum)((byte)dr["LAYOUTTYPE"]);
            
        }

        public virtual List<KitchenDisplayPrinter> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where printer.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayPrinter>(entry, cmd, CommandType.Text, PopulateKitchenPrinter);
            }
        }

        public virtual List<KitchenDisplayPrinter> GetList(IConnectionManager entry, KitchenPrinterSortingEnum sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where printer.DATAAREAID = @dataAreaId order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayPrinter>(entry, cmd, CommandType.Text, PopulateKitchenPrinter);
            }
        }

        public virtual KitchenDisplayPrinter Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where printer.DATAAREAID = @dataAreaId and printer.ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                var result = Get<KitchenDisplayPrinter>(entry, cmd, id, PopulateKitchenPrinter, cacheType, UsageIntentEnum.Normal);

                return result;
            }            
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYPRINTER", "ID", id);
        }

        /// <summary>
        /// Deletes the kitchen printer with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the kitchen printer to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYPRINTER", "ID", id, BusinessObjects.Permission.ManageKitchenPrinters);
        }

        /// <summary>
        /// Saves a kitchen printer into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="printer">The kitchen printer</param>
        public virtual void Save(IConnectionManager entry, KitchenDisplayPrinter printer)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYPRINTER");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenPrinters);

            bool isNew = false;
            if (printer.ID.IsEmpty)
            {
                isNew = true;
                printer.ID = DataProviderFactory.Instance.GenerateNumber<IKitchenDisplayPrinterData, KitchenDisplayPrinter>(entry);
            }

            if (isNew || !Exists(entry, printer.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (string)printer.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)printer.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", printer.Text);
            statement.AddField("PRINTERHOSTID", printer.HostId);
            statement.AddField("PRINTERNAME", printer.PrinterName);
            statement.AddField("USEEXTERNALROUTING", printer.UseExternalRouting ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VISUALPROFILEID", printer.VisualProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("CUTPAPER", printer.CutPaper, SqlDbType.TinyInt);
            statement.AddField("OPOSMAXCHARS", printer.OPosMaxChars, SqlDbType.Int);
            statement.AddField("OPOSFONTSIZEV", printer.OPosFontSizeV, SqlDbType.Int);
            statement.AddField("OPOSFONTSIZEH", printer.OPosFontSizeH, SqlDbType.Int);
            statement.AddField("OPOSLINESPACING", printer.OPosLineSpacing, SqlDbType.Int);
            statement.AddField("STATIONTYPE", (int)printer.StationType, SqlDbType.TinyInt);
            statement.AddField("LAYOUTTYPE", (int)printer.LayoutType, SqlDbType.TinyInt);
            statement.AddField("TOPSPACES", printer.TopSpaces, SqlDbType.Int);
            statement.AddField("BOTTOMSPACES", printer.BottomSpaces, SqlDbType.Int);
            statement.AddField("DIRECTTOLOGFILE", printer.DirectOutputToLog, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "KITCHENDISPLAYPRINTER", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public RecordIdentifier SequenceID
        {
            get { return "RestaurantStation"; }
        }

        #endregion
    }
}
