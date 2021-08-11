using System;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.Enums;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void SetHardwareProfile(RecordIdentifier terminalID, RecordIdentifier storeID, HardwareProfile profile,
            LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(terminalID)}: {terminalID}, {nameof(storeID)}: {storeID}", LogLevel.Trace);

                if (profile.ID == RecordIdentifier.Empty)
                {
                    Terminal terminal = Providers.TerminalData.Get(dataModel, terminalID, storeID);
                    Providers.HardwareProfileData.Save(dataModel, profile);
                    Providers.TerminalData.SetHardwareProfile(dataModel, terminal, profile.ID);
                }
                else
                {
                    Providers.TerminalData.SetHardwareProfile(dataModel, terminalID, storeID, profile.ID);
                }
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual ActivationResultEnum MarkAsActivated(RecordIdentifier terminalID, RecordIdentifier storeID,
            LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(terminalID)}: {terminalID}, {nameof(storeID)}: {storeID}", LogLevel.Trace);

                Terminal terminal = Providers.TerminalData.Get(dataModel, terminalID, storeID);
                if (terminal.Activated)
                {
                    return ActivationResultEnum.AlreadyActivated;
                }

                Providers.TerminalData.MarkAsActivated(dataModel, terminal);
                return ActivationResultEnum.Success;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                ThrowChannelError(ex);
                return ActivationResultEnum.UnknownError;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual void SetEFTForTerminal(RecordIdentifier terminalID, RecordIdentifier storeID, string ipAddress,
            string eftStoreID, string eftTerminalID, string customField1, string customField2, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(terminalID)}: {terminalID}, {nameof(storeID)}: {storeID}, {nameof(ipAddress)}: {ipAddress}, {nameof(eftStoreID)}: {eftStoreID}, {nameof(eftTerminalID)}: {eftTerminalID}, {nameof(customField1)}: {customField1}, {nameof(customField2)}: {customField2}", LogLevel.Trace);

                Terminal terminal = Providers.TerminalData.Get(dataModel, terminalID, storeID);
                terminal.IPAddress = ipAddress;
                terminal.EftStoreID = eftStoreID;
                terminal.EftTerminalID = eftTerminalID;
                terminal.EftCustomField1 = customField1;
                terminal.EftCustomField2 = customField2;

                Providers.TerminalData.Save(dataModel, terminal);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                ThrowChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual bool VerifyLogin(string userName, string cloudPassword, Guid userID)
        {
            IConnectionManager dataModel = null;
            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);

                string pwd = Cipher.Decrypt(cloudPassword, configurations[SiteServiceConfigurationConstants.PrivateHashKey]);

                dataModel = connectionPool.GetConnection(parameters.Item1,
                    parameters.Item2,
                    parameters.Item3,
                    parameters.Item4,
                    userName,
                    userID,
                    parameters.Rest.Item1,
                    parameters.Rest.Item2,
                    parameters.Rest.Item3);

                if (dataModel != null)
                {
                    bool locked;
                    Utils.Log(this, dataModel.Connection.DatabaseName, LogLevel.Trace);

                    return dataModel.VerifyCredentials(userName, SecureStringHelper.FromString(pwd),
                        out locked);
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw;
            }
            finally 
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return false;
        }
    }
}