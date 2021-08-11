using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDDataProviders
{
    public interface IDesignData : IDataProviderBase<JscTableDesign>
    {
        /// <summary>
        /// Saves a given table design to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesign">The table design to save</param>
        void Save(IConnectionManager entry, JscTableDesign tableDesign);

        /// <summary>
        /// Saves a given field design to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fieldDesign">The field design to save</param>
        void Save(IConnectionManager entry, JscFieldDesign fieldDesign);

        /// <summary>
        /// Saves a given database design to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="databaseDesign">The database design to save</param>
        void Save(IConnectionManager entry, JscDatabaseDesign databaseDesign);

        /// <summary>
        /// Saves a given table mapping to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableMap">The table mapping to save</param>
        void Save(IConnectionManager entry, JscTableMap tableMap);

        /// <summary>
        /// Saves a given linked table to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="linkedTable">The linked table to save</param>
        void Save(IConnectionManager entry, JscLinkedTable linkedTable);

        /// <summary>
        /// Saves a given linked table filter to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="linkedFilter">The linked table filter to save</param>
        void Save(IConnectionManager entry, JscLinkedFilter linkedFilter);

        /// <summary>
        /// Saves a given sub-job from-table filter to the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fromTableFilter">The from-table filter to save</param>
        void Save(IConnectionManager entry, JscSubJobFromTableFilter fromTableFilter);

        /// <summary>
        /// Deletes a range of table mappings from the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableMaps">A list containing the table mappings to delete</param>
        void DeleteTableMaps(IConnectionManager entry, IEnumerable<JscTableMap> tableMaps);

        /// <summary>
        /// Deletes the given field mapping from the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fieldMapToDelete">The field mapping to delete</param>
        void DeleteFieldMap(IConnectionManager entry, JscFieldMap fieldMapToDelete);

        /// <summary>
        /// Deletes the given sub-job from-table filter from the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fromTableFilter">The from-table filter to delete</param>
        void Delete(IConnectionManager entry, JscSubJobFromTableFilter fromTableFilter);

        /// <summary>
        /// Deletes the given linked table filter from the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="linkedFilter">The linked table filter to delete</param>
        void DeleteLinkedFilter(IConnectionManager entry, JscLinkedFilter linkedFilter);

        /// <summary>
        /// Deletes the given linked table from the database
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="linkedTable">The linked table to delete</param>
        void DeleteLinkedTable(IConnectionManager entry, JscLinkedTable linkedTable);
        
        /// <summary>
        /// Deletes the field design with the given ID
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fieldDesignID">The ID of the field design to delete</param>
        void DeleteFieldDesign(IConnectionManager entry, RecordIdentifier fieldDesignID);

        /// <summary>
        /// Gets a list of all database designs
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <returns></returns>
        List<DataSelector> GetDatabaseDesignList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of table mappings. Optionally filtered by from- or to-table designs
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignFilterFrom">Filters to mappins that map from the given table design</param>
        /// <param name="tableDesignFilterTo">Filters to mappings that match to the given table design</param>
        /// <returns></returns>
        IEnumerable<JscTableMap> GetDatabaseMaps(IConnectionManager entry, RecordIdentifier tableDesignFilterFrom = null, RecordIdentifier tableDesignFilterTo = null);

        /// <summary>
        /// Gets the specified table mapping
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableMapId">The ID of the table mapping to get</param>
        /// <returns></returns>
        JscTableMap GetTableMap(IConnectionManager entry, RecordIdentifier tableMapId);

        /// <summary>
        /// Gets all field designs for the given table design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design to get all field designs for</param>
        /// <returns></returns>
        IEnumerable<JscFieldDesign> GetFieldDesigns(IConnectionManager entry, RecordIdentifier tableDesignId);

        /// <summary>
        /// Gets the specified field design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fieldDesignId">The ID of the field design to get</param>
        /// <returns></returns>
        JscFieldDesign GetFieldDesign(IConnectionManager entry, RecordIdentifier fieldDesignId);

        /// <summary>
        /// Gets all field designs for the given table ordered by sequence
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design to get field designs for</param>
        /// <returns></returns>
        IEnumerable<JscFieldDesign> GetFieldDesignsOrderedBySequence(IConnectionManager entry, RecordIdentifier tableDesignId);

        /// <summary>
        /// Gets all field designs for the given table ordered by name
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design to get field designs for</param>
        /// <returns></returns>
        IEnumerable<JscFieldDesign> GetFieldDesignsOrderedByFieldName(IConnectionManager entry, RecordIdentifier tableDesignId);

        /// <summary>
        /// Gets all database designs
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="includeDisabled">If true then disabled database designs are also included in the list</param>
        /// <returns></returns>
        IEnumerable<JscDatabaseDesign> GetDatabaseDesigns(IConnectionManager entry, bool includeDisabled);

        /// <summary>
        /// Gets all table desings for the given database design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="databaseDesignId">The ID of the database design</param>
        /// <param name="onlyWhereLinkedTablesExists">If true then only table designs that are linked to other tables are returned</param>
        /// <returns></returns>
        IEnumerable<JscTableDesign> GetTableDesigns(IConnectionManager entry, RecordIdentifier databaseDesignId, bool onlyWhereLinkedTablesExists);

        /// <summary>
        /// Gets all tables that are linked to the given table design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design</param>
        /// <returns></returns>
        IEnumerable<JscLinkedTable> GetLinkedTables(IConnectionManager entry,RecordIdentifier tableDesignId);

        /// <summary>
        /// Gets all linked table filters for the given table design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design</param>
        /// <returns></returns>
        List<JscLinkedFilter> GetLinkedFilters(IConnectionManager entry, RecordIdentifier tableDesignId);

        /// <summary>
        /// Gets all locations that use the given database design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="databaseDesignId">The ID of the database design</param>
        /// <returns></returns>
        IEnumerable<JscLocation> GetLocationsUsingDatabase(IConnectionManager entry,RecordIdentifier databaseDesignId);

        /// <summary>
        /// Checks if a link exists between the given table designs
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="fromTable">The table design that should be from</param>
        /// <param name="toTable">The table design that should be linked to</param>
        /// <returns></returns>
        bool ExistsLinkedTable(IConnectionManager entry, RecordIdentifier fromTable, RecordIdentifier toTable);

        /// <summary>
        /// Gets the specified table design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design to get</param>
        /// <returns></returns>
        JscTableDesign GetTableDesign(IConnectionManager entry, RecordIdentifier tableDesignId);

        /// <summary>
        /// Gets the specified database design
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="id">The ID of the database design to get</param>
        /// <returns></returns>
        JscDatabaseDesign GetDatabaseDesign(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Checks if the specified table design has any field designs
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="tableDesignId">The ID of the table design</param>
        /// <returns></returns>
        bool HasFields(IConnectionManager entry, RecordIdentifier tableDesignId);
    }
}