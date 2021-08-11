using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerContacts.Dialogs
{
    public partial class ContactDialog : DialogBase
    {
        private IConnectionManager connection;
        private IApplicationCallbacks callbacks;
        private Contact contact;

        private RecordIdentifier ownerID;
        private ContactRelationTypeEnum ownerType;
        private bool canSave;

        public ContactDialog(IConnectionManager connection, IApplicationCallbacks callbacks, RecordIdentifier contactID,bool canSave)
            : this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
            this.canSave = canSave;

            if (!canSave)
            {
                btnOK.Visible = false;
            }

            fullNameField1.PopulateNamePrefixes(connection.Cache.GetNamePrefixes());

            contact = Providers.ContactData.Get(connection, contactID);

            if (contact.ContactType == TypeOfContactEnum.Person)
            {
                optCompanyAndPerson.Checked = true;
            }
            else
            {
                optCompany.Checked = true;
            }

            addressField.DataModel = connection;

            fullNameField1.NameRecord = contact.Name;
            
            addressField.SetData(connection, contact.Address);

            tbPhone.Text = contact.Phone;
            tbOtherPhone.Text = contact.Phone2;
            tbFax.Text = contact.Fax;
            tbEmail.Text = contact.Email;
            tbCompanyName.Text = contact.CompanyName;
        }

        protected ContactDialog(IConnectionManager connection, IApplicationCallbacks callbacks)
            : this()
        {
            Address address = new Address();

            this.connection = connection;
            this.callbacks = callbacks;
            this.canSave = true;

            addressField.DataModel = connection;
            fullNameField1.PopulateNamePrefixes(connection.Cache.GetNamePrefixes());
            addressField.SetData(connection, address);
        }

        public static RecordIdentifier NewContact(IConnectionManager connection, IApplicationCallbacks callbacks, RecordIdentifier ownerID, ContactRelationTypeEnum ownerType, IWin32Window owner)
        {
            ContactDialog dlg = new ContactDialog(connection, callbacks);
            dlg.ownerID = ownerID;
            dlg.ownerType = ownerType;
            

            if (dlg.ShowDialog(owner) == DialogResult.OK)
            {
                return dlg.Contact.ID;
            }
            else
            {
                return RecordIdentifier.Empty;
            }
        }

        protected ContactDialog()
            : base()
        {
            contact = null;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

           
        }

        public Contact Contact
        {
            get
            {
                return contact;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
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
            Contact ct = new Contact();

            if (contact == null)
            {
                ct = new Contact();
                ct.ID = DataProviderFactory.Instance.GenerateNumber<IContactData, Contact>(connection);
                ct.OwnerType = ownerType;
                ct.OwnerID = ownerID;
            }
            else
            {
                ct = contact;
            }

            contactType = optCompanyAndPerson.Checked ? TypeOfContactEnum.Person : TypeOfContactEnum.Company;

            ct.ContactType = contactType;
            
            ct.CompanyName = tbCompanyName.Text;
            fullNameField1.GetNameIntoRecord(ct.Name);
            addressField.GetAddressIntoField(ct.Address);
            ct.Phone = tbPhone.Text;
            ct.Phone2 = tbOtherPhone.Text;
            ct.Fax = tbFax.Text;
            ct.Email = tbEmail.Text;

            contact = ct;

            Providers.ContactData.Save(connection, contact);

            DialogResult = DialogResult.OK;

            Close();
        }
        
        private void CheckEnabled(object sender, EventArgs e)
        {
            TypeOfContactEnum contactType;

            bool enabled = (optCompanyAndPerson.Checked && fullNameField1.FirstName != "" && fullNameField1.LastName != "") ||
                    (optCompany.Checked && tbCompanyName.Text != "");

            if (contact != null)
            {
                contactType = optCompanyAndPerson.Checked ? TypeOfContactEnum.Person : TypeOfContactEnum.Company;

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
