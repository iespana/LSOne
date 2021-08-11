using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class CompanyInfoGeneralPage : UserControl, ITabView
    {
        CompanyInfo companyInfo;

        public CompanyInfoGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CompanyInfoGeneralPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            companyInfo = (CompanyInfo)internalContext;

            tbPhone.Text = companyInfo.Phone;
            tbFax.Text = companyInfo.Fax;
            tbEmail.Text = companyInfo.Email;
            tbTaxNr.Text = companyInfo.TaxNumber;

            cmbLanguage.SelectedIndex = 0;

            foreach (object item in cmbLanguage.Items)
            {
                if (item.ToString() == companyInfo.LanguageCode)
                {
                    cmbLanguage.SelectedItem = item;
                    break;
                }
            }
        }

        public bool DataIsModified()
        {
            string selectedLanguage;

            selectedLanguage = (cmbLanguage.SelectedIndex <= 0) ? "" : cmbLanguage.SelectedItem.ToString();


            return tbEmail.Text != companyInfo.Email ||
                    tbFax.Text != companyInfo.Fax ||
                    tbPhone.Text != companyInfo.Phone ||
                    tbTaxNr.Text != companyInfo.TaxNumber ||
                    selectedLanguage != companyInfo.LanguageCode;

        }

        public bool SaveData()
        {
            string selectedLanguage;

            selectedLanguage = (cmbLanguage.SelectedIndex <= 0) ? "" : cmbLanguage.SelectedItem.ToString();

            companyInfo.Phone = tbPhone.Text;
            companyInfo.Fax = tbFax.Text;
            companyInfo.Email = tbEmail.Text;
            companyInfo.TaxNumber = tbTaxNr.Text;
            companyInfo.Dirty = true;
            companyInfo.LanguageCode = selectedLanguage;

            return true;
        }

        // TODO
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

        

    }
}
