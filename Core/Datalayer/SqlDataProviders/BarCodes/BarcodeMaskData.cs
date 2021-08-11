using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.BarCodes
{
    public class BarcodeMaskData : SqlServerDataProviderBase, IBarcodeMaskData
    {
        protected virtual void PopulateBarcodeMaskWithCount(IConnectionManager entry, IDataReader dr, BarcodeMask barcodeMask, ref int rowCount)
        {
            PopulateBarcodeMask(dr, barcodeMask);
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

        protected virtual void PopulateBarcodeMask(IDataReader dr, BarcodeMask barcodeMask)
        {
            barcodeMask.Text = (string) dr["DESCRIPTION"];
            barcodeMask.Mask = (string) dr["MASK"];
            barcodeMask.Prefix = (string) dr["PREFIX"];
            barcodeMask.ID = (string) dr["MASKID"];
            barcodeMask.InternalType = (BarcodeInternalType) dr["TYPE"];
            barcodeMask.Type = (BarcodeType) dr["SYMBOLOGY"];
            barcodeMask.Found = true;
            barcodeMask.Sequence = (int) dr["SEQUENCE"];
        }

        public virtual bool MaskExists(IConnectionManager entry, string mask, RecordIdentifier excludeID)
        {
            if (excludeID.IsEmpty)
            {
                return RecordExists(entry, "RBOBARCODEMASKTABLE", "MASK", mask);
            }
            return RecordExists(entry, "RBOBARCODEMASKTABLE", "MASK", mask, new string[] { "MASKID" }, new RecordIdentifier[] { excludeID });
        }

        public virtual bool PrefixExists(IConnectionManager entry, string mask, RecordIdentifier excludeID)
        {
            if (excludeID.IsEmpty)
            {
                return RecordExists(entry, "RBOBARCODEMASKTABLE", "PREFIX", mask);
            }
            return RecordExists(entry, "RBOBARCODEMASKTABLE", "PREFIX", mask, new string[] { "MASKID" }, new RecordIdentifier[] { excludeID });
        }

        public virtual List<BarcodeMask> LoadBarcodeMasks(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecordsMatching)
        {
            List<TableColumn> columns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "MASKID", TableAlias = "m"},
                new TableColumn {ColumnName = "DESCRIPTION", IsNull = true, NullValue = "''", TableAlias = "m"},
                new TableColumn {ColumnName = "MASK", IsNull = true, NullValue = "''", TableAlias = "m"},
                new TableColumn {ColumnName = "PREFIX", IsNull = true, NullValue = "''", TableAlias = "m"},
                new TableColumn {ColumnName = "SYMBOLOGY", IsNull = true, NullValue = "0", TableAlias = "m"},
                new TableColumn {ColumnName = "TYPE", IsNull = true, NullValue = "0", TableAlias = "m"},
                new TableColumn {ColumnName = "SEQUENCE", IsNull = true, NullValue = "1", TableAlias = "m"}
            };

            columns.Add(new TableColumn
            {
                ColumnName = "ROW_NUMBER() OVER(order by m.MASKID)",
                ColumnAlias = "ROW"
            });
            columns.Add(new TableColumn
            {
                ColumnName =
                    "COUNT(1) OVER(ORDER BY m.MASKID RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
                ColumnAlias = "ROW_COUNT"
            });

            List<Condition> externalConditions = new List<Condition>();
            externalConditions.Add(new Condition
            {
                Operator = "AND",
                ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
            });


            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RBOBARCODEMASKTABLE", "m", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                int matchingRecords = 0;

                List<BarcodeMask> reply = Execute<BarcodeMask, int>(entry, cmd, CommandType.Text, ref matchingRecords,
                    PopulateBarcodeMaskWithCount);

                totalRecordsMatching = matchingRecords;
                return reply;
            }
        }

        public virtual BarcodeMask GetMaskForBarcode(IConnectionManager entry, BarCode barCode, CacheType cache = CacheType.CacheTypeNone)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(DESCRIPTION, '') AS DESCRIPTION, 
                                           ISNULL(MASK, '') AS MASK, 
                                           ISNULL(MASKID, '') AS MASKID, 
                                           ISNULL(PREFIX, '') AS PREFIX, 
                                           ISNULL(SYMBOLOGY, 0) AS SYMBOLOGY, 
                                           ISNULL(TYPE, 0) AS TYPE,
                                           ISNULL(SEQUENCE, 1) AS SEQUENCE
                                    FROM RBOBARCODEMASKTABLE 
                                    WHERE ((PREFIX = SUBSTRING(@barcode, 1, LEN(PREFIX)) AND PREFIX <> '') OR @barcode = PREFIX) 
                                    AND LEN(MASK) = LEN(@barcode)
                                    AND DATAAREAID = @dataAreaID 
                                    ORDER BY MASK";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "barcode", barCode.ID);
                return Get<BarcodeMask>(entry, cmd, barCode.ID, PopulateBarcodeMask, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual List<BarcodeMask> GetBarCodeMasks(IConnectionManager entry, int sortBy, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sort = "";

                switch (sortBy)
                {
                    case 0:
                        sort = "MASKID ASC";
                        break;

                    case 1:
                        sort = "DESCRIPTION ASC";
                        break;

                    case 2:
                        sort = "TYPE ASC";
                        break;

                    case 3:
                        sort = "SYMBOLOGY ASC";
                        break;

                    case 4:
                        sort = "PREFIX ASC";
                        break;

                    case 6:
                        sort = "LEN(MASK) ASC";
                        break;

                    default:
                        sort = "MASK ASC";
                        break;
                }

                if (sort != "")
                {
                    if (backwardsSort)
                    {
                        sort = sort.Replace("ASC", "DESC");
                    }

                    sort = " order by " + sort;
                }

                ValidateSecurity(entry);

                cmd.CommandText =
                    "select MASKID, ISNULL(DESCRIPTION,'') as DESCRIPTION," +
                    "ISNULL(MASK,'') as MASK,ISNULL(PREFIX,'') as PREFIX,ISNULL(TYPE,'') as TYPE," +
                    "ISNULL(SYMBOLOGY,0) as SYMBOLOGY," +
                    "ISNULL(SEQUENCE, 1) AS SEQUENCE " +
                    "from RBOBARCODEMASKTABLE " +
                    "where DataareaID = @dataAreaId " +
                    sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<BarcodeMask>(entry, cmd, CommandType.Text, PopulateBarcodeMask);
            }
        }


        public virtual BarcodeMask Get(IConnectionManager entry, RecordIdentifier barcodeMaskID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select MASKID, ISNULL(DESCRIPTION,'') as DESCRIPTION," +
                    "ISNULL(MASK,'') as MASK,ISNULL(PREFIX,'') as PREFIX,ISNULL(TYPE,'') as TYPE," +
                    "ISNULL(SYMBOLOGY,0) as SYMBOLOGY," +
                    "ISNULL(SEQUENCE, 1) AS SEQUENCE " +
                    "from RBOBARCODEMASKTABLE " +
                    "where DataareaID = @dataAreaId and MASKID = @maskID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "maskID", (string) barcodeMaskID);

                var result = Execute<BarcodeMask>(entry, cmd, CommandType.Text, PopulateBarcodeMask);

                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier barCode)
        {
            return RecordExists(entry, "RBOBARCODEMASKTABLE", "MASKID", barCode);
        }

        public virtual bool BarCodeMaskInUse(IConnectionManager entry, RecordIdentifier barCodeMask)
        {
            return RecordExists(entry, "BARCODESETUP", "RBOBARCODEMASK", barCodeMask);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier barCodeMaskID)
        {
            DeleteRecord(entry, "RBOBARCODEMASKTABLE", "MASKID", barCodeMaskID, BusinessObjects.Permission.ManageBarcodesMasks);
            DeleteRecord(entry, "RBOBARCODEMASKSEGMENT", "MASKID", barCodeMaskID, BusinessObjects.Permission.ManageBarcodesMasks);
        }

        public virtual void Save(IConnectionManager entry, BarcodeMask barCodeMask)
        {
            bool isNew = false;
            var statement = entry.Connection.CreateStatement("RBOBARCODEMASKTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageBarcodesMasks);

            if (barCodeMask.ID.IsEmpty)
            {
                isNew = true;
                barCodeMask.ID = DataProviderFactory.Instance.GenerateNumber<IBarcodeMaskData, BarcodeMask>(entry);
            }

            if (isNew || !Exists(entry, barCodeMask.ID))
            {
                if (MaskExists(entry, barCodeMask.Mask, RecordIdentifier.Empty))
                {
                    throw new DataIntegrityException("You can not add multiple barcodes with the same mask. Mask: " + barCodeMask.Mask);
                }

                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("MASKID", (string)barCodeMask.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("MASKID", (string)barCodeMask.ID);
            }

            statement.AddField("DESCRIPTION", barCodeMask.Text);
            statement.AddField("MASK", barCodeMask.Mask);
            statement.AddField("PREFIX", barCodeMask.Prefix);
            statement.AddField("TYPE", (int)barCodeMask.InternalType, SqlDbType.Int);
            statement.AddField("SYMBOLOGY", (int)barCodeMask.Type, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return DataProviderFactory.Instance.Get<IBarcodeMaskData, BarcodeMask>().Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "BARCODEMASK"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOBARCODEMASKTABLE", "MASKID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
