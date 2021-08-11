using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Replenishment.Containers
{
    public class ItemReplenishmentSettingsContainer
    {
        public ItemReplenishmentSettingsContainer()
        {
            ItemReplenishmentSetting = new List<ItemReplenishmentSetting>();
        }

        public List<ItemReplenishmentSetting> ItemReplenishmentSetting { get; set; }
    }
}
