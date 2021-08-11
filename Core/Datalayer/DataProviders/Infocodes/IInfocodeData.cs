using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Infocodes
{
    public interface IInfocodeData : IDataProvider<Infocode>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Returns a list of infocodes with a specific triggering setting
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="triggering">The triggering setting that should be selected on the returned infocodes</param>
        /// <returns>A list of infocodes</returns>
        List<DataEntity> GetList(IConnectionManager entry, TriggeringEnum triggering);
        List<DataEntity> GetListWithoutTriggerFunctions(IConnectionManager entry);
        List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, RefTableEnum refTable);
        List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, bool includeCategories, RecordIdentifier refRelation, RefTableEnum refTable);
        List<Infocode> GetInfocodes(IConnectionManager entry, InputTypesEnum[] inputTypes, InfocodeSorting sortBy, bool sortBackwards, RefTableEnum refTable);
        List<Infocode> GetInfocodes(IConnectionManager entry, InputTypesEnum[] inputTypes, bool includeInputTypes, RefTableEnum refTable);

        /// <summary>
        /// Gets a list of infocodes  including / excluding the given range of input types and sorted by the given sort enum.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="inputTypes">Range of input types</param>
        /// <param name="includeInputTypes">Including(true) or excluding(false) the range</param>
        /// <param name="sortBy">Sort by infocode or description</param>
        /// <param name="refTable"></param>
        /// <param name="sortBackwards"></param>
        /// <returns></returns>
        List<Infocode> GetInfocodes(IConnectionManager entry,
            InputTypesEnum[] inputTypes,
            bool includeInputTypes,
            InfocodeSorting sortBy, 
            bool sortBackwards,
            RefTableEnum refTable);

        List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, InputTypesEnum[] inputTypes, RefTableEnum refTable);
        List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, InputTypesEnum[] inputTypes, InfocodeSorting sortBy, bool sortBackwards, RefTableEnum refTable);

        List<Infocode> GetInfocodes(
            IConnectionManager entry
            , UsageCategoriesEnum[] usageCategories
            , bool includeCategories
            , InputTypesEnum[] inputTypes
            , bool includeInputTypes
            , RecordIdentifier refRelation
            , RefTableEnum refTable);

        List<DataEntity> GetTaxGroupInfocodes(IConnectionManager entry);

        /// <summary>
        /// Checks if an infocode is being used.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocode">The infocode being checked</param>
        /// <param name="operations">List of operations to check for</param>
        /// <returns>True if infocode is in use</returns>
        bool InfocodeInUseByOperation(IConnectionManager entry, Infocode infocode, List<string> operations);

        Infocode Get(IConnectionManager entry, RecordIdentifier infocodeId);
    }
}