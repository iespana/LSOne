using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.DataLayer.DataProviders.ItemMaster.Dimensions
{
    public interface IDimensionAttributeData : IDataProviderBase<DimensionAttribute>
    {
        DimensionAttribute Get(IConnectionManager entry, RecordIdentifier attributeID);

        List<DimensionAttribute> GetListForRetailItemDimension(IConnectionManager entry, RecordIdentifier retailItemDimensionID, CacheType cacheType = CacheType.CacheTypeNone);

        List<DimensionAttribute> GetListForDimensionTemplate(IConnectionManager entry, RecordIdentifier dimensionTemplateID);

        void Delete(IConnectionManager entry, RecordIdentifier templateID);
        void Save(IConnectionManager entry, DimensionAttribute attribute);

        /// <summary>
        /// Returns a list of all dimension attributes that are being used by the variant items that belong to the given master item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="masterItemID">The master ID for the master item</param>
        /// <returns></returns>
        List<DimensionAttribute> GetAttributesInUseByItem(IConnectionManager entry, RecordIdentifier masterItemID);

        /// <summary>
        /// Returns a dictionary that includes all item-attribute relations for items that belong to the given header item ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerItemID">The ID of the header item</param>
        /// <param name="showDeleted">Determines if provider returns deleted items</param>
        /// <param name="excludeItemswithSerialNumber">Should we skipt items with serial number</param>
        /// <returns></returns>
        Dictionary<RecordIdentifier, List<DimensionAttribute>> GetRetailItemDimensionAttributeRelations(IConnectionManager entry, RecordIdentifier headerItemID, bool showDeleted = true, bool excludeItemswithSerialNumber = false);

        /// <summary>
        /// Returns a dictionary that includes all dimensions and attributes that belong to the given header item ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerItemID">The ID of the header item</param>
        /// <param name="showDeleted">Determens if we also get the deleted attributes</param>
        /// <returns></returns>
        Dictionary<RecordIdentifier, List<DimensionAttribute>> GetHeaderItemDimensionsAndAttributes(IConnectionManager entry, RecordIdentifier headerItemID, bool showDeleted = false);
    }
}
