using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityTypeTableButtonGraphicalLayoutPage : UserControl, ITabView
    {
        private HospitalityType hospitalityType;

        public HospitalityTypeTableButtonGraphicalLayoutPage()
        {
            InitializeComponent();

            btnsEditAddDiningTableLayout.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityTypeTableButtonGraphicalLayoutPage();
        }


        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            hospitalityType = (HospitalityType)internalContext;

            RecordIdentifier diningTableLayoutID = new RecordIdentifier(hospitalityType.RestaurantID,
                                                    new RecordIdentifier(hospitalityType.Sequence,
                                                    new RecordIdentifier(hospitalityType.SalesType, hospitalityType.DiningTableLayoutID)));

            chkUpdateTableFromPOS.Checked = hospitalityType.UpdateTableFromPOS;
            chkPromptForCustomer.Checked = hospitalityType.PromptForCustomer;
            cmbCustomerNameOnTable.SelectedIndex = (int)hospitalityType.DisplayCustomerOnTable;
            cmbCustomerNameOnTable.Enabled = chkPromptForCustomer.Checked;
            chkRequestNoOfGuests.Checked = hospitalityType.RequestNoOfGuests;
            txtMaxNoOfGuests.Value = (double)hospitalityType.MaxGuestsPerTable;
            txtMaxNoOfGuests.Enabled = chkRequestNoOfGuests.Checked;
            cmbDiningTableLayoutID.SelectedData = Providers.DiningTableLayoutData.Get(PluginEntry.DataModel, diningTableLayoutID) ?? new DataEntity("", "") ;
            cmbTableButtonDescription.SelectedIndex = (int)hospitalityType.TableButtonDescription;
            cmbTableButtonStaffDescription.SelectedIndex = (int)hospitalityType.TableButtonStaffDescription;

            if (String.IsNullOrEmpty((string)hospitalityType.AccessToOtherRestaurant) && hospitalityType.SettingsFromRestaurant != hospitalityType.RestaurantID)
            {
                tbSettingsFromRestaurant.Text = "";
                cmbSettingsFromHospitalityType.SelectedData = null;
                tbSettingsFromSequence.Text = "";
            }
            else if (!String.IsNullOrEmpty((string)hospitalityType.AccessToOtherRestaurant) && (string)hospitalityType.SettingsFromRestaurant != (string)hospitalityType.AccessToOtherRestaurant)
            {
                tbSettingsFromRestaurant.Text = "";
                cmbSettingsFromHospitalityType.SelectedData = null;
                tbSettingsFromSequence.Text = "";
            }
            else
            {
                tbSettingsFromRestaurant.Text = (string)hospitalityType.SettingsFromRestaurant;                
                cmbSettingsFromHospitalityType.SelectedData = Providers.HospitalityTypeData.GetHostpitalityTypesForRestaurant(PluginEntry.DataModel, hospitalityType.SettingsFromRestaurant).FirstOrDefault(f => f.SalesType == hospitalityType.SettingsFromHospType);
                tbSettingsFromSequence.Text = Convert.ToString(hospitalityType.SettingsFromSequence);
            }

            tbSharingSalesTypeFilter.Text = hospitalityType.SharingSalesTypeFilter;  //TODO: add proper sales type filter code similar to the one in the Terminal.Hospitality tab
            // TODO: add button context menu
            chkAutomaticJoiningCheck.Checked = hospitalityType.AutomaticJoiningCheck;

            if (hospitalityType.Overview == HospitalityType.OverviewEnum.Listing)
            {
                tbSettingsFromRestaurant.Enabled = false;
                cmbSettingsFromHospitalityType.Enabled = false;
                tbSettingsFromSequence.Enabled = false;            
            }

            if (cmbDiningTableLayoutID.SelectedData != null && cmbDiningTableLayoutID.SelectedData.ID != "")
            {
                btnsEditAddDiningTableLayout.EditButtonEnabled = true;
            }

        }

        public bool DataIsModified()
        {
            if (chkUpdateTableFromPOS.Checked != hospitalityType.UpdateTableFromPOS) return true;
            if (chkPromptForCustomer.Checked != hospitalityType.PromptForCustomer) return true;
            if (cmbCustomerNameOnTable.SelectedIndex != (int)hospitalityType.DisplayCustomerOnTable) return true;
            if (chkRequestNoOfGuests.Checked != hospitalityType.RequestNoOfGuests) return true;
            if (txtMaxNoOfGuests.Value != (double)hospitalityType.MaxGuestsPerTable) return true;
            if (cmbDiningTableLayoutID.SelectedData != null && cmbDiningTableLayoutID.SelectedData.ID != hospitalityType.DiningTableLayoutID) return true;
            if (cmbTableButtonDescription.SelectedIndex != (int)hospitalityType.TableButtonDescription) return true;
            if (cmbTableButtonStaffDescription.SelectedIndex != (int)hospitalityType.TableButtonStaffDescription) return true;

            if (tbSettingsFromRestaurant.Text != (string)hospitalityType.SettingsFromRestaurant) return true;
            if (cmbSettingsFromHospitalityType.SelectedData != null && cmbSettingsFromHospitalityType.SelectedData.ID != hospitalityType.SettingsFromHospType) return true;
            if (tbSettingsFromSequence.Text != Convert.ToString(hospitalityType.SettingsFromSequence)) return true;

            //TODO: add proper sales type filter code similar to the one in the Terminal.Hospitality tab
            if (tbSharingSalesTypeFilter.Text != hospitalityType.SharingSalesTypeFilter) return true;
            // TODO: add button context menu
            if (chkAutomaticJoiningCheck.Checked != hospitalityType.AutomaticJoiningCheck) return true;

            return false;
        }

        public bool SaveData()
        {
            hospitalityType.UpdateTableFromPOS = chkUpdateTableFromPOS.Checked;
            hospitalityType.PromptForCustomer = chkPromptForCustomer.Checked;
            hospitalityType.DisplayCustomerOnTable = (HospitalityType.CustomerOnTable)cmbCustomerNameOnTable.SelectedIndex;
            hospitalityType.RequestNoOfGuests = chkRequestNoOfGuests.Checked;
            hospitalityType.MaxGuestsPerTable = (int)txtMaxNoOfGuests.Value;
            hospitalityType.DiningTableLayoutID = cmbDiningTableLayoutID.SelectedData != null && cmbDiningTableLayoutID.SelectedData.ID != "" ? ((DiningTableLayout)cmbDiningTableLayoutID.SelectedData).LayoutID : "";
            hospitalityType.TableButtonDescription = (HospitalityType.TableButtonDescriptionEnum)cmbTableButtonDescription.SelectedIndex;
            hospitalityType.TableButtonStaffDescription = (HospitalityType.TableButtonStaffDescriptionEnum)cmbTableButtonStaffDescription.SelectedIndex;
            hospitalityType.SettingsFromRestaurant = tbSettingsFromRestaurant.Text;
            hospitalityType.SettingsFromHospType = cmbSettingsFromHospitalityType.SelectedData != null ? ((HospitalityTypeListItem)cmbSettingsFromHospitalityType.SelectedData).SalesType : "";
            hospitalityType.SettingsFromSequence = tbSettingsFromSequence.Text == "" ? RecordIdentifier.Empty : Convert.ToInt32(tbSettingsFromSequence.Text);
            hospitalityType.SharingSalesTypeFilter = tbSharingSalesTypeFilter.Text; // TODO: add proper sales type filter code
            // TODO: add button context menu
            hospitalityType.AutomaticJoiningCheck = chkAutomaticJoiningCheck.Checked;
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "HospitalityType.AccessToOtherRestaurant":
                    if (changeHint == DataEntityChangeType.VariableChanged)
                    {
                        // Check if the restaurant ID has changed
                        if ((string)changeIdentifier != tbSettingsFromRestaurant.Text)
                        {
                            tbSettingsFromRestaurant.Text = "";
                            cmbSettingsFromHospitalityType.SelectedData = null;
                            tbSettingsFromSequence.Text = "";
                        }
                    }
                    break;

                case "HospitalityType.Overview":
                    if (changeHint == DataEntityChangeType.VariableChanged)
                    {
                        if ((HospitalityType.OverviewEnum)((int)changeIdentifier) == HospitalityType.OverviewEnum.Listing)
                        {
                            tbSettingsFromRestaurant.Enabled = false;
                            cmbSettingsFromHospitalityType.Enabled = false;
                            tbSettingsFromSequence.Enabled = false;
                        }
                        else
                        {
                            tbSettingsFromRestaurant.Enabled = true;
                            cmbSettingsFromHospitalityType.Enabled = true;
                            tbSettingsFromSequence.Enabled = true;
                        }
                    }
                    break;

                case "DiningTableLayout":
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        DiningTableLayout layout = (DiningTableLayout)cmbDiningTableLayoutID.SelectedData;

                        if (layout.ID == changeIdentifier)
                        {
                            cmbDiningTableLayoutID.SelectedData = new DataEntity("", "");
                        }
                    }
                    else if (changeHint == DataEntityChangeType.Edit)
                    {
                        DiningTableLayout layout = (DiningTableLayout)cmbDiningTableLayoutID.SelectedData;

                        if (layout.ID == changeIdentifier)
                        {
                            cmbDiningTableLayoutID.SelectedData = (DiningTableLayout)param;
                        }
                    }


                    break;
            }

        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");

            btnsEditAddDiningTableLayout.EditButtonEnabled = cmbDiningTableLayoutID.SelectedData != null && cmbDiningTableLayoutID.SelectedData.ID != "";
        }

        private void cmbDiningTableLayoutID_RequestData(object sender, EventArgs e)
        {
            cmbDiningTableLayoutID.SetWidth(350);

            cmbDiningTableLayoutID.SetHeaders(new string[] {
                Properties.Resources.LayoutID,
                Properties.Resources.Description },
                new int[] { 0, 1 });

            cmbDiningTableLayoutID.SetData(Providers.DiningTableLayoutData.GetList(PluginEntry.DataModel, hospitalityType.RestaurantID, hospitalityType.SalesType), null);
        }

        private void cmbSettingsFromHospitalityType_RequestData(object sender, EventArgs e)
        {

            cmbSettingsFromHospitalityType.SetWidth(350);

            cmbSettingsFromHospitalityType.SetHeaders(new string[] { 
                Properties.Resources.RestaurantId, 
                Properties.Resources.SalesType,
                Properties.Resources.Sequence,
                Properties.Resources.Description },
                new int[] { 0, 1, 2, 3 });

            cmbSettingsFromHospitalityType.SetData(Providers.HospitalityTypeData.GetHostpitalityTypesForRestaurant(PluginEntry.DataModel, String.IsNullOrEmpty((string)hospitalityType.AccessToOtherRestaurant) ? hospitalityType.RestaurantID : hospitalityType.AccessToOtherRestaurant, hospitalityType.Overview), null);
        }

        private void cmbSettingsFromHospitalityType_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbSettingsFromHospitalityType_SelectedDataChanged(object sender, EventArgs e)
        {
            // Fill out the textboxes for sequence and restaurant ID            
            if (cmbSettingsFromHospitalityType.SelectedData != null && cmbSettingsFromHospitalityType.SelectedData.ID != "")
            {
                HospitalityTypeListItem item = (HospitalityTypeListItem)cmbSettingsFromHospitalityType.SelectedData;
                tbSettingsFromRestaurant.Text = ((string)item.RestaurantID).Trim();
                tbSettingsFromSequence.Text = item.Sequence.ToString();
            }
        }

        private void btnsEditAddDiningTableLayout_EditButtonClicked(object sender, EventArgs e)
        {
            DiningTableLayout layout = (DiningTableLayout)cmbDiningTableLayoutID.SelectedData;

            PluginOperations.ShowDiningTableLayout(((DiningTableLayout)cmbDiningTableLayoutID.SelectedData).ID);
        }

        private void btnsEditAddDiningTableLayout_AddButtonClicked(object sender, EventArgs e)
        {
            DiningTableLayout newDiningTableLayout = PluginOperations.NewDiningTableLayout(hospitalityType.RestaurantID, hospitalityType.Sequence, hospitalityType.SalesType);

            if (newDiningTableLayout != null)
            {
                cmbDiningTableLayoutID.SelectedData = newDiningTableLayout;
            }
        }

        private void cmbDiningTableLayoutID_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddDiningTableLayout.EditButtonEnabled = cmbDiningTableLayoutID.SelectedData != null && cmbDiningTableLayoutID.SelectedData.ID != "";            
        }

        private void cmbDiningTableLayoutID_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void chkPromptForCustomer_CheckedChanged(object sender, EventArgs e)
        {
            cmbCustomerNameOnTable.Enabled = chkPromptForCustomer.Checked;
        }

        private void chkRequestNoOfGuests_CheckedChanged(object sender, EventArgs e)
        {
            txtMaxNoOfGuests.Enabled = chkRequestNoOfGuests.Checked;
        }
    }
}
