using System;
using System.Linq;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.EMail;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual bool IsEMailSetupForStore(RecordIdentifier storeID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");

                EMailSetting setting = Providers.EMailSettingData.Get(dataModel, storeID);
                bool isEmailConfigured = setting != null && (0 == string.Compare((string)setting.StoreID, (string)storeID));

                Utils.Log(this, isEmailConfigured ? "Email is configured for store" : "Email is not configured for store");

                return isEmailConfigured;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return false;
        }

        public virtual EMailSetting GetEMailSetupForStore(RecordIdentifier storeID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");

                return Providers.EMailSettingData.Get(dataModel, storeID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
            return null;
        }

        public virtual void SaveEMailSetupForStore(EMailSetting setting, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.EMailSettingData.Save(dataModel, setting);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void QueueEMailEntry(EMailQueueEntry entry, List<EMailQueueAttachment> attachments, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting + " EmailID: " + entry.EMailID);
                Providers.EMailQueueEntryData.Save(dataModel, entry);

                if (attachments != null && attachments.Count > 0)
                {
                    foreach (EMailQueueAttachment attachment in attachments)
                    {
                        attachment.EMailID = entry.EMailID;
                        Providers.EMailQueueAttachmentData.Save(dataModel, attachment);
                    }
                    Utils.Log(this, "Attachments saved");

                }
                else
                {
                    Utils.Log(this, "No attachments received");
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void SendQueuedEMailEntries(int maximumEntries, int maximumAttempts)
        {
            IConnectionManager dataModel = GetConnectionManager();

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(maximumEntries)}: {maximumEntries}, {nameof(maximumAttempts)}: {maximumAttempts}");

                if (maximumEntries < 5)
                {
                    maximumEntries = 5;
                }
                if (maximumAttempts < 1)
                {
                    maximumAttempts = 1;
                }

                Utils.Log(this, $"{nameof(maximumEntries)}: {maximumEntries}, {nameof(maximumAttempts)}: {maximumAttempts}");

                int sentMails = 0;
                int failures = 0;
                Dictionary<int, Exception> errors = new Dictionary<int, Exception>();

                List<EMailQueueEntry> unsentMails = Providers.EMailQueueEntryData.GetList(dataModel, true, maximumEntries, maximumAttempts);

                if (unsentMails == null || !unsentMails.Any())
                {
                    Utils.Log(this, "No unsent emails found");
                }

                EMailSetting genericEmailSettings = Providers.EMailSettingData.Get(dataModel, RecordIdentifier.Empty);

                if (genericEmailSettings == null)
                {
                    Utils.Log(this, "No generic email settings found");
                }

                EMailSetting storeEmailSettings = new EMailSetting();
                SmtpServerData smtpServerData = null;

                foreach (EMailQueueEntry unsentMail in unsentMails)
                {
                    Utils.Log(this, "Sending e-mail " + unsentMail.EMailID, LogLevel.Trace);

                    if (storeEmailSettings.StoreID != unsentMail.StoreID)
                    {
                        storeEmailSettings = Providers.EMailSettingData.Get(dataModel, unsentMail.StoreID);

                        //if no email settings were found for the store then use the generic email settings
                        if (storeEmailSettings == null)
                        {
                            Utils.Log(this, "No email settings found for the store, generic email settings used");
                            storeEmailSettings = genericEmailSettings;
                        }

                        if (storeEmailSettings != null)
                        {
                            smtpServerData = storeEmailSettings.ToSmtpServerData();
                        }
                    }

                    List<EMailQueueAttachment> attachments = new List<EMailQueueAttachment>();                    

                    if (smtpServerData != null)
                    {
                        attachments = Providers.EMailQueueAttachmentData.GetList(dataModel, unsentMail.ID);
                    }

                    EMailResult res = EMailSender.Send(smtpServerData, unsentMail.ToEMailMessage(attachments));                    

                    if (res.Success && emailTruncateSettings == EMailTruncateSetting.Each)
                    {
                        Utils.Log(this, "Truncating the email after sending - EmailTruncateSetting = Each");
                        // Delete the email and attachment
                        Providers.EMailQueueEntryData.Delete(dataModel, unsentMail.EMailID);
                    }
                                        
                    // Update e-mail entry
                    Providers.EMailQueueEntryData.Update(dataModel, unsentMail.EMailID,
                                               res.Success ? DateTime.Now : DateTime.MinValue,
                                               unsentMail.Attempts++,
                                               res.Success ? "" : res.ErrorMessage);

                    Utils.Log(this, "Email sent and updated - Sending was " + (res.Success ? "successful" : "unsuccessul"));

                    if (res.Success)
                    {
                        sentMails++;
                    }
                    else if (res.Exception == null)
                    {
                        failures++;
                    }
                    else
                    {
                        errors[(int)unsentMail.ID] = res.Exception;
                    }
                }

                DateTime deleteBefore = DateTime.MinValue;
                switch (emailTruncateSettings)
                {
                    case EMailTruncateSetting.Daily:
                        deleteBefore = DateTime.Now.AddDays(-1);
                        break;
                    case EMailTruncateSetting.Weekly:
                        deleteBefore = DateTime.Now.AddDays(-7);
                        break;
                    case EMailTruncateSetting.Monthly:
                        deleteBefore = DateTime.Now.AddMonths(-1);
                        break;
                    case EMailTruncateSetting.Each:
                    case EMailTruncateSetting.Never:
                        break;
                }
                

                if (deleteBefore != DateTime.MinValue)
                {
                    Providers.EMailQueueEntryData.Truncate(dataModel, deleteBefore);
                    Utils.Log(this, $"Emails truncated after sending - EmailTruncateSetting = {emailTruncateSettings}");
                }

                if (sentMails + failures + errors.Count == 0)
                {
                    Utils.Log(this, "No mail to send", LogLevel.Trace);
                }
                else
                {
                    if (sentMails > 0)
                    {
                        Utils.Log(this, $"Sent {sentMails} mails", LogLevel.Trace);
                    }
                    if (failures > 0)
                    {
                        Utils.Log(this, $"{failures} emails had errors", LogLevel.Trace);
                    }
                    if (errors.Count > 0)
                    {
                        Utils.Log(this, errors.Count + " emails had exception (follow)", LogLevel.Trace);
                        foreach (int id in errors.Keys)
                        {
                            Exception ex = errors[id];
                            Utils.Log(this, "   Email " + id + ": " + ex.Message, LogLevel.Trace);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual int GetEMailCount(bool unsentOnly, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.EMailQueueEntryData.Count(dataModel, unsentOnly);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return 0;
        }

        public virtual List<EMailQueueEntry> GetEMails(bool unsentOnly, int index, int maxEntries, EMailSortEnum sort, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.EMailQueueEntryData.GetIndexedList(dataModel, unsentOnly, index, maxEntries, sort);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        public virtual EMailQueueEntry GetEMail(int ID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(ID)}: {ID}");
                return Providers.EMailQueueEntryData.Get(dataModel, ID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        public virtual void TruncateEMailQueue(DateTime createdBefore, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(createdBefore)}: {createdBefore.ToShortDateString()}");

                Providers.EMailQueueEntryData.Truncate(dataModel, createdBefore);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        internal void SendQueuedEMailEntries()
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                SendQueuedEMailEntries(emailMaximumBatch, emailMaximumAttempts);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw e;
            }
        }
    }
}