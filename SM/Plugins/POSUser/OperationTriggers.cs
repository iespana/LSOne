using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.ViewPlugins.POSUser
{
    internal class OperationTriggers
    {
        internal static void TriggerHandler(object sender, OperationTriggerArguments args)
        {
            if (args.OperationName == "CopyUser")
            {
                CopyUser(args.ID, (RecordIdentifier)args.Param1);
            }

            if (args.OperationName == "UserChanged")
            {
                DataLayer.BusinessObjects.UserManagement.POSUser posUser = Providers.POSUserData.Get(PluginEntry.DataModel, ((User) args.Param1).StaffID, UsageIntentEnum.Normal);
                posUser.Text = PluginEntry.DataModel.Settings.NameFormatter.Format(((User) args.Param1).Name);
                Guid context = Guid.NewGuid();
                PluginEntry.DataModel.BeginPermissionOverride(context, new HashSet<string>() { DataLayer.BusinessObjects.Permission.SecurityEditUser });
                Providers.POSUserData.Save(PluginEntry.DataModel, posUser);
                PluginEntry.DataModel.EndPermissionOverride(context);
            }
        }

        internal static void CopyUser(RecordIdentifier newUserId, RecordIdentifier oldUserId)
        {
            User oldUser = Providers.UserData.Get(PluginEntry.DataModel, (Guid) oldUserId);
            User newUser = Providers.UserData.Get(PluginEntry.DataModel, (Guid) newUserId);
            DataLayer.BusinessObjects.UserManagement.POSUser posUser = Providers.POSUserData.Get(PluginEntry.DataModel, oldUser.StaffID, UsageIntentEnum.Normal);
            posUser.ID = newUser.StaffID;
            posUser.NameOnReceipt = newUser.Name.Last.Length <= 15 ? newUser.Name.Last : newUser.Name.Last.Left(15);
            posUser.Text = PluginEntry.DataModel.Settings.NameFormatter.Format(newUser.Name);
            Guid context = Guid.NewGuid();
            PluginEntry.DataModel.BeginPermissionOverride(context, new HashSet<string>(){DataLayer.BusinessObjects.Permission.SecurityEditUser});
            Providers.POSUserData.Save(PluginEntry.DataModel, posUser);
            PluginEntry.DataModel.EndPermissionOverride(context);
        }        
    }
}
