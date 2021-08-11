using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System.Windows.Forms;

namespace LSOne.Services
{
    public partial class InventoryService : IInventoryService
    {
        public IErrorLog ErrorLog
        {
            set
            {
            }
        }

        public void Init(IConnectionManager entry)
        {
        }
        

        public virtual void PostInventoryAdjustmentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryJournalTransaction inventoryJournalTrans, RecordIdentifier storeID, InventoryTypeEnum typeOfAdjustmentLine, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).PostInventoryAdjustmentLine(entry, siteServiceProfile, inventoryJournalTrans, storeID, typeOfAdjustmentLine, closeConnection);
        }

        public virtual decimal GetEffectiveInventoryForItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetEffectiveInventoryForItem(entry,siteServiceProfile, itemID, storeID, closeConnection);
        }

        public virtual void Disconnect(IConnectionManager entry)
        {
            Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
        }

        public virtual ConnectionEnum TestConnection(IConnectionManager entry, string host, UInt16 port)
        {
            return Interfaces.Services.SiteServiceService(entry).TestConnection(entry, host, port);
        }

        public virtual ConnectionEnum TestConnectionWithFeedback(IConnectionManager entry, string host, UInt16 port)
        {
            return Interfaces.Services.SiteServiceService(entry).TestConnectionWithFeedback(entry, host, port);
        }

        public virtual decimal GetInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryOnHand(entry, siteServiceProfile, itemID, storeID, closeConnection);
        }

        public virtual decimal GetInventoryOnHandForAssemblyItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryOnHandForAssemblyItem(entry, siteServiceProfile, itemID, storeID, closeConnection);
        }

        public virtual List<InventoryStatus> GetInventoryListForItemAndStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort, bool backwardsSort, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryListForItemAndStore(entry, siteServiceProfile, itemID, storeID, regionID, sort, backwardsSort, closeConnection);
        }

        public virtual List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort, bool backwardsSort, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryListForAssemblyItemAndStore(entry, siteServiceProfile, itemID, storeID, regionID, sort, backwardsSort, closeConnection);
        }

        public virtual RecordIdentifier GetInventoryUnitId(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryUnitId(entry, siteServiceProfile, itemID, closeConnection);
        }

        public virtual void UpdateInventoryUnit(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            decimal conversionFactor, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).UpdateInventoryUnit(entry, siteServiceProfile, itemID, conversionFactor, closeConnection);
        }

        public List<ItemLedger> GetItemLedgerList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            ItemLedgerSearchParameters itemSearch, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemLedgerList(entry, siteServiceProfile, itemSearch, closeConnection);
        }
    }
}
