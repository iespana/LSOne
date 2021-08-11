using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;

namespace LSOne.Services.SupportClasses
{
    public class DimensionAttributeExtended : DimensionAttribute
    {
        public bool Selectable { get; set; }
        public bool Selected { get; set; }

        public DimensionAttributeExtended(DimensionAttribute dimensionAttribute, bool selectable, bool selected)
        {
            base.ID = dimensionAttribute.ID;
            base.DimensionID = dimensionAttribute.ID;
            base.Sequence = dimensionAttribute.Sequence;
            base.Text = dimensionAttribute.Text;
            base.Code = dimensionAttribute.Code;
            base.Deleted = dimensionAttribute.Deleted;
            Selectable = selectable;
            Selected = selected;
        }
    }
}
