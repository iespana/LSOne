using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerContactPage : UserControl, ITabView
    {
        private LSOne.DataLayer.BusinessObjects.Customers.Customer customer;

        public CustomerContactPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerContactPage();
        }

        public bool DataIsModified()
        {
            if(tbEmail.Text != customer.Email ||
                tbPhone.Text != customer.Telephone ||
                tbMobilePhone.Text != customer.MobilePhone)
            {
                customer.Dirty = true;
                return true;
            }

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customer = (LSOne.DataLayer.BusinessObjects.Customers.Customer)internalContext;

            tbPhone.Text = customer.Telephone;
            tbMobilePhone.Text = customer.MobilePhone;
            tbEmail.Text = customer.Email;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            customer.Email = tbEmail.Text;
            customer.Telephone = tbPhone.Text;
            customer.MobilePhone = tbMobilePhone.Text;
            return true;
        }

        public void SaveUserInterface()
        {
            
        }
    }
}
