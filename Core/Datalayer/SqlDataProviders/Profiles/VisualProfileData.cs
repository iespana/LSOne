using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class VisualProfileData : SqlServerDataProviderBase, IVisualProfileData
    {
        protected string BaseSql
        {
            get
            {
                return " SELECT " +
                        "V.PROFILEID, " +
                        "ISNULL(V.NAME,'') as NAME, " +
                        "ISNULL(RESOLUTION, 0) as RESOLUTION, " +
                        "ISNULL(TERMINALTYPE, 0) as TERMINALTYPE, " +
                        "ISNULL(HIDECURSOR, 0) as HIDECURSOR, " +
                        "ISNULL(DESIGNALLOWEDONPOS, 0) as DESIGNALLOWEDONPOS, " +
                        "ISNULL(OPAQUEBACKGROUNDFORM, 0) as OPAQUEBACKGROUNDFORM, " +
                        "ISNULL(OPACITY, 0) as OPACITY, " +
                        "ISNULL(USEFORMBACKGROUNDIMAGE, 0) as USEFORMBACKGROUNDIMAGE,  " +
                        "ISNULL(SHOWCURRENCYSYMBOLONCOLUMNS, 0) as SHOWCURRENCYSYMBOLONCOLUMNS,  " +
                        "ISNULL(SCREENINDEX,0) as SCREENINDEX,  " +
                        "ISNULL(RECEIPTPAYMENTLINESSIZE, 30) as RECEIPTPAYMENTLINESSIZE," +
                        "ISNULL(RECEIPTRETURNBACKGROUNDIMAGEID, '') as RECEIPTRETURNBACKGROUNDIMAGEID, " +
                        "RECEIPTRETURNBACKGROUNDIMAGELAYOUT, " +
                        "RECEIPTRETURNBORDERCOLOR," +
                       @"CAST(CASE WHEN EXISTS(SELECT 1 FROM RBOTERMINALTABLE TE WHERE TE.VISUALPROFILE = V.PROFILEID)
                                     OR EXISTS(SELECT 1 FROM RBOSTAFFTABLE ST WHERE ST.VISUALPROFILE = V.PROFILEID)
                             THEN 1
                             ELSE 0
                         END AS BIT) AS PROFILEISUSED, " +
                        "ISNULL(CONFIRMBUTTONSTYLEID, '') as CONFIRMBUTTONSTYLEID," +
                        "ISNULL(CANCELBUTTONSTYLEID, '') as CANCELBUTTONSTYLEID," +
                        "ISNULL(ACTIONBUTTONSTYLEID, '') as ACTIONBUTTONSTYLEID," +
                        "ISNULL(NORMALBUTTONSTYLEID, '') as NORMALBUTTONSTYLEID," +
                        "ISNULL(OTHERBUTTONSTYLEID, '') as OTHERBUTTONSTYLEID," +
                        "ISNULL(OVERRIDEPOSCONTROLBORDERCOLOR, 0) as OVERRIDEPOSCONTROLBORDERCOLOR," +
                        $"ISNULL(POSCONTROLBORDERCOLOR, {ColorPalette.POSControlBorderColor.ToArgb()}) as POSCONTROLBORDERCOLOR," +
                        "ISNULL(OVERRIDEPOSSELECTEDROWCOLOR, 0) as OVERRIDEPOSSELECTEDROWCOLOR," +
                        $"ISNULL(POSSELECTEDROWCOLOR, {ColorPalette.POSSelectedRowColor.ToArgb()}) as POSSELECTEDROWCOLOR " +
                        " FROM POSVISUALPROFILE V ";
            }
        }

        private static void PopulateVisualProfile(IDataReader dr, VisualProfile visualProfile)
        {
            visualProfile.ID = (string)dr["PROFILEID"];
            visualProfile.Text = (string)dr["NAME"];
            visualProfile.Resolution = (ResolutionsEnum)dr["RESOLUTION"];
            visualProfile.TerminalType = (VisualProfile.HardwareTypes)dr["TERMINALTYPE"];
            visualProfile.HideCursor = ((byte)dr["HIDECURSOR"]) != 0;
            visualProfile.DesignAllowedOnPos = ((byte)dr["DESIGNALLOWEDONPOS"]) != 0;
            visualProfile.OpaqueBackgroundForm = ((byte)dr["OPAQUEBACKGROUNDFORM"]) != 0;
            visualProfile.Opacity = Convert.ToInt32((decimal)dr["OPACITY"]);
            visualProfile.UseFormBackgroundImage = ((byte)dr["USEFORMBACKGROUNDIMAGE"]) != 0;
            visualProfile.ScreenNumber = (ScreenNumberEnum)dr["SCREENINDEX"];
            visualProfile.ShowCurrencySymbolOnColumns = ((byte)dr["SHOWCURRENCYSYMBOLONCOLUMNS"]) != 0;
            visualProfile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
            visualProfile.ReceiptPaymentLinesSize = (int)dr["RECEIPTPAYMENTLINESSIZE"];
            visualProfile.ReceiptReturnBackgroundImageID = (string)dr["RECEIPTRETURNBACKGROUNDIMAGEID"];
            visualProfile.ReceiptReturnBackgroundImageLayout = (ImageLayout)(int)dr["RECEIPTRETURNBACKGROUNDIMAGELAYOUT"];
            visualProfile.ReceiptReturnBorderColor = (int)dr["RECEIPTRETURNBORDERCOLOR"];
            visualProfile.ConfirmButtonStyleID = (string)dr["CONFIRMBUTTONSTYLEID"];
            visualProfile.CancelButtonStyleID = (string)dr["CANCELBUTTONSTYLEID"];
            visualProfile.ActionButtonStyleID = (string)dr["ACTIONBUTTONSTYLEID"];
            visualProfile.NormalButtonStyleID = (string)dr["NORMALBUTTONSTYLEID"];
            visualProfile.OtherButtonStyleID = (string)dr["OTHERBUTTONSTYLEID"];
            visualProfile.OverridePOSControlBorderColor = (bool)dr["OVERRIDEPOSCONTROLBORDERCOLOR"];
            visualProfile.POSControlBorderColor = (int)dr["POSCONTROLBORDERCOLOR"];
            visualProfile.OverridePOSSelectedRowColor = (bool)dr["OVERRIDEPOSSELECTEDROWCOLOR"];
            visualProfile.POSSelectedRowColor = (int)dr["POSSELECTEDROWCOLOR"];
        }

        private void PopulateVisualProfileUnsecure(IConnectionManager entry, IDataReader dr, VisualProfile visualProfile, ref object param)
        {
            visualProfile.ID = (string)dr["PROFILEID"];
            visualProfile.Text = (string)dr["NAME"];
            visualProfile.Resolution = (ResolutionsEnum)dr["RESOLUTION"];
            visualProfile.TerminalType = (VisualProfile.HardwareTypes)dr["TERMINALTYPE"];
            visualProfile.HideCursor = ((byte)dr["HIDECURSOR"]) != 0;
            visualProfile.DesignAllowedOnPos = ((byte)dr["DESIGNALLOWEDONPOS"]) != 0;
            visualProfile.OpaqueBackgroundForm = ((byte)dr["OPAQUEBACKGROUNDFORM"]) != 0;
            visualProfile.Opacity = Convert.ToInt32((decimal)dr["OPACITY"]);
            visualProfile.UseFormBackgroundImage = ((byte)dr["USEFORMBACKGROUNDIMAGE"]) != 0;
            visualProfile.ScreenNumber = (ScreenNumberEnum)dr["SCREENINDEX"];
            visualProfile.ShowCurrencySymbolOnColumns = ((byte)dr["SHOWCURRENCYSYMBOLONCOLUMNS"]) != 0;
            visualProfile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
            visualProfile.ReceiptPaymentLinesSize = (int)dr["RECEIPTPAYMENTLINESSIZE"];
            visualProfile.ReceiptReturnBackgroundImageID = (string)dr["RECEIPTRETURNBACKGROUNDIMAGEID"];
            visualProfile.ReceiptReturnBackgroundImageLayout = (ImageLayout)(int)dr["RECEIPTRETURNBACKGROUNDIMAGELAYOUT"];
            visualProfile.ReceiptReturnBorderColor = (int)dr["RECEIPTRETURNBORDERCOLOR"];
            visualProfile.ConfirmButtonStyleID = (string)dr["CONFIRMBUTTONSTYLEID"];
            visualProfile.CancelButtonStyleID = (string)dr["CANCELBUTTONSTYLEID"];
            visualProfile.ActionButtonStyleID = (string)dr["ACTIONBUTTONSTYLEID"];
            visualProfile.NormalButtonStyleID = (string)dr["NORMALBUTTONSTYLEID"];
            visualProfile.OtherButtonStyleID = (string)dr["OTHERBUTTONSTYLEID"];
            visualProfile.OverridePOSControlBorderColor = (bool)dr["OVERRIDEPOSCONTROLBORDERCOLOR"];
            visualProfile.POSControlBorderColor = (int)dr["POSCONTROLBORDERCOLOR"];
            visualProfile.OverridePOSSelectedRowColor = (bool)dr["OVERRIDEPOSSELECTEDROWCOLOR"];
            visualProfile.POSSelectedRowColor = (int)dr["POSSELECTEDROWCOLOR"];
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList(entry, "NAME");
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            return GetList<DataEntity>(entry, "POSVISUALPROFILE", "NAME", "PROFILEID", sort);
        }

        public virtual List<VisualProfile> GetVisualProfileList(IConnectionManager entry, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                @"SELECT PROFILEID, 
                         ISNULL(NAME,'') as NAME,
	                     CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOTERMINALTABLE t WHERE t.VISUALPROFILE = pvp.PROFILEID)
                                     OR EXISTS (SELECT 1 FROM RBOSTAFFTABLE s WHERE s.VISUALPROFILE = pvp.PROFILEID)
	                     	THEN 1
	                     	ELSE 0
	                     END AS BIT) AS PROFILEISUSED
                       FROM POSVISUALPROFILE  pvp
                       WHERE DATAAREAID = @dataAreaId
                       ORDER BY " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<VisualProfile>(entry, cmd, CommandType.Text, PopulateVisualProfileList);
            }
        }

        private static void PopulateVisualProfileList(IDataReader dr, VisualProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
        }

        public virtual VisualProfile GetTerminalProfile(IConnectionManager entry, RecordIdentifier id, RecordIdentifier storeId, CacheType cache = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                  " JOIN RBOTERMINALTABLE T ON V.DATAAREAID = T.DATAAREAID AND V.PROFILEID = T.VISUALPROFILE " +
                                  " WHERE T.TERMINALID = @TERMINALID " +
                                  " AND T.STOREID = @storeId " +
                                  " AND V.DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "TERMINALID", (string)id);
                MakeParam(cmd, "storeId", (string)storeId);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Get<VisualProfile>(entry, cmd, id, PopulateVisualProfile, cache, UsageIntentEnum.Normal);
            }
        }

        public VisualProfile GetVisualProfileByTerminalUnsecure(IConnectionManager entry,
                                                                  string dataSource,
                                                                  bool windowsAuthentication,
                                                                  string sqlServerLogin,
                                                                  SecureString sqlServerPassword,
                                                                  string databaseName,
                                                                  ConnectionType connectionType,
                                                                  string dataAreaID,
                                                                  RecordIdentifier storeID,
                                                                  RecordIdentifier terminalID)
        {
            using (IDbCommand cmd = new SqlCommand("spPOSGetVisualProfile_1_0"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "terminalID", (string)terminalID);
                MakeParam(cmd, "dataareaID", dataAreaID);
                MakeParam(cmd, "posBorderColor", ColorPalette.POSControlBorderColor.ToArgb(), SqlDbType.Int);
                MakeParam(cmd, "posSelectedRowColor", ColorPalette.POSSelectedRowColor.ToArgb(), SqlDbType.Int);


                object parameter = new object();

                List<VisualProfile> result = entry.UnsecureExecuteReader<VisualProfile, object>(
                    dataSource,
                    windowsAuthentication,
                    sqlServerLogin,
                    sqlServerPassword,
                    databaseName,
                    connectionType,
                    dataAreaID,
                    cmd,
                    ref parameter,
                    PopulateVisualProfileUnsecure);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual VisualProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +  " where V.PROFILEID = @profileID and V.DATAAREAID = @dataAreaId order by PROFILEID";

                MakeParam(cmd, "profileID", (string)id);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Get<VisualProfile>(entry, cmd, id, PopulateVisualProfile, cache, UsageIntentEnum.Normal);
            }     
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<VisualProfile>(entry, "POSVISUALPROFILE", "PROFILEID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<VisualProfile>(entry, "POSVISUALPROFILE", "PROFILEID", id, BusinessObjects.Permission.VisualProfileEdit);
        }

        public virtual void Save(IConnectionManager entry, VisualProfile visualProfile)
        {
            var statement = new SqlServerStatement("POSVISUALPROFILE");

            ValidateSecurity(entry, BusinessObjects.Permission.VisualProfileEdit);
            visualProfile.Validate();

            bool isNew = false;
            if (visualProfile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                visualProfile.ID = DataProviderFactory.Instance.GenerateNumber<IVisualProfileData,VisualProfile>(entry);
            }

            if (isNew || !Exists(entry, visualProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PROFILEID", (string)visualProfile.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PROFILEID", (string)visualProfile.ID);    
            }

            statement.AddField("NAME", visualProfile.Text);
            statement.AddField("RESOLUTION", visualProfile.Resolution, SqlDbType.Int);
            statement.AddField("TERMINALTYPE", visualProfile.TerminalType, SqlDbType.Int);
            statement.AddField("HIDECURSOR", visualProfile.HideCursor, SqlDbType.Int);
            statement.AddField("DESIGNALLOWEDONPOS", visualProfile.DesignAllowedOnPos, SqlDbType.Int);
            statement.AddField("OPAQUEBACKGROUNDFORM", visualProfile.OpaqueBackgroundForm, SqlDbType.Int);
            statement.AddField("OPACITY", visualProfile.Opacity, SqlDbType.Int);
            statement.AddField("USEFORMBACKGROUNDIMAGE", visualProfile.UseFormBackgroundImage, SqlDbType.Int);
            statement.AddField("SCREENINDEX", visualProfile.ScreenNumber, SqlDbType.Int);
            statement.AddField("SHOWCURRENCYSYMBOLONCOLUMNS", visualProfile.ShowCurrencySymbolOnColumns, SqlDbType.Int);
            statement.AddField("RECEIPTPAYMENTLINESSIZE", visualProfile.ReceiptPaymentLinesSize, SqlDbType.Int);
            statement.AddField("RECEIPTRETURNBACKGROUNDIMAGEID", visualProfile.ReceiptReturnBackgroundImageID.StringValue);
            statement.AddField("RECEIPTRETURNBACKGROUNDIMAGELAYOUT", (int)visualProfile.ReceiptReturnBackgroundImageLayout, SqlDbType.Int);
            statement.AddField("RECEIPTRETURNBORDERCOLOR", visualProfile.ReceiptReturnBorderColor, SqlDbType.Int);
            statement.AddField("CONFIRMBUTTONSTYLEID", (string)visualProfile.ConfirmButtonStyleID);
            statement.AddField("CANCELBUTTONSTYLEID", (string)visualProfile.CancelButtonStyleID);
            statement.AddField("ACTIONBUTTONSTYLEID", (string)visualProfile.ActionButtonStyleID);
            statement.AddField("NORMALBUTTONSTYLEID", (string)visualProfile.NormalButtonStyleID);
            statement.AddField("OTHERBUTTONSTYLEID", (string)visualProfile.OtherButtonStyleID);
            statement.AddField("OVERRIDEPOSCONTROLBORDERCOLOR", visualProfile.OverridePOSControlBorderColor, SqlDbType.Bit);
            statement.AddField("POSCONTROLBORDERCOLOR", visualProfile.POSControlBorderColor, SqlDbType.Int);
            statement.AddField("OVERRIDEPOSSELECTEDROWCOLOR", visualProfile.OverridePOSSelectedRowColor, SqlDbType.Bit);
            statement.AddField("POSSELECTEDROWCOLOR", visualProfile.POSSelectedRowColor, SqlDbType.Int);

            Save(entry, visualProfile, statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "VISUALPROFILE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSVISUALPROFILE", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
