using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.GenericConnector.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class InitializeZReportDialog : TouchBaseForm
    {
        private ZReport zReport;
        private IConnectionManager entry;

        private InitializeZReportDialog(IConnectionManager entry)
        {
            InitializeComponent();

            touchDialogBanner1.Location = new Point(1, 1);
            touchDialogBanner1.Width = Width - 2;

            this.entry = entry;
        }

        public InitializeZReportDialog(IConnectionManager entry, ZReport zReport)
            : this(entry)
        {
            this.zReport = zReport;            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ntbGrossAmount.Value = ntbNetAmount.Value = 0;
            ntbNewZReportID.Value = 1;
        }

        private void ntbNewZReportID_ValueChanged(object sender, EventArgs e)
        {
            tbNewZReportIDPreview.Text = GenerateNumberFromSequence("########", (int)ntbNewZReportID.Value);
        }

        private static string GenerateNumberFromSequence(string format, int nextNumber)
        {
            int digitCount = 0;
            int handled = 0;
            string finalResult = "";

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '#')
                {
                    digitCount++;
                }
            }

            string result = nextNumber.ToString();
            int nextNumberLength = result.Length;

            for (int i = 0; i < (digitCount - nextNumberLength); i++)
            {
                result = "0" + result;
            }

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '#')
                {
                    finalResult += result[handled];
                    handled++;
                }
                else
                {
                    finalResult += format[i];
                }
            }

            return finalResult;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ntbGrossAmount.Value < ntbNetAmount.Value)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.GrossAmtLowerThanNetAmt, MessageBoxButtons.OK, MessageDialogType.Generic);                
                return;
            }

            if (ntbGrossAmount.Value == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.EnterGrossAmt, MessageBoxButtons.OK, MessageDialogType.Generic);
                return;
            }

            if (ntbNetAmount.Value == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.EnterNetAmt, MessageBoxButtons.OK, MessageDialogType.Generic);
                return;
            }

            zReport.TotalGrossAmount = (decimal)ntbGrossAmount.Value;
            zReport.TotalNetAmount = (decimal)ntbNetAmount.Value;
            zReport.InitZReportID = (int)ntbNewZReportID.Value;

            Close();
        }

        private void InitializeZReportDialog_Load(object sender, EventArgs e)
        {
            ntbNewZReportID_ValueChanged(this, EventArgs.Empty);
        }
    }
}
