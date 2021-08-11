using System.Collections.Generic;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.PeriodicDiscounts
{
    /// <summary>
    /// A class used by MixAndMatchMultiEditDialog to group offer lines with line groups
    /// </summary>
    internal class LineGroupLines
    {
        public LineGroupLines(RecordIdentifier lineGroupID, RecordIdentifier offerID, Color lineGroupColor)
        {
            SelectedLines = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>();
            SelectedLines.Add(MultiEditEnums.ItemGroupEnum.Item, Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID, lineGroupID, DiscountOfferLine.DiscountOfferTypeEnum.Item));
            SelectedLines.Add(MultiEditEnums.ItemGroupEnum.RetailDepartment, Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID, lineGroupID, DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment));
            SelectedLines.Add(MultiEditEnums.ItemGroupEnum.RetailGroup, Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID, lineGroupID, DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup));
            SelectedLines.Add(MultiEditEnums.ItemGroupEnum.SpecialGroup, Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID, lineGroupID, DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup));

            LinesToAdd = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>();
            LinesToAdd.Add(MultiEditEnums.ItemGroupEnum.Item, new List<MasterIDEntity>());
            LinesToAdd.Add(MultiEditEnums.ItemGroupEnum.RetailDepartment, new List<MasterIDEntity>());
            LinesToAdd.Add(MultiEditEnums.ItemGroupEnum.RetailGroup, new List<MasterIDEntity>());
            LinesToAdd.Add(MultiEditEnums.ItemGroupEnum.SpecialGroup, new List<MasterIDEntity>());

            LinesToRemove = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>();
            LinesToRemove.Add(MultiEditEnums.ItemGroupEnum.Item, new List<MasterIDEntity>());
            LinesToRemove.Add(MultiEditEnums.ItemGroupEnum.RetailDepartment, new List<MasterIDEntity>());
            LinesToRemove.Add(MultiEditEnums.ItemGroupEnum.RetailGroup, new List<MasterIDEntity>());
            LinesToRemove.Add(MultiEditEnums.ItemGroupEnum.SpecialGroup, new List<MasterIDEntity>());

            LineGroupID = new RecordIdentifier(offerID, lineGroupID);
            LineGroupColor = lineGroupColor;
        }

        public Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> SelectedLines { get; set; }

        public Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> LinesToAdd { get; set; }

        public Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> LinesToRemove { get; set; }

        public Color LineGroupColor { get; set; }

        /// <summary>
        /// The ID of the line group line which is (OfferID, LineGroupID). This is the primary key for a line group
        /// </summary>
        public RecordIdentifier LineGroupID { get; set; }

        public bool IsModified()
        {
            bool added = LinesToAdd[MultiEditEnums.ItemGroupEnum.Item].Count > 0 ||
                         LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count > 0 ||
                         LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup].Count > 0 ||
                         LinesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count > 0;

            bool removed = LinesToRemove[MultiEditEnums.ItemGroupEnum.Item].Count > 0 ||
                           LinesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count > 0 ||
                           LinesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup].Count > 0 ||
                           LinesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count > 0;

            return added || removed;
        }

        public int GetCurrentAddedLinesCount()
        {
            return LinesToAdd[MultiEditEnums.ItemGroupEnum.Item].Count +
                   LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count +
                   LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup].Count +
                   LinesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count;
        }
    }
}
