using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using LSRetail.StoreController.Administration.DataLayer.DataEntities;
using LSRetail.StoreController.SharedDatabase;
using LSRetail.Utilities.Cryptography;
using LSRetail.Utilities;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.StoreController.SharedDatabase.DataEntities;

namespace LSRetail.StoreController.Administration.DataLayer
{
    internal class DiscountAndPriceActivationData : DataProviderBase
    {
        private static void PopulateDiscountAndPriceActivation(SqlDataReader dr, DataEntities.DiscountAndPriceActivation discountAndPriceActivation)
        {
            discountAndPriceActivation.PriceCustomerItem = ((byte)dr["PRICECUSTOMERITEM"] == 1);
            discountAndPriceActivation.PriceCustomerGroupItem = ((byte)dr["PRICECUSTOMERGROUPITEM"] == 1);
            discountAndPriceActivation.PriceAllCustomersItem = ((byte)dr["PRICEALLCUSTOMERSITEM"] == 1);

            discountAndPriceActivation.LineDiscountCustomerItem = ((byte)dr["LINEDISCOUNTCUSTOMERITEM"] == 1);
            discountAndPriceActivation.LineDiscountCustomerGroupItem = ((byte)dr["LINEDISCOUNTCUSTOMERGROUPITEM"] == 1);
            discountAndPriceActivation.LineDiscountAllCustomersItem = ((byte)dr["LINEDISCOUNTALLCUSTOMERSITEM"] == 1);
            discountAndPriceActivation.LineDiscountCustomerItemGroup = ((byte)dr["LINEDISCOUNTCUSTOMERITEMGROUP"] == 1);
            discountAndPriceActivation.LineDiscountCustomerGroupItemGroup = ((byte)dr["LINEDISCOUNTCUSTOMERGROUPITEMGROUP"] == 1);
            discountAndPriceActivation.LineDiscountAllCustomersItemGroup = ((byte)dr["LINEDISCOUNTALLCUSTOMERSITEMGROUP"] == 1);
            discountAndPriceActivation.LineDiscountCustomerAllItems = ((byte)dr["LINEDISCOUNTCUSTOMERALLITEMS"] == 1);
            discountAndPriceActivation.LineDiscountCustomerGroupAllItems = ((byte)dr["LINEDISCOUNTCUSTOMERGROUPALLITEMS"] == 1);
            discountAndPriceActivation.LineDiscountAllCustomersAllItems = ((byte)dr["LINEDISCOUNTALLCUSTOMERSALLITEMS"] == 1);

            discountAndPriceActivation.MultilineDiscountCustomerItemGroup = ((byte)dr["MULTILINEDISCOUNTCUSTOMERITEMGROUP"] == 1);
            discountAndPriceActivation.MultilineDiscountCustomerGroupItemGroup = ((byte)dr["MULTILINEDISCOUNTCUSTOMERGROUPITEMGROUP"] == 1);
            discountAndPriceActivation.MultilineDiscountAllCustomersItemGroup = ((byte)dr["MULTILINEDISCOUNTALLCUSTOMERSITEMGROUP"] == 1);
            discountAndPriceActivation.MultilineDiscountCustomerAllItems = ((byte)dr["LINEDISCOUNTCUSTOMERALLITEMS"] == 1);
            discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems = ((byte)dr["MULTILINEDISCOUNTCUSTOMERGROUPALLITEMS"] == 1);
            discountAndPriceActivation.MultilineDiscountAllCustomersAllItems = ((byte)dr["MULTILINEDISCOUNTALLCUSTOMERSALLITEMS"] == 1);

            discountAndPriceActivation.TotalDiscountCustomerAllItems = ((byte)dr["TOTALDISCOUNTCUSTOMERALLITEMS"] == 1);
            discountAndPriceActivation.TotalDiscountCustomerGroupAllItems = ((byte)dr["TOTALDISCOUNTCUSTOMERGROUPALLITEMS"] == 1);
            discountAndPriceActivation.TotalDiscountAllCustomersAllItems = ((byte)dr["TOTALDISCOUNTALLCUSTOMERSALLITEMS"] == 1);
        }

        public static DataEntities.DiscountAndPriceActivation Get(IConnectionManager entry)
        {
            List<DataEntities.DiscountAndPriceActivation> result;

            ValidateSecurity(entry);

            SqlCommand cmd;

            using (cmd = new SqlCommand())
            {
                cmd.CommandText =
                    "Select " + 
                    "ISNULL(SALESPRICEACCOUNTITEM,0) as PRICECUSTOMERITEM," +
                    "ISNULL(SALESPRICEGROUPITEM,0) as PRICECUSTOMERGROUPITEM, " +
                    "ISNULL(SALESPRICEALLITEM,0) as PRICEALLCUSTOMERSITEM," +
                    "" + 
                    "ISNULL(SALESLINEACCOUNTITEM,0) as LINEDISCOUNTCUSTOMERITEM," +
                    "ISNULL(SALESLINEGROUPITEM,0) as LINEDISCOUNTCUSTOMERGROUPITEM, " +
                    "ISNULL(SALESLINEALLITEM,0) as LINEDISCOUNTALLCUSTOMERSITEM," +
                    "ISNULL(SALESLINEACCOUNTGROUP,0) as LINEDISCOUNTCUSTOMERITEMGROUP," +
                    "ISNULL(SALESLINEGROUPGROUP,0) as LINEDISCOUNTCUSTOMERGROUPITEMGROUP, " +
                    "ISNULL(SALESLINEALLGROUP,0) as LINEDISCOUNTALLCUSTOMERSITEMGROUP," +
                    "ISNULL(SALESLINEACCOUNTALL,0) as LINEDISCOUNTCUSTOMERALLITEMS," +
                    "ISNULL(SALESLINEGROUPALL,0) as LINEDISCOUNTCUSTOMERGROUPALLITEMS, " +
                    "ISNULL(SALESLINEALLALL,0) as LINEDISCOUNTALLCUSTOMERSALLITEMS," +                    
                    "" +
                    "ISNULL(SALESMULTILNACCOUNTGROUP,0) as MULTILINEDISCOUNTCUSTOMERITEMGROUP," +
                    "ISNULL(SALESMULTILNGROUPGROUP,0) as MULTILINEDISCOUNTCUSTOMERGROUPITEMGROUP, " +
                    "ISNULL(SALESMULTILNALLGROUP,0) as MULTILINEDISCOUNTALLCUSTOMERSITEMGROUP," +
                    "ISNULL(SALESMULTILNACCOUNTALL,0) as MULTILINEDISCOUNTCUSTOMERALLITEMS," +
                    "ISNULL(SALESMULTILNGROUPALL,0) as MULTILINEDISCOUNTCUSTOMERGROUPALLITEMS, " +
                    "ISNULL(SALESMULTILNALLALL,0) as MULTILINEDISCOUNTALLCUSTOMERSALLITEMS," + 
                    "" +
                    "ISNULL(SALESENDACCOUNTALL,0) as TOTALDISCOUNTCUSTOMERALLITEMS," +
                    "ISNULL(SALESENDGROUPALL,0) as TOTALDISCOUNTCUSTOMERGROUPALLITEMS, " +
                    "ISNULL(SALESENDALLALL,0) as TOTALDISCOUNTALLCUSTOMERSALLITEMS " + 
                    "from PRICEPARAMETERS " +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<DataEntities.DiscountAndPriceActivation>(entry, cmd, CommandType.Text, PopulateDiscountAndPriceActivation);
            }

            return (result.Count > 0) ? result[0] : new DiscountAndPriceActivation();
        }

        public static bool Exists(IConnectionManager entry)
        {
            return RecordExists(entry, "PRICEPARAMETERS", "KEY_", 1);
        }

        public static void Save(IConnectionManager entry, DataEntities.DiscountAndPriceActivation discountAndPriceActivation)
        {
            Statement statement = new Statement("PRICEPARAMETERS");

            if (!Exists(entry))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("KEY_", 1, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("KEY_", 1, SqlDbType.Int);
            }

            statement.AddField("SALESPRICEACCOUNTITEM", discountAndPriceActivation.PriceCustomerItem, SqlDbType.TinyInt);
            statement.AddField("SALESPRICEGROUPITEM", discountAndPriceActivation.PriceCustomerGroupItem, SqlDbType.TinyInt);
            statement.AddField("SALESPRICEALLITEM", discountAndPriceActivation.PriceAllCustomersItem, SqlDbType.TinyInt);

            statement.AddField("SALESLINEACCOUNTITEM", discountAndPriceActivation.LineDiscountCustomerItem, SqlDbType.TinyInt);
            statement.AddField("SALESLINEGROUPITEM", discountAndPriceActivation.LineDiscountCustomerGroupItem, SqlDbType.TinyInt);
            statement.AddField("SALESLINEALLITEM", discountAndPriceActivation.LineDiscountAllCustomersItem, SqlDbType.TinyInt);
            statement.AddField("SALESLINEACCOUNTGROUP", discountAndPriceActivation.LineDiscountCustomerItemGroup, SqlDbType.TinyInt);
            statement.AddField("SALESLINEGROUPGROUP", discountAndPriceActivation.LineDiscountCustomerGroupItemGroup, SqlDbType.TinyInt);
            statement.AddField("SALESLINEALLGROUP", discountAndPriceActivation.LineDiscountAllCustomersItemGroup, SqlDbType.TinyInt);
            statement.AddField("SALESLINEACCOUNTALL", discountAndPriceActivation.LineDiscountCustomerAllItems, SqlDbType.TinyInt);
            statement.AddField("SALESLINEGROUPALL", discountAndPriceActivation.LineDiscountCustomerGroupAllItems, SqlDbType.TinyInt);
            statement.AddField("SALESLINEALLALL", discountAndPriceActivation.LineDiscountAllCustomersAllItems, SqlDbType.TinyInt);

            statement.AddField("SALESMULTILNACCOUNTGROUP", discountAndPriceActivation.MultilineDiscountCustomerItemGroup, SqlDbType.TinyInt);
            statement.AddField("SALESMULTILNGROUPGROUP", discountAndPriceActivation.MultilineDiscountCustomerGroupItemGroup, SqlDbType.TinyInt);
            statement.AddField("SALESMULTILNALLGROUP", discountAndPriceActivation.MultilineDiscountAllCustomersItemGroup, SqlDbType.TinyInt);
            statement.AddField("SALESMULTILNACCOUNTALL", discountAndPriceActivation.MultilineDiscountCustomerAllItems, SqlDbType.TinyInt);
            statement.AddField("SALESMULTILNGROUPALL", discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems, SqlDbType.TinyInt);
            statement.AddField("SALESMULTILNALLALL", discountAndPriceActivation.MultilineDiscountAllCustomersAllItems, SqlDbType.TinyInt);

            statement.AddField("SALESENDACCOUNTALL", discountAndPriceActivation.TotalDiscountCustomerAllItems, SqlDbType.TinyInt);
            statement.AddField("SALESENDGROUPALL", discountAndPriceActivation.TotalDiscountCustomerGroupAllItems, SqlDbType.TinyInt);
            statement.AddField("SALESENDALLALL", discountAndPriceActivation.TotalDiscountAllCustomersAllItems, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);

        }
    }
}



