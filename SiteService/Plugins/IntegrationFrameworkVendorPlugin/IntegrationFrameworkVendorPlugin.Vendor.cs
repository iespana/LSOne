using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkVendorPlugin
{
    public partial class IntegrationFrameworkVendorPlugin
    {
        public virtual void Save(Vendor vendor)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.VendorData.Save(dataModel, vendor);
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

        public virtual SaveResult SaveList(List<Vendor> vendors)
        {
            return SaveList(vendors, Providers.VendorData);
        }

        public virtual Vendor Get(RecordIdentifier vendorID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.VendorData.Get(dataModel, vendorID);
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

        public virtual bool Delete(RecordIdentifier vendorID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    List<RecordIdentifier> vendorIdList = new List<RecordIdentifier> {vendorID};
                    List<RecordIdentifier> linkedVendors = new List<RecordIdentifier>();

                    Providers.VendorData.DeleteVendorsCanExecute(dataModel, vendorIdList, out linkedVendors);

                    if (linkedVendors.Count > 0)
                        return false;

                    bool hasItems = Providers.VendorItemData.VendorHasItems(dataModel, vendorID);
                    bool isDefault = Providers.VendorItemData.VendorIsDefaultVendor(dataModel, vendorID);

                    if (hasItems || isDefault)
                        return false;

                    Providers.VendorData.Delete(dataModel, vendorID);
                    return true;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return false;
            }
        }
    }
}
