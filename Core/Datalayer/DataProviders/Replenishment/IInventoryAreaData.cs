using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IInventoryAreaData : IDataProvider<InventoryArea>
    {
        InventoryArea Get(IConnectionManager entry, RecordIdentifier areaID, UsageIntentEnum usage = UsageIntentEnum.Normal);

        List<InventoryArea> GetList(IConnectionManager entry);

        void DeleteLine(IConnectionManager entry, RecordIdentifier lineID);

        void SaveLine(IConnectionManager entry, InventoryAreaLine line);

        InventoryAreaLine GetLine(IConnectionManager entry, RecordIdentifier masterID);

        List<InventoryAreaLine> GetLinesByArea(IConnectionManager entry, RecordIdentifier areaID);

        List<InventoryAreaLineListItem> GetAllAreaLines(IConnectionManager entry);

        bool AreaInUse(IConnectionManager entry, RecordIdentifier areaLineID);
    }
}
