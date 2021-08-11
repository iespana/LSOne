using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Infocodes
{
    public interface IInfocodeSubcodeData : IDataProvider<InfocodeSubcode>, ISequenceable
    {
        List<InfocodeSubcode> GetListForInfocodeTaxGroupOnly(IConnectionManager entry, RecordIdentifier infocodeID, 
            InfocodeSubcodeSorting sortEnum, bool sortBackwards);

        /// <summary>
        /// Returns a list of all infocodeSubcodes for the given infocode ID, in ASC order based on Infocode Description
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeID">The id of the Infocode</param>
        /// <returns>A list of all infocodeSubcodes for the given infocode ID</returns>
        List<InfocodeSubcode> GetListForInfocode(IConnectionManager entry, RecordIdentifier infocodeID);

        /// <summary>
        /// Returns a list of all infocodeSubcodes for the given infocode ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeID">The id of the Infocode</param>
        /// <param name="sortEnum">An enum which defines the sort ordering of the result set</param>
        /// <param name="sortBackwards">Wether to reverse the ordering of the result set or not</param>
        /// <returns>A list of all infocodeSubcodes for the given infocode ID</returns>
        List<InfocodeSubcode> GetListForInfocode(IConnectionManager entry, RecordIdentifier infocodeID, InfocodeSubcodeSorting sortEnum, bool sortBackwards);

        InfocodeSubcode Get(IConnectionManager entry, RecordIdentifier infocodeSubcodeID);
    }
}