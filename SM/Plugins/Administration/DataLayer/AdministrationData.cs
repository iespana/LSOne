using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.ViewPlugins.Administration.DataLayer
{
    internal class AdministrationData : SqlServerDataProviderBase, IAdministrationData
    {
        public List<ListViewItem> GetSignedActions(IConnectionManager entry, Guid actionID)
        {
            ListViewItem item;
            IDataReader reader;
            List<ListViewItem> items = new List<ListViewItem>();
            string sql;

            ValidateSecurity(entry, Permission.SecurityManageAuditLogs);


            sql = "select a.CreatedOn as [Date], COALESCE(u.Login, '') as [User], COALESCE(a.Reason, '') AS Reason from SIGNEDACTIONS a " +
                  "left join USERS u on a.UserGuid = u.Guid " +
                  "where a.ActionGuid = '" + actionID.ToString() + "' order by a.CreatedOn";

            reader = entry.Connection.ExecuteReader(sql);

            while (reader.Read())
            {
                item = new ListViewItem(((DateTime)reader["Date"]).ToShortDateString());
                item.SubItems.Add((string)reader["User"]);
                item.SubItems.Add((string)reader["Reason"]);

                items.Add(item);
            }

            reader.Close();

            return items;
        }
    }
}



