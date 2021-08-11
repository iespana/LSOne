using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.BusinessObjects.UserInterface.ListItems;
using LSOne.DataLayer.DataProviders.UserInterface;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.Utilities.IO.JSON;

namespace LSOne.DataLayer.SqlDataProviders.UserInterface
{
    public class UIStyleData : SqlServerDataProviderBase, IUIStyleData
    {
        private static void PopulateStyle(IDataReader dr, UIStyle style)
        {
            style.ID = (Guid)dr["ID"];
            style.Text = (string)dr["DESCRIPTION"];
            style.ContextID = (Guid)dr["CONTEXTID"];
            style.Deleted = (bool)dr["DELETED"];

            if (dr["PARENTSTYLE"] == DBNull.Value)
            {
                style.ParentStyleID = RecordIdentifier.Empty;
            }
            else
            {
                style.ParentStyleID = (Guid)dr["PARENTSTYLE"];
            }

            if (dr["STYLEDATA"] != DBNull.Value)
            {
                string data = (string)dr["STYLEDATA"];

                if (data != "")
                {
                    style.Style = JsonConvert.DeserializeObject<BaseStyle>(data);
                }
            }
        }

        private static void PopulateStyleListItem(IDataReader dr, UIStyleListItem style)
        {
            style.ID = (Guid)dr["ID"];
            style.Text = (string)dr["DESCRIPTION"];
            style.ContextID = (Guid)dr["CONTEXTID"];
            style.ParentStyleDescription = (string)dr["ParentDescription"];
        }

        public virtual List<UIStyleListItem> GetList(IConnectionManager entry, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select s1.ID,s1.CONTEXTID,s1.DESCRIPTION , s1.PARENTSTYLE,ISNULL(s2.DESCRIPTION,'') as ParentDescription
                                    from UISTYLES s1
                                    left outer join UISTYLES s2 on s1.PARENTSTYLE = s2.ID and s2.DATAAREAID = s1.DATAAREAID
                                    where s1.DATAAREAID = @dataAreaID and s1.DELETED = 0 order by s1.DESCRIPTION" + (sortBackwards ? " DESC" : "");

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<UIStyleListItem>(entry, cmd, CommandType.Text, PopulateStyleListItem);
            }
        }

        public virtual List<UIStyleListItem> GetList(IConnectionManager entry, RecordIdentifier contextID, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select s1.ID,s1.CONTEXTID,s1.DESCRIPTION , s1.PARENTSTYLE,ISNULL(s2.DESCRIPTION,'') as ParentDescription
                                    from UISTYLES s1
                                    left outer join UISTYLES s2 on s1.PARENTSTYLE = s2.ID and s2.DATAAREAID = s1.DATAAREAID
                                    where s1.CONTEXTID = @contextID and s1.DATAAREAID = @dataAreaID and s1.DELETED = 0 order by s1.DESCRIPTION" + (sortBackwards ? " DESC" : "");

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "contextID", (Guid)contextID);

                return Execute<UIStyleListItem>(entry, cmd, CommandType.Text, PopulateStyleListItem);
            }
        }

        public virtual UIStyle Get(IConnectionManager entry, RecordIdentifier id, bool resolveInheritedStyles = false, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                UIStyle result;

                cmd.CommandText = "select ID,CONTEXTID,PARENTSTYLE,DESCRIPTION,STYLEDATA,DELETED from UISTYLES where ID = @id and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "id", (Guid)id);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                // If we can find it in the cache and we are resolving inherited styles then we must intercept it from the cache right away since
                // we do not want to merge the styles again.
                if (cacheType != CacheType.CacheTypeNone && resolveInheritedStyles)
                {
                    result = (UIStyle)entry.Cache.GetEntityFromCache(typeof(UIStyle), id);

                    if (result != null)
                    {
                        return result;
                    }
                }

                result = Get<UIStyle>(entry, cmd, id, PopulateStyle, cacheType, UsageIntentEnum.Normal);

                if (resolveInheritedStyles && result != null)
                {
                    if (result.ParentStyleID != RecordIdentifier.Empty)
                    {
                        UIStyle parent = Get(entry, result.ParentStyleID, true, cacheType);

                        if (parent != null)
                        {
                            // Merge child on top of parent.
                            result.Style = StyleMerger.Merge(parent.Style, result.Style);
                        }
                    }
                }

                return result;
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<UIStyle>(entry, "UISTYLES", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageUIStyleSetup);

            SqlServerStatement statement = new SqlServerStatement("UISTYLES");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ID", (Guid)id, SqlDbType.UniqueIdentifier);

            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);

            UIStyle style = (UIStyle)entry.Cache.GetEntityFromCache(typeof(UIStyle), id);

            if (style != null)
            {
                style.Deleted = true;
            }
        }

        public virtual void Save(IConnectionManager entry, UIStyle style)
        {
            bool isNew = false;

            ValidateSecurity(entry, BusinessObjects.Permission.ManageUIStyleSetup);
            style.Validate();

            SqlServerStatement statement = new SqlServerStatement("UISTYLES");

            if (style.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                style.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, style.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)style.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)style.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("CONTEXTID", (Guid)style.ContextID, SqlDbType.UniqueIdentifier);
            statement.AddField("PARENTSTYLE", style.ParentStyleID.IsEmpty ? (object)DBNull.Value : (object)(Guid)style.ParentStyleID, SqlDbType.UniqueIdentifier);
            statement.AddField("DESCRIPTION", style.Text);
            statement.AddField("DELETED", style.Deleted, SqlDbType.Bit);
            statement.AddField("STYLEDATA", JsonConvert.SerializeObject(style.Style));

            Save<UIStyle>(entry, style, statement);
        }
    }
}
