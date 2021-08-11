using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for POS Users.
    /// </summary>
    public class UserGroupData : SqlServerDataProviderBase, IUserGroupData
    {
        /// <summary>
        /// Get id and name of selected Pos Users from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of Pos Users id and name</returns>
        public virtual List<DataEntity> GetPosUserPermissionGroupList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "USERGROUPS", "NAME", "GUID", "NAME");
        }

        /// <summary>
        /// Get pos user group list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>list of users </returns>
        public virtual List<UserGroup> GetPosUserGroupList(IConnectionManager entry, RecordIdentifier id)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(ID, '') as ID,
                                    ISNULL(PERMISSIONGROUPID, '') as PERMISSIONGROUPID,
                                    ISNULL(NAME, '') AS NAME 
                                    FROM WIZARDTEMPLATEPERMISSION 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<UserGroup>(entry, cmd, CommandType.Text, null, PopulatePosUsersGroupsItems);
            }
        }

        private static void PopulatePosUsersGroupsItems(IConnectionManager entry, IDataReader dr, UserGroup userGroup, object obj)
        {
            userGroup.PermissionGroupID = (string)dr["PERMISSIONGROUPID"];

            userGroup.ID = (string)dr["ID"];
            userGroup.Text = (string) dr["NAME"];
        }

        /// <summary>
        /// Save all selected users into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posUserList">userIds</param>
        public virtual void SavePosUserGroups(IConnectionManager entry, List<UserGroup> posUserList)
        {
            Delete(entry, posUserList.First().ID);

            foreach (var group in posUserList)
            {
                if (group.PermissionGroupID != string.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATEPERMISSION")
                        {
                            StatementType = StatementType.Insert
                        };

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("ID", (string)group.ID);
                    statement.AddField("NAME", group.Text);

                    statement.AddField("PERMISSIONGROUPID", group.PermissionGroupID, SqlDbType.NVarChar);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">Table name</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATEPERMISSION")
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.SecurityEditUser);
        }
    }
}
