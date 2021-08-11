using System;
using System.Collections.Generic;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailItem
{
    public partial class IntegrationFrameworkRetailItemPlugin
    {
        public virtual void Save(RetailItem retailItem)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    if (retailItem.RetailGroupMasterID.IsEmpty && !retailItem.RetailGroupID.IsEmpty)
                    {                        

                        RetailGroup retailGroup = GetRetailGroupForItem(dataModel, retailItem);                        

                        retailItem.RetailGroupMasterID = retailGroup.MasterID;                         
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.SalesUnitID) && !Providers.UnitData.Exists(dataModel, retailItem.SalesUnitID))
                    {
                        CreateUnit(dataModel, retailItem.SalesUnitID, retailItem.SalesUnitName);
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.InventoryUnitID) && !Providers.UnitData.Exists(dataModel, retailItem.InventoryUnitID))
                    {
                        CreateUnit(dataModel, retailItem.InventoryUnitID, retailItem.InventoryUnitName);
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.PurchaseUnitID) && !Providers.UnitData.Exists(dataModel, retailItem.PurchaseUnitID))
                    {
                        CreateUnit(dataModel, retailItem.PurchaseUnitID, retailItem.PurchaseUnitName);
                    }

                    if (RecordIdentifier.IsEmptyOrNull(retailItem.SalesUnitID) || RecordIdentifier.IsEmptyOrNull(retailItem.InventoryUnitID) || RecordIdentifier.IsEmptyOrNull(retailItem.PurchaseUnitID))
                    {
                        SetDefaultItemUnit(dataModel, retailItem);
                    }

                    Providers.RetailItemData.Save(dataModel, retailItem);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        public virtual SaveResult SaveList(List<RetailItem> retailItems)
        {
            Action<IConnectionManager, RetailItem> save =  (connectionManager, retailItem) =>
            {
                Providers.RetailItemData.Save(connectionManager, retailItem);
            };

            Action<List<RetailItem>> prepareDataForSaveList = (items) =>
            {
                Dictionary<string, Guid> retailGroupsMasterIDs = new Dictionary<string, Guid>();
                IConnectionManager dataModel = GetConnectionManagerIF();
                // Populate the dictionary with all existing retail groups to increase performance when saving multiple items
                List<MasterIDEntity> retailGroups = Providers.RetailGroupData.GetMasterIDList(dataModel);

                foreach (MasterIDEntity retailGroup in retailGroups)
                {
                    retailGroupsMasterIDs.Add((string)retailGroup.ReadadbleID, (Guid)retailGroup.ID);
                }

                foreach (RetailItem retailItem in items)
                {
                    if (retailItem.RetailGroupMasterID.IsEmpty && !retailItem.RetailGroupID.IsEmpty)
                    {
                        if (retailGroupsMasterIDs.ContainsKey((string)retailItem.RetailGroupID))
                        {
                            retailItem.RetailGroupMasterID = retailGroupsMasterIDs[(string)retailItem.RetailGroupID];
                        }
                        else
                        {
                            RetailGroup retailGroup = GetRetailGroupForItem(dataModel, retailItem);

                            retailItem.RetailGroupMasterID = retailGroup.MasterID;

                            retailGroupsMasterIDs.Add((string)retailItem.RetailGroupID, (Guid)retailItem.RetailGroupMasterID);
                        }
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.SalesUnitID) && !Providers.UnitData.Exists(dataModel, retailItem.SalesUnitID))
                    {
                        CreateUnit(dataModel, retailItem.SalesUnitID, retailItem.SalesUnitName);
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.InventoryUnitID) && !Providers.UnitData.Exists(dataModel, retailItem.InventoryUnitID))
                    {
                        CreateUnit(dataModel, retailItem.InventoryUnitID, retailItem.InventoryUnitName);
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.PurchaseUnitID) && !Providers.UnitData.Exists(dataModel, retailItem.PurchaseUnitID))
                    {
                        CreateUnit(dataModel, retailItem.PurchaseUnitID, retailItem.PurchaseUnitName);
                    }

                    if (RecordIdentifier.IsEmptyOrNull(retailItem.SalesUnitID) || RecordIdentifier.IsEmptyOrNull(retailItem.InventoryUnitID) || RecordIdentifier.IsEmptyOrNull(retailItem.PurchaseUnitID))
                    {
                        SetDefaultItemUnit(dataModel, retailItem);
                    }
                }
            };

            return SaveList(retailItems, Providers.RetailItemData, save, prepareDataForSaveList);
        }

        public virtual RetailItem Get(RecordIdentifier itemID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.RetailItemData.Get(dataModel, itemID);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual void Delete(RecordIdentifier itemID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.RetailItemData.Delete(dataModel, itemID);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        /// <summary>
        /// Checks if the given item has a sales and inventory unit. If it doesn't have a default sales unit it will be assigned a default LS One sales unit.
        /// The default sales unit will also be created if it does not exist
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="itemID">The retail item to check for</param>
        private void SetDefaultItemUnit(IConnectionManager entry, RetailItem item)
        {
            RecordIdentifier defaultUnitID = "LS1 DEFAULT";

            if (!Providers.UnitData.Exists(entry, defaultUnitID))
            {
                Unit defaultUnit = new Unit(defaultUnitID, "LS One default unit", 0, 2);
                Providers.UnitData.Save(entry, defaultUnit);
            }
            
            if(RecordIdentifier.IsEmptyOrNull(item.SalesUnitID))
            {
                item.SalesUnitID = defaultUnitID;
            }

            if (RecordIdentifier.IsEmptyOrNull(item.InventoryUnitID))
            {
                item.InventoryUnitID = defaultUnitID;
            }

            if (RecordIdentifier.IsEmptyOrNull(item.PurchaseUnitID))
            {
                item.PurchaseUnitID = defaultUnitID;
            }
        }

        private void CreateUnit(IConnectionManager entry, RecordIdentifier unitID, string unitName)
        {
            Unit unit = new Unit();
            unit.ID = unitID;
            unit.Text = string.IsNullOrEmpty(unitName) ? (string)unitID : unitName;
            unit.MaximumDecimals = 0;
            unit.MinimumDecimals = 0;
            Providers.UnitData.Save(entry, unit);
        }

        /// <summary>
        /// Gets the retail group for the given retail item. If the retail group does not exist it is created with ID of <see cref="RetailItem.RetailGroupID"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItem">The retail item to get the group for</param>
        private RetailGroup GetRetailGroupForItem(IConnectionManager entry, RetailItem retailItem)
        {
            RetailGroup retailGroup = Providers.RetailGroupData.Get(entry, retailItem.RetailGroupID);

            if (retailGroup == null)
            {
                retailGroup = new RetailGroup();
                retailGroup.ID = retailItem.RetailGroupID;
                retailGroup.Text = (string)retailGroup.ID; // Put the readable ID as the name since that's all the information we have at this point

                Providers.RetailGroupData.Save(entry, retailGroup);
            }

            return retailGroup;
        }
    }
}
