using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using LSRetail.SiteService.IntegrationFrameworkRetailItemInterface.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailItem
{
    public partial class IntegrationFrameworkRetailItemPlugin
    {
        /// <inheritdoc/>
        public virtual void SaveAssembly(Assembly assembly)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    SaveIncomingAssembly(dataModel, assembly);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        /// <inheritdoc/>        
        public virtual SaveResult SaveAssemblyList(List<Assembly> assemblies)
        {
            SaveResult result = new SaveResult
			{
				Success = true,
				ErrorInfos = new List<ErrorInfo>()
			};

            IConnectionManager dataModel = GetConnectionManagerIF();

            Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

            foreach(Assembly assembly in assemblies)
            {
                try
                {
                    SaveIncomingAssembly(dataModel, assembly);
                }
                catch(Exception e)
                {
                    string componentIDs = "";
					
					foreach(AssemblyComponent component in assembly.Components)
                    {
						componentIDs += component.ComponentItemID + ";";
                    }

					componentIDs.Trim(';');

					result.Success = false;
					result.ErrorInfos.Add(new ErrorInfo(assembly.AssemblyItemID, $"Components: {componentIDs}", e.ToString()));
                }
            }

            stopwatch.Stop();
			result.ExecutionTime = stopwatch.Elapsed.TotalSeconds;

			return result;
        }
        
        private void SaveIncomingAssembly(IConnectionManager entry, Assembly assembly)
        {
             // Check if an assembly already exists
			RetailItemAssembly activeAssembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(entry, assembly.AssemblyItemID, "");
			bool createNewAssembly = false;

			if(activeAssembly == null)
            {
				createNewAssembly = true;						
            }
			else
            {
				// Compare if we have the exact same assembly, or if we need to deactivate the old one and create a new assembly for the item
				var activeComponents = Providers.RetailItemAssemblyComponentData.GetList(entry, activeAssembly.ID);						

				if(activeComponents.Count != assembly.Components.Count)
                {
					createNewAssembly = true;
                }
				else 
                {
					foreach(AssemblyComponent component in assembly.Components)
                    {
						var existingComponent = activeComponents.FirstOrDefault(p => p.ItemID == component.ComponentItemID);
						createNewAssembly = existingComponent == null || existingComponent.Quantity != component.Quantity;
								
						if(createNewAssembly)
                        {
							break;
                        }
                    }
                }						
            }

			if(createNewAssembly)
            {
				// Create the new assembly
				RetailItemAssembly newAssembly = new RetailItemAssembly();
				newAssembly.ID = Guid.NewGuid();
				newAssembly.ItemID = assembly.AssemblyItemID;
				newAssembly.StoreID = ""; // Assemblies in SAP are not store specific
				newAssembly.ExpandAssembly = ExpandAssemblyLocation.OnPOS | ExpandAssemblyLocation.OnReceipt;
                newAssembly.SendAssemblyComponentsToKds = KitchenDisplayAssemblyComponentType.SendAsItemModifiers;
				newAssembly.CalculatePriceFromComponents = true;
				newAssembly.StartingDate = Date.Now;
				newAssembly.Enabled = true;

				Providers.RetailItemAssemblyData.Save(entry, newAssembly);

				foreach (AssemblyComponent newComponent in assembly.Components)
				{
					RetailItemAssemblyComponent component = new RetailItemAssemblyComponent();
					component.AssemblyID = newAssembly.ID;
					component.ItemID = newComponent.ComponentItemID;
					component.UnitID = Providers.RetailItemData.GetSalesUnitID(entry, newComponent.ComponentItemID);							
					component.Quantity = newComponent.Quantity;

					Providers.RetailItemAssemblyComponentData.Save(entry, component);
				}
            }
        }
    }
}
