using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.ScreenIdentity;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Visual
{
    public partial class VisualProfileGeneralPage : UserControl, ITabView
    {
        private Dictionary<string, ResolutionsEnum> resolutionMap;
        private VisualProfile visualProfile;

        public VisualProfileGeneralPage()
        {
            InitializeComponent();

            InitializeResolutions();
            InitializeScreenNumbers();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new VisualProfileGeneralPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            visualProfile = (VisualProfile)internalContext;

            chkCursorHide.Checked = visualProfile.HideCursor;

            cmbResolution.SelectedItem = MapResolutionToText(visualProfile.Resolution);
            cmbTerminalType.SelectedIndex = (int)visualProfile.TerminalType;
            cmbScreenNumber.SelectedIndex = (int)visualProfile.ScreenNumber;
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (chkCursorHide.Checked != visualProfile.HideCursor) return true;
            if (resolutionMap[cmbResolution.SelectedItem.ToString()] != visualProfile.Resolution) return true;
            if (cmbTerminalType.SelectedIndex != (int)visualProfile.TerminalType) return true;
            if (cmbScreenNumber.SelectedIndex != (int)visualProfile.ScreenNumber) return true;

            return false;
        }

        public bool SaveData()
        {
            visualProfile.HideCursor = chkCursorHide.Checked;
            visualProfile.Resolution = resolutionMap[cmbResolution.SelectedItem.ToString()];
            visualProfile.TerminalType = (VisualProfile.HardwareTypes)cmbTerminalType.SelectedIndex;
            visualProfile.ScreenNumber = (ScreenNumberEnum)cmbScreenNumber.SelectedIndex;

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
        private void OnLinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            var identifier = new ScreenIdentifier();
            identifier.Identify();
        }

        private string MapResolutionToText(ResolutionsEnum value)
        {
            foreach (string key in resolutionMap.Keys)
            {
                if (resolutionMap[key] == value)
                    return key;
            }

            return Properties.Resources.VisualProfileDefaultScreenResolution;
        }

        private void AddResolution(string description, ResolutionsEnum value)
        {
            cmbResolution.Items.Add(description);
            resolutionMap[description] = value;
        }

        private void InitializeResolutions()
        {
            resolutionMap = new Dictionary<string, ResolutionsEnum>();
            cmbResolution.Items.Clear();
            AddResolution(Properties.Resources.VisualProfileDefaultScreenResolution, ResolutionsEnum.DefaultScreenResolution);
            AddResolution(Properties.Resources.VisualProfileUserResizable, ResolutionsEnum.UserResizable);
            AddResolution("1600 x 1200", ResolutionsEnum._1600x1200);
            AddResolution("1600 x 1024", ResolutionsEnum._1600x1024);
            AddResolution("1280 x 1024", ResolutionsEnum._1280x1024);
            AddResolution("1280 x 768", ResolutionsEnum._1280x768);
            AddResolution("1240 x 1024", ResolutionsEnum._1240x1024);
            AddResolution("1152 x 864", ResolutionsEnum._1152x864);
            AddResolution("1024 x 768", ResolutionsEnum._1024x768);
        }

        private void InitializeScreenNumbers()
        {
            var screens = new object[]
            {
                    ScreenNumberHelper.ScreenNumberToString(ScreenNumberEnum.MainScreen),
                    ScreenNumberHelper.ScreenNumberToString(ScreenNumberEnum.Screen1),
                    ScreenNumberHelper.ScreenNumberToString(ScreenNumberEnum.Screen2),
                    ScreenNumberHelper.ScreenNumberToString(ScreenNumberEnum.Screen3),
            };

            cmbScreenNumber.Items.Clear();
            cmbScreenNumber.Items.AddRange(screens);
        }
    }
}
