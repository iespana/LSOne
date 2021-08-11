using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
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
    /// Data provider for the business object <see cref="LoyaltySchemes"/>
    /// </summary>
    public class LoyaltySchemesData : SqlServerDataProviderBase, ILoyaltySchemesData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                    "SELECT ISNULL(EXPIRATIONTIMEUNIT, 0) EXPIRATIONTIMEUNIT"
                    + ", ISNULL(EXPIRATIONTIMEVALUE, 0) EXPIRATIONTIMEVALUE"
                    + ", ISNULL(LOYALTYSCHEMEID, '') LOYALTYSCHEMEID"
                    + ", ISNULL(DESCRIPTION, '') DESCRIPTION"
                    + ", ISNULL(STARTINGCARDNO_EDIT, '') STARTINGCARDNO_EDIT"
                    + ", ISNULL(CARDNUMBERLENGTH, 0) CARDNUMBERLENGTH"
                    + ", ISNULL(DISPLAYMESSAGEONPOS, 0) DISPLAYMESSAGEONPOS"
                    + ", ISNULL(CALCULATIONTYPE, 0) CALCULATIONTYPE"
                    + ", ISNULL(CALCULATIONCODEUNIT_EDIT, 0) CALCULATIONCODEUNIT_EDIT"
                    + ", ISNULL(CARDFLITER_EDIT, '') CARDFLITER_EDIT"
                    + ", ISNULL(SHOWPOINTSONRECIEPT, '') SHOWPOINTSONRECIEPT"
                    + ", ISNULL(CARDREGISTRATION, 0) CARDREGISTRATION"
                    + ", EXPIRATIONCALCULATION"
                    + ", ISNULL(LOYALTYTENDERTYPE, '') LOYALTYTENDERTYPE"
                    + ", ISNULL(POINTSTATUS, 0) POINTSTATUS"
                    + ", ISNULL(ISSUEDPOINTS, 0) ISSUEDPOINTS"
                    + ", ISNULL(USEDPOINTS, 0) USEDPOINTS"
                    + ", DATEFILTER"
                    + ", ISNULL(CUSTOMERNOLINK_LINKSTOCUS20017, '') CUSTOMERNOLINK_LINKSTOCUS20017"
                    + ", ISNULL(EXPIREDPOINTS, 0) EXPIREDPOINTS"
                    + ", ISNULL(EXPIRATIONTIMEVALUE, 0) EXPIRATIONTIMEVALUE"
                    + ", ISNULL(EXPIRATIONTIMEUNIT, 0) EXPIRATIONTIMEUNIT"
                    + ", MODIFIEDDATE"
                    + ", ISNULL(MODIFIEDTIME, 0) MODIFIEDTIME"
                    + ", ISNULL(MODIFIEDBY, '00000000-0000-0000-0000-000000000000') MODIFIEDBY"
                    + ", CREATEDDATE"
                    + ", ISNULL(CREATEDTIME, 0) CREATEDTIME"
                    + ", ISNULL(CREATEDBY, '00000000-0000-0000-0000-000000000000') CREATEDBY"
                    + ", ISNULL(USELIMIT, 0) USELIMIT"
                    + " FROM RBOLOYALTYSCHEMESTABLE ";
            }
        }

        private static void PopulateLoyaltyCustomer(IDataReader dr, LoyaltySchemes schemes)
        {
            schemes.ExpirationTimeUnit = (TimeUnitEnum)dr["EXPIRATIONTIMEUNIT"];
            schemes.ExpireTimeValue = (int)dr["EXPIRATIONTIMEVALUE"];
            schemes.SchemeID = (string)dr["LOYALTYSCHEMEID"];
            schemes.Description = (string)dr["DESCRIPTION"];
            schemes.Text = schemes.Description;
            if (String.IsNullOrEmpty(schemes.Text))
            {
                schemes.Text = (string)schemes.SchemeID;
            }
            schemes.StartingCardNumber = (string)dr["STARTINGCARDNO_EDIT"];
            schemes.CardNumberLength = (int)dr["CARDNUMBERLENGTH"];
            schemes.DisplayMessageOnPos = (byte)dr["DISPLAYMESSAGEONPOS"] != 0;
            schemes.CalculationType = (CalculationTypeBase)dr["CALCULATIONTYPE"];
            schemes.CalculationAlgorithm = (int)dr["CALCULATIONCODEUNIT_EDIT"];
            schemes.CardFilter = (string)dr["CARDFLITER_EDIT"];
            schemes.ShowPointsOnReceipt = (LoyaltySchemes.ShowPointsOnReceiptEnum)dr["SHOWPOINTSONRECIEPT"];
            schemes.CardRegistration = (LoyaltySchemes.CardRegistrationEnum)dr["CARDREGISTRATION"];
            schemes.ExpirationCalculation = (dr["EXPIRATIONCALCULATION"] == null) ? Date.Empty : new Date((DateTime)dr["EXPIRATIONCALCULATION"]);
            schemes.LoyaltyTenderType = (string)dr["LOYALTYTENDERTYPE"];
            schemes.PointsStatus = (decimal)dr["POINTSTATUS"];
            schemes.IssuedPoints = (decimal)dr["ISSUEDPOINTS"];
            schemes.UsedPoints = (decimal)dr["USEDPOINTS"];
            schemes.DateFilter = (dr["DATEFILTER"] == null) ? Date.Empty : new Date((DateTime)dr["DATEFILTER"]);
            schemes.CustomerNoLink = (string)dr["CUSTOMERNOLINK_LINKSTOCUS20017"];
            schemes.ExpiredPoints = (decimal)dr["EXPIREDPOINTS"];
            schemes.ModifiedDate = (dr["MODIFIEDDATE"] == null) ? Date.Empty : new Date((DateTime)dr["MODIFIEDDATE"]);
            schemes.ModifiedTime = (int)dr["MODIFIEDTIME"];
            schemes.ModifiedBy = (Guid)dr["MODIFIEDBY"];
            schemes.CreatedDate = (dr["CREATEDDATE"] == null) ? Date.Empty : new Date((DateTime)dr["CREATEDDATE"]);
            schemes.CreatedTime = (int)dr["CREATEDTIME"];
            schemes.CreatedBy = (Guid)dr["CREATEDBY"];
            schemes.UseLimit = (int)dr["USELIMIT"];
        }

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemesID">The schemes ID.</param>
        /// <returns>An instance <see cref="LoyaltySchemes"/></returns>
        public virtual LoyaltySchemes Get(IConnectionManager entry, RecordIdentifier schemesID)
        {
            List<LoyaltySchemes> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "WHERE LOYALTYSCHEMEID = @schemesID AND DATAAREAID = @dataAreaId";

                MakeParam(cmd, "schemesID", (string)schemesID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltySchemes>(entry, cmd, CommandType.Text, PopulateLoyaltyCustomer);
            }

            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Gets the list of Loyalty schemes.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of instances of <see cref="LoyaltySchemes"/></returns>
        public virtual List<LoyaltySchemes> GetList(IConnectionManager entry)
        {
            List<LoyaltySchemes> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "WHERE DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltySchemes>(entry, cmd, CommandType.Text, PopulateLoyaltyCustomer);
            }

            return result;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier loyaltySchemeID)
        {
            return RecordExists(entry, "RBOLOYALTYSCHEMESTABLE", "LOYALTYSCHEMEID", loyaltySchemeID);
        }

        public virtual void Save(IConnectionManager entry, LoyaltySchemes loyaltyScheme)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SchemesEdit);

            loyaltyScheme.Validate();

            bool isNew = false;
            SqlServerStatement statement = new SqlServerStatement("RBOLOYALTYSCHEMESTABLE");
            if ((loyaltyScheme.ID == null) || loyaltyScheme.ID.IsEmpty || (string)loyaltyScheme.ID == "")
            {
                isNew = true;
                loyaltyScheme.ID = DataProviderFactory.Instance.GenerateNumber<ILoyaltySchemesData, LoyaltySchemes>(entry);
            }

            if (isNew || !Exists(entry, loyaltyScheme.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("LOYALTYSCHEMEID", (string)loyaltyScheme.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("LOYALTYSCHEMEID", (string)loyaltyScheme.ID);
            }

            statement.AddField("EXPIRATIONTIMEUNIT", loyaltyScheme.ExpirationTimeUnit, SqlDbType.Int);
            statement.AddField("EXPIRATIONTIMEVALUE", loyaltyScheme.ExpireTimeValue, SqlDbType.Int);
            if (loyaltyScheme.Description != null)
            {
                statement.AddField("DESCRIPTION", loyaltyScheme.Description);
            }
            if (loyaltyScheme.StartingCardNumber != null)
            {
                statement.AddField("STARTINGCARDNO_EDIT", loyaltyScheme.StartingCardNumber);
            }
            statement.AddField("CARDNUMBERLENGTH", loyaltyScheme.CardNumberLength, SqlDbType.Int);
            statement.AddField("DISPLAYMESSAGEONPOS", loyaltyScheme.DisplayMessageOnPos ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CALCULATIONTYPE", loyaltyScheme.CalculationType, SqlDbType.Int);
            statement.AddField("CALCULATIONCODEUNIT_EDIT", loyaltyScheme.CalculationAlgorithm, SqlDbType.Int);
            if (loyaltyScheme.CardFilter != null)
            {
                statement.AddField("CARDFLITER_EDIT", loyaltyScheme.CardFilter);
            }
            statement.AddField("SHOWPOINTSONRECIEPT", loyaltyScheme.ShowPointsOnReceipt, SqlDbType.Int);
            statement.AddField("CARDREGISTRATION", loyaltyScheme.CardRegistration, SqlDbType.Int);
            statement.AddField("EXPIRATIONCALCULATION", loyaltyScheme.ExpirationCalculation == null ? Date.Empty.ToAxaptaSQLDate(false) : loyaltyScheme.ExpirationCalculation.ToAxaptaSQLDate(false), SqlDbType.DateTime);
            if (loyaltyScheme.LoyaltyTenderType != null)
            {
                statement.AddField("LOYALTYTENDERTYPE", loyaltyScheme.LoyaltyTenderType);
            }
            statement.AddField("POINTSTATUS", loyaltyScheme.PointsStatus, SqlDbType.Decimal);
            statement.AddField("ISSUEDPOINTS", loyaltyScheme.IssuedPoints, SqlDbType.Decimal);
            statement.AddField("USEDPOINTS", loyaltyScheme.UsedPoints, SqlDbType.Decimal);
            statement.AddField("DATEFILTER", loyaltyScheme.DateFilter == null ? Date.Empty.ToAxaptaSQLDate(false) : loyaltyScheme.DateFilter.ToAxaptaSQLDate(false), SqlDbType.DateTime);
            statement.AddField("CUSTOMERNOLINK_LINKSTOCUS20017", loyaltyScheme.CustomerNoLink == null ? "" : loyaltyScheme.CustomerNoLink.ToString());
            statement.AddField("EXPIREDPOINTS", loyaltyScheme.ExpiredPoints, SqlDbType.Decimal);
            statement.AddField("USELIMIT", loyaltyScheme.UseLimit, SqlDbType.Int);

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

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Delete a loyalty scheme by given ID and points calculation lines assoiated with it
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="id">ID of the loyalty scheme</param>

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOLOYALTYSCHEMESTABLE", "LOYALTYSCHEMEID", id, BusinessObjects.Permission.SchemesEdit);
            DeleteRecord(entry, "RBOLOYALTYPOINTSTABLE", "LOYALTYSCHEMEID", id, BusinessObjects.Permission.SchemesEdit);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Providers.LoyaltySchemesData.Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "LOYALTYSCHEME"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOLOYALTYSCHEMESTABLE", "LOYALTYSCHEMEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

    }
}
