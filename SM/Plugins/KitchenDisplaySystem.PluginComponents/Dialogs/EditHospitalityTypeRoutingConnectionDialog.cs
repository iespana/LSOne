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
    public partial class EditHospitalityTypeRoutingConnectionDialog : DialogBase
    {
        LSOneKitchenDisplayHospitalityTypeRoutingConnection hospitalityTypeRoutingConnection;

        public EditHospitalityTypeRoutingConnectionDialog(RecordIdentifier hospitalityTypeRoutingConnectionId)
        {
            InitializeComponent();

            hospitalityTypeRoutingConnection = Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.Get(PluginEntry.DataModel, hospitalityTypeRoutingConnectionId);

            cmbHospitalityType.SelectedData = Providers.HospitalityTypeData.Get(PluginEntry.DataModel, hospitalityTypeRoutingConnection.Restaurant, hospitalityTypeRoutingConnection.SalesType);
            cmbInclude.SelectedIndex = (int)hospitalityTypeRoutingConnection.IncludeExclude;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            HospitalityType selectedHospitalityType = (HospitalityType)cmbHospitalityType.SelectedData;

            btnOK.Enabled =
                selectedHospitalityType.RestaurantID != hospitalityTypeRoutingConnection.Restaurant ||
                selectedHospitalityType.SalesType != hospitalityTypeRoutingConnection.SalesType ||
                cmbInclude.SelectedIndex != (int)hospitalityTypeRoutingConnection.IncludeExclude;
        }

        private void cmbHospitalityType_RequestData(object sender, EventArgs e)
        {
            cmbHospitalityType.SetData(Providers.HospitalityTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            HospitalityType selectedHospitalityType = (HospitalityType)cmbHospitalityType.SelectedData;
            hospitalityTypeRoutingConnection.Restaurant = selectedHospitalityType.RestaurantID;
            hospitalityTypeRoutingConnection.SalesType = selectedHospitalityType.SalesType;
            hospitalityTypeRoutingConnection.IncludeExclude = (LSOneKitchenDisplayHospitalityTypeRoutingConnection.IncludeEnum)cmbInclude.SelectedIndex;

            Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.Save(PluginEntry.DataModel, hospitalityTypeRoutingConnection);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "KitchenDisplayHospitalityTypeConnection", hospitalityTypeRoutingConnection.Id, hospitalityTypeRoutingConnection);

            Close();
        }
    }
}
