using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Hospitality.Properties;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class SalesTypeView : ViewBase
    {
        private RecordIdentifier salesTypeID = "";
        private SalesType salesType;

        public SalesTypeView(RecordIdentifier salesTypeID)
            : this()
        {
            this.salesTypeID = salesTypeID;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes);
        }

        public SalesTypeView()
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
            contexts.Add(new AuditDescriptor("SalesType", salesTypeID, Resources.SalesTypeText, true));
        }

        public string Description
        {
            get
            {
                return Resources.SalesTypeText + ": " + salesTypeID + " - " + tbDescription.Text;
            }
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
                return Resources.SalesTypeText;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return salesTypeID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            salesType = Providers.SalesTypeData.Get(PluginEntry.DataModel, salesTypeID.ToString());

            tbID.Text = (string)salesType.ID;
            tbDescription.Text = salesType.Text;

            cmbSalesTaxGroup.SelectedData = salesType.TaxGroupID == "" ? null : Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, salesType.TaxGroupID);

            if (salesType.PriceGroup == "")
            {
                cmbPriceGroup.SelectedData = null;
            }
            else
            {
                var priceGroup = Providers.PriceDiscountGroupData.Get(PluginEntry.DataModel, GetIDForPriceGroup());
                cmbPriceGroup.SelectedData = new DataEntity(priceGroup.GroupID, priceGroup.Text);
            }

            HeaderText = Description;
        }

        protected override bool DataIsModified()
        {
            if (tbID.Text != salesType.ID) return true;
            if (tbDescription.Text != salesType.Text) return true;

            if (cmbSalesTaxGroup.SelectedData != null && cmbSalesTaxGroup.SelectedData.ID != salesType.TaxGroupID) return true;
            if (cmbPriceGroup.SelectedData != null && cmbPriceGroup.SelectedData.ID != GetIDForPriceGroup().SecondaryID.SecondaryID) return true;

            return false;
        }

        protected override bool SaveData()
        {
            salesType.ID = tbID.Text;
            salesType.Text = tbDescription.Text;

            salesType.TaxGroupID = cmbSalesTaxGroup.SelectedData == null ? "" : (string)cmbSalesTaxGroup.SelectedData.ID;
            salesType.PriceGroup = cmbPriceGroup.SelectedData == null ? "" : cmbPriceGroup.SelectedData.ID.ToString();

            Providers.SalesTypeData.Save(PluginEntry.DataModel, salesType);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "SalesType", salesType.ID, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteSalesType(salesTypeID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Resources.ViewAllSalesTypes, null, new ContextbarClickEventHandler((ContextbarClickEventHandler)PluginOperations.ShowSalesTypesListView)), 200);
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "SalesType":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == salesTypeID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }
        }

        private RecordIdentifier GetIDForPriceGroup()
        {
            return new RecordIdentifier(1, new RecordIdentifier((int)PriceDiscGroupEnum.PriceGroup, salesType.PriceGroup));
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetListWithTaxCodes(PluginEntry.DataModel), null);
        }

        private void cmbPriceGroup_RequestData(object sender, EventArgs e)
        {            
            cmbPriceGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.PriceGroup), null);
        }

        protected override void OnClose()
        {
            base.OnClose();
        }
    }
}