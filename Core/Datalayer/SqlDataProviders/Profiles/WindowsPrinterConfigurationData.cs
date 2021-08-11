using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Data;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class WindowsPrinterConfigurationData : SqlServerDataProviderBase, IWindowsPrinterConfigurationData
    {
        private string BaseSQL = @"SELECT W.ID, W.DESCRIPTION,
                                    ISNULL(W.DEVICENAME, '') AS DEVICENAME,
                                    ISNULL(W.FONTNAME, 'Courier New') AS FONTNAME,
                                    ISNULL(W.FONTSIZE, '9.5') AS FONTSIZE,
                                    ISNULL(W.WIDEHIGHFONTSIZE, '18') AS WIDEHIGHFONTSIZE,
                                    ISNULL(W.LEFTMARGIN, '45') AS LEFTMARGIN,
                                    ISNULL(W.RIGHTMARGIN, '5') AS RIGHTMARGIN,
                                    ISNULL(W.TOPMARGIN, '5') AS TOPMARGIN,
                                    ISNULL(W.BOTTOMMARGIN, '60') AS BOTTOMMARGIN,
                                    ISNULL(W.PRINTDESIGNBOXES, 0) AS PRINTDESIGNBOXES,
                                    ISNULL(W.FOLDERLOCATION, '') AS FOLDERLOCATION,
                                    CAST(CASE WHEN EXISTS (SELECT 1 FROM POSHARDWAREPROFILE H WHERE H.WINDOWSPRINTERCONFIGURATIONID = W.ID)
                                                OR EXISTS (SELECT 1 FROM RESTAURANTSTATION R WHERE R.WINDOWSPRINTERCONFIGURATIONID = W.ID)
	        	                        THEN 1
	        	                        ELSE 0
	                                END AS BIT) AS CONFIGURATIONUSED
                                    FROM WINDOWSPRINTERCONFIGURATION W";

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            if (!(entry.HasPermission(Permission.HardwareProfileEdit) || entry.HasPermission(Permission.ManageStationPrinting)))
            {
                throw new PermissionException(Permission.HardwareProfileEdit + " or " + Permission.ManageStationPrinting);
            }

            DeleteRecord(entry, "WINDOWSPRINTERCONFIGURATION", "ID", ID, "", false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "WINDOWSPRINTERCONFIGURATION", "ID", ID, false);
        }

        public virtual WindowsPrinterConfiguration Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + " WHERE W.ID = @id";
                MakeParam(cmd, "id", (string)id);
                return Get<WindowsPrinterConfiguration>(entry, cmd, id, PopulateWindowsPrinterConfiguration, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<WindowsPrinterConfiguration> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + " ORDER BY W.DESCRIPTION";
                return Execute<WindowsPrinterConfiguration>(entry, cmd, CommandType.Text, PopulateWindowsPrinterConfiguration);
            }
        }

        public virtual List<DataEntity> GetDataEntityList(IConnectionManager entry)
        {
            ValidateSecurity(entry);
            return GetList<DataEntity>(entry, "WINDOWSPRINTERCONFIGURATION", "DESCRIPTION", "ID", "DESCRIPTION", false);
        }

        public virtual void Save(IConnectionManager entry, WindowsPrinterConfiguration config)
        {
            SqlServerStatement statement = new SqlServerStatement("WINDOWSPRINTERCONFIGURATION");

            ValidateSecurity(entry);

            if (!(entry.HasPermission(Permission.HardwareProfileEdit) || entry.HasPermission(Permission.ManageStationPrinting)))
            {
                throw new PermissionException(Permission.HardwareProfileEdit + " or " + Permission.ManageStationPrinting);
            }

            bool isNew = false;
            if (config.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                config.ID = DataProviderFactory.Instance.GenerateNumber<IWindowsPrinterConfigurationData, WindowsPrinterConfiguration>(entry);
            }

            if (isNew || !Exists(entry, config.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)config.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)config.ID);
            }

            statement.AddField("DESCRIPTION", config.Text);
            statement.AddField("DEVICENAME", config.PrinterDeviceName);
            statement.AddField("FONTNAME", config.FontName);
            statement.AddField("FONTSIZE", config.FontSize, SqlDbType.Decimal);
            statement.AddField("WIDEHIGHFONTSIZE", config.WideHighFontSize, SqlDbType.Decimal);
            statement.AddField("LEFTMARGIN", config.LeftMargin, SqlDbType.Int);
            statement.AddField("RIGHTMARGIN", config.RightMargin, SqlDbType.Int);
            statement.AddField("TOPMARGIN", config.TopMargin, SqlDbType.Int);
            statement.AddField("BOTTOMMARGIN", config.BottomMargin, SqlDbType.Int);
            statement.AddField("PRINTDESIGNBOXES", config.PrintDesignBoxes, SqlDbType.Bit);
            statement.AddField("FOLDERLOCATION", config.FolderLocation);

            Save(entry, config, statement);
        }

        private static void PopulateWindowsPrinterConfiguration(IDataReader dr, WindowsPrinterConfiguration config)
        {
            config.ID = (string)dr["ID"];
            config.Text = (string)dr["DESCRIPTION"];
            config.PrinterDeviceName = (string)dr["DEVICENAME"];
            config.FontName = (string)dr["FONTNAME"];
            config.FontSize = (decimal)dr["FONTSIZE"];
            config.WideHighFontSize = (decimal)dr["WIDEHIGHFONTSIZE"];
            config.LeftMargin = (int)dr["LEFTMARGIN"];
            config.RightMargin = (int)dr["RIGHTMARGIN"];
            config.TopMargin = (int)dr["TOPMARGIN"];
            config.BottomMargin = (int)dr["BOTTOMMARGIN"];
            config.PrintDesignBoxes = (bool)dr["PRINTDESIGNBOXES"];
            config.FolderLocation = (string)dr["FOLDERLOCATION"];
            config.ConfigurationUsed = (bool)dr["CONFIGURATIONUSED"];
        }

        #region ISequenceable Members

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "WINDOWSPRINTER"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "WINDOWSPRINTERCONFIGURATION", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
