using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface IDimensionService : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem"></param>
        /// <returns></returns>
        bool SetDimensionOnItem(IConnectionManager entry, ISaleLineItem saleLineItem);

        /// <summary>
        /// Shows the DimensionDialog which shows all available dimensions and attribute for the given master item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The master item to show dimensions and attributes for</param>
        /// <returns>The ID of the variant item for the attribute combination that the user selected</returns>
        RecordIdentifier ShowDimensionDialog(IConnectionManager entry, ISaleLineItem saleLineItem);

        /// <summary>
        /// Shows the DimensionDialog which shows all available dimensions and attribute for the given master item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="masterItemID">ID of the master item to show dimensions and attributes for</param>
        /// <param name="masterItemName">Name/description of the master item to show dimensions and attributes for</param>
        /// <returns>The ID of the variant item for the attribute combination that the user selected</returns>
        RecordIdentifier ShowDimensionDialog(IConnectionManager entry, RecordIdentifier masterItemID, string masterItemName);
    }
}
