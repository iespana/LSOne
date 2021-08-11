using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
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
        public static void ShowPurchaseOrderWizard(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                NewPurchaseOrderWizard();
            }
        }

        public static void ShowPurchaseOrders(RecordIdentifier purchaseOrderID)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.PurchaseOrdersView(purchaseOrderID));
            }
        }

        public static void ShowPurchaseOrdersView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PurchaseOrdersView());
        }

        public static void NewPurchaseOrderWizard()
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
            {
                InventoryTypeAction inventoryTypeAction = new InventoryTypeAction { InventoryType = InventoryEnum.PurchaseOrder };

                InventoryWizard dlg = new InventoryWizard(PluginEntry.DataModel, inventoryTypeAction);

                PluginEntry.Framework.SuspendSearchBarClosing();

                dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();
            }
        }

        public static void ShowPurchaseOrder(RecordIdentifier purchaseOrderID)
        {
            if(TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.PurchaseOrderView(purchaseOrderID));
            }
        }

        public static void ShowPurchaseOrder(object sender, EventArgs args)
        {
            ContextBarClickEventArguments a = (ContextBarClickEventArguments)args;
            RecordIdentifier purchaseOrderID = a.Item.Key;

            ShowPurchaseOrder(purchaseOrderID);
        }

        public static PurchaseOrderLinesDeleteResult DeletePurchaseOrder(RecordIdentifier purchaseOrderID)
        {
            PurchaseOrderLinesDeleteResult deleteResult = PurchaseOrderLinesDeleteResult.GoodsReceivingLinesExist;
            SiteServiceProfile siteServiceProfile = GetSiteServiceProfile();
            try
            {
                if (QuestionDialog.Show(Resources.DeletePurchaseOrderQuestion, Resources.DeletePurchaseOrder) == DialogResult.Yes)
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);


                    deleteResult = service.DeletePurchaseOrder(PluginEntry.DataModel, siteServiceProfile, purchaseOrderID, true);

                    if (deleteResult == PurchaseOrderLinesDeleteResult.GoodsReceivingLinesExist)
                    {
                        MessageDialog.Show(Resources.PurchaseOrderHasPostedGoodsReceivingLines);
                    }
                }
                return deleteResult;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            if (deleteResult == PurchaseOrderLinesDeleteResult.CanBeDeleted)
            {
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
            }

            return deleteResult;
        }

        public static List<PurchaseOrderLine> DeletePurchaseOrderLine(List<PurchaseOrderLine> linesToDelete)
        {
            List<PurchaseOrderLine> notDeleted = new List<PurchaseOrderLine>();
            try
            {
                SiteServiceProfile siteServiceProfile = GetSiteServiceProfile();

                if (linesToDelete.Count == 1)
                {
                    PurchaseOrderLine line = linesToDelete.FirstOrDefault();
                    bool hasPostedGRDLs = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                        PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(
                            PluginEntry.DataModel,
                            siteServiceProfile,
                            line.ID,
                            false);

                    if (hasPostedGRDLs)
                    {
                        MessageDialog.Show(Resources.PurchaseOrderLineHasPostedGoodsReceivingLine);
                        return new List<PurchaseOrderLine>();
                    }
                }

                if (QuestionDialog.Show(
                    Resources.DeletePurchaseOrderLineQuestionPlural + " " +
                    Resources.ThisWillDeleteAnyGRLinesAssociatedWithPOLines,
                    Resources.DeletePurchaseOrderLine) == DialogResult.No)
                {
                    return new List<PurchaseOrderLine>();
                }

                foreach (PurchaseOrderLine line in linesToDelete)
                {
                    if (!Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeletePurchaseOrderLine(PluginEntry.DataModel, siteServiceProfile, line.ID, false))
                    {
                        notDeleted.Add(line);
                    }
                }

                if (notDeleted.Count > 0)
                {
                    MessageDialog.Show(Resources.NotAllSelectedLinesCouldBeDeleted, Resources.DeletePurchaseOrderLine);
                }

                return notDeleted;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message, Resources.DeletePurchaseOrderLine);
                return notDeleted;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static DevExpress.XtraReports.UI.XtraReport GetPurchaseOrderReport(RecordIdentifier purchaseOrderID)
        {
            try
            {
                PurchaseOrder purchaseOrder = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                        GetPurchaseOrderWithReportFormatting(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseOrderID,
                            false);
                List<PurchaseOrderLine> purchaseOrderLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                        GetPurchaseOrderLinesWithReportFormatting(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseOrderID, purchaseOrder.StoreID,
                            false);

                CompanyInfo companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel, true);
                List<PurchaseOrderMiscCharges> purchaseOrderMiscCharges =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                        GetMischChargesForPurchaseOrderWithSorting(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseOrderID,
                            PurchaseOrderMiscChargesSorting.Type,
                            false,
                            true,
                            false);

                PurchaseOrderReport report = new PurchaseOrderReport(
                    purchaseOrder,
                    purchaseOrderLines,
                    purchaseOrderMiscCharges,
                    companyInfo);

                decimal totalPOLinesPrice = (from pol in purchaseOrderLines
                                             select pol.TotalPrice).Sum();

                decimal totalPOMischChargesPrice = (from pom in purchaseOrderMiscCharges
                                                    select pom.Amount + pom.TaxAmount).Sum();

                decimal totalPOPrice = totalPOLinesPrice + totalPOMischChargesPrice;

                DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                PurchaseOrderLine line = purchaseOrderLines.FirstOrDefault();
                if (line != null)
                {
                    priceLimiter = line.PriceLimiter;
                }
                string totalPriceString = totalPOPrice.FormatWithLimits(priceLimiter);
                report.TotalPrice = totalPriceString + " " + (string)purchaseOrder.CurrencyCode;

                return report;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return null;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static RecordIdentifier CreatePurchaseOrderFromFilter(PurchaseOrder purchaseOrderHeader, InventoryTemplateFilterContainer filter)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newOrderID = RecordIdentifier.Empty;

                CreatePurchaseOrderResult result = inventoryService.CreatePurchaseOrderFromFilter(PluginEntry.DataModel, GetSiteServiceProfile(), purchaseOrderHeader, filter, ref newOrderID, true);

                switch (result)
                {
                    case CreatePurchaseOrderResult.Success:
                        return newOrderID;

                    default:
                        CreatePurchaseOrderMessage(result);
                        break;
                }

                return newOrderID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static RecordIdentifier CreatePurchaseOrderFromTemplate(PurchaseOrder purchaseOrderHeader, TemplateListItem template)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newOrderID = RecordIdentifier.Empty;

                CreatePurchaseOrderResult result = inventoryService.CreatePurchaseOrderFromTemplate(PluginEntry.DataModel, GetSiteServiceProfile(), purchaseOrderHeader, template, ref newOrderID, true);

                switch (result)
                {
                    case CreatePurchaseOrderResult.Success:
                        return newOrderID;

                    default:
                        CreatePurchaseOrderMessage(result);
                        break;
                }

                return newOrderID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private static void CreatePurchaseOrderMessage(CreatePurchaseOrderResult result)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case CreatePurchaseOrderResult.PurchaseOrderNotFound:
                    MessageDialog.Show(Resources.UnableToCreatePurchaseOrder + CRLF + Resources.PurchaseOrderToBeCopiedCouldNotBeFound);
                    break;
                case CreatePurchaseOrderResult.TemplateNotFound:
                    MessageDialog.Show(Resources.UnableToCreatePurchaseOrder + CRLF + Resources.PurchaseOrderTemplatedNotBeFound);
                    break;
                case CreatePurchaseOrderResult.ErrorCreatingPurchaseOrder:
                    MessageDialog.Show(Resources.UnableToCreatePurchaseOrder);
                    break;
                default:
                    break;
            }
        }
    }
}