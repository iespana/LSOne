using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface IButtonGridButtonsData : IDataProvider<ButtonGridButton>
    {
        /// <summary>
        /// Gets a list of all ButtonGridButtons
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of ButtonGridButtons objects containing all button grid buttons</returns>
        List<ButtonGridButton> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all button grid buttons belonging to a given button grid id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid</param>
        /// <returns>A list of ButtonGridButtons objects containing all button grid buttons for the given button grid ID</returns>
        List<ButtonGridButton> GetList(IConnectionManager entry, RecordIdentifier buttonGridID);

        int GetNextButtonID(IConnectionManager entry);
    }
}