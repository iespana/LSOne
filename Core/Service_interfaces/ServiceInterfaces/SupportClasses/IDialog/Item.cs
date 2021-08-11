using System;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.Services.Interfaces.Enums;
using PriceHandlings = LSOne.Services.Interfaces.Enums.PriceHandlings;

namespace LSOne.Services.Interfaces.SupportClasses.IDialog
{
    public class Item
    {
        public Item()
        {
            GroupId = "";
            ItemId = "";
            Text = "";
            NumberOfClicks = 0;
            PrevSelection = 0;
            Index = -1;
            IsVariantItem = false;
            VariantId = "";
            Dimension1 = "";
            Dimension2 = "";
            Dimension3 = "";
            PrimaryKey = null;
        }

        public int Index { get; set; }
        public string GroupId { get; set; }
        public string ItemId { get; set; }
        public string Text { get; set; }
        public int NumberOfClicks { get; set; }
        public int PrevSelection { get; set; }
        public int MaxSelection { get; set; }

        public bool IsVariantItem { get; set; }
        public string VariantId { get; set; }
        public string Dimension1 { get; set; }
        public string Dimension2 { get; set; }
        public string Dimension3 { get; set; }
        public UsageCategories UseageCategory { get; set; }
        public PriceTypes PriceType { get; set; }
        public PriceHandlings PriceHandling { get; set; }
        public Decimal AmountPercentage { get; set; }

        public object PrimaryKey { get; set; }
    }
}
