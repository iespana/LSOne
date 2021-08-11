using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Scheduler.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataProviders
{
    public interface IDesignData : IDataProviderBase<JscTableDesign>
    {
        void Save(IConnectionManager entry, JscTableDesign tableDesign);
        void Save(IConnectionManager entry, JscFieldDesign fieldDesign);
        void Save(IConnectionManager entry, JscDatabaseDesign databaseDesign);
        void Save(IConnectionManager entry, JscTableMap tableMap);
        void Save(IConnectionManager entry, JscLinkedTable linkedTable);
        void Save(IConnectionManager entry, JscLinkedFilter linkedFilter);
        void Save(IConnectionManager entry, JscSubJobFromTableFilter fromTableFilter);
        void DeleteTableMaps(IConnectionManager entry, IEnumerable<JscTableMap> tableMaps);
        void DeleteFieldMap(IConnectionManager entry, JscFieldMap fieldMapToDelete);
        void Delete(IConnectionManager entry, JscSubJobFromTableFilter fromTableFilter);
        void DeleteLinkedFilter(IConnectionManager entry, JscLinkedFilter linkedFilter);
        void DeleteLinkedTable(IConnectionManager entry, JscLinkedTable linkedTable);
        List<DataSelector> GetDatabaseDesignList(IConnectionManager entry);
        IEnumerable<JscTableMap> GetDatabaseMaps(IConnectionManager entry, RecordIdentifier tableDesignFilterFrom, RecordIdentifier tableDesignFilterTo);
        JscTableMap GetTableMap(IConnectionManager entry, RecordIdentifier tableMapId);
        IEnumerable<JscFieldDesign> GetFieldDesigns(IConnectionManager entry, RecordIdentifier tableDesignId);
        JscFieldDesign GetFieldDesign(IConnectionManager entry, RecordIdentifier fieldDesignId);
        IEnumerable<JscFieldDesign> GetFieldDesignsOrderedBySequence(IConnectionManager entry, RecordIdentifier tableDesignId);
        IEnumerable<JscFieldDesign> GetFieldDesignsOrderedByFieldName(IConnectionManager entry, RecordIdentifier tableDesignId);
        IEnumerable<JscDatabaseDesign> GetDatabaseDesigns(IConnectionManager entry, bool includeDisabled);
        IEnumerable<JscTableDesign> GetTableDesigns(IConnectionManager entry, RecordIdentifier databaseDesignId, bool onlyWhereLinkedTablesExit);
        IEnumerable<JscLinkedTable> GetLinkedTables(IConnectionManager entry,RecordIdentifier tableDesignId);
        List<JscLinkedFilter> GetLinkedFilters(IConnectionManager entry, RecordIdentifier tableDesignId);
        IEnumerable<JscLocation> GetLocationsUsingDatabase(IConnectionManager entry,RecordIdentifier databaseId);
        bool ExistsLinkedTable(IConnectionManager entry, RecordIdentifier fromTable, RecordIdentifier toTable);
        JscTableDesign GetTableDesign(IConnectionManager entry, RecordIdentifier tableDesignId);
        JscDatabaseDesign GetDatabaseDesign(IConnectionManager entry, RecordIdentifier id);
        bool HasFields(IConnectionManager entry, RecordIdentifier tableDesignId);
    }
}