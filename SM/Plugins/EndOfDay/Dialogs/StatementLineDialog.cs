using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Statements;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.EndOfDay.Dialogs
{
    public partial class StatementLineDialog : DialogBase
    {
        private StatementLine statementLine;
        
        public StatementLineDialog(RecordIdentifier statementID, RecordIdentifier lineNumber,RecordIdentifier storeID, string storeName)
            : this()
        {
            statementLine = Providers.StatementLineData.Get(PluginEntry.DataModel, statementID, lineNumber);

            if (statementLine == null)
            {
                throw new DataIntegrityException(typeof(StatementLine), new RecordIdentifier(statementID, lineNumber));
            }

            tbStatementNumber.Text = statementID.ToString();
            cmbStore.SelectedData = new DataEntity(storeID,storeName);
            tbStaffID.Text = statementLine.StaffID;
            tbTerminalID.Text = statementLine.TerminalID;
            tbTender.Text = statementLine.TenderDescription;

            ntbTransactionAmount.Value = (double)statementLine.TransactionAmount;
            ntbBankedAmount.Value = (double)statementLine.BankedAmount;
            ntbSafeAmount.Value = (double)statementLine.SafeAmount;
            ntbCountedAmount.Value = (double)statementLine.CountedAmount;
            ntbDifference.Value = (double)statementLine.Difference;

        }

        public StatementLineDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Update the difference value
            double difference = ntbTransactionAmount.Value
                + ntbBankedAmount.Value
                + ntbSafeAmount.Value
                - ntbCountedAmount.Value;

            statementLine.Difference = (decimal)difference;

            statementLine.CountedAmount = (decimal)ntbCountedAmount.Value;
            

            Providers.StatementLineData.Save(PluginEntry.DataModel, statementLine);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
