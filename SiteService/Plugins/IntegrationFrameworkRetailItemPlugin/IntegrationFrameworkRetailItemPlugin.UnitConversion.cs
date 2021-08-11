using LSOne.DataLayer.BusinessObjects.Units;
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
        public virtual void SaveUnitConversion(UnitConversion unitConversion)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.UnitConversionData.Save(dataModel, unitConversion);
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

        public virtual SaveResult SaveUnitConversionList(List<UnitConversion> unitConversions)
        {
            return SaveList(unitConversions, Providers.UnitConversionData);
        }

        public virtual UnitConversion GetUnitConversion(RecordIdentifier unitConversionId)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.UnitConversionData.Get(dataModel, unitConversionId);
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

        public virtual void DeleteUnitConversion(RecordIdentifier unitConversionId)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.UnitConversionData.Delete(dataModel, unitConversionId);
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