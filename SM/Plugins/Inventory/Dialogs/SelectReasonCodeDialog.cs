using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class SelectReasonCodeDialog : DialogBase
    {
        private ReasonActionEnum type;

        private ReasonCode selectedReason;
        //private string reasonId = "";
        //private string reasonText = "";

        public ReasonCode SelectedReasonCode
        {
            get { return selectedReason; }
        }

        protected SelectReasonCodeDialog()
        {
            InitializeComponent();
            Text = Resources.SelectReasonCode;
            Header = Resources.SelectReason;
        }

        public SelectReasonCodeDialog(ReasonActionEnum reasonType)
            : this()
        {
            type = reasonType;
            btnEditReasonCode.Enabled = btnOK.Enabled = (cmbReason.Text != "");
        }

        #region DialogBase

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        #endregion

        /*
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
        */
        private void cmbReason_RequestData(object sender, EventArgs e)
        {
            List<ReasonCode> reasonList = null;
            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                reasonList = service.GetReasonCodesList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true,
                                                        new List<ReasonActionEnum> { type },
                                                        forPOS: null, open: true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            cmbReason.SetData(reasonList.ConvertAll(rc => (DataEntity)rc), null);
        }

        private void cmbReason_SelectedDataChanged(object sender, EventArgs e)
        {
            selectedReason = (cmbReason.Text == "" ? null : (ReasonCode)cmbReason.SelectedData);
            btnEditReasonCode.Enabled = btnOK.Enabled = (cmbReason.Text != "");
        }

        private void cmbReason_SelectedDataCleared(object sender, EventArgs e)
        {
            selectedReason = null;
            btnEditReasonCode.Enabled = btnOK.Enabled = false;
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


            using (ReasonCodeDialog dlg = new ReasonCodeDialog(action, type, selectedReasonID))
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

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            //Save to database
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
