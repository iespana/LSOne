using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class POSUserCardDialog : DialogBase
    {
        RecordIdentifier posUserCardID;
        MsrCardLink posUserCardLink;
        bool editingExisting;
        bool suspendEvents = false;

        public POSUserCardDialog()
            : base()
        {
            posUserCardID = RecordIdentifier.Empty;

            InitializeComponent();

            editingExisting = false;

            cmbPOSUsers.SelectedData = new DataEntity("", "");
        }

        public POSUserCardDialog(RecordIdentifier posUserCardID)
            : base()
        {
            this.posUserCardID = posUserCardID;

            InitializeComponent();

            editingExisting = true;
            suspendEvents = true;

            posUserCardLink = Providers.MsrCardLinkData.Get(PluginEntry.DataModel, posUserCardID);

            tbCardNumber.Text = (string)posUserCardLink.ID;
            cmbPOSUsers.SelectedData = Providers.POSUserData.Get(PluginEntry.DataModel, posUserCardLink.LinkID, UsageIntentEnum.Normal) ?? new DataEntity("", "");

            tbCardNumber.Enabled = false;
            suspendEvents = false;

            this.Text = Properties.Resources.POSUserCard;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PosUserCardID
        {
            get { return posUserCardID; }
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {
            DataEntity item = ((DataEntity)e.Data);
            e.TextToDisplay = (item.ID == "" || item.ID == RecordIdentifier.Empty ? "" : item.ID.ToString() + " - ") + item.Text;
        }

        private bool IsModified()
        {
            if ((string)posUserCardLink.ID != tbCardNumber.Text) return true;
            if ((string)posUserCardLink.LinkID != (string)cmbPOSUsers.SelectedData.ID.SecondaryID) return true;

            return false;
        }


        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
            {
                return;
            }

            errorProvider1.Clear();

            if (editingExisting)
            {
                btnOK.Enabled = IsModified();
            }
            else
            {
                btnOK.Enabled = (tbCardNumber.Text.Length > 0 && cmbPOSUsers.SelectedData.ID.SecondaryID != "");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (editingExisting)
            {
                posUserCardLink.LinkID = ((DataEntity)cmbPOSUsers.SelectedData).ID.SecondaryID;

                Providers.MsrCardLinkData.Save(PluginEntry.DataModel, posUserCardLink);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "POSUserCard", posUserCardLink.ID, null);

                posUserCardID = posUserCardLink.ID;
            }
            else
            {
                posUserCardLink = new MsrCardLink();

                posUserCardLink.ID = tbCardNumber.Text;
                posUserCardLink.LinkType = MsrCardLink.LinkTypeEnum.POSUser;
                posUserCardLink.LinkID = cmbPOSUsers.SelectedData.ID.SecondaryID;

                Providers.MsrCardLinkData.Save(PluginEntry.DataModel, posUserCardLink);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "POSUserCard", posUserCardLink.ID, null);

                posUserCardID = posUserCardLink.ID;
            }            

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {           
            DialogResult = DialogResult.Cancel;
            Close();
        }       

        private void cmbPOSUsers_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText, UserIdentifierEnum.StaffID);
        }
    }
}
