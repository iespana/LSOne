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
        public virtual void SaveUnit(Unit unit)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.UnitData.Save(dataModel, unit);
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

        public virtual SaveResult SaveUnitList(List<Unit> units)
        {
            return SaveList(units, Providers.UnitData, Providers.UnitData.Save);
        }

        public virtual Unit GetUnit(RecordIdentifier unitId)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.UnitData.Get(dataModel, unitId);
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

        public virtual void DeleteUnit(RecordIdentifier unitId)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.UnitData.Delete(dataModel, unitId);
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
