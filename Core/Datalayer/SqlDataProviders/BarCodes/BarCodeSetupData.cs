using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.BarCodes
{
    public class BarCodeSetupData : SqlServerDataProviderBase, IBarCodeSetupData
    {
        private static string BaseSql
        {
            get
            {
                return "select distinct bc.BARCODESETUPID, ISNULL(bc.MINIMUMLENGTH,0) as MINIMUMLENGTH," +
                "ISNULL(bc.MAXIMUMLENGTH,0) as MAXIMUMLENGTH,ISNULL(bc.RBOBARCODEMASK,'') as RBOBARCODEMASK," +
                "ISNULL(bc.BARCODETYPE,0) as BARCODETYPE,ISNULL(bc.FONTNAME,'') as FONTNAME," +
                "ISNULL(bc.FONTSIZE,0) as FONTSIZE,ISNULL(bc.DESCRIPTION,'') as DESCRIPTION, " +
                "ISNULL(bcm.DESCRIPTION,'') as BARCODEMASKDESCRIPTION, " +
                "ISNULL(bcm.MASKID,'') as BARCODEMASKID " +
                "from BARCODESETUP bc " +
                "left outer join RBOBARCODEMASKTABLE bcm on bc.DATAAREAID = bcm.DATAAREAID and bc.RBOBARCODEMASK = bcm.MASK ";
            }
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "BARCODESETUP", "DESCRIPTION", "BARCODESETUPID", "BARCODESETUPID");
        }

        private static void PopulateItem(IDataReader dr, BarCodeSetup barCodeSetup)
        {
            barCodeSetup.ID = (string)dr["BARCODESETUPID"];
            barCodeSetup.MinimumLength = (int)dr["MINIMUMLENGTH"];

            barCodeSetup.MaximumLength = (int)dr["MAXIMUMLENGTH"];
            barCodeSetup.BarCodeMask = (string)dr["RBOBARCODEMASK"];
            barCodeSetup.BarCodeMaskDescription = (string)dr["BARCODEMASKDESCRIPTION"];
            barCodeSetup.BarCodeType = (int)dr["BARCODETYPE"];
            barCodeSetup.FontName = (string)dr["FONTNAME"];
            barCodeSetup.FontSize = (int)dr["FONTSIZE"];
            barCodeSetup.Description = (string)dr["DESCRIPTION"];
            barCodeSetup.BarCodeMaskID = (string)dr["BARCODEMASKID"];
        }

        public virtual BarCodeSetup Get(IConnectionManager entry, RecordIdentifier barCodeSetupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageItemBarcodes);

                cmd.CommandText =
                    BaseSql + "where bc.DataareaID = @dataAreaId and bc.BARCODESETUPID = @barCodeSetupID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "barCodeSetupID", (string) barCodeSetupID);

                var records = Execute<BarCodeSetup>(entry, cmd, CommandType.Text, PopulateItem);

                return (records.Count > 0) ? records[0] : null;
            }
        }

        public virtual List<BarCodeSetup> GetBarcodes(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ManageItemBarcodes);

                cmd.CommandText =
                    BaseSql + "where bc.DataareaID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<BarCodeSetup>(entry, cmd, CommandType.Text, PopulateItem);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier barCodeSetupID)
        {
            return RecordExists(entry, "BARCODESETUP", "BARCODESETUPID", barCodeSetupID);
        }

        public virtual bool BarCodeSetupInUse(IConnectionManager entry, RecordIdentifier barCodeSetupID)
        {
            return RecordExists(entry, "RETAILITEM", "BARCODESETUPID", barCodeSetupID, false);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier barCodeSetupID)
        {
            DeleteRecord(entry, "BARCODESETUP", "BARCODESETUPID", barCodeSetupID, BusinessObjects.Permission.ManageItemBarcodes);
        }

        public virtual void Save(IConnectionManager entry, BarCodeSetup barCodeSetup)
        {
            var statement = new SqlServerStatement("BARCODESETUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageItemBarcodes);

            barCodeSetup.Validate();

            bool isNew = false;
            if (barCodeSetup.ID.IsEmpty)
            {
                isNew = true;
                barCodeSetup.ID =
                    DataProviderFactory.Instance.GenerateNumber<IBarCodeSetupData, BarCodeSetup>(entry);
            }

            if (isNew || !Exists(entry,barCodeSetup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("BARCODESETUPID", (string)barCodeSetup.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("BARCODESETUPID", (string)barCodeSetup.ID);
            }

            statement.AddField("DESCRIPTION", barCodeSetup.Text);
            statement.AddField("MINIMUMLENGTH", barCodeSetup.MinimumLength,SqlDbType.Int);
            statement.AddField("MAXIMUMLENGTH", barCodeSetup.MaximumLength,SqlDbType.Int);
            statement.AddField("RBOBARCODEMASK", barCodeSetup.BarCodeMask);
            statement.AddField("BARCODETYPE", barCodeSetup.BarCodeType,SqlDbType.Int);
            statement.AddField("FONTNAME", barCodeSetup.FontName);
            statement.AddField("FONTSIZE", barCodeSetup.FontSize, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "BARCODESETUP"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "BARCODESETUP", "BARCODESETUPID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

        public virtual void Save(IConnectionManager entry, BarCodeSetup item, RecordIdentifier oldID)
        {
            throw new System.NotImplementedException();
        }
    }
}
