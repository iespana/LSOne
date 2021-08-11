using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class DiningTableLayoutScreensDialog : DialogBase
    {
        private RecordIdentifier selectedID = "";
        private RecordIdentifier diningTableLayoutID;
        bool lockEvents;
        private DiningTableLayoutScreen diningTableLayoutScreen;

        public DiningTableLayoutScreensDialog(RecordIdentifier diningTableLayoutID, RecordIdentifier screenNo)
            : this(diningTableLayoutID)
        {
            selectedID = screenNo;
        }

        public DiningTableLayoutScreensDialog(RecordIdentifier diningTableLayoutID)
        {
            InitializeComponent();

            diningTableLayoutScreen = null;
            lockEvents = false;
            this.diningTableLayoutID = diningTableLayoutID;

            lvScreens.ContextMenuStrip = new ContextMenuStrip();
            lvScreens.ContextMenuStrip.Opening += new CancelEventHandler(lvScreens_Opening);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            lvScreens.SmallImageList = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadList();
        }

        private void LoadList()
        {
            List<DiningTableLayoutScreen> screens;
            ListViewItem item;

            lvScreens.Items.Clear();

            screens = Providers.DiningTableLayoutScreenData.GetList(PluginEntry.DataModel, diningTableLayoutID[0], diningTableLayoutID[1], diningTableLayoutID[2], diningTableLayoutID[3]);

            foreach (DiningTableLayoutScreen screen in screens)
            {
                item = new ListViewItem(screen.ScreenNo.ToString());
                item.SubItems.Add(screen.Text);
                item.Tag = screen.ID;                

                lvScreens.Add(item);

                if (selectedID == screen.ID)
                {
                    item.Selected = true;
                }
            }

            lvScreens.BestFitColumns();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnClose.Enabled = true;

            if (selectedID == RecordIdentifier.Empty)
            {
                lvScreens.Items.RemoveAt(lvScreens.Items.Count - 1);
            }

            lvScreens.Enabled = true;
            btnsAddRemove.AddButtonEnabled= true;

            lvScreens_SelectedIndexChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if we are creating a new screen
            if (selectedID == RecordIdentifier.Empty)
            {
                RecordIdentifier screenId = new RecordIdentifier(diningTableLayoutID[0],
                                            new RecordIdentifier(diningTableLayoutID[1],
                                            new RecordIdentifier(diningTableLayoutID[2],
                                            new RecordIdentifier(diningTableLayoutID[3], new RecordIdentifier((int)ntbScreenNo.Value)))));

                if (Providers.DiningTableLayoutScreenData.Exists(PluginEntry.DataModel, screenId))
                {
                    errorProvider1.SetError(ntbScreenNo, Properties.Resources.DiningTableLayoutScreenExists);
                    return;
                }
            }

            diningTableLayoutScreen = new DiningTableLayoutScreen();
            diningTableLayoutScreen.RestaurantID = diningTableLayoutID[0];
            diningTableLayoutScreen.Sequence = diningTableLayoutID[1];
            diningTableLayoutScreen.SalesType = diningTableLayoutID[2];
            diningTableLayoutScreen.LayoutID = diningTableLayoutID[3];
            diningTableLayoutScreen.ScreenNo = (int)ntbScreenNo.Value;
            diningTableLayoutScreen.Text = tbDescription.Text;
   
            Providers.DiningTableLayoutScreenData.Save(PluginEntry.DataModel, diningTableLayoutScreen);

            lvScreens.SelectedItems[0].Text = ntbScreenNo.Value.ToString();
            lvScreens.SelectedItems[0].SubItems[1].Text = tbDescription.Text;

            selectedID = diningTableLayoutScreen.ID;
            lvScreens.SelectedItems[0].Tag = selectedID;

            btnClose.Enabled = true;

            lvScreens.Enabled = true;
            btnsAddRemove.AddButtonEnabled = true;

            lvScreens_SelectedIndexChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;

            RemoveEditMode();
        }

        private void lvScreens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvScreens.SelectedItems.Count > 0)
            {
                selectedID = (RecordIdentifier)lvScreens.SelectedItems[0].Tag;

                lblNumber.Visible = lblDescription.Visible = true;
                ntbScreenNo.Visible = tbDescription.Visible = true;
                
                btnSave.Visible = btnCancel.Visible = true;

                lblNoSelection.Visible = false;

                lockEvents = true;

                if (selectedID == RecordIdentifier.Empty)
                {
                    ntbScreenNo.Enabled = true;
                    ntbScreenNo.Focus();

                    btnCancel.Enabled = true;
                    btnSave.Enabled = false;

                    ntbScreenNo.Text = "";
                    tbDescription.Text = "";

                    SetEditMode();
                    
                    btnsAddRemove.RemoveButtonEnabled = false;
                }
                else
                {
                    diningTableLayoutScreen = Providers.DiningTableLayoutScreenData.Get(PluginEntry.DataModel, selectedID);

                    //tbID.Text = (string)restaurantDiningTable.ID;
                    ntbScreenNo.Value = (int)diningTableLayoutScreen.ScreenNo;
                    tbDescription.Text = diningTableLayoutScreen.Text;

                    ntbScreenNo.Enabled = false;

                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    
                    btnsAddRemove.RemoveButtonEnabled = true;
                }

                lockEvents = false;
            }
            else
            {
                lblNumber.Visible = lblDescription.Visible = false;
                ntbScreenNo.Visible = tbDescription.Visible = false;
                btnSave.Visible = btnCancel.Visible = false;

                lblNoSelection.Visible = true;

                btnsAddRemove.RemoveButtonEnabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // We do this check in case if the event originates from escape key
            if (!btnClose.Enabled)
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnsAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem("");
            item.SubItems.Add("");
            item.Tag = RecordIdentifier.Empty;

            lvScreens.Add(item);
            item.Selected = true;
            lvScreens.EnsureVisible(lvScreens.Items.Count - 1);

            btnCancel.Enabled = true;
        }

        private void SetEditMode()
        {            
            btnsAddRemove.AddButtonEnabled = false;            
            btnsAddRemove.RemoveButtonEnabled = false;
            btnClose.Enabled = false;

            lvScreens.Enabled = false;
        }

        private void RemoveEditMode()
        {
            btnsAddRemove.AddButtonEnabled = true;
            btnsAddRemove.RemoveButtonEnabled = true;
            btnClose.Enabled = true;

            lvScreens.Enabled = true;
        }

        private void CheckSaveEnabled(object sender, EventArgs args)
        {
            if (lockEvents) return;

            if (selectedID == RecordIdentifier.Empty)
            {
                // Creating new item
                btnSave.Enabled = ntbScreenNo.Text.Length > 0 && tbDescription.Text.Length > 0;
                btnCancel.Enabled = true;
            }
            else
            {
                // Editing existing
                btnSave.Enabled = btnCancel.Enabled = tbDescription.Text != diningTableLayoutScreen.Text;
            }

            if (btnSave.Enabled && lvScreens.Enabled)
            {
                SetEditMode();
            }
            else if (btnSave.Enabled == false && lvScreens.Enabled == false && selectedID != RecordIdentifier.Empty)
            {
                RemoveEditMode();
            }
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier screenID = (RecordIdentifier)lvScreens.SelectedItems[0].Tag;

            if (Providers.DiningTableLayoutScreenData.ScreenIsInUse(PluginEntry.DataModel, screenID))
            {
                MessageDialog.Show(Properties.Resources.DiningTableLayoutSreenInUse);                
                return;
            }

            if (QuestionDialog.Show(
                Properties.Resources.DeleteDiningTableLayoutScreenQuestion,
                Properties.Resources.DeleteDiningTableLayoutScreen) == DialogResult.Yes)
            {
                
                Providers.DiningTableLayoutScreenData.Delete(PluginEntry.DataModel, screenID);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "DiningTableLayoutScreen", screenID, null);

                lvScreens.Items.Remove(lvScreens.SelectedItems[0]);

                lvScreens_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        void lvScreens_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvScreens.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnsAddRemove_AddButtonClicked));

            item.Enabled = btnsAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();

            item.Enabled = (lvScreens.SelectedItems.Count != 0);

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DiningTableLayoutScreen", lvScreens.ContextMenuStrip, lvScreens);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckSaveEnabled(this, EventArgs.Empty);
        }
        
    }
}
