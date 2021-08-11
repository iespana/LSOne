using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailDivisionPlugin
{
    public partial class IntegrationFrameworkRetailDivisionPlugin
    {
        public virtual void Save(RetailDivision retailDivision)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.RetailDivisionData.Save(dataModel, retailDivision);
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

        public virtual SaveResult SaveList(List<RetailDivision> retailDivisions)
        {
            return SaveList(retailDivisions, Providers.RetailDivisionData);
        }

        public virtual RetailDivision Get(RecordIdentifier retailDivisionID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.RetailDivisionData.Get(dataModel, retailDivisionID);
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

        public virtual void Delete(RecordIdentifier retailDivisionID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.RetailDivisionData.Delete(dataModel, retailDivisionID);
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
