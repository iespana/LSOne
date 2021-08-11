using System;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public enum ReplenishmentMethodEnum
    {
         StockLevel,
         LikeForLike
    }

    public enum BlockedForReplenishmentEnum
    {
        NotBlocked,
        BlockedForReplenishment
    }

    public enum PurchaseOrderMultipleRoundingEnum
    {
        Nearest,
        Down,
        Up
    }

    public class ItemReplenishmentSetting
    {
        public ItemReplenishmentSetting()
        {
            ID = Guid.Empty;
            BlockingDate = DateTime.Now;
            StoreId = "";
        }

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier ItemId { get; set; }
        public string ItemName { get; set; }
        public RecordIdentifier StoreId { get; set; }
        public string StoreName { get; set; }
        public ReplenishmentMethodEnum ReplenishmentMethod { get; set; }
        public string ReplenishmentMethodText
        {
            get { return ReplenishmentMethodToText(ReplenishmentMethod); }
        }
        public decimal ReorderPoint { get; set; }
        public decimal MaximumInventory { get; set; }
        public int PurchaseOrderMultiple { get; set; }
        public PurchaseOrderMultipleRoundingEnum PurchaseOrderMultipleRounding { get; set; }
        public string PurchaseOrderMultipleRoundingText
        {
            get { return PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRounding); }
        }
        public BlockedForReplenishmentEnum BlockedForReplenishment { get; set; }
        public string BlockedForReplenishmentText
        {
            get { return BlockedForReplenishmentToText(BlockedForReplenishment); }
        }
        public DateTime BlockingDate { get; set; }

        /// <summary>
        /// This value is read from the Item, changing this value will not be stored in the database in this context
        /// </summary>
        public Unit Unit
        {
            get;
            internal set;
        }

        public static string ReplenishmentMethodToText(ReplenishmentMethodEnum ReplenishmentMethod)
        {
            switch (ReplenishmentMethod)
            {
                case ReplenishmentMethodEnum.StockLevel:
                    return Properties.Resources.StockLevel;
                case ReplenishmentMethodEnum.LikeForLike:
                    return Properties.Resources.LikeForLike;
                default :
                    return "";
            }
        }

        public static string PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum PurchaseOrderMultipleRounding)
        {
            switch (PurchaseOrderMultipleRounding)
            {
                case PurchaseOrderMultipleRoundingEnum.Nearest:
                    return Properties.Resources.Nearest;
                case PurchaseOrderMultipleRoundingEnum.Down:
                    return Properties.Resources.Down;
                case PurchaseOrderMultipleRoundingEnum.Up:
                    return Properties.Resources.Up;
                default:
                    return "";
            }
        }

        public static string BlockedForReplenishmentToText(BlockedForReplenishmentEnum BlockedForReplenishment)
        {
            switch (BlockedForReplenishment)
            {
                case BlockedForReplenishmentEnum.NotBlocked:
                    return Properties.Resources.NotBlocked;
                case BlockedForReplenishmentEnum.BlockedForReplenishment:
                    return Properties.Resources.BlockedForReplenishment;
                default:
                    return "";
            }
        }

    }
}
