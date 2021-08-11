using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Loyalty
{
    /// <summary>
    /// A Data provider that retrieves the data for the business object <see cref="LoyaltyMSRCard"/>
    /// </summary>
    public class LoyaltyMSRCardData : SqlServerDataProviderBase, ILoyaltyMSRCardData
    {
        private static string ResolveSort(LoyaltyMSRCardSorting sort, bool backwards)
        {
            if (backwards)
            {
                switch (sort)
                {
                    case LoyaltyMSRCardSorting.CardNumber:
                        return "Len(CARDNUMBER) DESC,CARDNUMBER DESC";

                    case LoyaltyMSRCardSorting.Type:
                        return "LOYALTYTENDER DESC";
                }
            }
            else
            {
                switch (sort)
                {
                    case LoyaltyMSRCardSorting.CardNumber:
                        return "Len(CARDNUMBER) ASC,CARDNUMBER ASC";

                    case LoyaltyMSRCardSorting.Type:
                        return "LOYALTYTENDER ASC";
                }
            }

            return "";
        }

        private static string BaseSelectString(bool FilterByCustomerID)
        {
            //Points calculation from loyaltytrans
            //IssuePoints = 0,
            //UsePoints = 1,
            //ExpirePoints = 2
            /*
            SELECT ISNULL(CARDNUMBER, '') CARDNUMBER,
            ISNULL(LOYALTYSCHEMEID, '') LOYALTYSCHEMEID,
            ISNULL(LOYALTYCUSTID,'') LOYALTYCUSTID,
            ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where u.LOYALTYCUSTID = r.LOYALTYCUSTID AND u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 0),0) as ISSUEDPOINTS,
            ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 1),0) as USEDPOINTS,
            ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 2),0) as EXPIREDPOINTS,
            ISNULL((select SUM(r.REMAININGPOINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND r.TYPE = 0 AND r.STATUS <> 0),0) as POINTSTATUS
            from RBOLOYALTYMSRCARDTABLE as u
            */

            string result =
                    "SELECT ISNULL(u.CARDNUMBER, '') CARDNUMBER"
                    + ", ISNULL(u.LOYALTYSCHEMEID, '') LOYALTYSCHEMEID"
                    + ", ISNULL(rboscheme.DESCRIPTION, '') as SCHEMEDESCRIPTION"
                    + ", ISNULL(rboscheme.USELIMIT, 0) as USELIMIT"
                    + ", ISNULL(u.LOYALTYCUSTID,'') LOYALTYCUSTID"
                    + ", ISNULL(cust.NAME, '') as CUSTNAME"
                    + ", ISNULL(u.LINKTYPE, 0) LINKTYPE"
                    + ", ISNULL(u.LINKID, '') LINKID"
                    + ", ISNULL(u.STARTINGPOINTS, 0) STARTINGPOINTS"
                    + ", ISNULL(u.LOYALTYTENDER, 0) LOYALTYTENDER";

            if (FilterByCustomerID)
            {
                result = result
                + ", ISNULL((select SUM(r.REMAININGPOINTS) from RBOLOYALTYTRANS as r where"
                    + " u.LOYALTYCUSTID = r.LOYALTYCUSTID AND " + "u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND r.TYPE = 0 AND r.STATUS <> 0),0) as POINTSTATUS"
                + ", ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where"
                    + " u.LOYALTYCUSTID = r.LOYALTYCUSTID AND " + "u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 0),0) as ISSUEDPOINTS"
                + ", ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where"
                    + " u.LOYALTYCUSTID = r.LOYALTYCUSTID AND " + " u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 1),0) as USEDPOINTS"
                + ", ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where"
                    + " u.LOYALTYCUSTID = r.LOYALTYCUSTID AND " + " u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 2),0) as EXPIREDPOINTS";
            }
            else
            {
                result = result
                + ", ISNULL((select SUM(r.REMAININGPOINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND r.TYPE = 0 AND r.STATUS <> 0),0) as POINTSTATUS"
                + ", ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 0),0) as ISSUEDPOINTS"
                + ", ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 1),0) as USEDPOINTS"
                + ", ISNULL((select SUM(r.POINTS) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND TYPE = 2),0) as EXPIREDPOINTS";
            }
            result = result
                + ", u.[MODIFIEDDATE] MODIFIEDDATE"
                + ", u.[MODIFIEDTIME] MODIFIEDTIME"
                + ", ISNULL(u.[MODIFIEDBY], '00000000-0000-0000-0000-000000000000') MODIFIEDBY"
                + ", u.[CREATEDDATE] CREATEDDATE"
                + ", u.[CREATEDTIME] CREATEDTIME"
                + ", ISNULL(u.[CREATEDBY], '00000000-0000-0000-0000-000000000000') CREATEDBY"
                + ", ISNULL(u.MOBILECARD, 0) as MOBILECARD";
            return result;
        }

        private static void PopulateLoyaltyMsrCardLink(IDataReader dr, LoyaltyMSRCard loyaltyMsrCard)
        {
            loyaltyMsrCard.SchemeID = (string)dr["LOYALTYSCHEMEID"];
            loyaltyMsrCard.SchemeDescription = (string)dr["SCHEMEDESCRIPTION"];
            loyaltyMsrCard.UsePointsLimit = (int)dr["USELIMIT"];
            loyaltyMsrCard.CardNumber = (string)dr["CARDNUMBER"];
            loyaltyMsrCard.Text = loyaltyMsrCard.CardNumber;
            loyaltyMsrCard.LinkType = (LoyaltyMSRCard.LinkTypeEnum)dr["LINKTYPE"];
            loyaltyMsrCard.LinkID = (string)dr["LINKID"];
            loyaltyMsrCard.CustomerID = string.IsNullOrEmpty(loyaltyMsrCard.LinkID.StringValue) ? RecordIdentifier.Empty : loyaltyMsrCard.LinkID;
            loyaltyMsrCard.CustomerName = (string)(dr["CUSTNAME"]);
            loyaltyMsrCard.TenderType = (LoyaltyMSRCard.TenderTypeEnum)dr["LOYALTYTENDER"];
            loyaltyMsrCard.PointStatus = (decimal)dr["POINTSTATUS"];
            loyaltyMsrCard.IssuedPoints = (decimal)dr["ISSUEDPOINTS"];
            loyaltyMsrCard.UsedPoints = (decimal)dr["USEDPOINTS"];
            loyaltyMsrCard.ExpiredPoints = (decimal)dr["EXPIREDPOINTS"];
            loyaltyMsrCard.StartingPoints = (decimal)dr["STARTINGPOINTS"];
            loyaltyMsrCard.ModifiedDate = (dr["MODIFIEDDATE"] == null) ? Date.Empty : new Date((DateTime)dr["MODIFIEDDATE"]);
            loyaltyMsrCard.ModifiedTime = (int)dr["MODIFIEDTIME"];
            loyaltyMsrCard.ModifiedBy = (Guid)dr["MODIFIEDBY"];
            loyaltyMsrCard.CreatedDate = (dr["CREATEDDATE"] == null) ? Date.Empty : new Date((DateTime)dr["CREATEDDATE"]);
            loyaltyMsrCard.CreatedTime = (int)dr["CREATEDTIME"];
            loyaltyMsrCard.CreatedBy = (Guid)dr["CREATEDBY"];
            loyaltyMsrCard.MobileCard = (int)dr["MOBILECARD"] != 0;
        }

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyCardNumber">The loyalty card number.</param>
        /// <returns>An instance of <see cref="LoyaltyMSRCard"/></returns>
        public virtual LoyaltyMSRCard Get(IConnectionManager entry, RecordIdentifier loyaltyCardNumber)
        {
            List<LoyaltyMSRCard> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = BaseSelectString(false)
                    + " FROM RBOLOYALTYMSRCARDTABLE as u "
                    + "LEFT OUTER JOIN RBOLOYALTYSCHEMESTABLE rboscheme on u.LOYALTYSCHEMEID = rboscheme.LOYALTYSCHEMEID and u.DATAAREAID = rboscheme.DATAAREAID "
                    + "LEFT OUTER JOIN CUSTOMER cust on u.LINKID = cust.ACCOUNTNUM and u.DATAAREAID = cust.DATAAREAID "
                    + "where u.CARDNUMBER = @cardNumber and u.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "cardNumber", (string)loyaltyCardNumber);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyMSRCard>(entry, cmd, CommandType.Text, PopulateLoyaltyMsrCardLink);
            }

            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Gets the loyalty card for the customer used by mobile loyalt
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="customerID">ID of the customer</param>
        /// <returns></returns>
        public LoyaltyMSRCard GetCustomerMobileCard(IConnectionManager entry, RecordIdentifier customerID)
        {
            List<LoyaltyMSRCard> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = BaseSelectString(true)
                    + " FROM RBOLOYALTYMSRCARDTABLE as u "
                    + "LEFT OUTER JOIN RBOLOYALTYSCHEMESTABLE rboscheme on u.LOYALTYSCHEMEID = rboscheme.LOYALTYSCHEMEID and u.DATAAREAID = rboscheme.DATAAREAID "
                    + "LEFT OUTER JOIN CUSTOMER cust on u.LINKID = cust.ACCOUNTNUM and u.DATAAREAID = cust.DATAAREAID "
                    + "where u.LOYALTYCUSTID = @customerID and u.DATAAREAID = @dataAreaId and u.MOBILECARD = 1";

                MakeParam(cmd, "customerID", (string)customerID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyMSRCard>(entry, cmd, CommandType.Text, PopulateLoyaltyMsrCardLink);
            }

            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Gets the list of MSR cards.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customers"></param>
        /// <param name="schemas"></param>
        /// <param name="cardID">CardID</param>
        /// <param name="hasCustomers">Should customers exlusivly included or excluded</param>
        /// <param name="tenderType">Tender Type</param>
        /// <param name="status"></param>
        /// <param name="statusInequality"></param>
        /// <param name="rowFrom">from row </param>
        /// <param name="rowTo">to row</param>
        /// <param name="sortBy">sort descending</param>
        /// <param name="backwards">sort descending</param>
        /// <returns>List of instances of <see cref="LoyaltyMSRCard"/></returns>
        public List<LoyaltyMSRCard> GetList(IConnectionManager entry, 
                                                   List<DataEntity> customers, 
                                                   List<DataEntity> schemas, 
                                                   RecordIdentifier cardID, 
                                                   bool? hasCustomers,
                                                   int tenderType, 
                                                   double? status, 
                                                   LoyaltyMSRCardInequality statusInequality, 
                                                   int rowFrom, 
                                                   int rowTo, 
                                                   LoyaltyMSRCardSorting sortBy, 
                                                   bool backwards)
        {
            List<LoyaltyMSRCard> result;

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                string sort = ResolveSort(sortBy, backwards);

                if (rowTo > 0)
                {
                    cmd.CommandText = "select s.* from (";
                }

                cmd.CommandText = cmd.CommandText + BaseSelectString((customers != null) && customers.Count > 0);
                if (rowTo > 0)
                {
                    cmd.CommandText = cmd.CommandText
                                      + ",ROW_NUMBER() OVER(";
                    cmd.CommandText = cmd.CommandText
                                      + " ORDER BY " + sort;
                    cmd.CommandText = cmd.CommandText
                                      + ") AS ROW";
                }

                cmd.CommandText = cmd.CommandText + " FROM RBOLOYALTYMSRCARDTABLE as u ";
                cmd.CommandText = cmd.CommandText + "LEFT OUTER JOIN RBOLOYALTYSCHEMESTABLE rboscheme on u.LOYALTYSCHEMEID = rboscheme.LOYALTYSCHEMEID and u.DATAAREAID = rboscheme.DATAAREAID ";
                cmd.CommandText = cmd.CommandText + "LEFT OUTER JOIN CUSTOMER cust on u.LINKID = cust.ACCOUNTNUM and u.DATAAREAID = cust.DATAAREAID ";
                cmd.CommandText = cmd.CommandText + "where u.DATAAREAID = @dataAreaId";

                #region filtering

                if ((customers != null) && customers.Count > 0)
                {
                    cmd.CommandText += " and u.LINKTYPE = 1 and (";
                    for (int i = 0; i < customers.Count; i++)
                    {
                        cmd.CommandText += "u.LINKID = @customerId" + i;
                        cmd.CommandText += customers.Count > i + 1 ? " or " : "";
                        MakeParam(cmd, "customerId" + i, customers[i].ID);
                    }
                    cmd.CommandText += ")";
                }

                if (hasCustomers != null && hasCustomers.Value)
                {
                    cmd.CommandText += " and u.LINKTYPE = 1 AND u.LINKID <> ''";
                }
                else if(hasCustomers != null)
                {
                    cmd.CommandText += " and u.LINKTYPE = 1 AND u.LINKID = ''";
                }

                if ((schemas != null) && schemas.Count > 0)
                {
                    cmd.CommandText += " and (";
                    for (int i = 0; i < schemas.Count; i++)
                    {
                        cmd.CommandText += "u.LOYALTYSCHEMEID = @schemeId" + i;
                        cmd.CommandText += schemas.Count > i + 1 ? " or " : "";
                        MakeParam(cmd, "schemeId" + i, schemas[i].ID);
                    }
                    cmd.CommandText += ")";
                }

                if ((cardID != null) && ((string) cardID) != "")
                {
                    var searchString = PreProcessSearchText((string)cardID, true, true);
                    cmd.CommandText = cmd.CommandText + " and u.CARDNUMBER LIKE @cardId";
                    MakeParam(cmd, "cardId", searchString);
                }

                if (tenderType >= 0)
                {
                    if (tenderType >= (int) LoyaltyMSRCardTypeSearchEnum.CardTender)
                    {
                        LoyaltyMSRCardTypeSearchEnum types = (LoyaltyMSRCardTypeSearchEnum) tenderType;
                        cmd.CommandText += " and (";
                        cmd.CommandText += (types & LoyaltyMSRCardTypeSearchEnum.CardTender) == LoyaltyMSRCardTypeSearchEnum.CardTender ? "u.LOYALTYTENDER = 0" : "";
                        cmd.CommandText += (types & LoyaltyMSRCardTypeSearchEnum.ContactTender) == LoyaltyMSRCardTypeSearchEnum.ContactTender ?
                                               ((types & LoyaltyMSRCardTypeSearchEnum.CardTender) == LoyaltyMSRCardTypeSearchEnum.CardTender ? " or " : "") + "u.LOYALTYTENDER = 1" : "";
                        cmd.CommandText += (types & LoyaltyMSRCardTypeSearchEnum.NoTender) == LoyaltyMSRCardTypeSearchEnum.NoTender ?
                                               ((types & LoyaltyMSRCardTypeSearchEnum.CardTender) == LoyaltyMSRCardTypeSearchEnum.CardTender
                                                || (types & LoyaltyMSRCardTypeSearchEnum.ContactTender) == LoyaltyMSRCardTypeSearchEnum.ContactTender ? " or " : "") + "u.LOYALTYTENDER = 2" : "";
                        cmd.CommandText += (types & LoyaltyMSRCardTypeSearchEnum.Blocked) == LoyaltyMSRCardTypeSearchEnum.Blocked ?
                                               ((types & LoyaltyMSRCardTypeSearchEnum.NoTender) == LoyaltyMSRCardTypeSearchEnum.NoTender
                                                || (types & LoyaltyMSRCardTypeSearchEnum.CardTender) == LoyaltyMSRCardTypeSearchEnum.CardTender
                                                || (types & LoyaltyMSRCardTypeSearchEnum.ContactTender) == LoyaltyMSRCardTypeSearchEnum.ContactTender ? " or " : "") + "u.LOYALTYTENDER = 3" : "";
                        cmd.CommandText += ")";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + " and u.LOYALTYTENDER = @tenderType";
                        MakeParam(cmd, "tenderType", tenderType);
                    }
                }

                if (status != null)
                {
                    cmd.CommandText += " and (select ISNULL(SUM(r.REMAININGPOINTS), 0) from RBOLOYALTYTRANS as r where u.CARDNUMBER = r.CARDNUMBER AND r.ENTRYTYPE = 0 AND r.TYPE = 0 AND r.STATUS <> 0) ";
                    cmd.CommandText += statusInequality == LoyaltyMSRCardInequality.Equals ? " = " : statusInequality == LoyaltyMSRCardInequality.GreaterThan ? " > " : " < ";
                    cmd.CommandText += " @status ";
                    MakeParam(cmd, "status", status, SqlDbType.Decimal);
                }

                #endregion filtering

                if (rowTo > 0)
                {
                    cmd.CommandText = cmd.CommandText
                                      + ") as s"
                                      + " where "
                                      + "(s.ROW between " + rowFrom + " and " + rowTo + ")";
                }

                if (rowTo <= 0)
                {
                    cmd.CommandText = cmd.CommandText + " ORDER BY " + sort;
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyMSRCard>(entry, cmd, CommandType.Text, PopulateLoyaltyMsrCardLink);
            }

            return result;
        }



        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="LoyaltyMSRCard" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="sortBy">sort descending</param>
        /// <param name="backwards">sort descending</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        public virtual List<LoyaltyMSRCard> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, LoyaltyMSRCardSorting sortBy, bool backwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith); 
                string sort = ResolveSort(sortBy, backwards);

                cmd.CommandText = "Select s.* from (" 
                                    + BaseSelectString(false)
                                    + ",ROW_NUMBER() OVER(ORDER BY " + sort + ") AS ROW"
                                    + " FROM RBOLOYALTYMSRCARDTABLE as u "
                                    + "LEFT OUTER JOIN RBOLOYALTYSCHEMESTABLE rboscheme on u.LOYALTYSCHEMEID = rboscheme.LOYALTYSCHEMEID and u.DATAAREAID = rboscheme.DATAAREAID "
                                    + "LEFT OUTER JOIN CUSTOMER cust on u.LINKID = cust.ACCOUNTNUM and u.DATAAREAID = cust.DATAAREAID "
                                    + "where u.DATAAREAID = @dataAreaId and u.LOYALTYCUSTID = '' and u.CARDNUMBER Like "
                                    +"@searchString) as s where (s.ROW between " + rowFrom + " and " + rowTo + " )";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<LoyaltyMSRCard>(entry, cmd, CommandType.Text, PopulateLoyaltyMsrCardLink);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier loyaltyCardID)
        {
            return RecordExists(entry, "RBOLOYALTYMSRCARDTABLE", "CARDNUMBER", loyaltyCardID);
        }
        public virtual bool ExistsForLoyaltyScheme(IConnectionManager entry, RecordIdentifier loyaltySchemeID)
        {
            return RecordExists(entry, "RBOLOYALTYMSRCARDTABLE", "LOYALTYSCHEMEID", loyaltySchemeID);
        }
        public virtual void Save(IConnectionManager entry, LoyaltyMSRCard loyaltyCard)
        {

            ValidateSecurity(entry, new[] { LSOne.DataLayer.BusinessObjects.Permission.CardsEdit, BusinessObjects.Permission.AddCustomerToLoyaltyCard });

            bool isNew = false;
            SqlServerStatement statement = new SqlServerStatement("RBOLOYALTYMSRCARDTABLE");
            if ((loyaltyCard.ID == null) || loyaltyCard.ID.IsEmpty || ((string)loyaltyCard.ID == ""))
            {
                isNew = true;
                loyaltyCard.ID = DataProviderFactory.Instance.GenerateNumber<ILoyaltyMSRCardData, LoyaltyMSRCard>(entry); 
            }

            if (isNew || !Exists(entry, loyaltyCard.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CARDNUMBER", (string)loyaltyCard.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CARDNUMBER", (string)loyaltyCard.ID);
            }

            statement.AddField("LINKTYPE", (int)loyaltyCard.LinkType, SqlDbType.Int);
            statement.AddField("LINKID", loyaltyCard.LinkID == null ? "" : (string)loyaltyCard.LinkID);
            statement.AddField("LOYALTYTENDER", (int)loyaltyCard.TenderType, SqlDbType.Int);
            statement.AddField("LOYALTYSCHEMEID", loyaltyCard.SchemeID == null ? "" : (string)loyaltyCard.SchemeID);
            statement.AddField("LOYALTYCUSTID", loyaltyCard.CustomerID == null ? "" : (string)loyaltyCard.CustomerID);
            statement.AddField("POINTSTATUS", loyaltyCard.PointStatus, SqlDbType.Decimal);
            statement.AddField("ISSUEDPOINTS", loyaltyCard.IssuedPoints, SqlDbType.Decimal);
            statement.AddField("USEDPOINTS", loyaltyCard.UsedPoints, SqlDbType.Decimal);
            statement.AddField("EXPIREDPOINTS", loyaltyCard.ExpiredPoints, SqlDbType.Decimal);
            statement.AddField("STARTINGPOINTS", loyaltyCard.StartingPoints, SqlDbType.Decimal);

            RecordIdentifier userID = entry.CurrentUser == null ? RecordIdentifier.Empty : entry.CurrentUser.ID;
            statement.AddField("MODIFIEDDATE", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("MODIFIEDTIME", DateTime.Now.TimeOfDay.TotalSeconds, SqlDbType.Int);
            if (userID == RecordIdentifier.Empty)
            {
                statement.AddField("MODIFIEDBY", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("MODIFIEDBY", (Guid)userID, SqlDbType.UniqueIdentifier);
            }

            if (isNew)
            {
                statement.AddField("CREATEDDATE", DateTime.Now, SqlDbType.DateTime);
                statement.AddField("CREATEDTIME", DateTime.Now.TimeOfDay.TotalSeconds, SqlDbType.Int);
                if (userID == RecordIdentifier.Empty)
                {
                    statement.AddField("CREATEDBY", DBNull.Value, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    statement.AddField("CREATEDBY", (Guid)userID, SqlDbType.UniqueIdentifier);
                }
            }
            statement.AddField("MOBILECARD", loyaltyCard.MobileCard ? 1 : 0, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
            
        }

        /// <summary>
        /// Delete a MSR card by given ID
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="id">ID of the MSR card</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOLOYALTYMSRCARDTABLE", "CARDNUMBER", id, BusinessObjects.Permission.CardsEdit);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Providers.LoyaltyMSRCardData.Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "LOYALTYCARD"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOLOYALTYMSRCARDTABLE", "CARDNUMBER", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

    }
}
