using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class RetailDepartmentMapper : MapperBase
    {
        

        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems, MergeModeEnum mergeMode)
        {
            RetailDepartment department;
            RecordIdentifier id;
            bool inserting;
            bool dirty = false;

            CheckMandatoryColumns(dt, new string[] { "ID", "DESCRIPTION" });

            //start importing
            foreach (DataRow row in dt.Rows)
            {
                int lineNumber = dt.GetRowNumber(row, "Line Number");
                string description = row.GetStringValue("DESCRIPTION");
                id = row.GetStringValue("ID");
                inserting = false;

                if (id == "")
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.MandatoryFieldMissing.Replace("#1", "ID"),
                        lineNumber, description));

                    continue;
                }

                try
                {
                    if (Providers.RetailDepartmentData.Exists(PluginEntry.DataModel, id))
                    {
                        if (mergeMode == MergeModeEnum.InsertIfNotExists)
                        {
                            continue;
                        }
                        department = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, id);
                        dirty = false;
                    }

                    else
                    {
                        department = new RetailDepartment();
                        department.ID = id;
                        dirty = true;
                        inserting = true;
                    }

                    if (mergeMode == MergeModeEnum.Merge && !inserting)
                    {
                        // Merge in the changes if something changed 

                        if (row["DESCRIPTION"] != DBNull.Value)
                        {
                            if (row.GetStringValue("DESCRIPTION") != department.Text)
                            {
                                department.Text = row.GetStringValue("DESCRIPTION");
                                dirty = true;
                            }
                        }
                    }
                    else
                    {
                        if (!CheckMandatoryFields(dt, row, id, new string[] { "DESCRIPTION" }, inserting, importLogItems, lineNumber, description))
                        {
                            continue;
                        }

                        if (row.GetStringValue("DESCRIPTION") != department.Text)
                        {
                            department.Text = row.GetStringValue("DESCRIPTION");
                            dirty = true;
                        }

                    }

                    // Post processing
                    // ------------------------------------------------------------------------------------


                    //-------------------------------------------------------------------------------------
                    //End of post processing


                    //saving

                    if (dirty)
                    {
                        Providers.RetailDepartmentData.Save(PluginEntry.DataModel, department);

                        importLogItems.Add(new ImportLogItem(inserting ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, id, "",
                            lineNumber, description));
                    }
                }
                catch(Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped,dt.TableName, id, ex.Message));
                    continue;
                }

            }
        }


    }
}
