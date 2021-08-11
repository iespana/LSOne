using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Helper.Import;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LSOne.ViewPlugins.RMSMigration.Model.Enums;

namespace LSOne.ViewPlugins.RMSMigration.Helper
{
    public static class ConvertionExtensions
    {
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                PropertyInfo[] properties = typeof(T).GetProperties();
                List<string> propertyNames = properties.Select(el => el.Name).ToList();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (DataColumn dc in table.Columns)
                    {
                        if (!propertyNames.Contains(dc.ColumnName))
                        {
                            continue;
                        }
                        try
                        {
                            PropertyInfo propertyInfo = properties.Where(p => p.Name == dc.ColumnName).FirstOrDefault();
                            object val = propertyInfo.PropertyType == typeof(RecordIdentifier) ? (RecordIdentifier)(row[dc.ColumnName].ToString()) : row[dc.ColumnName].ChangeType(propertyInfo.PropertyType);
                            propertyInfo.SetValue(obj, val, null);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static List<RMSItem> ToList(this DataTable table)
        {
            List<RMSItem> list = new List<RMSItem>();

            foreach (DataRow dr in table.Rows)
            {
                RMSItem ri = new RMSItem();

                if (table.Columns.Contains("RMS_ID"))
                {
                    ri.RMS_ID = dr["RMS_ID"].ToString().ToInt();
                }
                if (table.Columns.Contains("RMS_MasterItemID"))
                {
                    ri.RMS_MasterItemID = dr["RMS_MasterItemID"].ToString().ToInt();
                }
                if (table.Columns.Contains("ItemNumber"))
                {
                    ri.ItemNumber = dr["ItemNumber"].ToString();
                }
                if (table.Columns.Contains("VariantCode"))
                {
                    ri.VariantCode = dr["VariantCode"].ToString();
                }
                if (table.Columns.Contains("Text"))
                {
                    ri.Text = dr["Text"].ToString();
                }
                if (table.Columns.Contains("VariantName"))
                {
                    ri.VariantName = dr["VariantName"].ToString();
                }
                if (table.Columns.Contains("ExtendedDescription"))
                {
                    ri.ExtendedDescription = dr["ExtendedDescription"].ToString();
                }
                if (table.Columns.Contains("InventoryUnitOfMeasure"))
                {
                    ri.InventoryUnitOfMeasure = dr["InventoryUnitOfMeasure"].ToString();
                }
                if (table.Columns.Contains("SalesUnitOfMeasure"))
                {
                    ri.SalesUnitOfMeasure = dr["SalesUnitOfMeasure"].ToString();
                }
                if (table.Columns.Contains("RMS_RetailGroupID"))
                {
                    ri.RMS_RetailGroupID = dr["RMS_RetailGroupID"].ToString().ToInt();
                }
                if (table.Columns.Contains("RMS_DepartmentID"))
                {
                    ri.RMS_DepartmentID = dr["RMS_DepartmentID"].ToString().ToInt();
                }
                if (table.Columns.Contains("RMS_SalesTaxItemGroupID"))
                {
                    ri.RMS_SalesTaxItemGroupID = dr["RMS_SalesTaxItemGroupID"].ToString().ToNullableInt();
                }
                if (table.Columns.Contains("RMS_CustomerID"))
                {
                    ri.RMS_CustomerID = dr["RMS_CustomerID"].ToString().ToNullableInt();
                }
                if (table.Columns.Contains("Barcode"))
                {
                    ri.Barcode = dr["Barcode"].ToString();
                }
                if (table.Columns.Contains("PurchasePrice"))
                {
                    ri.PurchasePrice = dr["PurchasePrice"].ToString().ToDecimal();
                }
                if (table.Columns.Contains("SalesPrice"))
                {
                    ri.SalesPrice = dr["SalesPrice"].ToString().ToDecimal();
                }
                if (table.Columns.Contains("Dimension1"))
                {
                    ri.Dimension1 = dr["Dimension1"].ToString();
                }
                if (table.Columns.Contains("Dimension2"))
                {
                    ri.Dimension2 = dr["Dimension2"].ToString();
                }
                if (table.Columns.Contains("Dimension3"))
                {
                    ri.Dimension3 = dr["Dimension3"].ToString();
                }
                if (table.Columns.Contains("DimensionAttribute1"))
                {
                    ri.DimensionAttribute1 = dr["DimensionAttribute1"].ToString();
                }
                if (table.Columns.Contains("DimensionAttribute2"))
                {
                    ri.DimensionAttribute2 = dr["DimensionAttribute2"].ToString();
                }
                if (table.Columns.Contains("DimensionAttribute3"))
                {
                    ri.DimensionAttribute3 = dr["DimensionAttribute3"].ToString();
                }
                if (table.Columns.Contains("PictureName"))
                {
                    ri.PictureName = dr["PictureName"].ToString();
                }
                if (table.Columns.Contains("TaxSystem"))
                {
                    ri.TaxSystem = dr["TaxSystem"].ToString().ToInt();
                }
                if (table.Columns.Contains("PriceMustBeEntered"))
                {
                    ri.KeyInPrice = ((bool)dr["PriceMustBeEntered"]) ? DataLayer.BusinessObjects.ItemMaster.RetailItem.KeyInPriceEnum.MustKeyInNewPrice : DataLayer.BusinessObjects.ItemMaster.RetailItem.KeyInPriceEnum.NotMandatory;
                }

                if (table.Columns.Contains("ItemNotDiscountable") && dr["ItemNotDiscountable"] != null)
                {
                    ri.NoDiscountAllowed = ((bool)dr["ItemNotDiscountable"]);
                }
                if (table.Columns.Contains("QuantityEntryNotAllowed") && dr["QuantityEntryNotAllowed"] != null)
                {
                    ri.KeyInQuantity = ((bool)dr["QuantityEntryNotAllowed"]) ? DataLayer.BusinessObjects.ItemMaster.RetailItem.KeyInQuantityEnum.MustNotKeyInQuantity : DataLayer.BusinessObjects.ItemMaster.RetailItem.KeyInQuantityEnum.NotMandatory;
                }
                if (table.Columns.Contains("BlockSalesType") && dr["BlockSalesType"] != null && dr["BlockSalesType"].ToString().ToInt() == 1)
                {
                    if (table.Columns.Contains("BlockSalesAfterDate") && dr["BlockSalesAfterDate"] != null && dr["BlockSalesAfterDate"] != DBNull.Value)
                    {
                        ri.DateToBeBlocked = new Date(((DateTime)dr["BlockSalesAfterDate"]));
                    }
                    if (table.Columns.Contains("BlockSalesBeforeDate") && dr["BlockSalesBeforeDate"] != null && dr["BlockSalesBeforeDate"] != DBNull.Value)
                    {
                        ri.DateToActivateItem = new Date(((DateTime)dr["BlockSalesBeforeDate"]));
                    }
                }
                if (table.Columns.Contains("ReorderPoint"))
                {
                    ri.ReorderPoint = dr["ReorderPoint"].ToString().ToInt();
                }
                if (table.Columns.Contains("RestockLevel"))
                {
                    ri.RestockLevel = dr["RestockLevel"].ToString().ToInt();
                }
                if (table.Columns.Contains("TagAlongItem"))
                {
                    ri.TagAlongItem = dr["TagAlongItem"].ToString().ToInt();
                }
                if (table.Columns.Contains("TagAlongQuantity"))
                {
                    ri.TagAlongQuantity = dr["TagAlongQuantity"].ToString().ToDecimal();
                }
                list.Add(ri);
            }

            return list;
        }

        public static object ChangeType(this object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            try
            {
                var obj = Convert.ChangeType(value, conversionType);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Loads a data table using a datareader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IDataReader dataReader)
        {
            DataTable dt = new DataTable();
            dt.Load(dataReader);

            return dt;
        }

        /// <summary>
        /// Converts string to decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            decimal result;
            decimal.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Converts string to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Converts string to nullable int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? ToNullableInt(this string value)
        {
            int result;
            bool valid = int.TryParse(value, out result);
            if (valid)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static Dictionary<int, RecordIdentifier> ToDictionary<T>(this List<T> list, string rmsIdPropertyName, string lsOneIdPropertyName)
        {
            Dictionary<int, RecordIdentifier> dic = new Dictionary<int, RecordIdentifier>();

            if (list == null || list.Count == 0)
            {
                return dic;
            }

            list.ForEach(obj =>
            {
                PropertyInfo rmsPropertyInfo = obj.GetType().GetProperty(rmsIdPropertyName);
                PropertyInfo lsPropertyInfo = obj.GetType().GetProperty(lsOneIdPropertyName);
                if (rmsPropertyInfo != null && lsPropertyInfo != null)
                {
                    int rmsID = (int)rmsPropertyInfo.GetValue(obj, null);
                    RecordIdentifier lsId = (RecordIdentifier)lsPropertyInfo.GetValue(obj, null);
                    if (!dic.ContainsKey(rmsID))
                    {
                        dic.Add(rmsID, lsId);
                    }
                }
            });

            return dic;
        }

        public static IImportManager ResolveImporter(this RMSMigrationItemType type)
        {
            switch (type)
            {
                case RMSMigrationItemType.Currency:
                    return new CurrencyImport();
                case RMSMigrationItemType.SaleTax:
                    return new SalesTaxImport();
                case RMSMigrationItemType.Customer:
                    return new CustomerImport();
                case RMSMigrationItemType.Item:
                    return new DataItemsImport();
                case RMSMigrationItemType.Vendor:
                    return new VendorImport();
                case RMSMigrationItemType.User:
                    return new UserImport();
                case RMSMigrationItemType.OpenPurchaseOrder:
                    return new PurchaseOrderImport();
                case RMSMigrationItemType.Transaction:
                    return new TransactionImport();
                default:
                    return new DefaultImport();
            }
        }

        public static ITenderLineItem CreateTender(this TenderTypeEnum tenderType)
        {
            switch (tenderType)
            {
                case TenderTypeEnum.None:
                    return new TenderLineItem();
                case TenderTypeEnum.CardTender:
                    return new CardTenderLineItem();
                case TenderTypeEnum.ChequeTender:
                    return new ChequeTenderLineItem();
                case TenderTypeEnum.CorporateCardTender:
                    return new CorporateCardTenderLineItem();
                case TenderTypeEnum.CouponTender:
                    return new CouponTenderLineItem();
                case TenderTypeEnum.CreditMemoTender:
                    return new CreditMemoTenderLineItem();
                case TenderTypeEnum.CustomerTender:
                    return new CustomerTenderLineItem();
                case TenderTypeEnum.DepositTender:
                    return new DepositTenderLineItem();
                case TenderTypeEnum.GiftCertificateTender:
                    return new GiftCertificateTenderLineItem();
                case TenderTypeEnum.LoyaltyTender:
                    return new LoyaltyTenderLineItem();
                case TenderTypeEnum.TenderLine:
                    return new TenderLineItem();
                default:
                    return new TenderLineItem();
            }
        }
    }
}
