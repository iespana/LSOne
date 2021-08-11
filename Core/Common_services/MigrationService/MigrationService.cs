using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlDataProviders.ItemMaster;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Services.Datalayer.DataProviders.Dimensions;
using LSOne.Services.Datalayer.SqlDataProviders;
using LSOne.Services.Datalayer.SqlDataProviders.Dimensions;
using LSOne.Services.Interfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class MigrationService : IMigrationService
    {
        Dictionary<string, RetailDivision> migratedDivisions = new Dictionary<string, RetailDivision>();
        Dictionary<string, RetailDepartment> migratedDepartments = new Dictionary<string, RetailDepartment>();
        Dictionary<string, RetailGroup> migratedRetailGroups= new Dictionary<string, RetailGroup>();
        Dictionary<string, SpecialGroup> migratedSpecialGroups = new Dictionary<string, SpecialGroup>();
        Dictionary<string, DiscountOffer> migratedDiscountOffers = new Dictionary<string, DiscountOffer>();
        Dictionary<string, RetailItem> migratedRetailItems = new Dictionary<string, RetailItem>();
        Dictionary<string, Guid> migratedVariants = new Dictionary<string, Guid>();
        Dictionary<string, RecordIdentifier> migratedVariantIDs = new Dictionary<string, RecordIdentifier>();
        Dictionary<string, string> colorNames = new Dictionary<string, string>(); 
        Dictionary<string, string> sizeNames = new Dictionary<string, string>(); 
        Dictionary<string, string> styleNames = new Dictionary<string, string>();
        private IErrorLog errorLog;

        public void Init(IConnectionManager entry)
        {
            DLLEntry.DataModel = entry;

            DataProviderFactory.Instance.Register<IOldSpecialGroupData, OldSpecialGroupData, DataEntity>();
            DataProviderFactory.Instance.Register<IOldRetailDivisionData, OldRetailDivisionData, RetailDivision>();
            DataProviderFactory.Instance.Register<IOldRetailDepartmentData, OldRetailDepartmentData, RetailDepartment>();
            DataProviderFactory.Instance.Register<IOldRetailGroupData, OldRetailGroupData, RetailGroup>();
            DataProviderFactory.Instance.Register<IOldRetailItemData, OldRetailItemData, OldRetailItem>();
            DataProviderFactory.Instance.Register<IDiscountOfferDataOLD, DiscountOfferDataOLD, DiscountOffer>();
            DataProviderFactory.Instance.Register<IDiscountOfferLineDataOLD, DiscountOfferLineDataOLD, DiscountOfferLine>();
            DataProviderFactory.Instance.Register<IRetailItemData, RetailItemData, RetailItem>();
            DataProviderFactory.Instance.Register<IOldDimensionData, OldDimensionData, OldDimension>();
            DataProviderFactory.Instance.Register<IOldColorData, OldColorData, OldItemWithDescription>();
            DataProviderFactory.Instance.Register<IOldSizeData, OldSizeData, OldItemWithDescription>();
            DataProviderFactory.Instance.Register<IOldStyleData, OldStyleData, OldItemWithDescription>();
            DataProviderFactory.Instance.Register<IOLDTradeAgreementData, OLDTradeAgreementData, OLDTradeAgreementEntry > ();
            DataProviderFactory.Instance.Register<IPromotionOfferLineDataOLD, PromotionOfferLineDataOLD, PromotionOfferLine>();
            DataProviderFactory.Instance.Register<IInventoryMigrators, InventoryMigrators, InventoryTransaction>();
        }
        private void SendMessage(string msg)
        {
            SendMessage(msg, MessageCallbackHandler);
        }

        private static void SendMessage(string msg, MessageCallback callBack)
        {
            if (callBack != null)
            {
                callBack(Resources.MigrationService, msg);
            }
        }

        private void PreProcessOldDimensions(IConnectionManager entry)
        {
            var colors = DataProviderFactory.Instance.Get<IOldColorData, OldItemWithDescription>().GetColorList(entry);
            foreach (var color in colors)
            {
                colorNames[(string) color.ID] = color.Text;
            }
            var sizes = DataProviderFactory.Instance.Get<IOldSizeData, OldItemWithDescription>().GetSizeList(entry);
            foreach (var size in sizes)
            {
                sizeNames[(string) size.ID] = size.Text;
            }
            var styles = DataProviderFactory.Instance.Get<IOldStyleData, OldItemWithDescription>().GetStyleList(entry);
            foreach (var style in styles)
            {
                styleNames[(string) style.ID] = style.Text;
            }
        


        }

        public bool ExecuteMigrationTarget(IConnectionManager entry, int target)
        {
            if (target == 652)
            {

                PreProcessOldDimensions(entry);
                try
                {

                    SendMessage(Resources.StartedMigratingHiearchy);
                    var divisionProvider = DataProviderFactory.Instance.Get<IOldRetailDivisionData, RetailDivision>();
                    var divisions = divisionProvider.GetDetailedList(entry,RetailDivisionSorting.RetailDivisionName, false);
                    foreach (RetailDivision division in divisions)
                    {
                        migratedDivisions[(string) division.ID] = MigrateRetailDivision(entry, division);
                    }

                    var departmentProvider = DataProviderFactory.Instance.Get<IOldRetailDepartmentData, RetailDepartment>();
                    var departments = departmentProvider.GetDetailedList(entry, RetailDepartment.SortEnum.Description, false);
                    foreach (RetailDepartment department in departments)
                    {
                        
                        migratedDepartments[(string)department.ID] = MigrateRetailDpartment(entry,department);
                    }

                    var groupProvider = DataProviderFactory.Instance.Get<IOldRetailGroupData, RetailGroup>();
                    var groups = groupProvider.GetDetailedList(entry, RetailGroupSorting.RetailGroupName, false);
                    foreach (RetailGroup retailGroup in groups)
                    {
                        migratedRetailGroups[(string) retailGroup.ID] = MigrateRetailGroup(entry, retailGroup);
                    }
                    SendMessage(Resources.StartedMigratingItems);
                    var provider = DataProviderFactory.Instance.Get<IOldRetailItemData, OldRetailItem>();
                    var items = DataProviderFactory.Instance.Get<IOldRetailItemData, OldRetailItem>().GetAllItems(entry);
                    foreach (var retailItem in items)
                    {
                        bool headerItem = provider.ItemHasDimentionGroup(entry, retailItem);
                        RetailItem item = MigrateRetailItem(entry, retailItem,headerItem);
                        migratedRetailItems[(string) retailItem.ID] = item;
                        if (headerItem)
                        {
                            var dimensionProvider = DataProviderFactory.Instance.Get<IOldDimensionData, OldDimension>();
                            var dimensions = dimensionProvider.GetList(entry, retailItem.ID, 0, false);
                            Dictionary<string, RetailItemDimension> retailDimensions =
                                new Dictionary<string, RetailItemDimension>();

                            Dictionary<string, DimensionAttribute> migratedColors =
                                new Dictionary<string, DimensionAttribute>();
                            Dictionary<string, DimensionAttribute> migratedSizes =
                                new Dictionary<string, DimensionAttribute>();
                            Dictionary<string, DimensionAttribute> migratedStyles =
                                new Dictionary<string, DimensionAttribute>();
                            foreach (var oldDimension in dimensions)
                            {
                                int sequence = 0;
                                if (!string.IsNullOrEmpty((string)oldDimension.SizeID))
                                {
                                    RetailItemDimension retailItemDimensionSize;

                                    if (!retailDimensions.ContainsKey("SIZE"))
                                    {
                                        retailItemDimensionSize = new RetailItemDimension
                                        {
                                            Deleted = false,
                                            ID = MD5.StringToGuid((string)retailItem.ID + "SI"),
                                            RetailItemMasterID = item.MasterID,
                                            Text = retailItem.SizeGroupName,
                                            Sequence = sequence++
                                        };

                                        Providers.RetailItemDimensionData.Save(entry, retailItemDimensionSize);
                                        retailDimensions.Add("SIZE", retailItemDimensionSize);
                                    }
                                    else
                                    {
                                        retailItemDimensionSize = retailDimensions["SIZE"];
                                    }

                                    if (!migratedSizes.ContainsKey((string)oldDimension.SizeID))
                                    {
                                        string description = sizeNames.ContainsKey((string)oldDimension.SizeID)
                                           ? sizeNames[(string)oldDimension.SizeID]
                                           : (string)oldDimension.SizeID;

                                        var groupItem = DataProviderFactory.Instance.Get<IOldSizeData, OldItemWithDescription>().GetGroupLine(entry,retailItem.SizeGroupID,oldDimension.SizeID);

                                        DimensionAttribute dimensionAttribute = new DimensionAttribute
                                        {
                                            Code = (string)oldDimension.SizeID,
                                            Deleted = false,
                                            ID = MD5.StringToGuid((string)retailItem.ID + (string)oldDimension.SizeID),
                                            Sequence = groupItem.SortingIndex,
                                            Text = description,
                                            DimensionID = retailItemDimensionSize.ID
                                        };
                                        migratedSizes.Add((string)oldDimension.SizeID, dimensionAttribute);
                                        Providers.DimensionAttributeData.Save(entry, dimensionAttribute);
                                    }

                                }

                                if (!string.IsNullOrEmpty((string) oldDimension.ColorID))
                                {

                                    RetailItemDimension retailItemDimensionColor;
                                   
                                    if (!retailDimensions.ContainsKey("COLOR"))
                                    {
                                        retailItemDimensionColor = new RetailItemDimension
                                        {
                                            Deleted = false,
                                            ID = MD5.StringToGuid((string) retailItem.ID + "CO"),
                                            RetailItemMasterID = item.MasterID,
                                            Text = retailItem.ColorGroupName,
                                            Sequence = sequence++
                                        };
                                        Providers.RetailItemDimensionData.Save(entry, retailItemDimensionColor);
                                        retailDimensions.Add("COLOR", retailItemDimensionColor);
                                    }
                                    else
                                    {
                                        retailItemDimensionColor = retailDimensions["COLOR"];
                                    }


                                    if (!migratedColors.ContainsKey((string) oldDimension.ColorID))
                                    {
                                        string description = colorNames.ContainsKey((string) oldDimension.ColorID)
                                            ? colorNames[(string) oldDimension.ColorID]
                                            : (string) oldDimension.ColorID;

                                        var groupItem = DataProviderFactory.Instance.Get<IOldColorData, OldItemWithDescription>().GetGroupLine(entry, retailItem.ColorGroupID, oldDimension.ColorID);

                                        DimensionAttribute dimensionAttribute = new DimensionAttribute
                                        {
                                            Code = (string) oldDimension.ColorID,
                                            Deleted = false,
                                            ID =
                                                MD5.StringToGuid((string) retailItem.ID + (string) oldDimension.ColorID),
                                            Sequence = groupItem.SortingIndex,
                                            Text = description,
                                            DimensionID = retailItemDimensionColor.ID
                                        };
                                        migratedColors.Add((string) oldDimension.ColorID, dimensionAttribute);
                                        Providers.DimensionAttributeData.Save(entry, dimensionAttribute);
                                    }
                                }



                                if (!string.IsNullOrEmpty((string) oldDimension.StyleID))
                                {
                                    RetailItemDimension retailItemDimensionStyle;
                                    if (!retailDimensions.ContainsKey("STYLE"))
                                    {
                                        retailItemDimensionStyle = new RetailItemDimension
                                        {
                                            Deleted = false,
                                            ID = MD5.StringToGuid((string) retailItem.ID + "ST"),
                                            RetailItemMasterID = item.MasterID,
                                            Text = retailItem.StyleGroupName,
                                            Sequence = sequence++
                                        };
                                        Providers.RetailItemDimensionData.Save(entry, retailItemDimensionStyle);
                                        retailDimensions.Add("STYLE", retailItemDimensionStyle);
                                    }
                                    else
                                    {
                                        retailItemDimensionStyle = retailDimensions["STYLE"];
                                    }                                  

                                    if (!migratedStyles.ContainsKey((string) oldDimension.StyleID))
                                    {
                                        string description = styleNames.ContainsKey((string)oldDimension.StyleID)
                                           ? styleNames[(string)oldDimension.StyleID]
                                           : (string)oldDimension.StyleID;

                                        var groupItem = DataProviderFactory.Instance.Get<IOldStyleData, OldItemWithDescription>().GetGroupLine(entry, retailItem.StyleGroupID, oldDimension.StyleID);

                                        DimensionAttribute dimensionAttribute = new DimensionAttribute
                                        {
                                            Code = (string) oldDimension.StyleID,
                                            Deleted = false,
                                            ID =
                                                MD5.StringToGuid((string) retailItem.ID + (string) oldDimension.StyleID),
                                            Sequence = groupItem.SortingIndex,
                                            Text = description,
                                            DimensionID = retailItemDimensionStyle.ID
                                        };
                                        migratedStyles.Add((string) oldDimension.StyleID, dimensionAttribute);
                                        Providers.DimensionAttributeData.Save(entry, dimensionAttribute);
                                    }

                                }

                                migratedVariants[(string) oldDimension.VariantNumber] = MigrateRetailItemVariant(entry,
                                    item, oldDimension);

                                if (!string.IsNullOrEmpty((string)oldDimension.ColorID))
                                {
                                    Providers.RetailItemData.AddDimensionAttribute(entry,
                                        migratedVariants[(string) oldDimension.VariantNumber],
                                        migratedColors[(string) oldDimension.ColorID].ID);
                                }

                                if (!string.IsNullOrEmpty((string)oldDimension.SizeID))
                                {
                                    Providers.RetailItemData.AddDimensionAttribute(entry,
                                        migratedVariants[(string) oldDimension.VariantNumber],
                                        migratedSizes[(string) oldDimension.SizeID].ID);
                                }

                                if (!string.IsNullOrEmpty((string)oldDimension.StyleID))
                                {
                                    Providers.RetailItemData.AddDimensionAttribute(entry,
                                        migratedVariants[(string) oldDimension.VariantNumber],
                                        migratedStyles[(string) oldDimension.StyleID].ID);
                                }
                            }

                        }
                    }

                    var specialGroupProvider = DataProviderFactory.Instance.Get<IOldSpecialGroupData, DataEntity>();
                    var specialGroups = specialGroupProvider.GetList(entry);
                    foreach (DataEntity specialGroup in specialGroups)
                    {

                        migratedSpecialGroups[(string)specialGroup.ID] = MigrateSpecialGroup(entry, specialGroup);
                        var specialGroupItems = specialGroupProvider.ItemsInSpecialGroup(entry, specialGroup.ID,0,int.MaxValue,OldSpecialGroupItemSorting.ItemName, false);
                        foreach (DataEntity specialGroupItem in specialGroupItems)
                        {
                            Providers.SpecialGroupData.AddItemToSpecialGroup(entry, specialGroupItem.ID,specialGroup.ID);
                        }
                    }

                    SendMessage(Resources.StartedMigratingDiscounts);
                    var discountOfferProvider = DataProviderFactory.Instance.Get<IDiscountOfferDataOLD, DiscountOffer>();

                    var discountOfferLineProvider = DataProviderFactory.Instance.Get<IDiscountOfferLineDataOLD, DiscountOfferLine>();
                    var promotionLineProvider = DataProviderFactory.Instance.Get<IPromotionOfferLineDataOLD, PromotionOfferLine>();
                    var offers = discountOfferProvider.GetAllOffers(entry, DiscountOfferSorting.Description, false);
                    foreach (DiscountOffer discountOffer in offers)
                    {
                        migratedDiscountOffers[(string)discountOffer.ID] = MigrateDiscountOffer(entry, discountOffer);
                        int totalLines;
                        var offerLiness = discountOfferLineProvider.GetLines(entry,discountOffer.ID,1,false,int.MaxValue,out totalLines);
                        foreach (DiscountOfferLine discountOfferLine in offerLiness)
                        {

                            MigrateDiscountOfferLine(entry, discountOfferLine);


                        }
                        var promotionLines = promotionLineProvider.GetLines(entry, discountOffer.ID,PromotionOfferLineSorting.ItemId, false);
                        foreach (PromotionOfferLine discountOfferLine in promotionLines)
                        {

                            MigratePromotionLine(entry, discountOfferLine);


                        }

                    }
                     (DataProviderFactory.Instance.Get<IInventoryMigrators, InventoryTransaction>()).DropVariantIDColumnFromInventSum(entry);

                    
                }
                catch (Exception e)
                {
                    if (errorLog != null)
                    {
                        errorLog.LogMessage(LogMessageType.Error, e.Message);
                    }
                    return false;
                }
            }
            return true;
        }

        public virtual bool MigrationWillBeRun(string version, string maxVersion)
        {

            DatabaseVersion current = new DatabaseVersion();
            current.ParseVersion(version);
            DatabaseVersion maxUtilVersion = new DatabaseVersion();
            maxUtilVersion.ParseVersion(maxVersion);
            foreach (int migrationTarget in MigrationTargets())
            {
                if (current.DbVersion < migrationTarget && migrationTarget <= maxUtilVersion.DbVersion)
                {
                    return true;
                }
            }
            return false;
        }

        public event MessageCallback MessageCallbackHandler;

        public List<int> MigrationTargets()
        {
            List<int> reply = new List<int>() { 652 };
            return reply;
        }

        public IErrorLog ErrorLog
        {
            set { errorLog = value; }
        }
        private RetailDivision MigrateRetailDivision(IConnectionManager entry, RetailDivision division)
        {

            if (!DataProviderFactory.Instance.Get<IRetailDivisionData, RetailDivision>().Exists(entry, division.ID))
            {
                //Interfaces.Services.BackupService(entry).NewBackup();
                division.MasterID = MD5.StringToGuid((string)division.ID);

                DataProviderFactory.Instance.Get<IRetailDivisionData, RetailDivision>().Save(entry, division);
                return division;
            }
            return DataProviderFactory.Instance.Get<IRetailDivisionData, RetailDivision>()
                .Get(entry, division.ID);
        }
        private RetailDepartment MigrateRetailDpartment(IConnectionManager entry, RetailDepartment department)
        {
            if (!RecordIdentifier.IsEmptyOrNull(department.RetailDivisionID) &&
                            migratedDivisions.ContainsKey((string)department.RetailDivisionID))
            {
                department.RetailDivisionMasterID =
                    migratedDivisions[(string)department.RetailDivisionID].MasterID;
            }
            if (!DataProviderFactory.Instance.Get<IRetailDepartmentData, RetailDepartment>().Exists(entry, department.ID))
            {
                //Interfaces.Services.BackupService(entry).NewBackup();
                department.MasterID = MD5.StringToGuid((string)department.ID);

                DataProviderFactory.Instance.Get<IRetailDepartmentData, RetailDepartment>().Save(entry, department);
                return department;
            }
            return DataProviderFactory.Instance.Get<IRetailDepartmentData, RetailDepartment>()
                .Get(entry, department.ID);
        }
        private RetailGroup MigrateRetailGroup(IConnectionManager entry, RetailGroup group)
        {
            if (!RecordIdentifier.IsEmptyOrNull(group.RetailDepartmentID) &&
                             migratedDepartments.ContainsKey((string)group.RetailDepartmentID))
            {
                group.RetailDepartmentMasterID =
                    migratedDepartments[(string)group.RetailDepartmentID].MasterID;
            }


            if (!DataProviderFactory.Instance.Get<IRetailGroupData, RetailGroup>().Exists(entry, group.ID))
            {
                //Interfaces.Services.BackupService(entry).NewBackup();
                group.MasterID = MD5.StringToGuid((string)group.ID);

                DataProviderFactory.Instance.Get<IRetailGroupData, RetailGroup>().Save(entry, group);
                return group;
            }
            return DataProviderFactory.Instance.Get<IRetailGroupData, RetailGroup>()
                .Get(entry, group.ID);
        }

        private SpecialGroup MigrateSpecialGroup(IConnectionManager entry, DataEntity oldGroup)
        {
            SpecialGroup group = new SpecialGroup
            {
                ID = oldGroup.ID,
                Text = oldGroup.Text
            };

            if (!DataProviderFactory.Instance.Get<ISpecialGroupData, DataEntity>().Exists(entry, group.ID))
            {
                //Interfaces.Services.BackupService(entry).NewBackup();
                group.MasterID = MD5.StringToGuid((string)group.ID);

                DataProviderFactory.Instance.Get<ISpecialGroupData, DataEntity>().Save(entry, group);
                return group;
            }
            return DataProviderFactory.Instance.Get<ISpecialGroupData, DataEntity>().GetSpecialGroup(entry,group.ID);
        }

        private DiscountOffer MigrateDiscountOffer(IConnectionManager entry, DiscountOffer offer)
        {


            if (!DataProviderFactory.Instance.Get<IDiscountOfferData, DiscountOffer>().Exists(entry, offer.ID))
            {
                //Interfaces.Services.BackupService(entry).NewBackup();
                offer.MasterID = MD5.StringToGuid((string) offer.ID);

                DataProviderFactory.Instance.Get<IDiscountOfferData, DiscountOffer>().Save(entry, offer);
                return offer;
            }
            return DataProviderFactory.Instance.Get<IDiscountOfferData, DiscountOffer>()
                .Get(entry, offer.ID, offer.OfferType);
        }

        private DiscountOfferLine MigrateDiscountOfferLine(IConnectionManager entry, DiscountOfferLine offerLine)
        {

            if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedRetailItems.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedRetailItems[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedRetailGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedRetailGroups[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedDepartments.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedDepartments[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedSpecialGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedSpecialGroups[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Variant && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedSpecialGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedSpecialGroups[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Variant && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedSpecialGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.ItemRelation = migratedVariantIDs[(string) offerLine.ItemRelation];
                offerLine.TargetMasterID = migratedRetailItems[(string)offerLine.ItemRelation].MasterID;
                offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.All)
            {
                offerLine.TargetMasterID = Guid.Empty;
            }
            if (!RecordIdentifier.IsEmptyOrNull(offerLine.OfferID) &&
                             migratedDiscountOffers.ContainsKey((string)offerLine.OfferID))
            {
                offerLine.OfferMasterID =
                    migratedDiscountOffers[(string)offerLine.OfferID].MasterID;
            }
            DataProviderFactory.Instance.Get<IDiscountOfferLineData, DiscountOfferLine>().Save(entry, offerLine);
            return offerLine;

        }
        private DiscountOfferLine MigratePromotionLine(IConnectionManager entry, PromotionOfferLine offerLine)
        {         

            if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedRetailItems.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedRetailItems[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedRetailGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedRetailGroups[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedDepartments.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedDepartments[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedSpecialGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedSpecialGroups[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Variant && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedSpecialGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.TargetMasterID = migratedSpecialGroups[(string)offerLine.ItemRelation].MasterID;
            }
            else if (offerLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Variant && !RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation) && migratedSpecialGroups.ContainsKey((string)offerLine.ItemRelation))
            {
                offerLine.ItemRelation = migratedVariantIDs[(string)offerLine.ItemRelation];
                offerLine.TargetMasterID = migratedRetailItems[(string)offerLine.ItemRelation].MasterID;
                offerLine.Type = DiscountOfferLine.DiscountOfferTypeEnum.Item;
            }
            if (!RecordIdentifier.IsEmptyOrNull(offerLine.OfferID) &&
                             migratedDiscountOffers.ContainsKey((string)offerLine.OfferID))
            {
                offerLine.OfferMasterID =
                    migratedDiscountOffers[(string)offerLine.OfferID].MasterID;
            }
            DataProviderFactory.Instance.Get<IDiscountOfferLineData, DiscountOfferLine>().Save(entry, offerLine);
            return offerLine;

        }


        private RetailItem MigrateRetailItem(IConnectionManager entry, OldRetailItem item,bool headerItem)
        {
            //Interfaces.Services.BackupService(entry).NewBackup();
            RetailItem newItem = new RetailItem
            {
                
                MasterID = MD5.StringToGuid((string)item.ID),
                BarCodeSetupDescription = item.BarCodeSetupDescription,
                BarCodeSetupID = item.BarCodeSetupID,
                BlockedOnPOS = item.BlockedOnPOS,
                DateToActivateItem = item.DateToActivateItem,
                DateToBeBlocked = item.DateToBeBlocked,
                DefaultVendorID = item.DefaultVendorItemId,
                ExtendedDescription = item.Notes,
                GradeID = item.GradeID,
                ID = item.ID,
                InventoryUnitID = item[OldRetailItem.ModuleTypeEnum.Inventory].Unit,
                InventoryUnitName = item[OldRetailItem.ModuleTypeEnum.Inventory].UnitText,
                IsFuelItem = item.IsFuelItem,
                ItemType =  headerItem? ItemTypeEnum.MasterItem : (ItemTypeEnum)item.ItemType,
                KeyInPrice = (RetailItem.KeyInPriceEnum)item.KeyInPrice,
                KeyInQuantity = (RetailItem.KeyInQuantityEnum)item.KeyInQuantity,
                MustKeyInComment = item.MustKeyInComment,
                MustSelectUOM = item.MustSelectUOM,
                NameAlias = item.NameAlias,
                NoDiscountAllowed = item.NoDiscountAllowed,
                PrintVariantsShelfLabels = item.PrintVariantsShelfLabels,
                ProfitMargin = item.ProfitMargin,
                PurchasePrice = item[OldRetailItem.ModuleTypeEnum.Purchase].Price,
                PurchaseUnitID = item[OldRetailItem.ModuleTypeEnum.Purchase].Unit,
                PurchaseUnitName = item[OldRetailItem.ModuleTypeEnum.Purchase].UnitText,
                QuantityBecomesNegative = item.QuantityBecomesNegative,
                SalesAllowTotalDiscount = item[OldRetailItem.ModuleTypeEnum.Sales].TotalDiscount,
                SalesLineDiscount = item[OldRetailItem.ModuleTypeEnum.Sales].LineDiscount,
                SalesLineDiscountName = item[OldRetailItem.ModuleTypeEnum.Sales].LineDiscountName,
                SalesMarkup = item[OldRetailItem.ModuleTypeEnum.Sales].Markup,
                SalesMultiLineDiscount = item[OldRetailItem.ModuleTypeEnum.Sales].MultilineDiscount,
                SalesMultiLineDiscountName = item[OldRetailItem.ModuleTypeEnum.Sales].MultiLineDiscountName,
                SalesPrice = item[OldRetailItem.ModuleTypeEnum.Sales].Price,
                SalesPriceIncludingTax = item[OldRetailItem.ModuleTypeEnum.Sales].LastKnownPriceWithTax,
                SalesTaxItemGroupID = item[OldRetailItem.ModuleTypeEnum.Sales].TaxItemGroupID,
                SalesTaxItemGroupName = item[OldRetailItem.ModuleTypeEnum.Sales].TaxItemGroupName,
                SalesUnitID = item[OldRetailItem.ModuleTypeEnum.Sales].Unit,
                SalesUnitName = item[OldRetailItem.ModuleTypeEnum.Sales].UnitText,
                ScaleItem = item.ScaleItem,
                TaxItemGroupName = item.TaxGroupName,
                Text = item.Text,
                UsageIntent = item.UsageIntent,
                ValidationPeriodID = item.ValidationPeriod,
                ZeroPriceValid = item.ZeroPriceValid
            };
            if (!RecordIdentifier.IsEmptyOrNull(item.RetailDivisionID) &&
               migratedDivisions.ContainsKey((string)item.RetailDivisionID))
            {
                newItem.RetailDivisionMasterID =
                    migratedDivisions[(string)item.RetailDivisionID].MasterID;
            }
            if (!RecordIdentifier.IsEmptyOrNull(item.RetailDepartmentID) &&
                           migratedDepartments.ContainsKey((string)item.RetailDepartmentID))
            {
                newItem.RetailDepartmentMasterID =
                    migratedDepartments[(string)item.RetailDepartmentID].MasterID;
            }
            if (!RecordIdentifier.IsEmptyOrNull(item.RetailGroupID) &&
                           migratedRetailGroups.ContainsKey((string)item.RetailGroupID))
            {
                newItem.RetailGroupMasterID =
                    migratedRetailGroups[(string)item.RetailGroupID].MasterID;
            }

            if (!DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>().Exists(entry, newItem.ID))
            {
                DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>().Save(entry, newItem);
            }
            else
            {
                var existingItem = DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>()
                    .Get(entry, newItem.ID);
                return existingItem;
            }

            var provider = DataProviderFactory.Instance.Get<IOldRetailItemData, OldRetailItem>();
            var itemImages = provider.GetImages(entry, newItem.ID);
            foreach (var oldItemImage in itemImages)
            {
                ItemImage image = new ItemImage
                {
                    Image = oldItemImage.Image,
                    ImageIndex = oldItemImage.ImageIndex,
                    ItemMasterID = newItem.MasterID,
                    ID = newItem.ID

                };
                DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>().SaveImage(entry, image);
            }
            return newItem;
        }

        private Guid MigrateRetailItemVariant(IConnectionManager entry, RetailItem item, OldDimension oldDimension)
        {
            string color = colorNames.ContainsKey((string) oldDimension.ColorID)
                ? colorNames[(string) oldDimension.ColorID]
                : (string) oldDimension.ColorID;

            string size = sizeNames.ContainsKey((string) oldDimension.SizeID)
                ? sizeNames[(string) oldDimension.SizeID]
                : (string) oldDimension.SizeID;

            string style = styleNames.ContainsKey((string) oldDimension.StyleID)
                ? styleNames[(string) oldDimension.StyleID]
                : (string) oldDimension.StyleID;

            string variantName = string.Format("{0}{1}{2}{3}{4}",
                color,
                string.IsNullOrEmpty(color) ||
                (string.IsNullOrEmpty(size) &&
                 string.IsNullOrEmpty(style))
                    ? string.Empty
                    : " ",
                size,
                (string.IsNullOrEmpty(color) &&
                 string.IsNullOrEmpty(size)) ||
                string.IsNullOrEmpty(style)
                    ? string.Empty
                    : " ",
                style 


                );
            RetailItem newItem = new RetailItem
            {
                MasterID = MD5.StringToGuid((string) oldDimension.VariantNumber),
                VariantName = variantName,
                HeaderItemID = item.MasterID,
                BarCodeSetupDescription = item.BarCodeSetupDescription,
                BarCodeSetupID = item.BarCodeSetupID,
                BlockedOnPOS = item.BlockedOnPOS,
                DateToActivateItem = item.DateToActivateItem,
                DateToBeBlocked = item.DateToBeBlocked,
                DefaultVendorID = item.DefaultVendorID,
                ExtendedDescription = item.ExtendedDescription,
                GradeID = item.GradeID,
                InventoryUnitID = item.InventoryUnitID,
                InventoryUnitName = item.InventoryUnitName,
                IsFuelItem = item.IsFuelItem,
                ItemType = ItemTypeEnum.Item,
                KeyInPrice = item.KeyInPrice,
                KeyInQuantity = item.KeyInQuantity,
                MustKeyInComment = item.MustKeyInComment,
                MustSelectUOM = item.MustSelectUOM,
                NameAlias = item.NameAlias,
                NoDiscountAllowed = item.NoDiscountAllowed,
                PrintVariantsShelfLabels = item.PrintVariantsShelfLabels,
                ProfitMargin = item.ProfitMargin,
                PurchasePrice = item.PurchasePrice,
                PurchaseUnitID = item.PurchaseUnitID,
                PurchaseUnitName = item.PurchaseUnitName,
                QuantityBecomesNegative = item.QuantityBecomesNegative,
                SalesAllowTotalDiscount = item.SalesAllowTotalDiscount,
                SalesLineDiscount = item.SalesLineDiscount,
                SalesLineDiscountName = item.SalesLineDiscountName,
                SalesMarkup = item.SalesMarkup,
                SalesMultiLineDiscount = item.SalesMultiLineDiscount,
                SalesMultiLineDiscountName = item.SalesMultiLineDiscountName,
                SalesPrice = item.SalesPrice,
                SalesPriceIncludingTax = item.SalesPriceIncludingTax,
                SalesTaxItemGroupID = item.SalesTaxItemGroupID,
                SalesTaxItemGroupName = item.SalesTaxItemGroupName,
                SalesUnitID = item.SalesUnitID,
                SalesUnitName = item.SalesUnitName,
                ScaleItem = item.ScaleItem,
                //TaxGroupID = item.
                TaxItemGroupName = item.TaxItemGroupName,
                Text = item.Text,
                UsageIntent = item.UsageIntent,
                ValidationPeriodID = item.ValidationPeriodID,
                //VariantName = 
                ZeroPriceValid = item.ZeroPriceValid
            };


            newItem.RetailDivisionMasterID =
                item.RetailDivisionMasterID;
        
                newItem.RetailDepartmentMasterID = item.RetailDepartmentMasterID;
            
            
                newItem.RetailGroupMasterID =item.RetailGroupMasterID;
            


            if (!DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>().Exists(entry, newItem.ID))
            {
                DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>().Save(entry, newItem);
                migratedVariantIDs[(string) oldDimension.VariantNumber] = newItem.ID;
            }
            else
            {
                var existingItem = DataProviderFactory.Instance.Get<IRetailItemData, RetailItem>()
                    .Get(entry, newItem.ID);
                return (Guid) existingItem.MasterID;
            }

            List<OLDTradeAgreementEntry> tradeAgreementEntries =  DataProviderFactory.Instance.Get<IOLDTradeAgreementData, OLDTradeAgreementEntry>().GetAgreementsForVariant(entry,oldDimension.VariantNumber);

            foreach (OLDTradeAgreementEntry oldTradeAgreementEntry in tradeAgreementEntries)
            {
                oldTradeAgreementEntry.ItemRelation = newItem.ID;
                MigrateTradeAgreement(entry,oldTradeAgreementEntry);
            }
            List<BarCode> variantBarCodes = Providers.BarCodeData.GetListForVariant(entry, oldDimension.VariantNumber);
            foreach (var variantBarCode in variantBarCodes)
            {
                variantBarCode.ItemID = newItem.ID;
                Providers.BarCodeData.Save(entry, variantBarCode);
            }

            IInventoryMigrators inventoryMigrator = DataProviderFactory.Instance.Get<IInventoryMigrators, InventoryTransaction>();
            List<InventoryTransaction> inventoryTransactions = inventoryMigrator.GetInventoryTransactionsForVariant(entry, oldDimension.VariantNumber);
            foreach (var inventoryTransaction in inventoryTransactions)
            {
                inventoryTransaction.Initialize(newItem.ID, newItem.ItemType);
                inventoryMigrator.SaveInventoryTransaction(entry, inventoryTransaction);
            }
            var inventoryJournals = inventoryMigrator.GetJournalTransactionListForVariant(entry,oldDimension.VariantNumber);
            foreach (var inventoryJournalTransaction in inventoryJournals)
            {
                inventoryJournalTransaction.ItemId = newItem.ID;
                inventoryMigrator.SaveInventoryJournal(entry, inventoryJournalTransaction);
            }

            var inventoryTransferOrders = inventoryMigrator.GetInventoryTransferOrderForVariant(entry, oldDimension.VariantNumber);
            foreach (var inventoryTransferOrderLine in inventoryTransferOrders)
            {
                inventoryTransferOrderLine.ItemId = newItem.ID;
                inventoryMigrator.SaveInventoryTransferOrder(entry, inventoryTransferOrderLine);
            }

            var inventoryTransferRequest = inventoryMigrator.GetInventoryTransferRequestListForVariant(entry, oldDimension.VariantNumber);
            foreach (var inventoryTransferRequestLine in inventoryTransferRequest)
            {
                inventoryTransferRequestLine.ItemId = newItem.ID;
                inventoryMigrator.SaveInventoryTransferRequest(entry, inventoryTransferRequestLine);
            }

            var purchaseOrderLines = inventoryMigrator.GetPurchaseOrderLinesForVariant(entry, oldDimension.VariantNumber);
            foreach (var purchaseOrderLine in purchaseOrderLines)
            {
                purchaseOrderLine.ItemID = (string) newItem.ID;
                inventoryMigrator.SavePurchaseOrderLines(entry, purchaseOrderLine);
            }

            var vendorItems = inventoryMigrator.GetVendorItemsForVariant(entry, oldDimension.VariantNumber);
            foreach (VendorItem vendorItem in vendorItems)
           
            {
                vendorItem.RetailItemID = (string)newItem.ID;
                inventoryMigrator.SaveVendorItem(entry, vendorItem);
            }
            return (Guid) newItem.MasterID;
        }

        private void MigrateTradeAgreement(IConnectionManager entry,  OLDTradeAgreementEntry oldTradeAgreement)
        {
            TradeAgreementEntry tradeAgreement = new TradeAgreementEntry
            {
                StyleID = oldTradeAgreement.StyleID,
                SizeID = oldTradeAgreement.SizeID,
                ColorID = oldTradeAgreement.ColorID,
                ID = oldTradeAgreement.ID,
                Text = oldTradeAgreement.Text,
                ColorName = oldTradeAgreement.ColorName,
                SizeName = oldTradeAgreement.SizeName,
                StyleName = oldTradeAgreement.StyleName,
                AccountCode = oldTradeAgreement.AccountCode,
                AccountName = oldTradeAgreement.AccountName,
                AccountRelation = oldTradeAgreement.AccountRelation,
                Amount = oldTradeAgreement.Amount,
                AmountIncludingTax = oldTradeAgreement.AmountIncludingTax,
                Currency = oldTradeAgreement.Currency,
                CurrencyDescription = oldTradeAgreement.CurrencyDescription,
                FromDate = oldTradeAgreement.FromDate,
                ItemCode = oldTradeAgreement.ItemCode,
                ItemID = oldTradeAgreement.ItemID,
                ItemName = oldTradeAgreement.ItemName,
                ItemRelationName = oldTradeAgreement.ItemRelationName,
                ItemRelation = oldTradeAgreement.ItemRelation,
                Markup = oldTradeAgreement.Markup,
                Percent1 = oldTradeAgreement.Percent1,
                Percent2 = oldTradeAgreement.Percent2,
                QuantityAmount = oldTradeAgreement.QuantityAmount,
                Relation = oldTradeAgreement.Relation,
                SearchAgain = oldTradeAgreement.SearchAgain,
                ToDate = oldTradeAgreement.ToDate,
                UnitID = oldTradeAgreement.UnitID,
            };
            Providers.TradeAgreementData.Save(entry,tradeAgreement, Permission.ManageDiscounts);


        }
    }
}

