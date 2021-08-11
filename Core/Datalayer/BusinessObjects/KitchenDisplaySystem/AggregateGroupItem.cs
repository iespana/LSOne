using LSOne.DataLayer.KDSBusinessObjects;


namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class AggregateGroupItem : KDSBusinessObjects.AggregateGroupItem
    {
        /// <summary>
        /// The item relation type
        /// </summary>
        public TypeEnum Type;

        public enum TypeEnum
        {
            Item,
            RetailGroup,
            SpecialGroup
        }

        public string TypeAsString()
        {
            switch (Type)
            {
                case TypeEnum.Item:
                    return Properties.Resources.Item;
                case TypeEnum.RetailGroup:
                    return Properties.Resources.RetailGroup;
                case TypeEnum.SpecialGroup:
                    return Properties.Resources.SpecialGroup;
                default:
                    return string.Empty;
            }
        }
    }
}