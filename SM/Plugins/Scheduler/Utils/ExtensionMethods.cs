using System;
using LSRetail.Scheduler.Model;
using LSRetail.StoreController.BusinessObjects;
using LSRetail.DD.Common.Data;

namespace LSRetail.SiteManager.Plugins.Scheduler.Utils
{
    public static class ExtensionMethods
    {
        public static DataEntity[] ToDataEntities(this DataSelector[] dataSelectors, Func<DataSelector, DataEntity> converter)
        {
            if (dataSelectors == null)
                return null;

            DataEntity[] entities = new DataEntity[dataSelectors.Length];
            for (int i = 0; i < dataSelectors.Length; i++)
            {
                entities[i] = converter(dataSelectors[i]);
            }
            return entities;
        }
    }
}
