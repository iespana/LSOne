using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class ReasonCodeDialog : DialogBase
    {
        private List<ReasonCode> reasons;
        private ReasonCodeDialogBehaviour dialogBehaviour;
        private bool loaded = false;

        private ReasonCode lastReasonCode = null;

        /// <summary>
        /// Returns the edited reason code or the last added reason code. Returns null if multi-edit.
        /// </summary>
        public ReasonCode LastReasonCode
        {
            get { return lastReasonCode; }
        }

        protected ReasonCodeDialog()
        {
            InitializeComponent();
        }

        public ReasonCodeDialog(ReasonCodeDialogBehaviour behaviour, ReasonActionEnum defaultAction, RecordIdentifier reasonCodeID)
           : this(behaviour, RecordIdentifier.IsEmptyOrNull(reasonCodeID) ? new List<RecordIdentifier>() : new List<RecordIdentifier> { reasonCodeID })
        {
            cmbAction.SelectedItem = ReasonActionHelper.ReasonActionEnumToString(defaultAction);
            cmbAction.Enabled = false;
        }

        public ReasonCodeDialog(ReasonCodeDialogBehaviour behaviour, List<RecordIdentifier> reasonCodes)
            : this()
        {
            
            dialogBehaviour = behaviour;

            cmbAction.Items.Clear();
            foreach (object enumItem in Enum.GetValues(typeof(ReasonActionEnum)))
            {
                cmbAction.Items.Add(ReasonActionHelper.ReasonActionEnumToString((ReasonActionEnum)Convert.ToByte(enumItem)));
            }
            cmbAction.SelectedIndex = 0;

            dtBegin.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            dtEnd.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            dtBegin.Value = DateTime.Now.Date;
            dtEnd.Value = DateTime.Now.Date;

            if (IsAddBehaviour)
            {
                Text = Properties.Resources.NewReasonCode;
            }

            if (IsEditBehaviour || IsMultiEditBehaviour)
            {
                Text = IsEditBehaviour ? Properties.Resources.EditReasonCode : Properties.Resources.EditReasonCodes;
                chkCreateAnother.Visible = false;
                cmbAction.Enabled = false;

                tbDescription.Enabled = IsEditBehaviour;
                LoadData(reasonCodes);
            }

            btnOK.Enabled = false;
            loaded = true;
        }

        private void CheckEnabled()
        {
            if (!loaded)
                return;

            bool canSave = false;

            if (IsAddBehaviour)
            {
                canSave = !string.IsNullOrEmpty(tbDescription.Text.Trim());
            }
            else if (IsEditBehaviour)
            {
                ReasonCode editedReason = reasons.Single();

                canSave = (!string.IsNullOrEmpty(tbDescription.Text.Trim()) && tbDescription.Text.Trim() != editedReason.Text) ||
                           cmbAction.SelectedIndex != (int)editedReason.Action ||
                           chkPos.Checked != editedReason.ShowOnPos ||
                           dtBegin.Value != editedReason.BeginDate ||
                           (dtEnd.Checked && !editedReason.EndDate.HasValue) ||
                           (!dtEnd.Checked && editedReason.EndDate.HasValue) ||
                           (dtEnd.Checked && editedReason.EndDate.HasValue && dtEnd.Value != editedReason.EndDate.Value);
            }
            else
            {
                canSave = true;
            }

            btnOK.Enabled = canSave;
        }

        private bool IsAddBehaviour
        {
            get { return dialogBehaviour == ReasonCodeDialogBehaviour.Add; }
        }

        private bool IsEditBehaviour
        {
            get { return dialogBehaviour == ReasonCodeDialogBehaviour.Edit; }
        }
        private bool IsMultiEditBehaviour
        {
            get { return dialogBehaviour == ReasonCodeDialogBehaviour.MultiEdit; }
        }

        private void LoadData(List<RecordIdentifier> reasonCodes)
        {
            reasons = new List<ReasonCode>();

            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                SiteServiceProfile profile = PluginOperations.GetSiteServiceProfile();

                foreach (RecordIdentifier reasonId in reasonCodes)
                {
                    reasons.Add(service.GetReasonById(PluginEntry.DataModel, profile, reasonId, true));
                }

                PopulateReasonCodes();
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void PopulateReasonCodes()
        {
            if (IsEditBehaviour)
            {
                ReasonCode reason = reasons.Single();
                tbDescription.Text = reason.Text;
                cmbAction.SelectedIndex = (int)reason.Action;
                dtBegin.Value = reason.BeginDate;
                dtEnd.Checked = reason.EndDate.HasValue;

                if (reason.EndDate.HasValue)
                {
                    dtEnd.Value = reason.EndDate.Value;
                }

                chkPos.Checked = reason.ShowOnPos;

                if(reason.IsSystemReasonCode)
                {
                    dtBegin.Enabled = false;
                    dtEnd.Enabled = false;
                    chkPos.Enabled = false;
                }
            }

            if (IsMultiEditBehaviour)
            {
                cmbAction.Items.Clear();
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private bool Save()
        {
            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                SiteServiceProfile profile = PluginOperations.GetSiteServiceProfile();

                if(IsAddBehaviour)
                {
                    lastReasonCode = new ReasonCode
                    {
                        ID = RecordIdentifier.Empty,
                        Text = tbDescription.Text,
                        Action = (ReasonActionEnum)cmbAction.SelectedIndex,
                        BeginDate = dtBegin.Value,
                        EndDate = dtEnd.Checked ? dtEnd.Value : (DateTime?)null,
                        IsSystemReasonCode = false,
                        ShowOnPos = chkPos.Checked
                    };

                    service.SaveReasonCode(PluginEntry.DataModel, profile, lastReasonCode, true);

                    if (chkCreateAnother.Checked)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(
                                this,
                                IsAddBehaviour ? ViewCore.Enums.DataEntityChangeType.Add : ViewCore.Enums.DataEntityChangeType.Edit,
                                "ReasonCode",
                                lastReasonCode.ID,
                                null);

                        tbDescription.Text = string.Empty;
                        tbDescription.Focus();
                        return false;
                    }
                }
                else if(IsEditBehaviour)
                {
                    ReasonCode reason = reasons.Single();

                    if (reason.IsSystemReasonCode)
                    {
                        reason.Text = tbDescription.Text;
                    }
                    else
                    {
                        reason.Text = tbDescription.Text;
                        reason.BeginDate = dtBegin.Value;
                        reason.EndDate = dtEnd.Checked ? dtEnd.Value : (DateTime?)null;
                        reason.ShowOnPos = chkPos.Checked;
                    }

                    service.SaveReasonCode(PluginEntry.DataModel, profile, reason, true);

                    lastReasonCode = reason;
                }
                else
                {
                    foreach (ReasonCode reason in reasons)
                    {
                        if (!reason.IsSystemReasonCode)
                        {
                            reason.BeginDate = dtBegin.Value;
                            reason.EndDate = dtEnd.Checked ? dtEnd.Value : (DateTime?)null;
                            reason.ShowOnPos = chkPos.Checked;
                            service.SaveReasonCode(PluginEntry.DataModel, profile, reason, true);
                        }
                    }

                    lastReasonCode = null;
                }
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                return false;
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(Save())
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(
                    null,
                    IsAddBehaviour ? ViewCore.Enums.DataEntityChangeType.Add : ViewCore.Enums.DataEntityChangeType.Edit,
                    "ReasonCode",
                    null,
                    null);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Control_DataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

    }

    public enum ReasonCodeDialogBehaviour
    {
        Add,
        Edit,
        MultiEdit
    }
}
