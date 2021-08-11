using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class RetailGroupMapper : MapperBase
    {

        private static void SetRetailDepartment(RetailGroup group, RecordIdentifier departmentID)
        {
            group.RetailDepartmentID = departmentID;

            if (!Providers.RetailDepartmentData.Exists(PluginEntry.DataModel, departmentID))
            {
                RetailDepartment department = new RetailDepartment();
                department.ID = departmentID;
                department.Text = (string)departmentID;

                Providers.RetailDepartmentData.Save(PluginEntry.DataModel, department);
            }
        }

        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems, MergeModeEnum mergeMode)
        {
            RetailGroup group;
            RecordIdentifier id;
            bool inserting;
            bool dirty = false;

            List<RetailDepartment> departments = Providers.RetailDepartmentData.GetDetailedList(PluginEntry.DataModel, RetailDepartment.SortEnum.Description, false);
            Dictionary<RecordIdentifier, RecordIdentifier> departmentLookup = new Dictionary<RecordIdentifier, RecordIdentifier>();
            departments.ForEach(d => 
            {
                if (!departmentLookup.ContainsKey(d.ID))
                {
                    departmentLookup.Add(d.ID, d.MasterID);
                }
            });

            // Check that mandatory and semi mandatory columns exits
            CheckMandatoryColumns(dt, new string[] { "ID", "DESCRIPTION" });


            // Start importing
            foreach (DataRow row in dt.Rows)
            {
                int lineNumber = dt.GetRowNumber(row, "Line Number");
                string description = row.GetStringValue("DESCRIPTION");
                id = row.GetStringValue("ID");

                if (id == "")
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.MandatoryFieldMissing.Replace("#1", "ID"),
                        lineNumber, description));

                    continue;
                }

                inserting = false;

                try
                {

                    if (Providers.RetailGroupData.Exists(PluginEntry.DataModel, id))
                    {
                        if (mergeMode == MergeModeEnum.InsertIfNotExists)
                        {
                            continue;
                        }

                        group = Providers.RetailGroupData.Get(PluginEntry.DataModel, id);
                        dirty = false;
                    }
                    else
                    {
                        group = new RetailGroup();
                        group.ID = id;
                        dirty = true;
                        inserting = true;
                    }

                    if (mergeMode == MergeModeEnum.Merge && !inserting)
                    {
                        // We need to do merge, thus only change if something changed
                        if (row["DESCRIPTION"] != DBNull.Value)
                        {
                            if (row.GetStringValue("DESCRIPTION") != group.Text)
                            {
                                group.Text = row.GetStringValue("DESCRIPTION");
                                dirty = true;
                            }
                        }


                        if (row["SALES TAX GROUP"] != DBNull.Value)
                        {
                            if (row.GetStringValue("SALES TAX GROUP") != group.ItemSalesTaxGroupId)
                            {
                                group.ItemSalesTaxGroupId = row.GetStringValue("SALES TAX GROUP");
                                dirty = true;
                            }
                        }

                        if (row["RETAIL DEPARTMENT ID"] != DBNull.Value)
                        {
                            if (row.GetStringValue("RETAIL DEPARTMENT ID") != group.RetailDepartmentID)
                            {
                                string departmentID = row.GetStringValue("RETAIL DEPARTMENT ID");
                                if (departmentLookup.ContainsKey(departmentID))
                                {
                                    group.RetailDepartmentID = departmentID;
                                    group.RetailDepartmentMasterID = departmentLookup[departmentID];
                                    dirty = true;
                                }
                            }
                        }

                        if(row["TARE WEIGHT"] != DBNull.Value)
                        {
                            if(row.GetDecimalValue("TARE WEIGHT") != group.TareWeight)
                            {
                                group.TareWeight = row.GetIntegerValue("TARE WEIGHT");
                                dirty = true;
                            }
                        }

                    }
                    else
                    {
                        // We just do dumb insert since we are either inserting new or in overide mode
                        if (!CheckMandatoryFields(dt, row, id, new string[] { "DESCRIPTION" }, inserting, importLogItems, lineNumber, description))
                        {
                            continue;
                        }

                        if (row["DESCRIPTION"] != DBNull.Value)
                        {
                            group.Text = row.GetStringValue("DESCRIPTION");
                            dirty = true;
                        }


                        if (row["SALES TAX GROUP"] != DBNull.Value)
                        {
                            group.ItemSalesTaxGroupId = row.GetStringValue("SALES TAX GROUP");
                            dirty = true;
                        }

                        if (row["RETAIL DEPARTMENT ID"] != DBNull.Value)
                        {
                            string departmentID = row.GetStringValue("RETAIL DEPARTMENT ID");
                            if (departmentLookup.ContainsKey(departmentID))
                            {
                                group.RetailDepartmentID = departmentID;
                                group.RetailDepartmentMasterID = departmentLookup[departmentID];
                                dirty = true;
                            }
                        }

                        if (row["TARE WEIGHT"] != DBNull.Value)
                        {
                            group.TareWeight = row.GetIntegerValue("TARE WEIGHT");
                            dirty = true;
                        }
                    }

                    // Post processing
                    // ------------------------------------------------------------------------------------
                    if (group.ItemSalesTaxGroupId != "")
                    {
                        if (!Providers.ItemSalesTaxGroupData.Exists(PluginEntry.DataModel, group.ItemSalesTaxGroupId))
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.TaxGroupDoesNotExist,
                                lineNumber, description));
                            continue;
                        }
                    }

                    if (group.RetailDepartmentID != "")
                    {
                        if (!Providers.RetailDepartmentData.Exists(PluginEntry.DataModel, group.RetailDepartmentID))
                        {
                            RetailDepartment department = new RetailDepartment();
                            department.ID = group.RetailDepartmentID;
                            department.Text = (string)group.RetailDepartmentID;

                            try
                            {
                                Providers.RetailDepartmentData.Save(PluginEntry.DataModel, department);
                            }
                            catch (Exception ex)
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, ex.Message, lineNumber, description));
                                continue;
                            }

                        }

                    }
                    // End of post processing------------------------------------------------------------------------------------

                    // Saving
                    if (dirty)
                    {
                        Providers.RetailGroupData.Save(PluginEntry.DataModel, group);

                        importLogItems.Add(new ImportLogItem(inserting ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, id, "",
                            lineNumber, description));
                    }

                }
                catch (Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, ex.Message));
                    continue;
                }
            }
        }
    }
}
