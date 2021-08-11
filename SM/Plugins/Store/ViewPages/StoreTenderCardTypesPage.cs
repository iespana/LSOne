using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

//using LSRetail.Utilities.Locale;

//using LSRetail.StoreController.Common.Settings;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class StoreTenderCardTypesPage : UserControl, ITabView
    {
        RecordIdentifier storeAndPaymentMethodID;
        StorePaymentMethod paymentMethod;

        public StoreTenderCardTypesPage()
        {
            InitializeComponent();

            lvCardTypes.ContextMenuStrip = new ContextMenuStrip();
            lvCardTypes.ContextMenuStrip.Opening += lvCardTypes_ContextMenuStripOpening;

            btnEditCardTypes.Visible = (PluginEntry.Framework.FindImplementor(this, "CanEditCardTypes", null) != null);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreTenderCardTypesPage();
        }

        private void PopulateCardList(RecordIdentifier selectedCard)
        {
            RecordIdentifier itemIdentifier;
            ListViewItem item;

            List<StoreCardType> cards;

            cards = Providers.PaymentTypeCardTypesData.GetCardListForTenderType(PluginEntry.DataModel, storeAndPaymentMethodID.PrimaryID, storeAndPaymentMethodID.SecondaryID);

            lvCardTypes.Items.Clear();

            foreach (StoreCardType card in cards)
            {
                item = new ListViewItem(card.Description);
                item.SubItems.Add(card.CheckModulus.ToString());
                item.SubItems.Add(card.CheckExpiredDate.ToString());
                item.SubItems.Add(card.ProcessLocally.ToString());
                item.SubItems.Add(card.AllowManualInput.ToString());

                // Make a tripple identifier so that if other plugin even extends this view that it has all the data
                itemIdentifier = (RecordIdentifier)storeAndPaymentMethodID.Clone();
                itemIdentifier.SecondaryID.SecondaryID = card.CardTypeID;

                item.Tag = itemIdentifier;

                lvCardTypes.Add(item);

                if(selectedCard == itemIdentifier)
                {
                    item.Selected = true;
                }
            }
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            storeAndPaymentMethodID = context;
            paymentMethod = (StorePaymentMethod)internalContext;

            if (paymentMethod.PosOperation == (int) POSOperations.PayCreditMemo)
            {
                lvCardTypes.Enabled = btnsContextButtons.Enabled = btnEditCardTypes.Enabled = false;
                return;
            }

            PopulateCardList(RecordIdentifier.Empty);

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StoreEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == context.PrimaryID);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvCardTypes.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void lvCardTypes_SizeChanged(object sender, EventArgs e)
        {
            int columnSize = (lvCardTypes.Width - 20) / 5;

            foreach (ColumnHeader header in lvCardTypes.Columns)
            {
                header.Width = columnSize;
            }
        
          

        }

        private void lvCardTypes_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;  
        }

        private void lvCardTypes_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.Graphics.DrawLine(SystemPens.ControlLight, e.Bounds.Right - 1, e.Bounds.Top - 1, e.Bounds.Right - 1, e.Bounds.Bottom - 1);
            e.Graphics.DrawLine(SystemPens.ControlLight, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right - 1, e.Bounds.Bottom - 1);

           
            if(e.SubItem.Text == "True")
            {
                e.Graphics.DrawImageUnscaled(Properties.Resources.CheckSymbol, (e.Bounds.Width / 2) - 8 + e.Bounds.Left, e.Bounds.Top);
            }
            else
            {
               
            }
            
            

            e.DrawDefault = false;
            e.DrawFocusRectangle(e.Bounds);
        }

        private void lvCardTypes_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Selected) != 0 && e.Item.Selected)
            {
                if (lvCardTypes.Focused)
                {
                    e.Item.ForeColor = ColorPalette.White;
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                }
                else
                {
                    Brush brush;
                    e.Item.ForeColor = ColorPalette.Black;
                    brush = new SolidBrush(Color.WhiteSmoke);
                    e.Graphics.FillRectangle(brush, e.Bounds);
                    brush.Dispose();
                }

            }
            else
            {
                e.Item.ForeColor = ColorPalette.Black;
                
            }
            e.Item.UseItemStyleForSubItems = true;



            e.DrawText();
            e.DrawDefault = false;
        }

        private void lvCardTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled =
                lvCardTypes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
        }

        void lvCardTypes_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvCardTypes.ContextMenuStrip.Items.Clear();


            ExtendedMenuItem item;

            item = new ExtendedMenuItem(
                    Properties.Resources.EditCardType,
                    ContextButtons.GetEditButtonImage(),
                    100,
                    btnEdit_Click);
            item.Default = true;
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            lvCardTypes.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.AddAllowedCardType,
                    ContextButtons.GetAddButtonImage(),
                    110,
                    btnAdd_Click);
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            lvCardTypes.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.RemoveAllowedCardType,
                    ContextButtons.GetRemoveButtonImage(),
                    120,
                    btnRemove_Click);
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            lvCardTypes.ContextMenuStrip.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StoreTenderCardTypes", lvCardTypes.ContextMenuStrip, lvCardTypes);

            e.Cancel = false;
        }

        private void lvCardTypes_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            RecordIdentifier itemID = (RecordIdentifier)lvCardTypes.SelectedItems[0].Tag;

            Dialogs.CardTypeDialog dlg = new Dialogs.CardTypeDialog(storeAndPaymentMethodID, itemID.SecondaryID.SecondaryID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PopulateCardList(itemID);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.CardTypeDialog dlg = new Dialogs.CardTypeDialog(storeAndPaymentMethodID, RecordIdentifier.Empty);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PopulateCardList(RecordIdentifier.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RecordIdentifier itemID = (RecordIdentifier)lvCardTypes.SelectedItems[0].Tag;

            if (QuestionDialog.Show(
                Properties.Resources.RemoveCardTypeQuestion, 
                Properties.Resources.RemoveAllowedCardType) == DialogResult.Yes)
            {
                Providers.PaymentTypeCardTypesData.Delete(PluginEntry.DataModel, itemID);

                PopulateCardList(RecordIdentifier.Empty);
            }
        }

        private void btnEditCardTypes_Click(object sender, EventArgs e)
        {
            IPlugin cardTypeEditor = PluginEntry.Framework.FindImplementor(this, "CanEditCardTypes", null);

            cardTypeEditor.Message(this, "EditCardTypes", null);
        }

        
    }
}
