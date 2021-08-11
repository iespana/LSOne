namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    /// <summary>
    /// The entry type for inventory templates
    /// Currently only PurchaseOrder, TransferStock and StockCounting are used by LSOne
    /// </summary>
    public enum TemplateEntryTypeEnum
    {
        PurchaseOrder = 0,
        Sale = 1,
        PositiveAdjustment = 2,
        NegativeAdjustment = 3,
        TransferStock = 4,
        StockCounting = 5,
        PriceCheck = 6,
        Labels = 7,
        GoodsReceiving = 8,
        Picking = 9,
        PurchaseReturns = 10,
        StoreRequest = 11,
        Prepack = 12,
    }

    public class TemplateEntryTypeEnumHelper
    {
        public static string ToString(TemplateEntryTypeEnum entryType)
        {
            switch(entryType)
            {
                case TemplateEntryTypeEnum.PurchaseOrder:       return Properties.Resources.PurchaseOrder;
                case TemplateEntryTypeEnum.Sale:                return Properties.Resources.Sale;
                case TemplateEntryTypeEnum.PositiveAdjustment:  return Properties.Resources.PositiveAdjustment;
                case TemplateEntryTypeEnum.NegativeAdjustment:  return Properties.Resources.NegativeAdjustment;
                case TemplateEntryTypeEnum.TransferStock:       return Properties.Resources.TransferStock;
                case TemplateEntryTypeEnum.StockCounting:       return Properties.Resources.StockCounting;
                case TemplateEntryTypeEnum.PriceCheck:          return Properties.Resources.PriceCheck;
                case TemplateEntryTypeEnum.Labels:              return Properties.Resources.Labels;
                case TemplateEntryTypeEnum.GoodsReceiving:      return Properties.Resources.GoodsReceiving;
                case TemplateEntryTypeEnum.Picking:             return Properties.Resources.Picking;
                case TemplateEntryTypeEnum.PurchaseReturns:     return Properties.Resources.PurchaseReturns;
                case TemplateEntryTypeEnum.StoreRequest:        return Properties.Resources.StoreRequest;
                case TemplateEntryTypeEnum.Prepack:             return Properties.Resources.Prepack;
                default: return "";
            }
        }

        public static TemplateEntryTypeEnum ToEnum(string entryType)
        {
            if(entryType == Properties.Resources.PurchaseOrder)         return TemplateEntryTypeEnum.PurchaseOrder;
            if(entryType == Properties.Resources.Sale)                  return TemplateEntryTypeEnum.Sale;              
            if(entryType == Properties.Resources.PositiveAdjustment)    return TemplateEntryTypeEnum.PositiveAdjustment;
            if(entryType == Properties.Resources.NegativeAdjustment)    return TemplateEntryTypeEnum.NegativeAdjustment;
            if(entryType == Properties.Resources.TransferStock)         return TemplateEntryTypeEnum.TransferStock;
            if(entryType == Properties.Resources.StockCounting)         return TemplateEntryTypeEnum.StockCounting;     
            if(entryType == Properties.Resources.PriceCheck)            return TemplateEntryTypeEnum.PriceCheck;     
            if(entryType == Properties.Resources.Labels)                return TemplateEntryTypeEnum.Labels;        
            if(entryType == Properties.Resources.GoodsReceiving)        return TemplateEntryTypeEnum.GoodsReceiving;    
            if(entryType == Properties.Resources.Picking)               return TemplateEntryTypeEnum.Picking;    
            if(entryType == Properties.Resources.PurchaseReturns)       return TemplateEntryTypeEnum.PurchaseReturns;   
            if(entryType == Properties.Resources.StoreRequest)          return TemplateEntryTypeEnum.StoreRequest;   
            if(entryType == Properties.Resources.Prepack)               return TemplateEntryTypeEnum.Prepack;
            return TemplateEntryTypeEnum.PurchaseOrder;
        }
    }
}
