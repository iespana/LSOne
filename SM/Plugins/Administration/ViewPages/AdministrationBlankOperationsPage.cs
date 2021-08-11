using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Operations;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Administration.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationBlankOperationsPage : UserControl, ITabViewV2
    {
        public AdministrationBlankOperationsPage()
        {
            InitializeComponent();

            DoubleBuffered = true;

            lvBlankOperations.SmallImageList = PluginEntry.Framework.GetImageList();
            lvBlankOperations.ContextMenuStrip = new ContextMenuStrip();
            lvBlankOperations.ContextMenuStrip.Opening += lvBlankOperations_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings);

            lvBlankOperations_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvBlankOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = lvBlankOperations.SelectedItems.Count > 0 && btnsContextButtons.AddButtonEnabled;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new AdministrationBlankOperationsPage();
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            LoadItems();
        }

        public void LoadItems()
        {
            ListViewItem item;                       
            
            lvBlankOperations.Items.Clear();            
            List<BlankOperation> blankOperations = Providers.BlankOperationData.GetBlankOperations(PluginEntry.DataModel);

            foreach (BlankOperation bo in blankOperations)
            {
                item = new ListViewItem((string)bo.ID);
                item.SubItems.Add((bo.OperationParameter == null || bo.OperationParameter == "") ? Properties.Resources.None : bo.OperationParameter);
                item.SubItems.Add(bo.OperationDescription);
                item.SubItems.Add((bo.CreatedOnPOS) ? Properties.Resources.CreatedOnPOS : Properties.Resources.CreatedOnSiteManager);
                lvBlankOperations.Add(item);
            }

            lvBlankOperations.BestFitColumns();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            LoadItems();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
           
        }

        public void SaveUserInterface()
        {
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvBlankOperations.SmallImageList = null;
        }

        #endregion

        private void lvBlankOperations_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvBlankOperations.SelectedIndices.Count > 0)
            {
                ListViewItem item = lvBlankOperations.Items[lvBlankOperations.SelectedIndices[0]];
                BlankOperation bo = Providers.BlankOperationData.Get(PluginEntry.DataModel, item.Text);

                NewBlankOperationDialog nbo = new NewBlankOperationDialog(bo); //, false);

                if (nbo.ShowDialog() == DialogResult.OK)
                {
                    LoadItems();
                }
            }
            else
                MessageDialog.Show("");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NewBlankOperationDialog nbo = new NewBlankOperationDialog();

            if (nbo.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvBlankOperations.SelectedIndices.Count > 0)
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteBlankOperationQuestion, Properties.Resources.BlankOperations) == DialogResult.Yes)
                {
                    ListViewItem item = lvBlankOperations.Items[lvBlankOperations.SelectedIndices[0]];
                    Providers.BlankOperationData.Delete(PluginEntry.DataModel, item.Text);
                    LoadItems();
                }
            }
            else
            {
                MessageDialog.Show("");
            }
        }

        void lvBlankOperations_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvBlankOperations.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit + "...",
                    100,
                    btnEdit_Click);                    
             
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;

            menu.Items.Add(item);

            //*************************
            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnAdd_Click);

            item.Enabled = btnsContextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            //**************************
           
            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);

            //*************************

            PluginEntry.Framework.ContextMenuNotify("BlankOperationSetupList", lvBlankOperations.ContextMenuStrip, lvBlankOperations);
            e.Cancel = (menu.Items.Count == 0);
        }

        
    }
}
