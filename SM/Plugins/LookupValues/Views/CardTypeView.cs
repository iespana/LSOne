using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class CardTypeView : ViewBase
    {
        private RecordIdentifier cardID;
        private CardType cardType;

        public CardTypeView(RecordIdentifier cardID) 
            : this()
        {
            this.cardID = cardID;
        }

        public CardTypeView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Save |
                ViewAttributes.Revert |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvNumberSeries.ContextMenuStrip = new ContextMenuStrip();
            lvNumberSeries.ContextMenuStrip.Opening += lvNumberSeries_Opening;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.CardTypesEdit);
            btnsContextButtons.AddButtonEnabled = !this.ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("CardTypes", cardID, Properties.Resources.CardType, true));
            descriptors.Add(new AuditDescriptor("CardTypeNumbers", cardID, Properties.Resources.CardNumberSeries, false));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (lvNumberSeries != null)
            {
                int width = lvNumberSeries.Size.Width;

                lvNumberSeries.Columns[0].Width = lvNumberSeries.Columns[1].Width = Math.Min((width - 20) / 2,180);
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CardType;
            }
        }

        protected override string SecondaryRevertText
        {
            get
            {
                return Properties.Resources.CannotRevertSeries;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return cardID;
	        }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.CardType + ": " + (string)tbDescription.Text;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                cardType = Providers.CardTypeData.Get(PluginEntry.DataModel, cardID);

                AddParentViewDescriptor(new ParentViewDescriptor(
                        RecordIdentifier.Empty,
                        Properties.Resources.CardTypes,
                        Properties.Resources.CreditCard,
                        new ShowParentViewHandler(PluginOperations.ShowCardTypesSheet)));
            }

            tbID.Text = (string)cardType.ID;
            tbDescription.Text = cardType.Text;
            cmbType.SelectedIndex =  cardType.CardTypes == CardTypesEnum.Unknown ? cmbType.Items.Count - 1 : (int)cardType.CardTypes;

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.CreditCard;

            LoadSeries();
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != cardType.Text) return true;
            if (cmbType.SelectedIndex != (cardType.CardTypes == CardTypesEnum.Unknown ? cmbType.Items.Count - 1 : (int)cardType.CardTypes)) return true;

            return false;
        }

        protected override bool SaveData()
        {
            cardType.Text = tbDescription.Text;
            cardType.CardTypes = cmbType.SelectedIndex == cmbType.Items.Count - 1 ? CardTypesEnum.Unknown : (CardTypesEnum)cmbType.SelectedIndex;

            Providers.CardTypeData.Save(PluginEntry.DataModel, cardType);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "CardType", cardType.ID, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteCardType(cardID);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "CardType":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == cardID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.CardSerieDialog((RecordIdentifier)lvNumberSeries.SelectedItems[0].Tag);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadSeries();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.CardSerieDialog dlg = new Dialogs.CardSerieDialog(cardID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var serie = dlg.Serie;

                var item = new ListViewItem(serie.CardNumberFrom);
                item.SubItems.Add(serie.CardNumberTo);
                // Record identifier on this record is tripple.
                item.Tag = new RecordIdentifier(serie.CardTypeID, new RecordIdentifier(serie.CardNumberFrom, serie.CardNumberTo));

                lvNumberSeries.Items.Add(item);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteCardNumberSerieQuestion,
                Properties.Resources.DeleteCardNumberSerie) == DialogResult.Yes)
            {
                Providers.CardNumberSerieData.Delete(
                    PluginEntry.DataModel, (RecordIdentifier)lvNumberSeries.SelectedItems[0].Tag);

                lvNumberSeries.Items.Remove(lvNumberSeries.SelectedItems[0]);
            }
        }

        void lvNumberSeries_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvNumberSeries.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
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

            PluginEntry.Framework.ContextMenuNotify("CardTypeSeriesList", lvNumberSeries.ContextMenuStrip, lvNumberSeries);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvNumberSeries_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvNumberSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = (lvNumberSeries.SelectedItems.Count != 0 && !ReadOnly);
        }

        private void LoadSeries()
        {
            var series = Providers.CardNumberSerieData.GetNumberSeries(PluginEntry.DataModel, cardID);

            lvNumberSeries.Items.Clear();

            foreach (CardNumberSerie serie in series)
            {
                var item = new ListViewItem(serie.CardNumberFrom);
                item.SubItems.Add(serie.CardNumberTo);
                // Record identifier on this record is tripple.
                item.Tag = new RecordIdentifier(serie.CardTypeID, new RecordIdentifier(serie.CardNumberFrom, serie.CardNumberTo));

                lvNumberSeries.Items.Add(item);
            }
        }

        protected override void OnClose()
        {
            lvNumberSeries.SmallImageList = null;

            base.OnClose();
        }
    }
}
