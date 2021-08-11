using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        [LSOneUsage(CodeUsage.LSCommerce)]
        public virtual SendOmniJournalResult SendOmniJournal(IConnectionManager entry, SiteServiceProfile siteServiceProfile, OmniJournal omniJournal, bool closeConnection)
        {
            SendOmniJournalResult result = SendOmniJournalResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SendOmniJournal(CreateLogonInfo(entry), omniJournal), closeConnection);
            return result;
        }

        [LSOneUsage(CodeUsage.LSCommerce)]
        public virtual void AddInventoryJournalLineImage(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryEnum lineType, string templateID, string omniTransactionID, string omniLineID, string image, string imageDescription, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.AddInventoryJournalLineImage(CreateLogonInfo(entry), lineType, templateID, omniTransactionID, omniLineID, image, imageDescription), closeConnection);
        }
    }
}
