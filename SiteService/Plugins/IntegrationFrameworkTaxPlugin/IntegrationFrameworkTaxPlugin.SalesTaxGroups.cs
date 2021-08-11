using System;
using System.Collections.Generic;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkTax
{
    public partial class IntegrationFrameworkTaxPlugin
    {
        public virtual SalesTaxGroup GetSalesTaxGroup(RecordIdentifier salesTaxGroupID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.SalesTaxGroupData.Get(dataModel, salesTaxGroupID);
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

        public virtual List<SalesTaxGroup> GetSalesTaxGroups()
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.SalesTaxGroupData.GetSalesTaxGroups(dataModel, SalesTaxGroup.SortEnum.ID, false);
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

        public virtual SaveResult SaveSalesTaxGroup(SalesTaxGroup salesTaxGroup)
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
                        Providers.SalesTaxGroupData.Save(dataModel, salesTaxGroup);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(salesTaxGroup.ID.StringValue, salesTaxGroup.Text, ex.ToString()));
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

        public virtual SaveResult SaveSalesTaxGroupList(List<SalesTaxGroup> salesTaxGroups)
        {
            return SaveList(salesTaxGroups, Providers.SalesTaxGroupData);
        }

        public virtual void DeleteSalesTaxGroup(RecordIdentifier salesTaxGroupID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.SalesTaxGroupData.Delete(dataModel, salesTaxGroupID);
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

        public virtual SaveResult AddTaxCodeToSalesTaxGroup(TaxCodeInSalesTaxGroup taxCode)
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
                        Providers.SalesTaxGroupData.AddTaxCodeToSalesTaxGroup(dataModel, taxCode);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(taxCode.ID.StringValue, taxCode.Text, ex.ToString()));
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

        public virtual SaveResult AddTaxCodeToSalesTaxGroupList(List<TaxCodeInSalesTaxGroup> taxCodes)
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
                    foreach (var taxCode in taxCodes)
                    {
                        try
                        {
                            Providers.SalesTaxGroupData.AddTaxCodeToSalesTaxGroup(dataModel, taxCode);
                        }
                        catch (Exception ex)
                        {
                            result.Success = false;
                            result.ErrorInfos.Add(new ErrorInfo(taxCode.ID.StringValue, taxCode.Text, ex.ToString()));
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

        public virtual void RemoveTaxCodeFromSalesTaxGroup(TaxCodeInSalesTaxGroup taxCode)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.SalesTaxGroupData.RemoveTaxCodeFromSalesTaxGroup(dataModel, new RecordIdentifier((string)taxCode.SalesTaxGroup, (string)taxCode.TaxCode));
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

        private void AddDefaultStoreCustomerTaxGroup(IConnectionManager dataModel)
        {
            if (!Providers.SalesTaxGroupData.Exists(dataModel, DefaultStoreTaxGroupID))
            {
                SalesTaxGroup defaultSalesTaxGroup = new SalesTaxGroup();
                defaultSalesTaxGroup.ID = DefaultStoreTaxGroupID;
                defaultSalesTaxGroup.Text = DefaultStoreTaxGroupName;
                Providers.SalesTaxGroupData.Save(dataModel, defaultSalesTaxGroup);
            }
        }
    }
}
