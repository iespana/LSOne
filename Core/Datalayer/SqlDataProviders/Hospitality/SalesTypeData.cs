using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    /// <summary>
    /// Data provider class for Sales types
    /// </summary>
    public class SalesTypeData : SqlServerDataProviderBase, ISalesTypeData
    {
        private static string ResolveSort(SalesTypeSorting sort, bool backwards)
        {
            switch (sort)
            {
                case SalesTypeSorting.SalesTypeId:
                    return backwards ? "CODE DESC" : "CODE ASC";

                case SalesTypeSorting.SalesTypeDescription:
                    return backwards ? "DESCRIPTION DESC" : "DESCRIPTION ASC";
            }

            return "";
        }

        private static void PopulateSalesType(IDataReader dr, SalesType salesType)
        {
            salesType.ID = (string)dr["CODE"];
            salesType.Text = (string)dr["DESCRIPTION"];
            salesType.RequestSalesperson = ((byte)dr["REQUESTSALESPERSON"] != 0);
            salesType.RequestDepositPerc = (int)dr["REQUESTDEPOSITPERC"];
            salesType.RequestChargeAccount = ((byte)dr["REQUESTCHARGEACCOUNT"] != 0);
            salesType.PurchasingCode = (string)dr["PURCHASINGCODE"];
            salesType.DefaultOrderLimit = (decimal)dr["DEFAULTORDERLIMIT"];
            salesType.LimitSetting = (SalesType.LimitSettingEnum)(int)dr["LIMITSETTING"];
            salesType.RequestConfirmation = ((byte)dr["REQUESTCONFIRMATION"] != 0);
            salesType.RequestDescription = ((byte)dr["REQUESTDESCRIPTION"] != 0);
            salesType.NewGlobalDimension2 = (string)dr["NEWGLOBALDIMENSION2"];
            salesType.SuspendPrinting = (SalesType.SuspendPrintingEnum)(int)dr["SUSPENDPRINTING"];
            salesType.SuspendType = (SalesType.SuspendTypeEnum)(int)dr["SUSPENDTYPE"];
            salesType.PrePaymentAccountNo = (string)dr["PREPAYMENTACCOUNTNO"];
            salesType.MinimumDeposit = (decimal)dr["MINIMUMDEPOSIT"];
            salesType.PrintItemLinesOnPosSlip = ((byte)dr["PRINTITEMLINESONPOSSLIP"] != 0);
            salesType.VoidedPrepaymentAccountNo = (string)dr["VOIDEDPREPAYMENTACCOUNTNO"];
            salesType.DaysOpenTransExist = (int)dr["DAYSOPENTRANSEXIST"];
            salesType.TaxGroupID = (string)dr["TAXGROUPID"];
            salesType.PriceGroup = (string)dr["PRICEGROUP"];
            salesType.TransDeleteReminder = (int)dr["TRANSDELETEREMINDER"];
            salesType.LocationCode = (string)dr["LOCATIONCODE"];
            salesType.PaymentIsPrepayment = ((byte)dr["PAYMENTISPREPAYMENT"] != 0);
            salesType.CalcPriceFromVatPrice = ((byte)dr["CALCPRICEFROMVATPRICE"] != 0);
        }


        /// <summary>
        /// Gets a list of data entities containing ID and name for each sales type, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all sales types, ordered by a chosen field</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, SalesTypeSorting sortBy, bool sortBackwards)
        {
            if (sortBy != SalesTypeSorting.SalesTypeId && sortBy != SalesTypeSorting.SalesTypeDescription)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, "SALESTYPE", "DESCRIPTION", "CODE", ResolveSort(sortBy, sortBackwards));
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all sales types, ordered by the sales type description
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of sales types, ordered by the sales type description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "SALESTYPE", "DESCRIPTION", "CODE", "DESCRIPTION");
        }

        /// <summary>
        /// Gets a data entity for Sales type wich contains the Code (ID) field and description field for the given sales type id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="code">The code (ID) field for the sales type</param>
        /// <returns>A SalesType object with the givn Code (ID), or null if the sales type was not found</returns>
        public virtual SalesType GetSalesTypeIdDescription(IConnectionManager entry, RecordIdentifier code)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select CODE, ISNULL(DESCRIPTION,'') as Description from SALESTYPE where DATAAREAID = @dataAreaId and CODE = @code";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "code", (string) code);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return new SalesType((string) dr["CODE"], (string) dr["DESCRIPTION"]);
                    }

                    return null;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a sales type with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID of the sales type to get</param>
        /// <returns>A sales type with a given ID. If no sales type if found a null value is returned</returns>
        public virtual SalesType Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "select CODE, " +
                    "ISNULL(DESCRIPTION, '') as DESCRIPTION, " +
                    "ISNULL(REQUESTSALESPERSON, 0) as REQUESTSALESPERSON, " +
                    "ISNULL(REQUESTDEPOSITPERC, 0) as REQUESTDEPOSITPERC, " +
                    "ISNULL(REQUESTCHARGEACCOUNT, 0) as REQUESTCHARGEACCOUNT, " +
                    "ISNULL(PURCHASINGCODE, '') as PURCHASINGCODE, " +
                    "ISNULL(DEFAULTORDERLIMIT, 0) as DEFAULTORDERLIMIT, " +
                    "ISNULL(LIMITSETTING, 0) as LIMITSETTING, " +
                    "ISNULL(REQUESTCONFIRMATION, 0) as REQUESTCONFIRMATION, " +
                    "ISNULL(REQUESTDESCRIPTION, 0) as REQUESTDESCRIPTION, " +
                    "ISNULL(NEWGLOBALDIMENSION2, '') as NEWGLOBALDIMENSION2, " +
                    "ISNULL(SUSPENDPRINTING, 0) as SUSPENDPRINTING, " +
                    "ISNULL(SUSPENDTYPE, 0) as SUSPENDTYPE, " +
                    "ISNULL(PREPAYMENTACCOUNTNO, '') as PREPAYMENTACCOUNTNO, " +
                    "ISNULL(MINIMUMDEPOSIT, 0) as MINIMUMDEPOSIT, " +
                    "ISNULL(PRINTITEMLINESONPOSSLIP, 0) as PRINTITEMLINESONPOSSLIP, " +
                    "ISNULL(VOIDEDPREPAYMENTACCOUNTNO, '') as VOIDEDPREPAYMENTACCOUNTNO, " +
                    "ISNULL(DAYSOPENTRANSEXIST, 0) as DAYSOPENTRANSEXIST, " +
                    "ISNULL(TAXGROUPID, '') as TAXGROUPID, " +
                    "ISNULL(PRICEGROUP, '') as PRICEGROUP, " +
                    "ISNULL(TRANSDELETEREMINDER, 0) as TRANSDELETEREMINDER, " +
                    "ISNULL(LOCATIONCODE, '') as LOCATIONCODE, " +
                    "ISNULL(PAYMENTISPREPAYMENT, 0) as PAYMENTISPREPAYMENT, " +
                    "ISNULL(CALCPRICEFROMVATPRICE, 0) as CALCPRICEFROMVATPRICE " +
                    "from SALESTYPE " +
                    "where CODE = @id and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) id);

                var result = Execute<SalesType>(entry, cmd, CommandType.Text, PopulateSalesType);
                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a given sales type exists.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The id of the sales type to look for</param>
        /// <returns>True if the sales typ exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "SALESTYPE", "CODE", id);
        }

        /// <summary>
        /// Deletes a sales type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="salesTypeID">The id of the sales type to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier salesTypeID)
        {
            DeleteRecord(entry, "SALESTYPE", "CODE", salesTypeID, BusinessObjects.Permission.ManageSalesTypes);
        }

        /// <summary>
        /// Inserts a new sales type or updates an existing one
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="salesType">The sales type object to save</param>
        public virtual void Save(IConnectionManager entry, SalesType salesType)
        {
            var statement = new SqlServerStatement("SALESTYPE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageSalesTypes);

            bool isNew = false;
            if (salesType.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                salesType.ID = DataProviderFactory.Instance.GenerateNumber<ISalesTypeData, SalesType>(entry);
            }

            if (isNew || !Exists(entry, salesType.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CODE", (string)salesType.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CODE", (string)salesType.ID);
            }

            statement.AddField("DESCRIPTION", salesType.Text);
            statement.AddField("REQUESTSALESPERSON", salesType.RequestSalesperson ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REQUESTDEPOSITPERC", salesType.RequestDepositPerc, SqlDbType.Int);
            statement.AddField("REQUESTCHARGEACCOUNT", salesType.RequestChargeAccount ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PURCHASINGCODE", salesType.PurchasingCode);
            statement.AddField("DEFAULTORDERLIMIT", salesType.DefaultOrderLimit, SqlDbType.Decimal);
            statement.AddField("LIMITSETTING", salesType.LimitSetting, SqlDbType.Int);
            statement.AddField("REQUESTCONFIRMATION", salesType.RequestConfirmation ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REQUESTDESCRIPTION", salesType.RequestDescription ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("NEWGLOBALDIMENSION2", salesType.NewGlobalDimension2);
            statement.AddField("SUSPENDPRINTING", (int)salesType.SuspendPrinting, SqlDbType.Int);
            statement.AddField("SUSPENDTYPE", (int)salesType.SuspendType, SqlDbType.Int);
            statement.AddField("PREPAYMENTACCOUNTNO", salesType.PrePaymentAccountNo);
            statement.AddField("MINIMUMDEPOSIT", salesType.MinimumDeposit, SqlDbType.Decimal);
            statement.AddField("PRINTITEMLINESONPOSSLIP", salesType.PrintItemLinesOnPosSlip ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VOIDEDPREPAYMENTACCOUNTNO", salesType.VoidedPrepaymentAccountNo);
            statement.AddField("DAYSOPENTRANSEXIST", salesType.DaysOpenTransExist, SqlDbType.Int);
            statement.AddField("TAXGROUPID", salesType.TaxGroupID);
            statement.AddField("PRICEGROUP", salesType.PriceGroup);
            statement.AddField("TRANSDELETEREMINDER", salesType.TransDeleteReminder, SqlDbType.Int);
            statement.AddField("LOCATIONCODE", salesType.LocationCode);
            statement.AddField("PAYMENTISPREPAYMENT", salesType.PaymentIsPrepayment ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CALCPRICEFROMVATPRICE", salesType.CalcPriceFromVatPrice ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "SALESTYPES"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "SALESTYPE", "CODE", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}