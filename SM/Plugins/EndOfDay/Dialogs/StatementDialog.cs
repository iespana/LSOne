using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Statements;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.EndOfDay.Properties;

namespace LSOne.ViewPlugins.EndOfDay.Dialogs
{
    public partial class StatementDialog : DialogBase
    {
        private StatementInfo statement;
        private static RecordIdentifier storeID;

        public StatementDialog(RecordIdentifier statementID, RecordIdentifier storeId, string storeName)
            : this()
        {
            statement = Providers.StatementInfoData.Get(PluginEntry.DataModel, statementID);
            storeID = storeId;

            cmbStore.SelectedData = new DataEntity(storeID, storeName);

            rbFromManual.TabStop = true;
            rbFromPeriods.TabStop = true;
            rbToManual.TabStop = true;
            rbToPeriods.TabStop = true;

            // we do this becouse radiobuttons.Checked is also determined by tab order
            int tabIndex = rbFromManual.TabIndex;
            rbFromManual.TabIndex = rbFromPeriods.TabIndex;
            rbFromPeriods.TabIndex = tabIndex;


            if (statement.FromType == StatementPeriodFormEnum.Manual)
            {
                rbFromManual.Checked = true;
                rbFromPeriods.Checked = false;
            }
            else
            {
                cmbSince1.SelectedIndex = (int)statement.FromPeriodType;
                cmbSince2.SelectedData = new DataEntity(statement.StartingTime, statement.StartingTime.ToString());
            }
            if (statement.ToType == StatementPeriodFormEnum.Manual)
            {
                rbToManual.Checked = true;
                rbToPeriods.Checked = false;
            }
            else
            {
                cmbSince3.SelectedIndex = (int)statement.ToPeriodType;
                cmbSince4.SelectedData = new DataEntity(statement.EndingTime, statement.EndingTime.ToString());
            }

            dtpStartingTime.Value = statement.StartingTime;
            dtpStartingDate.Value = statement.StartingTime;
            dtpEndingTime.Value = statement.EndingTime;
            dtpEndingDate.Value = statement.EndingTime;
        }


        public StatementDialog(RecordIdentifier storeId, string storeName)
            : this()
        {
            statement = null;
            storeID = storeId;
            cmbStore.SelectedData = new DataEntity(storeID,storeName);

            rbFromManual.TabStop = true;
            rbFromPeriods.TabStop = true;
            rbToManual.TabStop = true;
            rbToPeriods.TabStop = true;
            cmbSince1.SelectedIndex = (int)StatementPeriodTypeEnum.LastStatementEnd;
            cmbSince3.SelectedIndex = (int)StatementPeriodTypeEnum.EndOfDay;

            CheckEnabled(null, EventArgs.Empty);
        }

        private StatementDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
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
            DateTime startingTime;
            DateTime endingTime; 
            
            if (statement == null)
            {
                // statement is null if we are creating a new statement
                statement = new StatementInfo();
            }

            statement.StoreID = (string)cmbStore.SelectedData.ID;
            statement.PostingDate = Date.Now;

            if (rbFromManual.Checked)
            {
                statement.FromType = StatementPeriodFormEnum.Manual;

                startingTime = DateTimeFromTimeAndDateControls(dtpStartingDate, dtpStartingTime);
            }
            else
            {
                statement.FromType = StatementPeriodFormEnum.Period;
                statement.FromPeriodType = (StatementPeriodTypeEnum)cmbSince1.SelectedIndex;

                startingTime = (DateTime)cmbSince2.SelectedData.ID;
            }
            if (rbToManual.Checked)
            {
                statement.ToType = StatementPeriodFormEnum.Manual;

                endingTime = DateTimeFromTimeAndDateControls(dtpEndingDate, dtpEndingTime);
            }
            else
            {
                statement.ToType = StatementPeriodFormEnum.Period;
                statement.ToPeriodType = (StatementPeriodTypeEnum)cmbSince3.SelectedIndex;

                endingTime = (DateTime)cmbSince4.SelectedData.ID;
            }

            statement.StartingTime = startingTime;
            statement.EndingTime = endingTime;

            if (startingTime >= endingTime)
            {
                MessageDialog.Show(Properties.Resources.StartingTimeCannotBeBigger);
                return;
            }

            Providers.StatementInfoData.Save(PluginEntry.DataModel, statement);

            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.TodaysStatementsForHeadOfficeID);

            DialogResult = DialogResult.OK;
            Close();
        }

        public RecordIdentifier GetSelectedId()
        {
            return statement.ID;
        }

        private void FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData.ID != "")
            {
                e.TextToDisplay = ((DualDataComboBox)sender).SelectedData.ID + " - " + ((DualDataComboBox)sender).SelectedData.Text;
            }
            else
            {
                e.TextToDisplay = "";
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            rbFromManual.TabStop = true;
            rbFromPeriods.TabStop = true;
            rbToManual.TabStop = true;
            rbToPeriods.TabStop = true;

            if (statement != null)
            {
                bool enabled = true;
                DateTime startingTime = new DateTime();
                DateTime endingTime = new DateTime();

                if (rbFromPeriods.Checked)
                {
                    enabled = cmbSince2.SelectedData != null && cmbSince2.SelectedData.ID != 0;

                    if (enabled)
                    {
                        startingTime = (cmbSince2.SelectedData != null ? (DateTime)cmbSince2.SelectedData.ID : new DateTime());
                    }
                }
                else
                {
                    startingTime = DateTimeFromTimeAndDateControls(dtpStartingDate, dtpStartingTime);
                }
                if (rbToPeriods.Checked)
                {
                    enabled = enabled && cmbSince4.SelectedData != null && cmbSince4.SelectedData.ID != 0;
                    if (enabled)
                    {
                        endingTime = (cmbSince4.SelectedData != null ? (DateTime)cmbSince4.SelectedData.ID : new DateTime());
                    }
                }
                else
                {
                    endingTime = DateTimeFromTimeAndDateControls(dtpEndingDate, dtpEndingTime);
                }
                enabled = enabled &&
                          startingTime != endingTime && 
                          (startingTime != statement.StartingTime || endingTime != statement.EndingTime);


                btnOK.Enabled = enabled;
            }
            else
            {
                bool enabled = true;
                if (rbFromPeriods.Checked)
                {
                    enabled = cmbSince2.SelectedData != null && cmbSince2.SelectedData.ID != 0;
                }
                if (rbToPeriods.Checked)
	            {
		            enabled = enabled && cmbSince4.SelectedData != null && cmbSince4.SelectedData.ID != 0;
	            }
                if (rbToManual.Checked && rbFromManual.Checked)
                {
                    DateTime startingTime = DateTimeFromTimeAndDateControls(dtpStartingDate, dtpStartingTime);
                    DateTime endingTime = DateTimeFromTimeAndDateControls(dtpEndingDate, dtpEndingTime);

                    enabled = enabled && startingTime != endingTime;
                }
                btnOK.Enabled = enabled;
            }
        }

        private DateTime DateTimeFromTimeAndDateControls(DateTimePicker dateControl, DateTimePicker timeControl)
        {
            int seconds = timeControl.Value.Second;
            int minutes = timeControl.Value.Minute;
            int hours = timeControl.Value.Hour;

            int day = dateControl.Value.Day;
            int month = dateControl.Value.Month;
            int year = dateControl.Value.Year;

            return new DateTime(year, month, day, hours, minutes, seconds);
        }

        private void rbFrom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFromPeriods.Checked)
            {
                label3.Enabled = false;
                label1.Enabled = false;
                dtpStartingDate.Enabled = false;
                dtpStartingTime.Enabled = false;
                label2.Enabled = true;
                cmbSince1.Enabled = true;
                cmbSince2.Enabled = cmbSince1.SelectedIndex != (int)StatementPeriodTypeEnum.LastStatementEnd;
            }
            else
            {
                label3.Enabled = true;
                label1.Enabled = true;
                dtpStartingDate.Enabled = true;
                dtpStartingTime.Enabled = true;
                label2.Enabled = false;
                cmbSince1.Enabled = false;
                cmbSince2.Enabled = false;
            }
            CheckEnabled(sender, e);
        }

        private void rbTo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbToPeriods.Checked)
            {
                label6.Enabled = false;
                label7.Enabled = false;
                dtpEndingDate.Enabled = false;
                dtpEndingTime.Enabled = false;
                label8.Enabled = true;
                cmbSince3.Enabled = true;
                cmbSince4.Enabled = true;
            }
            else
            {
                label6.Enabled = true;
                label7.Enabled = true;
                dtpEndingDate.Enabled = true;
                dtpEndingTime.Enabled = true;
                label8.Enabled = false;
                cmbSince3.Enabled = false;
                cmbSince4.Enabled = false;
            }
            CheckEnabled(sender, e);
        }

        private void cmbSince2_RequestData(object sender, EventArgs e)
        {
            if ((StatementPeriodTypeEnum)cmbSince1.SelectedIndex != StatementPeriodTypeEnum.LastStatementEnd)
	        {
                cmbSince2.SetData(GetDateTimeForTransactionType((StatementPeriodTypeEnum)cmbSince1.SelectedIndex), null);
	        }        
        }

        private void cmbSince4_RequestData(object sender, EventArgs e)
        {
            cmbSince4.SetData(GetDateTimeForTransactionType((StatementPeriodTypeEnum)cmbSince3.SelectedIndex), null);
        }

        private void cmbSince1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DataEntity> endOfDayTransactionsFrom = GetDateTimeForTransactionType((StatementPeriodTypeEnum)cmbSince3.SelectedIndex);
            List<StatementInfo> oldStatements = Providers.StatementInfoData.GetAllStatements(PluginEntry.DataModel, storeID, StatementInfoSorting.EndingTime, true);
            switch ((StatementPeriodTypeEnum)cmbSince1.SelectedIndex)
            {
                case StatementPeriodTypeEnum.EndOfDay:
                case StatementPeriodTypeEnum.EndOfShift:
                    cmbSince2.SelectedData = new DataEntity(0, "");
                    DataEntity timeEndOf = endOfDayTransactionsFrom.Count > 0 ? new DataEntity(endOfDayTransactionsFrom[0].ID, endOfDayTransactionsFrom[0].ToString()) : new DataEntity(0, Resources.NoTransactionFound);
                    cmbSince2.SelectedData = timeEndOf;
                    cmbSince2.Enabled = endOfDayTransactionsFrom.Count > 0;
                    break;
                case StatementPeriodTypeEnum.LastStatementEnd:
                    DataEntity time = (oldStatements.Count > 0 ? new DataEntity(((StatementInfo)oldStatements[0]).EndingTime, ((StatementInfo)oldStatements[0]).EndingTime.ToString()) : new DataEntity(0, Properties.Resources.NoStatementFound));
                    cmbSince2.SelectedData = time;
                    cmbSince2.Enabled = false;
                    break;
            }
        }

        private void cmbSince3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DataEntity>  endOfDayTransactionsTo = GetDateTimeForTransactionType((StatementPeriodTypeEnum)cmbSince3.SelectedIndex);
            cmbSince4.SelectedData = new DataEntity(0, "");
            DataEntity time = endOfDayTransactionsTo.Count > 0 ? new DataEntity(endOfDayTransactionsTo[0].ID, endOfDayTransactionsTo[0].ToString()) : new DataEntity(0, Resources.NoTransactionFound);
            cmbSince4.SelectedData = time;
            cmbSince4.Enabled = endOfDayTransactionsTo.Count > 0;
        }

        private static List<DataEntity> GetDateTimeForTransactionType(StatementPeriodTypeEnum type)
        {
            TypeOfTransaction transactionType = TypeOfTransaction.EndOfDay;
            if (type == StatementPeriodTypeEnum.EndOfShift)
            {
                transactionType = TypeOfTransaction.EndOfShift;
            }
            List<DataEntity> dateTime = new List<DataEntity>();
            List<Transaction> transactions = Providers.TransactionData.GetTransactionsFromType(PluginEntry.DataModel, storeID, transactionType, true);
            
            foreach (Transaction transaction in transactions)
	        {
                dateTime.Add(new DataEntity(transaction.TransactionDate.DateTime, transaction.TransactionDate.DateTime.ToString()));
	        }

            return dateTime;
        }
    }
}
