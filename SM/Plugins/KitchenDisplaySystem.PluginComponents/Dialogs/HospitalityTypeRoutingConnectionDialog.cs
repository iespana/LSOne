using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class HospitalityTypeRoutingConnectionDialog : DialogBase
    {
        private RecordIdentifier kdsId;
        private LSOneKitchenDisplayHospitalityTypeRoutingConnection hospitalityTypeRoutingConnection;
        DataEntitySelectionList selectionList = null;
        private List<DataEntity> selectedItems;
        private List<DataEntity> entities;

        public HospitalityTypeRoutingConnectionDialog(RecordIdentifier kdsId)
        {
            InitializeComponent();
            this.kdsId = kdsId;
            entities = Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.HospitalitTypesNotConnectedToKds(PluginEntry.DataModel, (string)kdsId);            
            selectionList = new DataEntitySelectionList(entities);
            cmbHospitalityType.DropDown += cmbHospitalityType_DropDown;
            cmbHospitalityType.SelectedData = selectionList;
            cmbInclude.SelectedIndex = 0;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ID
        {
            get { return hospitalityTypeRoutingConnection.Id; }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = (cmbHospitalityType.SelectedData.ID != "");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var item in selectedItems)
            {
                hospitalityTypeRoutingConnection = new LSOneKitchenDisplayHospitalityTypeRoutingConnection();
                hospitalityTypeRoutingConnection.Id = Guid.NewGuid();
                hospitalityTypeRoutingConnection.StationId = (string)kdsId;
                hospitalityTypeRoutingConnection.Type = StationSelection.HospitalityTypeConnectionEnum.HospitalityType;
                hospitalityTypeRoutingConnection.ConnectionId = (string)item.ID[0];
                hospitalityTypeRoutingConnection.Restaurant = (string)item.ID[1];
                hospitalityTypeRoutingConnection.SalesType = (string)item.ID[2];
                hospitalityTypeRoutingConnection.IncludeExclude = (LSOneKitchenDisplayHospitalityTypeRoutingConnection.IncludeEnum)cmbInclude.SelectedIndex;

                Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.Save(PluginEntry.DataModel, hospitalityTypeRoutingConnection);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbHospitalityType_DropDown(object sender, DropDownEventArgs e)
        {
            DataEntitySelectionList list = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            if (list != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(list);
            }
        }

        private void cmbHospitalityType_SelectedDataChanged(object sender, EventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                selectionList = (DataEntitySelectionList)((DualDataComboBox)sender).SelectedData;

                selectedItems = selectionList.GetSelectedItems();

                var names = new List<string>();
                var ids = new List<string>();

                foreach (DataEntity item in selectedItems)
                {
                    ids.Add((string)item.ID);
                    names.Add(item.Text);
                }
            }

            CheckEnabled();
        }
    }
}
