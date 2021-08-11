using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    /// <summary>
    /// Data provider class for discount periods
    /// </summary>
    public class DiscountPeriodData : SqlServerDataProviderBase, IDiscountPeriodData
    {
        private static void PopulateDiscount(IDataReader dr, DiscountPeriod discountPeriod)
        {
            discountPeriod.ID = (string)dr["ID"];
            discountPeriod.Text = (string)dr["DESCRIPTION"];
            discountPeriod.StartingDate = Date.FromAxaptaDate(dr["STARTINGDATE"]);
            discountPeriod.EndingDate = Date.FromAxaptaDate(dr["ENDINGDATE"]);
            discountPeriod.StartingTime = TimeSpan.FromSeconds((int)dr["STARTINGTIME"]);
            discountPeriod.EndingTime = TimeSpan.FromSeconds((int)dr["ENDINGTIME"]);
            discountPeriod.TimeWithinBounds = ((byte)dr["TIMEWITHINBOUNDS"] == 1);
            discountPeriod.EndingTimeAfterMidnight = ((byte)dr["ENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.MonStartingTime = TimeSpan.FromSeconds((int)dr["MONSTARTINGTIME"]);
            discountPeriod.MonEndingTime = TimeSpan.FromSeconds((int)dr["MONENDINGTIME"]);
            discountPeriod.MonTimeWithinBounds = ((byte)dr["MONTIMEWITHINBOUNDS"] == 1);
            discountPeriod.MonEndingTimeAfterMidnight = ((byte)dr["MONENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.TueStartingTime = TimeSpan.FromSeconds((int)dr["TUESTARTINGTIME"]);
            discountPeriod.TueEndingTime = TimeSpan.FromSeconds((int)dr["TUEENDINGTIME"]);
            discountPeriod.TueTimeWithinBounds = ((byte)dr["TUETIMEWITHINBOUNDS"] == 1);
            discountPeriod.TueEndingTimeAfterMidnight = ((byte)dr["TUEENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.WedStartingTime = TimeSpan.FromSeconds((int)dr["WEDSTARTINGTIME"]);
            discountPeriod.WedEndingTime = TimeSpan.FromSeconds((int)dr["WEDENDINGTIME"]);
            discountPeriod.WedTimeWithinBounds = ((byte)dr["WEDTIMEWITHINBOUNDS"] == 1);
            discountPeriod.WedEndingTimeAfterMidnight = ((byte)dr["WEDENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.ThuStartingTime = TimeSpan.FromSeconds((int)dr["THUSTARTINGTIME"]);
            discountPeriod.ThuEndingTime = TimeSpan.FromSeconds((int)dr["THUENDINGTIME"]);
            discountPeriod.ThuTimeWithinBounds = ((byte)dr["THUTIMEWITHINBOUNDS"] == 1);
            discountPeriod.ThuEndingTimeAfterMidnight = ((byte)dr["THUENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.FriStartingTime = TimeSpan.FromSeconds((int)dr["FRISTARTINGTIME"]);
            discountPeriod.FriEndingTime = TimeSpan.FromSeconds((int)dr["FRIENDINGTIME"]);
            discountPeriod.FriTimeWithinBounds = ((byte)dr["FRITIMEWITHINBOUNDS"] == 1);
            discountPeriod.FriEndingTimeAfterMidnight = ((byte)dr["FRIENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.SatStartingTime = TimeSpan.FromSeconds((int)dr["SATSTARTINGTIME"]);
            discountPeriod.SatEndingTime = TimeSpan.FromSeconds((int)dr["SATENDINGTIME"]);
            discountPeriod.SatTimeWithinBounds = ((byte)dr["SATTIMEWITHINBOUNDS"] == 1);
            discountPeriod.SatEndingTimeAfterMidnight = ((byte)dr["SATENDINGTIMEAFTERMIDNIGHT"] == 1);

            discountPeriod.SunStartingTime = TimeSpan.FromSeconds((int)dr["SUNSTARTINGTIME"]);
            discountPeriod.SunEndingTime = TimeSpan.FromSeconds((int)dr["SUNENDINGTIME"]);
            discountPeriod.SunTimeWithinBounds = ((byte)dr["SUNTIMEWITHINBOUNDS"] == 1);
            discountPeriod.SunEndingTimeAfterMidnight = ((byte)dr["SUNENDINGTIMEAFTERMIDNIGHT"] == 1);
        }

        private static void PopulateDiscountList(IDataReader dr, DiscountPeriod discountPeriod)
        {
            discountPeriod.ID = (string)dr["ID"];
            discountPeriod.Text = (string)dr["DESCRIPTION"];
            discountPeriod.StartingDate = Date.FromAxaptaDate(dr["STARTINGDATE"]);
            discountPeriod.EndingDate = Date.FromAxaptaDate(dr["ENDINGDATE"]);
            discountPeriod.StartingTime = TimeSpan.FromSeconds((int)dr["STARTINGTIME"]);

            discountPeriod.EndingTime = TimeSpan.FromSeconds((int)dr["ENDINGTIME"]);
            discountPeriod.TimeWithinBounds = ((byte)dr["TIMEWITHINBOUNDS"] == 1);
            discountPeriod.EndingTimeAfterMidnight = ((byte)dr["ENDINGTIMEAFTERMIDNIGHT"] == 1);
        }

        /// <summary>
        /// Gets a discount period with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="discountPeriodID">The ID of the discount period to fetch</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A discount period with a given ID, or null if not found</returns>
        public virtual DiscountPeriod Get(IConnectionManager entry, RecordIdentifier discountPeriodID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select ID,  " +
                        "ISNULL(DESCRIPTION,'') AS DESCRIPTION, " +
                        "ISNULL(StartingDate,'1900-01-01 00:00:00.000') AS STARTINGDATE, " +
                        "ISNULL(ENDINGDATE,'1900-01-01 00:00:00.000') AS ENDINGDATE, " +
                        "ISNULL(STARTINGTIME,0) AS STARTINGTIME, " +
                        "ISNULL(ENDINGTIME,0)AS ENDINGTIME, " +
                        "ISNULL(TIMEWITHINBOUNDS,1) AS TIMEWITHINBOUNDS, " +
                        "ISNULL(ENDTIMEAFTERMID,0) AS ENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(MONSTARTINGTIME,0) AS MONSTARTINGTIME, " +
                        "ISNULL(MONENDINGTIME,0) AS MONENDINGTIME, " +
                        "ISNULL(MONWITHINBOUNDS,1) AS MONTIMEWITHINBOUNDS, " +
                        "ISNULL(MONAFTERMIDNIGHT,0) AS MONENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(TUESTARTINGTIME,0) AS TUESTARTINGTIME, " +
                        "ISNULL(TUEENDINGTIME,0) AS TUEENDINGTIME, " +
                        "ISNULL(TUEWITHINBOUNDS,1) AS TUETIMEWITHINBOUNDS, " +
                        "ISNULL(TUEAFTERMIDNIGHT,0) AS TUEENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(WEDSTARTINGTIME,0) AS WEDSTARTINGTIME, " +
                        "ISNULL(WEDENDINGTIME,0) AS WEDENDINGTIME, " +
                        "ISNULL(WEDWITHINBOUNDS,1) AS WEDTIMEWITHINBOUNDS, " +
                        "ISNULL(WEDAFTERMIDNIGHT,0) AS WEDENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(THUSTARTINGTIME,0) AS THUSTARTINGTIME, " +
                        "ISNULL(THUENDINGTIME,0) AS THUENDINGTIME, " +
                        "ISNULL(THUWITHINBOUNDS,1) AS THUTIMEWITHINBOUNDS, " +
                        "ISNULL(THUAFTERMIDNIGHT,0) AS THUENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(FRISTARTINGTIME,0) AS FRISTARTINGTIME, " +
                        "ISNULL(FRIENDINGTIME,0) AS FRIENDINGTIME, " +
                        "ISNULL(FRIWITHINBOUNDS,1) AS FRITIMEWITHINBOUNDS, " +
                        "ISNULL(FRIAFTERMIDNIGHT,0) AS FRIENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(SATSTARTINGTIME,0) AS SATSTARTINGTIME, " +
                        "ISNULL(SATENDINGTIME,0) AS SATENDINGTIME, " +
                        "ISNULL(SATWITHINBOUNDS,1) AS SATTIMEWITHINBOUNDS, " +
                        "ISNULL(SATAFTERMIDNIGHT,0) AS SATENDINGTIMEAFTERMIDNIGHT, " +

                        "ISNULL(SUNSTARTINGTIME,0) AS SUNSTARTINGTIME, " +
                        "ISNULL(SUNENDINGTIME,0) AS SUNENDINGTIME, " +
                        "ISNULL(SUNWITHINBOUNDS,1) AS SUNTIMEWITHINBOUNDS, " +
                        "ISNULL(SUNAFTERMIDNIGHT,0) AS SUNENDINGTIMEAFTERMIDNIGHT " +

                    "From POSDISCVALIDATIONPERIOD " +
                    "Where DATAAREAID = @dataAreaId and ID = @discountID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "discountID", (string)discountPeriodID);

                return Get<DiscountPeriod>(entry, cmd, discountPeriodID, PopulateDiscount, cacheType,UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and descriptions of all discount periods. The list is ordered by descriptions
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all discount periods</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSDISCVALIDATIONPERIOD", "DESCRIPTION", "ID", "DESCRIPTION");
        } 

        /// <summary>
        /// Gets a list of all discount periods, ordered by a given field name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">The field name to orderd results by</param>
        /// <returns>A list of all discount periods, ordered by a given field name</returns>
        public virtual List<DiscountPeriod> GetDiscountPeriods(IConnectionManager entry, string sort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select ID,  " +
                        "ISNULL(DESCRIPTION,'') AS DESCRIPTION, " +
                        "ISNULL(StartingDate,'1900-01-01 00:00:00.000') AS STARTINGDATE, " +
                        "ISNULL(ENDINGDATE,'1900-01-01 00:00:00.000') AS ENDINGDATE, " +
                        "ISNULL(STARTINGTIME,0) AS STARTINGTIME, " +
                        "ISNULL(ENDINGTIME,0) AS ENDINGTIME, " +
                        "ISNULL(TIMEWITHINBOUNDS,1) AS TIMEWITHINBOUNDS, " +
                        "ISNULL(ENDTIMEAFTERMID,1) AS ENDINGTIMEAFTERMIDNIGHT " +

                    "From POSDISCVALIDATIONPERIOD " +
                    "Where DATAAREAID = @dataAreaId " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                
                return Execute<DiscountPeriod>(entry, cmd, CommandType.Text, PopulateDiscountList);
            }
        }

        /// <summary>
        /// Checks if a discount periods with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="discountPeriodID">The ID of the discount period to check for</param>
        /// <returns>Whether a discount group with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier discountPeriodID)
        {
            return RecordExists<DiscountPeriod>(entry, "POSDISCVALIDATIONPERIOD", "ID", discountPeriodID);
        }

        /// <summary>
        /// Saves a given discount period to the database
        /// </summary>
        /// <remarks>Requires the 'Manage discounts' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="discountPeriod">The discount period to save</param>
        public virtual void Save(IConnectionManager entry, DiscountPeriod discountPeriod)
        {
            var statement = new SqlServerStatement("POSDISCVALIDATIONPERIOD");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiscounts);

            bool isNew = false;
            if (discountPeriod.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                discountPeriod.ID = DataProviderFactory.Instance.GenerateNumber<IDiscountPeriodData, DiscountPeriod>(entry);
            }

            if (isNew || !Exists(entry, discountPeriod.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)discountPeriod.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)discountPeriod.ID);
            }

            statement.AddField("DESCRIPTION", discountPeriod.Text);
            statement.AddField("STARTINGDATE", discountPeriod.StartingDate.ToAxaptaSQLDate().Date, SqlDbType.DateTime);
            statement.AddField("ENDINGDATE", discountPeriod.EndingDate.ToAxaptaSQLDate().Date, SqlDbType.DateTime);
            statement.AddField("STARTINGTIME", discountPeriod.StartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("ENDINGTIME", discountPeriod.EndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("TIMEWITHINBOUNDS", (discountPeriod.TimeWithinBounds ? 1:0), SqlDbType.TinyInt);
            statement.AddField("ENDTIMEAFTERMID", (discountPeriod.EndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("MONSTARTINGTIME", discountPeriod.MonStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("MONENDINGTIME", discountPeriod.MonEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("MONWITHINBOUNDS", (discountPeriod.MonTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("MONAFTERMIDNIGHT", (discountPeriod.MonEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("TUESTARTINGTIME", discountPeriod.TueStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("TUEENDINGTIME", discountPeriod.TueEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("TUEWITHINBOUNDS", (discountPeriod.TueTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("TUEAFTERMIDNIGHT", (discountPeriod.TueEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("WEDSTARTINGTIME", discountPeriod.WedStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("WEDENDINGTIME", discountPeriod.WedEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("WEDWITHINBOUNDS", (discountPeriod.WedTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("WEDAFTERMIDNIGHT", (discountPeriod.WedEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("THUSTARTINGTIME", discountPeriod.ThuStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("THUENDINGTIME", discountPeriod.ThuEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("THUWITHINBOUNDS", (discountPeriod.ThuTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("THUAFTERMIDNIGHT", (discountPeriod.ThuEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("FRISTARTINGTIME", discountPeriod.FriStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("FRIENDINGTIME", discountPeriod.FriEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("FRIWITHINBOUNDS", (discountPeriod.FriTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("FRIAFTERMIDNIGHT", (discountPeriod.FriEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("SATSTARTINGTIME", discountPeriod.SatStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("SATENDINGTIME", discountPeriod.SatEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("SATWITHINBOUNDS", (discountPeriod.SatTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("SATAFTERMIDNIGHT", (discountPeriod.SatEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            statement.AddField("SUNSTARTINGTIME", discountPeriod.SunStartingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("SUNENDINGTIME", discountPeriod.SunEndingTime.TotalSeconds, SqlDbType.Int);
            statement.AddField("SUNWITHINBOUNDS", (discountPeriod.SunTimeWithinBounds ? 1 : 0), SqlDbType.TinyInt);
            statement.AddField("SUNAFTERMIDNIGHT", (discountPeriod.SunEndingTimeAfterMidnight ? 1 : 0), SqlDbType.TinyInt);

            Save(entry, discountPeriod, statement);
        }

        /// <summary>
        /// Deletes a discount period
        /// </summary>
        /// <remarks>Requires the 'Manage discounts' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="discountPeriodID">The ID of the discount period to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier discountPeriodID)
        {
            DeleteRecord<DiscountPeriod>(entry, "POSDISCVALIDATIONPERIOD", "ID", discountPeriodID, BusinessObjects.Permission.ManageDiscounts);
        }

        public virtual bool IsDiscountPeriodValid(IConnectionManager entry, RecordIdentifier discountPeriodID, DateTime dateTime)
        {
            var emptyDate = new DateTime(1900, 1, 1); //01.01.1900, is an empty date in Axpata
            var discValidationPeriod = Get(entry, discountPeriodID);
            var transDate = dateTime.Date;
            var transTime = dateTime.TimeOfDay;

            if (discValidationPeriod == null) return false;

            if ((discValidationPeriod.StartingDate.DateTime <= transDate) && (transDate <= discValidationPeriod.EndingDate.DateTime)
                || ((discValidationPeriod.StartingDate.DateTime <= transDate) && (discValidationPeriod.EndingDate.DateTime == emptyDate))
                || ((discValidationPeriod.StartingDate.DateTime == emptyDate) && (discValidationPeriod.EndingDate.DateTime == emptyDate))
                )
            {
                double startTime = 0;
                double endTime = 0;
                bool withInBounds = false;
                bool afterMidnight = false;

                switch (transDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        {
                            startTime = discValidationPeriod.SunStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.SunEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.SunTimeWithinBounds;
                            afterMidnight = discValidationPeriod.SunEndingTimeAfterMidnight;
                        }
                        break;
                    case DayOfWeek.Monday:
                        {
                            startTime = discValidationPeriod.MonStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.MonEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.MonTimeWithinBounds;
                            afterMidnight = discValidationPeriod.MonEndingTimeAfterMidnight;
                        }
                        break;
                    case DayOfWeek.Tuesday:
                        {
                            startTime = discValidationPeriod.TueStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.TueEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.TueTimeWithinBounds;
                            afterMidnight = discValidationPeriod.TueEndingTimeAfterMidnight;
                        }
                        break;
                    case DayOfWeek.Wednesday:
                        {
                            startTime = discValidationPeriod.WedStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.WedEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.WedTimeWithinBounds;
                            afterMidnight = discValidationPeriod.WedEndingTimeAfterMidnight;
                        }
                        break;
                    case DayOfWeek.Thursday:
                        {
                            startTime = discValidationPeriod.ThuStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.ThuEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.ThuTimeWithinBounds;
                            afterMidnight = discValidationPeriod.ThuEndingTimeAfterMidnight;
                        }
                        break;
                    case DayOfWeek.Friday:
                        {
                            startTime = discValidationPeriod.FriStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.FriEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.FriTimeWithinBounds;
                            afterMidnight = discValidationPeriod.FriEndingTimeAfterMidnight;
                        }
                        break;
                    case DayOfWeek.Saturday:
                        {
                            startTime = discValidationPeriod.SatStartingTime.TotalSeconds;
                            endTime = discValidationPeriod.SatEndingTime.TotalSeconds;
                            withInBounds = discValidationPeriod.SatTimeWithinBounds;
                            afterMidnight = discValidationPeriod.SatEndingTimeAfterMidnight;
                        }
                        break;
                }

                //If nothing is found for the weekdays we will use the general conditions
                if ((startTime == 0) && (endTime == 0))
                {
                    startTime = discValidationPeriod.StartingTime.TotalSeconds;
                    endTime = discValidationPeriod.EndingTime.TotalSeconds;
                    withInBounds = discValidationPeriod.TimeWithinBounds;
                    afterMidnight = discValidationPeriod.EndingTimeAfterMidnight;
                }

                if ((startTime == 0) && (endTime == 0))
                {
                    return withInBounds;
                }

                int timeInSeconds = Convert.ToInt32(transTime.TotalSeconds);


                if ((startTime <= timeInSeconds) && (timeInSeconds <= endTime))
                {
                    return withInBounds;
                }

                if (afterMidnight && (timeInSeconds <= endTime))
                {
                    return withInBounds;
                }

                return (!withInBounds);
            }
            return false;
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "DISCOUNTPERIOD"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSDISCVALIDATIONPERIOD", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
