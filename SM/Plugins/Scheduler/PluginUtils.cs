using System;
using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler
{
    internal static class PluginUtils
    {
        public static List<DatabaseDriverType> LoadSchedulerDriverTypes()
        {
            var driverTypes = new List<DatabaseDriverType>();
            string localizedContent = "";

            foreach (var jscDriverType in DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetDriverTypes(PluginEntry.DataModel))
            {
                localizedContent =
                    Properties.Resources.ResourceManager.GetString(
                        jscDriverType.Name.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", ""),
                        Properties.Resources.Culture);
                //todo implement new type
                var driverType = new DatabaseDriverType
                    {
                        DisplayText = string.IsNullOrEmpty(localizedContent) ? jscDriverType.Name : localizedContent,
                        DataSrvType =  jscDriverType.DatabaseServerType,
                        DatabaseParams = jscDriverType.DatabaseParams,
                        Format = jscDriverType.ConnectionStringFormat,

                    };
                var stringFields = jscDriverType.EnabledFields.Split(',');
                var enumFields = new List<DatabaseStringFields>();
                foreach (var stringField in stringFields)
                {
                    DatabaseStringFields enumField;
                    if (Enum.TryParse(stringField, out enumField))
                    {
                        enumFields.Add(enumField);
                    }
                }
                driverType.EnabledFields = enumFields.ToArray();

                driverTypes.Add(driverType);
            }


            return driverTypes;
        }
    }
}
