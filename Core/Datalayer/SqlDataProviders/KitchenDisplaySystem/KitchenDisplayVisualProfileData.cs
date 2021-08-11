using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.Utilities.IO.JSON;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayVisualProfileData : SqlServerDataProviderBase, IKitchenDisplayVisualProfileData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"   select 
                            ID
                            ,NAME
                            ,ORDERPANEWIDTH
                            ,ORDERPANEHEIGHT
                            ,ORDERPANEX
                            ,ORDERPANEY
                            ,ORDERPANEVISIBLE
                            ,BUTTONPANEWIDTH
                            ,BUTTONPANEHEIGHT
                            ,BUTTONPANEX
                            ,BUTTONPANEY
                            ,BUTTONPANEVISIBLE
                            ,BUTTONPANEPOSITION
                            ,NUMBEROFCOLUMNS
                            ,NUMBEROFROWS
                            ,ISNULL(AGGREGATEPANEWIDTH,0) as AGGREGATEPANEWIDTH
                            ,ISNULL(AGGREGATEPANEHEIGHT,0) as AGGREGATEPANEHEIGHT
                            ,ISNULL(AGGREGATEPANEX,0) as AGGREGATEPANEX
                            ,ISNULL(AGGREGATEPANEY,0) as AGGREGATEPANEY
                            ,ISNULL(AGGREGATEPANEVISIBLE,0) as AGGREGATEPANEVISIBLE
                            ,ISNULL(AGGREGATEPANENUMBEROFCOLUMNS,0) as AGGREGATEPANENUMBEROFCOLUMNS
                            ,ISNULL(AGGREGATEPANEPOSITION,0) as AGGREGATEPANEPOSITION
                            ,ISNULL(ITEMMODIFIERINCREASEPREFIX,'') as ITEMMODIFIERINCREASEPREFIX
                            ,ISNULL(ITEMMODIFIERDECREASEPREFIX,'') as ITEMMODIFIERDECREASEPREFIX
                            ,ISNULL(ITEMMODIFIERNORMALPREFIX,'') as ITEMMODIFIERNORMALPREFIX
                            ,ISNULL(SHOWDEALS,0) as SHOWDEALS
                            ,SHOWNAME
                            ,LANGUAGECODE 
                            ,CHITREFRESHRATE
                            ,ISNULL(CHITSIZE,0) as CHITSIZE
                            ,ISNULL(ITEMMODIFIERHEAVYPREFIX,'') as ITEMMODIFIERHEAVYPREFIX
                            ,ISNULL(ITEMMODIFIERLIGHTPREFIX,'') as ITEMMODIFIERLIGHTPREFIX
                            ,ISNULL(ITEMMODIFIERONLYPREFIX,'') as ITEMMODIFIERONLYPREFIX
                            ,ISNULL(ITEMMODIFIERDONEPREFIX,'') as ITEMMODIFIERDONEPREFIX
                            ,ISNULL(HISTORYPANEWIDTH,0) as HISTORYPANEWIDTH
                            ,ISNULL(HISTORYPANEHEIGHT,0) as HISTORYPANEHEIGHT
                            ,ISNULL(HISTORYPANEX,0) as HISTORYPANEX
                            ,ISNULL(HISTORYPANEY,0) as HISTORYPANEY
                            ,ISNULL(HISTORYPANEVISIBLE,0) as HISTORYPANEVISIBLE
                            ,ISNULL(HISTORYPANEPOSITION,0) as HISTORYPANEPOSITION
                            ,ISNULL(HISTORYMAXSUMMARYLINES,0) as HISTORYMAXSUMMARYLINES
                            ,ISNULL(HISTORYMAXLASTBUMPEDLINES,0) as HISTORYMAXLASTBUMPEDLINES
                            ,ISNULL(HISTORYLIFESPAN,0) as HISTORYLIFESPAN
                            ,HEADERPANEID
                            ,ISNULL(HEADERPANEHEIGHT,0) as HEADERPANEHEIGHT
                            ,HEADERPANEX
                            ,HEADERPANEY
                          from KITCHENDISPLAYVISUALPROFILE ";
            }
        }

        private static void PopulateProfile(IDataReader dr, KitchenDisplayVisualProfile visualProfile)
        {
            visualProfile.ID = (Guid)dr["ID"];
            visualProfile.Text = (string)dr["NAME"];
            visualProfile.OrderPaneWidth = (decimal)dr["ORDERPANEWIDTH"];
            visualProfile.OrderPaneHeight = (decimal)dr["ORDERPANEHEIGHT"];
            visualProfile.OrderPaneX = (decimal)dr["ORDERPANEX"];
            visualProfile.OrderPaneY = (decimal)dr["ORDERPANEY"];
            visualProfile.OrderPaneVisible = ((byte)dr["ORDERPANEVISIBLE"] == 1);
            visualProfile.ButtonPaneWidth = (decimal)dr["BUTTONPANEWIDTH"];
            visualProfile.ButtonPaneHeight = (decimal)dr["BUTTONPANEHEIGHT"];
            visualProfile.ButtonPaneX = (decimal)dr["BUTTONPANEX"];
            visualProfile.ButtonPaneY = (decimal)dr["BUTTONPANEY"];
            visualProfile.ButtonPaneVisible = ((byte)dr["BUTTONPANEVISIBLE"] == 1);
            visualProfile.ButtonPanePosition = (KitchenDisplayVisualProfile.ButtonPositionEnum)(byte)dr["BUTTONPANEPOSITION"];
            visualProfile.NumberOfColumns = (int)dr["NUMBEROFCOLUMNS"];
            visualProfile.NumberOfRows = (int)dr["NUMBEROFROWS"];
            visualProfile.AggregatePaneWidth = (decimal)dr["AGGREGATEPANEWIDTH"];
            visualProfile.AggregatePaneHeight = (decimal)dr["AGGREGATEPANEHEIGHT"];
            visualProfile.AggregatePaneX = (decimal)dr["AGGREGATEPANEX"];
            visualProfile.AggregatePaneY = (decimal)dr["AGGREGATEPANEY"];
            visualProfile.AggregatePaneVisible = AsBool(dr["AGGREGATEPANEVISIBLE"]);
            visualProfile.AggregatePaneNumberofColumns = (int)(dr["AGGREGATEPANENUMBEROFCOLUMNS"]);
            visualProfile.AggregatePanePosition = (KitchenDisplayVisualProfile.AggregatePositionEnum)dr["AGGREGATEPANEPOSITION"];
            visualProfile.ItemModifierIncreasePrefix = (string)dr["ITEMMODIFIERINCREASEPREFIX"];
            visualProfile.ItemModifierDecreasePrefix = (string)dr["ITEMMODIFIERDECREASEPREFIX"];
            visualProfile.ItemModifierNormalPrefix = (string)dr["ITEMMODIFIERNORMALPREFIX"];
            visualProfile.ShowDeals = ((byte)dr["SHOWDEALS"] == 1);
            visualProfile.ShowName = AsBool(dr["SHOWNAME"]);
            visualProfile.LanguageCode = (string)dr["LANGUAGECODE"];
            visualProfile.ChitRefreshRate = (int)dr["CHITREFRESHRATE"];
            visualProfile.ChitSize = (KitchenDisplayVisualProfile.ChitSizeEnum)dr["CHITSIZE"];
            visualProfile.ItemModifierHeavyPrefix = (string)dr["ITEMMODIFIERHEAVYPREFIX"];
            visualProfile.ItemModifierLightPrefix = (string)dr["ITEMMODIFIERLIGHTPREFIX"];
            visualProfile.ItemModifierOnlyPrefix = (string)dr["ITEMMODIFIERONLYPREFIX"];
            visualProfile.ItemModifierDonePrefix = (string)dr["ITEMMODIFIERDONEPREFIX"];
            visualProfile.HistoryPaneWidth = (decimal)dr["HISTORYPANEWIDTH"];
            visualProfile.HistoryPaneHeight = (decimal)dr["HISTORYPANEHEIGHT"];
            visualProfile.HistoryPaneX = (decimal)dr["HISTORYPANEX"];
            visualProfile.HistoryPaneY = (decimal)dr["HISTORYPANEY"];
            visualProfile.HistoryPaneVisible = AsBool(dr["HISTORYPANEVISIBLE"]);
            visualProfile.HistoryPanePosition = (KitchenDisplayVisualProfile.HistoryPositionEnum)dr["HISTORYPANEPOSITION"];
            visualProfile.HistoryMaxSummaryLines = (int)dr["HISTORYMAXSUMMARYLINES"];
            visualProfile.HistoryMaxLastBumpedLines = (int)dr["HISTORYMAXLASTBUMPEDLINES"];
            visualProfile.HistoryLifespanInMinutes = (int)dr["HISTORYLIFESPAN"];

            if (dr["HEADERPANEID"] != DBNull.Value)
            {
                visualProfile.HeaderProfileId = ((Guid)dr["HEADERPANEID"]).ToString();
            }
            else
            {
                visualProfile.HeaderProfileId = Guid.Empty.ToString();
            }

            visualProfile.HeaderPaneWidth = (decimal)1.0;
            visualProfile.HeaderPaneHeight = (decimal)dr["HEADERPANEHEIGHT"];
            visualProfile.HeaderPaneX = (decimal)dr["HEADERPANEX"];
            visualProfile.HeaderPaneY = (decimal)dr["HEADERPANEY"];
        }

        public virtual KitchenDisplayVisualProfile Get(IConnectionManager entry, RecordIdentifier visualProfileId, RecordIdentifier displayProfileID)
        {            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                BaseSelectString +
                "where ID = @profileId and  DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "profileId", (Guid)visualProfileId.PrimaryID, SqlDbType.UniqueIdentifier);

                List<KitchenDisplayVisualProfile> results;

                results = Execute<KitchenDisplayVisualProfile>(entry, cmd, CommandType.Text, PopulateProfile);
                

                if (results.Count > 0)
                {
                    if (displayProfileID == null)
                    {
                        // VisualProfile for printer: We create a dummy guid for the Display Profile ID 
                        displayProfileID = Guid.Parse("{00000000-0000-0000-0000-000000000000}");
                    }
                    AddHeaderFooterSetupToProfile(entry, displayProfileID, visualProfileId, results[0]);
                }

                return (results.Count == 1) ? results[0] : null;
            }
        }

        private static void AddHeaderFooterSetupToProfile(IConnectionManager entry, RecordIdentifier displayProfileId, RecordIdentifier visualProfileId, KitchenDisplayVisualProfile visualProfile)
        {            
            string cmd1 = string.Format("SELECT * FROM KITCHENDISPLAYLINE "
                + " WHERE [DISPLAYPROFILEID]='{0}' AND [DATAAREAID]='{1}'"
                + " ORDER BY LINETYPE, LINENUMBER", displayProfileId.PrimaryID, entry.Connection.DataAreaId);

            IDbCommand cmdRows = entry.Connection.CreateCommand(cmd1);
            using (IDataReader drRows = entry.Connection.ExecuteReader(cmdRows, CommandType.Text))
            {
                int rowcount = 0;
                int partcount = 0;
                while (drRows.Read())
                {
                    rowcount += 1;
                    Guid rowID = (Guid)drRows["ID"];
                    string dataareaid = (string)drRows["DATAAREAID"];
                    KitchenDisplayStation.DisplayRowTypeEnum rowType = (KitchenDisplayStation.DisplayRowTypeEnum)AsInt(drRows["LINETYPE"]);
                    int lineNo = (int)drRows["LINENUMBER"];

                    string cmd2 = string.Format("SELECT * FROM KITCHENDISPLAYLINECOLUMN WHERE [LINENUMBERID] = '{0}' order by COLUMNNUMBER ", rowID);

                    IDbCommand cmdParts = entry.Connection.CreateCommand(cmd2);
                    KitchenDisplayHeaderFooterRow row = new KitchenDisplayHeaderFooterRow();
                    using (IDataReader drRowParts = entry.Connection.ExecuteReader(cmdParts, CommandType.Text))
                    {
                        while (drRowParts.Read())
                        {
                            KitchenDisplayHeaderFooterRowPart part = new KitchenDisplayHeaderFooterRowPart();
                            partcount += 1;
                            int partNo = (int)drRowParts["COLUMNNUMBER"];
                            PartTypeEnum partType = (PartTypeEnum)AsInt(drRowParts["COLUMNTYPE"]);
                            PartAlignmentEnum partAlignment = (PartAlignmentEnum)AsInt(drRowParts["ALIGNMENT"]);
                            PartOrderPropertyEnum partOrderProperty = (PartOrderPropertyEnum)AsInt(drRowParts["ORDERPROPERTY"]);
                            string mappingKey = (string)drRowParts["MAPPINGKEY"];
                            int relativeSize = AsInt(drRowParts["RELATIVESIZE"]);

                            if (drRowParts["COLORSTYLE"] != DBNull.Value)
                            {
                                string colorStyle = (string)drRowParts["COLORSTYLE"];
                                part.Style = JsonConvert.DeserializeObject<BaseStyle>(colorStyle);
                            }
                            else
                            {
                                part.Style = new BaseStyle();
                            }
                            part.PartType = partType;
                            part.Alignment = partAlignment;
                            part.MappingKey = mappingKey;
                            part.OrderProperty = partOrderProperty;
                            part.RelativeSize = relativeSize;

                            if (rowType == KitchenDisplayStation.DisplayRowTypeEnum.LineDisplayRow)
                            {
                                string lineDisplayCaption = (string)drRowParts["CAPTION"];
                                part.ColumnCaption = lineDisplayCaption;
                            }
                            row.Parts.Add(part);
                        }
                        drRowParts.Close();
                        cmdParts.Dispose();

                    }
                    if (partcount > 0)
                        switch (rowType)
                        {
                            case KitchenDisplayStation.DisplayRowTypeEnum.ChitHeader:
                                visualProfile.ChitHeaderRows.Add(row);
                                break;
                            case KitchenDisplayStation.DisplayRowTypeEnum.ChitFooter:
                                visualProfile.ChitFooterRows.Add(row);
                                break;
                            case KitchenDisplayStation.DisplayRowTypeEnum.LineDisplayRow:
                                visualProfile.LineDisplayRows.Add(row);
                                break;
                        }
                }
                drRows.Close();
                cmdRows.Dispose();

                if ((rowcount == 0) || (partcount == 0))
                {
                    // If there are no rows or parts in db 
                    GetDefaultDisplayProfileData(visualProfile, visualProfileId);
                }
            }            
        }

        private static void GetDefaultDisplayProfileData(KitchenDisplayVisualProfile visualProfile, RecordIdentifier visualProfileId)
        {
            KitchenDisplayHeaderFooterRow row = new KitchenDisplayHeaderFooterRow();

            if (visualProfileId.HasSecondaryID)
            {
                // Only customer facing displays have a secondary profile id
                // The reason for treating customer facing displays specially is because they do not have 
                // the same info in header and footer

                row.Parts.Add(new KitchenDisplayHeaderFooterRowPart
                {
                    MappingKey = "TransactionNumber",
                    Alignment = PartAlignmentEnum.Center
                });
                visualProfile.ChitHeaderRows.Add(row);

                row = new KitchenDisplayHeaderFooterRow();
                row.Parts.Add(new KitchenDisplayHeaderFooterRowPart
                {
                    Alignment = PartAlignmentEnum.Left,
                    MappingKey = "CustomerName"
                });
                visualProfile.ChitFooterRows.Add(row);

                row = new KitchenDisplayHeaderFooterRow();
                row.Parts.Add(new KitchenDisplayHeaderFooterRowPart
                {
                    PartType = PartTypeEnum.CountDownClockDisplayCountUp,
                    Alignment = PartAlignmentEnum.Right,
                    Style = new BaseStyle()
                    {
                        Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold)
                    }
                });
                visualProfile.ChitFooterRows.Add(row);
            }
            else
            {
                // NOTE: There's no actual Web Service / NAV communication. This just seems to be two different hard-coded styles. We can keep this commented until we know which 
                // style we want

                //if ((kdsDatabaseType == KDSDatabaseTypeEnum.LSNav) || (kdsDatabaseType == KDSDatabaseTypeEnum.WebService))
                //{
                //    // This is the previously hardcoded look for NAV (Hard Rock look)
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart { MappingKey = "TableNo" });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart()
                //    {
                //        MappingKey = "HospTypeText",
                //        Style = new BaseStyle()
                //        {
                //            Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold)
                //        }
                //    });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart { PartType = PartTypeEnum.CountDownClockDisplayCountUp });
                //    visualProfile.HeaderRows.Add(row);

                //    row = new KitchenDisplayHeaderFooterRow();
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart
                //    {
                //        PartType = PartTypeEnum.StationStatus,
                //        Style = new BaseStyle
                //        {
                //            FontStyle = System.Drawing.FontStyle.Bold
                //        }
                //    });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart { PartType = PartTypeEnum.CountDownClock });
                //    visualProfile.HeaderRows.Add(row);

                //    row = new KitchenDisplayHeaderFooterRow();
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart { MappingKey = "EmployeeName" });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart { MappingKey = "POSOrderStatus", Style = new BaseStyle { ForeColor = Color.Red } });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart { MappingKey = "ReceiptNumber" });

                //    visualProfile.FooterRows.Add(row);

                //    //Line Display Default values
                //    row = new KitchenDisplayHeaderFooterRow();
                //    BaseStyle style = new BaseStyle() { Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular) };

                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.Generic, OrderProperty = PartOrderPropertyEnum.TableNumber, ColumnCaption = "Table", RelativeSize = 4 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.Generic, OrderProperty = PartOrderPropertyEnum.ID, ColumnCaption = "KOT#", RelativeSize = 15 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.CountDownClock, ColumnCaption = "Display time", RelativeSize = 7 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.CountDownClockDisplayCountUp, ColumnCaption = "Time left", RelativeSize = 7 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.StationStatus, ColumnCaption = "Status", RelativeSize = 5 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.Generic, OrderProperty = PartOrderPropertyEnum.ItemQuantity, ColumnCaption = "Quantity", RelativeSize = 6 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.Generic, OrderProperty = PartOrderPropertyEnum.ItemText, ColumnCaption = "Item", RelativeSize = 30 });
                //    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.Generic, OrderProperty = PartOrderPropertyEnum.ItemModifiers, ColumnCaption = "Modifiers", RelativeSize = 26 });

                //    visualProfile.LineDisplayRows.Add(row);
                //}
                //else
                //{
                    // This is the previously hardcoded look for LS First
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { MappingKey = "EmployeeName" });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.SVG, MappingKey = "Priority" });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart()
                    {
                        PartType = PartTypeEnum.CountDownClockDisplayCountUp,
                        Style = new BaseStyle()
                        {
                            Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold)
                        }
                    });
                    visualProfile.ChitHeaderRows.Add(row);

                    row = new KitchenDisplayHeaderFooterRow();
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart()
                    {
                        PartType = PartTypeEnum.StationStatus,
                        Style = new BaseStyle()
                        {
                            FontStyle = System.Drawing.FontStyle.Bold
                        }
                    });

                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart()
                    {
                        MappingKey = "HospitalityTypeText",
                        Style = new BaseStyle()
                        {
                            Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold)
                        }
                    });

                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { MappingKey = "TableNo" });
                    visualProfile.ChitHeaderRows.Add(row);

                    row = new KitchenDisplayHeaderFooterRow();
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { MappingKey = "TransactionNumber" });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { MappingKey = "Recalled" });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart()
                    {
                        MappingKey = "POSOrderStatus",
                        Style = new BaseStyle()
                        {
                            Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold),
                            ForeColor = Color.Red
                        }
                    });

                    visualProfile.ChitFooterRows.Add(row);

                    visualProfile.ItemNestingIndent = 10;
                    visualProfile.ItemModifierNestingIndent = 0;

                    //Line Display Default values
                    row = new KitchenDisplayHeaderFooterRow();
                    BaseStyle style = new BaseStyle() { Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular) };

                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { OrderProperty = PartOrderPropertyEnum.TableNumber, ColumnCaption = "Table", RelativeSize = 4 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.CountDownClock, ColumnCaption = "Display time", RelativeSize = 8 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { PartType = PartTypeEnum.StationStatus, ColumnCaption = "Status", RelativeSize = 5 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { OrderProperty = PartOrderPropertyEnum.ItemQuantity, ColumnCaption = "Quantity", RelativeSize = 6 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { OrderProperty = PartOrderPropertyEnum.ItemText, ColumnCaption = "Item", RelativeSize = 38 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { OrderProperty = PartOrderPropertyEnum.ItemModifiers, ColumnCaption = "Modifiers", RelativeSize = 28 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { MappingKey = "DeliveryType", ColumnCaption = "Delivery", RelativeSize = 7 });
                    row.Parts.Add(new KitchenDisplayHeaderFooterRowPart() { MappingKey = "PaymentStatus", ColumnCaption = "Status", RelativeSize = 4 });

                    visualProfile.LineDisplayRows.Add(row);
                //}
            }
        }

        public virtual List<KitchenDisplayVisualProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var results = Execute<KitchenDisplayVisualProfile>(entry, cmd, CommandType.Text, PopulateProfile);

                return results;
            }
        }

        public virtual List<KitchenDisplayVisualProfile> GetList(IConnectionManager entry, RecordIdentifier headerPaneID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where HEADERPANEID = @headerPaneId";

                MakeParam(cmd, "headerPaneId", headerPaneID);

                var results = Execute<KitchenDisplayVisualProfile>(entry, cmd, CommandType.Text, PopulateProfile);

                return results;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier profileId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYVISUALPROFILE", "ID", profileId, BusinessObjects.Permission.ManageKitchenDisplayProfiles);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier profileId)
        {
            return RecordExists(entry, "KITCHENDISPLAYVISUALPROFILE", "ID", profileId);
        }

        public virtual void Save(IConnectionManager entry, KitchenDisplayVisualProfile kitchenDisplayVisualProfile)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYVISUALPROFILE");
            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            bool isNew = false;
            if (kitchenDisplayVisualProfile.ID.IsEmpty)
            {
                kitchenDisplayVisualProfile.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, kitchenDisplayVisualProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)kitchenDisplayVisualProfile.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)kitchenDisplayVisualProfile.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", kitchenDisplayVisualProfile.Text);
            statement.AddField("ORDERPANEWIDTH", kitchenDisplayVisualProfile.OrderPaneWidth, SqlDbType.Decimal);
            statement.AddField("ORDERPANEHEIGHT", kitchenDisplayVisualProfile.OrderPaneHeight, SqlDbType.Decimal);
            statement.AddField("ORDERPANEX", kitchenDisplayVisualProfile.OrderPaneX, SqlDbType.Decimal);
            statement.AddField("ORDERPANEY", kitchenDisplayVisualProfile.OrderPaneY, SqlDbType.Decimal);
            statement.AddField("ORDERPANEVISIBLE", kitchenDisplayVisualProfile.OrderPaneVisible ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BUTTONPANEWIDTH", kitchenDisplayVisualProfile.ButtonPaneWidth, SqlDbType.Decimal);
            statement.AddField("BUTTONPANEHEIGHT", kitchenDisplayVisualProfile.ButtonPaneHeight, SqlDbType.Decimal);
            statement.AddField("BUTTONPANEX", kitchenDisplayVisualProfile.ButtonPaneX, SqlDbType.Decimal);
            statement.AddField("BUTTONPANEY", kitchenDisplayVisualProfile.ButtonPaneY, SqlDbType.Decimal);
            statement.AddField("BUTTONPANEVISIBLE", kitchenDisplayVisualProfile.ButtonPaneVisible ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BUTTONPANEPOSITION", kitchenDisplayVisualProfile.ButtonPanePosition, SqlDbType.TinyInt);
            statement.AddField("NUMBEROFCOLUMNS", kitchenDisplayVisualProfile.NumberOfColumns, SqlDbType.Int);
            statement.AddField("NUMBEROFROWS", kitchenDisplayVisualProfile.NumberOfRows, SqlDbType.Int);
            statement.AddField("AGGREGATEPANEWIDTH", kitchenDisplayVisualProfile.AggregatePaneWidth, SqlDbType.Decimal);
            statement.AddField("AGGREGATEPANEHEIGHT", kitchenDisplayVisualProfile.AggregatePaneHeight, SqlDbType.Decimal);
            statement.AddField("AGGREGATEPANEX", kitchenDisplayVisualProfile.AggregatePaneX, SqlDbType.Decimal);
            statement.AddField("AGGREGATEPANEY", kitchenDisplayVisualProfile.AggregatePaneY, SqlDbType.Decimal);
            statement.AddField("AGGREGATEPANEVISIBLE", kitchenDisplayVisualProfile.AggregatePaneVisible, SqlDbType.TinyInt);
            statement.AddField("AGGREGATEPANENUMBEROFCOLUMNS", kitchenDisplayVisualProfile.AggregatePaneNumberofColumns, SqlDbType.Int);
            statement.AddField("AGGREGATEPANEPOSITION", kitchenDisplayVisualProfile.AggregatePanePosition, SqlDbType.Int);
            statement.AddField("ITEMMODIFIERINCREASEPREFIX", kitchenDisplayVisualProfile.ItemModifierIncreasePrefix);
            statement.AddField("ITEMMODIFIERDECREASEPREFIX", kitchenDisplayVisualProfile.ItemModifierDecreasePrefix);
            statement.AddField("ITEMMODIFIERNORMALPREFIX", kitchenDisplayVisualProfile.ItemModifierNormalPrefix);
            statement.AddField("SHOWDEALS", kitchenDisplayVisualProfile.ShowDeals ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SHOWNAME", kitchenDisplayVisualProfile.ShowName ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LANGUAGECODE", kitchenDisplayVisualProfile.LanguageCode);
            statement.AddField("CHITREFRESHRATE", kitchenDisplayVisualProfile.ChitRefreshRate, SqlDbType.Int);
            statement.AddField("CHITSIZE", kitchenDisplayVisualProfile.ChitSize, SqlDbType.Int);
            statement.AddField("ITEMMODIFIERHEAVYPREFIX", kitchenDisplayVisualProfile.ItemModifierHeavyPrefix);
            statement.AddField("ITEMMODIFIERLIGHTPREFIX", kitchenDisplayVisualProfile.ItemModifierLightPrefix);
            statement.AddField("ITEMMODIFIERONLYPREFIX", kitchenDisplayVisualProfile.ItemModifierOnlyPrefix);
            statement.AddField("ITEMMODIFIERDONEPREFIX", kitchenDisplayVisualProfile.ItemModifierDonePrefix);
            statement.AddField("HISTORYPANEWIDTH", kitchenDisplayVisualProfile.HistoryPaneWidth, SqlDbType.Decimal);
            statement.AddField("HISTORYPANEHEIGHT", kitchenDisplayVisualProfile.HistoryPaneHeight, SqlDbType.Decimal);
            statement.AddField("HISTORYPANEX", kitchenDisplayVisualProfile.HistoryPaneX, SqlDbType.Decimal);
            statement.AddField("HISTORYPANEY", kitchenDisplayVisualProfile.HistoryPaneY, SqlDbType.Decimal);
            statement.AddField("HISTORYPANEVISIBLE", kitchenDisplayVisualProfile.HistoryPaneVisible, SqlDbType.TinyInt);
            statement.AddField("HISTORYPANEPOSITION", kitchenDisplayVisualProfile.HistoryPanePosition, SqlDbType.Int);
            statement.AddField("HISTORYMAXSUMMARYLINES", kitchenDisplayVisualProfile.HistoryMaxSummaryLines, SqlDbType.Int);
            statement.AddField("HISTORYMAXLASTBUMPEDLINES", kitchenDisplayVisualProfile.HistoryMaxLastBumpedLines, SqlDbType.Int);
            statement.AddField("HISTORYLIFESPAN", kitchenDisplayVisualProfile.HistoryLifespanInMinutes, SqlDbType.Int);
            statement.AddField("HEADERPANEHEIGHT", kitchenDisplayVisualProfile.HeaderPaneHeight, SqlDbType.Decimal);
            statement.AddField("HEADERPANEX", kitchenDisplayVisualProfile.HeaderPaneX, SqlDbType.Decimal);
            statement.AddField("HEADERPANEY", kitchenDisplayVisualProfile.HeaderPaneY, SqlDbType.Decimal);

            if (kitchenDisplayVisualProfile.HeaderProfileId != null)
            {
                statement.AddField("HEADERPANEID", Guid.Parse(kitchenDisplayVisualProfile.HeaderProfileId), SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("HEADERPANEID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
