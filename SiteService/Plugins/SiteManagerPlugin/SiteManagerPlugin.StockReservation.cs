using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using Quartz.Util;


namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public InventoryAdjustment SaveInventoryAdjustment(LogonInfo logonInfo, InventoryAdjustment adjustmentJournal)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            adjustmentJournal.CreatedDateTime = DateTime.Now;
            adjustmentJournal.PostedDateTime = Date.Empty;
            Providers.InventoryAdjustmentData.Save(entry, adjustmentJournal);

            return FindInventoryAdjustment(logonInfo, adjustmentJournal.MasterID);
        }

        public void MoveInventoryAdjustment(LogonInfo logonInfo, InventoryAdjustment adjustmentJournal)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            List<InventoryJournalTransaction> transactions = Providers.InventoryJournalTransactionData.GetJournalTransactionList(entry, adjustmentJournal.MasterID);

            if (transactions != null && transactions.Count > 0)
            {
                foreach (InventoryJournalTransaction jt in transactions)
                {
                    List<InventoryJournalTransaction> items = transactions.Where(w => w.ItemId == jt.ItemId && w.UnitID == jt.UnitID && w.InventoryUnitID == jt.InventoryUnitID).ToList();
                    if (items.Sum(s => s.AdjustmentInInventoryUnit) != decimal.Zero)
                    {
                        //Save the adjustment to the new store id
                        jt.Adjustment = items.Sum(s => s.Adjustment);
                        jt.AdjustmentInInventoryUnit = items.Sum(s => s.AdjustmentInInventoryUnit);
                        ReserveStockTransaction(logonInfo, jt, adjustmentJournal.StoreId);

                        //Zero out the old adjustment
                        jt.Adjustment *= -1;
                        jt.AdjustmentInInventoryUnit *= -1;
                        ReserveStockTransaction(logonInfo, jt, jt.StoreId);
                    }
                }
            }

        }

        public InventoryAdjustment FindInventoryAdjustment(LogonInfo logonInfo, RecordIdentifier masterID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            return Providers.InventoryAdjustmentData.Get(entry, masterID);
        }

        public void CloseInventoryAdjustment(LogonInfo logonInfo, RecordIdentifier journalId)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            Providers.InventoryAdjustmentData.PostAdjustment(entry, journalId);
        }


        public void ReserveStockTransaction(LogonInfo logonInfo, InventoryJournalTransaction reservation, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            
            Services.Interfaces.Services.InventoryService(entry).PostInventoryAdjustmentLine(entry, reservation, storeID);
        }

        public void ReserveStockItem(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID, decimal adjustment, InventoryTypeEnum inventoryType,
            decimal costPrice, decimal netSalesPricePerItem, decimal salesPricePerItem, string unitID, decimal adjustmentInventoryUnit, RecordIdentifier reasonCode, string reasonText)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            if (!string.IsNullOrWhiteSpace(reasonCode.StringValue))
            {
                if (!Providers.InventoryTransactionReasonData.Exists(entry, reasonCode))
                {
                    InventoryTransactionReason reason = new InventoryTransactionReason
                    {
                        ID = reasonCode,
                        Text = reasonText
                    };

                    Providers.InventoryTransactionReasonData.Save(entry, reason);
                }
            }

            InventoryTransaction inventTrans = new InventoryTransaction();
            inventTrans.ItemID = itemID;
            inventTrans.StoreID = storeID;
            inventTrans.Adjustment = adjustment;
            inventTrans.Type = inventoryType;
            inventTrans.CostPricePerItem = costPrice;
            inventTrans.SalesPriceWithoutTaxPerItem = netSalesPricePerItem;
            inventTrans.SalesPriceWithTaxPerItem = salesPricePerItem;
            inventTrans.InventoryUnitID = unitID;
            inventTrans.AdjustmentInInventoryUnit = adjustmentInventoryUnit;
            inventTrans.ReasonCode = reasonCode;

            Providers.InventoryTransactionData.Save(entry, inventTrans, true);
        }

        public void UpdateReasonCodes(LogonInfo logonInfo, List<DataEntity> reasonCodes, InventoryJournalTypeEnum inventoryType)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            if (inventoryType == InventoryJournalTypeEnum.Reservation)
            {
                DataEntity toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID && f.Text != CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID);
                if (toUpdate != null)
                {
                    Providers.ReasonsData.Save(entry, toUpdate);
                }

                toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID && f.Text != CustomerOrderReasonConstants.ConstPickupFromOrderReasonID);
                if (toUpdate != null)
                {
                    Providers.ReasonsData.Save(entry, toUpdate);
                }

                toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID && f.Text != CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID);
                if (toUpdate != null)
                {
                    Providers.ReasonsData.Save(entry, toUpdate);
                }
            }
        }

        public List<DataEntity> GetReasonCodes(LogonInfo logonInfo, InventoryJournalTypeEnum inventoryType)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            List<DataEntity> reasonCodes = Providers.ReasonsData.GetList(entry);

            if (inventoryType == InventoryJournalTypeEnum.Reservation)
            {
                DataEntity reserveStockToOrder = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID);
                if (reserveStockToOrder == null)
                {
                    reserveStockToOrder = new DataEntity(CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID, CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID);
                    Providers.ReasonsData.Save(entry, reserveStockToOrder);
                }

                DataEntity voidStockFromOrder = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID);
                if (voidStockFromOrder == null)
                {
                    voidStockFromOrder = new DataEntity(CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID, CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID);
                    Providers.ReasonsData.Save(entry, voidStockFromOrder);
                }

                DataEntity pickupFromOrder = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID);
                if (pickupFromOrder == null)
                {
                    pickupFromOrder = new DataEntity(CustomerOrderReasonConstants.ConstPickupFromOrderReasonID, CustomerOrderReasonConstants.ConstPickupFromOrderReasonID);
                    Providers.ReasonsData.Save(entry, pickupFromOrder);
                }

                reasonCodes.Clear();
                reasonCodes.Add(reserveStockToOrder);
                reasonCodes.Add(voidStockFromOrder);
                reasonCodes.Add(pickupFromOrder);
            }

            return reasonCodes;
        }
    }
}
