using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for RetailGroups
    /// </summary>
    public class RetailGroupsData : SqlServerDataProviderBase, IRetailGroupsData
    {
        /// <summary>
        /// Get RetailGroup List from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>List of retail group</returns>
        public virtual List<RetailGroups> GetRetailGroupList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(ID, '') as ID,
                                    ISNULL(RETAILGROUPID, '') as RETAILGROUPID 
                                    FROM WIZARDTEMPLATERETAILGROUPS 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<RetailGroups>(entry, cmd, CommandType.Text, null, PopulateRetailGroupsItems);
            }
        }

        private static void PopulateRetailGroupsItems(IConnectionManager entry, IDataReader dr, RetailGroups retailGroups , object obj)
        {
            retailGroups.RetailGroupID = Convert.ToString(dr["RETAILGROUPID"]);

            retailGroups.ID = Convert.ToString(dr["ID"]);
        }

        /// <summary>
        /// Save RetailGroup list into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupsList">RetailGroups List</param>
        public virtual void SaveGroups(IConnectionManager entry, List<RetailGroups> retailGroupsList)
        {
            Delete(entry, retailGroupsList.First().ID);
            
            foreach (var group in retailGroupsList)
            {
                if (group.RetailGroupID != string.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATERETAILGROUPS")
                        {
                            StatementType = StatementType.Insert
                        };

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("ID", (string)group.ID);

                    statement.AddField("RETAILGROUPID", group.RetailGroupID, SqlDbType.NVarChar);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">Table name</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATERETAILGROUPS")
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.ManageRetailGroups);
        }

        private static void PopulateRetailGroup(IDataReader dr, RetailGroup group)
        {
            group.ID = (string)dr["GROUPID"];
            group.Text = (string)dr["NAME"];
            group.RetailDepartmentID = (string)dr["RetailDepartmentId"];
            group.RetailDepartmentName = (string)dr["RetailDepartmentName"];
            group.ItemSalesTaxGroupId = (string)dr["TaxItemGroupId"];
            group.ItemSalesTaxGroupName = (string)dr["TaxItemGroupName"];
            group.ProfitMargin = (dr["DEFAULTPROFIT"] != DBNull.Value) ? Convert.ToDecimal(dr["DEFAULTPROFIT"]) : 0;
            group.ValidationPeriod = (string)dr["POSPERIODICID"];
            group.ValidationPeriodDescription = (string)dr["VALIDATIONPERIODDISCOUNTDESCRIPTION"];
        }

        /// <summary>
        /// Gets a retail group with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the retail group to get</param>
        /// <returns>A retail group with a given ID, or null if not found</returns>
        public virtual RetailGroup GetSelectedRetailGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            var cmd = entry.Connection.CreateCommand();

            ValidateSecurity(entry);

            cmd.CommandText =
                "Select a.GROUPID as GROUPID, ISNULL(a.NAME,'') AS NAME, ISNULL(a.DEFAULTPROFIT,0) AS DEFAULTPROFIT, " +
                " ISNULL(a.POSPERIODICID, '') AS POSPERIODICID,  " +
                "    ISNULL(iid.DEPARTMENTID,'') AS RetailDepartmentId, ISNULL(iid.NAME,'') AS RetailDepartmentName, " +
                "    ISNULL(sig.SIZEGROUP,'') AS SizeGroupId, ISNULL(sig.DESCRIPTION,'') AS SizeGroupName, " +
                "    ISNULL(cg.COLORGROUP,'') AS ColorGroupId, ISNULL(cg.DESCRIPTION,'') AS ColorGroupName, " +
                "    ISNULL(stg.STYLEGROUP,'') AS StyleGroupID, ISNULL(stg.DESCRIPTION,'') AS StyleGroupName, " +
                "    ISNULL(idg.DIMGROUPID,'') AS DimensionGroupId, ISNULL(idg.NAME,'') AS DimensionGroupName, " +
                "    ISNULL(tigh.TAXITEMGROUP,'') AS TaxItemGroupId, ISNULL(tigh.NAME,'') AS TaxItemGroupName, " +
                "    ISNULL(PDV.ID, '') AS POSPERIODICID, ISNULL(PDV.DESCRIPTION, '') AS VALIDATIONPERIODDISCOUNTDESCRIPTION " +

                "From RBOINVENTITEMRETAILGROUP a " +

                "    left outer join RBOINVENTITEMDEPARTMENT iid on a.DATAAREAID = iid.DATAAREAID AND a.DEPARTMENTID = iid.DEPARTMENTID " +
                "    left outer join INVENTITEMGROUP iig on a.DATAAREAID = iig.DATAAREAID AND a.ITEMGROUPID = iig.ITEMGROUPID " +
                "    left outer join RBOSIZEGROUPTABLE sig on a.DATAAREAID = sig.DATAAREAID AND a.SIZEGROUPID = sig.SIZEGROUP " +
                "    left outer join RBOCOLORGROUPTABLE cg on a.DATAAREAID = cg.DATAAREAID AND a.COLORGROUPID = cg.COLORGROUP " +
                "    left outer join RBOSTYLEGROUPTABLE  stg on a.DATAAREAID = stg.DATAAREAID AND a.STYLEGROUPID = stg.STYLEGROUP " +
                "    left outer join INVENTDIMGROUP idg on a.DATAAREAID = idg.DATAAREAID AND a.INVENTDIMGROUPID = idg.DIMGROUPID " +
                "    left outer join TAXITEMGROUPHEADING tigh on a.DATAAREAID = tigh.DATAAREAID AND a.SALESTAXITEMGROUP = tigh.TAXITEMGROUP " +
                "    left outer join POSDISCVALIDATIONPERIOD PDV on a.DATAAREAID = PDV.DATAAREAID and a.POSPERIODICID = PDV.ID " +
                "    Where a.GROUPID = @GROUPID AND a.DATAAREAID = @DATAAREAID";

            MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
            MakeParam(cmd, "GROUPID", (string)groupID);

            var groups = Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);

            return (groups.Count > 0) ? groups[0] : null;
        }

        /// <summary>
        /// New Save method for RetailItemModule to save retail item module into database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="module">RetailItemModule</param>
        public virtual void SmartSave(IConnectionManager entry, RetailItemOld.RetailItemModule module)
        {
            for (int moduleType = 0; moduleType < 3; moduleType++)
            {
                if (module.Dirty)
                {
                    var statement = new SqlServerStatement("INVENTTABLEMODULE");

                    if (!InventTableModuleExists(entry, module.ItemID, moduleType))
                    {
                        statement.StatementType = StatementType.Insert;

                        statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                        statement.AddKey("ITEMID", (string)module.ItemID);
                        statement.AddKey("MODULETYPE", moduleType, SqlDbType.Int);
                    }
                    else
                    {
                        statement.StatementType = StatementType.Update;

                        statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                        statement.AddCondition("ITEMID", (string)module.ItemID);
                        statement.AddCondition("MODULETYPE", moduleType, SqlDbType.Int);
                    }

                    statement.AddField("UNITID", (string)module.Unit);
                    statement.AddField("PRICE", module.Price, SqlDbType.Decimal);
                    statement.AddField("PRICEUNIT", module.PriceUnit, SqlDbType.Decimal);
                    statement.AddField("MARKUP", module.Markup, SqlDbType.Decimal);
                    statement.AddField("LINEDISC", (string)module.LineDiscount);
                    statement.AddField("MULTILINEDISC", (string)module.MultilineDiscount);
                    statement.AddField("ENDDISC", module.TotalDiscount ? 1 : 0, SqlDbType.TinyInt);
                    statement.AddField("PRICEDATE", module.PriceDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                    statement.AddField("PRICEQTY", module.PriceQty, SqlDbType.Decimal);
                    statement.AddField("ALLOCATEMARKUP", module.AllocateMarkup ? 1 : 0, SqlDbType.TinyInt);
                    statement.AddField("TAXITEMGROUPID", (string)module.TaxItemGroupID);
                    statement.AddField("PRICEINCLTAX", module.LastKnownPriceWithTax, SqlDbType.Decimal);

                    entry.Connection.ExecuteStatement(statement);
                }

                module.Dirty = false;
            }
        }

        /// <summary>
        /// Method to check data in InventTableModule for RetailItemModule
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">itemID</param>
        /// <param name="moduleID">moduleID</param>
        /// <returns>boolean result</returns>
        private static bool InventTableModuleExists(IConnectionManager entry, RecordIdentifier itemID, int moduleID)
        {
            return RecordExists(entry, "INVENTTABLEMODULE", new[] { "ITEMID", "MODULETYPE" }, new RecordIdentifier(itemID, moduleID));
        }

        /// <summary>
        /// Generates new RetailGroupID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>new RetailGroupId</returns>
        public virtual RecordIdentifier GenerateRetailGroupID(IConnectionManager entry)
        {
            return DataProviderFactory.Instance.GenerateNumber<IRetailGroupData, RetailGroup>(entry);
        }
    }
}
