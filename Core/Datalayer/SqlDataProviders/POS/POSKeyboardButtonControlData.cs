using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LSOne.DataLayer.BusinessObjects.POS;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.POS;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.DataProviders.Images;

namespace LSOne.DataLayer.SqlDataProviders.POS
{
    public class POSKeyboardButtonControlData : SqlServerDataProviderBase, IPOSKeyboardButtonControlData
    {
        public virtual void PopulateKeyboardButtonControl(IDataReader dr, POSKeyboardButtonControl posKeyboardButtonControl)
        {
            posKeyboardButtonControl.ID = (int)dr["ID"];
            posKeyboardButtonControl.ButtonControlID = (int)dr["BUTTONCONTROLID"];
            posKeyboardButtonControl.Type = (int)dr["TYPE"];
            posKeyboardButtonControl.OperationID = (int)dr["OPERATIONID"];
            posKeyboardButtonControl.ShowMenuID = (int)dr["SHOWMENUID"];
            posKeyboardButtonControl.Action = (int)dr["ACTION"];
            posKeyboardButtonControl.ActionProperty = (string)dr["ACTIONPROPERTY"];
            posKeyboardButtonControl.DisplayText = (string)dr["DISPLAYDEXT"];
            posKeyboardButtonControl.Colour = (string)dr["COLOUR"];
            posKeyboardButtonControl.FontSize = (int)dr["FONTSIZE"];
            posKeyboardButtonControl.FontStyle = (int)dr["FONTSTYLE"];
            posKeyboardButtonControl.Alles = (string)dr["ALLES"];
            
            var rawPicture = (byte[])dr["PICTURE"];
            using (var stream = new MemoryStream())
            {
                for (int i = 0; i < rawPicture.Length; i++)
                {
                    stream.WriteByte(rawPicture[i]);
                }
                posKeyboardButtonControl.Picture = Image.FromStream(stream);
            }
        }

        public virtual POSKeyboardButtonControl Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, ISNULL(BUTTONCONTROLID, 0) AS BUTTONCONTROLID, ISNULL(ROWNUMBER, 0) AS ROWNUMBER, ISNULL(TYPE, 0) AS TYPE,
                                     ISNULL(OPERATIONID, 0) AS OPERATIONID, ISNULL(SHOWMENUID, '') AS SHOWMENUID, ISNULL(ACTION, 0) AS ACTION, 
                                     ISNULL(ACTIONPROPERTY, '') AS ACTIONPROPERTY, PICTURE, ISNULL(DISPLAYTEXT, '') AS DISPLAYTEXT, 
                                     ISNULL(COLOUR, '') AS COLOUR, ISNULL(FONTSIZE, 0) AS FONTSIZE, ISNULL(FONTSTYLE, 0) AS FONTSTYLE, ISNULL(ALLES, '') AS ALLES
                                     FROM POSISKEYBOARDBUTTONCONTROLB WHERE DATAAREAID = @dataAreaID AND ID = @ID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", id);
                return Get<POSKeyboardButtonControl>(entry, cmd, id, PopulateKeyboardButtonControl, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual List<POSKeyboardButtonControl> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, ISNULL(BUTTONCONTROLID, 0) AS BUTTONCONTROLID, ISNULL(ROWNUMBER, 0) AS ROWNUMBER, ISNULL(TYPE, 0) AS TYPE,
                                     ISNULL(OPERATIONID, 0) AS OPERATIONID, ISNULL(SHOWMENUID, '') AS SHOWMENUID, ISNULL(ACTION, 0) AS ACTION, 
                                     ISNULL(ACTIONPROPERTY, '') AS ACTIONPROPERTY, PICTURE, ISNULL(DISPLAYTEXT, '') AS DISPLAYTEXT, 
                                     ISNULL(COLOUR, '') AS COLOUR, ISNULL(FONTSIZE, 0) AS FONTSIZE, ISNULL(FONTSTYLE, 0) AS FONTSTYLE, ISNULL(ALLES, '') AS ALLES
                                     FROM POSISKEYBOARDBUTTONCONTROLB WHERE DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Execute<POSKeyboardButtonControl>(entry, cmd, CommandType.Text, PopulateKeyboardButtonControl);
            }
        }

        public virtual List<POSKeyboardButtonControl> GetButtonControlList(IConnectionManager entry, RecordIdentifier buttonControlID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, ISNULL(BUTTONCONTROLID, 0) AS BUTTONCONTROLID, ISNULL(ROWNUMBER, 0) AS ROWNUMBER, ISNULL(TYPE, 0) AS TYPE,
                                     ISNULL(OPERATIONID, 0) AS OPERATIONID, ISNULL(SHOWMENUID, '') AS SHOWMENUID, ISNULL(ACTION, 0) AS ACTION, 
                                     ISNULL(ACTIONPROPERTY, '') AS ACTIONPROPERTY, PICTURE, ISNULL(DISPLAYTEXT, '') AS DISPLAYTEXT, 
                                     ISNULL(COLOUR, '') AS COLOUR, ISNULL(FONTSIZE, 0) AS FONTSIZE, ISNULL(FONTSTYLE, 0) AS FONTSTYLE, ISNULL(ALLES, '') AS ALLES
                                     FROM POSISKEYBOARDBUTTONCONTROLB WHERE DATAAREAID = @dataAreaID AND BUTTONCONTROLID = @buttonControlID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "buttonControlID", buttonControlID);
                return Execute<POSKeyboardButtonControl>(entry, cmd, CommandType.Text, PopulateKeyboardButtonControl);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<POSKeyboardButtonControl>(entry, "POSISKEYBORARDBUTTONCONTROLB", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<POSKeyboardButtonControl>(entry, "POSISKEYBORARDBUTTONCONTROLB", "ID", id, BusinessObjects.Permission.EditPosMenus);
        }

        public virtual void Save(IConnectionManager entry, POSKeyboardButtonControl posKeyboardButtonControl)
        {
            var statement = new SqlServerStatement("POSISKEYBORARDBUTTONCONTROLB");
            ValidateSecurity(entry, BusinessObjects.Permission.EditPosMenus);
            posKeyboardButtonControl.Validate();

            bool isNew = false;
            if (posKeyboardButtonControl.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                posKeyboardButtonControl.ID = DataProviderFactory.Instance.GenerateNumber<IImageData, BusinessObjects.Images.Image>(entry);
            }
            if (isNew || !Exists(entry, posKeyboardButtonControl.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)posKeyboardButtonControl.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)posKeyboardButtonControl.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            byte[] rawPicture;
            using (var stream = new MemoryStream())
            {
                if (posKeyboardButtonControl.Picture is Bitmap)
                {
                    posKeyboardButtonControl.Picture.Save(stream, ImageFormat.Png);
                }
                else
                {
                    posKeyboardButtonControl.Picture.Save(stream, posKeyboardButtonControl.Picture.RawFormat);
                }
                rawPicture = stream.ToArray();
            }
            statement.AddField("PICTURE", rawPicture, SqlDbType.Image);
            statement.AddField("BUTTONCONTROLID", (string)posKeyboardButtonControl.ButtonControlID);
            statement.AddField("ROWNUMBER", posKeyboardButtonControl.RowNumber, SqlDbType.Int);
            statement.AddField("TYPE", posKeyboardButtonControl.Type, SqlDbType.Int);
            statement.AddField("OPERATIONSID", posKeyboardButtonControl.OperationID, SqlDbType.Int);
            statement.AddField("SHOWMENUID", (string)posKeyboardButtonControl.ShowMenuID);
            statement.AddField("ACTION", posKeyboardButtonControl.Action, SqlDbType.Int);
            statement.AddField("ACIONPROPERTY", posKeyboardButtonControl.ActionProperty);
            statement.AddField("DISPLAYTEXT", posKeyboardButtonControl.DisplayText);
            statement.AddField("COLOUR", posKeyboardButtonControl.Colour);
            statement.AddField("FRONTSIZE", posKeyboardButtonControl.FontSize, SqlDbType.Int);
            statement.AddField("FONTSTYLE", posKeyboardButtonControl.FontStyle, SqlDbType.Int);
            statement.AddField("ALLES", posKeyboardButtonControl.Alles);
            Save(entry, posKeyboardButtonControl, statement);
        }
    }
}
