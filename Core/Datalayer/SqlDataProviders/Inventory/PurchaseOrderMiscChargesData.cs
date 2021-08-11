
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    /// <summary>
    /// Data provider class for purchase order lines
    /// </summary>
    public class PurchaseOrderMiscChargesData : SqlServerDataProviderBase, IPurchaseOrderMiscChargesData
    {
        private static string BaseSqlString
        {
            get
            {
                return @"SELECT PURCHASEORDERID, 
                        LINENUMBER, 
                        ISNULL(TYPE,0) as TYPE , 
                        ISNULL(REASON,'') as REASON,
                        ISNULL(AMOUNT,0) as AMOUNT, 
                        ISNULL(TAXAMOUNT,0) as TAXAMOUNT,
                        ISNULL(PC.TAXGROUP, '') as TAXGROUP,
                        ISNULL(TH.TAXGROUPNAME, '') AS TAXGROUPNAME
                        FROM PURCHASEORDERMISCCHARGES PC
                        LEFT JOIN TAXGROUPHEADING TH ON TH.DATAAREAID = PC.DATAAREAID AND PC.TAXGROUP = TH.TAXGROUP ";
            }
        }

        private static string ResolveSort(PurchaseOrderMiscChargesSorting sort, bool reverseSort)
        {
            var sortString = "";

            switch (sort)
            {
                case PurchaseOrderMiscChargesSorting.PurchaseOrderID:
                    sortString = "PURCHASEORDERID DESC";
                    break;
                case PurchaseOrderMiscChargesSorting.LineNumber:
                    sortString = "LINENUMBER DESC";
                    break;
                case PurchaseOrderMiscChargesSorting.Type:
                    sortString = "TYPE DESC";
                    break;
                case PurchaseOrderMiscChargesSorting.Reason:
                    sortString = "REASON DESC";
                    break;
                case PurchaseOrderMiscChargesSorting.Amount:
                    sortString = "AMOUNT DESC";
                    break;
                case PurchaseOrderMiscChargesSorting.TaxAmount:
                    sortString = "TAXAMOUNT DESC";
                    break;
                case PurchaseOrderMiscChargesSorting.TaxGroup:
                    sortString = "TAXGROUPNAME DESC";
                    break;
            }

            if (reverseSort)
            {
                sortString = sortString.Replace("DESC", "ASC");
            }

            return sortString;
        }

        private static void PopulatePurchaseOrderMiscCharge(IConnectionManager entry, IDataReader dr, PurchaseOrderMiscCharges miscCharge, object includeReportFormatting)
        {
            if ((bool)includeReportFormatting)
            {
                miscCharge.PriceLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Prices);                
            }

            miscCharge.PurchaseOrderID = (string)dr["PURCHASEORDERID"];
            miscCharge.LineNumber = (string)dr["LINENUMBER"];
            miscCharge.Type = (PurchaseOrderMiscCharges.PurchaseOrderMiscChargesEnum)dr["TYPE"];
            miscCharge.Reason = (string)dr["REASON"];
            miscCharge.Amount = (decimal)dr["AMOUNT"];
            miscCharge.TaxAmount = (decimal)dr["TAXAMOUNT"];
            miscCharge.TaxGroupID = (string)dr["TAXGROUP"]; 
            miscCharge.TaxGroupName = (string)dr["TAXGROUPNAME"];
        }

        /// <summary>
        /// Checks if a purchase order misc charge with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderMiscChargesID">The ID of the purchase order misc charge to check for</param>
        /// <returns>Whether a purchase order misc charge with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier purchaseOrderMiscChargesID)
        {
            return RecordExists(entry, "PURCHASEORDERMISCCHARGES", new[] { "PURCHASEORDERID", "LINENUMBER" }, purchaseOrderMiscChargesID);
        }

        private static bool LineNumberExists(IConnectionManager entry, RecordIdentifier lineNumber)
        {
            return RecordExists(entry, "PURCHASEORDERMISCCHARGES", "LINENUMBER", lineNumber);
        }

        /// <summary>
        /// Deletes a purchase order misc charge with a given ID
        /// </summary>
        /// <remarks>Requires the 'ManagePurchaseOrders' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderMiscChargesID">The ID of the purchase order misc charge to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier purchaseOrderMiscChargesID)
        {
            DeleteRecord(entry, "PURCHASEORDERMISCCHARGES", new[] { "PURCHASEORDERID", "LINENUMBER" }, purchaseOrderMiscChargesID, BusinessObjects.Permission.ManagePurchaseOrders);
        }

        /// <summary>
        /// Gets a purchase order misc charge with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderMiscCharge">The ID of the purchase order misc charge to get</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        public virtual PurchaseOrderMiscCharges Get(IConnectionManager entry, RecordIdentifier purchaseOrderMiscCharge, bool includeReportFormatting)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSqlString +
                    "WHERE PC.DATAAREAID = @DATAAREAID AND PURCHASEORDERID = @PURCHASEORDERID AND LINENUMBER = @LINENUMBER"; 

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderMiscCharge.PrimaryID);
                MakeParam(cmd, "LINENUMBER", (string)purchaseOrderMiscCharge.SecondaryID);

                var result = Execute<PurchaseOrderMiscCharges>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrderMiscCharge);
                return (result.Count > 0) ? result[0] : null;
            }            
        }

        /// <summary>
        /// Gets a purchase order misc charges for a given purchase order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get misc charges for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <param name="sort">An enum that tells us which column to sort by</param>
        /// <param name="sortBackwards">Whether to reverse the result set</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        public virtual List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID, PurchaseOrderMiscChargesSorting sort, bool sortBackwards, bool includeReportFormatting)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSqlString +
                    "WHERE PC.DATAAREAID = @DATAAREAID AND PC.PURCHASEORDERID = @PURCHASEORDERID Order by " + ResolveSort(sort,sortBackwards);

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderID);

                return Execute<PurchaseOrderMiscCharges>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrderMiscCharge);
            }
        }

        /// <summary>
        /// Saves a given purchase order misc charge into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderMiscCharge">The Purchase order misc charge to save</param>
        public virtual void Save(IConnectionManager entry, PurchaseOrderMiscCharges purchaseOrderMiscCharge)
        {
            SqlServerStatement statement = new SqlServerStatement("PURCHASEORDERMISCCHARGES");

            ValidateSecurity(entry, BusinessObjects.Permission.ManagePurchaseOrders);
            
            if (purchaseOrderMiscCharge.LineNumber == RecordIdentifier.Empty)
            {
                purchaseOrderMiscCharge.LineNumber = DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderMiscChargesData, PurchaseOrderMiscCharges>(entry);
            }

            if (!Exists(entry, purchaseOrderMiscCharge.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PURCHASEORDERID", (string)purchaseOrderMiscCharge.PurchaseOrderID);
                statement.AddKey("LINENUMBER", (string)purchaseOrderMiscCharge.LineNumber);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PURCHASEORDERID", (string)purchaseOrderMiscCharge.PurchaseOrderID);
                statement.AddCondition("LINENUMBER", (string)purchaseOrderMiscCharge.LineNumber);
            }

            statement.AddField("REASON", purchaseOrderMiscCharge.Reason);
            statement.AddField("TYPE", (int)purchaseOrderMiscCharge.Type, SqlDbType.Int);
            statement.AddField("AMOUNT", purchaseOrderMiscCharge.Amount, SqlDbType.Decimal);
            statement.AddField("TAXAMOUNT", purchaseOrderMiscCharge.TaxAmount, SqlDbType.Decimal);
            statement.AddField("TAXGROUP", (string)purchaseOrderMiscCharge.TaxGroupID);
            
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Copies all misc charges from oldPurchaseOrderID to newPurchaseOrderID
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="oldPurchaseOrderID"></param>
        /// <param name="newPurchaseOrderID"></param>
        public virtual void CopyMiscChargesBetweenPOs(IConnectionManager entry, RecordIdentifier oldPurchaseOrderID, RecordIdentifier newPurchaseOrderID)
        {
            var miscCharges = GetMischChargesForPurchaseOrder(entry, oldPurchaseOrderID, PurchaseOrderMiscChargesSorting.Amount, false, false);
            
            foreach (var miscCharge in miscCharges)
            {
                miscCharge.PurchaseOrderID = newPurchaseOrderID;
                Save(entry, miscCharge);
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return LineNumberExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "PURCHASEORDERMISCC"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "PURCHASEORDERMISCCHARGES", "LINENUMBER", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
