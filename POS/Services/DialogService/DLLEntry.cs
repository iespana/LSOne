using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    internal class DLLEntry
    {
        internal static IConnectionManager DataModel;        

        internal static ISettings Settings
        {
            get { return DataModel.Settings != null ? 
                (ISettings)DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication) :
                null; }
        }
        private static Dictionary<RecordIdentifier, MasterIDEntity>   retailGroups = new Dictionary<RecordIdentifier, MasterIDEntity>();
        internal static string GetGroupName(RecordIdentifier groupID)
        {
            if (retailGroups.ContainsKey(groupID))
            {
                return retailGroups[groupID].Text;
            }

            List<MasterIDEntity> groups =  Providers.RetailGroupData.GetMasterIDList(DataModel);

            foreach (var dataEntity in groups)
            {
                if (!retailGroups.ContainsKey((Guid) dataEntity.ID))
                {
                    retailGroups.Add((Guid)dataEntity.ID,dataEntity);
                }
                if (retailGroups.ContainsKey(groupID))
                {
                    return retailGroups[groupID].Text;
                }
            }
            return string.Empty;
        }
    }
}
