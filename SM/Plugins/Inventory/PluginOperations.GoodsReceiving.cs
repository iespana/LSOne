using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory
{
    internal partial class PluginOperations
    {
        public static void NewGoodReceivingDocument(object sender, EventArgs args)
        {
            GoodsReceivingDocumentDialog dlg = new GoodsReceivingDocumentDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.Add(new Views.GoodsReceivingDocumentView(dlg.SelectedID));
            }
        }

        public static void ShowGoodsReceivingDocument(RecordIdentifier goodsReceivingDocumentID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.GoodsReceivingDocumentView(goodsReceivingDocumentID));
        }

        public static void ShowGoodsReceivingDocument(object sender, EventArgs args)
        {
            ContextBarClickEventArguments a = (ContextBarClickEventArguments)args;
            RecordIdentifier goodsReceivingDocumentID = a.Item.Key;

            ShowGoodsReceivingDocument(goodsReceivingDocumentID);
        }

        public static void ShowGoodsReceivingDocuments(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.GoodsReceivingDocumentsView());
            }
        }

        public static void ShowGoodsReceivingDocuments(RecordIdentifier goodsReceivingID)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.GoodsReceivingDocumentsView(goodsReceivingID));
            }
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        public static bool CheckMaxOverReceivedOK(RecordIdentifier goodsReceivingID,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier itemID,
            RecordIdentifier unitID,
            decimal receivedQuantity,
            RecordIdentifier excludedGRDLineNumber)
        {
            try
            {
                // Check if we are receiving over the receiving limit
                int maxOverReceived = MaxOverGoodsReceive();

                if (maxOverReceived != 0) // 0 means no limit
                {
                    List<PurchaseOrderLine> PoLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseOrderLines(
                        PluginEntry.DataModel,
                        GetSiteServiceProfile(),
                        new PurchaseOrderLineSearch { PurchaseOrderID = purchaseOrderID },
                        PurchaseOrderLineSorting.ItemID,
                        false,
                        out int totalRecords,
                        false);

                    decimal firstAmount = (from x in PoLines
                                           where x.ItemID == itemID &&
                                                 x.UnitID == unitID
                                           select x.Quantity).FirstOrDefault();
                    int orderedAmount = (int)firstAmount;

                    //If the ordered amount is 0 then this line was created by the GR document as an additional line that 
                    //was not originally in the purchase order so the max goods receive configuration cannot apply to it
                    if (orderedAmount == 0)
                    {
                        return true;
                    }

                    List<GoodsReceivingDocumentLine> existingGRDLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetGoodsReceivingDocumentLines(
                        PluginEntry.DataModel,
                        GetSiteServiceProfile(),
                        goodsReceivingID,
                        false);

                    int alreadyReceivedOfThisItem;

                    if (excludedGRDLineNumber == RecordIdentifier.Empty)
                    {
                        alreadyReceivedOfThisItem = (int)(from x in existingGRDLines
                                                          where x.purchaseOrderLine.ItemID == itemID &&
                                                                x.purchaseOrderLine.UnitID == unitID
                                                          select x.ReceivedQuantity).Sum();
                    }
                    else
                    {
                        alreadyReceivedOfThisItem = (int)(from x in existingGRDLines
                                                          where x.purchaseOrderLine.ItemID == itemID &&
                                                                x.purchaseOrderLine.UnitID == unitID &&
                                                                x.LineNumber != excludedGRDLineNumber
                                                          select x.ReceivedQuantity).Sum();
                    }

                    if ((alreadyReceivedOfThisItem + receivedQuantity - orderedAmount) > maxOverReceived)
                    {
                        int maximumReceivable = (orderedAmount + maxOverReceived);
                        MessageDialog.Show(Resources.TryingToReceiveOverLimit.Replace("#1", maximumReceivable.ToString()));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return false;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
            return true;
        }

        public static bool DeleteGoodsReceivingDocument(RecordIdentifier goodsReceivingDocumentID)
        {
            if (QuestionDialog.Show(
                Resources.DeleteGoodsReceivingDocumentQuestion,
                Resources.DeleteGoodsReceivingDocument) == DialogResult.Yes)
            {
                GoodsReceivingDocumentDeleteResult result =
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteGoodsReceivingDocument(
                            PluginEntry.DataModel,
                            GetSiteServiceProfile(),
                            goodsReceivingDocumentID, true);

                if (result == GoodsReceivingDocumentDeleteResult.HasPostedLines)
                {
                    MessageDialog.Show(Resources.GoodsReceivingDocumentHasPostedLines);
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool DeleteGoodsReceivingDocumentLine(RecordIdentifier goodsReceivingDocumentLineID)
        {
            if (QuestionDialog.Show(
                Resources.DeleteGoodsReceivingDocumentLineQuestion,
                Resources.DeleteGoodsReceivingDocumentLine) == DialogResult.Yes)
            {
                try
                {
                    if (Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GoodReceivingDocumentLineIsPosted(
                            PluginEntry.DataModel,
                            GetSiteServiceProfile(),
                            goodsReceivingDocumentLineID,
                            true))
                    {
                        MessageDialog.Show(Resources.GoodsReceivingDocumentLineIsPosted);
                        return false;
                    }
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                        DeleteGoodsReceivingDocumentLine(
                            PluginEntry.DataModel,
                            GetSiteServiceProfile(),
                            goodsReceivingDocumentLineID,
                            true);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
                finally
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .Disconnect(PluginEntry.DataModel);
                }
            }

            return false;
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        /// <summary>
        /// Saves a goods receiving document line to the database with the given properties.
        /// </summary>
        /// <param name="GoodsReceivingDocumentID"></param>
        /// <param name="itemID"></param>
        /// <param name="unitID"></param>
        /// <param name="lineNumber">This is RecordIdentifier.Empty if we are dealing with a new line</param>
        /// <param name="StoreID"></param>
        /// <param name="receivedQuantity"></param>
        /// <param name="receivedDate"></param>
        /// <returns></returns>
        public static GoodsReceivingDocumentLine SaveGoodsReceivingDocumentLine(
            RecordIdentifier GoodsReceivingDocumentID,
            RecordIdentifier itemID,
            RecordIdentifier unitID,
            RecordIdentifier lineNumber,
            string StoreID,
            decimal receivedQuantity,
            DateTime receivedDate
            )
        {
            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            GoodsReceivingDocumentLine grdLine = new GoodsReceivingDocumentLine();
            grdLine.GoodsReceivingDocumentID = GoodsReceivingDocumentID;
            grdLine.PurchaseOrderID = (string)service.GetPurchaseOrderID(
                    PluginEntry.DataModel,
                    GetSiteServiceProfile(),
                    grdLine.GoodsReceivingDocumentID, false);

            grdLine.PurchaseOrderLineNumber =
                service.GetPurchaseOrderLineNumberFromItemInfo(
                    PluginEntry.DataModel,
                    GetSiteServiceProfile(),
                    grdLine.PurchaseOrderID,
                    itemID,
                    unitID,
                    false);

            // This means we did not find a matching purchase order line. That should never happen :)
            if (grdLine.PurchaseOrderLineNumber == "0")
            {
                MessageDialog.Show(Resources.PurchaseOrderLineMissing);
                return null;
            }

            grdLine.LineNumber = lineNumber;

            grdLine.StoreID = StoreID;
            grdLine.ReceivedQuantity = receivedQuantity;
            grdLine.ReceivedDate = receivedDate;
            grdLine.Posted = false;

            service.SaveGoodsReceivingDocumentLine(PluginEntry.DataModel, GetSiteServiceProfile(), grdLine, true);

            return grdLine;
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        private static int MaxOverGoodsReceive()
        {
            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {
                return service.MaxOverGoodsReceive(PluginEntry.DataModel, siteServiceProfile, true);
            }
            catch
            {
               Setting systemSettingsMaxOverReceiveGoods = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.MaxOverReceiveGoods, SettingsLevel.System);
               return Convert.ToInt32(systemSettingsMaxOverReceiveGoods.Value);
            }
        }

        public static void DisplayGoodsReceivingPostingResult(GoodsReceivingPostResult result)
        {
            switch (result)
            {
                case GoodsReceivingPostResult.MissingUnitConversion:
                    MessageDialog.Show(Resources.GoodsReceivingUnitConversionRule, Resources.PostGoodsReceiving);
                    break;
                case GoodsReceivingPostResult.InvalidReceivingQuantity:
                    MessageDialog.Show(Resources.GoodsReceivingInvalidReceiveQuantity, Resources.PostGoodsReceiving);
                    break;
                case GoodsReceivingPostResult.NotFound:
                    MessageDialog.Show(Resources.GoodsReceivingNotFound, Resources.PostGoodsReceiving);
                    break;
                case GoodsReceivingPostResult.NoLinesToPost:
                    MessageDialog.Show(Resources.GoodsReceivingNoLinesToPost, Resources.PostGoodsReceiving);
                    break;
                case GoodsReceivingPostResult.AlreadyProcessing:
                    MessageDialog.Show(Resources.GoodsReceivingAlreadyProcessing, Resources.PostGoodsReceiving);
                    break;
                case GoodsReceivingPostResult.AlreadyPosted:
                    MessageDialog.Show(Resources.GoodsReceivingAlreadyPosted, Resources.PostGoodsReceiving);
                    break;
                case GoodsReceivingPostResult.Error:
                    MessageDialog.Show(Resources.PostGoodsReceivingError, Resources.PostGoodsReceiving);
                    break;
            }
        }
    }
}