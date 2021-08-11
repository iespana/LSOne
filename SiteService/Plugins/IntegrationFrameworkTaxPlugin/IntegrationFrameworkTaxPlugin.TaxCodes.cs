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
        public virtual TaxCode GetTaxCode(RecordIdentifier taxCodeID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.TaxCodeData.Get(dataModel, taxCodeID);
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

        public virtual List<TaxCode> GetTaxCodes()
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.TaxCodeData.GetTaxCodes(dataModel, TaxCode.SortEnum.SalesTaxCode, false);
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

        public virtual SaveResult SaveTaxCode(TaxCode taxcode)
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
                        Providers.TaxCodeData.Save(dataModel, taxcode);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(taxcode.ID.StringValue, taxcode.Text, ex.ToString()));
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

        public virtual SaveResult SaveTaxCodeList(List<TaxCode> taxCodes)
        {
            return SaveList(taxCodes, Providers.TaxCodeData);
        }

        public virtual void DeleteTaxCode(RecordIdentifier taxCodeID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.TaxCodeData.Delete(dataModel, taxCodeID);
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
