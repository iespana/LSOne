using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class EmailReceiptPage : UserControl, ITabView
    {
        private SiteServiceProfile profile;
        private List<DataEntity> options;
        
        private RecordIdentifier selectedShortHandID;
        private List<ShorthandItem> shorthandItems;

        public EmailReceiptPage()
        {
            InitializeComponent();

            selectedShortHandID = RecordIdentifier.Empty;
            shorthandItems = new List<ShorthandItem>();

            options = new List<DataEntity>();
            options.Add(new DataEntity((int)ReceiptEmailOptionsEnum.Never, Properties.Resources.Email_Never));
            options.Add(new DataEntity((int)ReceiptEmailOptionsEnum.Always, Properties.Resources.Email_Always));
            options.Add(new DataEntity((int) ReceiptEmailOptionsEnum.OnlyToCustomers, Properties.Resources.Email_OnlyToCustomers));
            options.Add(new DataEntity((int)ReceiptEmailOptionsEnum.OnRequest, Properties.Resources.Email_OnRequest));

            foreach (DataEntity entity in options)
            {
                cmbEmailOption.Items.Add(entity);
            }

            btnAddEditShortHand.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit);
            btnAddEditShortHand.EditButtonEnabled = false;
            btnAddEditShortHand.RemoveButtonEnabled = false;

            lvShorthand.ContextMenuStrip = new ContextMenuStrip();
            lvShorthand.ContextMenuStrip.Opening += lvShorthand_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new EmailReceiptPage();
        }

        void lvShorthand_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvShorthand.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnAddEditShortHand_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnAddEditShortHand.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAddEditShortHand_AddButtonClicked);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnAddEditShortHand.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnAddEditShortHand_RemoveButtonClicked);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnAddEditShortHand.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ShorthandList", lvShorthand.ContextMenuStrip, lvShorthand);

            e.Cancel = (menu.Items.Count == 0);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;

            chkEmailReceipt.Checked = profile.SendReceiptEmails != ReceiptEmailOptionsEnum.Never;
            cmbEmailOption.SelectedIndex = (int)profile.SendReceiptEmails;
            cmbEmailOption.Enabled = chkEmailReceipt.Checked;
            cmbPrinterConfigurationID.Enabled = chkEmailReceipt.Checked;
            cmbPrinterConfigurationID.SelectedData = Providers.WindowsPrinterConfigurationData.Get(PluginEntry.DataModel, profile.EmailWindowsPrinterConfigurationID) ?? new DataEntity();

            LoadShortHandItems();
        }

        public bool DataIsModified()
        {
            if (cmbEmailOption.SelectedIndex != (int)profile.SendReceiptEmails || cmbPrinterConfigurationID.SelectedDataID != profile.EmailWindowsPrinterConfigurationID) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.SendReceiptEmails = (ReceiptEmailOptionsEnum)cmbEmailOption.SelectedIndex;
            profile.EmailWindowsPrinterConfigurationID = cmbPrinterConfigurationID.SelectedDataID;
            
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            errorProvider1.Clear();

            if ((changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add) && objectName == "ShortHandItem" && changeIdentifier == profile.ID)
            {
                LoadShortHandItems();
            }

            if (changeHint == DataEntityChangeType.Delete && objectName == "ShortHandItem")
            {
                selectedShortHandID = RecordIdentifier.Empty;
                LoadShortHandItems();
                lvShorthand_SelectionChanged(null, EventArgs.Empty);
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void chkEmailReceipt_CheckedChanged(object sender, EventArgs e)
        {
            cmbEmailOption.Enabled = chkEmailReceipt.Checked;
            cmbEmailOption.SelectedIndex = chkEmailReceipt.Checked ? (int)ReceiptEmailOptionsEnum.OnRequest : (int)ReceiptEmailOptionsEnum.Never;
            cmbPrinterConfigurationID.Enabled = chkEmailReceipt.Checked;

            if (!cmbPrinterConfigurationID.Enabled)
            {
                cmbPrinterConfigurationID.SelectedData = new DataEntity();
            }
        }

        private void LoadShortHandItems()
        {
            shorthandItems = Providers.ShortHandItemData.GetList(PluginEntry.DataModel, profile.ID);
            lvShorthand.Rows.Clear();

            Row row;

            foreach (ShorthandItem item in shorthandItems.OrderBy(o => o.Text))
            {
                row = new Row();
                row.AddText(item.Text);
                row.Tag = item.ID;
                lvShorthand.AddRow(row);
                if (selectedShortHandID == (RecordIdentifier)row.Tag)
                {
                    lvShorthand.Selection.Set(lvShorthand.RowCount - 1);
                }
            }

            lvShorthand.AutoSizeColumns();
        }

        private void lvShorthand_SelectionChanged(object sender, EventArgs e)
        {
            selectedShortHandID = (lvShorthand.Selection.Count > 0) ? (RecordIdentifier)lvShorthand.Row(lvShorthand.Selection.FirstSelectedRow).Tag : RecordIdentifier.Empty;
            if (lvShorthand.Selection.Count > 0)
            {
                btnAddEditShortHand.AddButtonEnabled =
                    btnAddEditShortHand.EditButtonEnabled = 
                    btnAddEditShortHand.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit);
            }
        }

        private void btnAddEditShortHand_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.AddShortHandItem(profile.ID);
        }

        private void btnAddEditShortHand_EditButtonClicked(object sender, EventArgs e)
        {
            if (selectedShortHandID != RecordIdentifier.Empty)
            {
                ShorthandItem item = shorthandItems.FirstOrDefault(f => f.ID == selectedShortHandID);
                if (item != null)
                {
                    PluginOperations.EditShortHandItem(item);
                }
            }
        }
        private void btnAddEditShortHand_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (selectedShortHandID != RecordIdentifier.Empty)
            {
                PluginOperations.DeleteShortHandItem(selectedShortHandID);
            }
        }

        private void cmbEmailOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ReceiptEmailOptionsEnum) cmbEmailOption.SelectedIndex == ReceiptEmailOptionsEnum.Never)
            {
                chkEmailReceipt.Checked = false;
            }
        }

        private void cmbPrinterConfigurationID_RequestData(object sender, EventArgs e)
        {
            cmbPrinterConfigurationID.SetData(Providers.WindowsPrinterConfigurationData.GetDataEntityList(PluginEntry.DataModel), null);
        }
    }
}
