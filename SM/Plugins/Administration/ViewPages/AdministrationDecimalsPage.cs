using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Settings;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationDecimalsPage : ContainerControl, ITabViewV2
    {
        RecordIdentifier selectedID;

        public AdministrationDecimalsPage()
        {
            selectedID = "";

            InitializeComponent();

            lvDecimals.SmallImageList = PluginEntry.Framework.GetImageList();
            lvDecimals.SortColumn = 0;

            lvDecimals.ContextMenuStrip = new ContextMenuStrip();
            lvDecimals.ContextMenuStrip.Opening += lvDecimals_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.AdministrationDecimalsPage();
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            LoadItems(1, false, true);
        }

        public bool DataIsModified()
        {
            bool returnValue = false;

           

            return returnValue;
        }

        public bool SaveData()
        {
           
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("DecimalSettings",RecordIdentifier.Empty,Properties.Resources.DecimalFormats,false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            LoadItems(1, false, true);
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvDecimals.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadItems(int sortBy, bool backwards, bool doBestFit)
        {
            ListViewItem listItem;

            lvDecimals.Items.Clear();

            List<DecimalSetting> settings = Providers.DecimalSettingsData.Get(PluginEntry.DataModel, sortBy, backwards);

            foreach (DecimalSetting setting in settings)
            {
                listItem = new ListViewItem((string)setting.ID);
                listItem.SubItems.Add(setting.Text);
                listItem.SubItems.Add(setting.Min.ToString());
                listItem.SubItems.Add(setting.Max.ToString());
                listItem.Tag = setting.ID;
                listItem.ImageIndex = -1;

                lvDecimals.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvDecimals.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvDecimals.SortColumn = sortBy;

            lvDecimals_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvDecimals.BestFitColumns();
            }
        }

        private void lvDecimals_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditValue.Enabled = lvDecimals.SelectedItems.Count > 0;

            selectedID = lvDecimals.SelectedItems.Count > 0 ? (RecordIdentifier)lvDecimals.Tag : "";
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            Dialogs.EditDecimalFormatDialog dlg = new Dialogs.EditDecimalFormatDialog((RecordIdentifier)lvDecimals.SelectedItems[0].Tag);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvDecimals.SortColumn, lvDecimals.SortedBackwards, true);
            }
        }

        private void lvDecimals_DoubleClick(object sender, EventArgs e)
        {
            if (btnEditValue.Enabled)
            {
                btnEditValue_Click(this, EventArgs.Empty);
            }
        }

        private void lvDecimals_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            

            if (lvDecimals.SortColumn == e.Column)
            {
                lvDecimals.SortedBackwards = !lvDecimals.SortedBackwards;
            }
            else
            {
                if (lvDecimals.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvDecimals.Columns[lvDecimals.SortColumn].ImageIndex = 2;
                }
                lvDecimals.SortedBackwards = false;
            }

            LoadItems(e.Column, lvDecimals.SortedBackwards, false);
        }

        void lvDecimals_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvDecimals.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit + "...",
                    100,
                    btnEditValue_Click)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnEditValue.Enabled,
                Default = true
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DecimalSetupList", lvDecimals.ContextMenuStrip, lvDecimals);

            e.Cancel = (menu.Items.Count == 0);
        }
    }
}
