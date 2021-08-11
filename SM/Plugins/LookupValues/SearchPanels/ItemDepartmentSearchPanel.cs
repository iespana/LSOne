using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.LookupValues.SearchPanels
{
    public partial class ItemDepartmentSearchPanel : UserControl, IControlClosable
    {
        RecordIdentifier selectedID;
        bool lockEvents;
        bool itemSearch;

        #pragma warning disable 0067 // We suppress this warning until we actually implement the RequestClear on all forms but its needed for the interface to have it in until then.
        public event EventHandler RequestClear;
        public event EventHandler RequestNoChange;
#pragma warning restore 0067

        public ItemDepartmentSearchPanel(RecordIdentifier selectedID,bool itemSearch)
            : this()
        {
            this.itemSearch = itemSearch;
            this.selectedID = selectedID;
        }

        private ItemDepartmentSearchPanel()
        {
            lockEvents = false;
            itemSearch = false;

            InitializeComponent();
        }

        public bool SelectNoneAllowed { get; set; }
        public bool NoChangeAllowed { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (selectedID != "")
            {
                Search((string)selectedID);
            }

            tbLookFor.Focus();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            tbLookFor.Focus();
        }

        private void Search(string text)
        {
            ListViewItem lvItem;
            ListViewItem selected = null;
            List<DataEntity> foundItems;

            lockEvents = true;

            if(itemSearch)
            {
                foundItems = Providers.RetailItemData.FindItem(PluginEntry.DataModel, text);
            }
            else
            {
                foundItems = Providers.RetailItemData.FindItemDepartment(PluginEntry.DataModel, text);
            }

            lvItems.Items.Clear();

            foreach (DataEntity item in foundItems)
            {
                if (item.ID == RecordIdentifier.Empty)
                {
                    lvItem = new ListViewItem("");
                }
                else
                {
                    lvItem = new ListViewItem(item.ID.ToString());
                }


                lvItem.SubItems.Add(item.Text);
                lvItem.Tag = item;
                
                lvItems.Items.Add(lvItem);

                if (item.ID == selectedID)
                {
                    selected = lvItem;
                }
            }


            if (selected != null)
            {
                selected.Selected = true;
            }
            
            lockEvents = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search(tbLookFor.Text);
        }

        private void tbLookFor_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = tbLookFor.Text.Length > 0;
        }


        private void tbLookFor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (btnSearch.Enabled)
                {
                    btnSearch_Click(this, EventArgs.Empty);
                }
                e.Handled = true;
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lockEvents)
            {
                DropDownForm form = ((DropDownForm)this.FindForm());

                if (lvItems.SelectedItems.Count > 0)
                {
                    form.SelectedData = ((DataEntity)lvItems.SelectedItems[0].Tag);
                }
            }
        }

        private void lvItems_Click(object sender, EventArgs e)
        {
            DropDownForm form = ((DropDownForm)this.FindForm());

            if (lvItems.SelectedItems.Count > 0)
            {
                form.SelectedData = ((DataEntity)lvItems.SelectedItems[0].Tag);
                form.Close();
            }
        }

        private void lvItems_KeyDown(object sender, KeyEventArgs e)
        {
            DropDownForm form = ((DropDownForm)this.FindForm());

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (lvItems.SelectedItems.Count > 0)
                {
                    form.Close();
                }
            }
        }

        #region IControlClosable Members

        public void OnClose()
        {
            lvItems.SmallImageList = null;
        }

        public Control EmbeddedControl
        {
            get { return this; }
        }

        #endregion
    }
}
