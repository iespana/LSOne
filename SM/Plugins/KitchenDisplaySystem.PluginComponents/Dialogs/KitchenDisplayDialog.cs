using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class KitchenDisplayDialog : DialogBase
    {
        KitchenDisplayStation kds;
           
        internal KitchenDisplayDialog()
        {
            InitializeComponent();

            cmbKdsFunctionalProfile.SelectedData = new DataEntity("","");
            cmbKdsStyleProfile.SelectedData = new DataEntity("", "");
            cmbKdsVisualProfile.SelectedData = new DataEntity("", "");
            cmbDisplayProfile.SelectedData = new DataEntity("", "");
            cmbScreenNumber.SelectedIndex = 0;
            cmbHorizontalLocation.SelectedIndex = 0;
            cmbVerticalLocation.SelectedIndex = 0;
            chkFullScreen.Checked = true;

            foreach (var mode in Enum.GetValues(typeof(KitchenDisplayStation.StationTypeEnum)))
            {
                // Upcoming orders not supported yet in LS One
                if ((KitchenDisplayStation.StationTypeEnum)mode != KitchenDisplayStation.StationTypeEnum.UpcomingOrdersStation)
                {
                    cmbStationType.Items.Add(
                        KitchenDisplayStation.GetStationTypeText((KitchenDisplayStation.StationTypeEnum)mode));
                }
            }
            cmbStationType.SelectedIndex = 0;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier KitchenDisplayStationId { get; private set; }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (kds == null)
            {
                btnOK.Enabled = tbStationName.Text.Length > 0
                    && cmbKdsFunctionalProfile.SelectedData.ID != ""
                    && cmbKdsStyleProfile.SelectedData.ID != ""
                    && cmbKdsVisualProfile.SelectedData.ID != ""
                    && cmbDisplayProfile.SelectedData.ID != "";

            }
            else
            {
                btnOK.Enabled = ((tbStationName.Text.Length > 0) && kds.Text != tbStationName.Text)
                                ||
                                kds.KitchenDisplayFunctionalProfileId != cmbKdsFunctionalProfile.SelectedData.ID
                                ||
                                kds.KitchenDisplayStyleProfileId != cmbKdsStyleProfile.SelectedData.ID
                                ||
                                kds.KitchenDisplayVisualProfileId != cmbKdsVisualProfile.SelectedData.ID
                                ||
                                cmbScreenNumber.SelectedIndex != (int) kds.ScreenNumber
                                ||
                                kds.KitchenDisplayProfileId != cmbDisplayProfile.SelectedData.ID;
            }
        }

        private bool Save()
        {
            if (kds == null)
            {
                kds = new KitchenDisplayStation();
            }

            if (tbID.Text.Trim() == "")
            {
                if (QuestionDialog.Show(Properties.Resources.IDMissingQuestion, Properties.Resources.IDMissing) != System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                kds.ID = tbID.Text.Trim();
                
                if (((string)kds.ID).Contains(" "))
                {
                    errorProvider1.SetError(tbID, Properties.Resources.IDCannotContainWhitespace);
                    return false;
                }
                else if (Providers.KitchenDisplayStationData.Exists(PluginEntry.DataModel, kds.ID))
                {
                    errorProvider1.SetError(tbID, Properties.Resources.DisplayStationIDExists);
                    return false;
                }                
            }

            kds.Text = tbStationName.Text;

            kds.KitchenDisplayFunctionalProfileId = (Guid)cmbKdsFunctionalProfile.SelectedData.ID;
            kds.KitchenDisplayStyleProfileId = (Guid)cmbKdsStyleProfile.SelectedData.ID;
            kds.KitchenDisplayVisualProfileId = (Guid)cmbKdsVisualProfile.SelectedData.ID;
            kds.KitchenDisplayProfileId = cmbDisplayProfile.SelectedData.ID;
            kds.ScreenNumber = (KitchenDisplayStation.ScreenNumberEnum)cmbScreenNumber.SelectedIndex;
            kds.NextStationId = "";
            kds.UseExternalRouting = false;
            kds.FullScreen = chkFullScreen.Checked;
            kds.HorizontalPosition = (KitchenDisplayStation.HorizontalPositionEnum)cmbHorizontalLocation.SelectedIndex;
            kds.VerticalPosition = (KitchenDisplayStation.VerticalPositionEnum)cmbVerticalLocation.SelectedIndex;
            kds.StationType = (KitchenDisplayStation.StationTypeEnum)cmbStationType.SelectedIndex;

            Providers.KitchenDisplayStationData.Save(PluginEntry.DataModel, kds);

            KitchenDisplayStationId = kds.ID;
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbKdsFunctionalProfile_RequestData(object sender, EventArgs e)
        {
            var listOfFunctionalProfiles = Providers.KitchenDisplayFunctionalProfileData.GetList(PluginEntry.DataModel);
            cmbKdsFunctionalProfile.SetData(listOfFunctionalProfiles, null);
        }

        private void cmbKdsStyleProfile_RequestData(object sender, EventArgs e)
        {
            var listOfStyleProfiles = Providers.KitchenDisplayStyleProfileData.GetList(PluginEntry.DataModel);
            cmbKdsStyleProfile.SetData(listOfStyleProfiles, null);
        }

        private void cmbKdsVisualProfile_RequestData(object sender, EventArgs e)
        {
            var listOfVisualProfiles = Providers.KitchenDisplayVisualProfileData.GetList(PluginEntry.DataModel);
            cmbKdsVisualProfile.SetData(listOfVisualProfiles, null);
        }

        private void btnsFunctionalProfile_AddButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new Dialogs.NewFunctionalProfileDialog())
            {
                dlg.ShowDialog();
            }
        }

        private void btnStyleProfile_AddButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new Dialogs.NewStyleProfileDialog())
            {
                dlg.ShowDialog();
            }
        }

        private void btnsVisualProfile_AddButtonClicked(object sender, EventArgs e)
        {
            using (var dlg = new Dialogs.NewVisualProfileDialog())
            {
                dlg.ShowDialog();
            }
        }

        private void chkFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            bool checkState = chkFullScreen.Checked;
            lblHorizontalLocation.Enabled = !checkState;
            lblVertialLocation.Enabled = !checkState;
            cmbHorizontalLocation.Enabled = !checkState;
            cmbVerticalLocation.Enabled = !checkState;
        }

        private void cmbDisplayProfile_RequestData(object sender, EventArgs e)
        {
            cmbDisplayProfile.SetData(Providers.KitchenDisplayProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void btnDisplayProfileAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new Dialogs.NewDisplayProfileDialog())
            {
                dlg.ShowDialog();
            }
        }
    }
}
