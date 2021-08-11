using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Loyalty
{
    public class LoyaltyCustomerData : SqlServerDataProviderBase, ILoyaltyCustomerData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                    "SELECT ISNULL(ACCOUNTNUM, '') ACCOUNTNUM " +
                    ", ISNULL([LOYALTYCUSTOMER], 0) LOYALTYCUSTOMER"
                    + ", ISNULL([NAME], 0) NAME"
                    + " FROM CUSTTABLE ";
            }
        }

        private static void PopulateLoyaltyCustomer(IDataReader dr, LoyaltyCustomer loyaltyCustomer)
        {
            loyaltyCustomer.CustomerID = (string)dr["ACCOUNTNUM"];
            //loyaltyCustomer.IsLoyaltyCustomer = ((byte)dr["LOYALTYCUSTOMER"] == 1);
            loyaltyCustomer.Name = (string)dr["NAME"];
            loyaltyCustomer.Text = loyaltyCustomer.Name;
            if (String.IsNullOrEmpty(loyaltyCustomer.Text))
            {
                loyaltyCustomer.Text = (string)loyaltyCustomer.CustomerID;
            }
        }

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyalityCustomerID">The loyality customer ID.</param>
        /// <returns>An instance of <see cref="LoyaltyCustomer"/>.</returns>
        public virtual LoyaltyCustomer Get(IConnectionManager entry, RecordIdentifier loyalityCustomerID)
        {
            List<LoyaltyCustomer> result;

            if ((loyalityCustomerID == null) || ((string)loyalityCustomerID == ""))
            {
                return null;
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where ACCOUNTNUM = @cardNumber and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "cardNumber", (string)loyalityCustomerID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyCustomer>(entry, cmd, CommandType.Text, PopulateLoyaltyCustomer);
            }

            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Gets the list of loyalty customers.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyCustomersOnly">return only customers with LOYALTYCUSTOMER = 1.</param>
        /// <returns>A list of <see cref="LoyaltyCustomer"/>.</returns>
        public virtual List<LoyaltyCustomer> GetList(IConnectionManager entry, bool loyaltyCustomersOnly = true)
        {
            List<LoyaltyCustomer> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";
                if (loyaltyCustomersOnly)
                {
                    cmd.CommandText = cmd.CommandText + " and LOYALTYCUSTOMER = 1";
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyCustomer>(entry, cmd, CommandType.Text, PopulateLoyaltyCustomer);
            }

            return result;
        }

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The customer ID</param>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier customerID)
        {
            return RecordExists(entry, "CUSTTABLE", new[] { "ACCOUNTNUM" }, customerID);
        }

        /// <summary>
        /// Saves the given class to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyCustomer">The customer to be saved to the database</param>
        public virtual void Save(IConnectionManager entry, LoyaltyCustomer loyaltyCustomer)
        {
            SqlServerStatement statement = new SqlServerStatement("CUSTTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.CustomerEdit);

            if (!Exists(entry, loyaltyCustomer.CustomerID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ACCOUNTNUM", (string)loyaltyCustomer.CustomerID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", (string)loyaltyCustomer.CustomerID);
            }

            //statement.AddField("LOYALTYCUSTOMER", loyaltyCustomer.IsLoyaltyCustomer, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
