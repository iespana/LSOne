using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.PaymentLimitations
{
    internal class PluginOperations
    {
        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
           
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Profiles.Views.FunctionalityProfileView")
            {
                args.Add(new TabControl.Tab(Properties.Resources.Limitations, ViewPages.FunctionalityProfileLimitationsPage.CreateInstance), 200);
            }

            if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreTenderView")
            {
                args.Add(new TabControl.Tab(Properties.Resources.PaymentLimitations, ViewPages.AllowedPaymentTypesLimitationsPage.CreateInstance), 200);
            }

            if (args.ContextName == "LSOne.ViewPlugins.LookupValues.Views.PaymentMethodView")
            {
                args.Add(new TabControl.Tab(Properties.Resources.Limitations, ViewPages.PaymentMethodLimitationsPage.CreateInstance), 200);
            }
        }

        public static PaymentMethodLimitation CreateNewPaymentMethodLimitation(RecordIdentifier ID, RecordIdentifier limitationMasterID, RecordIdentifier restrictionCode, RecordIdentifier tenderID, bool include, LimitationType limitationType, RecordIdentifier relationMasterID, bool taxExempt)
        {
            PaymentMethodLimitation limitation = new PaymentMethodLimitation();

            limitation.LimitationMasterID = limitationMasterID == RecordIdentifier.Empty ? Providers.PaymentLimitationsData.GetLimitationMasterID(PluginEntry.DataModel, restrictionCode) : limitationMasterID;
            limitation.LimitationMasterID = limitation.LimitationMasterID == RecordIdentifier.Empty ? Guid.NewGuid() : limitation.LimitationMasterID;

            limitation.ID = ID;
            limitation.RestrictionCode = restrictionCode;
            limitation.TenderID = tenderID;
            limitation.Include = include;
            limitation.Type = limitationType;
            limitation.RelationMasterID = relationMasterID;
            limitation.TaxExempt = taxExempt;

            return limitation;
        }
    }
}
