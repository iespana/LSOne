using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class HospitalityTypeView : ViewBase
    {
        private RecordIdentifier hospitalityTypeID = "";
        private HospitalityType hospitalityType;

        private TabControl.Tab generalTab;
        private TabControl.Tab tableButtonGraphicalLayoutTab;
        private TabControl.Tab listingTab;
        private TabControl.Tab splitBillTransferLinesTab;
        private TabControl.Tab tipsTab;

        public HospitalityTypeView(RecordIdentifier hospitalityTypeID)
            : this()
        {
            this.hospitalityTypeID = hospitalityTypeID;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);
        }

        public HospitalityTypeView()
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
            contexts.Add(new AuditDescriptor("HospitalityType", new RecordIdentifier(hospitalityTypeID[0], hospitalityTypeID[2]), Properties.Resources.HospitalityTypeText, true));    
        }


        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HospitalityTypeText;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return hospitalityTypeID;
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.ViewAllHospitalityTypes, null, new ContextbarClickEventHandler((ContextbarClickEventHandler)PluginOperations.ShowHospitalityTypesLisetView)), 200);
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.HospitalityTypeGeneralPage.CreateInstance);
                tableButtonGraphicalLayoutTab = new TabControl.Tab(Properties.Resources.TableButtonGraphicalLayout, ViewPages.HospitalityTypeTableButtonGraphicalLayoutPage.CreateInstance);
                listingTab = new TabControl.Tab(Properties.Resources.Listing, ViewPages.HospitalityTypeListingPage.CreateInstance);
                splitBillTransferLinesTab = new TabControl.Tab(Properties.Resources.SplitBillTransferLines, ViewPages.HospitalityTypeSplitBillTransferLinesPage.CreateInstance);
                tipsTab = new TabControl.Tab(Properties.Resources.Tips, ViewPages.HospitalityTypeTipsPage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);
                tabSheetTabs.AddTab(tableButtonGraphicalLayoutTab);
                //tabSheetTabs.AddTab(listingTab);
                tabSheetTabs.AddTab(splitBillTransferLinesTab);
                //tabSheetTabs.AddTab(tipsTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, hospitalityTypeID);
            }

            hospitalityType = Providers.HospitalityTypeData.Get(PluginEntry.DataModel, hospitalityTypeID);

            tbRestaurantID.Text = (string)hospitalityType.RestaurantID;
            tbDescription.Text = hospitalityType.Text;
            cmbSalesType.SelectedData = Providers.SalesTypeData.Get(PluginEntry.DataModel, hospitalityType.SalesType);
            cmbSalesType.Enabled = false;

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.SalesTypeText;


            tabSheetTabs.SetData(isRevert, hospitalityTypeID, hospitalityType);
        }

        public string Description
        {
            get
            {
                return Properties.Resources.HospitalityTypeText + ": " + hospitalityType.RestaurantID + "," + hospitalityType.SalesType + " - " + tbDescription.Text;
            }
        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified()) return true;

            if (cmbSalesType.SelectedData != null && cmbSalesType.SelectedData.ID != hospitalityType.SalesType) return true;
            if (tbDescription.Text != hospitalityType.Text) return true;            

            return false;
        }

        protected override bool SaveData()
        {
            hospitalityType.SalesType = cmbSalesType.SelectedData == null ? "" : cmbSalesType.SelectedData.ID;
            hospitalityType.Text = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.HospitalityTypeData.Save(PluginEntry.DataModel, hospitalityType);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "HospitalityType", new RecordIdentifier(hospitalityType.ID, new RecordIdentifier(hospitalityType.ID.SecondaryID,hospitalityType.ID.SecondaryID.SecondaryID)),null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteHospitalityType(hospitalityTypeID);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "HospitalityType":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == hospitalityTypeID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbSalesType_RequestData(object sender, EventArgs e)
        {
            cmbSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
