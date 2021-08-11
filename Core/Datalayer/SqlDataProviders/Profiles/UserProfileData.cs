using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.SqlConnector.DataProviders;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.DataProviders;
using System.Data;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.GenericConnector.Enums;
using System;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class UserProfileData : SqlServerDataProviderBase, IUserProfileData
    {
        protected static List<TableColumn> SelectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "PROFILEID", TableAlias = "UP", ColumnAlias = "PROFILEID" },
            new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "UP", ColumnAlias = "DESCRIPTION" },
            new TableColumn {ColumnName = "MAXDISCOUNTPCT", TableAlias = "UP", ColumnAlias = "MAXDISCOUNTPCT" },
            new TableColumn {ColumnName = "MAXLINEDISCOUNTAMOUNT", TableAlias = "UP", ColumnAlias = "MAXLINEDISCOUNTAMOUNT" },
            new TableColumn {ColumnName = "MAXTOTALDISCOUNTPCT", TableAlias = "UP", ColumnAlias = "MAXTOTALDISCOUNTPCT" },
            new TableColumn {ColumnName = "MAXTOTALDISCOUNTAMOUNT", TableAlias = "UP", ColumnAlias = "MAXTOTALDISCOUNTAMOUNT" },
            new TableColumn {ColumnName = "MAXLINERETURNAMOUNT", TableAlias = "UP", ColumnAlias = "MAXLINERETURNAMOUNT" },
            new TableColumn {ColumnName = "MAXTOTALRETURNAMOUNT", TableAlias = "UP", ColumnAlias = "MAXTOTALRETURNAMOUNT" },
            new TableColumn {ColumnName = "LAYOUTID", TableAlias = "UP", ColumnAlias = "LAYOUTID", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "VISUALPROFILE", TableAlias = "UP", ColumnAlias = "VISUALPROFILE", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "OPERATORCULTURE", TableAlias = "UP", ColumnAlias = "OPERATORCULTURE", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "KEYBOARDCODE", TableAlias = "UP", ColumnAlias = "KEYBOARDCODE", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "LAYOUTNAME", TableAlias = "UP", ColumnAlias = "LAYOUTNAME", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "STOREID", TableAlias = "UP", ColumnAlias = "STOREID", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "NAME", TableAlias = "VP", ColumnAlias = "VISUALPROFILENAME", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "NAME", TableAlias = "S", ColumnAlias = "STORENAME", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "NAME", TableAlias = "T", ColumnAlias = "TOUCHLAYOUTNAME", IsNull = true, NullValue = "''" },
            new TableColumn {ColumnName = "CASE WHEN EXISTS(SELECT 1 FROM RBOSTAFFTABLE ST WHERE ST.USERPROFILE = UP.PROFILEID) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END", ColumnAlias = "PROFILEUSED" }
        };

        protected static List<Join> Joins = new List<Join>
        {
            new Join {TableAlias = "VP", Table = "POSVISUALPROFILE", Condition = "UP.VISUALPROFILE = VP.PROFILEID AND UP.DATAAREAID = VP.DATAAREAID", JoinType = "LEFT OUTER" },
            new Join {TableAlias = "S", Table = "RBOSTORETABLE", Condition = "UP.STOREID = S.STOREID AND UP.DATAAREAID = S.DATAAREAID", JoinType = "LEFT OUTER" },
            new Join {TableAlias = "T", Table = "POSISTILLLAYOUT", Condition = "UP.LAYOUTID = T.LAYOUTID AND UP.DATAAREAID = T.DATAAREAID", JoinType = "LEFT OUTER" },
        };

        public RecordIdentifier SequenceID
        {
            get { return "USERPROFILE"; }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord<VisualProfile>(entry, "POSUSERPROFILE", "PROFILEID", ID, string.Empty);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists<VisualProfile>(entry, "POSUSERPROFILE", "PROFILEID", ID);
        }

        public virtual void Save(IConnectionManager entry, UserProfile userProfile)
        {
            var statement = new SqlServerStatement("POSUSERPROFILE");

            ValidateSecurity(entry);
            userProfile.Validate();

            bool isNew = false;
            if (userProfile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                userProfile.ID = DataProviderFactory.Instance.GenerateNumber<IUserProfileData, UserProfile>(entry);
            }

            if (isNew || !Exists(entry, userProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PROFILEID", (string)userProfile.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PROFILEID", (string)userProfile.ID);
            }

            statement.AddField("DESCRIPTION", userProfile.Text);
            statement.AddField("MAXDISCOUNTPCT", userProfile.MaxLineDiscountPercentage, SqlDbType.Decimal);
            statement.AddField("MAXTOTALDISCOUNTPCT", userProfile.MaxTotalDiscountPercentage, SqlDbType.Decimal);
            statement.AddField("MAXLINEDISCOUNTAMOUNT", userProfile.MaxLineDiscountAmount, SqlDbType.Decimal);
            statement.AddField("MAXTOTALDISCOUNTAMOUNT", userProfile.MaxTotalDiscountAmount, SqlDbType.Decimal);
            statement.AddField("MAXLINERETURNAMOUNT", userProfile.MaxLineReturnAmount, SqlDbType.Decimal);
            statement.AddField("MAXTOTALRETURNAMOUNT", userProfile.MaxTotalReturnAmount, SqlDbType.Decimal);
            statement.AddField("LAYOUTID", (string)userProfile.LayoutID);
            statement.AddField("VISUALPROFILE", (string)userProfile.VisualProfileID);
            statement.AddField("OPERATORCULTURE", userProfile.LanguageCode);
            statement.AddField("KEYBOARDCODE", userProfile.KeyboardCode);
            statement.AddField("LAYOUTNAME", userProfile.KeyboardLayoutName);
            statement.AddField("STOREID", (string)userProfile.StoreID);

            Save(entry, userProfile, statement);
        }

        protected static void PopulateUserProfile(IDataReader dr, UserProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["DESCRIPTION"];
            profile.MaxLineDiscountPercentage = (decimal)dr["MAXDISCOUNTPCT"];
            profile.MaxTotalDiscountPercentage = (decimal)dr["MAXTOTALDISCOUNTPCT"];
            profile.MaxLineDiscountAmount = (decimal)dr["MAXLINEDISCOUNTAMOUNT"];
            profile.MaxTotalDiscountAmount = (decimal)dr["MAXTOTALDISCOUNTAMOUNT"];
            profile.MaxLineReturnAmount = (decimal)dr["MAXLINERETURNAMOUNT"];
            profile.MaxTotalReturnAmount = (decimal)dr["MAXTOTALRETURNAMOUNT"];
            profile.LanguageCode = (string)dr["OPERATORCULTURE"];
            profile.KeyboardCode = (string)dr["KEYBOARDCODE"];
            profile.KeyboardLayoutName = (string)dr["LAYOUTNAME"];
            profile.StoreID = (string)dr["STOREID"];
            profile.LayoutID = (string)dr["LAYOUTID"];
            profile.VisualProfileID = (string)dr["VISUALPROFILE"];
            profile.VisualProfileName = (string)dr["VISUALPROFILENAME"];
            profile.StoreName = (string)dr["STORENAME"];
            profile.LayoutName = (string)dr["TOUCHLAYOUTNAME"];
            profile.ProfileIsUsed = AsBool(dr["PROFILEUSED"]);
        }

        public virtual UserProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                Condition condition = new Condition { Operator = "AND", ConditionValue = "UP.PROFILEID = @profileID AND UP.DATAAREAID = @dataAreaID" };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSUSERPROFILE", "UP"),
                    QueryPartGenerator.InternalColumnGenerator(SelectionColumns),
                    QueryPartGenerator.JoinGenerator(Joins),
                    QueryPartGenerator.ConditionGenerator(condition),
                    string.Empty);

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "profileID", (string)id);

                return Get<UserProfile>(entry, cmd, id, PopulateUserProfile, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual List<UserProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                Condition condition = new Condition { Operator = "AND", ConditionValue = "UP.DATAAREAID = @dataAreaID" };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSUSERPROFILE", "UP"),
                    QueryPartGenerator.InternalColumnGenerator(SelectionColumns),
                    QueryPartGenerator.JoinGenerator(Joins),
                    QueryPartGenerator.ConditionGenerator(condition),
                    "ORDER BY UP.DESCRIPTION");

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<UserProfile>(entry, cmd, CommandType.Text, PopulateUserProfile);
            }
        }

        public virtual List<UserProfile> GetListAdvanced(IConnectionManager entry, UserProfileFilter filter)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "UP.DATAAREAID = @dataAreaID" }
                };

                if(!string.IsNullOrWhiteSpace(filter.Description))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "UP.DESCRIPTION LIKE @description" });
                    MakeParam(cmd, "description", PreProcessSearchText(filter.Description, true, filter.DescriptionBeginsWith));
                }

                if (!string.IsNullOrWhiteSpace(filter.LanguageCode))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "UP.OPERATORCULTURE LIKE @language" });
                    MakeParam(cmd, "language", PreProcessSearchText(filter.LanguageCode, true, filter.LanguageCodeBeginsWith));
                }

                if(filter.LayoutID != RecordIdentifier.Empty && filter.LayoutID.StringValue != "")
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "UP.LAYOUTID = @layoutID" });
                    MakeParam(cmd, "layoutID", filter.LayoutID.StringValue);
                }

                if (filter.StoreID != RecordIdentifier.Empty && filter.StoreID.StringValue != "")
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "UP.STOREID = @storeID" });
                    MakeParam(cmd, "storeID", filter.StoreID.StringValue);
                }

                if (filter.VisualProfileID != RecordIdentifier.Empty && filter.VisualProfileID.StringValue != "")
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "UP.VISUALPROFILE = @visualProfileID" });
                    MakeParam(cmd, "visualProfileID", filter.VisualProfileID.StringValue);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSUSERPROFILE", "UP"),
                    QueryPartGenerator.InternalColumnGenerator(SelectionColumns),
                    QueryPartGenerator.JoinGenerator(Joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.Sort, filter.SortBackwards));

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<UserProfile>(entry, cmd, CommandType.Text, PopulateUserProfile);
            }
        }

        private string ResolveSort(UserProfileSortEnum sortEnum, bool sortBackwards)
        {
            string sort = "ORDER BY ";
            switch(sortEnum)
            {
                case UserProfileSortEnum.Description:
                    sort += "UP.DESCRIPTION";
                    break;
                case UserProfileSortEnum.Store:
                    sort += "S.NAME";
                    break;
                case UserProfileSortEnum.VisualProfile:
                    sort += "VP.NAME";
                    break;
                case UserProfileSortEnum.Layout:
                    sort += "T.NAME";
                    break;
                case UserProfileSortEnum.LanguageCode:
                    sort += "UP.OPERATORCULTURE";
                    break;
                case UserProfileSortEnum.KeyboardLanguage:
                    sort += "UP.KEYBOARDCODE";
                    break;

            }

            sort += sortBackwards ? " DESC" : " ASC";

            return sort;
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSUSERPROFILE", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}
