using LSOne.ViewPlugins.RMSMigration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.Profiles;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class UserImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;

        private string RMSConnectionString { get; set; }

        private static string InitialPassword = "1234";

        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            UserGroup defaultUserGroup = Providers.UserGroupData.AllGroups(PluginEntry.DataModel).FirstOrDefault();
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            try
            {
                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                List<Guid> userGroups = new List<Guid>();
                if (defaultUserGroup != null)
                {
                    userGroups.Add((Guid)defaultUserGroup.ID);
                }

                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                UserProfile defaultUserProfile = null;
                List<UserProfile> userProfiles = Providers.UserProfileData.GetList(PluginEntry.DataModel);

                if(userProfiles.Count > 0)
                {
                    defaultUserProfile = userProfiles[0];
                }
                else
                {
                    defaultUserProfile = new UserProfile();
                    defaultUserProfile.Text = Properties.Resources.DefaultUserProfileText;
                    Providers.UserProfileData.Save(PluginEntry.DataModel, defaultUserProfile);
                }                

                List<RMSUser> users = entry.Connection.ExecuteReader(Constants.GET_ALL_USERS).ToDataTable().ToList<RMSUser>();
                SetProgressSize(users.Count());
                users.ForEach(u =>
                {
                    try
                    {
                        var newUserInfo = Providers.UserData.New(PluginEntry.DataModel,
                                               u.Login,
                                               InitialPassword,
                                               u.Name,
                                               userGroups,
                                               false,
                                               false,
                                               "");                        

                        POSUser posUser = new POSUser()
                        {
                            ID = newUserInfo.StaffID,
                            Name = u.Name,
                            Text = u.Login,
                            NameOnReceipt = u.Login,
                            Password = InitialPassword,
                            NeedsPasswordChange = true,
                            UserProfileID = defaultUserProfile.ID
                        };

                        Providers.POSUserData.Save(PluginEntry.DataModel, posUser);
                        

                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingUser, u.RMS_ID) + " " + ex.Message });
                        ReportProgress?.Invoke();
                    }
                });
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = ex.Message });
                return logItems;
            }
            return logItems;
        }
    }
}
