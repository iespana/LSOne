using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using System.Reflection;
using LSOne.DataLayer.BusinessObjects.Attributes;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Tax;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class CustomerPage : UserControl, ITabView
    {
        private Dictionary<string, bool> mandatoryCustomerFields;
        private SiteServiceProfile profile;
        private WeakReference salesTaxGroupEditor;

        public CustomerPage()
        {
            InitializeComponent();

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "ViewSalesTaxGroups", null);
            salesTaxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditSalesTaxGroup.Visible = (salesTaxGroupEditor != null);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;

            chkCentralizedCustomers.Checked = profile.CheckCustomer;
            cmbSalesTaxGroup.SelectedData = new DataEntity(profile.NewCustomerDefaultTaxGroup, profile.NewCustomerDefaultTaxGroupName);
            cmbCashCustomer.SelectedIndex = (int)profile.CashCustomerSetting;
            chkManualID.Checked = profile.AllowCustomerManualID;
            ntbDefaultCreditLimit.Value = (double)profile.DefaultCreditLimit;
            LoadCustomerMandatoryFields();
            chkCentralizedCustomers_CheckedChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (chkCentralizedCustomers.Checked != profile.CheckCustomer) return true;
            if (cmbSalesTaxGroup.SelectedData.ID != profile.NewCustomerDefaultTaxGroup) return true;
            if (cmbCashCustomer.SelectedIndex != (int)profile.CashCustomerSetting) return true;
            if (chkManualID.Checked != profile.AllowCustomerManualID) return true;
            if (ntbDefaultCreditLimit.Value != (double)profile.DefaultCreditLimit) return true;
            if (CustomerMandatoryFieldsChanged()) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.CheckCustomer = chkCentralizedCustomers.Checked;
            profile.NewCustomerDefaultTaxGroup = cmbSalesTaxGroup.SelectedData.ID;
            profile.NewCustomerDefaultTaxGroupName = cmbSalesTaxGroup.SelectedData.Text;
            profile.CashCustomerSetting = (SiteServiceProfile.CashCustomerSettingEnum)cmbCashCustomer.SelectedIndex;
            profile.AllowCustomerManualID = chkManualID.Checked;
            profile.DefaultCreditLimit = (decimal)ntbDefaultCreditLimit.Value;
            SaveCustomerMandatoryFields();
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnEditSalesTaxGroup_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupEditor.IsAlive)
            {
                ((IPlugin)salesTaxGroupEditor.Target).Message(this, "ViewSalesTaxGroups", cmbSalesTaxGroup.SelectedData.ID);
            }
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel, TaxGroupTypeFilter.Standard), null);
        }

        private void DualDataComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void chkCentralizedCustomers_CheckedChanged(object sender, EventArgs e)
        {
            lblDefaultSalesTaxGroup.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            cmbSalesTaxGroup.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            btnEditSalesTaxGroup.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            cmbCashCustomer.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
            lblCashCustomer.Enabled = chkCentralizedCustomers.CheckState == CheckState.Checked;
        }

        private void LoadCustomerMandatoryFields()
        {
            mandatoryCustomerFields = new Dictionary<string, bool>();
            chkListMandatoryFields.Items.Clear();
            mandatoryCustomerFields.Clear();

            PropertyInfo[] props = typeof(SiteServiceProfile).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    CustomerMandatoryProperty propDispAttr = attr as CustomerMandatoryProperty;
                    if (propDispAttr != null)
                    {
                        string propName = prop.Name;
                        string auth = propDispAttr.DisplayName;
                        object objVal = prop.GetValue(profile);
                        
                        if(objVal is bool)
                        {
                            mandatoryCustomerFields.Add(propName, (bool)objVal);
                            chkListMandatoryFields.Items.Add(auth, (bool)objVal);
                        } 
                    }
                }
            }
        }

        private bool CustomerMandatoryFieldsChanged()
        {
            for(int i = 0; i < chkListMandatoryFields.Items.Count; i++)
            {
                if(chkListMandatoryFields.GetItemChecked(i) != mandatoryCustomerFields.ElementAt(i).Value)
                {
                    return true;
                }
            }

            return false;
        }

        private void SaveCustomerMandatoryFields()
        {
            PropertyInfo[] props = typeof(SiteServiceProfile).GetProperties();
            int index = 0;
            foreach (PropertyInfo prop in props)
            {
                if(mandatoryCustomerFields.ContainsKey(prop.Name))
                {
                    bool setValue = chkListMandatoryFields.GetItemChecked(index);
                    mandatoryCustomerFields[prop.Name] = setValue;
                    prop.SetValue(profile, setValue);
                    index++;
                }
            }
        }
    }
}
