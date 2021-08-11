using System;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.Services.Interfaces.Constants;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LookupValues.Properties;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class CustomerCardDialog : DialogBase
    {
        RecordIdentifier customerCardID;
        MsrCardLink customerCardLink;
        bool editingExisting;
        bool suspendEvents = false;

        public CustomerCardDialog()
            : base()
        {
            customerCardID = RecordIdentifier.Empty;

            InitializeComponent();

            editingExisting = false;

            cmbCustomers.SelectedData = new DataEntity("", "");
        }

        public CustomerCardDialog(RecordIdentifier customerCardID)
            : base()
        {
            this.customerCardID = customerCardID;

            InitializeComponent();

            editingExisting = true;
            suspendEvents = true;

            customerCardLink = Providers.MsrCardLinkData.Get(PluginEntry.DataModel, customerCardID);

            tbCardNumber.Text = (string)customerCardLink.ID;
            cmbCustomers.SelectedData = Providers.CustomerData.Get(PluginEntry.DataModel, customerCardLink.LinkID, UsageIntentEnum.Normal) ?? new DataEntity("", "");

            tbCardNumber.Enabled = false;
            suspendEvents = false;

            this.Text = Properties.Resources.CustomerCard;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RecordIdentifier hardwareProfileID = PluginEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.SMHardwareProfile) as RecordIdentifier;
            if (hardwareProfileID != null)
            {
                HardwareProfile profile = Providers.HardwareProfileData.Get(PluginEntry.DataModel, hardwareProfileID);
                if (profile != null)
                {
                    tbCardNumber.TrackSeperation = TrackSeperation.Before;
                    tbCardNumber.StartCharacter = profile.StartTrack1;
                    tbCardNumber.EndCharacter = profile.EndTrack1;
                    tbCardNumber.Seperator = profile.Separator1;
                }
                
            }
            if (hardwareProfileID == null || hardwareProfileID == "")
            {
                errorProvider2.SetError(tbCardNumber, Resources.HardwareProfileNotSet);
            }
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier CustomerCardID
        {
            get { return customerCardID; }
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {
            DataEntity item = ((DataEntity)e.Data);
            e.TextToDisplay = (item.ID == "" || item.ID == RecordIdentifier.Empty ? "" : item.ID.ToString() + " - ") + item.Text;
        }

        private bool IsModified()
        {
            if ((string)customerCardLink.ID != tbCardNumber.Text) return true;
            if ((string)customerCardLink.LinkID != ((DataEntity)cmbCustomers.SelectedData).ID) return true;

            return false;
        }


        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
            {
                return;
            }

            errorProvider1.Clear();
            errorCardNumberLength.Clear();

            if (editingExisting)
            {
                btnOK.Enabled = IsModified();
            }
            else
            {
                btnOK.Enabled = (tbCardNumber.Text.Length > 0 && ((DataEntity)cmbCustomers.SelectedData).ID != "");
            }
            
            if (tbCardNumber.Text.Count() > 30)
            {
                errorCardNumberLength.SetError(tbCardNumber, Resources.ErrorCardNumberLength);
                btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (editingExisting)
            {
                customerCardLink.LinkID = ((DataEntity)cmbCustomers.SelectedData).ID;
                
                Providers.MsrCardLinkData.Save(PluginEntry.DataModel, customerCardLink);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "CustomerCard", customerCardLink.ID, null);

                customerCardID = customerCardLink.ID;
            }
            
            else
            {
                customerCardLink = new MsrCardLink();                
                customerCardLink.ID = tbCardNumber.Text;
                customerCardLink.LinkType = MsrCardLink.LinkTypeEnum.Customer;
                customerCardLink.LinkID = ((DataEntity)cmbCustomers.SelectedData).ID;

                Providers.MsrCardLinkData.Save(PluginEntry.DataModel, customerCardLink);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "CustomerCard", customerCardLink.ID, null);

                customerCardID = customerCardLink.ID;
            }            

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {           
            DialogResult = DialogResult.Cancel;
            Close();
        }       

        private void cmbCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbCustomers.SelectedData).ID;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.Customers,
            textInitallyHighlighted);

        }
    }
}
