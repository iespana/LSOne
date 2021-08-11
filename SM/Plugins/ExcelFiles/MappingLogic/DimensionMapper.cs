using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    public class DimensionMapper
    {
        public enum ErrorCode
        {
            None,
            AttributesCannotBeEmpty,
            DimensionsAlreadyDefined,
            AttributeCountMismatch,
            AttributeCombinationAlreadyExists,
        }

        internal class DimensionMapping
        {
            public string DimensionDescription { get; set; }
            public DimensionTemplate DimensionTemplate { get; set; }
        }

        internal class AttributeMapping
        {
            public string AttributeDescription { get; set; }
            public bool ExistsForTemplate { get; set; }
            public bool ExistsForItem { get; set; }
            public RecordIdentifier DimensionTemplateID { get; set; }
            public RecordIdentifier DimensionAttributeID { get; set; }
            public int LastItemAttributeSequence { get; set; }
            public int LastTemplateAttributeSequence { get; set; }
        }

        internal class AttributeContainer
        {
            public List<AttributeMapping> Attributes { get; set; }
            public List<RetailItemDimension> RetailItemDimensions { get; set; }
        }

        public bool IsValid { get; private set; }
        public ErrorCode Error { get; private set; }

        private List<DimensionMapping> dimensions;
        private AttributeContainer attributeContainer;

        private DimensionMapper(bool valid, ErrorCode error, List<DimensionMapping> dimensions, AttributeContainer attributeContainer)
        {
            IsValid = valid;
            Error = error;
            this.dimensions = dimensions;
            this.attributeContainer = attributeContainer;
        }

        public static DimensionMapper PrepareAttributes(string importAttributes, RecordIdentifier headerItemID, char separator)
        {
            if(string.IsNullOrEmpty(importAttributes))
            {
                return new DimensionMapper(false, ErrorCode.AttributesCannotBeEmpty, null, null);
            }

            List<RetailItemDimension> itemDimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, headerItemID);

            List<AttributeMapping> attributeMappings = new List<AttributeMapping>();
            string[] splitAttributes = importAttributes.Split(separator);

            //Parse attributes
            for (int i = 0; i < splitAttributes.Length; i++)
            {
                string newAttribute = splitAttributes[i].Trim();

                AttributeMapping am = new AttributeMapping
                {
                    AttributeDescription = newAttribute,
                };

                attributeMappings.Add(am);
            }

            //Validate dimension and attributes count
            if(itemDimensions.Count != attributeMappings.Count)
            {
                return new DimensionMapper(false, ErrorCode.AttributeCountMismatch, null, null);
            }

            //Validate existing combinations
            Dictionary<RecordIdentifier, List<DimensionAttribute>> existingVariantItems = Providers.DimensionAttributeData.GetRetailItemDimensionAttributeRelations(PluginEntry.DataModel, headerItemID);
            List<string> mappingsChecks = attributeMappings.Select(x => x.AttributeDescription.ToUpperInvariant()).ToList();

            bool variantCombinationExists = existingVariantItems.Any(x => x.Value.Select(y => y.Text.ToUpperInvariant()).All(mappingsChecks.Contains) && x.Value.Count == mappingsChecks.Count);

            if (variantCombinationExists)
            {
                return new DimensionMapper(false, ErrorCode.AttributeCombinationAlreadyExists, null, null);
            }

            //Check for existing attributes
            for (int i = 0; i < attributeMappings.Count; i++)
            {
                RetailItemDimension itemDimension = itemDimensions[i];
                DimensionTemplate dimensionTemplate = Providers.DimensionTemplateData.GetByName(PluginEntry.DataModel, itemDimension.Text);

                if (dimensionTemplate == null)
                {
                    dimensionTemplate = new DimensionTemplate(); 
                    dimensionTemplate.ID = Providers.DimensionTemplateData.SaveAndReturnTemplateID(PluginEntry.DataModel, itemDimension);
                    dimensionTemplate.Text = itemDimension.Text;
                }

                attributeMappings[i].DimensionTemplateID = dimensionTemplate.ID;

                List<DimensionAttribute> existingItemAttributes = Providers.DimensionAttributeData.GetListForRetailItemDimension(PluginEntry.DataModel, itemDimension.ID);
                List<DimensionAttribute> existingTemplateAttributes = Providers.DimensionAttributeData.GetListForDimensionTemplate(PluginEntry.DataModel, dimensionTemplate.ID);

                DimensionAttribute existingItemAttribute = existingItemAttributes.Find(x => x.Text.ToUpperInvariant() == attributeMappings[i].AttributeDescription.ToUpperInvariant());
                DimensionAttribute existingTemplateAttribute = existingTemplateAttributes.Find(x => x.Text.ToUpperInvariant() == attributeMappings[i].AttributeDescription.ToUpperInvariant());

                if(existingItemAttribute != null)
                {
                    attributeMappings[i].ExistsForItem = true;
                    attributeMappings[i].DimensionAttributeID = existingItemAttribute.ID;
                    attributeMappings[i].AttributeDescription = existingItemAttribute.Text;
                }

                if (existingTemplateAttribute != null)
                {
                    attributeMappings[i].ExistsForTemplate = true;
                    
                    if(!attributeMappings[i].ExistsForItem)
                    {
                        attributeMappings[i].AttributeDescription = existingTemplateAttribute.Text;
                    }
                }

                if(existingItemAttributes.Any())
                {
                    attributeMappings[i].LastItemAttributeSequence = existingItemAttributes.Last().Sequence;
                }

                if (existingTemplateAttributes.Any())
                {
                    attributeMappings[i].LastTemplateAttributeSequence = existingTemplateAttributes.Last().Sequence;
                }
            }

            return new DimensionMapper(true, ErrorCode.None, null, new AttributeContainer { Attributes = attributeMappings, RetailItemDimensions = itemDimensions });
        }

        public static DimensionMapper PrepareDimensions(string importDimensions, RecordIdentifier itemMasterID, char separator)
        {
            List<DimensionMapping> dimensions = new List<DimensionMapping>();

            if (string.IsNullOrEmpty(importDimensions))
            {
                return new DimensionMapper(true, ErrorCode.None, null, null);
            }

            string[] splitDimensions = importDimensions.Split(separator);

            //Parse dimensions and check for existing templates
            for(int i = 0; i < splitDimensions.Length; i++)
            {
                string newDimension = splitDimensions[i].Trim();

                DimensionMapping dm = new DimensionMapping
                {
                    DimensionDescription = newDimension,
                    DimensionTemplate = Providers.DimensionTemplateData.GetByName(PluginEntry.DataModel, newDimension)
                };

                if(dm.DimensionTemplate != null)
                {
                    //Update casing to the one already saved
                    dm.DimensionDescription = dm.DimensionTemplate.Text;
                }

                dimensions.Add(dm);
            }

            if(!RecordIdentifier.IsEmptyOrNull(itemMasterID))
            {
                List<RetailItemDimension> retailItemDimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, itemMasterID);

                if(retailItemDimensions.Count > 0)
                {
                    return new DimensionMapper(false, ErrorCode.DimensionsAlreadyDefined, null, null);
                }
            }

            return new DimensionMapper(true, ErrorCode.None, dimensions, null);
        }

        public void Save(RecordIdentifier itemMasterID)
        {
            if(IsValid)
            {
                if (dimensions != null)
                {
                    int sequence = 0;
                    foreach (DimensionMapping dimensionMapping in dimensions)
                    {
                        if (dimensionMapping.DimensionTemplate == null)
                        {
                            Providers.DimensionTemplateData.Save(PluginEntry.DataModel, new DimensionTemplate { Text = dimensionMapping.DimensionDescription });
                        }

                        Providers.RetailItemDimensionData.Save(PluginEntry.DataModel, new RetailItemDimension { Text = dimensionMapping.DimensionDescription, RetailItemMasterID = itemMasterID, Sequence = sequence++ });
                    }
                }
                else if (attributeContainer != null)
                {
                    for (int i = 0; i < attributeContainer.Attributes.Count; i++)
                    {
                        AttributeMapping am = attributeContainer.Attributes[i];

                        if(!am.ExistsForTemplate)
                        {
                            Providers.DimensionAttributeData.Save(PluginEntry.DataModel, new DimensionAttribute
                            {
                                DimensionID = am.DimensionTemplateID,
                                Sequence = am.LastTemplateAttributeSequence + 1,
                                Code = am.AttributeDescription,
                                Text = am.AttributeDescription
                            });
                        }

                        if(!am.ExistsForItem)
                        {
                            DimensionAttribute newAttribute = new DimensionAttribute
                            {
                                Code = am.AttributeDescription,
                                Text = am.AttributeDescription,
                                Sequence = am.LastItemAttributeSequence + 1,
                                DimensionID = attributeContainer.RetailItemDimensions[i].ID
                            };

                            Providers.DimensionAttributeData.Save(PluginEntry.DataModel, newAttribute);

                            am.DimensionAttributeID = newAttribute.ID;
                        }

                        Providers.RetailItemData.AddDimensionAttribute(PluginEntry.DataModel, itemMasterID, am.DimensionAttributeID);
                    }
                }
            }
        }

        public string GetVariantNameFromAttributes()
        {
            if(attributeContainer != null && attributeContainer.Attributes != null)
            {
                return string.Join(" ", attributeContainer.Attributes.Select(x => x.AttributeDescription));
            }

            return "";
        }
    }
}
