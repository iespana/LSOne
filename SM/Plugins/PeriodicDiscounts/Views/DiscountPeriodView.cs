using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.PeriodicDiscounts.Dialogs;
using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    public partial class DiscountPeriodView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public DiscountPeriodView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public DiscountPeriodView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("DiscountPeriods", RecordIdentifier.Empty, Properties.Resources.ValidationPeriods, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ValidationPeriods;
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
            

            if (isRevert)
            {
                
            }

            HeaderText = Properties.Resources.ValidationPeriods;
           // HeaderIcon = Properties.Resources.PriceTag16;

            LoadItems();

            
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ValidationPeriodDialog dlg = new ValidationPeriodDialog();
            dlg.ShowDialog();
            LoadItems();
            
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //switch (objectName)
            //{
            //    case "DimensionGroup":
            //        LoadRecords(lvGroups.SortColumn, lvGroups.SortedBackwards);
            //        break;
            //}
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvItems.SelectedItems.Count > 0) ? (RecordIdentifier)lvItems.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = (lvItems.SelectedItems.Count != 0 && PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts));
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ValidationPeriodDialog dlg = new ValidationPeriodDialog((RecordIdentifier)lvItems.SelectedItems[0].Tag);
            dlg.ShowDialog();
            LoadItems();
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DiscountPeriods", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteDiscountPeriodQuestion,
                Properties.Resources.DeleteDiscountPeriod) == DialogResult.Yes)
            {
                Providers.DiscountPeriodData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvItems.SelectedItems[0].Tag);

                LoadItems();
            }

        }

        private void LoadItems()
        {
            List<DiscountPeriod> discounts;
            ListViewItem item;

            lvItems.Items.Clear();

            string[] columns = new string[] { "ID", "DESCRIPTION", "STARTINGDATE", "ENDINGDATE", "STARTINGTIME", "ENDINGTIME","TIMEWITHINBOUNDS" };

            string sort = "";

            if (lvItems.SortColumn < columns.Length)
            {
                sort = " order by " + columns[lvItems.SortColumn] + (lvItems.SortedBackwards ? " DESC" : " ASC");

            }

            discounts = Providers.DiscountPeriodData.GetDiscountPeriods(PluginEntry.DataModel, sort);

            foreach (var discount in discounts)
            {
                item = new ListViewItem((string)discount.ID);
                item.SubItems.Add(discount.Text);
                item.SubItems.Add(discount.StartingDate.ToShortDateString());
                item.SubItems.Add(discount.EndingDate.ToShortDateString());
                item.SubItems.Add(("0" + discount.StartingTime.Hours).Right(2) + ":" + ("0" + discount.StartingTime.Minutes).Right(2));
                
                if (discount.EndingTime.Days == 1)
                {
                    string endingTimeWith24Hours = "24:00";
                    item.SubItems.Add(endingTimeWith24Hours);
                }
                else
                {
                    item.SubItems.Add(("0" + discount.EndingTime.Hours).Right(2) + ":" + ("0" + discount.EndingTime.Minutes).Right(2));
                }

                item.SubItems.Add(discount.TimeWithinBounds ? Properties.Resources.Yes : Properties.Resources.No);                                
                
                item.Tag = discount.ID;
                item.ImageIndex = -1;

                lvItems.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvItems.Columns[lvItems.SortColumn].ImageIndex = (lvItems.SortedBackwards ? 1 : 0);

            lvItems.BestFitColumns();

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvItems.SortColumn == e.Column)
            {
                lvItems.SortedBackwards = !lvItems.SortedBackwards;
            }
            else
            {
                if (lvItems.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvItems.Columns[lvItems.SortColumn].ImageIndex = 2;
                    lvItems.SortColumn = e.Column;
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            //if (arguments.CategoryKey == GetType().ToString() + ".View")
            //{
            //    arguments.Add(new ContextBarItem(Properties.Resources.Add, Properties.Resources.PlusImage, new ContextbarClickEventHandler(btnAdd_Click)), 10);
            //}
        }

        protected override void OnClose()
        {
            lvItems.SmallImageList = null;

            base.OnClose();
        }
    }
}
