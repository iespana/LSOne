using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class EFTMappingView : ViewBase
    {
        private RecordIdentifier eftMappingID;
        private EFTMapping eftMapping;
        private Dictionary<string, RecordIdentifier> paymentMapping;
        private Dictionary<string, RecordIdentifier> cardTypeMapping;

        public EFTMappingView(RecordIdentifier eftMappingID) 
            : this()
        {
            this.eftMappingID = eftMappingID;
        }

        public EFTMappingView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                         ViewAttributes.Revert |
                         ViewAttributes.Save |
                         ViewAttributes.Delete |
                         ViewAttributes.Close |
                         ViewAttributes.Audit |
                         ViewAttributes.Help;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit);

            paymentMapping = new Dictionary<string, RecordIdentifier>();
            cmbTenderType.Items.Clear();
            var paymentMethods = Providers.PaymentMethodData.GetList(PluginEntry.DataModel);
            foreach (var paymentMethod in paymentMethods)
            {
                cmbTenderType.Items.Add(paymentMethod.Text);
                paymentMapping[paymentMethod.Text] = paymentMethod.ID;
            }

            cardTypeMapping = new Dictionary<string, RecordIdentifier>();
            cmbCardType.Items.Clear();
            var cardInfoList = Providers.CardInfoData.GetAll(PluginEntry.DataModel);
            foreach (var cardInfo in cardInfoList)
            {
                cmbCardType.Items.Add(cardInfo.CardName);
                cardTypeMapping[cardInfo.CardName] = cardInfo.ID;
            }

            if (cmbTenderType.Items.Count > 0)
                cmbTenderType.SelectedIndex = 0;
            if (cmbCardType.Items.Count > 0)
                cmbCardType.SelectedIndex = 0;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("EFTMapping", eftMappingID, Properties.Resources.EFTMapping, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.EFTMapping;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return eftMappingID;
	        }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.EFTMapping + ": " + (string)tbSchemeName.Text;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                eftMapping = Providers.EFTMappingData.Get(PluginEntry.DataModel, eftMappingID);

                if (eftMapping == null)
                    eftMapping = new EFTMapping();
                /*
                AddParentViewDescriptor(new ParentViewDescriptor(
                        RecordIdentifier.Empty,
                        Properties.Resources.PaymentTypes,
                        Properties.Resources.PaymentMethodImage,
                        new ShowParentViewHandler(PluginOperations.ShowPaymentMethodsSheet)));
                */
                // Allow other plugins to extend this tab panel
            }

            tbID.Text = (string)eftMapping.ID;
            tbSchemeName.Text = eftMapping.Text;
            if (cmbTenderType.FindString(eftMapping.TenderTypeName) >= 0)
                cmbTenderType.SelectedItem = eftMapping.TenderTypeName;
            if (cmbCardType.FindString(eftMapping.CardTypeName) >= 0)
                cmbCardType.SelectedItem = eftMapping.CardTypeName;
            chkEnabled.Checked = eftMapping.Enabled;
            lblCreatedValue.Text = eftMapping.Created == DateTime.MinValue ? "" :
                eftMapping.Created.ToString(System.Threading.Thread.CurrentThread.CurrentCulture);
            lblCreated.Visible = lblCreatedValue.Text.Length > 0;

            ntbLookupOrder.Value = eftMapping.LookupOrder;
  
            HeaderText = Description;
        }

        protected override bool DataIsModified()
        {
            if (tbSchemeName.Text != eftMapping.Text) return true;
            if (chkEnabled.Checked != eftMapping.Enabled) return true;
            if (cmbTenderType.SelectedItem.ToString() != eftMapping.TenderTypeName) return true;
            if (cardTypeMapping[cmbCardType.SelectedItem.ToString()] != eftMapping.CardTypeID) return true;
            if (ntbLookupOrder.Value != eftMapping.LookupOrder) return true;
            return false;
        }

        protected override bool SaveData()
        {
            eftMapping.SchemeName = tbSchemeName.Text;
            eftMapping.Enabled = chkEnabled.Checked;
            eftMapping.TenderTypeID = paymentMapping[cmbTenderType.SelectedItem.ToString()];
            eftMapping.CardTypeID = cardTypeMapping[cmbCardType.SelectedItem.ToString()];
            eftMapping.LookupOrder = Convert.ToInt16(ntbLookupOrder.Value);

            Providers.EFTMappingData.Save(PluginEntry.DataModel, eftMapping);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "EFTMapping", eftMapping.ID, eftMapping);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteEFTMapping(eftMappingID);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "EFTMapping":
                    if ((changeHint & DataEntityChangeType.Delete) == DataEntityChangeType.Delete && changeIdentifier == eftMappingID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }
        }
    }
}
