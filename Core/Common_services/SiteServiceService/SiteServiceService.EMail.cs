using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public bool IsEMailSetupForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, bool useCentralDatbase)
        {
            try
            {
                if (useCentralDatbase && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (useCentralDatbase)
                {
                    return server.IsEMailSetupForStore(storeID, CreateLogonInfo(entry));
                }

                var setting = Providers.EMailSettingData.Get(entry, storeID);
                return setting != null && ((string)setting.StoreID == (string)storeID);
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public EMailSetting GetEMailSetupForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, bool useCentralDatabase)
        {
            try
            {
                if (useCentralDatabase && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (useCentralDatabase)
                {
                    return server.GetEMailSetupForStore(storeID, CreateLogonInfo(entry));
                }

                return Providers.EMailSettingData.Get(entry, storeID);
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public void SaveEMailSetupForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, EMailSetting setting, bool useCentralDatabase)
        {
            try
            {
                if (useCentralDatabase && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (useCentralDatabase)
                {
                    server.SaveEMailSetupForStore(setting, CreateLogonInfo(entry));
                }
                else
                {
                    Providers.EMailSettingData.Save(entry, setting);
                }
                
                
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public void QueueEMailEntry(IConnectionManager entry, SiteServiceProfile siteServiceProfile, EMailQueueEntry emailQueue, List<EMailQueueAttachment> attachments, bool useCentralDatabase)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (useCentralDatabase)
                {
                    server.QueueEMailEntry(emailQueue, attachments, CreateLogonInfo(entry));
                }
                else
                {
                    Providers.EMailQueueEntryData.Save(entry, emailQueue);

                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (var attachment in attachments)
                        {
                            attachment.EMailID = emailQueue.EMailID;
                            Providers.EMailQueueAttachmentData.Save(entry, attachment);
                        }
                    }
                }

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public void SendQueuedEMailEntries(IConnectionManager entry, SiteServiceProfile siteServiceProfile, int maximumEntries, int maximumAttempts)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }
                
                server.SendQueuedEMailEntries(maximumEntries, maximumAttempts);
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public int GetEMailCount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool unsentOnly)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                return server.GetEMailCount(unsentOnly, CreateLogonInfo(entry));
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public List<EMailQueueEntry> GetEMails(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool unsentOnly, int index, int maxEntries, EMailSortEnum sort)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                return server.GetEMails(unsentOnly, index, maxEntries, sort, CreateLogonInfo(entry));
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public EMailQueueEntry GetEMail(IConnectionManager entry, SiteServiceProfile siteServiceProfile, int ID)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                return server.GetEMail(ID, CreateLogonInfo(entry));
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public void TruncateEMailQueue(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DateTime createdBefore)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                server.TruncateEMailQueue(createdBefore, CreateLogonInfo(entry));
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }
    }
}