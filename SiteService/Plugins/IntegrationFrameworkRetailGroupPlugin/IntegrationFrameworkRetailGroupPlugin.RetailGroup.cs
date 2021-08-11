using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailGroup
{
    public partial class IntegrationFrameworkRetailGroupPlugin
    {
        public virtual void Save(RetailGroup retailGroup)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {                  
                    Providers.RetailGroupData.Save(dataModel, retailGroup);
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

        public virtual SaveResult SaveList(List<RetailGroup> retailGroups)
        {
            return SaveList(retailGroups, Providers.RetailGroupData);
        }

        public virtual RetailGroup Get(RecordIdentifier retailGroupID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.RetailGroupData.Get(dataModel, retailGroupID);
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

        public virtual void Delete(RecordIdentifier retailGroupID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.RetailGroupData.Delete(dataModel, retailGroupID);
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
