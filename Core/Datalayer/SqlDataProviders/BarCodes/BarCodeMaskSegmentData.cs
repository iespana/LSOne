using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.BarCodes
{
    public class BarCodeMaskSegmentData : SqlServerDataProviderBase, IBarCodeMaskSegmentData
    {

        protected virtual void PopulateSegmentWithCount(IConnectionManager entry, IDataReader dr, BarcodeMaskSegment barcodeMask, ref int rowCount)
        {
            PopulateSegment(dr, barcodeMask);
            PopulateRowCount(entry, dr, ref rowCount);
        }

        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        protected virtual void PopulateSegment(IDataReader dr, BarcodeMaskSegment segment)
        {
            segment.MaskId = (string)dr["MASKID"];
            segment.SegmentNum = (int)dr["SEGMENTNUM"];
            segment.Length = Convert.ToInt32(dr["LENGTH"]);
            segment.Type = (BarcodeSegmentType)dr["TYPE"];
            segment.Decimals = (int)dr["DECIMALS"];
            segment.SegmentChar = (string)dr["CHAR"];
            segment.MaskSequence = (int) dr["MASKSEQUENCE"];
            segment.Sequence = (int) dr["SEQUENCE"];
        }

        public virtual List<BarcodeMaskSegment> GetAllSegments(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, Permission.ManageItemBarcodes);

                cmd.CommandText =
                   @"select rbs.MASKID,
                            rbs.SEGMENTNUM, 
                            ISNULL(rbs.LENGTH,0) as LENGTH,
                            ISNULL(rbs.TYPE,0) as TYPE,
                            ISNULL(rbs.DECIMALS,0) as DECIMALS,
                            ISNULL(rbs.CHAR,'') as CHAR,
	                        rbm.SEQUENCE as MASKSEQUENCE,
                            rbs.SEQUENCE
                     from RBOBARCODEMASKSEGMENT rbs
                     join RBOBARCODEMASKTABLE rbm on rbm.MASKID = rbs.MASKID and rbm.DATAAREAID = rbs.DATAAREAID
                     where rbs.DATAAREAID = @dataAreaId
                     order by rbs.MASKID, rbs.SEGMENTNUM";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);                

                return Execute<BarcodeMaskSegment>(entry, cmd, CommandType.Text, PopulateSegment);
            }
        }

        public virtual List<BarcodeMaskSegment> LoadSegments(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecordsMatching)
        {
            List<TableColumn> columns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "MASKID", TableAlias = "m"},
                new TableColumn {ColumnName = "SEGMENTNUM", TableAlias = "m"},
                new TableColumn {ColumnName = "LENGTH", IsNull = true, NullValue = "0", TableAlias = "m"},
                new TableColumn {ColumnName = "TYPE", IsNull = true, NullValue = "0", TableAlias = "m"},
                new TableColumn {ColumnName = "DECIMALS", IsNull = true, NullValue = "0", TableAlias = "m"},
                new TableColumn {ColumnName = "CHAR", IsNull = true, NullValue = "''", TableAlias = "m"},
                new TableColumn {ColumnName = "SEQUENCE", TableAlias = "m"},
                new TableColumn {ColumnName = "SEQUENCE", TableAlias = "ms", ColumnAlias = "MASKSEQUENCE"},
                
            };

            columns.Add(new TableColumn
            {
                ColumnName = "ROW_NUMBER() OVER(order by m.MASKID, m.SEGMENTNUM)",
                ColumnAlias = "ROW"
            });
            columns.Add(new TableColumn
            {
                ColumnName =
                    "COUNT(1) OVER(ORDER BY m.MASKID, m.SEGMENTNUM RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
                ColumnAlias = "ROW_COUNT"
            });

            List<Condition> externalConditions = new List<Condition>();
            externalConditions.Add(new Condition
            {
                Operator = "AND",
                ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
            });


            List<Join> joins = new List<Join>();
            joins.Add(new Join
            {
                Table = "RBOBARCODEMASKTABLE",
                TableAlias = "ms",
                Condition = "ms.MASKID = m.MASKID and ms.DATAAREAID = m.DATAAREAID"
            });

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RBOBARCODEMASKSEGMENT", "m", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                int matchingRecords = 0;

                List<BarcodeMaskSegment> reply = Execute<BarcodeMaskSegment, int>(entry, cmd, CommandType.Text, ref matchingRecords,
                    PopulateSegmentWithCount);

                totalRecordsMatching = matchingRecords;
                return reply;
            }
        }

        public virtual List<BarcodeMaskSegment> Get(IConnectionManager entry, RecordIdentifier maskID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, Permission.ManageItemBarcodes);

                cmd.CommandText =
                    @"select rbs.MASKID,
                             rbs.SEGMENTNUM, 
                             ISNULL(rbs.LENGTH,0) as LENGTH,
                             ISNULL(rbs.TYPE,0) as TYPE,
                             ISNULL(rbs.DECIMALS,0) as DECIMALS,
                             ISNULL(rbs.CHAR,'') as CHAR,
	                         rbm.SEQUENCE as MASKSEQUENCE,
                             rbs.SEQUENCE
                      from RBOBARCODEMASKSEGMENT rbs
                      join RBOBARCODEMASKTABLE rbm on rbm.MASKID = rbs.MASKID and rbm.DATAAREAID = rbs.DATAAREAID
                      where rbs.DATAAREAID = @dataAreaId  and rbs.MASKID = @maskID
                      order by rbs.MASKID, rbs.SEGMENTNUM";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "maskID", (string) maskID);

                return Execute<BarcodeMaskSegment>(entry, cmd, CommandType.Text, PopulateSegment);
            }
        }

        public virtual void DeleteAllSegments(IConnectionManager entry, RecordIdentifier barCodeMaskID)
        {
            DeleteRecord(entry, "RBOBARCODEMASKSEGMENT", "MASKID", barCodeMaskID, Permission.ManageBarcodesMasks);
        }

        public virtual void Save(IConnectionManager entry, RecordIdentifier barCodeMaskID,List<BarcodeMaskSegment> segments)
        {
            ValidateSecurity(entry, Permission.ManageBarcodesMasks);            

            foreach (BarcodeMaskSegment segment in segments)
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    var statement = entry.Connection.CreateStatement("RBOBARCODEMASKSEGMENT");

                    if (!Exists(entry, segment.UniqueID))
                    {
                        statement.StatementType = StatementType.Insert;

                        statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                        statement.AddKey("MASKID", (string)segment.MaskId);
                        statement.AddKey("SEGMENTNUM", (int)segment.SegmentNum, SqlDbType.Int);
                    }
                    else
                    {
                        statement.StatementType = StatementType.Update;

                        statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                        statement.AddCondition("MASKID", (string)segment.MaskId);

                        // This is done because the user might be re-arranging the barcode mask segments. Then we cannot have the SEGMENTNUM column
                        // as a condition because we might want to update it
                        statement.AddCondition("SEQUENCE", (int)segment.Sequence, SqlDbType.Int);
                        statement.AddField("SEGMENTNUM", (int)segment.SegmentNum, SqlDbType.Int);
                    }
                    
                    statement.AddField("LENGTH", segment.Length, SqlDbType.Int);
                    statement.AddField("TYPE", (int) segment.Type, SqlDbType.Int);
                    statement.AddField("DECIMALS", (int) segment.Decimals, SqlDbType.Int);
                    statement.AddField("CHAR", segment.SegmentChar);

                    entry.Connection.ExecuteStatement(statement);
                }
            }  
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier barcodeMaskSegmentID)
        {
            DeleteRecord(entry, "RBOBARCODEMASKSEGMENT", new string[] { "MASKID", "SEQUENCE" }, barcodeMaskSegmentID, Permission.ManageBarcodesMasks);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier barcodeMaskSegmentID)
        {
            return RecordExists(entry, "RBOBARCODEMASKSEGMENT", new string[] {"MASKID", "SEQUENCE"}, barcodeMaskSegmentID);
        }
    }
}
