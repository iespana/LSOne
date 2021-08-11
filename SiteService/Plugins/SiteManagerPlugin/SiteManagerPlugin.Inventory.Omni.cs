using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Plugins.SiteManager.Properties;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
        [LSOneUsage(CodeUsage.LSCommerce)]
        public virtual SendOmniJournalResult SendOmniJournal(LogonInfo logonInfo, OmniJournal omniJournal)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.OmniJournalData.Save(entry, omniJournal);

                Utils.Log(this, $"LS Commerce journal {omniJournal.ID} received. Begin processing lines.");

                if(omniJournal.JournalType == (int)TemplateEntryTypeEnum.GoodsReceiving)
                {
                    ImportOmniJournalLinesFromXML(entry, omniJournal);
                }
                else
                {
                    ImportOmniJournalLinesFromXML(entry, omniJournal);
                }

                return SendOmniJournalResult.Success;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                return SendOmniJournalResult.Error;
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Create an document header based on the template linked to the LS Commerce journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="omniJournal">LS Commerce journal</param>
        /// <param name="createdDocumentID">Out param - contains the created document ID or empty if the document already exists and was not created or there was an error</param>
        /// <returns>Result of the operation</returns>
        [LSOneUsage(CodeUsage.LSCommerce)]
        internal SendOmniJournalResult CreateOmniJournalHeader(IConnectionManager entry, OmniJournal omniJournal, out RecordIdentifier createdDocumentID)
        {
            createdDocumentID = RecordIdentifier.Empty;

            try
            {
                Utils.Log(this, Utils.Starting);

                InventoryTemplate template = Providers.InventoryTemplateData.Get(entry, omniJournal.TemplateID);
                
                if(template == null)
                {
                    Utils.Log(this, $"Inventory template not found for LS Commerce journal {omniJournal.ID}.");
                    return SendOmniJournalResult.TemplateNotFound;
                }

                SendOmniJournalResult result = SendOmniJournalResult.Success;

                switch ((TemplateEntryTypeEnum)omniJournal.JournalType)
                {
                    case TemplateEntryTypeEnum.PurchaseOrder:
                        PurchaseOrder purchaseOrder = Providers.PurchaseOrderData.GetOmniPurchaseOrderByTemplate(entry, omniJournal.TemplateID, omniJournal.StoreID);

                        if (purchaseOrder == null)
                        {
                            Utils.Log(this, "Purchase order header not found. Creating new purchase order header.");

                            purchaseOrder = new PurchaseOrder();
                            purchaseOrder.Description = template.Text;
                            purchaseOrder.StoreID = omniJournal.StoreID;
                            purchaseOrder.TemplateID = omniJournal.TemplateID;
                            purchaseOrder.CreatedFromOmni = true;
                            purchaseOrder.CreatedDate = DateTime.Now;
                            purchaseOrder.OrderingDate = Date.Now;

                            Vendor vendor = Providers.VendorData.Get(entry, template.DefaultVendor);

                            if(vendor == null)
                            {
                                Utils.Log(this, "Vendor was not found for creating purchase order.");
                                result = SendOmniJournalResult.Error;
                                break;
                            }

                            purchaseOrder.CurrencyCode = vendor.CurrencyID;
                            purchaseOrder.VendorID = (string)template.DefaultVendor;
                            purchaseOrder.DeliveryDate = vendor.DeliveryDaysType == DeliveryDaysTypeEnum.Days ? DateTime.Now.AddDays(vendor.DefaultDeliveryTime) : DateTime.Now.AddBusinessDays(vendor.DefaultDeliveryTime);
                            Providers.PurchaseOrderData.Save(entry, purchaseOrder);
                            createdDocumentID = purchaseOrder.ID;
                        }
                        break;
                    case TemplateEntryTypeEnum.TransferStock:
                        if(RecordIdentifier.IsEmptyOrNull(template.DefaultStore))
                        {
                            Utils.Log(this, "Inventory template has no default receiving store for transfer order.");
                            return SendOmniJournalResult.Error;
                        }

                        InventoryTransferOrder transferOrder = Providers.InventoryTransferOrderData.GetOmniTransferOrderByTemplate(entry, omniJournal.TemplateID, omniJournal.StoreID);

                        if(transferOrder == null)
                        {
                            Utils.Log(this, "Transfer order header not found. Creating new transfer order header.");

                            transferOrder = new InventoryTransferOrder();
                            transferOrder.Text = template.Text;
                            transferOrder.SendingStoreId = omniJournal.StoreID;
                            transferOrder.ReceivingStoreId = template.DefaultStore;
                            transferOrder.TemplateID = omniJournal.TemplateID;
                            transferOrder.CreatedFromOmni = true;
                            transferOrder.CreatedBy = omniJournal.StoreID;
                            transferOrder.CreationDate = DateTime.Now;
                            transferOrder.ExpectedDelivery = ExpectedDeliveryDate(entry, DateTime.MinValue, omniJournal.StoreID);
                            Providers.InventoryTransferOrderData.Save(entry, transferOrder);
                            createdDocumentID = transferOrder.ID;
                        }
                        break;
                    case TemplateEntryTypeEnum.StockCounting:
                        InventoryAdjustment stockCountingJournal = Providers.InventoryAdjustmentData.GetOmniInventoryAdjustmentByTemplate(entry, omniJournal.TemplateID, omniJournal.StoreID);

                        if(stockCountingJournal == null)
                        {
                            Utils.Log(this, "Stock counting journal header not found. Creating new stock counting journal header.");

                            stockCountingJournal = new InventoryAdjustment();
                            stockCountingJournal.JournalType = InventoryJournalTypeEnum.Counting;
                            stockCountingJournal.Text = template.Text;
                            stockCountingJournal.StoreId = omniJournal.StoreID;
                            stockCountingJournal.CreatedFromOmni = true;
                            stockCountingJournal.TemplateID = omniJournal.TemplateID;
                            stockCountingJournal.CreatedDateTime = DateTime.Now;
                            stockCountingJournal.PostedDateTime = Date.Empty;
                            Providers.InventoryAdjustmentData.Save(entry, stockCountingJournal);
                            createdDocumentID = stockCountingJournal.ID;
                        }
                        break;
                }

                return result;
            }
            catch(Exception ex)
            {
                Utils.LogException(this, ex);
                return SendOmniJournalResult.Error;
            }
        }

        [LSOneUsage(CodeUsage.LSCommerce)]
        internal void ImportOmniJournalLinesFromXML(IConnectionManager entry, OmniJournal omniJournal)
        {
            try
            {
                Utils.Log(this, Utils.Starting + " " + omniJournal.ID);
                int rowsAdded = 0;

                if((TemplateEntryTypeEnum)omniJournal.JournalType == TemplateEntryTypeEnum.GoodsReceiving)
                {
                    rowsAdded = Providers.OmniJournalData.ImportOmniGoodsReceivingLinesFromXML(entry, omniJournal.ID);
                }
                else 
                {
                    RecordIdentifier createdDocumentID;

                    if (CreateOmniJournalHeader(entry, omniJournal, out createdDocumentID) == SendOmniJournalResult.Success)
                    {
                        rowsAdded = Providers.OmniJournalData.ImportOmniLinesFromXML(entry, omniJournal.ID);

                        if((TemplateEntryTypeEnum)omniJournal.JournalType == TemplateEntryTypeEnum.PurchaseOrder && !RecordIdentifier.IsEmptyOrNull(createdDocumentID))
                        {
                            InventoryTemplate template = Providers.InventoryTemplateData.Get(entry, omniJournal.TemplateID);
                            CreateGoodsReceivingForPurchaseOrderTemplate(entry, template, createdDocumentID);
                        }
                    }
                }

                if (rowsAdded > 0)
                {
                    Utils.Log(this, $"LS Commerce journal {omniJournal.ID} processed {rowsAdded} rows.");
                    Providers.OmniJournalData.Delete(entry, omniJournal.ID);

                    if ((TemplateEntryTypeEnum)omniJournal.JournalType == TemplateEntryTypeEnum.PurchaseOrder)
                    {
                        PurchaseOrder purchaseOrder = Providers.PurchaseOrderData.GetOmniPurchaseOrderByTemplate(entry, omniJournal.TemplateID, omniJournal.StoreID);

                        if (purchaseOrder != null)
                        {
                            Utils.Log(this, $"LS Commerce journal {omniJournal.ID} is a purchase order. Calculating tax for all lines.");
                            IConnectionManagerTemporary temporaryConnection = entry.CreateTemporaryConnection();
                            Task.Run(() => CalculatePurchaseOrderLinesTax(temporaryConnection, purchaseOrder));
                        }
                    }
                }
                else
                {
                    Providers.OmniJournalData.IncrementRetryCounter(entry, omniJournal.ID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                try
                {
                    Providers.OmniJournalData.IncrementRetryCounter(entry, omniJournal.ID);
                }
                catch(Exception innnerEx)
                {
                    Utils.LogException(this, innnerEx);
                }
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        [LSOneUsage(CodeUsage.LSCommerce)]
        internal void ProcessOmniJournalJob()
        {
            IConnectionManager entry = GetConnectionManager();

            try
            {
                Utils.Log(this, Utils.Starting);

                List<OmniJournal> omniJournals = Providers.OmniJournalData.GetOmniJournals(entry, OmniJournalStatus.Received);

                foreach(OmniJournal journal in omniJournals.Where(x => x.RetryCounter < maxRetryCounter))
                {
                    ImportOmniJournalLinesFromXML(entry, journal);
                }
            }
            catch (Exception e)
            {
                if(entry == null)
                {
                    Utils.Log(this, "Connection manager is null. Make sure SiteManagerUser and password are correctly configured in the Site Service configuation file.", LogLevel.Error);
                }

                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        [LSOneUsage(CodeUsage.LSCommerce)]
        public virtual void AddInventoryJournalLineImage(LogonInfo logonInfo, InventoryEnum lineType, string templateID, string omniTransactionID, string omniLineID, string image, string imageDescription)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}, {nameof(omniTransactionID)}: {omniTransactionID}, {nameof(omniLineID)}: {omniLineID}");

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                LSOne.DataLayer.BusinessObjects.Images.Image imageBankImage = new LSOne.DataLayer.BusinessObjects.Images.Image();
                imageBankImage.ImageType = LSOne.DataLayer.BusinessObjects.Images.ImageTypeEnum.Inventory;
                imageBankImage.Text = imageDescription;
                
                imageBankImage.Picture = LSOne.Utilities.GUI.ImageUtils.ByteArrayToImage(Convert.FromBase64String(image));

                Providers.ImageData.Save(dataModel, imageBankImage);

                switch (lineType)
                {
                    case InventoryEnum.PurchaseOrder:
                        Providers.PurchaseOrderLineData.SetPictureIDForOmniLine(dataModel, omniTransactionID, omniLineID, imageBankImage.ID);
                        break;
                    case InventoryEnum.StockCounting:
                        Providers.InventoryJournalTransactionData.SetPictureIDForOmniLine(dataModel, omniTransactionID, omniLineID, imageBankImage.ID);
                        break;
                    case InventoryEnum.GoodsReceiving:
                        throw new NotSupportedException($"Type {nameof(lineType)} is not supported");
                    case InventoryEnum.InventoryJournal:
                        throw new NotSupportedException($"Type {nameof(lineType)} is not supported");
                    case InventoryEnum.StoreTransfer:
                        Providers.InventoryTransferOrderLineData.SetPictureIDForOmniLine(dataModel, omniTransactionID, omniLineID, imageBankImage.ID);
                        break;
                }                


                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
                    dataModel.Connection.RestoreContext();
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}