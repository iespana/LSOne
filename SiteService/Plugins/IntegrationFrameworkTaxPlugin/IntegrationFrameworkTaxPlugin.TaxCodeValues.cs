using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkTax
{
    public partial class IntegrationFrameworkTaxPlugin
    {
        public virtual SaveResult SaveTaxCodeValue(TaxCodeValue taxCodeValue, bool overwriteExistingRange)
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
                        List<TaxCodeValue> existingValues = Providers.TaxCodeValueData.GetTaxCodeValues(dataModel, taxCodeValue.TaxCode, TaxCodeValue.SortEnum.FromDate, false);

                        foreach (TaxCodeValue existingValue in existingValues)
                        {                           
                            if(existingValue.RangeIntersects(taxCodeValue))
                            {
                                if (overwriteExistingRange)
                                {
                                    taxCodeValue.ID = existingValue.ID;
                                }
                                else
                                {
                                    result.Success = false;
                                    result.ErrorInfos.Add(new ErrorInfo(taxCodeValue.ID.StringValue, taxCodeValue.Text, "Another tax code value covering this period already exists."));
                                }
                            }
                        }

                        if (result.Success)
                        {
                            Providers.TaxCodeValueData.Save(dataModel, taxCodeValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(taxCodeValue.ID.StringValue, taxCodeValue.TaxCode.StringValue, ex.ToString()));
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

        public virtual SaveResult SaveTaxCodeValueList(List<TaxCodeValue> taxCodeValues, bool overwriteExistingRange)
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
                    List<TaxCodeValue> allTaxCodeValues = Providers.TaxCodeValueData.GetList(dataModel);
                    bool saveValue;

                    foreach (var taxCodeValue in taxCodeValues)
                    {
                        saveValue = true;

                        try
                        {
                            TaxCodeValue existingValue = allTaxCodeValues.FirstOrDefault(p => p.TaxCode == taxCodeValue.TaxCode && p.RangeIntersects(taxCodeValue));

                            if (existingValue != null)
                            {
                                if (existingValue.RangeIntersects(taxCodeValue))
                                {
                                    if (overwriteExistingRange)
                                    {
                                        taxCodeValue.ID = existingValue.ID;
                                    }
                                    else
                                    {
                                        saveValue = false;
                                        result.Success = false;
                                        result.ErrorInfos.Add(new ErrorInfo(taxCodeValue.ID.StringValue, taxCodeValue.TaxCode.StringValue, "Another tax code value covering this period already exists."));
                                    }                                    
                                }
                            }

                            if (saveValue && 
                                (
                                    existingValue == null ||
                                    taxCodeValue.FromDate != existingValue.FromDate ||
                                    taxCodeValue.ToDate != existingValue.ToDate ||
                                    taxCodeValue.Value != existingValue.Value
                                ))
                            {
                                Providers.TaxCodeValueData.Save(dataModel, taxCodeValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Success = false;
                            result.ErrorInfos.Add(new ErrorInfo(taxCodeValue.ID.StringValue, taxCodeValue.TaxCode.StringValue, ex.ToString()));
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

        public virtual void DeleteTaxCodeValue(RecordIdentifier taxCodeValueID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.TaxCodeValueData.Delete(dataModel, taxCodeValueID);
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
