using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class EFTMappingDialog : DialogBase
    {
        private EFTMapping eftMapping;

        private RecordIdentifier eftMappingID;
        private Dictionary<string, RecordIdentifier> paymentMapping;
        private Dictionary<string, RecordIdentifier> cardTypeMapping;

        public RecordIdentifier EFTMappingID
        {
            get
            {
                return eftMappingID;
            }
        }

        public EFTMapping EFTMapping
        {
            get
            {
                return eftMapping;
            }
        }


        public EFTMappingDialog(EFTMapping eftMapping)
            : this()
        {
            this.eftMapping = eftMapping;
            this.eftMappingID = eftMapping.ID;

            //tbID.Text = (string)eftMapping.TenderTypeID;
            //tbSchemeName.Text = eftMapping.SchemeName;

            LoadData();
        }

        public EFTMappingDialog(RecordIdentifier eftMappingID) : this()
        {
            tbID.Text = (string)eftMappingID;
            this.eftMappingID = eftMappingID;

            LoadData();
        }

        public EFTMappingDialog()
            : base()
        {
            InitializeComponent();

            //eftMapping = Providers.EFTMappingData.Get(PluginEntry.DataModel, eftMappingID);

            //if (eftMapping == null)
            //    eftMapping = new EFTMapping();

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

            //HeaderText = Description;
        }


        protected void LoadData()
        {
           eftMapping = Providers.EFTMappingData.Get(PluginEntry.DataModel, eftMappingID);

            if (eftMapping == null)
            {
                return;
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

            //HeaderText = Description;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public string Description
        {
            get
            {
                return Properties.Resources.EFTMapping + ": " + (string)tbSchemeName.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            if (eftMapping == null && Providers.EFTMappingData.Exists(PluginEntry.DataModel, new RecordIdentifier((string)tbID.Text)))
            {
                errorProvider1.SetError(tbID, Properties.Resources.EFTMappingExists);
                return;
            }

            if (eftMapping == null)
            {
                eftMapping = new EFTMapping();
            }

            eftMapping.SchemeName = tbSchemeName.Text;
            eftMapping.Enabled = chkEnabled.Checked;
            eftMapping.TenderTypeID = paymentMapping[cmbTenderType.SelectedItem.ToString()];
            eftMapping.CardTypeID = cardTypeMapping[cmbCardType.SelectedItem.ToString()];
            eftMapping.LookupOrder = Convert.ToInt16(ntbLookupOrder.Value);

            Providers.EFTMappingData.Save(PluginEntry.DataModel, eftMapping);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (eftMapping == null)
            {
                btnOK.Enabled = cmbTenderType.SelectedIndex != -1 && cmbCardType.SelectedIndex != -1;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }

    }
}
