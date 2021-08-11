using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class ReasonCodeSelectDialog : TouchBaseForm
    {
        public ReasonCode SelectedReasonCode;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public ReasonCodeSelectDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            touchBanner.BannerText = Resources.SelectAReasonCode;

            LoadReasonCodes();
        }

        private void LoadReasonCodes()
        {
            List<ReasonCode> reasonCodes = new List<ReasonCode>();

            try
            {
                reasonCodes = Providers.ReasonCodeData.GetReasonCodesForReturn(dlgEntry);

                if(!reasonCodes.Any())
                    reasonCodes = Interfaces.Services.InventoryService(dlgEntry).GetReasonCodesForReturn(dlgEntry, dlgSettings.SiteServiceProfile, true);

                if (!reasonCodes.Any())
                {
                    //If no return reason codes, create a default one locally and at HQ
                    var defaultReason = GetDefaultReturnReasonCode(dlgEntry, dlgSettings.SiteServiceProfile, true);
                    reasonCodes = new List<ReasonCode> { defaultReason };
                }
            }
            catch (Exception e)
            {
                ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                DialogResult = DialogResult.Abort;
                return;
            }

            foreach (ReasonCode code in reasonCodes)
            {
                Row row = new Row();
                row.AddText(code.Text);
                row.AddText(ReasonActionHelper.ReasonActionEnumToString(code.Action));
                row.Tag = code;
                lvReasonCodes.AddRow(row);
            }

            if(reasonCodes.Count > 0)
            {
                lvReasonCodes.Selection.Set(0);
            }

            lvReasonCodes.AutoSizeColumns();
            if (lvReasonCodes.Columns.Count > 0)
            {
                lvReasonCodes.Columns[1].Width = (short)(lvReasonCodes.Width - lvReasonCodes.Columns[0].Width);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(lvReasonCodes.Selection.Count == 1)
            {
                SelectedReasonCode = (ReasonCode)lvReasonCodes.Selection[0].Tag;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void lvReasonCodes_SelectionChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = lvReasonCodes.Selection.Count == 1;
        }

        /// <summary>
        /// Creates and saves a default system reason code for returns
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="closeConnection"></param>
        /// <returns></returns>
        /// <remarks>Same code is used in "Return Item" operation (POSPRocesses>Operations>ItemSale) and in ReturnTransactionDialog</remarks>
        private ReasonCode GetDefaultReturnReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            ReasonCode defaultReason = ReasonCode.DefaultReturns();

            if (!Providers.ReasonCodeData.Exists(entry, defaultReason.ID))
            {
                Providers.ReasonCodeData.Save(entry, defaultReason);
            }

            ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            if (service.TestConnection(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber) == ConnectionEnum.Success)
            {
                service.SaveReasonCode(entry, siteServiceProfile, defaultReason, closeConnection);
            }

            return defaultReason;
        }

        private void lvReasonCodes_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            btnOk_Click(sender, EventArgs.Empty);
        }
    }
}