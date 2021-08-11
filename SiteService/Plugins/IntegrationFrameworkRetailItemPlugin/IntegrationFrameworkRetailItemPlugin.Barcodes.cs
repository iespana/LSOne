using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailItem
{
    public partial class IntegrationFrameworkRetailItemPlugin
    {
        public virtual BarCode GetBarCode(RecordIdentifier barCodeID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.BarCodeData.Get(dataModel, barCodeID);
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

        public virtual List<BarCode> GetBarCodes()
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.BarCodeData.GetAllBarcodes(dataModel);
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

        public virtual SaveResult SaveBarCode(BarCode barcode)
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
                        Providers.BarCodeData.Save(dataModel, barcode);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.ErrorInfos.Add(new ErrorInfo(barcode.ItemBarcodeID.StringValue, barcode.Text, ex.ToString()));
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

        public virtual SaveResult SaveBarCodeList(List<BarCode> barCodes)
        {
            return SaveList(barCodes, Providers.BarCodeData);
        }

        public virtual void DeleteBarCode(RecordIdentifier barCodeID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.BarCodeData.Delete(dataModel, barCodeID);
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