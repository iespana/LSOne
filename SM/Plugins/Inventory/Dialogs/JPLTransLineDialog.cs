using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController.SharedDialogs;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.BusinessObjects.DataEntities;
using LSRetail.StoreController.BusinessObjects;
using LSRetail.StoreController.BusinessObjects.DataEntities.Dimensions;
using LSRetail.StoreController.Controls;
using LSRetail.StoreController.BusinessObjects.Dimensions;
using LSRetail.StoreController.BusinessObjects.DataEntities.Inventory;
using LSRetail.StoreController.BusinessObjects.Inventory;
using LSRetail.StoreController.Inventory.Datalayer;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedCore.Enums;

namespace LSRetail.StoreController.Inventory.Dialogs
{

    public partial class AdjustmentLineDialog : LSRetail.StoreController.SharedCore.Dialogs.DialogBase
    {        
        private InventoryJournalTransaction journalTransactionLine;
     
        private RecordIdentifier selectedJournalID;
        private RecordIdentifier itemRecId;
        private RecordIdentifier unitId = "";
        
        private int salesUnitDecimals;
                
        private decimal standardPrice;
        private decimal standardPriceWithTax;
        private decimal lookedUpItemVATPercent;

        private string itemId = "";
        private string itemText = "";
        private string reasonId = "";
        private string reasonText = "";
        private string unitText = "";

        public AdjustmentLineDialog()
        {
            InitializeComponent();          
        }

           public AdjustmentLineDialog(Utilities.DataTypes.RecordIdentifier selectedJournalID) : this()
           {
               this.selectedJournalID = selectedJournalID;
               tbJournalID.Text = selectedJournalID.PrimaryID.DBValue.ToString();
               if (PluginEntry.DataModel.CurrentStoreID.IsEmpty)
                   tbWarehouse.Text = Properties.Resources.HeadOffice;
               else
                   tbWarehouse.Text = (string)PluginEntry.DataModel.CurrentStoreID;

               tbWarehouse.Enabled = false;
               btnOK.Enabled = false;
           }

           private void cmbRelation_DropDown(object sender, Controls.DropDownEventArgs e)
           {
               e.ControlToEmbed = new RetailItemSearchPanel(PluginEntry.DataModel, "");
           }

           private void cmbRelation_FormatData(object sender, Controls.DropDownFormatDataArgs e)
           {
               if (((DataEntity)e.Data).ID == "")
               {
                   e.TextToDisplay = "";
               }
               else
               {
                   itemRecId = ((DataEntity)e.Data).ID;
                   itemId = (string)((DataEntity)e.Data).ID;
                   itemText = ((DataEntity)e.Data).Text;
                   e.TextToDisplay = itemRecId.PrimaryID.ToString() + " - " + itemText;              
               }

               CheckEnabled(this, EventArgs.Empty);
           }

           private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
           {
               bool defaultStoreExists = true;
               bool defaultStoreHasTaxGroup = true;
               bool itemHasTaxGroup = true;

               tbColor.Clear();
               tbSize.Clear();
               tbStyle.Clear();


               if (cmbRelation.SelectedData.ID != "")
               {
                   lblVariantNumber.ForeColor = SystemColors.ControlText;
                   cmbVariantNumber.Enabled = true;
                   cmbVariantNumber.SelectedData = new Dimension();

                   RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);
                   standardPrice = retailItem[RetailItem.ModuleTypeEnum.Sales].Price;
                   standardPriceWithTax = retailItem[RetailItem.ModuleTypeEnum.Sales].PriceIncludeTax;
                   unitId = retailItem[RetailItem.ModuleTypeEnum.Inventory].Unit;
                   unitText = (string)retailItem[RetailItem.ModuleTypeEnum.Inventory].UnitText;

                   LookupData.GetInfoForRetailItem(
                        PluginEntry.DataModel,
                        itemRecId, 
                        ref itemText,
                        ref unitId,
                        ref unitText, 
                        ref salesUnitDecimals);

                   ntbQuantity.DecimalLetters = salesUnitDecimals;
                   if (salesUnitDecimals > 0)
                       ntbQuantity.AllowDecimal = true;
                   else
                       ntbQuantity.AllowDecimal = false;

                   ntbQuantity.Text = "0";
                   lblUnitText.Text = unitText;

                   if (Providers.DimensionData.GetList(PluginEntry.DataModel, cmbRelation.SelectedData.ID, 0, false).Count > 0)
                   {
                       cmbVariantNumber.Enabled = true;
                   }
                   else
                   {
                       cmbVariantNumber.Enabled = false;
                   }

                   lookedUpItemVATPercent = BusinessObjects.Providers.RetailItemData.GetItemTaxPercentage(PluginEntry.DataModel, cmbRelation.SelectedData.ID, ref defaultStoreExists, ref defaultStoreHasTaxGroup, ref itemHasTaxGroup);
                    
               }
               
               CheckEnabled(this, EventArgs.Empty);
           }

           private void cmbVariantNumber_FormatData(object sender, Controls.DropDownFormatDataArgs e)
           {
               Dimension dimension = (Dimension)((DualDataComboBox)sender).SelectedData;

               e.TextToDisplay = dimension.VariantNumber.ToString();
           }

           private void cmbVariantNumber_RequestData(object sender, EventArgs e)
           {
               cmbVariantNumber.SetWidth(350);

               cmbVariantNumber.SetHeaders(new string[] { 
                Properties.Resources.VariantNumber,
                Properties.Resources.Size,
                Properties.Resources.Color,
                Properties.Resources.Style               
                },
                   new int[] { 0, 1, 2, 3 });

               cmbVariantNumber.SetData(Providers.DimensionData.GetList(PluginEntry.DataModel, cmbRelation.SelectedData.ID, 4, false).Cast<DataEntity>(),
                  null,
                  -1);

               CheckEnabled(this, EventArgs.Empty);
           }

           private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
           {
               Dimension dimension = (Dimension)cmbVariantNumber.SelectedData;

               tbColor.Text =  dimension.ColorName;
               tbSize.Text =  dimension.SizeName;
               tbStyle.Text =  dimension.StyleName;

               CheckEnabled(this, EventArgs.Empty);

           }

           private void CheckEnabled(AdjustmentLineDialog jPLTransLineDialog, EventArgs eventArgs)
           {
               bool enabled = false;

               enabled = cmbRelation.Text != "";

               enabled = enabled && (((cmbVariantNumber.Enabled == true) && (cmbVariantNumber.Text != "")) || ((cmbVariantNumber.Enabled == false) && (cmbVariantNumber.Text == "")));

               enabled = enabled && (cmbReason.Text != "");

               enabled = enabled && ((ntbQuantity.Text != "") && (ntbQuantity.Value != 0));

               btnOK.Enabled = enabled;               
           }

           private void btnOK_Click(object sender, EventArgs e)
           {
               string s = cmbVariantNumber.Text;
               journalTransactionLine = new InventoryJournalTransaction();
               journalTransactionLine.JournalId = tbJournalID.Text;
               journalTransactionLine.Origin = PluginEntry.DataModel.CurrentStoreID;
               if (journalTransactionLine.Origin.IsEmpty)
                   journalTransactionLine.Origin = Properties.Resources.HeadOffice;

               MessageBox.Show("Here the jplTransaLine dialog should get lineNum from sequence manager.");
               //journalTransactionLine.LineNum = Providers.InventoryJournalTransactionData.GetNextId(PluginEntry.DataModel, tbJournalID.Text, journalTransactionLine.Origin);

               journalTransactionLine.TransDate = DateTime.Now;
               journalTransactionLine.Voucher = "";
               journalTransactionLine.JournalType = InventoryJournalTable.JournalTypeEnum.Adjustment;
               journalTransactionLine.ItemId = itemId;
               try
               {
                   journalTransactionLine.Adjustment = Convert.ToDecimal(ntbQuantity.Text);
               }
               catch (Exception)
               {
                   journalTransactionLine.Adjustment = 0;
               }

               journalTransactionLine.CostPrice = 0;
               journalTransactionLine.PriceUnit = 0;
               journalTransactionLine.CostMarkup = 0;
               journalTransactionLine.CostAmount = 0;
               journalTransactionLine.SalesAmount = 0;
               journalTransactionLine.InventOnHand = 0;
               journalTransactionLine.Counted = 0;
               journalTransactionLine.VariantId = cmbVariantNumber.Text;
               journalTransactionLine.Dimension = "";
               journalTransactionLine.Dimension2 = "";
               journalTransactionLine.Dimension3 = "";
               journalTransactionLine.ReasonId = reasonId;
            
               Providers.InventoryJournalTransactionData.Save(PluginEntry.DataModel, journalTransactionLine);
               Providers.InventoryJournalTransactionData.PostInventTransLine(PluginEntry.DataModel, journalTransactionLine);

               PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemInventory", itemId, null);

               DialogResult = DialogResult.OK;
               Close();                              
           }


           private void cmbReason_FormatData(object sender, DropDownFormatDataArgs e)
           {
               if (((DataEntity)e.Data).ID == "")
               {
                   e.TextToDisplay = "";
               }
               else
               {
                   DataEntity reason = ((DualDataComboBox)sender).SelectedData;
                   reasonId = (string)((DataEntity)e.Data).ID;
                   reasonText = ((DataEntity)e.Data).Text;
                   e.TextToDisplay = reasonText;
               }               
           }

           private void cmbReason_SelectedDataChanged(object sender, EventArgs e)
           {
               DataEntity reason = (DataEntity)cmbReason.SelectedData;
               CheckEnabled(this, EventArgs.Empty);
           }

           private void cmbReason_RequestData(object sender, EventArgs e)
           {
               cmbReason.SetData(Providers.ReasonsData.GetList(PluginEntry.DataModel), null, -1);               
           }

           private void ntbQuantity_TextChanged(object sender, EventArgs e)
           {
               CheckEnabled(this, EventArgs.Empty);
           }

           private void btnCancel_Click(object sender, EventArgs e)
           {
               DialogResult = DialogResult.Cancel;
               Close();
           }

           private void btnEditReasons_Click(object sender, EventArgs e)
           {
               InventoryAdjustmentReasonDialog dlg = new InventoryAdjustmentReasonDialog();

               dlg.ShowDialog(this);
           }
                     
    }
}
