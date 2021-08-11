using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController.Controls;
using LSRetail.StoreController.SharedCore;
using LSRetail.StoreController.SharedCore.Enums;
using LSRetail.StoreController.SharedCore.Dialogs;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.DataEntities;

namespace LSRetail.StoreController.BarCodes.Views
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

        public override Utilities.DataTypes.RecordIdentifier ID
        {
	        get 
	        { 
                return Utilities.DataTypes.RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.BarCodeMaskSetup;
            HeaderIcon = Properties.Resources.BarcodeImage;
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

        protected override void OnSetupContextBarItems(LSRetail.StoreController.SharedCore.EventArguments.ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Add, btnsContextButtons.AddButtonImage, new ContextbarClickEventHandler(btnAdd_Click)), 10);
            }
        }

        private void lvMasks_SizeChanged(object sender, EventArgs e)
        {
            lvMasks.Columns[1].Width = -2;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.BarCodeMaskDialog dlg = new Dialogs.BarCodeMaskDialog((RecordIdentifier)lvMasks.SelectedItems[0].Tag);

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
            ExtendedMenuitem item;
            ContextMenuStrip menu;

            menu = lvMasks.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuitem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));

            item.Image = btnsContextButtons.EditButtonImage;
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            item.Image = btnsContextButtons.AddButtonImage;
            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            item.Image = btnsContextButtons.RemoveButtonImage;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ColorGroupList", lvMasks.ContextMenuStrip, lvMasks);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            if (LSRetail.StoreController.SharedCore.Dialogs.QuestionDialog.Show(Properties.Resources.DeleteBarCodeMaskQuestion, Properties.Resources.DeleteBarCodeMask) == DialogResult.Yes)
            {
                Datalayer.BarCodeMaskData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvMasks.SelectedItems[0].Tag);

                LoadItems(lvMasks.SortColumn, lvMasks.SortedBackwards, true,selectedID);

                
            }
        }

        private void LoadLines()
        {
            List<Datalayer.DataEntities.BarCodeMaskSegment> segments;
            ListViewItem listItem;

            if (lvMasks.SelectedItems.Count == 0)
            {
                return;
            }

            lvSegments.Items.Clear();

            segments = Datalayer.BarCodeMaskSegmentData.Get(PluginEntry.DataModel,(RecordIdentifier)lvMasks.SelectedItems[0].Tag);

            foreach (Datalayer.DataEntities.BarCodeMaskSegment segment in segments)
            {
                listItem = new ListViewItem((string)segment.SegmentNum.ToString());
 
                listItem.SubItems.Add(segment.TypeText);
                listItem.SubItems.Add(((int)segment.Length).ToString());
                listItem.SubItems.Add(segment.Character);
                listItem.SubItems.Add(segment.Decimals.ToString());

                listItem.Tag = segment.ID;
                listItem.ImageIndex = -1;

                lvSegments.Add(listItem);
            }

            lvSegments.BestFitColumns();
        }

        private void LoadItems(int sortBy, bool backwards,bool doBestFit,RecordIdentifier idToSelect)
        {
            List<Datalayer.DataEntities.BarCodeMask> items;
            ListViewItem listItem;

            lvMasks.Items.Clear();


            items = Datalayer.BarCodeMaskData.GetBarCodeMasks(PluginEntry.DataModel, sortBy,backwards);

            foreach (Datalayer.DataEntities.BarCodeMask item in items)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);
                listItem.SubItems.Add(item.MaskTypeText);
                listItem.SubItems.Add(item.SymbologyText);
                listItem.SubItems.Add(item.Prefix);
                listItem.SubItems.Add(item.Mask);
                listItem.SubItems.Add(item.Mask.Length.ToString());
            
                listItem.Tag = item.ID;
                listItem.ImageIndex = -1;

                lvMasks.Add(listItem);

                if (idToSelect == (RecordIdentifier)listItem.Tag)
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
            selectedID = (lvMasks.SelectedItems.Count > 0) ? (RecordIdentifier)lvMasks.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = (lvMasks.SelectedItems.Count != 0);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
            ///
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
    }
}
