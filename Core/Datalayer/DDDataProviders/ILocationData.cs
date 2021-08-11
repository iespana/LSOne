using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDDataProviders
{
    public interface ILocationData : IDataProviderBase<JscLocation>
    {
        void PopulateDatabaseDesign(IConnectionManager entry, IDataReader dr, JscDatabaseDesign databaseDesign, object param);
        void PopulateLocationNormal(IConnectionManager entry, IDataReader dr, JscLocation location, object param);
        void PopulateLocation(IDataReader dr, JscLocation location);
        void PopulateLocationMember(IDataReader dr, JscLocationMember locationMember);
        void AddMembers(IConnectionManager entry, JscLocation ownerLocation, IEnumerable<JscLocation> locations);
        void AddParents(IConnectionManager entry, JscLocation memberLocation, IEnumerable<JscLocation> locations);
        void SaveMember(IConnectionManager entry, JscLocationMember member);
        void RemoveMembers(IConnectionManager entry, JscLocation ownerLocation, IEnumerable<JscLocation> memberLocations);
        void RemoveParents(IConnectionManager entry, JscLocation memberLocation,IEnumerable<JscLocation> ownerLocations);
        void Save(IConnectionManager entry, JscLocation store);
        void DeleteLocationAndMemberships(IConnectionManager entry, JscLocation location);

        /// <summary>
        /// Makes sure that every store has a location and a location group. Makes sure that every terminal has
        /// a location and is a member of its corresponding store location group. Make sure that changes in the 
        /// name of a store or a terminal are propegated to the corresponding locations.
        /// </summary>
        void SynchronizeLocations(IConnectionManager entry);

        bool Exists(IConnectionManager entry, RecordIdentifier locationID);
        bool MembershipExists(IConnectionManager entry, RecordIdentifier member, RecordIdentifier owner);
        List<JscLocationMember> GetAllMemberships(IConnectionManager entry, RecordIdentifier owner, bool loadMembers);
        List<JscLocationMember> GetMembership(IConnectionManager entry, RecordIdentifier owner, RecordIdentifier member);
        List<DataEntity> GetList(IConnectionManager entry);
        IEnumerable<JscLocation> GetLocations(IConnectionManager entry, bool includeDisabled);
        JscLocation GetLocation(IConnectionManager entry, RecordIdentifier locationId);
        JscLocation GetLocationByExId(IConnectionManager entry, RecordIdentifier locationId);
        IEnumerable<JscDriverType> GetDriverTypes(IConnectionManager entry);

        /// <summary>
        /// Gets a list of JscLocations that can safely be added as member to the specified location.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ownerLocation">The location that can be safely set as an owner of the new members.</param>
        IList<JscLocation> GetNewMemberList(IConnectionManager entry, JscLocation ownerLocation);

        IEnumerable<JscLocation> GetLocationsWhereConnectable(IConnectionManager entry);
        IEnumerable<JscLocation> GetLocationsWhereDDHost(IConnectionManager entry);

        /// <summary>
        /// Returns all member locations of the specified location and all members of those members recurseively.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceLocation"></param>
        /// <returns></returns>
        IEnumerable<JscLocation> GetMemberTree(IConnectionManager entry, JscLocation sourceLocation);

        /// <summary>
        /// Gets the specified location's database design. This is either the database design
        /// directly assigned to the location or the database design assigned to any of the location's
        /// member locations.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="location">The location whose datbase design should be determined.</param>
        /// <returns>A database design or null if no database design is assigned.</returns>
        JscDatabaseDesign GetLocationDatabaseDesign(IConnectionManager entry, JscLocation location);

        List<JscLocation> GetMembers(IConnectionManager entry, RecordIdentifier ownerLocationId);
        List<JscLocation> GetParents(IConnectionManager entry, RecordIdentifier locationId);
    }
}