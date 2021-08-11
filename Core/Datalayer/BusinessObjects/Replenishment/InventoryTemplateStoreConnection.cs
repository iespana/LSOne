using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class InventoryTemplateStoreConnection : DataEntity
    {
        public InventoryTemplateStoreConnection()
        {
            ID = Guid.NewGuid();
        }

        public RecordIdentifier TemplateID { get; set; }
        public RecordIdentifier StoreID { get; set; }

        public override object Clone()
        {
            InventoryTemplateStoreConnection connection = new InventoryTemplateStoreConnection();
            base.Populate(connection);
            Populate(connection);
            return connection;
        }

        protected void Populate(InventoryTemplateStoreConnection item)
        {
            item.TemplateID = this.TemplateID;
            item.StoreID = this.StoreID;
        }
    }
}
