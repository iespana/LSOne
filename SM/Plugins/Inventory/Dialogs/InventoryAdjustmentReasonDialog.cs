using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class InventoryAdjustmentReasonDialog : DialogBase
    {
        RecordIdentifier selectedID;
        bool lockEvents;
        DataEntity item;
        public InventoryAdjustmentReasonDialog()
            : base()
        {
            item = null;
            lockEvents = false;
            selectedID = RecordIdentifier.Empty;
            LastCreatedReason = new DataEntity(RecordIdentifier.Empty, "");

            InitializeComponent();
            lvReasons.ContextMenuStrip = new ContextMenuStrip();
            lvReasons.ContextMenuStrip.Opening += new CancelEventHandler(lvReasons_Opening);
        }

        public DataEntity LastCreatedReason { get; set; }

        public bool DataIsModified { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadList();
        }

        private void LoadList()
        {
            List<ReasonCode> reasons = new List<ReasonCode>();

            lvReasons.ClearRows();

            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {
                reasons = service.GetReasonCodes(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),InventoryJournalTypeEnum.Adjustment, true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            foreach (DataEntity reason in reasons)
            {
                Row row = new Row();
                row.AddText(reason.Text);
                row.Tag = reason;
                lvReasons.AddRow(row);
            }

            lvReasons.AutoSizeColumns();
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
                lvReasons.Rows.RemoveAt(lvReasons.Rows.Count - 1);
            }

            lvReasons.Enabled = true;
            btnsAddEdit.AddButtonEnabled = true;

            lvReasons_SelectionChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            item = new DataEntity();
            DataIsModified = true;
            bool creatingNewReason = false;

            if (selectedID != RecordIdentifier.Empty)
            {
                item.ID = selectedID;
            }
            else
            {
                creatingNewReason = true;
            }

            item.Text = tbDescription.Text;

            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {
                item.ID = service.SaveInventoryAdjustmentReasonReturnID(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), item, true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lvReasons.Row(lvReasons.Selection.FirstSelectedRow)[0].Text = tbDescription.Text;

            selectedID = item.ID;
            lvReasons.Row(lvReasons.Selection.FirstSelectedRow).Tag = item;

            if (creatingNewReason)
            {
                LastCreatedReason = item;
            }

            btnClose.Enabled = true;

            lvReasons.Enabled = true;
            btnsAddEdit.AddButtonEnabled = true;

            lvReasons_SelectionChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;

            RemoveEditMode();
            lvReasons.AutoSizeColumns();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Row row = new Row();
            row.AddText("");
            row.Tag = new DataEntity();
            lvReasons.AddRow(row);
            lvReasons.Selection.Set(lvReasons.RowCount - 1);            

            btnsAddEdit.AddButtonEnabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = false;
            SetEditMode();
        }

        private void SetEditMode()
        {
            btnsAddEdit.AddButtonEnabled = false;
            btnsAddEdit.RemoveButtonEnabled = false;
            btnClose.Enabled = false;
            lvReasons.Enabled = false;
            tbDescription.Focus();
        }

        private void RemoveEditMode()
        {
            btnsAddEdit.AddButtonEnabled = true;
            btnsAddEdit.RemoveButtonEnabled = true;
            btnClose.Enabled = true;
            lvReasons.Enabled = true;
        }

        private void CheckSaveEnabled(object sender,EventArgs args)
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
                btnSave.Enabled = btnCancel.Enabled = tbDescription.Text != item.Text;
            }

            if (btnSave.Enabled && lvReasons.Enabled)
            {
                SetEditMode();
            }
            else if (btnSave.Enabled == false && lvReasons.Enabled == false && selectedID != RecordIdentifier.Empty)
            {
                RemoveEditMode();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RecordIdentifier reasonID = ((DataEntity)lvReasons.Row(lvReasons.Selection.FirstSelectedRow).Tag).ID;
            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {
                if (service.InventoryAdjustmentReasonIsInUse(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), reasonID, true))
                {
                    MessageDialog.Show(Properties.Resources.ReasonInUse);
                    return;
                }
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }


            if (QuestionDialog.Show(Properties.Resources.DeleteReasonQuestion, Properties.Resources.DeleteReason) == DialogResult.Yes)
            {
                try
                {
                    service.DeleteInventoryAdjustmentReason(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), selectedID, true);
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }

                if (LastCreatedReason.ID == reasonID)
                {
                    LastCreatedReason = new DataEntity(RecordIdentifier.Empty, "");
                }

                lvReasons.Rows.RemoveAt(lvReasons.Selection.FirstSelectedRow);
                LoadList();

                lvReasons_SelectionChanged(this, EventArgs.Empty);
                DataIsModified = true;                
            }
        }

        void lvReasons_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvReasons.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));

            item.Enabled = btnsAddEdit.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = (lvReasons.Selection.Count != 0);

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ColorList", lvReasons.ContextMenuStrip, lvReasons);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckSaveEnabled(this, EventArgs.Empty);
        }

        private void InventoryAdjustmentReasonDialog_Load(object sender, EventArgs e)
        {

        }

        private void lvReasons_SelectionChanged(object sender, EventArgs e)
        {
            if (lvReasons.Selection.Count > 0)
            {
                selectedID = ((DataEntity)lvReasons.Row(lvReasons.Selection.FirstSelectedRow).Tag).ID;

                lblName.Visible = true;
                tbDescription.Visible = true;
                btnSave.Visible = btnCancel.Visible = true;

                lblNoSelection.Visible = false;

                lockEvents = true;



                lockEvents = false;

                if (selectedID == null)
                {
                    tbDescription.Focus();

                    btnCancel.Enabled = true;
                    btnSave.Enabled = false;

                    tbDescription.Text = "";

                    SetEditMode();

                    btnsAddEdit.RemoveButtonEnabled = false;
                }
                else
                {
                    item = (DataEntity)lvReasons.Row(lvReasons.Selection.FirstSelectedRow).Tag;

                    tbDescription.Text = item.Text;


                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;

                    btnsAddEdit.RemoveButtonEnabled = true;
                }
            }
            else
            {
                lblName.Visible = false;
                tbDescription.Visible = false;
                btnSave.Visible = btnCancel.Visible = false;

                lblNoSelection.Visible = true;

                btnsAddEdit.RemoveButtonEnabled = false;
            }
        }
    }
}
