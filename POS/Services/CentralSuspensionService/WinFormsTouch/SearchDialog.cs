using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// Search dialog for recalling suspended transactions
    /// </summary>
    public partial class SearchDialog : TouchBaseForm
    {
        private bool loaded;
        private string appliedSearchText;
        private DateTime appliedSelectedFromDate;
        private bool appliedSelectedFromDateChecked;
        private DateTime appliedSelectedToDate;
        private bool appliedSelectedToDateChecked;
        private RecordIdentifier appliedTerminalID;
        private RecordIdentifier appliedStaffID;
        private decimal appliedMinAmount;
        private decimal appliedMaxAmount;

        private IConnectionManager entry;

        /// <summary>
        /// Text to search for
        /// </summary>
        public string SearchText => txtSearch.Text;

        /// <summary>
        /// Selected from date
        /// </summary>
        public DateTime? SelectedFromDate => dpFromDate.Checked ? dpFromDate.Value.Date : (DateTime?)null;

        /// <summary>
        /// Selected to date
        /// </summary>
        public DateTime? SelectedToDate => dpToDate.Checked ? dpToDate.Value.Date.AddDays(1).AddMilliseconds(-1) : (DateTime?)null;

        /// <summary>
        /// Selected terminal ID
        /// </summary>
        public RecordIdentifier TerminalID => cmbTerminal.SelectedDataID ?? RecordIdentifier.Empty;

        /// <summary>
        /// Selected staff ID
        /// </summary>
        public RecordIdentifier StaffID => cmbStaff.SelectedDataID ?? RecordIdentifier.Empty;

        /// <summary>
        /// Selected min amount. If both min and max amount are 0, this value is null (ignored)
        /// </summary>
        public decimal? MinAmount => ntbFromAmount.Value == 0 && ntbToAmount.Value == 0 ? (decimal?)null : (decimal)ntbFromAmount.Value;

        /// <summary>
        /// Selected max amount
        /// </summary>
        public decimal? MaxAmount => ntbToAmount.Value == 0 ? (decimal?)null : (decimal)ntbToAmount.Value;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchDialog(IConnectionManager entry)
        {
            InitializeComponent();

            this.entry = entry;
            dpFromDate.MaxDate = DateTime.Now;
            dpToDate.MaxDate = DateTime.Now;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RevertFilter();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveFilter();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ntbFromAmount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ntbFromAmount.Text))
            {
                ntbFromAmount.Value = 0;
            }
        }

        private void ntbToAmount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ntbToAmount.Text))
            {
                ntbToAmount.Value = 0;
            }
        }

        private void SearchDialog_Load(object sender, EventArgs e)
        {
            if (!loaded)
            {
                dpFromDate.Value = DateTime.Now.Date;
                dpToDate.Value = DateTime.Now.Date;
                appliedSelectedFromDate = DateTime.Now.Date;
                appliedSelectedToDate = DateTime.Now.Date;

                loaded = true;
            }
        }

        private void SaveFilter()
        {
            appliedSearchText = txtSearch.Text;
            appliedSelectedFromDate = dpFromDate.Value;
            appliedSelectedFromDateChecked = dpFromDate.Checked;
            appliedSelectedToDate = dpToDate.Value;
            appliedSelectedToDateChecked = dpToDate.Checked;
            appliedTerminalID = TerminalID;
            appliedStaffID = StaffID;
            appliedMinAmount = (decimal)ntbFromAmount.Value;
            appliedMaxAmount = (decimal)ntbToAmount.Value;
        }

        private void RevertFilter()
        {
            txtSearch.Text = appliedSearchText;
            dpFromDate.Value = appliedSelectedFromDate;
            dpFromDate.Checked = appliedSelectedFromDateChecked;
            dpToDate.Value = appliedSelectedToDate;
            dpToDate.Checked = appliedSelectedToDateChecked;
            cmbTerminal.SelectedDataID = !RecordIdentifier.IsEmptyOrNull(appliedTerminalID) ? appliedTerminalID : RecordIdentifier.Empty;
            cmbStaff.SelectedDataID = !RecordIdentifier.IsEmptyOrNull(appliedStaffID) ? appliedStaffID : RecordIdentifier.Empty;
            ntbFromAmount.Value = (double)appliedMinAmount;
            ntbToAmount.Value = (double)appliedMaxAmount;
        }

        private void SearchDialog_Shown(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(sender, EventArgs.Empty);
            }
        }

        private void cmbTerminal_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier selectedID = cmbTerminal.SelectedDataID ?? RecordIdentifier.Empty;
            DualDataPanel panelToEmbed = new DualDataPanel(
                selectedID,
                Providers.TerminalData.GetList(entry, entry.CurrentStoreID),
                null,
                true,
                cmbTerminal.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.SelectNoneAllowed = true;
            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbStaff_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier selectedID = cmbStaff.SelectedDataID ?? RecordIdentifier.Empty;
            DualDataPanel panelToEmbed = new DualDataPanel(
                selectedID,
                Providers.POSUserData.GetList(entry, "", UsageIntentEnum.Minimal, CacheType.CacheTypeApplicationLifeTime),
                null,
                true,
                cmbStaff.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.SelectNoneAllowed = true;
            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbTerminal_RequestClear(object sender, EventArgs e)
        {
            cmbTerminal.SelectedDataID = RecordIdentifier.Empty;
        }

        private void cmbStaff_RequestClear(object sender, EventArgs e)
        {
            cmbStaff.SelectedDataID = RecordIdentifier.Empty;
        }
    }
}
