using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class StyleProfileOrderPage : UserControl, ITabView
    {
        LSOneKitchenDisplayStyleProfile styleProfile;
        RecordIdentifier selectedTimeStyleId;

        public StyleProfileOrderPage()
        {
            InitializeComponent();
            cmbOrder.Tag = PluginEntry.DefaultChitGuid;
            cmbOrderPane.Tag = PluginEntry.OrderPaneGuid;
            cmbMarkedForAlert.Tag = PluginEntry.OrderMarkedAlertGuid;

            lvTimeStyles.ContextMenuStrip = new ContextMenuStrip();
            lvTimeStyles.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StyleProfileOrderPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            styleProfile = (LSOneKitchenDisplayStyleProfile)internalContext;
            cmbOrder.SelectedData = styleProfile.OrderStyle;
            cmbOrderPane.SelectedData = styleProfile.OrderPaneStyle;
            cmbMarkedForAlert.SelectedData = styleProfile.AlertStyle;

            if(styleProfile.OrderStyle.ID != null)
            {
                cwSeparatorColor.SelectedColor = Color.FromArgb(styleProfile.OrderStyle.ForeColor);
            }
            else
            {
                cwSeparatorColor.Enabled = false;
            }

            LoadTimeStyleLines();

            btnsOrder.SetBuddyControl(cmbOrder);
            btnsOrderPane.SetBuddyControl(cmbOrderPane);
            btnsMarkedForAlert.SetBuddyControl(cmbMarkedForAlert);

            cmbDoneChitOverlay.Items.Add(new DataEntity((int)DoneChitsOverlayEnum.BezierCurve,
                styleProfile.DoneChitOverlayStyleText(DoneChitsOverlayEnum.BezierCurve)));
            cmbDoneChitOverlay.Items.Add(new DataEntity((int)DoneChitsOverlayEnum.SolidGreen,
                styleProfile.DoneChitOverlayStyleText(DoneChitsOverlayEnum.SolidGreen)));
            cmbDoneChitOverlay.Text = styleProfile.DoneChitOverlayStyleText(styleProfile.DoneChitOverlayStyle);
        }

        public bool DataIsModified()
        {
            if (cmbOrder.SelectedData.ID != styleProfile.OrderStyle.ID) return true;
            if (cmbOrderPane.SelectedData.ID != styleProfile.OrderPaneStyle.ID) return true;
            if (cmbMarkedForAlert.SelectedData.ID != styleProfile.AlertStyle.ID) return true;
            if (((DataEntity)cmbDoneChitOverlay.SelectedItem).ID != (int)styleProfile.DoneChitOverlayStyle) return true;

            return false;
        }

        public bool SaveData()
        {
            styleProfile.OrderStyle.ID = cmbOrder.SelectedData.ID;
            styleProfile.OrderPaneStyle.ID = cmbOrderPane.SelectedData.ID;
            styleProfile.AlertStyle.ID = cmbMarkedForAlert.SelectedData.ID;
            styleProfile.DoneChitOverlayStyle = (DoneChitsOverlayEnum)(int)((DataEntity)cmbDoneChitOverlay.SelectedItem).ID;
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

        private void cmb_RequestData(object sender, EventArgs e)
        {
            var comboBox = (DualDataComboBox) sender;
            var styles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME");
            comboBox.SetData(styles, null);
        }

        private void cmb_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }

        private void cmbOrder_SelectedDataChanged(object sender, EventArgs e)
        {
            if(cmbOrder.SelectedData.ID == string.Empty)
            {
                cwSeparatorColor.Enabled = false;
                cwSeparatorColor.SelectedColor = Color.Black;
            }
            else
            {
                cwSeparatorColor.Enabled = true;
                cwSeparatorColor.SelectedColor = Color.FromArgb(((PosStyle)cmbOrder.SelectedData).ForeColor);
            }
        }

        private void cwSeparatorColor_SelectedColorChanged(object sender, EventArgs e)
        {
            int selectedColor = cwSeparatorColor.SelectedColor.ToArgb();
            PosStyle selectedOrderStyle = (PosStyle)cmbOrder.SelectedData;
            selectedOrderStyle.ForeColor = selectedColor;

            Providers.PosStyleData.Save(PluginEntry.DataModel, selectedOrderStyle);
        }

        private void LoadTimeStyleLines()
        {
            lvTimeStyles.ClearRows();

            List<KitchenDisplayTimeStyle> kitchenDisplayTimeStyles = Providers.KitchenDisplayTimeStyleData.GetList(PluginEntry.DataModel, styleProfile.ID);

            foreach (KitchenDisplayTimeStyle timeStyle in kitchenDisplayTimeStyles)
            {
                var row = new Row();
                row.AddText(timeStyle.SecondsPassed.ToString());
                row.AddText(timeStyle.UiStyle.Text);

                row.Tag = timeStyle.ID;
               
                lvTimeStyles.AddRow(row);

                if (selectedTimeStyleId == timeStyle.ID)
                {
                    lvTimeStyles.Selection.Set(lvTimeStyles.RowCount - 1);
                }
            }

            lvTimeStyles_SelectedIndexChanged(this, EventArgs.Empty);
            lvTimeStyles.AutoSizeColumns();
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.TimeStyleDialog(styleProfile.ID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadTimeStyleLines();
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            selectedTimeStyleId = (RecordIdentifier)lvTimeStyles.Selection[0].Tag;
            var dlg = new Dialogs.TimeStyleDialog(styleProfile.ID, selectedTimeStyleId);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadTimeStyleLines();
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            selectedTimeStyleId = (RecordIdentifier)lvTimeStyles.Selection[0].Tag;
            Providers.KitchenDisplayTimeStyleData.Delete(PluginEntry.DataModel, selectedTimeStyleId);
            LoadTimeStyleLines();
        }

        private void lvTimeStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled =
                lvTimeStyles.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        private void lvTimeStyles_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(sender, EventArgs.Empty);
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTimeStyles.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                btnsEditAddRemove_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemove.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                btnsEditAddRemove_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemove.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("KdsTimeStyles", lvTimeStyles.ContextMenuStrip, lvTimeStyles);

            e.Cancel = (menu.Items.Count == 0);
        }
    }
}


