using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.BarCodes.Datalayer;
using LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities;
using LSOne.ViewPlugins.BarCodes.Properties;

namespace LSOne.ViewPlugins.BarCodes.Views
{
    public partial class BarCodeMaskSetupView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public BarCodeMaskSetupView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public BarCodeMaskSetupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvMasks.ContextMenuStrip = new ContextMenuStrip();
            lvMasks.ContextMenuStrip.Opening += new CancelEventHandler(lvMasks_Opening);

            lvMasks.SmallImageList = PluginEntry.Framework.GetImageList();


            btnsContextButtons.AddButtonEnabled = !ReadOnly;

            LoadItems(0, false, true,selectedID);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("BarCodeMasks", RecordIdentifier.Empty, Properties.Resources.BarCodeMasks, false));
            contexts.Add(new AuditDescriptor("BarCodeMaskSegments", RecordIdentifier.Empty, Properties.Resources.BarCodeMaskSegments, false));
            
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.BarCodeMaskSetup;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.BarCodeMaskSetup;
            //HeaderIcon = Properties.Resources.BarcodeImage;
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "BarCodeMaskSetup")
            {
                LoadItems(lvMasks.SortColumn, lvMasks.SortedBackwards, true, selectedID);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), new ContextbarClickEventHandler(btnAdd_Click)), 10);
            }
        }

        private void lvMasks_SizeChanged(object sender, EventArgs e)
        {
            lvMasks.Columns[1].Width = -2;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.BarCodeMaskDialog(((BarcodeMask)lvMasks.SelectedItems[0].Tag).ID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvMasks.SortColumn, lvMasks.SortedBackwards, true, selectedID);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.BarCodeMaskDialog dlg = new Dialogs.BarCodeMaskDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvMasks.SortColumn, lvMasks.SortedBackwards, true, selectedID);
            }
        }

        private void lvMasks_DoubleClick(object sender, EventArgs e)
        {
            if (lvMasks.SelectedItems.Count != 0)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvMasks_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvMasks.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            item.Image = ContextButtons.GetRemoveButtonImage();
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ColorGroupList", lvMasks.ContextMenuStrip, lvMasks);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (Providers.BarcodeMaskData.BarCodeMaskInUse(PluginEntry.DataModel, ((BarcodeMask)lvMasks.SelectedItems[0].Tag).Mask))
            {
                MessageDialog.Show(Resources.BarcodeMaskInUse);
                return;
            }
            if (QuestionDialog.Show(Properties.Resources.DeleteBarCodeMaskQuestion, Properties.Resources.DeleteBarCodeMask) == DialogResult.Yes)
            {
                DataProviderFactory.Instance.Get<IBarcodeMaskData, BarcodeMask>().Delete(PluginEntry.DataModel, ((BarcodeMask)lvMasks.SelectedItems[0].Tag).ID);

                LoadItems(lvMasks.SortColumn, lvMasks.SortedBackwards, true,selectedID);
            }
        }

        private void LoadLines()
        {
            List<BarcodeMaskSegment> segments;
            ListViewItem listItem;

            if (lvMasks.SelectedItems.Count == 0)
            {
                return;
            }

            lvSegments.Items.Clear();

            segments = Providers.BarcodeMaskSegmentData.Get(PluginEntry.DataModel,((BarcodeMask)lvMasks.SelectedItems[0].Tag).ID);

            foreach (BarcodeMaskSegment segment in segments)
            {
                listItem = new ListViewItem((string)segment.SegmentNum.ToString());

                listItem.SubItems.Add(PluginOperations.TypeText(segment.Type));
                listItem.SubItems.Add(((int)segment.Length).ToString());
                listItem.SubItems.Add(segment.SegmentChar);
                listItem.SubItems.Add(segment.Decimals.ToString());

                listItem.Tag = segment.UniqueID;
                listItem.ImageIndex = -1;

                lvSegments.Add(listItem);
            }

            lvSegments.BestFitColumns();
        }

        private void LoadItems(int sortBy, bool backwards,bool doBestFit,RecordIdentifier idToSelect)
        {
            lvMasks.Items.Clear();

            var items = DataProviderFactory.Instance.Get<IBarcodeMaskData, BarcodeMask>().GetBarCodeMasks(PluginEntry.DataModel, sortBy,backwards);

            foreach (BarcodeMask item in items)
            {
                var listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);
                listItem.SubItems.Add(MaskTypeText(item.InternalType));
                listItem.SubItems.Add(PluginOperations.SymbologyText(item.Type));
                listItem.SubItems.Add(item.Prefix);
                listItem.SubItems.Add(item.Mask);
                listItem.SubItems.Add(item.Mask.Length.ToString());
            
                listItem.Tag = item;
                listItem.ImageIndex = -1;

                lvMasks.Add(listItem);

                if (idToSelect == ((BarcodeMask)listItem.Tag).ID)
                {
                    listItem.Selected = true;
                }
            }

            lvMasks.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvMasks.SortColumn = sortBy;

            lvMasks_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvMasks.BestFitColumns();
            }
        }

        private void lvMasks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvMasks.SortColumn == e.Column)
            {
                lvMasks.SortedBackwards = !lvMasks.SortedBackwards;
            }
            else
            {
                if (lvMasks.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvMasks.Columns[lvMasks.SortColumn].ImageIndex = 2;
                }
                lvMasks.SortedBackwards = false;
            }

            LoadItems(e.Column, lvMasks.SortedBackwards,false,selectedID);
        }

        private void lvMasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvMasks.SelectedItems.Count > 0) ? ((BarcodeMask)lvMasks.SelectedItems[0].Tag).ID : "";
            btnsContextButtons.EditButtonEnabled = (lvMasks.SelectedItems.Count != 0);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;

            if (lvMasks.SelectedItems.Count > 0)
            {
                if (lblGroupHeader.Visible == false)
                {
                    lblGroupHeader.Visible = true;
                    lvSegments.Visible = true;

                    lblNoSelection.Visible = false;
                }

                LoadLines();
            }
            else if (lblGroupHeader.Visible == true)
            {
                lblGroupHeader.Visible = false;
                lvSegments.Visible = false;

                lblNoSelection.Visible = true;
            }
        }

        protected override void OnClose()
        {
            lvMasks.SmallImageList = null;
            lvSegments.SmallImageList = null;

            base.OnClose();
        }

        private string MaskTypeText(BarcodeInternalType maskType)
        {
            switch (maskType)
            {
                case BarcodeInternalType.None:
                    return Resources.None;

                case BarcodeInternalType.Item:
                    return Resources.Item;

                case BarcodeInternalType.Customer:
                    return Resources.Customer;

                case BarcodeInternalType.Employee:
                    return Resources.Employee;

                case BarcodeInternalType.Coupon:
                    return Resources.Coupon;

                case BarcodeInternalType.DataEntry:
                    return Resources.DataEntry;

                case BarcodeInternalType.SalesPerson:
                    return Resources.SalesPerson;

                case BarcodeInternalType.Pharmacy:
                    return Resources.Pharmacy;

                case BarcodeInternalType.Customized:
                    return Resources.Customized;

                case BarcodeInternalType.ReceiptBarcode:
                    return Resources.ReceiptBarcode;

                case BarcodeInternalType.Discount:
                    return Resources.Discount;

                case BarcodeInternalType.CreditMemo:
                    return Resources.CreditMemo;

                case BarcodeInternalType.GiftCard:
                    return Resources.GiftCard;

                case BarcodeInternalType.QR:
                    return Resources.QR;

                case BarcodeInternalType.Loyalty:
                    return Resources.Loyalty;

                default:
                    return Resources.Unknown;
            }
        }

    }
}
