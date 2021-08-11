using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System;
using System.Collections.Generic;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailDepartment
{
    public partial class IntegrationFrameworkRetailDepartmentPlugin
    {
        public virtual void Save(RetailDepartment retailDepartment)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.RetailDepartmentData.Save(dataModel, retailDepartment);
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

        public virtual SaveResult SaveList(List<RetailDepartment> retailDepartments)
        {
            return SaveList(retailDepartments, Providers.RetailDepartmentData);
        }

        public virtual RetailDepartment Get(RecordIdentifier retailDepartmentID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.RetailDepartmentData.Get(dataModel, retailDepartmentID);
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

        public virtual void Delete(RecordIdentifier retailDepartmentID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.RetailDepartmentData.Delete(dataModel, retailDepartmentID);
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
