using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    public partial class ItemSearchDialog : DialogBase
    {

        private int itemRecordFrom;
        private int itemRecordTo;
        private int maxNumberOfItemRecordsDisplayed = 500;

        private RecordIdentifier itemID;

        public ItemSearchDialog()
        {
            InitializeComponent();

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            ResetRecordCounter();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lvItems.Columns[1].ImageIndex = 0;
            lvItems.SortColumn = 1;

            Search();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            lvItems.SmallImageList = null;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ItemID
        {
            get { return itemID; }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = tbSearch.Text.Length > 0;
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = (lvItems.SelectedItems.Count > 0);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            itemID = (RecordIdentifier)lvItems.SelectedItems[0].Tag;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblToManyError.Visible = false;
            errorProvider1.Clear();

            //lastSearch = tbSearch.Text;

            Search();
        }

        private void Search()
        {
            ListViewItem listItem;

            lvItems.Items.Clear();

            List<SimpleRetailItem> items = Providers.RetailItemData.Search(PluginEntry.DataModel, tbSearch.Text, itemRecordFrom, itemRecordTo + 1, true, SortEnum.Description);

            foreach (var item in items)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);
                listItem.Tag = item.ID;

                if (lvItems.Items.Count == maxNumberOfItemRecordsDisplayed)
                {
                    lblToManyError.Visible = true;
                    errorProvider1.SetIconAlignment(lblToManyError, ErrorIconAlignment.MiddleLeft);
                    errorProvider1.SetError(lblToManyError, lblToManyError.Text);
                    break;
                }
                else
                {
                    lvItems.Add(listItem);
                }

            }
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (btnSearch.Enabled)
                {
                    btnSearch_Click(this, EventArgs.Empty);
                }
                e.Handled = true;
            }

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);
        }


        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                btnSelect_Click(this, EventArgs.Empty);
            }
        }

        private void ResetRecordCounter()
        {
            itemRecordFrom = 1;
            itemRecordTo = maxNumberOfItemRecordsDisplayed;
        }

        
    }
}
