using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors.Popup;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Panels
{
    public partial class CustomerContactInfoPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;
        private DateTime defaultDateTime;

        public CustomerContactInfoPanel(IConnectionManager entry)
        {
            InitializeComponent();

            this.entry = entry;
            settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;

            defaultDateTime = new DateTime(1900, 1, 1);
            dtDateOfBirth.Value = defaultDateTime;
            dtDateOfBirth.Checked = false;
            cmbGender.SelectedIndex = 0;            

            chkIsCashCustomer.Enabled = settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.YesAndSetOnPos
                                        || settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.NoAndSetOnPos;

            chkIsCashCustomer.Checked = settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.YesAndSetOnPos
                                        || settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.AlwaysYes;
        }                         

        public DateTime DateOfBirth => dtDateOfBirth.Checked ? dtDateOfBirth.Value.Date : new DateTime(1900, 1, 1);

        public GenderEnum Gender => (GenderEnum)cmbGender.SelectedIndex;

        public bool IsCashCustomer => chkIsCashCustomer.Checked;

        public RecordIdentifier SalesTaxGroupID => cmbSalesTaxGroup.SelectedDataID ?? RecordIdentifier.Empty;

        private void cmbSalesTaxGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier selectedID = cmbSalesTaxGroup.SelectedDataID ?? RecordIdentifier.Empty;
            DualDataPanel panelToEmbed = new DualDataPanel(
                selectedID, 
                Providers.SalesTaxGroupData.GetList(entry), 
                null, 
                true, 
                cmbSalesTaxGroup.SkipIDColumn, 
                false, 
                50,
                false);

            panelToEmbed.SelectNoneAllowed = true;
            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        public bool ValidateData()
        {
            bool reply = true;

            if (settings.SiteServiceProfile.CustomerGenderIsMandatory && ((GenderEnum) cmbGender.SelectedIndex) == GenderEnum.None)
            {
                reply = false;
                errorProvider1.SetError(cmbGender, Properties.Resources.GenderMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerBirthDateIsMandatory && dtDateOfBirth.Value == defaultDateTime)
            {
                reply = false;
                errorProvider1.SetError(dtDateOfBirth, Properties.Resources.DateOfBirthMandatoryError);
            }

            return reply;
        }

        private void ClearErrorProvider(object sender, EventArgs args)
        {
            errorProvider1.Clear();
        }

        private void CustomerContactInfoPanel_Load(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SelectedData = new DataEntity(
                settings.SiteServiceProfile.NewCustomerDefaultTaxGroup,
                settings.SiteServiceProfile.NewCustomerDefaultTaxGroupName);
        }
    }
}
