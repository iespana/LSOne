using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public TableInfo SaveHospitalityTableState(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TableInfo table, bool closeConnection)
        {
            TableInfo result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveHospitalityTableState(table, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public void SaveUnlockedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Guid transactionID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveUnlockedTransaction(transactionID, CreateLogonInfo(entry)), closeConnection);
        }

        public bool ExistsUnlockedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Guid transactionID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ExistsUnlockedTransaction(transactionID, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public void ClearTerminalLocks(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string terminalID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.ClearTerminalLocks(terminalID, CreateLogonInfo(entry)), closeConnection);
        }

        public TableInfo LoadSpecificTableState(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TableInfo table, bool closeConnection)
        {
            TableInfo result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.LoadSpecificTableState(table, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public List<TableInfo> LoadHospitalityTableState(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DiningTableLayout tableLayout, bool closeConnection)
        {
            List<TableInfo> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.LoadHospitalityTableState(tableLayout, CreateLogonInfo(entry)), closeConnection);

            return result;
        }
    }
}
