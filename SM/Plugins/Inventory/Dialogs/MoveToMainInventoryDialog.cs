using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class MoveToMainInventoryDialog : DialogBase
    {
        private const InventoryJournalTypeEnum journalType = InventoryJournalTypeEnum.Parked;

        private InventoryJournalTransaction journalLine;

        private string reasonId = "";
        private string reasonText = "";

        protected MoveToMainInventoryDialog()
        {
            InitializeComponent();
            Text = Resources.MoveToMainInventory;
            Header = Resources.MoveToInventoryDialogHeader;
        }

        public MoveToMainInventoryDialog(InventoryJournalTransaction selectedLine)
            : this()
        {
            //don't open dialog if no journal line was provided.
            if (selectedLine == null)
            {
                Close();
            }

            journalLine = selectedLine;

            cmbItem.SelectedData = new DataEntity(journalLine.ItemId, journalLine.ItemName);
            cmbVariantNumber.SelectedData = new DataEntity(journalLine.ItemId, journalLine.VariantName);
            cmbUnit.SelectedData = new DataEntity(journalLine.UnitID, journalLine.UnitDescription);
            ntbQuantity.Text = (Math.Abs(journalLine.Adjustment) - journalLine.MovedQuantity).FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity));

            btnEditReasonCode.Enabled = (cmbReason.Text != "");
            btnOK.Enabled = false;
        }

        #region DialogBase

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        #endregion

        private void cmbReason_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                reasonId = (string)((DataEntity)e.Data).ID;
                reasonText = ((DataEntity)e.Data).Text;
                e.TextToDisplay = reasonText;
            }
        }

        private void cmbReason_RequestData(object sender, EventArgs e)
        {
            List<ReasonCode> reasonList = null;
            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                reasonList = service.GetReasonCodesList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true,
                                                        new List<ReasonActionEnum> { ReasonActionEnum.MainInventory },
                                                        forPOS: null, open: true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            cmbReason.SetData(reasonList, null);
        }

        private void cmbReason_SelectedDataChanged(object sender, EventArgs e)
        {
            btnEditReasonCode.Enabled = (cmbReason.Text != "");
            btnOK.Enabled = (cmbReason.Text != "" && ntbQuantity.FullPrecisionValue > 0 && ntbQuantity.FullPrecisionValue <= Math.Abs(journalLine.Adjustment));
        }

        private void cmbReason_SelectedDataCleared(object sender, EventArgs e)
        {
            btnEditReasonCode.Enabled = btnOK.Enabled = false;
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (ntbQuantity.FullPrecisionValue > 0 && ntbQuantity.FullPrecisionValue <= Math.Abs(journalLine.Adjustment) && cmbReason.Text != "");
        }

        private void btnAddEditReasonCode_Click(object sender, EventArgs e)
        {
            ReasonCodeDialogBehaviour action = ReasonCodeDialogBehaviour.Add;
            if (sender is ContextButton && ((ContextButton)sender).Context == ButtonType.Edit)
            {
                action = ReasonCodeDialogBehaviour.Edit;
            }

            RecordIdentifier selectedReasonID = cmbReason.SelectedData != null
                                            ? ((ReasonCode)cmbReason.SelectedData).ID
                                            : RecordIdentifier.Empty;

            using (ReasonCodeDialog dlg = new ReasonCodeDialog(action, ReasonActionEnum.MainInventory, selectedReasonID))
            {
                DialogResult result = dlg.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    try
                    {
                        cmbReason_RequestData(sender, e);
                        if (dlg.LastReasonCode != null && !RecordIdentifier.IsEmptyOrNull(dlg.LastReasonCode.ID))
                        {
                            cmbReason.SelectedData = dlg.LastReasonCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + Environment.NewLine + ex.Message);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                CostCalculation costCalculation = (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation).IntValue;

                PluginOperations.MoveToInventory(journalLine, Convert.ToDecimal(ntbQuantity.Text), (ReasonCode)cmbReason.SelectedData, costCalculation);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
