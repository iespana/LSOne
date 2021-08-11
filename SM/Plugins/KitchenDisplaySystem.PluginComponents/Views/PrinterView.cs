using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class PrinterView : ViewBase
    {
        private RecordIdentifier kitchenPrinterId;
        private KitchenDisplayPrinter kitchenPrinter;


        public PrinterView(RecordIdentifier kitchenPrinterId)
            : this()
        {
            this.kitchenPrinterId = kitchenPrinterId;
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenPrinters);
        }

        private PrinterView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
            
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public string Description
        {
            get
            {
                return Properties.Resources.Printer + ": " + kitchenPrinter.ID;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return kitchenPrinter.PrinterName;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Printer + ": " + kitchenPrinter.ID;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return kitchenPrinterId;
            }
        }       

        protected override void LoadData(bool isRevert)
        {
            kitchenPrinter = Providers.KitchenDisplayPrinterData.Get(PluginEntry.DataModel, kitchenPrinterId);
            
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(
                    Properties.Resources.General,
                    ViewPages.KitchenPrinterGeneralPage.CreateInstance));

                tabSheetTabs.Broadcast(this, kitchenPrinterId, kitchenPrinter);
            }

            tabSheetTabs.SetData(isRevert,  kitchenPrinterId, kitchenPrinter);
            HeaderText = Description;
        }

        protected override bool DataIsModified()
        {

            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();

            Providers.KitchenDisplayPrinterData.Save(PluginEntry.DataModel, kitchenPrinter);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenPrinter", kitchenPrinterId, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperationsHelper.DeleteKitchenPrinter(kitchenPrinterId);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenPrinter":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == kitchenPrinterId)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

            }
        }

        
    }
}
