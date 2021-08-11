using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Scheduler;
using LSRetail.DD.Common;

namespace LSOne.DataLayer.SqlDDDataProviders
{
    public class LocationData : SqlServerDataProvider, ILocationData
    {
        private static string BaseGetString
        {
            get { return @"
                    SELECT 
                        Id,
                        Name,
                        EXDATAAREAID,
                        ExCode,
                        DatabaseDesign,
                        LocationKind,
                        DefaultOwner,
                        DDHost,
                        DDPort,
                        DDNetMode,
                        Enabled,
                        Company,
                        UserId,
                        Password,
                        DBServerIsUsed,
                        DBServerHost,
                        DBPathName,
                        DBDriverType,
                        DBConnectionString,
                        SystemTag
                      FROM JscLocations a"; }
        }

        #region Populators

        public void PopulateDatabaseDesign(IConnectionManager entry, IDataReader dr, JscDatabaseDesign databaseDesign, object param)
        {
            databaseDesign.Description = (string) dr["Description"];
            databaseDesign.CodePage = DBNull.Value == dr["CodePage"] ? null : (int?) dr["CodePage"];
            databaseDesign.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            databaseDesign.ID = (Guid) dr["Id"];

        }

        public void PopulateLocationNormal(IConnectionManager entry, IDataReader dr, JscLocation location, object param)
        {
            PopulateLocation(dr, location);
        }

        public void PopulateLocation(IDataReader dr, JscLocation location)
        {
            location.Text = (string)dr["Name"];
            location.ID = (Guid) dr["ID"];
            location.ExDataAreaId = DBNull.Value == dr["EXDATAAREAID"] ? string.Empty : (string) dr["EXDATAAREAID"];
            location.ExCode = DBNull.Value == dr["ExCode"] ? string.Empty : (string) dr["ExCode"];
            if (DBNull.Value != dr["DatabaseDesign"])
            {
                location.DatabaseDesign = (Guid) dr["DatabaseDesign"];
            }

            location.LocationKind = (LocationKind) (Int16) dr["LocationKind"];
            if (DBNull.Value != dr["DefaultOwner"])
            {
                location.DefaultOwner = (Guid) dr["DefaultOwner"];
            }
            location.DDHost = DBNull.Value == dr["DDHost"] ? string.Empty : (string) dr["DDHost"];
            location.DDPort = DBNull.Value == dr["DDPort"] ? string.Empty : (string) dr["DDPort"];
            location.DDNetMode = (NetMode) (Int16) dr["DDNetMode"];
            location.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            location.Company = DBNull.Value == dr["Company"] ? string.Empty : (string) dr["Company"];
            location.UserId = DBNull.Value == dr["UserId"] ? string.Empty : (string) dr["UserId"];
            location.Password = DBNull.Value == dr["Password"] ? string.Empty : (string) dr["Password"];
            location.DBServerIsUsed = (Convert.ToInt16(dr["DBServerIsUsed"]) != 0);
            location.DBServerHost = DBNull.Value == dr["DBServerHost"] ? string.Empty : (string) dr["DBServerHost"];
            location.DBPathName = DBNull.Value == dr["DBPathName"] ? string.Empty : (string) dr["DBPathName"];
            if (DBNull.Value != dr["DBDriverType"])
            {
                location.DBDriverType = (Guid) dr["DBDriverType"];
            }

            location.DBConnectionString = DBNull.Value == dr["DBConnectionString"]
                                              ? string.Empty
                                              : (string) dr["DBConnectionString"];
            location.SystemTag = DBNull.Value == dr["SystemTag"] ? string.Empty : (string) dr["SystemTag"];
            if (DBNull.Value != dr["DBDriverType"])
            {
                location.DBDriverType = (string) dr["DBDriverType"];
            }

            location.DBServerHost = DBNull.Value == dr["DBServerHost"] ? string.Empty : (string) dr["DBServerHost"];
        }

        public void PopulateLocationMember(IDataReader dr, JscLocationMember locationMember)
        {
            locationMember.OwnerLocation = (Guid) dr["OwnerLocation"];
            locationMember.MemberLocation = (Guid) dr["MemberLocation"];
            locationMember.Sequence = (int) dr["Sequence"];
        }

        private static void PopulateDriverType(IDataReader dr, JscDriverType driverType)
        {

            driverType.Name = (string) dr["Name"];
            driverType.ConnectionStringFormat = (string) dr["ConnectionStringFormat"];
            driverType.DatabaseParams = dr["DatabaseParams"] == DBNull.Value
                                            ? string.Empty
                                            : (string) dr["DatabaseParams"];
            driverType.DatabaseServerType = (DataSrvType) (Int16) dr["DatabaseServerType"];
            driverType.EnabledFields = (string) dr["EnabledFields"];
            driverType.Id = (Guid) dr["ID"];

        }

        #endregion

        #region Member management, Save and Delete

        public void AddMembers(IConnectionManager entry, JscLocation ownerLocation, IEnumerable<JscLocation> locations)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

            int sequence = GetMaxMemberSequence(entry, ownerLocation);
            foreach (JscLocation location in locations)
            {
                JscLocationMember member = new JscLocationMember();
                member.OwnerLocation = ownerLocation.ID;
                member.MemberLocation = location.ID;
                member.Sequence = ++sequence;
                SaveMember(entry, member);
            }
        }

        public void AddParents(IConnectionManager entry, JscLocation memberLocation, IEnumerable<JscLocation> locations)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

            
            foreach (JscLocation location in locations)
            {
                int sequence = GetMaxMemberSequence(entry, location);
                JscLocationMember member = new JscLocationMember();
                member.OwnerLocation = location.ID;
                member.MemberLocation = memberLocation.ID;
                member.Sequence = sequence;
                SaveMember(entry, member);
            }
        }


        public void SaveMember(IConnectionManager entry, JscLocationMember member)
        {
            var statement = new SqlServerStatement("JSCLOCATIONMEMBERS");
            var memberMemberships = GetMembership(entry, member.MemberLocation, member.OwnerLocation);
            if (memberMemberships.Count > 0)
                return;
            var memberships = GetMembership(entry, member.OwnerLocation, member.MemberLocation);
            if (memberships != null && memberships.Count > 0)
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("OwnerLocation", member.OwnerLocation.ToString());
                statement.AddCondition("MemberLocation", member.MemberLocation.ToString());
            }
            else
            {


                statement.StatementType = StatementType.Insert;
                statement.AddKey("OwnerLocation", member.OwnerLocation.ToString());
                statement.AddKey("MemberLocation", member.MemberLocation.ToString());

            }
            statement.AddField("Sequence", member.Sequence, SqlDbType.Int);
            entry.Connection.ExecuteStatement(statement);

        }

        public void RemoveMembers(IConnectionManager entry, JscLocation ownerLocation, IEnumerable<JscLocation> memberLocations)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

            IEnumerable<JscLocation> ownerLocationMembers = GetMemberTree(entry, ownerLocation);


            foreach (JscLocation memberLocation in memberLocations)
            {
                JscLocation member = ownerLocationMembers.FirstOrDefault(x => x.ID == memberLocation.ID);
                if (member != null)
                {
                    using (var cmd = entry.Connection.CreateCommand())
                    {
                        ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

                        cmd.CommandText = @"Delete from JscLocationMembers 
                                where OwnerLocation = @ownerLocation
                                and MemberLocation = @memberlocation
                                   ";

                        MakeParam(cmd, "ownerLocation", (Guid) ownerLocation.ID);
                        MakeParam(cmd, "memberlocation", (Guid) member.ID);

                        entry.Connection.DeleteFromTable(cmd, "JscLocationMembers", CommandType.Text);
                    }
                }
            }
        }

        public void RemoveParents(IConnectionManager entry, JscLocation memberLocation, IEnumerable<JscLocation> ownerLocations)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

            IEnumerable<JscLocation> ownerLocationMembers = GetParentTree(entry, memberLocation);


            foreach (JscLocation ownerLocation in ownerLocations)
            {
                JscLocation member = ownerLocationMembers.FirstOrDefault(x => x.ID == ownerLocation.ID);
                if (member != null)
                {
                    using (var cmd = entry.Connection.CreateCommand())
                    {
                        ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

                        cmd.CommandText = @"Delete from JscLocationMembers 
                                where OwnerLocation = @ownerLocation
                                and MemberLocation = @memberlocation
                                   ";

                        MakeParam(cmd, "ownerLocation", (Guid)member.ID);
                        MakeParam(cmd, "memberlocation", (Guid)memberLocation.ID);

                        entry.Connection.DeleteFromTable(cmd, "JscLocationMembers", CommandType.Text);
                    }
                }
            }
        }

        private void CreateMembership(IConnectionManager entry, JscLocation ownerLocation, JscLocation memberLocation)
        {
            if (!MembershipExists(entry, memberLocation.ID, ownerLocation.ID))
            {
                // Create the membership
                var member = new JscLocationMember
                {
                    OwnerLocation = ownerLocation.ID,
                    MemberLocation = memberLocation.ID,
                    Sequence = GetMaxMemberSequence(entry, ownerLocation) + 1
                };
                SaveMember(entry, member);
            }
        }

        public void Save(IConnectionManager entry, JscLocation store)
        {
            var statement = new SqlServerStatement("JscLocations");

            bool isNew = false;
            if (store.ID == null || store.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                store.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, store.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("Id", (Guid) store.ID, SqlDbType.UniqueIdentifier);

                //var storeList = GetList(entry);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid) store.ID, SqlDbType.UniqueIdentifier);
            }


            statement.AddField("NAME", store.Text);
            statement.AddField("EXDATAAREAID", store.ExDataAreaId);
            statement.AddField("EXCODE", store.ExCode);
            if (store.DatabaseDesign != null)
            {
                statement.AddField("DATABASEDESIGN", store.DatabaseDesign.ToString());
            }
            statement.AddField("LOCATIONKIND", store.LocationKind, SqlDbType.Int);
            statement.AddField("DDHOST", store.DDHost);
            statement.AddField("DDPORT", store.DDPort);
            statement.AddField("DDNETMODE", store.DDNetMode, SqlDbType.Int);
            statement.AddField("ENABLED", store.Enabled ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("COMPANY", store.Company);
            statement.AddField("USERID", store.UserId);
            statement.AddField("PASSWORD", store.Password);
            statement.AddField("DBSERVERISUSED", store.DBServerIsUsed ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DBSERVERHOST", store.DBServerHost);
            statement.AddField("DBPATHNAME", store.DBPathName);
            if (store.DBDriverType != null)
            {
                statement.AddField("DBDRIVERTYPE", (Guid) store.DBDriverType, SqlDbType.UniqueIdentifier);
            }
            statement.AddField("DBCONNECTIONSTRING", store.DBConnectionString);
            statement.AddField("SYSTEMTAG", store.SystemTag); // Note the reversed meaning here


            entry.Connection.ExecuteStatement(statement);

        }

        private JscLocation CreateLocation(IConnectionManager entry, string exDataAreaId, string externalId, string name, LocationKind locationKind, RecordIdentifier defaultOwner)
        {
            var location = new JscLocation
                {
                    ID = Guid.NewGuid(),
                    ExDataAreaId = exDataAreaId,
                    ExCode = externalId,
                    Text = name,
                    LocationKind = locationKind,
                    Enabled = true,
                    DDNetMode = NetMode.TCP,
                    DefaultOwner = defaultOwner
                };
            Save(entry, location);

            return location;
        }

        public void DeleteLocationAndMemberships(IConnectionManager entry, JscLocation location)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);
            IConnectionManagerTransaction transaction = entry.CreateTransaction();
            try
            {
                // Remove refrences from tables
                var statement = new SqlServerStatement("JscJobs") {StatementType = StatementType.Update};
                statement.AddField("Source", DBNull.Value, SqlDbType.UniqueIdentifier);
                statement.AddCondition("Source", (Guid) location.ID, SqlDbType.UniqueIdentifier);
                transaction.Connection.ExecuteStatement(statement);

                statement = new SqlServerStatement("JscJobs") {StatementType = StatementType.Update};
                statement.AddField("Destination", DBNull.Value, SqlDbType.UniqueIdentifier);
                statement.AddCondition("Destination", (Guid) location.ID, SqlDbType.UniqueIdentifier);
                transaction.Connection.ExecuteStatement(statement);

                statement = new SqlServerStatement("JscLocations") {StatementType = StatementType.Update};
                statement.AddField("DefaultOwner", DBNull.Value, SqlDbType.UniqueIdentifier);
                statement.AddCondition("DefaultOwner", (Guid) location.ID, SqlDbType.UniqueIdentifier);
                transaction.Connection.ExecuteStatement(statement);

                // Delete memberships
                statement = new SqlServerStatement("JscLocationMembers") {StatementType = StatementType.Delete};
                statement.AddCondition("OwnerLocation", (Guid) location.ID, SqlDbType.UniqueIdentifier);
                transaction.Connection.ExecuteStatement(statement);

                statement = new SqlServerStatement("JscLocationMembers") {StatementType = StatementType.Delete};
                statement.AddCondition("MemberLocation", (Guid) location.ID, SqlDbType.UniqueIdentifier);
                transaction.Connection.ExecuteStatement(statement);

                statement = new SqlServerStatement("JscRepCounters") {StatementType = StatementType.Delete};
                statement.AddCondition("Location", (Guid) location.ID, SqlDbType.UniqueIdentifier);
                transaction.Connection.ExecuteStatement(statement);

                // Finally delete the location and submit changes
                DeleteRecord(transaction, "JscLocations", "Id", location.ID, SchedulerPermissions.LocationEdit);

            }

            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
            }

        }

        #endregion

        private static string MakeIntegrationKey(JscLocation location)
        {
            return MakeIntegrationKey(location.ExCode, location.LocationKind);
        }

        private static string MakeIntegrationKey(string exCode, LocationKind locationKind)
        {
            return string.Format("{0}#{1}", exCode, (int)locationKind);
        }

        /// <summary>
        /// Makes sure that every store has a location and a location group. Makes sure that every terminal has
        /// a location and is a member of its corresponding store location group. Make sure that changes in the 
        /// name of a store or a terminal are propegated to the corresponding locations.
        /// </summary>
        public void SynchronizeLocations(IConnectionManager entry)
        {
            // Create a dictionary of existing locations
            Dictionary<string, JscLocation> existingLocations = GetIntegratedLocations(entry);

            // Loop through all stores. Note: must load to list because of cursor issues
            var stores = Providers.StoreData.GetList(entry);
            foreach (var store in stores)
            {
                JscLocation storeGroupLocation = HandleStore(existingLocations, store, entry);

                // Deal with terminals of the store
                List<TerminalListItem> terminals =
                    Providers.TerminalData.GetList(entry, store.ID);


                foreach (var terminal in terminals)
                {
                    HandleTerminal(existingLocations, storeGroupLocation, terminal, entry);
                }
            }
        }

        /// <summary>
        /// Checks a Store for consitency and updates information as needed
        /// </summary>
        /// <param name="existingLocations"></param>
        /// <param name="store"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private JscLocation HandleStore(Dictionary<string, JscLocation> existingLocations, DataEntity store, IConnectionManager entry)
        {
            string storeID = store.ID.ToString().Trim();
            string storeIntegrationKey = MakeIntegrationKey(storeID, LocationKind.Store);
            JscLocation storeGroupLocation = null;
            JscLocation storeLocation;
            if (!existingLocations.TryGetValue(storeIntegrationKey, out storeLocation))
            {
                // New store

                // Create a location group for it
                storeGroupLocation =
                    CreateLocation(entry, null, null,
                                   string.Format(
                                       Properties.Resources.LocationGroupFormat, 

                                       store.Text), LocationKind.General, null);

                // Create a location for the store proper
                CreateLocation(entry, entry.Connection.DataAreaId, storeID, store.Text,
                               LocationKind.Store, null);
            }
            else
            {
                // Existing store

                // Has the name changed?
                if (StringComparer.CurrentCulture.Compare(store.Text, storeLocation.Text) != 0)
                {
                    storeLocation.Text = store.Text;
                    Save(entry, storeLocation);
                    
                }
            }
            return storeGroupLocation;
        }

        /// <summary>
        /// Checks a Terminal for consitency and updates information as needed
        /// </summary>
        /// <param name="existingLocations"></param>
        /// <param name="groupLocation"></param>
        /// <param name="terminal"></param>
        /// <param name="entry"></param>
        private void HandleTerminal(Dictionary<string, JscLocation> existingLocations, JscLocation groupLocation, TerminalListItem terminal, IConnectionManager entry)
        {
            string exCode = $"{terminal.StoreID}-{terminal.ID}";            
            string terminalIntegrationKey = MakeIntegrationKey(exCode, LocationKind.Terminal);
            JscLocation terminalLocation;
            if (!existingLocations.TryGetValue(terminalIntegrationKey, out terminalLocation))
            {
                // New terminal

                // Create a location for the terminal proper
                RecordIdentifier groupLocationId = null;
                if (groupLocation != null)
                {
                    groupLocationId = groupLocation.ID;
                }

                terminalLocation = CreateLocation(entry, entry.Connection.DataAreaId, exCode, terminal.Text,
                                                  LocationKind.Terminal, groupLocationId);

                if (groupLocation != null)
                {
                    // Make the location a member of the group
                    CreateMembership(entry, groupLocation, terminalLocation);
                }
            }
            else
            {
                // Existing terminal

                // Has the name changed?
                if (StringComparer.CurrentCulture.Compare(terminal.Text, terminalLocation.Text) != 0)
                {
                    terminalLocation.Text = terminal.Text;
                    Save(entry,terminalLocation);
                }
            }
        }

        /// <summary>
        /// Gets all existing locations in a dictionary suitable for integration purposes where
        /// the key of the dictionary is created by MakeIntegrationKey(...)
        /// </summary>
        private Dictionary<string, JscLocation> GetIntegratedLocations(IConnectionManager entry)
        {
            Dictionary<string, JscLocation> locations = new Dictionary<string, JscLocation>();
            foreach (var location in GetLocations(entry, true))
            {
                var key = MakeIntegrationKey(location);
                // Locations might have multiple integration key values after importing locations from other systems,
                // we simply take the first one that fits and use it.
                if (!locations.ContainsKey(key))
                {
                    locations.Add(MakeIntegrationKey(location), location);
                }
            }

            return locations;
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier locationID)
        {
            return RecordExists(entry, "JscLocations", "Id", locationID);
        }

        public bool MembershipExists(IConnectionManager entry, RecordIdentifier member, RecordIdentifier owner)
        {

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"
                                SELECT 
                                    OwnerLocation,
                                    MemberLocation,
                                    Sequence
                                  FROM JscLocationMembers
                                  WHERE [OwnerLocation] = @Owner AND 
		                                memberlocation = @Member";

                MakeParam(cmd, "Owner", (Guid) owner);

                MakeParam(cmd, "Member", (Guid) member);

                return Execute<JscLocationMember>(entry, cmd, CommandType.Text, PopulateLocationMember).Count > 0;
            }
        }

        public List<JscLocationMember> GetAllMemberships(IConnectionManager entry, RecordIdentifier owner, bool loadMembers)
        {

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"
                                SELECT 
                                    OwnerLocation,
                                    MemberLocation,
                                    Sequence
                                  FROM JscLocationMembers
                                  WHERE OwnerLocation = @Owner";

                MakeParam(cmd, "Owner", (Guid) owner, SqlDbType.UniqueIdentifier);


                var result = Execute<JscLocationMember>(entry, cmd, CommandType.Text, PopulateLocationMember);

                if (result != null && loadMembers)
                {
                    foreach (var locm in result)
                    {
                        if (locm.Member == null && !string.IsNullOrEmpty((string)locm.MemberLocation))
                        {
                            locm.Member = GetLocation(entry, locm.MemberLocation);
                        }
                    }
                }

                return result;
            }
        }

        public List<JscLocationMember> GetMembership(IConnectionManager entry, RecordIdentifier owner, RecordIdentifier member)
        {

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"
                                SELECT 
                                    OwnerLocation,
                                    MemberLocation,
                                    Sequence
                                  FROM JscLocationMembers
                                  WHERE OwnerLocation = @Owner AND 
		                                memberlocation = @Member";

                MakeParam(cmd, "Owner", (Guid) owner, SqlDbType.UniqueIdentifier);

                MakeParam(cmd, "Member", (Guid) member, SqlDbType.UniqueIdentifier);

                return Execute<JscLocationMember>(entry, cmd, CommandType.Text, PopulateLocationMember);
            }
        }

        public List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "JscLocations", "Name", "Id", "Name");
        }

        private int GetMaxMemberSequence(IConnectionManager entry, JscLocation ownerLocation)
        {
            if (ownerLocation.MemberLocations == null)
            {
                ownerLocation.MemberLocations = GetAllMemberships(entry, ownerLocation.ID, false);
            }
            if (ownerLocation.MemberLocations.Count > 0)
            {
                return ownerLocation.MemberLocations.Max(x => x.Sequence);
            }

            return 0;
        }

        public IEnumerable<JscLocation> GetLocations(IConnectionManager entry, bool includeDisabled)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseGetString ;

                if (!includeDisabled)
                {
                    cmd.CommandText += " WHERE ENABLED = 1";
                }
                cmd.CommandText += " order by NAME";

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }


        }

        public JscLocation GetLocation(IConnectionManager entry, RecordIdentifier locationId)
        {
            ValidateSecurity(entry);


            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    BaseGetString +
                    " where Id = @locationId";


                MakeParam(cmd, "locationId", locationId.ToString());

                var records = Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);

                if (records.Count > 0)
                {
                    return records[0];
                }
                return null;
            }
        }
        public JscLocation GetLocationByExId(IConnectionManager entry, RecordIdentifier locationId)
        {
            ValidateSecurity(entry, new[]
                {
                    SchedulerPermissions.LocationView,
                    SchedulerPermissions.JobSubjobView
                });


            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    BaseGetString +
                    " where ExCode = @locationId";


                MakeParam(cmd, "locationId", locationId.ToString());

                var records = Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);

                if (records.Count > 0)
                {
                    return records[0];
                }
                return null;
            }
        }

        public IEnumerable<JscDriverType> GetDriverTypes(IConnectionManager entry)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    @"select 
                        Id,
                        Name,
                        DatabaseServerType,
                        DatabaseParams,
                        ConnectionStringFormat,
                        EnabledFields
                  FROM JscDriverTypes";

                return Execute<JscDriverType>(entry, cmd, CommandType.Text, PopulateDriverType);
            }
        }

        /// <summary>
        /// Gets a list of JscLocations that can safely be added as member to the specified location.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ownerLocation">The location that can be safely set as an owner of the new members.</param>
        public IList<JscLocation> GetNewMemberList(IConnectionManager entry, JscLocation ownerLocation)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    @"-- Collect all Owners
                        WITH RecursiveOwners(MemberLocation,OwnerLocation, Sequence) AS (

	                        SELECT 
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation, 
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm
	                        WHERE 
		                        jlm.MemberLocation = @locationMemberId
	                        UNION ALL 
	                        SELECT  
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation,
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm
	                        INNER JOIN 
		                        RecursiveOwners on RecursiveOwners.OwnerLocation = jlm.MemberLocation
                        ),
                        -- Collect All Members
                        RecursiveMembers(MemberLocation,OwnerLocation, Sequence) AS (
	                        SELECT 
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation, 
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm

	                        WHERE 
		                        jlm.OwnerLocation = @locationOwnerId
	                        UNION ALL 
	                        SELECT  
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation, 
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm
	                        INNER JOIN 
		                        RecursiveMembers on RecursiveMembers.MemberLocation = jlm.OwnerLocation
                        )
                        SELECT 
                                Id,
                                Name,
                                EXDATAAREAID,
                                ExCode,
                                DatabaseDesign,
                                LocationKind,
                                DefaultOwner,
                                DDHost,
                                DDPort,
                                DDNetMode,
                                Enabled,
                                Company,
                                UserId,
                                Password,
                                DBServerIsUsed,
                                DBServerHost,
                                DBPathName,
                                DBDriverType,
                                DBConnectionString,
                                SystemTag
	                        FROM 
		                        JscLocations 
	                        WHERE 
		                        Enabled = 1 AND 
                                EXDATAAREAID = @EXDATAAREAID AND
		                        -- Exclude all unsafe IDs
		                        id != @locationId and 
		                        id not in( 
			                        SELECT 
				                        MemberLocation 
			                        FROM 
				                        RecursiveMembers
			                        UNION ALL 
			                        SELECT 
				                        OwnerLocation 
			                        FROM 
				                        RecursiveOwners
		                        )
	                        ORDER BY LocationKind, Name
                        ";


                MakeParam(cmd, "locationMemberId", (Guid) ownerLocation.ID);
                MakeParam(cmd, "locationOwnerId", (Guid) ownerLocation.ID);
                MakeParam(cmd, "locationId", (Guid) ownerLocation.ID);
                MakeParam(cmd, "EXDATAAREAID", entry.Connection.DataAreaId);

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }
        }



        public IEnumerable<JscLocation> GetLocationsWhereConnectable(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseGetString +
                    " where DBConnectionString is not null " +
                    " order by Name";

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }
        }

        public IEnumerable<JscLocation> GetLocationsWhereDDHost(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseGetString +
                    " where DDHost is not null " +
                    " order by Name"; ;

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }


        }

        /// <summary>
        /// Returns all member locations of the specified location and all members of those members recurseively.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceLocation"></param>
        /// <returns></returns>
        public IEnumerable<JscLocation> GetMemberTree(IConnectionManager entry, JscLocation sourceLocation)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" -- Collect All Members
                        WITH RecursiveMembers(MemberLocation,OwnerLocation, Sequence) AS (
	                        SELECT 
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation, 
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm

	                        WHERE 
		                        jlm.OwnerLocation = @locationOwnerId
	                        UNION ALL 
	                        SELECT  
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation, 
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm
	                        INNER JOIN 
		                        RecursiveMembers on RecursiveMembers.MemberLocation = jlm.OwnerLocation
                        )
                        SELECT 
                                Id,
                                Name,
                                EXDATAAREAID,
                                ExCode,
                                DatabaseDesign,
                                LocationKind,
                                DefaultOwner,
                                DDHost,
                                DDPort,
                                DDNetMode,
                                Enabled,
                                Company,
                                UserId,
                                Password,
                                DBServerIsUsed,
                                DBServerHost,
                                DBPathName,
                                DBDriverType,
                                DBConnectionString,
                                SystemTag 
	                        FROM 
		                        JscLocations 
	                        WHERE 
		                        Enabled = 1 AND 
		                        -- Include only members
		                        id != @locationId and 
		                        id in( 
			                        SELECT 
				                        MemberLocation 
			                        FROM 
				                       RecursiveMembers
			                    )
	                        ORDER BY LocationKind, Name
                        ";


                MakeParam(cmd, "locationOwnerId", (Guid) sourceLocation.ID);
                MakeParam(cmd, "locationId", (Guid) sourceLocation.ID);

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }
        }

        /// <summary>
        /// Returns all member locations of the specified location and all members of those members recurseively.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceLocation"></param>
        /// <returns></returns>
        public IEnumerable<JscLocation> GetParentTree(IConnectionManager entry, JscLocation sourceLocation)
        {
            ValidateSecurity(entry, SchedulerPermissions.LocationEdit);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" -- Collect all Owners
                        WITH RecursiveOwners(MemberLocation,OwnerLocation, Sequence) AS (

	                        SELECT 
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation, 
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm
	                        WHERE 
		                        jlm.MemberLocation = @locationMemberId
	                        UNION ALL 
	                        SELECT  
		                        jlm.MemberLocation, 
		                        jlm.OwnerLocation,
		                        jlm.Sequence
	                        FROM 
		                        [dbo].[JscLocationMembers] jlm
	                        INNER JOIN 
		                        RecursiveOwners on RecursiveOwners.OwnerLocation = jlm.MemberLocation
                        )
                        SELECT 
                                Id,
                                Name,
                                EXDATAAREAID,
                                ExCode,
                                DatabaseDesign,
                                LocationKind,
                                DefaultOwner,
                                DDHost,
                                DDPort,
                                DDNetMode,
                                Enabled,
                                Company,
                                UserId,
                                Password,
                                DBServerIsUsed,
                                DBServerHost,
                                DBPathName,
                                DBDriverType,
                                DBConnectionString,
                                SystemTag 
	                        FROM 
		                        JscLocations 
	                        WHERE 
		                        Enabled = 1 AND 
		                        -- Include only members
		                        id != @locationId and 
		                        id in( 
			                        SELECT 
				                        OwnerLocation 
			                        FROM 
				                       RecursiveOwners
			                    )
	                        ORDER BY LocationKind, Name
                        ";


                MakeParam(cmd, "locationMemberId", (Guid)sourceLocation.ID);
                MakeParam(cmd, "locationId", (Guid)sourceLocation.ID);

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }
        }

        /// <summary>
        /// Gets the specified location's database design. This is either the database design
        /// directly assigned to the location or the database design assigned to any of the location's
        /// member locations.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="location">The location whose datbase design should be determined.</param>
        /// <returns>A database design or null if no database design is assigned.</returns>
        public JscDatabaseDesign GetLocationDatabaseDesign(IConnectionManager entry, JscLocation location)
        {
            ValidateSecurity(entry, new string[] {SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView});

            if (location == null)
            {
                return null;
            }

            if (location.DatabaseDesign != null)
            {
                return GetDatabaseDesign(entry, location.DatabaseDesign);
            }

            foreach (var sourceLocation in GetMemberTree(entry, location))
            {
                if (sourceLocation.DatabaseDesign != null)
                {
                    return GetDatabaseDesign(entry, sourceLocation.DatabaseDesign);
                }
            }

            return null;
        }

        private JscDatabaseDesign GetDatabaseDesign(IConnectionManager entry, RecordIdentifier jscDatabaseDesignId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"
                        SELECT Id,
                              Description,
                              CodePage,
                              Enabled
                          FROM JscDatabaseDesigns
                        where Id = @databaseDesignId";


                MakeParam(cmd, "databaseDesignId", (Guid) jscDatabaseDesignId);

                return Get<JscDatabaseDesign>(entry, cmd, CommandType.Text, jscDatabaseDesignId, null,
                                              PopulateDatabaseDesign, CacheType.CacheTypeNone,
                                              UsageIntentEnum.Normal);
            }
        }

        public List<JscLocation> GetMembers(IConnectionManager entry, RecordIdentifier ownerLocationId)
        {
            ValidateSecurity(entry, new[]
                {
                    SchedulerPermissions.LocationView,
                    SchedulerPermissions.JobSubjobView
                });

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"
                        SELECT  
                            Id,
	                        Name,
	                        EXDATAAREAID,
	                        ExCode,
	                        DatabaseDesign,
	                        LocationKind,
	                        DefaultOwner,
	                        DDHost,
	                        DDPort,
	                        DDNetMode,
	                        Enabled,
	                        Company,
	                        UserId,
	                        Password,
	                        DBServerIsUsed,
	                        DBServerHost,
	                        DBPathName,
	                        DBDriverType,
	                        DBConnectionString,
	                        SystemTag

                        FROM 
	                        JscLocations JL  
                        INNER JOIN
	                        JscLocationMembers JLM on JLM.MemberLocation = JL.Id
                          WHERE 
	                        JLM.OwnerLocation = @locationOwnerId
                        ORDER BY JL.Name";

                MakeParam(cmd, "locationOwnerId", (Guid) ownerLocationId);

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }
        }

        public List<JscLocation> GetParents(IConnectionManager entry, RecordIdentifier locationId)
        {
            ValidateSecurity(entry, new[]
                {
                    SchedulerPermissions.LocationView,
                    SchedulerPermissions.JobSubjobView
                });

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"
                        SELECT  
                            Id,
	                        Name,
	                        EXDATAAREAID,
	                        ExCode,
	                        DatabaseDesign,
	                        LocationKind,
	                        DefaultOwner,
	                        DDHost,
	                        DDPort,
	                        DDNetMode,
	                        Enabled,
	                        Company,
	                        UserId,
	                        Password,
	                        DBServerIsUsed,
	                        DBServerHost,
	                        DBPathName,
	                        DBDriverType,
	                        DBConnectionString,
	                        SystemTag

                        FROM 
	                        JscLocations JL  
                        INNER JOIN
	                        JscLocationMembers JLM on JLM.OwnerLocation = JL.Id
                          WHERE 
	                        JLM.MemberLocation = @locationId
                        ORDER BY JL.Name";

                MakeParam(cmd, "locationId", (Guid)locationId);

                return Execute<JscLocation>(entry, cmd, CommandType.Text, PopulateLocation);
            }
        }
    }
}
