using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Currencies
{
    /// <summary>
    /// Data prover class for cash declarations. Cash Declarations are definitions of sizes within a currency, f.x. the US currency has 0.01 (Penny), 0.05 (Nickel) ...
    /// </summary>
    public class CashDenominatorData : SqlServerDataProviderBase, ICashDenominatorData
    {
        private static void PopulateCashDenomination(IDataReader dr, CashDenominator cashDenominator)
        {
            cashDenominator.CurrencyCode = (string)dr["CURRENCY"];
            cashDenominator.CashType = (CashDenominator.Type)((int)dr["TYPE"]);
            cashDenominator.Amount = (decimal)dr["AMOUNT"];
            cashDenominator.Denomination = (string)dr["DENOMINATIONDESCRIPTION"];
        }
        
        /// <summary>
        /// Checks if a cash declaration with a given ID exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the cash declaration to check for</param>
        /// <returns>Whether a cash declaration with a given ID exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOSTORECASHDECLARATIONTABLE", new[] { "AMOUNT", "TYPE", "CURRENCY" }, id);
        }

        /// <summary>
        /// Deletes a cash declaration with a given ID
        /// </summary>
        /// <remarks>Requires the 'CurrencyEdit' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the cash declaration to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOSTORECASHDECLARATIONTABLE", new[] { "AMOUNT", "TYPE", "CURRENCY" }, id,
                LSOne.DataLayer.BusinessObjects.Permission.CurrencyEdit);
        }

        /// <summary>
        /// Gets a cash declaration with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cashDeclarationID">The ID of the cash declaration to get</param>
        /// <returns>A cash declaration with a given ID</returns>
        public virtual CashDenominator Get(IConnectionManager entry, RecordIdentifier cashDeclarationID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select CURRENCY, TYPE, AMOUNT, ISNULL(DENOMINATIONDESCRIPTION, '') AS DENOMINATIONDESCRIPTION " +
                    "From RBOSTORECASHDECLARATIONTABLE " +
                    "Where DATAAREAID = @dataareaid AND CURRENCY = @currencyCode and TYPE = @type and AMOUNT = @amount";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "currencyCode", (string)cashDeclarationID.PrimaryID);
                MakeParam(cmd, "type", (string)cashDeclarationID.SecondaryID.PrimaryID);
                MakeParam(cmd, "amount", (string)cashDeclarationID.SecondaryID.SecondaryID.PrimaryID);

                var result = Execute<CashDenominator>(entry, cmd, CommandType.Text, PopulateCashDenomination);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Get a list of all cash declarations for a given currency. The list is sorted by a column index, and ordered ascending or descending based on the sortedBackwards parameter.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">The currency code to get cash declarations for</param>
        /// <param name="sortColumn">The column index to sort by. The columns are ["TYPE", "AMOUNT"]</param>
        /// <param name="sortedBackwards">Wheter to sort by ascending order or not</param>
        /// <returns>A list of all cash declarations for a given currency</returns>
        public virtual List<CashDenominator> GetCashDenominators(IConnectionManager entry, RecordIdentifier currencyCode, int sortColumn, bool sortedBackwards)
        {
            ValidateSecurity(entry);

            string[] columns = { "TYPE", "AMOUNT" };

            string sort = "";

            if (sortColumn < columns.Length)
            {
                sort = " order by " + columns[sortColumn] + (sortedBackwards ? " DESC" : " ASC");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select CURRENCY, TYPE, AMOUNT, ISNULL(DENOMINATIONDESCRIPTION, '') AS DENOMINATIONDESCRIPTION " +
                    "From RBOSTORECASHDECLARATIONTABLE " +
                    "Where DATAAREAID = @dataareaid AND CURRENCY = @currencyCode" + sort;

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "currencyCode", (string)currencyCode.PrimaryID);

                return Execute<CashDenominator>(entry, cmd, CommandType.Text, PopulateCashDenomination);
            }
        }

        public virtual List<CashDenominator> GetBills(IConnectionManager entry, RecordIdentifier currencyCode)
        {
            ValidateSecurity(entry);
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT CURRENCY, TYPE, AMOUNT, ISNULL(DENOMINATIONDESCRIPTION, '') AS DENOMINATIONDESCRIPTION " +
                                  "FROM RBOSTORECASHDECLARATIONTABLE " +
                                  "WHERE DATAAREAID = @dataAreaID AND CURRENCY =  @currencyCode AND TYPE = 1 " +
                                  "ORDER BY AMOUNT ";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "currencyCode", (string) currencyCode);
                return Execute<CashDenominator>(entry, cmd, CommandType.Text, PopulateCashDenomination);
            }
        }

        /// <summary>
        /// Saves a given cash declaration into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cashDeclaration">The cash declaration to save</param>
        public virtual void Save(IConnectionManager entry, CashDenominator cashDeclaration)
        {
            var statement = new SqlServerStatement("RBOSTORECASHDECLARATIONTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.CurrencyEdit);

            if (Exists(entry,cashDeclaration.ID))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STOREID", "");
                statement.AddCondition("TYPE", (int)cashDeclaration.CashType, SqlDbType.Int);
                statement.AddCondition("CURRENCY", cashDeclaration.CurrencyCode);
                statement.AddCondition("AMOUNT", cashDeclaration.Amount, SqlDbType.Decimal);
            }
            else
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STOREID", "");
                statement.AddKey("TYPE", (int)cashDeclaration.CashType, SqlDbType.Int);
                statement.AddKey("CURRENCY", cashDeclaration.CurrencyCode);
                statement.AddKey("AMOUNT", cashDeclaration.Amount, SqlDbType.Decimal);
            }
            
            statement.AddField("DENOMINATIONDESCRIPTION", cashDeclaration.Denomination);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
