using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.ViewPlugins.Inventory.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory
{
    internal partial class PluginOperations
    {

        public static void ShowInventoryAdjustmentsView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.StockReservations
                    || PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.ParkedInventory)
                {
                    PluginEntry.Framework.ViewController.ReplaceView(PluginEntry.Framework.ViewController.CurrentView, new Views.InventoryJournalsView(PluginEntry.DataModel, InventoryJournalTypeEnum.Adjustment, PluginEntry.DataModel.CurrentStoreID));
                }
                else
                {
                    PluginEntry.Framework.ViewController.Add(new Views.InventoryJournalsView(PluginEntry.DataModel, InventoryJournalTypeEnum.Adjustment, PluginEntry.DataModel.CurrentStoreID));
                }
            }
        }

        public static void ShowStockReservationsView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.InventoryAdjustments
                 || PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.ParkedInventory)
                {
                    PluginEntry.Framework.ViewController.ReplaceView(PluginEntry.Framework.ViewController.CurrentView, new Views.InventoryJournalsView(PluginEntry.DataModel, InventoryJournalTypeEnum.Reservation, PluginEntry.DataModel.CurrentStoreID));
                }
                else
                {
                    PluginEntry.Framework.ViewController.Add(new Views.InventoryJournalsView(PluginEntry.DataModel, InventoryJournalTypeEnum.Reservation, PluginEntry.DataModel.CurrentStoreID));
                }
            }
        }
        public static void ShowParkedInventoryView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.StockReservations
                    || PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.InventoryAdjustments)
                {
                    PluginEntry.Framework.ViewController.ReplaceView(PluginEntry.Framework.ViewController.CurrentView, new Views.InventoryJournalsView(PluginEntry.DataModel, InventoryJournalTypeEnum.Parked, PluginEntry.DataModel.CurrentStoreID));
                }
                else
                {
                    PluginEntry.Framework.ViewController.Add(new Views.InventoryJournalsView(PluginEntry.DataModel, InventoryJournalTypeEnum.Parked, PluginEntry.DataModel.CurrentStoreID));
                }
            }
        }

        /// <summary>
        ///  Shows the <see cref="InventoryWizard"/> window if SiteService is available. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void ShowInventoryJournalWizard(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                InventoryTypeAction inventoryTypeAction = new InventoryTypeAction
                {
                    InventoryType = InventoryEnum.InventoryJournal,
                    Action = InventoryActionEnum.Manage
                };

                ShowInventoryJournalWizard(inventoryTypeAction);
            }
        }

        /// <summary>
        /// Shows the <see cref="InventoryWizard"/> window. It does not check if SiteService is available. 
        /// </summary>
        /// <param name="inventoryTypeAction"></param>
        /// <param name="journalType"></param>
        /// <remarks>If called directly, always place it in an <code>if (TestSiteService())</code> block.</remarks>
        public static void ShowInventoryJournalWizard(InventoryTypeAction inventoryTypeAction, InventoryJournalTypeEnum journalType = InventoryJournalTypeEnum.Adjustment)
        {
            InventoryWizard dlg = new InventoryWizard(PluginEntry.DataModel, inventoryTypeAction, journalType);

            PluginEntry.Framework.SuspendSearchBarClosing();

            dlg.ShowDialog();

            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        public static void ShowInventoryJournalView(InventoryJournalTypeEnum journalType, RecordIdentifier journalId)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.InventoryJournalView(PluginEntry.DataModel, journalType, journalId));
            }
        }

        /// <summary>
        /// Verifies user permissions to manage inventory journals features (adjustments, stock reservations, parked inventory).
        /// </summary>
        /// <param name="journalType"></param>
        /// <returns></returns>
        internal static bool HasInventoryJournalPermission(InventoryJournalTypeEnum journalType)
        {
            bool hasAccess = false;

            switch (journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    hasAccess = (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores)
                                || (!PluginEntry.DataModel.IsHeadOffice
                                    && (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
                                        || PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments))));
                    break;
                case InventoryJournalTypeEnum.Reservation:
                    hasAccess = (PluginEntry.DataModel.HasPermission(Permission.ManageStockReservationsForAllStores)
                                || (!PluginEntry.DataModel.IsHeadOffice
                                    && PluginEntry.DataModel.HasPermission(Permission.ManageStockReservations)));
                    break;
                case InventoryJournalTypeEnum.Parked:
                    hasAccess = (PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventoryForAllStores)
                                || (!PluginEntry.DataModel.IsHeadOffice
                                    && PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory)));
                    break;
            }

            return hasAccess;
        }

        internal static bool MoveToInventory(InventoryAdjustment parkedJournal, List<InventoryJournalTransaction> parkedJournalLines, ReasonCode reason)
        {
            if (parkedJournalLines == null || parkedJournalLines.Count == 0)
            {
                return true;
            }

            bool result = false;

            try
            {
                CostCalculation costCalculation = (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation).IntValue;

                foreach (var line in parkedJournalLines)
                {
                    if (Math.Abs(line.Adjustment) - line.MovedQuantity == 0)
                        continue;

                    if (MoveToInventory(line, Math.Abs(line.Adjustment) - line.MovedQuantity, reason, costCalculation) == false)
                    {
                        return false;
                    }
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        internal static bool MoveToInventory(List<InventoryAdjustment> parkedJournals, ReasonCode reason)
        {
            bool result = false;

            try
            {
                if (parkedJournals != null && parkedJournals.Count > 0)
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    int totalLines = 0;
                    foreach (var journal in parkedJournals)
                    {
                        var journalLines = service.AdvancedSearch(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                                                  new InventoryJournalLineSearch { JournalId = journal.ID, SortBackwards = false, SortBy = InventoryJournalTransactionSorting.LineNumber, RowFrom = 0 , RowTo = 0 },
                                                                  out totalLines, true);
                        if (MoveToInventory(journal, journalLines, reason))
                        {
                            service.CloseInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journal.ID, true);
                        };
                    }
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        internal static bool MoveToInventory(InventoryJournalTransaction parkedJournalLine, decimal quantity, ReasonCode reason, CostCalculation costCalculation)
        {
            bool result = false;

            try
            {
                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                //get child rows
                var moved2Inventory = service.GetMovedToInventoryLines(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                                                        parkedJournalLine.JournalId,
                                                                        parkedJournalLine.MasterID,
                                                                        InventoryJournalTransactionSorting.ItemName,
                                                                        false,
                                                                        false);

                decimal returnedQty = (moved2Inventory != null && moved2Inventory.Count > 0)
                                            ? moved2Inventory.Sum(line => line.Adjustment)
                                            : 0;

                if (Math.Abs(parkedJournalLine.Adjustment) < returnedQty + quantity)
                    return false;

                if (Math.Abs(parkedJournalLine.Adjustment) >= returnedQty + quantity)
                {
                    //Update first the parent line such that a master id will be generated if it's missing
                    InventoryJournalStatus parentLineStatus = Math.Abs(parkedJournalLine.Adjustment) > returnedQty + quantity
                                                                ? InventoryJournalStatus.PartialPosted
                                                                : InventoryJournalStatus.Closed;
                    RecordIdentifier parentMasterId = service.UpdateStatus(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                                                            parkedJournalLine.JournalId,
                                                                            parkedJournalLine.LineNum,
                                                                            parkedJournalLine.MasterID,
                                                                            parentLineStatus,
                                                                            false);

                    InventoryJournalTransaction m2iLine = (InventoryJournalTransaction)parkedJournalLine.Clone();
                    m2iLine.LineNum = RecordIdentifier.Empty;
                    m2iLine.ParentMasterID = RecordIdentifier.IsEmptyOrNull(parkedJournalLine.MasterID) ? parentMasterId : parkedJournalLine.MasterID;
                    m2iLine.MasterID = RecordIdentifier.Empty;
                    m2iLine.Adjustment = quantity;
                    m2iLine.Status = InventoryJournalStatus.Posted;
                    m2iLine.PostedDateTime = DateTime.Now;
                    m2iLine.TransDate = DateTime.Now;
                    m2iLine.ReasonId = reason.ID;
                    m2iLine.ReasonText = reason.Text;
                    m2iLine.AdjustmentInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel,
                                                                                        m2iLine.ItemId,
                                                                                        m2iLine.UnitID,
                                                                                        m2iLine.InventoryUnitID,
                                                                                        m2iLine.Adjustment);
                    m2iLine.StaffID = PluginEntry.DataModel.CurrentUser.StaffID;

                    if (costCalculation == CostCalculation.Manual)
                    {
                        m2iLine.CostPrice = Providers.RetailItemData.GetItemPrice(PluginEntry.DataModel, m2iLine.ItemId).PurchasePrice;
                    }
                    else
                    {
                        RetailItemCost itemCost = service.GetRetailItemCost(PluginEntry.DataModel, GetSiteServiceProfile(), m2iLine.ItemId, m2iLine.StoreId, false);
                        m2iLine.CostPrice = itemCost == null ? 0 : itemCost.Cost;
                    }

                    service.PostInventoryAdjustmentLine(PluginEntry.DataModel,
                                                        GetSiteServiceProfile(),
                                                        m2iLine,
                                                        parkedJournalLine.StoreId,
                                                        InventoryTypeEnum.Parked,
                                                        true);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "MovedToInventoryLine", parkedJournalLine.MasterID, m2iLine.Adjustment);
                };

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;

        }




    }
}
