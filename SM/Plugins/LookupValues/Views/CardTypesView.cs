using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class CardTypesView : ViewBase
    {
        RecordIdentifier selectedID;

        public CardTypesView()
        {
            selectedID = RecordIdentifier.Empty;

            InitializeComponent();

            imageList1.Images.Add(Properties.Resources.CreditCard);

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Close | 
                ViewAttributes.Help;

            lvCardTypes.ContextMenuStrip = new ContextMenuStrip();
            lvCardTypes.ContextMenuStrip.Opening += lvCardTypes_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CardTypesEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("AllCardTypes", RecordIdentifier.Empty, Properties.Resources.CardType, false));
        }


        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CardTypes;
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
            var cardInfolist = Providers.CardTypeData.GetList(PluginEntry.DataModel);

            lvCardTypes.Items.Clear();

            foreach (var cardInfo in cardInfolist)
            {
                var item = new ListViewItem((string)cardInfo.ID);
                item.SubItems.Add(cardInfo.Text);
                item.Tag = cardInfo.ID;
                item.ImageIndex = 0;

                lvCardTypes.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvCardTypes.BestFitColumns();

            HeaderText = Properties.Resources.CardTypes;
            //HeaderIcon = Properties.Resources.CreditCard;

            lvCardTypes_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected override bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            return false;
        }

        protected override bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "CardType":
                    LoadData(false);
                    break;
            }

        }

        private void lvCardTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvCardTypes.SelectedItems.Count > 0) ? (RecordIdentifier)lvCardTypes.SelectedItems[0].Tag : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = (lvCardTypes.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.CardTypesEdit);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        void lvCardTypes_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCardTypes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
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

            PluginEntry.Framework.ContextMenuNotify("CardTypesList", lvCardTypes.ContextMenuStrip, lvCardTypes);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowCardTypeSheet((RecordIdentifier)lvCardTypes.SelectedItems[0].Tag);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewCardType();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteCardType(selectedID);
        }

        private void lvCardTypes_DoubleClick(object sender, EventArgs e)
        {
            if (lvCardTypes.SelectedItems.Count != 0)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CardTypesEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                }
            }
        }

        protected override void OnClose()
        {
            lvCardTypes.SmallImageList = null;

            base.OnClose();
        }
    }
}
