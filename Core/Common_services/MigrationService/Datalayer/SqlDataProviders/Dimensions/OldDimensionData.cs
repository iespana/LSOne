using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Services.Datalayer.DataProviders.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders.Dimensions
{
    /// <summary>
    /// A data provider that gets data for the dimensions combinations
    /// </summary>
    public class OldDimensionData : SqlServerDataProviderBase, IOldDimensionData
    {
        private class InventDimCombinationSequenceHandler : SqlServerDataProviderBase, ISequenceable
        {
            #region ISequenceable Members
            public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
            {
                return RecordExists(entry, "INVENTDIMCOMBINATION", "RBOVARIANTID", id);
            }

            public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
            {
                throw new NotImplementedException();
            }

            public RecordIdentifier SequenceID
            {
                get { return "Variant_1"; }
            }
            #endregion
        }

        private static void PopulateDimensionCombination(IDataReader dr, OldDimensionCombination dimension)
        {
            dimension.ItemID = (string)dr["ITEMID"];
            dimension.ColorID = (string)dr["INVENTCOLORID"];
            dimension.SizeID = (string)dr["INVENTSIZEID"];
            dimension.StyleID = (string)dr["INVENTSTYLEID"];

            dimension.SizeName = (string)dr["SizeName"];
            dimension.ColorName = (string)dr["ColorName"];
            dimension.StyleName = (string)dr["StyleName"];
        }

        private static void PopulateDimension(IDataReader dr, OldDimension dimension)
        {
            PopulateDimensionCombination(dr, dimension);

            dimension.ColorSortingIndex = (int) dr["COLORSORTINGINDEX"];
            dimension.StyleSortingIndex = (int) dr["STYLESORTINGINDEX"];
            dimension.SizeSortingIndex = (int) dr["SIZESORTINGINDEX"];

            dimension.Text = (string)dr["NAME"];
            dimension.VariantNumber = (string)dr["RBOVARIANTID"];
            dimension.DimensionID = (string)dr["INVENTDIMID"];
        }

        /// <summary>
        /// Returns a dimension by variant ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="variantNumber">The unique ID of the variant that is to be searched
        /// for</param>
        /// <returns>
        /// Returns an instance of <see cref="Dimension"/>
        /// </returns>
        public virtual OldDimension GetByVariantID(IConnectionManager entry, RecordIdentifier variantNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "select i.ITEMID, i.INVENTCOLORID,i.INVENTSIZEID,i.INVENTSTYLEID," +
                    "ISNULL(i.NAME,'') as NAME,ISNULL(i.RBOVARIANTID,'') as RBOVARIANTID," +
                    "ISNULL(c.NAME,'') as ColorName,ISNULL(s.NAME,'') as SizeName,ISNULL(t.NAME,'') as StyleName," +
                    "ISNULL(c.WEIGHT, 0) AS COLORSORTINGINDEX, ISNULL(s.WEIGHT, 0) AS STYLESORTINGINDEX, " +
                    "ISNULL(t.WEIGHT, 0) AS SIZESORTINGINDEX, " +
                    "ISNULL(i.INVENTDIMID,'') as INVENTDIMID " +
                    "from INVENTDIMCOMBINATION i " +
                    "LEFT JOIN RBOINVENTTABLE ri on ri.ITEMID = i.ITEMID " +
                    "LEFT OUTER JOIN RBOCOLORGROUPTRANS c ON i.INVENTCOLORID = c.COLOR AND i.DATAAREAID = c.DATAAREAID AND ri.COLORGROUP = c.COLORGROUP " +
                    "LEFT OUTER JOIN RBOSIZEGROUPTRANS s ON i.INVENTSIZEID = s.SIZE_ AND i.DATAAREAID = s.DATAAREAID AND ri.SIZEGROUP = s.SIZEGROUP " +
                    "LEFT OUTER JOIN RBOSTYLEGROUPTRANS t ON i.INVENTSTYLEID = t.STYLE AND i.DATAAREAID = t.DATAAREAID AND ri.STYLEGROUP = t.STYLEGROUP " +
                    "where i.DATAAREAID = @dataAreaId and " +
                    "i.RBOVARIANTID = @variantNumber";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "variantNumber", (string) variantNumber);

                var dimensions = Execute<OldDimension>(entry, cmd, CommandType.Text, PopulateDimension);

                return dimensions.Count > 0 ? dimensions[0] : new OldDimension();
            }
        }

        /// <summary>
        /// Returns dimension information for a specific item and specific variant number
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="variantNumber">The unique ID of the variant</param>
        /// <returns>
        /// Returns an instance of <see cref="Dimension"/>
        /// </returns>
        public virtual OldDimension Get(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier variantNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "select i.ITEMID, i.INVENTCOLORID,i.INVENTSIZEID,i.INVENTSTYLEID," +
                    "ISNULL(i.NAME,'') as NAME,ISNULL(i.RBOVARIANTID,'') as RBOVARIANTID," +
                    "ISNULL(c.NAME,'') as ColorName,ISNULL(s.NAME,'') as SizeName,ISNULL(t.NAME,'') as StyleName, " +
                    "ISNULL(c.WEIGHT, 0) AS COLORSORTINGINDEX, ISNULL(s.WEIGHT, 0) AS STYLESORTINGINDEX, " +
                    "ISNULL(t.WEIGHT, 0) AS SIZESORTINGINDEX, " +
                    "ISNULL(i.INVENTDIMID,'') as INVENTDIMID " +
                    "from INVENTDIMCOMBINATION i " +
                    "LEFT JOIN RBOINVENTTABLE ri on ri.ITEMID = i.ITEMID " +
                    "LEFT OUTER JOIN RBOCOLORGROUPTRANS c ON i.INVENTCOLORID = c.COLOR AND i.DATAAREAID = c.DATAAREAID AND ri.COLORGROUP = c.COLORGROUP " +
                    "LEFT OUTER JOIN RBOSIZEGROUPTRANS s ON i.INVENTSIZEID = s.SIZE_ AND i.DATAAREAID = s.DATAAREAID AND ri.SIZEGROUP = s.SIZEGROUP " +
                    "LEFT OUTER JOIN RBOSTYLEGROUPTRANS t ON i.INVENTSTYLEID = t.STYLE AND i.DATAAREAID = t.DATAAREAID AND ri.STYLEGROUP = t.STYLEGROUP " +
                    "where i.DATAAREAID = @dataAreaId and i.ITEMID = @itemID and " +
                    "i.RBOVARIANTID = @variantNumber";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) itemID);
                MakeParam(cmd, "variantNumber", (string) variantNumber);

                var dimensions = Execute<OldDimension>(entry, cmd, CommandType.Text, PopulateDimension);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        /// <summary>
        /// Returns dimension information for a specific item and specific dimension combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimensionQuadID">The unique ID of a combination that can contain color, size and/or style</param>
        /// <returns>
        /// Returns an instance of <see cref="Dimension"/>
        /// </returns>
        public virtual OldDimension Get(IConnectionManager entry, RecordIdentifier dimensionQuadID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "select i.ITEMID, i.INVENTCOLORID,i.INVENTSIZEID,i.INVENTSTYLEID," +
                    "ISNULL(i.NAME,'') as NAME,ISNULL(i.RBOVARIANTID,'') as RBOVARIANTID," +
                    "ISNULL(c.NAME,'') as ColorName,ISNULL(s.NAME,'') as SizeName,ISNULL(t.NAME,'') as StyleName," +
                    "ISNULL(c.WEIGHT, 0) AS COLORSORTINGINDEX, ISNULL(s.WEIGHT, 0) AS STYLESORTINGINDEX, " +
                    "ISNULL(t.WEIGHT, 0) AS SIZESORTINGINDEX, " +
                    "ISNULL(i.INVENTDIMID,'') as INVENTDIMID " +
                    "from INVENTDIMCOMBINATION i " +
                    "LEFT JOIN RBOINVENTTABLE ri on ri.ITEMID = i.ITEMID " +
                    "LEFT OUTER JOIN RBOCOLORGROUPTRANS c ON i.INVENTCOLORID = c.COLOR AND i.DATAAREAID = c.DATAAREAID AND ri.COLORGROUP = c.COLORGROUP " +
                    "LEFT OUTER JOIN RBOSIZEGROUPTRANS s ON i.INVENTSIZEID = s.SIZE_ AND i.DATAAREAID = s.DATAAREAID AND ri.SIZEGROUP = s.SIZEGROUP " +
                    "LEFT OUTER JOIN RBOSTYLEGROUPTRANS t ON i.INVENTSTYLEID = t.STYLE AND i.DATAAREAID = t.DATAAREAID AND ri.STYLEGROUP = t.STYLEGROUP " +
                    "where i.DataareaID = @dataAreaId and i.ITEMID = @itemID and " +
                    "i.INVENTSIZEID = @sizeID and i.INVENTCOLORID = @colorID and i.INVENTSTYLEID = @styleID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) dimensionQuadID);
                MakeParam(cmd, "sizeID", (string) dimensionQuadID.SecondaryID);
                MakeParam(cmd, "colorID", (string) dimensionQuadID.SecondaryID.SecondaryID);
                MakeParam(cmd, "styleID", (string) dimensionQuadID.SecondaryID.SecondaryID.SecondaryID);

                var dimensions = Execute<OldDimension>(entry, cmd, CommandType.Text, PopulateDimension);

                return dimensions.Count > 0 ? dimensions[0] : null;
            }
        }

        /// <summary>
        /// Returns a variant ID that is associated with a dimension ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimensionID">The unique ID of a dimension</param>
        /// <returns>
        /// Returns a unique variant ID
        /// </returns>
        public virtual RecordIdentifier GetVariantIDFromDimID(IConnectionManager entry, RecordIdentifier dimensionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select RBOVARIANTID from INVENTDIMCOMBINATION " +
                    "where INVENTDIMID = @inventDimID and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "inventDimID", (string) dimensionID);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return (string) dr["RBOVARIANTID"];
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }

                return "";
            }
        }

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
        public virtual List<OldDimension> GetList(IConnectionManager entry, RecordIdentifier itemID, int sortColumn, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sort;
                switch (sortColumn)
                {
                    case 0:
                        sort = "NAME ASC";
                        break;

                    case 1:
                        sort = "INVENTCOLORID ASC";
                        break;

                    case 2:
                        sort = "INVENTSIZEID ASC";
                        break;

                    case 3:
                        sort = "INVENTSTYLEID ASC";
                        break;

                    case 4:
                        sort = "RBOVARIANTID ASC";
                        break;

                    default:
                        sort = "";
                        break;
                }

                if (sort != "")
                {
                    if (backwardsSort)
                    {
                        sort = sort.Replace("ASC", "DESC");
                    }

                    sort = " order by " + sort;
                }

                ValidateSecurity(entry);

                cmd.CommandText =
                    "select i.ITEMID, i.INVENTCOLORID,i.INVENTSIZEID,i.INVENTSTYLEID," +
                    "ISNULL(i.NAME,'') as NAME,ISNULL(i.RBOVARIANTID,'') as RBOVARIANTID," +
                    "ISNULL(c.NAME,'') as ColorName,ISNULL(s.NAME,'') as SizeName,ISNULL(t.NAME,'') as StyleName," +
                    "ISNULL(c.WEIGHT, 0) AS COLORSORTINGINDEX, ISNULL(t.WEIGHT, 0) AS STYLESORTINGINDEX, " +
                    "ISNULL(s.WEIGHT, 0) AS SIZESORTINGINDEX, " +
                    "ISNULL(i.INVENTDIMID,'') as INVENTDIMID " +
                    "from INVENTDIMCOMBINATION i " +
                    "LEFT JOIN RBOINVENTTABLE ri on ri.DATAAREAID = i.DATAAREAID AND ri.ITEMID = i.ITEMID " +
                    "LEFT OUTER JOIN RBOCOLORGROUPTRANS c ON i.INVENTCOLORID = c.COLOR AND i.DATAAREAID = c.DATAAREAID AND ri.COLORGROUP = c.COLORGROUP " +
                    "LEFT OUTER JOIN RBOSIZEGROUPTRANS s ON i.INVENTSIZEID = s.SIZE_ AND i.DATAAREAID = s.DATAAREAID AND ri.SIZEGROUP = s.SIZEGROUP " +
                    "LEFT OUTER JOIN RBOSTYLEGROUPTRANS t ON i.INVENTSTYLEID = t.STYLE AND i.DATAAREAID = t.DATAAREAID AND ri.STYLEGROUP = t.STYLEGROUP " +
                    "where i.DataareaID = @dataAreaId and i.ITEMID = @itemID" + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) itemID);

                return Execute<OldDimension>(entry, cmd, CommandType.Text, PopulateDimension);
            }
        }

        /// <summary>
        /// Returns a list of possible dimensions that can be used to create combinations on
        /// the item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// A list of possible dimension combinations for the item
        /// </returns>
        public virtual List<OldDimensionCombination> GetCreatableDimensionCombinations(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select k.ITEMID, k.INVENTSIZEID ,k.INVENTCOLORID,k.INVENTSTYLEID, " +
                    "ISNULL(s.NAME,'') as SizeName, ISNULL(c.NAME,'') as ColorName, ISNULL(st.NAME,'') as StyleName " +
                    "from (" +
                    "Select it.ITEMID,ISNULL(sg.SIZE_,'') as INVENTSIZEID,ISNULL(cg.COLOR,'') as INVENTCOLORID," +
                    "ISNULL(stg.STYLE,'') as INVENTSTYLEID,it.DATAAREAID " +
                    "from RBOINVENTTABLE it " +
                    "left outer join RBOSTYLEGROUPTRANS stg on stg.STYLEGROUP = it.STYLEGROUP and stg.DATAAREAID = it.DATAAREAID " +
                    "left outer join RBOSIZEGROUPTRANS sg on sg.SIZEGROUP = it.SIZEGROUP and sg.DATAAREAID = it.DATAAREAID " +
                    "left outer join RBOCOLORGROUPTRANS cg on cg.COLORGROUP = it.COLORGROUP and cg.DATAAREAID = it.DATAAREAID " +
                    "where it.ITEMID = @itemID and it.DataareaID = @dataAreaId " +
                    "Except " +
                    "select ITEMID, INVENTSIZEID ,INVENTCOLORID,INVENTSTYLEID,DATAAREAID " +
                    "from INVENTDIMCOMBINATION " +
                    "where ITEMID = @itemID and DataareaID = @dataAreaId" +
                    ") k " +
                    "left outer join RBOSIZES s on k.INVENTSIZEID = s.SIZE_ and s.DATAAREAID = k.DATAAREAID " +
                    "left outer join RBOCOLORS c on k.INVENTCOLORID = c.COLOR and c.DATAAREAID = k.DATAAREAID " +
                    "left outer join RBOSTYLES st on k.INVENTSTYLEID = st.STYLE and st.DATAAREAID = k.DATAAREAID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) itemID);

                return Execute<OldDimensionCombination>(entry, cmd, CommandType.Text, PopulateDimensionCombination);
            }
        }

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
        public List<DataEntity> GetColorsNotInDimension(
            IConnectionManager entry, 
            RecordIdentifier itemID,
            RecordIdentifier selectedSizeID,
            RecordIdentifier selectedStyleID,
            RecordIdentifier existingSizeID,
            RecordIdentifier existingColorID,
            RecordIdentifier existingStyleID,
            bool allowSize,
            bool allowStyle)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var includePart = "";
                var includePart2 = "";
                var joinPart = "";
                var existingCondition = "";
                var prefilterCondition = "";

                ValidateSecurity(entry);

                if (allowSize)
                {
                    includePart += ",s.SIZE_ as INVENTSIZEID";
                    includePart2 += ",INVENTSIZEID";
                    joinPart += "join RBOSIZES s on c.DATAAREAID = s.DATAAREAID ";
                    existingCondition += " and INVENTSIZEID <> @existingSize";

                    if (selectedSizeID != "") prefilterCondition += " and s.SIZE_ = @selectedSizeID ";
                    
                    MakeParam(cmd, "existingSize", (string)existingSizeID);
                    MakeParam(cmd, "selectedSizeID", (string)selectedSizeID);
                }

                if (allowStyle)
                {
                    includePart += ",st.STYLE as INVENTSTYLEID";
                    includePart2 += ",INVENTSTYLEID";
                    joinPart += "join RBOSTYLES st on c.DATAAREAID = st.DATAAREAID ";
                    existingCondition += " and INVENTSTYLEID <> @existingStyle";
                    if (selectedStyleID != "") prefilterCondition += " and st.STYLE = @selectedStyleID ";

                    MakeParam(cmd, "existingStyle", (string)existingStyleID);
                    MakeParam(cmd, "selectedStyleID", (string)selectedStyleID);
                }

                cmd.CommandText =
                    "select distinct k.INVENTCOLORID,ISNULL(m.NAME,'') as NAME from (" +
                    "select c.Color as INVENTCOLORID" + includePart + " " +
                    "from RBOCOLORS c " + joinPart +
                    "where c.DataareaID = @dataAreaId " + prefilterCondition +
                    "Except " +
                    "select INVENTCOLORID" + includePart2 +
                    " from INVENTDIMCOMBINATION where ITEMID = @itemID and DataareaID = @dataAreaId " +
                    "and (INVENTCOLORID <> @existingColor" + existingCondition + ") " +
                    ") k left outer join RBOCOLORS m on m.COLOR = k.INVENTCOLORID order by k.INVENTCOLORID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) itemID);
                MakeParam(cmd, "existingColor", (string) existingColorID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "INVENTCOLORID");
            }
        }

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
        public List<DataEntity> GetSizesNotInDimension(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier selectedColorID,
            RecordIdentifier selectedStyleID,
            RecordIdentifier existingSizeID,
            RecordIdentifier existingColorID,
            RecordIdentifier existingStyleID,
            bool allowColor,
            bool allowStyle)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var includePart = "";
                var includePart2 = "";
                var joinPart = "";
                var existingCondition = "";
                var prefilterCondition = "";

                ValidateSecurity(entry);

                if (allowColor)
                {
                    includePart += ",s.COLOR as INVENTCOLORID";
                    includePart2 += ",INVENTCOLORID";
                    joinPart += "join RBOCOLORS s on c.DATAAREAID = s.DATAAREAID ";
                    existingCondition += " and INVENTCOLORID <> @existingColor";
                    if (selectedColorID != "") prefilterCondition += " and s.COLOR = @selectedColorID ";

                    MakeParam(cmd, "existingColor", (string)existingColorID);
                    MakeParam(cmd, "selectedColorID", (string)selectedColorID);
                }

                if (allowStyle)
                {
                    includePart += ",st.STYLE as INVENTSTYLEID";
                    includePart2 += ",INVENTSTYLEID";
                    joinPart += "join RBOSTYLES st on c.DATAAREAID = st.DATAAREAID ";
                    existingCondition += " and INVENTSTYLEID <> @existingStyle";
                    if (selectedStyleID != "") prefilterCondition += " and st.STYLE = @selectedStyleID ";

                    MakeParam(cmd, "existingStyle", (string)existingStyleID);
                    MakeParam(cmd, "selectedStyleID", (string)selectedStyleID);
                }

                cmd.CommandText =
                    "select distinct k.INVENTSIZEID,ISNULL(m.NAME,'') as NAME from (" +
                    "select c.Size_ as INVENTSIZEID" + includePart + " " +
                    "from RBOSIZES c " + joinPart +
                    "where c.DataareaID = @dataAreaId " + prefilterCondition +
                    "Except " +
                    "select INVENTSIZEID" + includePart2 +
                    " from INVENTDIMCOMBINATION where ITEMID = @itemID and DataareaID = @dataAreaId " +
                    "and (INVENTSIZEID <> @existingSize" + existingCondition + ") " +
                    ") k left outer join RBOSIZES m on m.Size_ = k.INVENTSIZEID order by k.INVENTSIZEID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) itemID);
                MakeParam(cmd, "existingSize", (string) existingSizeID);
                

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "INVENTSIZEID");
            }
        }

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
        public List<DataEntity> GetStylesNotInDimension(
           IConnectionManager entry,
           RecordIdentifier itemID,
           RecordIdentifier selectedSizeID,
           RecordIdentifier selectedColorID,
           RecordIdentifier existingSizeID,
           RecordIdentifier existingColorID,
           RecordIdentifier existingStyleID,
           bool allowSize,
           bool allowColor)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var includePart = "";
                var includePart2 = "";
                var joinPart = "";
                var existingCondition = "";
                var prefilterCondition = "";

                ValidateSecurity(entry);

                if (allowSize)
                {
                    includePart += ",s.SIZE_ as INVENTSIZEID";
                    includePart2 += ",INVENTSIZEID";
                    joinPart += "join RBOSIZES s on c.DATAAREAID = s.DATAAREAID ";
                    existingCondition += " and INVENTSIZEID <> @existingSize";
                    if (selectedSizeID != "") prefilterCondition += " and s.SIZE_ = @selectedSizeID ";

                    MakeParam(cmd, "selectedSizeID", (string)selectedSizeID);
                    MakeParam(cmd, "existingSize", (string)existingSizeID);
                }

                if (allowColor)
                {
                    includePart += ",st.COLOR as INVENTCOLORID";
                    includePart2 += ",INVENTCOLORID";
                    joinPart += "join RBOCOLORS st on c.DATAAREAID = st.DATAAREAID ";
                    existingCondition += " and INVENTCOLORID <> @existingColor";
                    if (selectedColorID != "") prefilterCondition += " and st.COLOR = @selectedColorID ";
                   
                    MakeParam(cmd, "existingColor", (string)existingColorID);
                    MakeParam(cmd, "selectedColorID", (string)selectedColorID);
                }

                cmd.CommandText =
                    "select distinct k.INVENTSTYLEID,ISNULL(m.NAME,'') as NAME from (" +
                    "select c.Style as INVENTSTYLEID" + includePart + " " +
                    "from RBOSTYLES c " + joinPart +
                    "where c.DataareaID = @dataAreaId " + prefilterCondition +
                    "Except " +
                    "select INVENTSTYLEID" + includePart2 +
                    " from INVENTDIMCOMBINATION where ITEMID = @itemID and DataareaID = @dataAreaId " +
                    "and (INVENTSTYLEID <> @existingStyle" + existingCondition + ") " +
                    ") k left outer join RBOSTYLES m on m.Style = k.INVENTSTYLEID order by k.INVENTSTYLEID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string) itemID);
                MakeParam(cmd, "existingStyle", (string) existingStyleID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "INVENTSTYLEID");
            }
        }

        /// <summary>
        /// Delete the given dimension combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimensionQuadID">The unique ID of a dimension combination</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier dimensionQuadID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

                cmd.CommandText = "Delete from INVENTDIM " +
                    "where INVENTDIMID = (" +
                        "Select INVENTDIMID from INVENTDIMCOMBINATION " +
                        "where ITEMID = @itemID " +
                        "and INVENTSIZEID = @sizeID " +
                        "and INVENTCOLORID = @colorID " +
                        "and INVENTSTYLEID = @styleID " +
                        "and DATAAREAID = @dataAreaId)";

                MakeParam(cmd, "itemID", (string)dimensionQuadID.PrimaryID);
                MakeParam(cmd, "sizeID", (string)dimensionQuadID.SecondaryID);
                MakeParam(cmd, "colorID", (string)dimensionQuadID.SecondaryID.SecondaryID);
                MakeParam(cmd, "styleID", (string)dimensionQuadID.SecondaryID.SecondaryID.SecondaryID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                entry.Connection.DeleteFromTable(cmd, "INVENTDIM", CommandType.Text);

                DeleteRecord(entry, "INVENTDIMCOMBINATION", new[] { "ITEMID", "INVENTSIZEID", "INVENTCOLORID", "INVENTSTYLEID" }, dimensionQuadID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
            }
        }

        /// <summary>
        /// Deletes all dimension combinations for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        public virtual void DeleteAllDimensionCombinations(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

                cmd.CommandText =
                  @"delete from INVENTDIM
                    where DATAAREAID = @dataAreaId and INVENTDIMID in 
                    (
                      select i.INVENTDIMID
                      from INVENTDIMCOMBINATION i
                      where i.ITEMID =  @itemID
                    )";

                MakeParam(cmd, "itemID", (string)itemID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                entry.Connection.DeleteFromTable(cmd, "INVENTDIM", CommandType.Text);

                DeleteRecord(entry, "INVENTDIMCOMBINATION", "ITEMID", itemID, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);
            }
        }

        public virtual bool ItemHasDimension(IConnectionManager entry, RecordIdentifier itemID)
        {
            return RecordExists(entry, "INVENTDIMCOMBINATION", "ITEMID", itemID);
        }

        private static bool InventDimExists(IConnectionManager entry, RecordIdentifier dimensionID)
        {
            return RecordExists(entry, "INVENTDIM", "INVENTDIMID", dimensionID);
        }

        /// <summary>
        /// Saves the given dimesion to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimension">The dimension data to be saved</param>
        /// <param name="existingID">If set then the dimesion data is updated but if it's
        /// empty new dimension data is created</param>
        public virtual void Save(IConnectionManager entry,OldDimension dimension,RecordIdentifier existingID)
        {
            var statement = new SqlServerStatement("INVENTDIMCOMBINATION");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

            if (dimension.DimensionID == "")
            {
                dimension.DimensionID = DataProviderFactory.Instance.GenerateNumber<IOldDimensionData, OldDimension>(entry); 
            }

            if (existingID == RecordIdentifier.Empty || existingID == "")
            {
                statement.StatementType = StatementType.Insert;

                var variantID = DataProviderFactory.Instance.GenerateNumber(entry, new InventDimCombinationSequenceHandler()); 
                
                if (variantID == RecordIdentifier.Empty)
                {
                    throw new Exception("Unable to generate VariantID");
                }

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ITEMID", (string)dimension.ItemID);
                statement.AddKey("RBOVARIANTID", (string)variantID);

                statement.AddKey("INVENTSIZEID", (string)dimension.SizeID);
                statement.AddKey("INVENTCOLORID", (string)dimension.ColorID);
                statement.AddKey("INVENTSTYLEID", (string)dimension.StyleID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)existingID);
                statement.AddCondition("INVENTSIZEID", (string)existingID.SecondaryID);
                statement.AddCondition("INVENTCOLORID", (string)existingID.SecondaryID.SecondaryID);
                statement.AddCondition("INVENTSTYLEID", (string)existingID.SecondaryID.SecondaryID.SecondaryID);

                statement.AddField("INVENTSIZEID", (string)dimension.SizeID);
                statement.AddField("INVENTCOLORID", (string)dimension.ColorID);
                statement.AddField("INVENTSTYLEID", (string)dimension.StyleID);
            }

            statement.AddField("NAME", dimension.Text);
            statement.AddField("INVENTDIMID", (string)dimension.DimensionID);

            entry.Connection.ExecuteStatement(statement);

            // We have no choice we have to write also to the useless INVENTDIM table which is not used
            // for anything except to map INVENTDIMID to SIZE,COLOR,STYLE and VariantID. 

            statement = new SqlServerStatement("INVENTDIM");

            if (!InventDimExists(entry, dimension.DimensionID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddField("INVENTDIMID", (string)dimension.DimensionID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddCondition("INVENTDIMID", (string)dimension.DimensionID);
            }

            statement.AddField("INVENTSIZEID", (string)dimension.SizeID);
            statement.AddField("INVENTCOLORID", (string)dimension.ColorID);
            statement.AddField("INVENTSTYLEID", (string)dimension.StyleID);

            entry.Connection.ExecuteStatement(statement);
        }

        private bool InventDimCombinationExists(IConnectionManager entry, OldDimension dimension)
        {

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    @"select 
                        i.ITEMID
                            from INVENTDIMCOMBINATION i 
                            LEFT JOIN RBOINVENTTABLE ri on ri.DATAAREAID = i.DATAAREAID AND ri.ITEMID = i.ITEMID 
                            LEFT OUTER JOIN RBOCOLORGROUPTRANS c ON i.INVENTCOLORID = c.COLOR AND i.DATAAREAID = c.DATAAREAID AND ri.COLORGROUP = c.COLORGROUP 
                            LEFT OUTER JOIN RBOSIZEGROUPTRANS s ON i.INVENTSIZEID = s.SIZE_ AND i.DATAAREAID = s.DATAAREAID AND ri.SIZEGROUP = s.SIZEGROUP 
                            LEFT OUTER JOIN RBOSTYLEGROUPTRANS t ON i.INVENTSTYLEID = t.STYLE AND i.DATAAREAID = t.DATAAREAID AND ri.STYLEGROUP = t.STYLEGROUP 
                            where i.DataareaID = @dataAreaId and i.ITEMID = @itemID and 
                            i.INVENTSIZEID = @sizeID and i.INVENTCOLORID = @colorID and i.INVENTSTYLEID = @styleID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string)dimension.ItemID);
                MakeParam(cmd, "sizeID", (string)dimension.SizeID);
                MakeParam(cmd, "colorID", (string)dimension.ColorID);
                MakeParam(cmd, "styleID", (string)dimension.StyleID);

                var dimensions = Execute(entry, cmd, CommandType.Text, "ITEMID");

                return dimensions.Count > 0;
            }
        }


        /// <summary>
        /// Saves the given dimesion to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="dimension">The dimension data to be saved</param>
        public virtual void Save(IConnectionManager entry, OldDimension dimension)
        {
            Dictionary<string, long > metrics = new Dictionary<string, long>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var statement = entry.Connection.CreateStatement("INVENTDIMCOMBINATION");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ColorSizeStyleEdit);

            bool isNew = false;
            if (dimension.DimensionID == RecordIdentifier.Empty || dimension.DimensionID == "" || dimension.DimensionID.IsEmpty)
            {
                isNew = true;
                dimension.DimensionID = DataProviderFactory.Instance.GenerateNumber<IOldDimensionData, OldDimension>(entry);
            }
            if (isNew || !InventDimCombinationExists(entry, dimension))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ITEMID", (string)dimension.ItemID);
                statement.AddKey("RBOVARIANTID", (string)dimension.DimensionID);

                statement.AddKey("INVENTSIZEID", (string)dimension.SizeID);
                statement.AddKey("INVENTCOLORID", (string)dimension.ColorID);
                statement.AddKey("INVENTSTYLEID", (string)dimension.StyleID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)dimension.ItemID);
                statement.AddCondition("INVENTSIZEID", (string)dimension.SizeID);
                statement.AddCondition("INVENTCOLORID", (string)dimension.ColorID);
                statement.AddCondition("INVENTSTYLEID", (string)dimension.StyleID);

            }

            statement.AddField("NAME", dimension.Text);
            statement.AddField("INVENTDIMID", (string)dimension.DimensionID);
            //if (statement.StatementType == StatementType.Update)
            //{
            //    statement.Command.CommandText += "and (NAME<>@NAME or INVENTDIMID<>@INVENTDIMID)";

            //}
            entry.Connection.ExecuteStatement(statement);

            // We have no choice we have to write also to the useless INVENTDIM table which is not used
            // for anything except to map INVENTDIMID to SIZE,COLOR,STYLE and VariantID. 


            statement = entry.Connection.CreateStatement("INVENTDIM");

            if (!InventDimExists(entry, dimension.DimensionID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddField("INVENTDIMID", (string)dimension.DimensionID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddCondition("INVENTDIMID", (string)dimension.DimensionID);
            }

            statement.AddField("INVENTSIZEID", (string)dimension.SizeID);
            statement.AddField("INVENTCOLORID", (string)dimension.ColorID);
            statement.AddField("INVENTSTYLEID", (string)dimension.StyleID);
            entry.Connection.ExecuteStatement(statement);
        }    


        #region ISequenceable Members
        /// <summary>
        /// Returns true if the unique ID already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The unique sequence ID to search for</param>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return InventDimExists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a unique ID for the class
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "INV_3"; }
        }
        #endregion
    }
}
