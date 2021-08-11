using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.LookupValues
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;

        public static void EditRemoteHosts(object sender, EventArgs args)
        {
            Dialogs.RemoteHostsDialog dlg = new Dialogs.RemoteHostsDialog();
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void EditRemoteHosts(RecordIdentifier remoteHostId)
        {
            Dialogs.RemoteHostsDialog dlg = new Dialogs.RemoteHostsDialog(remoteHostId);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void ShowCardTypesSheet(RecordIdentifier args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CardTypesView());
        }

        public static void ShowCardTypesSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CardTypesView());
        }

        public static void ShowPaymentMethodsSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PaymentmethodsView());
        }

        public static void ShowPaymentMethodsSheet(RecordIdentifier args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PaymentmethodsView());
        }

        public static void ShowPaymentMethodSheet(RecordIdentifier paymentMethodID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PaymentMethodView(paymentMethodID));
        }

        public static void ShowCardTypeSheet(RecordIdentifier cardID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CardTypeView(cardID));
        }

        public static void ShowCurrencySheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CurrencyView());
        }

        public static void ShowCurrencySheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CurrencyView(id));
        }

        public static void ShowUnitsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UnitsView());
        }

        public static void ShowUnitsView(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UnitsView(selectedID));
        }

        public static void ShowUnitConversionsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UnitConversionView());
        }

        public static void ShowUnitConversionsView(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UnitConversionView(selectedID));
        }

        public static void ShowUnitConversionsViewFromContext(object sender, ContextBarClickEventArguments args)
        {
            ShowUnitConversionsView(args.ContextID);
        }

        public static void ShowCustomerCards(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerCardsView());
        }

        public static void ShowPOSUserCards(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.POSUserCardsView());
        }

        private static void ShowImageBankView(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ImageBankView());
        }

        public static void ShowImageBank(object sender, PluginOperationArguments args)
        {
            if (args.WantsViewReturned)
            {
                if(args.Param != null)
                {
                    args.ResultView = new Views.ImageBankView((ImageTypeEnum)args.Param);
                }
                else
                {
                    args.ResultView = new Views.ImageBankView();
                }
                
            }
        }

        /// <summary>
        /// Deletes an image from the image bank
        /// </summary>
        /// <param name="sender">The source of the request</param>
        /// <param name="args">Contains the parameters for the request. Use <see cref="PluginOperationArguments.ID"/> to pass the picture ID</param>
        public static void DeleteImage(object sender, PluginOperationArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageImageBank))
            {                
                Providers.ImageData.Delete(PluginEntry.DataModel, args.ID);
            }
        }
        

        /// <summary>
        /// Used to show the <see cref="Dialogs.ImageDialog"/> for an image in the image bank
        /// </summary>
        /// <param name="sender">The source of the request</param>
        /// <param name="args">Contains the parameters for the dialog. Use <see cref="PluginOperationArguments.ID"/> to pass the picture ID</param>
        public static void AddEditImageHandler(object sender, PluginOperationArguments args)
        {
            AddEditImage(args.ID);
        }

        public static RecordIdentifier AddEditImage(RecordIdentifier imageID)
        {
            Dialogs.ImageDialog imageDialog = new Dialogs.ImageDialog(imageID);
            imageDialog.ShowDialog(PluginEntry.Framework.MainWindow);
            return imageDialog.ImageID;
        }

        public static void EditCustomerCard(RecordIdentifier customerCardID)
        {
            Dialogs.CustomerCardDialog customerCardDialog = new Dialogs.CustomerCardDialog(customerCardID);
            customerCardDialog.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void NewCustomerCard()
        {
            Dialogs.CustomerCardDialog customerCardDialog = new Dialogs.CustomerCardDialog();
            customerCardDialog.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void EditPOSUserCard(RecordIdentifier posUserCardID)
        {
            Dialogs.POSUserCardDialog posUserCardDialog = new Dialogs.POSUserCardDialog(posUserCardID);
            posUserCardDialog.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void NewPOSUserCard()
        {
            Dialogs.POSUserCardDialog posUserCardDialog = new Dialogs.POSUserCardDialog();
            posUserCardDialog.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static bool DeleteMsrCardLink(RecordIdentifier cardID, MsrCardLink.LinkTypeEnum linkType)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageMsrCardLinks))
            {
                string question = "";
                string questionDescription = "";

                switch (linkType)
                {
                    case MsrCardLink.LinkTypeEnum.Customer:
                        question = Properties.Resources.DeleteCustomerCardQuestion;
                        questionDescription = Properties.Resources.DeleteCustomerCard;
                        break;

                    case MsrCardLink.LinkTypeEnum.POSUser:
                        question = Properties.Resources.DeletePOSUserCardQuestion;
                        questionDescription = Properties.Resources.DeletePOSUserCard;
                        break;
                }

                if (QuestionDialog.Show(question, questionDescription) == DialogResult.Yes)
                {
                    Providers.MsrCardLinkData.Delete(PluginEntry.DataModel, cardID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "MsrCardLink", cardID, linkType);
                    return true;
                }
            }
            return false;
        }

        public static void SetPaymentMethodAsLocalCurrency(RecordIdentifier paymentMethodID)
        {
            if(QuestionDialog.Show(Properties.Resources.SetLocalCurrencyQuestion, Properties.Resources.SetLocalCurrencyQuestionHeader) == DialogResult.Yes)
            {
                Providers.PaymentMethodData.SetAsLocalCurrency(PluginEntry.DataModel, paymentMethodID);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PaymentMethod", paymentMethodID, null);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "SetLocalCurrency", paymentMethodID, null);
            }
        }

        public static void NewPaymentMethod(object sender, EventArgs args)
        {
            NewPaymentMethod();
        }

        public static RecordIdentifier NewPaymentMethod()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;
            Dialogs.NewLookupPaymentMethodDialog dlg = new Dialogs.NewLookupPaymentMethodDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.PaymentMethodID;
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PaymentMethod", selectedID, null);
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PaymentTypeDashboardItemID);
                PluginOperations.ShowPaymentMethodSheet(selectedID);
            }
            return selectedID;
        }

        public static bool NewUnitConversion(DataEntity retailItem, RecordIdentifier fromUnit, RecordIdentifier toUnit)
        {
            return NewUnitConversionDetailed(retailItem, fromUnit, toUnit) != null;
        }

        public static bool NewUnitConversionSiteService(DataEntity retailItem, RecordIdentifier fromUnit, RecordIdentifier toUnit)
        {
            Dialogs.UnitConversionDialog dlg = new Dialogs.UnitConversionDialog(retailItem, fromUnit, toUnit);
            dlg.SaveToSiteService = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "UnitConversions", dlg.UnitConversionID, null);
                return true;
            }
            return false;
        }

        public static UnitConversion NewUnitConversionNoSave(DataEntity retailItemDescription, RecordIdentifier fromUnit, RecordIdentifier toUnit)
        {
            Dialogs.UnitConversionDialog dlg = new Dialogs.UnitConversionDialog(retailItemDescription, fromUnit, toUnit);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.UnitConversion;
            }
            return null;
        }

        public static Dialogs.UnitConversionDialog NewUnitConversionDetailed(DataEntity retailItem,RecordIdentifier fromUnit,RecordIdentifier toUnit)
        {
            Dialogs.UnitConversionDialog dlg = new Dialogs.UnitConversionDialog(retailItem, fromUnit, toUnit);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "UnitConversions", dlg.UnitConversionID, null);
                return dlg;
            }
            return null;
        }

        public static RecordIdentifier NewCardType()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(Permission.CardTypesEdit))
            {
                Dialogs.NewCardTypeDialog dlg = new Dialogs.NewCardTypeDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.CardTypeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "CardType", selectedID, null);
                    PluginOperations.ShowCardTypeSheet(selectedID);
                }
            }
            return selectedID;
        }

        public static bool DeletePaymentMethod(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePaymentMethodQuestion, Properties.Resources.DeletePaymentMethod) == DialogResult.Yes)
                {
                    if (Providers.PaymentMethodData.InUse(PluginEntry.DataModel, id))
                    {
                        MessageDialog.Show(Properties.Resources.CannotDeletePaymentMethod);
                        return true;
                    }                   
                    
                    Providers.PaymentMethodData.Delete(PluginEntry.DataModel, id);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PaymentMethod", id, null);
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PaymentTypeDashboardItemID);
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteCardType(RecordIdentifier cardID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.CardTypesEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteCardTypeQuestion, Properties.Resources.DeleteCardType) == DialogResult.Yes)
                {
                    if (Providers.CardInfoData.InUse(PluginEntry.DataModel, cardID))
                    {
                        MessageDialog.Show(Properties.Resources.CannotDeleteCardType);
                        return true;
                    }

                    Providers.CardTypeData.Delete(PluginEntry.DataModel, (string)cardID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "CardType", cardID, null);
                    return true;
                }
            }
            return false;
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.CardTypesView) ||
                PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsView) ||
                PluginEntry.DataModel.HasPermission(Permission.CurrencyView))
            {
                args.Add(new Category(Properties.Resources.GeneralSetup, "General setup", null), 100);
            }
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CardTypesView) || 
                    PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsView) || 
                    PluginEntry.DataModel.HasPermission(Permission.CurrencyView))
                {
                     args.Add(new Item(Properties.Resources.Payments, "Payments", null), 400);   
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageUnits))
                {
                    args.Add(new Item(Properties.Resources.Units, "Units", null), 600);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "Payments")
            {
                if(PluginEntry.DataModel.HasPermission(Permission.CardTypesView))
                {
                    args.Add(new ItemButton(Properties.Resources.CardTypes, Properties.Resources.CardTypesDescription, new EventHandler(ShowCardTypesSheet)), 300);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsView))
                {
                    args.Add(new ItemButton(Properties.Resources.PaymentTypes, Properties.Resources.PaymentTypesDescription, new EventHandler(ShowPaymentMethodsSheet)), 200);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.EFTMappingView))
                {
                    args.Add(new ItemButton(Properties.Resources.EFTMappings, Properties.Resources.EditEFTMappingsDescription, new EventHandler(ShowEFTMappingsSheet)), 350);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.CurrencyView))
                {
                    args.Add(new ItemButton(Properties.Resources.EditCurrencies, Properties.Resources.EditCurrenciesDescription, new EventHandler(ShowCurrencySheet)), 400);
                }
            }

            if (args.CategoryKey == "General setup" && args.ItemKey == "Units")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageUnits))
                {
                    args.Add(new ItemButton(Properties.Resources.Units,
                        Properties.Resources.UnitsDescription,
                        new EventHandler(ShowUnitsView)), 10);

                    args.Add(new ItemButton(Properties.Resources.UnitConversions,
                        Properties.Resources.UnitConversionDescription,
                        new EventHandler(ShowUnitConversionsView)), 20);
                }
            }

            /* -- not used at the moment
            if (args.CategoryKey == "General setup" && args.ItemKey == "System")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageRemoteHosts))
                {
                    args.Add(new ItemButton(Properties.Resources.RemoteHosts,
                             Properties.Resources.RemoteHostsDescription,
                             new EventHandler(EditRemoteHosts)), 1100);
                }
            }
            */

            if (args.CategoryKey == "Retail" && args.ItemKey == "Customers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageMsrCardLinks))
                {
                    args.Add(new ItemButton(Properties.Resources.CustomerCards, Properties.Resources.CustomerCardsDescription, new EventHandler(ShowCustomerCards)), 30);
                }
            }

            if (args.CategoryKey == "Security" && args.ItemKey == "POSUsers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageMsrCardLinks))
                {
                    args.Add(new ItemButton(Properties.Resources.POSUserCards, Properties.Resources.POSUserCardsDescription, new EventHandler(ShowPOSUserCards)), 270);
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Setup, "Setup"), 900);
            args.Add(new Page(Properties.Resources.Customers, "Customers"), 500);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Setup")
            {
                args.Add(new PageCategory(Properties.Resources.Payments, "Payments"), 100);

                args.Add(new PageCategory(Properties.Resources.Units, "Units"), 500);

            }
            if (args.PageKey == "Customers")
            {
                args.Add(new PageCategory(Properties.Resources.Groups, "CustomerGroups"), 200);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Setup" && args.CategoryKey == "Payments")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CardTypesView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.CardTypes,
                        Properties.Resources.CardTypes,
                        Properties.Resources.CardTypesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        Properties.Resources.card_types_16,
                        new EventHandler(ShowCardTypesSheet),
                        "CardTypes"),30);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.PaymentTypes,
                        Properties.Resources.PaymentTypes,
                        Properties.Resources.PaymentTypesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        Properties.Resources.payment_types_16,
                        new EventHandler(ShowPaymentMethodsSheet), 
                        "PaymentMethods"), 20);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.CurrencyView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.EditCurrencies,
                        Properties.Resources.EditCurrencies,
                        Properties.Resources.EditCurrenciesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        Properties.Resources.currency_16,
                        new EventHandler(ShowCurrencySheet),
                        "Currencies"), 10);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit)|| PluginEntry.DataModel.HasPermission(Permission.EFTMappingView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.EFTMappings,
                        Properties.Resources.EFTMappings,
                        Properties.Resources.EditEFTMappingsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                        null,
                        new EventHandler(ShowEFTMappingsSheet),
                        "EFTMAppings"), 40);
                }
            }

            if (args.PageKey == "Setup" && args.CategoryKey == "Units")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageUnits))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.Units,
                            Properties.Resources.Units,
                            Properties.Resources.UnitsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.units_16,
                            null,
                            new EventHandler(ShowUnitsView),
                            "Units"), 10);

                    args.Add(new CategoryItem(
                            Properties.Resources.Conversions,
                            Properties.Resources.Conversions,
                            Properties.Resources.UnitConversionTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.unit_conversions_16,
                            null,
                            new EventHandler(ShowUnitConversionsView),
                            "UnitConversions"), 20);
                }
            }
            if (args.PageKey == "Customers" && args.CategoryKey == "Customers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.CustomerCards,
                            Properties.Resources.CustomerCards,
                            Properties.Resources.CustomerCardsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.card_types_16,
                            null,
                            ShowCustomerCards,
                            "CustomerCards"), 30);
                }
            }

            if (args.PageKey == "Sites" && args.CategoryKey == "Look and feel")
            {
                args.Add(new CategoryItem(
                    Properties.Resources.ImageBank,
                    Properties.Resources.ImageBank,
                    Properties.Resources.ImageBankTooltipDescription,
                    CategoryItem.CategoryItemFlags.Button,
                    Properties.Resources.photo_gallery_16,
                    ShowImageBankView,
                    "ImageBank"), 40);
            }

            /* -- not used at the moment
            if (args.PageKey == "Tools" && args.CategoryKey == "Administration")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageRemoteHosts))
                {
                    args.Add(new CategoryItem(
                                Properties.Resources.RemoteHosts,
                                Properties.Resources.RemoteHosts,
                                Properties.Resources.RemoteHostsDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                null,
                                null,
                                new EventHandler(EditRemoteHosts),
                                "RemoteHosts"), 1100);
                }
            }
            */
        }

        internal static RecordIdentifier NewCurrency()
        {            
            RecordIdentifier selectedID = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit))
            {
                Dialogs.CurrencyDialog dlg = new Dialogs.CurrencyDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    //selectedID = dlg.CurrencyID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Currencies", dlg.CurrencyID, null);  //caught also in the CurrencyView.cs...

                    //PluginOperations.ShowFunctionalityProfileSheet(null, selectedID);
                    //PluginOperations.ShowCurrencySheet(this, 1);
                }
            }
            return selectedID;
        }
        /// <summary>
        /// Returns the selected site service profile for the Site Manager. If no site service profile has been selected then the function returns null
        /// </summary>
        /// <returns>The Site managers site service profile. Returns null if no site service profile has been selected</returns>
        public static SiteServiceProfile GetSiteServiceProfile()
        {
            if (siteServiceProfile == null)
            {
                Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
                if (parameters != null)
                {
                    siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, parameters.SiteServiceProfile);
                }
            }

            return siteServiceProfile;
        }
        internal static void EditCurrency(Views.CurrencyView currencyView, RecordIdentifier recordIdentifier)
        {
            Dialogs.CurrencyDialog dlg = new Dialogs.CurrencyDialog(recordIdentifier);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Currencies", dlg.CurrencyID, null);
            }
        }

        internal static bool DeleteCurrency(RecordIdentifier recId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteCurrencyQuestion, Properties.Resources.DeleteCurrency) == DialogResult.Yes)
                {
                    var currencyProvider = Providers.CurrencyData;
                    if (currencyProvider.InUse(PluginEntry.DataModel, recId))
                    {
                        MessageDialog.Show(Properties.Resources.CannotDeleteCurrency);
                        return true;
                    }
                    currencyProvider.Delete(PluginEntry.DataModel, recId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Currencies", (string)recId, null);

                    return true;
                }
            }
            return false;
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageUnits))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.UnitConversions,
                        new ContextbarClickEventHandler(PluginOperations.ShowUnitConversionsViewFromContext)), 400);
                }
            }
        }

        public static void ShowEFTMappingsSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.EFTMappingsView());
        }

        internal static void NewEFTMapping()
        {
            Dialogs.EFTMappingDialog eftMappingDialog = new Dialogs.EFTMappingDialog();
            eftMappingDialog.ShowDialog(PluginEntry.Framework.MainWindow);

            if (eftMappingDialog.DialogResult == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "EFTMapping", eftMappingDialog.EFTMappingID, eftMappingDialog.EFTMapping);
            }
        }

        internal static void EditEFTMapping(RecordIdentifier id)
        {
            Dialogs.EFTMappingDialog eftMappingDialog = new Dialogs.EFTMappingDialog(id);
            eftMappingDialog.ShowDialog(PluginEntry.Framework.MainWindow);

            if (eftMappingDialog.DialogResult == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "EFTMapping", eftMappingDialog.EFTMappingID, eftMappingDialog.EFTMapping);
            }
        }

        public static bool DeleteEFTMapping(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteEFTMappingQuestion, Properties.Resources.DeleteEFTMapping) == DialogResult.Yes)
                {
                    Providers.EFTMappingData.Delete(PluginEntry.DataModel, id);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "EFTMapping", id, null);
                    return true;
                }
            }
            return false;
        }

        public static RecordIdentifier SavePaymentMethod(string description, PaymentMethodDefaultFunctionEnum defaultFunction)
        {
            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.Text = description;
            paymentMethod.DefaultFunction = defaultFunction;

            Providers.PaymentMethodData.Save(PluginEntry.DataModel, paymentMethod);

            return paymentMethod.ID;
        }

        internal static object EnforceInventoryUnitConversion(DataEntity itemDataEntity, RecordIdentifier targetUnitId)
        {
            RecordIdentifier inventoryUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemDataEntity.ID, RetailItem.UnitTypeEnum.Inventory);
                
            while (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemDataEntity.ID, targetUnitId, inventoryUnitId))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.UnitConversionQuestion,
                    Properties.Resources.UnitConversionRuleMissing) == DialogResult.Yes)
                {
                    return NewUnitConversion(itemDataEntity, inventoryUnitId, targetUnitId);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}