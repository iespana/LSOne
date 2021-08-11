using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class ContactDialog : DialogBase
    {
        private Contact contact;

        private RecordIdentifier ownerID;
        private ContactRelationTypeEnum ownerType;
        private bool canSave;
        private SiteServiceProfile siteServiceProfile;

        public Contact Contact => contact;

        protected ContactDialog()
        {
            contact = null;

            InitializeComponent();

            siteServiceProfile = PluginOperations.GetSiteServiceProfile();

            Address address = new Address();
            canSave = true;

            addressField.DataModel = PluginEntry.DataModel;
            fullNameField1.PopulateNamePrefixes(PluginEntry.DataModel.Cache.GetNamePrefixes());
            addressField.SetData(PluginEntry.DataModel, address);
        }

        public ContactDialog(RecordIdentifier contactID)
            : this()
        {
            this.canSave = PluginEntry.DataModel.HasPermission(Permission.VendorEdit);
            btnOK.Visible = canSave;

            fullNameField1.PopulateNamePrefixes(PluginEntry.DataModel.Cache.GetNamePrefixes());

            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {
                contact = service.GetVendorContact(PluginEntry.DataModel, siteServiceProfile, contactID, true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }


            if (contact.ContactType == TypeOfContactEnum.Person)
            {
                optCompanyAndPerson.Checked = true;
            }
            else
            {
                optCompany.Checked = true;
            }

            addressField.DataModel = PluginEntry.DataModel;

            fullNameField1.NameRecord = contact.Name;

            addressField.SetData(PluginEntry.DataModel, contact.Address);

            tbPhone.Text = contact.Phone;
            tbOtherPhone.Text = contact.Phone2;
            tbFax.Text = contact.Fax;
            tbEmail.Text = contact.Email;
            tbCompanyName.Text = contact.CompanyName;
        }

        public ContactDialog(RecordIdentifier ownerID, ContactRelationTypeEnum ownerType) : this()
        {
            this.ownerID = ownerID;
            this.ownerType = ownerType;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(222, 222, 222));

            e.Graphics.DrawLine(pen, 1, 1, panel2.Width, 1);

            pen.Dispose();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TypeOfContactEnum contactType;
            Contact newContact = new Contact();

            if (contact == null)
            {
                newContact = new Contact();
                newContact.OwnerType = ownerType;
                newContact.OwnerID = ownerID;
            }
            else
            {
                newContact = contact;
            }

            contactType = optCompanyAndPerson.Checked ? TypeOfContactEnum.Person : TypeOfContactEnum.Company;

            newContact.ContactType = contactType;

            newContact.CompanyName = tbCompanyName.Text;
            fullNameField1.GetNameIntoRecord(newContact.Name);
            addressField.GetAddressIntoField(newContact.Address);
            newContact.Phone = tbPhone.Text;
            newContact.Phone2 = tbOtherPhone.Text;
            newContact.Fax = tbFax.Text;
            newContact.Email = tbEmail.Text;

            contact = newContact;

            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {
                service.SaveVendorContact(PluginEntry.DataModel, siteServiceProfile, contact, true);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "VendorContact", null, null);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            bool enabled = (optCompanyAndPerson.Checked && fullNameField1.FirstName != "" && fullNameField1.LastName != "") ||
                    (optCompany.Checked && tbCompanyName.Text != "");

            if (contact != null)
            {
                TypeOfContactEnum contactType = optCompanyAndPerson.Checked ? TypeOfContactEnum.Person : TypeOfContactEnum.Company;

                enabled = enabled &
                    (contact.ContactType != contactType ||
                     contact.Name != fullNameField1.NameRecord ||
                     contact.Address != addressField.AddressRecord ||
                     contact.Phone != tbPhone.Text ||
                     contact.Phone2 != tbOtherPhone.Text ||
                     contact.Fax != tbFax.Text ||
                     contact.Email != tbEmail.Text ||
                     contact.CompanyName != tbCompanyName.Text) & canSave;
            }


            btnOK.Enabled = enabled;

            errorProvider1.Clear();
        }

        private void optCompany_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void optCompanyAndPerson_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}
