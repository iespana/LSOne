using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Peripherals.Interfaces;
using LSOne.Peripherals.Properties;
using LSOne.POS.Core;
using LSOne.Services.Interfaces.Configurations;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using Microsoft.Win32;
using LSOne.Utilities.ColorPalette;
using LSOne.Controls.SupportClasses;

namespace LSOne.Peripherals.Dialogs
{
    public partial class HardwareConfigurationDialog : TouchBaseForm
    {
        private HardwareProfile profile;
        private List<DataEntity> profiles;
        private int currentStepIndex = 0;
        private HardwareConfigurationStep currentStep;
        private List<HardwareConfigurationStep> hardwareConfigurationSteps;
        private int selectedPanelIndex;
        public bool HardwareProfileIsModified { get; set; }
        public Stopwatch sw { get; set; }
        private DetectionDialog dlg;

        private enum Buttons
        {
            Detect,
            Finish,
            Exit
        }

        public HardwareConfigurationDialog()
        {
            InitializeComponent();

            Screen sc = Screen.PrimaryScreen;
            Location = new Point(sc.Bounds.Left + (sc.Bounds.Width / 2) - (Width / 2), sc.Bounds.Top + (sc.Bounds.Height / 2) - (Height / 2));
            selectedPanelIndex = -1;
        }

        public Guid LastUserContext { get; set; }
        public SQLServerLoginEntry SqlServerLoginEntry { get; set; }

        public SiteServiceConfig SiteServiceConfiguration { get; set; }

        public List<HardwareConfigurationStep> HardwareConfigurationSteps
        {
            get
            {
                if (hardwareConfigurationSteps == null)
                {
                    hardwareConfigurationSteps = new List<HardwareConfigurationStep>();
                }
                return hardwareConfigurationSteps;
            }
            set { hardwareConfigurationSteps = value; }
        }

        public HardwareProfile Profile
        {
            get
            {
                if (profile == null)
                {
                    profile = new HardwareProfile();
                }
                return profile;
            }
            set { profile = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DLLEntry.DataModel.HasPermission(Permission.TerminalEdit))
            {
                LSOne.Controls.TouchMessageDialog.Show(this, Resources.CouldNotLoadHardware + "\r\n\n" + Resources.CurrentProfileWillBeApplied);
                DialogResult = DialogResult.OK;
                Close();
            }

            for (int i = 0; i < HardwareConfigurationSteps.Count; i++)
            {
                buttonPanelSteps.AddButton(HardwareConfigurationSteps[i].Description, null, "");
                buttonPanelSteps.SetButtonClickable(selectedPanelIndex, i != 0);

                HardwareConfigurationSteps[i].Step.Dock = DockStyle.Fill;
                HardwareConfigurationSteps[i].Step.Visible = false;
                pnlPages.Controls.Add(HardwareConfigurationSteps[i].Step);
            }

            buttonPanelSteps.SetButtonPressed(0, true);

            buttonPanelActions.AddButton(Resources.AutoDetect, Buttons.Detect, "", TouchButtonType.Action, DockEnum.DockEnd);
            buttonPanelActions.AddButton(Resources.Finish, Buttons.Finish, "", TouchButtonType.OK, DockEnum.DockEnd);
            buttonPanelActions.AddButton(Resources.Exit, Buttons.Exit, "", TouchButtonType.Cancel, DockEnum.DockEnd);


            currentStep = HardwareConfigurationSteps.Find(x => x.StepIndex == currentStepIndex);
            SetSelectedPanel(0, true);

            string sort = "NAME ASC";
            profiles = Providers.HardwareProfileData.GetList(DLLEntry.DataModel, sort);
            
            if (profile != null)
            {
                var selectedProfile = profiles.Find(x => x.ID == profile.ID);

                cmbHardwareProfile.SelectedData = selectedProfile;
                foreach (var hardwareConfigurationStep in HardwareConfigurationSteps)
                {
                    var step = hardwareConfigurationStep.Step as IHardwareValidator;
                    if (step != null)
                    {
                        step.LoadProfile(profile);
                    }
                }
            }
        }

        internal static List<string> GetRegistryStrings(string oposdevice)
        {
            List<string> registryStrings = new List<string>();

            using (RegistryKey Key = Registry.LocalMachine.OpenSubKey("Software\\OLEforRetail\\ServiceOPOS\\" + oposdevice + "\\", false))
            {
                if (Key != null)
                {
                    registryStrings = Key.GetSubKeyNames().ToList();
                }
                else
                {
                    using (RegistryKey Key2 = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\OLEforRetail\\ServiceOPOS\\" + oposdevice + "\\", false))
                    {
                        if (Key2 != null)
                        {
                            registryStrings = Key2.GetSubKeyNames().ToList();
                        }
                    }
                }
            }

            return registryStrings;
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetSelectedPanel(int panelIndex, bool initializing = false)
        {
            bool showStep = true;
            if (currentStep.Step is IHardwareValidator && !initializing)
            {
                showStep = ((IHardwareValidator) currentStep.Step).ValiddateInput();
            }
            if (showStep)
            {

                int oldPanelIndex = selectedPanelIndex;

                if (selectedPanelIndex != panelIndex)
                {
                    selectedPanelIndex = panelIndex;

                    HardwareConfigurationSteps[selectedPanelIndex].Step.Visible = true;

                    buttonPanelSteps.SetButtonClickable(selectedPanelIndex, false);

                    buttonPanelSteps.SetButtonPressed(selectedPanelIndex, true);
                    if (oldPanelIndex >= 0)
                    {
                        HardwareConfigurationSteps[oldPanelIndex].Step.Visible = false;
                        buttonPanelSteps.SetButtonClickable(oldPanelIndex, true);

                        buttonPanelSteps.SetButtonPressed(oldPanelIndex, false);
                    }
                    currentStepIndex = selectedPanelIndex;
                    currentStep = HardwareConfigurationSteps.Find(x => x.StepIndex == currentStepIndex);
                }
            }
            else
            {
                buttonPanelSteps.SetButtonClickable(selectedPanelIndex, false);
                buttonPanelSteps.SetButtonPressed(selectedPanelIndex, true);
                buttonPanelSteps.SetButtonPressed(panelIndex, false);
            }
        }

        private void cmbHardwareProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            profile = Providers.HardwareProfileData.Get(DLLEntry.DataModel, cmbHardwareProfile.SelectedDataID);
            foreach (var hardwareConfigurationStep in HardwareConfigurationSteps)
            {
                var step = hardwareConfigurationStep.Step as IHardwareValidator;
                if (step != null)
                {
                    step.LoadProfile(profile);
                }
            }
        }

        private void buttonPanelCategories_Click(object sender, LSOne.Controls.SupportClasses.ScrollButtonEventArguments args)
        {
            SetSelectedPanel(args.Index);
        }

        private void buttonPanelActions_Click(object sender, LSOne.Controls.SupportClasses.ScrollButtonEventArguments args)
        {
            if (args.Tag != null && args.Tag is Buttons)
            {
                switch ((Buttons)args.Tag)
                {
                    case Buttons.Detect:
                        AutoDetect();
                        break;
                    case Buttons.Finish:
                        Finish();
                        break;
                    case Buttons.Exit:
                        DialogResult = DialogResult.Cancel;
                        Close();
                        break;
                }
            }
        }

        private void Finish()
        {
            if (currentStep.Step is IHardwareValidator)
            {
                if (!((IHardwareValidator) currentStep.Step).ValiddateInput())
                    return;
            }
            if (HardwareProfileIsModified)
            {
                profile.ID = RecordIdentifier.Empty;
            }

            Terminal terminal = Providers.TerminalData.Get(DLLEntry.DataModel, SqlServerLoginEntry.TerminalID, SqlServerLoginEntry.StoreID);

            profile.Text = terminal.ID + "-" + terminal.Name + "-" + DateTime.Now.ToShortDateString();

            try
            {           
                Services.Interfaces.Services.SiteServiceService(DLLEntry.DataModel).SetHardwareProfile(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, SqlServerLoginEntry.TerminalID, SqlServerLoginEntry.StoreID, profile);
            }
            catch
            {
                TouchMessageDialog.Show(
                    new Point(Screen.PrimaryScreen.Bounds.Width/2, Screen.PrimaryScreen.Bounds.Height/2),
                    Resources.HardwareNotSavedOnHO, MessageBoxButtons.OK,
                    MessageDialogType.Attention);
            }

            Providers.HardwareProfileData.Save(DLLEntry.DataModel, profile);

            terminal.HardwareProfileID = profile.ID;

            Providers.TerminalData.Save(DLLEntry.DataModel, terminal);
            DLLEntry.Settings.Terminal.HardwareProfileID = profile.ID;
            DLLEntry.Settings.LoadProfiles(DLLEntry.DataModel, DLLEntry.DataModel.CurrentUser.ID);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AutoDetect()
        {
            buttonPanelActions.Enabled = false;
            dlg = new DetectionDialog(this);

            dlg.ShowDialog();

            foreach (var hardwareConfigurationStep in HardwareConfigurationSteps)
            {
                if (hardwareConfigurationStep.Step is IHardwareValidator)
                {
                    IHardwareValidator step = hardwareConfigurationStep.Step as IHardwareValidator;
                    step.SetDetectedDevice();
                    step.ValiddateInput();
                }
            }
          
            buttonPanelActions.Enabled = true;
        }

        private void HardwareConfigurationDialog_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            Rectangle r = new Rectangle(pnlPages.Location.X-1, pnlPages.Location.Y-1, pnlPages.Width+1, pnlPages.Height+1);
            e.Graphics.DrawRectangle(p, r);
            p.Dispose();
        }

        private void cmbHardwareProfile_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                profiles,
                null,
                true,
                cmbHardwareProfile.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }
   }
}
