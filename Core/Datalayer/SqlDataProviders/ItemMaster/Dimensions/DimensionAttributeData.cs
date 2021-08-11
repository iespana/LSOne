using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster.Dimensions
{
    public class DimensionAttributeData : SqlServerDataProviderBase, IDimensionAttributeData
    {
        private static void PopulateDimensionAttribute(IDataReader dr, DimensionAttribute entity)
        {
            entity.ID = (Guid)dr["ID"];
            entity.DimensionID = (Guid)dr["DIMENSIONID"];
            entity.Text = (string)dr["DESCRIPTION"];
            entity.Code = (string)dr["CODE"];
            entity.Sequence = (int)dr["SEQUENCE"];
        }

        public virtual DimensionAttribute Get(IConnectionManager entry, RecordIdentifier attributeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"select ID, 
                                           DIMENSIONID, 
                                           DESCRIPTION, 
                                           CODE, 
                                           SEQUENCE 
                                    from DIMENSIONATTRIBUTE 
                                    where ID = @id and DELETED = 0";

                MakeParam(cmd, "id", (Guid)attributeID);

                List<DimensionAttribute> dimensionAttributes = Execute<DimensionAttribute>(entry, cmd, CommandType.Text, PopulateDimensionAttribute);

                return dimensionAttributes.Count > 0 ? dimensionAttributes[0] : null;
            }
        }

        public virtual List<DimensionAttribute> GetListForRetailItemDimension(IConnectionManager entry, RecordIdentifier retailItemDimensionID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            return GetListForParentObject(entry, retailItemDimensionID);
        }

        public virtual List<DimensionAttribute> GetListForDimensionTemplate(IConnectionManager entry, RecordIdentifier dimensionTemplateID)
        {
            return GetListForParentObject(entry, dimensionTemplateID);
        }

        protected virtual List<DimensionAttribute> GetListForParentObject(IConnectionManager entry, RecordIdentifier parentObjectID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (cacheType != CacheType.CacheTypeNone)
            {
                var bucket = (CacheBucket) entry.Cache.GetEntityFromCache(typeof (CacheBucket), "DimensionAttributes" + (string) parentObjectID);

                if (bucket != null)
                {
                    return (List<DimensionAttribute>) bucket.BucketData;
                }
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select ID, DIMENSIONID, DESCRIPTION, CODE, SEQUENCE from DIMENSIONATTRIBUTE where DIMENSIONID = @id and DELETED = 0 order by SEQUENCE";

                MakeParam(cmd, "id", (Guid)parentObjectID);

                List<DimensionAttribute> results = Execute<DimensionAttribute>(entry, cmd, CommandType.Text, PopulateDimensionAttribute);

                if (cacheType != CacheType.CacheTypeNone)
                {
                    var bucket = new CacheBucket("DimensionAttributes" + (string) parentObjectID, "", results);
                    entry.Cache.AddEntityToCache(bucket.ID, bucket, cacheType);
                }

                return results;                
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier templateID)
        {
            if (entry.HasPermission(BusinessObjects.Permission.ColorSizeStyleEdit) || entry.HasPermission(BusinessObjects.Permission.ItemsEdit))
            {
                MarkAsDeleted(entry, "DIMENSIONATTRIBUTE", "ID", templateID, "");
            }
            else
            {
                throw new PermissionException(BusinessObjects.Permission.ColorSizeStyleEdit);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier attributeID)
        {
            return RecordExists(entry, "DIMENSIONATTRIBUTE", "ID", attributeID,false);
        }

        public virtual void Save(IConnectionManager entry, DimensionAttribute attribute)
        {
            bool isNew = false;

            SqlServerStatement statement = new SqlServerStatement("DIMENSIONATTRIBUTE");

            if (!entry.HasPermission(BusinessObjects.Permission.ManagePurchaseOrders))
            {
                ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

                if (!(entry.HasPermission(BusinessObjects.Permission.ColorSizeStyleEdit) || entry.HasPermission(BusinessObjects.Permission.ItemsEdit)))
                {
                    throw new PermissionException(BusinessObjects.Permission.ColorSizeStyleEdit);
                }
            }

            attribute.Validate();

            if (attribute.ID.IsEmpty)
            {
                attribute.ID = Guid.NewGuid(); // We generate the Guid here even if the SQL server can and does do it, we do this so we dont need extra execute scalar, to grab the Guid.
                isNew = true;
            }

            if (isNew || !Exists(entry,attribute.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (Guid)attribute.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (Guid)attribute.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DIMENSIONID", (Guid)attribute.DimensionID, SqlDbType.UniqueIdentifier);
            statement.AddField("DESCRIPTION", attribute.Text);
            statement.AddField("CODE", attribute.Code);
            statement.AddField("SEQUENCE", (int)attribute.Sequence, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns a list of all dimension attributes that are being used by the variant items that belong to the given master item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="masterItemID">The master ID for the master item</param>
        /// <returns></returns>
        public virtual List<DimensionAttribute> GetAttributesInUseByItem(IConnectionManager entry, RecordIdentifier masterItemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    @"
                      SELECT  A.ID, 
                              A.DIMENSIONID,
                              A.DESCRIPTION,
                              A.CODE, 
                              A.SEQUENCE 
                      FROM DIMENSIONATTRIBUTE A
                      WHERE A.ID IN
                      (
	                      SELECT DISTINCT DIMENSIONATTRIBUTEID
	                      FROM RETAILITEMDIMENSIONATTRIBUTE
	                      WHERE RETAILITEMID IN
	                      (
		                      SELECT MASTERID 
		                      FROM RETAILITEM 
		                      WHERE HEADERITEMID = @MASTERITEMID
                              AND DELETED = 0
	                      )
                      )
                      AND A.DELETED = 0
                      ORDER BY A.DIMENSIONID, A.SEQUENCE";

                MakeParam(cmd, "MASTERITEMID", (Guid)masterItemID, SqlDbType.UniqueIdentifier);

                return Execute<DimensionAttribute>(entry, cmd, CommandType.Text, PopulateDimensionAttribute);
            }
        }

        /// <summary>
        /// Returns a dictionary that includes all item-attribute relations for items that belong to the given header item ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerItemID">The ID of the header item</param>
        /// <param name="showDeleted">Determines if provider returns deleted items</param>
        /// <param name="excludeItemswithSerialNumber"> Should we get variants with serial numbers yused by omni server</param>
        /// <returns></returns>
        public virtual Dictionary<RecordIdentifier, List<DimensionAttribute>> GetRetailItemDimensionAttributeRelations(IConnectionManager entry,
            RecordIdentifier headerItemID, bool showDeleted = true,bool excludeItemswithSerialNumber = false)
        {
            Dictionary<RecordIdentifier, List<DimensionAttribute>> attributesForHeaderItem = new Dictionary<RecordIdentifier, List<DimensionAttribute>>();

            List<Condition> conditions = new List<Condition>();
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();

                columns.Add(new TableColumn
                {
                    ColumnName = "rda.RETAILITEMID",
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ri.ITEMID"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "da.ID"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "da.DIMENSIONID"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "da.DESCRIPTION"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "da.CODE"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "da.SEQUENCE"
                });

                List<Join> joins = new List<Join>();

                joins.Add(new Join
                {
                    Table = "DIMENSIONATTRIBUTE",
                    Condition = "da.ID = rda.DIMENSIONATTRIBUTEID",
                    JoinType = "",
                    TableAlias = "da"
                });
                if (showDeleted)
                {
                    joins.Add(new Join
                    {
                        Table = "RETAILITEM",
                        Condition = "ri.MASTERID = rda.RETAILITEMID and ri.HEADERITEMID = @itemID",
                        JoinType = "",
                        TableAlias = "ri"
                    });
                }
                else
                {
                    joins.Add(new Join
                    {
                        Table = "RETAILITEM",
                        Condition = "ri.MASTERID = rda.RETAILITEMID and ri.DELETED = 0 and ri.HEADERITEMID = @itemID",
                        JoinType = "",
                        TableAlias = "ri"
                    });
                }

                joins.Add(new Join
                {
                    Table = "RETAILITEMDIMENSION",
                    Condition = "da.DIMENSIONID = di.ID",
                    JoinType = "",
                    TableAlias = "di"
                });
                if (excludeItemswithSerialNumber)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = " ri.KEYINSERIALNUMBER = 0 "
                    });

                }
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMDIMENSIONATTRIBUTE", "rda"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "order by ri.ITEMID, di.SEQUENCE, da.SEQUENCE");


                MakeParam(cmd, "itemID", headerItemID);

                int count = 0;
                using (var reader = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    while (reader.Read())
                    {
                        RecordIdentifier itemID = (Guid) reader["RETAILITEMID"];
                        itemID.SecondaryID = (string) reader["ITEMID"];

                        DimensionAttribute attribute = new DimensionAttribute();
                        PopulateDimensionAttribute(reader, attribute);

                        if (attributesForHeaderItem.ContainsKey(itemID))
                        {
                            attributesForHeaderItem[itemID].Add(attribute);
                        }
                        else
                        {
                            attributesForHeaderItem[itemID] = new List<DimensionAttribute>() {attribute};
                        }
                        count++;
                    }
                }
            }

            return attributesForHeaderItem;
        }

        /// <summary>
        /// Returns a dictionary that includes all dimensions and attributes that belong to the given header item ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerItemID">The ID of the header item</param>
        /// <param name="showDeleted">Determens if we also get the deleted attributes</param>
        /// <returns></returns>
        public virtual Dictionary<RecordIdentifier, List<DimensionAttribute>> GetHeaderItemDimensionsAndAttributes(IConnectionManager entry, RecordIdentifier headerItemID, bool showDeleted = false)
        {
            Dictionary<RecordIdentifier, List<DimensionAttribute>> attributesForHeaderItem = new Dictionary<RecordIdentifier, List<DimensionAttribute>>();

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();

                columns.Add(new TableColumn { ColumnName = "RETAILITEMID", TableAlias = "RDA" });
                columns.Add(new TableColumn { ColumnName = "ID", TableAlias = "DA" });
                columns.Add(new TableColumn { ColumnName = "DIMENSIONID", TableAlias = "DA" });
                columns.Add(new TableColumn { ColumnName = "DESCRIPTION", TableAlias = "DA" });
                columns.Add(new TableColumn { ColumnName = "CODE", TableAlias = "DA" });
                columns.Add(new TableColumn { ColumnName = "SEQUENCE", TableAlias = "RDA" });
                columns.Add(new TableColumn { ColumnName = "SEQUENCE", TableAlias = "DA" });

                Join join = new Join { Condition = "DA.DIMENSIONID = RDA.ID", Table = "DIMENSIONATTRIBUTE", TableAlias = "DA" };

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "RDA.RETAILITEMID = @ITEMID" });
                MakeParam(cmd, "ITEMID", headerItemID);

                if (!showDeleted)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "DA.DELETED = 0" });
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEMDIMENSION", "RDA"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    join,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY RDA.SEQUENCE, DA.SEQUENCE"
                    );

                int count = 0;
                using (var reader = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    while (reader.Read())
                    {
                        RecordIdentifier dimensionID = (Guid)reader["DIMENSIONID"];

                        DimensionAttribute attribute = new DimensionAttribute();
                        PopulateDimensionAttribute(reader, attribute);

                        if (attributesForHeaderItem.ContainsKey(dimensionID))
                        {
                            attributesForHeaderItem[dimensionID].Add(attribute);
                        }
                        else
                        {
                            attributesForHeaderItem[dimensionID] = new List<DimensionAttribute>() { attribute };
                        }
                        count++;
                    }
                }
            }

            return attributesForHeaderItem;
        }
    }
}
