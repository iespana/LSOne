using System;
using System.Collections.Generic;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkTax
{
    public partial class IntegrationFrameworkTaxPlugin
    {
        public virtual ItemSalesTaxGroup GetItemSalesTaxGroup(RecordIdentifier itemSalesTaxGroupID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.ItemSalesTaxGroupData.Get(dataModel, itemSalesTaxGroupID);
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

        public virtual List<ItemSalesTaxGroup> GetItemSalesTaxGroups()
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.ItemSalesTaxGroupData.GetItemSalesTaxGroups(dataModel, ItemSalesTaxGroup.SortEnum.ID, false);
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

        public virtual SaveResult SaveItemSalesTaxGroup(ItemSalesTaxGroup itemSalesTaxGroup)
        {
            SaveResult result = new SaveResult();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                result.Success = true;
                result.ErrorInfos = new List<ErrorInfo>();

                try
                {
                    try
                    {
                        Providers.ItemSalesTaxGroupData.Save(dataModel, itemSalesTaxGroup);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(itemSalesTaxGroup.ID.StringValue, itemSalesTaxGroup.Text, ex.ToString()));
                    }
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                ThrowChannelError(e);
            }
            finally
            {
                stopwatch.Stop();
                result.ExecutionTime = stopwatch.Elapsed.TotalSeconds;
            }

            return result;
        }

        public virtual SaveResult SaveItemSalesTaxGroupList(List<ItemSalesTaxGroup> itemSalesTaxGroups)
        {
            return SaveList(itemSalesTaxGroups, Providers.ItemSalesTaxGroupData);
        }

        public virtual void DeleteItemSalesTaxGroup(RecordIdentifier itemSalesTaxGroupID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.ItemSalesTaxGroupData.Delete(dataModel, itemSalesTaxGroupID);
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

        public virtual SaveResult AddTaxCodeToItemSalesTaxGroup(TaxCodeInItemSalesTaxGroup item)
        {
            SaveResult result = new SaveResult();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                result.Success = true;
                result.ErrorInfos = new List<ErrorInfo>();

                try
                {
                    AddDefaultStoreCustomerTaxGroup(dataModel);

                    try
                    {
                        Providers.ItemSalesTaxGroupData.AddTaxCodeToItemSalesTaxGroup(dataModel, item);

                        TaxCodeInSalesTaxGroup taxCodeInDefaultGroup = new TaxCodeInSalesTaxGroup
                        {
                            SalesTaxGroup = DefaultStoreTaxGroupID,
                            TaxCode = item.ID
                        };

                        Providers.SalesTaxGroupData.AddTaxCodeToSalesTaxGroup(dataModel, taxCodeInDefaultGroup);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(item.ID.StringValue, item.Text, ex.ToString()));
                    }
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                ThrowChannelError(e);
            }
            finally
            {
                stopwatch.Stop();
                result.ExecutionTime = stopwatch.Elapsed.TotalSeconds;
            }

            return result;
        }

        public virtual SaveResult AddTaxCodeToItemSalesTaxGroupList(List<TaxCodeInItemSalesTaxGroup> items)
        {
            SaveResult result = new SaveResult();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                result.Success = true;
                result.ErrorInfos = new List<ErrorInfo>();

                try
                {
                    AddDefaultStoreCustomerTaxGroup(dataModel);

                    foreach (var item in items)
                    {
                        try
                        {
                            Providers.ItemSalesTaxGroupData.AddTaxCodeToItemSalesTaxGroup(dataModel, item);

                            TaxCodeInSalesTaxGroup taxCodeInDefaultGroup = new TaxCodeInSalesTaxGroup
                            {
                                SalesTaxGroup = DefaultStoreTaxGroupID,
                                TaxCode = item.ID
                            };

                            Providers.SalesTaxGroupData.AddTaxCodeToSalesTaxGroup(dataModel, taxCodeInDefaultGroup);
                        }
                        catch (Exception ex)
                        {
                            result.Success = false;
                            result.ErrorInfos.Add(new ErrorInfo(item.ID.StringValue, item.Text, ex.ToString()));
                        }
                    }
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                ThrowChannelError(e);
            }
            finally
            {
                stopwatch.Stop();
                result.ExecutionTime = stopwatch.Elapsed.TotalSeconds;
            }

            return result;
        }

        public virtual void RemoveTaxCodeFromItemSalesTaxGroup(TaxCodeInItemSalesTaxGroup item)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {                    
                    Providers.ItemSalesTaxGroupData.RemoveTaxCodeFromItemSalesTaxGroup(dataModel, new RecordIdentifier((string)item.TaxItemGroup, (string)item.TaxCode));
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
    }
}
