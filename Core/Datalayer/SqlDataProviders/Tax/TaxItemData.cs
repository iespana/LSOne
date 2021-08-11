using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlDataProviders.Customers;
using LSOne.DataLayer.SqlDataProviders.Hospitality;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Tax
{
    public class TaxItemData : SqlServerDataProviderBase, ITaxItemData
    {
        public Dictionary<RecordIdentifier, string> GetTaxGroupDictionary(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"Select g.TAXGROUP,ISNULL(g.TAXGROUPNAME,'') as TAXGROUPNAME from TAXGROUPHEADING g";

                var result = new Dictionary<RecordIdentifier, string>();

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    while (dr.Read())
                    {
                        result.Add((RecordIdentifier)(string)dr["TAXGROUP"], (string)dr["TAXGROUPNAME"]);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
                return result;
            }
        }
        public Dictionary<RecordIdentifier, string> GetItemTaxGroupDictionary(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"Select g.TAXITEMGROUP,ISNULL(g.NAME,'') as TAXGROUPNAME from TAXITEMGROUPHEADING g";

                var result = new Dictionary<RecordIdentifier, string>();

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    while (dr.Read())
                    {
                        result.Add((RecordIdentifier)(string)dr["TAXITEMGROUP"], (string)dr["TAXGROUPNAME"]);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
                return result;
            }
        }

        private static void PopulateTaxItem(IDataReader dr, TaxItem ti)
        {
            ti.StoreTaxGroup = (string)dr["TAXGROUP"];
            ti.ItemTaxGroup = (string)dr["TAXITEMGROUP"];
            ti.TaxCode = (string)dr["TAXCODE"];
            ti.Percentage = (decimal)dr["TAXVALUE"];
            ti.TaxRoundOff = (decimal)dr["TAXROUNDOFF"];
            ti.TaxRoundOffType = (int)dr["TAXROUNDOFFTYPE"];
            ti.TaxCodeDisplay = (string)dr["PRINTCODE"];
            ti.ItemTaxGroupDisplay = (string)dr["RECEIPTDISPLAY"];
        }

        public virtual List<TaxItem> GetTaxRate(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier customerID, RecordIdentifier salesTypeID, UseTaxGroupFromEnum useTaxGroupFrom, bool useOverrideTaxGroup, RecordIdentifier overrideTaxGroup,CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (cacheType != CacheType.CacheTypeNone)
            {
                // The combined key here bellow is just a hash key
                var bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), "Tax" + (string)itemID + ":" + (string)customerID + ":" + useTaxGroupFrom + ":" + useOverrideTaxGroup + ":" + (string)overrideTaxGroup + ":"  + salesTypeID);
                
                if (bucket != null)
                {
                    return (List<TaxItem>)bucket.BucketData;
                }
            }

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT DISTINCT TAXONITEM.TAXITEMGROUP, TAXONITEM.TAXCODE, TAXDATA.TAXVALUE, ISNULL(TAXTABLE.TAXROUNDOFF, 0) AS TAXROUNDOFF, " +
                                  "ISNULL(TAXTABLE.TAXROUNDOFFTYPE, 0) AS TAXROUNDOFFTYPE, COALESCE(TAXTABLE.PRINTCODE, '') PRINTCODE, COALESCE(TAXITEMGROUPHEADING.RECEIPTDISPLAY, '') RECEIPTDISPLAY, " +
                                  "ISNULL(TAXGROUPHEADING.TAXGROUP, '') TAXGROUP " +
                                  "FROM TAXGROUPHEADING  ";

                if (!useOverrideTaxGroup)
                {
                    bool customerExists = false;
                    bool salesTypeExists = false;
                    UseTaxGroupFromEnum useTaxGroupFromLocal = useTaxGroupFrom;

                    if (useTaxGroupFromLocal == UseTaxGroupFromEnum.Customer)
                    {
                        if (RecordIdentifier.IsEmptyOrNull(customerID))
                        {
                            customerExists = false;
                        }
                        else
                        {
                            // Check if the customer actually exists 
                            Customer existingCustomer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal);
                            customerExists = existingCustomer != null;

                            //If there is no tax group on the customer, revert back to the store tax
                            if(existingCustomer.TaxExempt == TaxExemptEnum.No && RecordIdentifier.IsEmptyOrNull(existingCustomer.TaxGroup))
                            {
                                useTaxGroupFromLocal = UseTaxGroupFromEnum.Store;
                            }
                        }
                    }
                    else if (useTaxGroupFromLocal == UseTaxGroupFromEnum.SalesType)
                    {
                        if (salesTypeID == null || salesTypeID == "" || salesTypeID == RecordIdentifier.Empty)
                        {
                            salesTypeExists = false;
                        }
                        else
                        {
                            salesTypeExists = Providers.SalesTypeData.Exists(entry, salesTypeID);
                        }
                    }

                    if (customerExists && useTaxGroupFromLocal == UseTaxGroupFromEnum.Customer)
                    {
                        cmd.CommandText += "INNER JOIN CUSTOMER ON  " +
                                            "TAXGROUPHEADING.DATAAREAID = CUSTOMER.DATAAREAID  " +
                                            "AND TAXGROUPHEADING.TAXGROUP = CUSTOMER.TAXGROUP " +
                                            "AND CUSTOMER.ACCOUNTNUM = @CUSTID ";

                        MakeParam(cmd, "CUSTID", (string)customerID);
                    }
                    else if (salesTypeExists && useTaxGroupFromLocal == UseTaxGroupFromEnum.SalesType)
                    {
                        cmd.CommandText += "INNER JOIN SALESTYPE ON " +
                                           "TAXGROUPHEADING.DATAAREAID = SALESTYPE.DATAAREAID  " +
                                           "AND TAXGROUPHEADING.TAXGROUP = SALESTYPE.TAXGROUPID " +
                                           "AND SALESTYPE.CODE = @SALESTYPE ";

                        MakeParam(cmd, "SALESTYPE", (string)salesTypeID);
                    }
                    else
                    {
                        cmd.CommandText += "INNER JOIN RBOSTORETABLE ON " +
                                            "TAXGROUPHEADING.DATAAREAID = RBOSTORETABLE.DATAAREAID  " +
                                            "AND RBOSTORETABLE.TAXGROUP = TAXGROUPHEADING.TAXGROUP  " +
                                            "AND dbo.RBOSTORETABLE.STOREID = @STOREID ";

                        MakeParam(cmd, "STOREID", (string)entry.CurrentStoreID);
                    }
                }
               
                cmd.CommandText += "INNER JOIN TAXGROUPDATA ON  " +
                                    "TAXGROUPHEADING.DATAAREAID = TAXGROUPDATA.DATAAREAID  " +
                                    "AND TAXGROUPHEADING.TAXGROUP = TAXGROUPDATA.TAXGROUP  " +

                                    "INNER JOIN TAXONITEM ON  " +
                                    "TAXGROUPDATA.DATAAREAID = TAXONITEM.DATAAREAID  " +
                                    "AND  TAXGROUPDATA.TAXCODE = TAXONITEM.TAXCODE  " +

                                    "INNER JOIN TAXITEMGROUPHEADING ON " +
                                    "TAXONITEM.TAXITEMGROUP = TAXITEMGROUPHEADING.TAXITEMGROUP " +
                                    "AND TAXITEMGROUPHEADING.DATAAREAID = TAXGROUPHEADING.DATAAREAID " +

                                    "INNER JOIN RETAILITEM ON  " +
                                    "RETAILITEM.SALESTAXITEMGROUPID = TAXONITEM.TAXITEMGROUP  " +

                                    "INNER JOIN TAXDATA ON  " +
                                    "TAXONITEM.DATAAREAID = TAXDATA.DATAAREAID  " +
                                    "AND TAXONITEM.TAXCODE = TAXDATA.TAXCODE " +

                                    "LEFT JOIN TAXTABLE ON TAXONITEM.TAXCODE = TAXTABLE.TAXCODE AND TAXONITEM.DATAAREAID = TAXTABLE.DATAAREAID " +

                                    "WHERE  (TAXGROUPHEADING.DATAAREAID = @DATAAREAID) " +
                                    "AND (RETAILITEM.ITEMID = @ITEMID) " +
                                    "AND ((CONVERT(DATE,GETDATE()) >= TAXDATA.TAXFROMDATE OR TAXDATA.TAXFROMDATE < '01.01.1901' ) AND (CONVERT(DATE,GETDATE()) <= TAXDATA.TAXTODATE OR TAXDATA.TAXTODATE < '01.01.1901')) ";


                if (useOverrideTaxGroup)
                {
                    cmd.CommandText += "AND TAXGROUPHEADING.TAXGROUP = @TAXGROUPID";

                    MakeParam(cmd, "TAXGROUPID", (string)overrideTaxGroup);
                }

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ITEMID", (string)itemID);


                var result = Execute<TaxItem>(entry, cmd, CommandType.Text, PopulateTaxItem);

                if (cacheType != CacheType.CacheTypeNone)
                {
                    var bucket = new CacheBucket {BucketData = result};

                    // The combined key here bellow is just a hash key
                    entry.Cache.AddEntityToCache("Tax" + (string)itemID + ":" + (string)customerID + ":" + useTaxGroupFrom.ToString() + ":" + useOverrideTaxGroup.ToString() + ":" + (string)overrideTaxGroup + ":" + salesTypeID, bucket, cacheType);
                }

                return result;
            }
        }
    }
}
