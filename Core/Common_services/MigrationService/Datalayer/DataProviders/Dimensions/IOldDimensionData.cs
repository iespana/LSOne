using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders.Dimensions
{
    public interface IOldDimensionData : IDataProviderBase<OldDimension>, ISequenceable
    {
        /// <summary>
        /// Returns a dimension by variant ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="variantNumber">The unique ID of the variant that is to be searched
        /// for</param>
        /// <returns>
        /// Returns an instance of <see cref="Dimension"/>
        /// </returns>
       OldDimension GetByVariantID(IConnectionManager entry, RecordIdentifier variantNumber);

        /// <summary>
        /// Returns dimension information for a specific item and specific variant number
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="variantNumber">The unique ID of the variant</param>
        /// <returns>
        /// Returns an instance of <see cref="Dimension"/>
        /// </returns>
       OldDimension Get(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier variantNumber);

        /// <summary>
        /// Returns a variant ID that is associated with a dimension ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimensionID">The unique ID of a dimension</param>
        /// <returns>
        /// Returns a unique variant ID
        /// </returns>
        RecordIdentifier GetVariantIDFromDimID(IConnectionManager entry, RecordIdentifier dimensionID);

        /// <summary>
        /// Returns a list of dimensions associated with an item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item to be searched for</param>
        /// <param name="sortColumn">What sorting should be applied to the returned
        /// list</param>
        /// <param name="backwardsSort">If set to <see langword="true" />, then sort the
        /// list descending otherwise, ascending</param>
        /// <returns>
        /// Returns a list of <see cref="Dimension" /> associated with the item
        /// </returns>
        List<OldDimension> GetList(IConnectionManager entry, RecordIdentifier itemID, int sortColumn, bool backwardsSort);

        /// <summary>
        /// Returns a list of possible dimensions that can be used to create combinations on
        /// the item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// A list of possible dimension combinations for the item
        /// </returns>
        List<OldDimensionCombination> GetCreatableDimensionCombinations(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Returns a list of colors that are not already in a dimension combinations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="selectedSizeID">The unique ID of the selected size</param>
        /// <param name="selectedStyleID">The unique ID of the selected style</param>
        /// <param name="existingSizeID">The unique ID of the possible existing size</param>
        /// <param name="existingColorID">The unique ID of the possible existing
        /// color</param>
        /// <param name="existingStyleID">The unique ID of the possible existing
        /// style</param>
        /// <param name="allowSize">If set to <see langword="true" />, then allow size in
        /// combination otherwise, not</param>
        /// <param name="allowStyle">If set to <see langword="true" />, then allow style in
        /// combination otherwise, not</param>
        /// <returns>
        /// A list of colors not included in the dimension
        /// </returns>
        List<DataEntity> GetColorsNotInDimension(
            IConnectionManager entry, 
            RecordIdentifier itemID,
            RecordIdentifier selectedSizeID,
            RecordIdentifier selectedStyleID,
            RecordIdentifier existingSizeID,
            RecordIdentifier existingColorID,
            RecordIdentifier existingStyleID,
            bool allowSize,
            bool allowStyle);

        /// <summary>
        /// Returns a list of sizes that are not already in a dimension combinations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="selectedColorID">The unique ID of the selected color</param>
        /// <param name="selectedStyleID">The unique ID of the selected style</param>
        /// <param name="existingSizeID">The unique ID of the possible existing size</param>
        /// <param name="existingColorID">The unique ID of the possible existing
        /// color</param>
        /// <param name="existingStyleID">The unique ID of the possible existing
        /// style</param>
        /// <param name="allowColor">If set to <see langword="true" />, then allow color in
        /// combination</param>
        /// <param name="allowStyle">If set to <see langword="true" />, then allow style in
        /// combination</param>
        /// <returns>
        /// A list of colors not included in the dimension
        /// </returns>
        List<DataEntity> GetSizesNotInDimension(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier selectedColorID,
            RecordIdentifier selectedStyleID,
            RecordIdentifier existingSizeID,
            RecordIdentifier existingColorID,
            RecordIdentifier existingStyleID,
            bool allowColor,
            bool allowStyle);

        /// <summary>
        /// Returns a list of styles that are not already in a dimension combinations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="selectedColorID">The unique ID of the selected color</param>
        /// <param name="selectedSizeID">The unique ID of the selected size</param>
        /// <param name="existingSizeID">The unique ID of the possible existing color</param>
        /// <param name="existingColorID">The unique ID of the possible existing
        /// color</param>
        /// <param name="existingStyleID">The unique ID of the possible existing
        /// style</param>
        /// <param name="allowColor">If set to <see langword="true" />, then allow color in
        /// combination</param>
        /// <param name="allowSize">If set to <see langword="true" />, then allow size in
        /// combination</param>
        /// <returns>
        /// A list of colors not included in the dimension
        /// </returns>
        List<DataEntity> GetStylesNotInDimension(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier selectedSizeID,
            RecordIdentifier selectedColorID,
            RecordIdentifier existingSizeID,
            RecordIdentifier existingColorID,
            RecordIdentifier existingStyleID,
            bool allowSize,
            bool allowColor);

        bool ItemHasDimension(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Saves the given dimesion to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimension">The dimension data to be saved</param>
        /// <param name="existingID">If set then the dimension data is updated but if it's
        /// empty new dimension data is created</param>
        void Save(IConnectionManager entry, OldDimension dimension, RecordIdentifier existingID);

        /// <summary>
        /// Returns dimension information for a specific item and specific dimension combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimensionQuadID">The unique ID of a combination that can contain color, size and/or style</param>
        /// <returns>
        /// Returns an instance of <see cref="Dimension"/>
        /// </returns>
        OldDimension Get(IConnectionManager entry, RecordIdentifier dimensionQuadID);

        /// <summary>
        /// Delete the given dimension combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimensionQuadID">The unique ID of a dimension combination</param>
        void Delete(IConnectionManager entry, RecordIdentifier dimensionQuadID);

        /// <summary>
        /// Deletes all dimension combinations for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        void DeleteAllDimensionCombinations(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Saves the given dimesion to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimension">The dimension data to be saved</param>
        void Save(IConnectionManager entry, OldDimension dimension);
    }
}