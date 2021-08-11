using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class RemoteHostsDialog : DialogBase
    {
        RecordIdentifier selectedID;
        bool lockEvents;
        RemoteHost remoteHost;

        public RemoteHostsDialog()
        {
            InitializeComponent();

            remoteHost = null;
            lockEvents = false;
            selectedID = RecordIdentifier.Empty;

            lvRemoteHosts.ContextMenuStrip = new ContextMenuStrip();
            lvRemoteHosts.ContextMenuStrip.Opening += new CancelEventHandler(lvRemoteHosts_Opening);

            btnsAddRemove.Enabled = tbDescription.Enabled = tbAddress.Enabled = ntbPort.Enabled = btnSave.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManageRemoteHosts);
        }

        public RemoteHostsDialog(RecordIdentifier remoteHostId)
            : this()
        {
            selectedID = remoteHostId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadList();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            lvRemoteHosts.SmallImageList = null;
        }

        private void LoadList()
        {
            List<RemoteHost> remoteHosts;
            ListViewItem item;

            lvRemoteHosts.Items.Clear();

            remoteHosts = Providers.RemoteHostData.GetList(PluginEntry.DataModel);

            foreach (RemoteHost host in remoteHosts)
            {
                item = new ListViewItem((string)host.ID);
                item.SubItems.Add(host.Text);
                item.Tag = host.ID;

                if (host.ID == selectedID)
                {
                    item.Selected = true;
                }

                lvRemoteHosts.Add(item);
            }

            lvRemoteHosts.BestFitColumns();
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
                lvRemoteHosts.Items.RemoveAt(lvRemoteHosts.Items.Count - 1);
            }

            lvRemoteHosts.Enabled = true;
            btnsAddRemove.AddButtonEnabled= true;

            lvRemoteHosts_SelectedIndexChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            remoteHost = new RemoteHost();

            if (selectedID != RecordIdentifier.Empty)
            {
                remoteHost.ID = selectedID;
            }

            remoteHost.Text = tbDescription.Text;
            remoteHost.Address = tbAddress.Text;
            remoteHost.Port = (int)ntbPort.Value;

            Providers.RemoteHostData.Save(PluginEntry.DataModel, remoteHost);

            lvRemoteHosts.SelectedItems[0].Text = (string)remoteHost.ID;
            lvRemoteHosts.SelectedItems[0].SubItems[1].Text = tbDescription.Text;

            selectedID = remoteHost.ID;
            lvRemoteHosts.SelectedItems[0].Tag = selectedID;
            lvRemoteHosts.SelectedItems[0].Selected = false;

            btnClose.Enabled = true;

            lvRemoteHosts.Enabled = true;            
            btnsAddRemove.AddButtonEnabled = true;

            lvRemoteHosts_SelectedIndexChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            tbDescription.Text = "";
            tbAddress.Text = "";
            ntbPort.Text = "";            

            RemoveEditMode();
        }

        private void lvRemoteHosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRemoteHosts.SelectedItems.Count > 0)
            {
                selectedID = (RecordIdentifier)lvRemoteHosts.SelectedItems[0].Tag;

                lblDescription.Visible = lblAddress.Visible = lblPort.Visible = true;
                tbDescription.Visible =  tbAddress.Visible = ntbPort.Visible = true;
                
                btnSave.Visible = btnCancel.Visible = true;

                lblNoSelection.Visible = false;

                lockEvents = true;

                if (selectedID == RecordIdentifier.Empty)
                {
                    tbDescription.Focus();

                    btnCancel.Enabled = true;
                    btnSave.Enabled = false;

                    tbDescription.Text = "";
                    tbAddress.Text = "";
                    ntbPort.Value = 0;

                    SetEditMode();
                    
                    btnsAddRemove.RemoveButtonEnabled = false;
                }
                else
                {
                    remoteHost = Providers.RemoteHostData.Get(PluginEntry.DataModel, selectedID);

                    tbDescription.Text = remoteHost.Text;
                    tbAddress.Text = remoteHost.Address;
                    ntbPort.Value = remoteHost.Port;

                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    
                    btnsAddRemove.RemoveButtonEnabled = true;
                }

                lockEvents = false;
            }
            else
            {
                lblDescription.Visible = lblAddress.Visible = lblPort.Visible = false;
                tbDescription.Visible = tbAddress.Visible = ntbPort.Visible = false;
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

            lvRemoteHosts.Add(item);
            item.Selected = true;
            lvRemoteHosts.EnsureVisible(lvRemoteHosts.Items.Count - 1);

            btnCancel.Enabled = true;
        }

        private void SetEditMode()
        {            
            btnsAddRemove.AddButtonEnabled = false;            
            btnsAddRemove.RemoveButtonEnabled = false;
            btnClose.Enabled = false;

            lvRemoteHosts.Enabled = false;
        }

        private void RemoveEditMode()
        {
            btnsAddRemove.AddButtonEnabled = true;
            btnsAddRemove.RemoveButtonEnabled = true;
            btnClose.Enabled = true;

            lvRemoteHosts.Enabled = true;
        }

        private void CheckSaveEnabled(object sender, EventArgs args)
        {
            if (lockEvents) return;

            if (selectedID == RecordIdentifier.Empty)
            {
                // Creating new item
                btnSave.Enabled = tbDescription.Text.Length > 0;
                btnCancel.Enabled = true;
            }
            else
            {
                // Editing existing
                btnSave.Enabled = btnCancel.Enabled = (tbDescription.Text.Length > 0) && (tbDescription.Text != remoteHost.Text || tbAddress.Text != remoteHost.Address || ntbPort.Value != (double)remoteHost.Port);
            }

            if (btnSave.Enabled && lvRemoteHosts.Enabled)
            {
                SetEditMode();
            }
            else if (btnSave.Enabled == false && lvRemoteHosts.Enabled == false && selectedID != RecordIdentifier.Empty)
            {
                RemoveEditMode();
            }
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier remoteHostID = (RecordIdentifier)lvRemoteHosts.SelectedItems[0].Tag;

            if (QuestionDialog.Show(
                Properties.Resources.DeleteRemoteHostQuestion,
                Properties.Resources.DeleteRemoteHost) == DialogResult.Yes)
            {

                Providers.RemoteHostData.Delete(PluginEntry.DataModel, remoteHostID);

                lvRemoteHosts.Items.Remove(lvRemoteHosts.SelectedItems[0]);

                lvRemoteHosts_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        void lvRemoteHosts_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvRemoteHosts.ContextMenuStrip;

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

            item.Enabled = (lvRemoteHosts.SelectedItems.Count != 0);

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RemoteHostList", lvRemoteHosts.ContextMenuStrip, lvRemoteHosts);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckSaveEnabled(this, EventArgs.Empty);
        }
        
    }
}
