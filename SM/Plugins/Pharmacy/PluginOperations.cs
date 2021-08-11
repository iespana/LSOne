using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Pharmacy
{
	internal class PluginOperations
	{
		internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
		{
			if (args.ContextName == "LSOne.ViewPlugins.Profiles.Views.HardwareProfileView")
			{
				if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManagePosHardwareProfile))
				{
					args.Add(new TabControl.Tab("Pharmacy", new PanelFactoryHandler(ViewPages.HardwareProfilePharmacyPage.CreateInstance)), 250);
				}
			}
		}
	}
}
