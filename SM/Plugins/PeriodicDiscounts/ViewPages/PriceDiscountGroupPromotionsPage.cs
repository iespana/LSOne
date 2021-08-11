using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    public partial class PriceDiscountGroupPromotionsPage : UserControl, ITabView
    {
        PriceDiscGroupEnum displayType;
        private RecordIdentifier discountGroupID;
        private RecordIdentifier selectedID;

        public PriceDiscountGroupPromotionsPage(TabControl owner, TabControl.Tab ownerTab)
            : this()
        {
      
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {            
            return new PriceDiscountGroupPromotionsPage((TabControl)sender, tab);
        }
        
        public static void TabMessage(TabControl sender, TabControl.Tab tab, int hint, object data)
        {
            PriceDiscGroupEnum displayType = (PriceDiscGroupEnum)hint;

            tab.Visible = displayType == PriceDiscGroupEnum.LineDiscountGroup;
            sender.Invalidate();
        }

        public PriceDiscountGroupPromotionsPage()
        {
            InitializeComponent();

            lvValues.Columns[0].Tag = DiscountOfferSorting.OfferNumber;
            lvValues.Columns[1].Tag = DiscountOfferSorting.Description;           
            lvValues.Columns[2].Tag = DiscountOfferSorting.Status;
            lvValues.Columns[3].Tag = DiscountOfferSorting.DiscountValidationPeriod;
            lvValues.Columns[4].Tag = DiscountOfferSorting.StartingDate;
            lvValues.Columns[5].Tag = DiscountOfferSorting.EndingDate;
            lvValues.SetSortColumn(0, true);

            selectedID = "";

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;
        }       

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            displayType = (PriceDiscGroupEnum)((int)internalContext);
            discountGroupID = context[2];            

            if (displayType == PriceDiscGroupEnum.LineDiscountGroup)
            {
                LoadLines();
            }
        }

        private void LoadLines()
        {
            List<DiscountOffer> offers = Providers.DiscountOfferData.GetPromotionsForLineDiscountGroup(PluginEntry.DataModel, discountGroupID, (DiscountOfferSorting)lvValues.SortColumn.Tag, !lvValues.SortedAscending);

            lvValues.ClearRows();

            foreach (DiscountOffer offer in offers)
            {
                Row row = new Row();
                row.AddText((string)offer.ID);
                row.AddText(offer.Text);
                row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                row.AddText(offer.ValidationPeriodDescription);
                row.AddCell(new DateTimeCell(offer.StartingDate.ToShortDateString(), offer.StartingDate.DateTime));
                row.AddCell(new DateTimeCell(offer.EndingDate.ToShortDateString(), offer.EndingDate.DateTime));
                row.AddText(offer.PriceGroupName);

                row.Tag = offer;
                
                lvValues.AddRow(row);

                if (offer.ID == selectedID)
                {
                    lvValues.Selection.Set(lvValues.RowCount - 1);
                }
            }

            lvValues.AutoSizeColumns();            
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
            contexts.Add(new AuditDescriptor("DiscountOffers", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion, Properties.Resources.Promotions, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PromotionOffer")
            {
                selectedID = changeIdentifier;
                LoadLines();
            }
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                Properties.Resources.EditDiscount,
                100,
                EditOfferHandler);
            item.Enabled = btnEdit.Enabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DiscountGroupPromotionsList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void EditOfferHandler(object sender, EventArgs e)
        {
            DiscountOffer offer = (DiscountOffer)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag;                        
            PluginOperations.ShowSpecificPromotionsView(offer.ID);
        }


        private void lvValues_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvValues.SortColumn == args.Column)
            {
                lvValues.SetSortColumn(args.Column, !lvValues.SortedAscending);
                LoadLines();
            }
            else
            {
                lvValues.SetSortColumn(args.Column, true);
                LoadLines();
            }
        }

        private void lvValues_RowDoubleClick(object sender, RowEventArgs args)
        {
            EditOfferHandler(this, EventArgs.Empty);
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            if (lvValues.Selection.Count > 0)
            {
                selectedID = ((DiscountOffer)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;
            }

            btnEdit.Enabled = lvValues.Selection.Count > 0;
        }
    }
}
