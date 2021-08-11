using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual void DeleteImage(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier pictureID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteImage(CreateLogonInfo(entry), pictureID), closeConnection);
        }

        public virtual void DeleteImageList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> pictureIDs, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteImageList(CreateLogonInfo(entry), pictureIDs), closeConnection);
        }
    }
}