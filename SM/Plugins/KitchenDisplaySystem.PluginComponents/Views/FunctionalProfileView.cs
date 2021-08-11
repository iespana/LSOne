using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class FunctionalProfileView : ViewBase
    {
        private RecordIdentifier profileId;
        private KitchenDisplayFunctionalProfile kitchenDisplayFunctionalProfile;

        public FunctionalProfileView(RecordIdentifier profileId)
            : this()
        {
            this.profileId = profileId;
            kitchenDisplayFunctionalProfile = Providers.KitchenDisplayFunctionalProfileData.Get(PluginEntry.DataModel, profileId);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        private FunctionalProfileView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            AddToComboBoxes();
        }

        private void AddToComboBoxes()
        {
            // Bump Possible
            cmbBumpPossible.Items.Add(
                 new DataEntity((int)BumpPossibleEnum.Always,
                               KitchenDisplayFunctionalProfile.BumpPossibleText(BumpPossibleEnum.Always)));
            cmbBumpPossible.Items.Add(
                 new DataEntity((int)BumpPossibleEnum.WhenOrderPaid,
                               KitchenDisplayFunctionalProfile.BumpPossibleText(BumpPossibleEnum.WhenOrderPaid)));
            cmbBumpPossible.Items.Add(
                 new DataEntity((int)BumpPossibleEnum.WhenOrderDone,
                               KitchenDisplayFunctionalProfile.BumpPossibleText(BumpPossibleEnum.WhenOrderDone)));

            cmbRecalledOrdersAppear.Items.Add(
                 new DataEntity((int)RecalledOrdersAppearEnum.InFront,
                               KitchenDisplayFunctionalProfile.RecalledOrdersAppearText(RecalledOrdersAppearEnum.InFront)));
            cmbRecalledOrdersAppear.Items.Add(
                 new DataEntity((int)RecalledOrdersAppearEnum.WhereTheyWereBefore,
                               KitchenDisplayFunctionalProfile.RecalledOrdersAppearText(RecalledOrdersAppearEnum.WhereTheyWereBefore)));

            cmbDoneOrdersAppear.Items.Add(
                new DataEntity((int)DoneOrdersAppearEnum.OriginalPosition,
                              KitchenDisplayFunctionalProfile.DoneOrdersAppearText(DoneOrdersAppearEnum.OriginalPosition)));
            cmbDoneOrdersAppear.Items.Add(
                new DataEntity((int)DoneOrdersAppearEnum.InFront,
                              KitchenDisplayFunctionalProfile.DoneOrdersAppearText(DoneOrdersAppearEnum.InFront)));
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYFUNCTIONALPROFILELog",profileId, Properties.Resources.FunctionalProfile, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.FunctionalProfile + ": " + kitchenDisplayFunctionalProfile.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return kitchenDisplayFunctionalProfile.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FunctionalProfile + ": " + kitchenDisplayFunctionalProfile.Text;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return profileId;
            }
        }       

        protected override void LoadData(bool isRevert)
        {
            string bumpPossibleString =
                KitchenDisplayFunctionalProfile.BumpPossibleText(kitchenDisplayFunctionalProfile.BumpPossible);
            string RecalledOrdersAppearString =
                KitchenDisplayFunctionalProfile.RecalledOrdersAppearText(kitchenDisplayFunctionalProfile.RecalledOrdersAppear);
            string DoneOrdersAppearString =
                KitchenDisplayFunctionalProfile.DoneOrdersAppearText(kitchenDisplayFunctionalProfile.DoneOrdersAppear);

            cmbBumpPossible.SelectedIndex = cmbBumpPossible.FindStringExact(bumpPossibleString);
            cmbRecalledOrdersAppear.SelectedIndex = cmbRecalledOrdersAppear.FindStringExact(RecalledOrdersAppearString);
            cmbDoneOrdersAppear.SelectedIndex = cmbDoneOrdersAppear.FindStringExact(DoneOrdersAppearString);
            tbProfileName.Text = kitchenDisplayFunctionalProfile.Text;
            cmbButtons.SelectedData = new DataEntity(kitchenDisplayFunctionalProfile.ButtonsMenuId, kitchenDisplayFunctionalProfile.ButtonsMenuDescription);
            chkSoundOnNewOrder.Checked = kitchenDisplayFunctionalProfile.SoundOnNewOrder;
            tbAutoBump.Value = kitchenDisplayFunctionalProfile.TimeUntilForceBumpFromStation;

            HeaderText = Description;
        }

        protected override bool DataIsModified()
        {
            if (tbProfileName.Text != kitchenDisplayFunctionalProfile.Text) return true;
            if (cmbButtons.SelectedData.ID != kitchenDisplayFunctionalProfile.ButtonsMenuId) return true;
            if (((DataEntity)cmbBumpPossible.SelectedItem).ID != (int)kitchenDisplayFunctionalProfile.BumpPossible) return true;
            if (((DataEntity)cmbRecalledOrdersAppear.SelectedItem).ID != (int)kitchenDisplayFunctionalProfile.RecalledOrdersAppear) return true;
            if (((DataEntity)cmbDoneOrdersAppear.SelectedItem).ID != (int)kitchenDisplayFunctionalProfile.DoneOrdersAppear) return true;
            if ((int)((DataEntity)cmbBumpPossible.SelectedItem).ID != (int)kitchenDisplayFunctionalProfile.BumpPossible) return true;
            if (chkSoundOnNewOrder.Checked != kitchenDisplayFunctionalProfile.SoundOnNewOrder) return true;
            if ((int)tbAutoBump.Value != kitchenDisplayFunctionalProfile.TimeUntilForceBumpFromStation) return true;

            return false;
        }

        protected override bool SaveData()
        {
            kitchenDisplayFunctionalProfile.Text = tbProfileName.Text;
            kitchenDisplayFunctionalProfile.ButtonsMenuId = (string)cmbButtons.SelectedData.ID;
            kitchenDisplayFunctionalProfile.BumpPossible = (BumpPossibleEnum)(int)((DataEntity)cmbBumpPossible.SelectedItem).ID;
            kitchenDisplayFunctionalProfile.RecalledOrdersAppear = (RecalledOrdersAppearEnum)(int)((DataEntity)cmbRecalledOrdersAppear.SelectedItem).ID;
            kitchenDisplayFunctionalProfile.DoneOrdersAppear = (DoneOrdersAppearEnum)(int)((DataEntity)cmbDoneOrdersAppear.SelectedItem).ID;
            kitchenDisplayFunctionalProfile.SoundOnNewOrder = chkSoundOnNewOrder.Checked;
            kitchenDisplayFunctionalProfile.TimeUntilForceBumpFromStation = (int)tbAutoBump.Value;

            Providers.KitchenDisplayFunctionalProfileData.Save(PluginEntry.DataModel, kitchenDisplayFunctionalProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenDisplayFunctionalProfile", profileId, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperationsHelper.DeleteFunctionalProfile(profileId);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayFunctionalProfile":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == profileId)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.ButtonProfileDialog();
            dlg.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var buttonGridId = cmbButtons.SelectedData.ID;
            PluginOperationsHelper.ShowButtonProfilesView(buttonGridId);
        }

        private void cmbButtons_RequestData(object sender, EventArgs e)
        {
            var listOfKDSButtonGrids = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.KitchenDisplay);
            cmbButtons.SetData(listOfKDSButtonGrids, null);
        }
    }
}
