using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Profiles.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Visual
{
    public partial class VisualProfileColorPalettePage : UserControl, ITabView
    {

        private VisualProfile visualProfile;

        public VisualProfileColorPalettePage()
        {
            InitializeComponent();

            btnsEditAddConfirmButtonStyle.Visible =
                btnsEditAddCancelButtonStyle.Visible =
                btnsEditAddActionButtonStyle.Visible =
                btnsEditAddNormalButtonStyle.Visible =
                btnsEditAddOtherButtonStyle.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageStyleSetup) && PluginEntry.Framework.CanRunOperation("NewUIStyle");
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new VisualProfileColorPalettePage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            visualProfile = (VisualProfile)internalContext;

            LoadStyleData();            

            chkOverridePOSBorderColor.Checked = visualProfile.OverridePOSControlBorderColor;
            cwPOSControlBorderColor.SelectedColor = Color.FromArgb(visualProfile.POSControlBorderColor);

            chkOverridePOSSelectedRowColor.Checked = visualProfile.OverridePOSSelectedRowColor;
            cwPOSSelectedRowColor.SelectedColor = Color.FromArgb(visualProfile.POSSelectedRowColor);
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (cmbConfirmButtonStyle.SelectedDataID != visualProfile.ConfirmButtonStyleID) return true;
            if (cmbCancelButtonStyle.SelectedDataID != visualProfile.CancelButtonStyleID) return true;
            if (cmbActionButtonStyle.SelectedDataID != visualProfile.ActionButtonStyleID) return true;
            if (cmbNormalButtonStyle.SelectedDataID != visualProfile.NormalButtonStyleID) return true;
            if (cmbOtherButtonStyle.SelectedDataID != visualProfile.OtherButtonStyleID) return true;

            if (chkOverridePOSBorderColor.Checked != visualProfile.OverridePOSControlBorderColor) return true;
            if (cwPOSControlBorderColor.SelectedColor.ToArgb() != visualProfile.POSControlBorderColor) return true;

            if (chkOverridePOSSelectedRowColor.Checked != visualProfile.OverridePOSSelectedRowColor) return true;
            if (cwPOSSelectedRowColor.SelectedColor.ToArgb() != visualProfile.POSSelectedRowColor) return true;

            return false;
        }

        public bool SaveData()
        {
            visualProfile.ConfirmButtonStyleID = cmbConfirmButtonStyle.SelectedDataID;
            visualProfile.CancelButtonStyleID = cmbCancelButtonStyle.SelectedDataID;
            visualProfile.ActionButtonStyleID = cmbActionButtonStyle.SelectedDataID;
            visualProfile.NormalButtonStyleID = cmbNormalButtonStyle.SelectedDataID;
            visualProfile.OtherButtonStyleID = cmbOtherButtonStyle.SelectedDataID;

            visualProfile.OverridePOSControlBorderColor = chkOverridePOSBorderColor.Checked;
            visualProfile.POSControlBorderColor = cwPOSControlBorderColor.SelectedColor.ToArgb();

            visualProfile.OverridePOSSelectedRowColor = chkOverridePOSSelectedRowColor.Checked;
            visualProfile.POSSelectedRowColor = cwPOSSelectedRowColor.SelectedColor.ToArgb();

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {            
            if(objectName == "UIStyle" && (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.MultiDelete))
            {
                LoadStyleData();
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadStyleData()
        {
            cmbConfirmButtonStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, visualProfile.ConfirmButtonStyleID) ?? new DataEntity("", "");
            cmbCancelButtonStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, visualProfile.CancelButtonStyleID) ?? new DataEntity("", ""); ;
            cmbActionButtonStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, visualProfile.ActionButtonStyleID) ?? new DataEntity("", ""); ;
            cmbNormalButtonStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, visualProfile.NormalButtonStyleID) ?? new DataEntity("", ""); ;
            cmbOtherButtonStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, visualProfile.OtherButtonStyleID) ?? new DataEntity("", ""); ;

            cmbConfirmButtonStyle.TriggerSelectedDataChangedEvent();
            cmbCancelButtonStyle.TriggerSelectedDataChangedEvent();
            cmbActionButtonStyle.TriggerSelectedDataChangedEvent();
            cmbNormalButtonStyle.TriggerSelectedDataChangedEvent();
            cmbOtherButtonStyle.TriggerSelectedDataChangedEvent();
        }

        private void chkOverridePOSBorderColor_CheckedChanged(object sender, EventArgs e)
        {
            lblPOSControlBorderColor.Enabled = cwPOSControlBorderColor.Enabled = chkOverridePOSBorderColor.Checked;
        }

        private void chkOverridePOSSelectedRowColor_CheckedChanged(object sender, EventArgs e)
        {
            lblPOSSelectedRowColor.Enabled = cwPOSSelectedRowColor.Enabled = chkOverridePOSSelectedRowColor.Checked;
        }

        private void colorPaletteStyleCombobox_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC"), null);
        }

        private void colorPaletteStyleCombobox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void btnsEditAddConfirmButtonStyle_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity result = RunNewUIStyleOperation();

            if (result != null)
            {
                cmbConfirmButtonStyle.SelectedData = result;
                cmbConfirmButtonStyle_SelectedDataChanged(this, EventArgs.Empty);
            }
        }

        private void cmbConfirmButtonStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddConfirmButtonStyle.EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(cmbConfirmButtonStyle.SelectedDataID);
        }

        private void cmbCancelButtonStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddCancelButtonStyle.EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(cmbCancelButtonStyle.SelectedDataID);
        }

        private void cmbActionButtonStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddActionButtonStyle.EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(cmbActionButtonStyle.SelectedDataID);
        }

        private void cmbNormalButtonStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddNormalButtonStyle.EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(cmbNormalButtonStyle.SelectedDataID);
        }

        private void cmbOtherButtonStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAddOtherButtonStyle.EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(cmbOtherButtonStyle.SelectedDataID);
        }

        private void btnsEditAddConfirmButtonStyle_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.Framework.CanRunOperation("EditUIStyle"))
            {
                PluginEntry.Framework.RunOperation("EditUIStyle", this, new PluginOperationArguments(cmbConfirmButtonStyle.SelectedDataID, this));
            }
        }

        private void btnsEditAddCancelButtonStyle_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.Framework.CanRunOperation("EditUIStyle"))
            {
                PluginEntry.Framework.RunOperation("EditUIStyle", this, new PluginOperationArguments(cmbCancelButtonStyle.SelectedDataID, this));
            }
        }

        private void btnsEditAddActionButtonStyle_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.Framework.CanRunOperation("EditUIStyle"))
            {
                PluginEntry.Framework.RunOperation("EditUIStyle", this, new PluginOperationArguments(cmbActionButtonStyle.SelectedDataID, this));
            }
        }

        private void btnsEditAddNormalButtonStyle_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.Framework.CanRunOperation("EditUIStyle"))
            {
                PluginEntry.Framework.RunOperation("EditUIStyle", this, new PluginOperationArguments(cmbNormalButtonStyle.SelectedDataID, this));
            }
        }

        private void btnsEditAddOtherButtonStyle_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.Framework.CanRunOperation("EditUIStyle"))
            {
                PluginEntry.Framework.RunOperation("EditUIStyle", this, new PluginOperationArguments(cmbOtherButtonStyle.SelectedDataID, this));
            }
        }

        private DataEntity RunNewUIStyleOperation()
        {
            if (PluginEntry.Framework.CanRunOperation("NewUIStyle"))
            {
                PluginOperationArguments args = new PluginOperationArguments(RecordIdentifier.Empty, null);
                PluginEntry.Framework.RunOperation("NewUIStyle", this, args);
                return args.Result != null ? (DataEntity)args.Result : null;
            }

            return null;
        }

        private void btnsEditAddCancelButtonStyle_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity result = RunNewUIStyleOperation();

            if (result != null)
            {
                cmbCancelButtonStyle.SelectedData = result;
                cmbCancelButtonStyle_SelectedDataChanged(this, EventArgs.Empty);
            }
        }

        private void btnsEditAddActionButtonStyle_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity result = RunNewUIStyleOperation();

            if (result != null)
            {
                cmbActionButtonStyle.SelectedData = result;
                cmbActionButtonStyle_SelectedDataChanged(this, EventArgs.Empty);
            }
        }
        private void btnsEditAddNormalButtonStyle_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity result = RunNewUIStyleOperation();

            if (result != null)
            {
                cmbNormalButtonStyle.SelectedData = result;
                cmbNormalButtonStyle_SelectedDataChanged(this, EventArgs.Empty);
            }
        }
        private void btnsEditAddOtherButtonStyle_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity result = RunNewUIStyleOperation();

            if (result != null)
            {
                cmbOtherButtonStyle.SelectedData = result;
                cmbOtherButtonStyle_SelectedDataChanged(this, EventArgs.Empty);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ApplyColorPaletteStyleOverrides();
            using (ColorPalettePreviewDialog dlg = new ColorPalettePreviewDialog())
            {
                dlg.ShowDialog(this);
            }

            TouchButtonTypeHelper.ClearStyleOverrides();
            LSOne.Utilities.ColorPalette.ColorPalette.SetPOSControlBorderColorOverride(null);
            LSOne.Utilities.ColorPalette.ColorPalette.SetPOSSelectedRowColorOverride(null);
        }

        private void ApplyColorPaletteStyleOverrides()
        {
            if (!RecordIdentifier.IsEmptyOrNull(cmbConfirmButtonStyle.SelectedDataID))
            {
                TouchButtonTypeHelper.SetStyleOverride(TouchButtonType.OK, Providers.PosStyleData.Get(PluginEntry.DataModel, cmbConfirmButtonStyle.SelectedDataID).ConvertToStyle());
            }

            if (!RecordIdentifier.IsEmptyOrNull(cmbCancelButtonStyle.SelectedDataID))
            {
                TouchButtonTypeHelper.SetStyleOverride(TouchButtonType.Cancel, Providers.PosStyleData.Get(PluginEntry.DataModel, cmbCancelButtonStyle.SelectedDataID).ConvertToStyle());
            }

            if (!RecordIdentifier.IsEmptyOrNull(cmbActionButtonStyle.SelectedDataID))
            {
                TouchButtonTypeHelper.SetStyleOverride(TouchButtonType.Action, Providers.PosStyleData.Get(PluginEntry.DataModel, cmbActionButtonStyle.SelectedDataID).ConvertToStyle());
            }

            if (!RecordIdentifier.IsEmptyOrNull(cmbNormalButtonStyle.SelectedDataID))
            {
                TouchButtonTypeHelper.SetStyleOverride(TouchButtonType.Normal, Providers.PosStyleData.Get(PluginEntry.DataModel, cmbNormalButtonStyle.SelectedDataID).ConvertToStyle());
            }

            if (!RecordIdentifier.IsEmptyOrNull(cmbOtherButtonStyle.SelectedDataID))
            {
                TouchButtonTypeHelper.SetStyleOverride(TouchButtonType.None, Providers.PosStyleData.Get(PluginEntry.DataModel, cmbOtherButtonStyle.SelectedDataID).ConvertToStyle());
            }

            if (chkOverridePOSBorderColor.Checked)
            {
                LSOne.Utilities.ColorPalette.ColorPalette.SetPOSControlBorderColorOverride(cwPOSControlBorderColor.SelectedColor);
            }

            if (chkOverridePOSSelectedRowColor.Checked)
            {
                LSOne.Utilities.ColorPalette.ColorPalette.SetPOSSelectedRowColorOverride(cwPOSSelectedRowColor.SelectedColor);
            }
        }
    }
}
