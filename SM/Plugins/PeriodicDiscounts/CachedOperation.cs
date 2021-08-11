using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.PeriodicDiscounts
{
    internal class CachedOperation
    {
        public CachedOperation(DataEntity entity, bool isDeleteOperation)
        {
            Entity = entity;
            IsDeleteOperation = isDeleteOperation;
        }

        public DataEntity Entity { get; set; }
        public bool IsDeleteOperation { get; set; }
    }
}
