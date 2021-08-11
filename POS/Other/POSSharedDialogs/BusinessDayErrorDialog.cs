using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class BusinessDayErrorDialog : TouchBaseForm
    {
        public DateTime BusinessDay
        {
            set
            {
                lblBusinessDay.Text = Properties.Resources.BusinessDay + ": " + value.ToString("d");
            }
        }

        public BusinessDayErrorDialog()
        {            
            InitializeComponent();

            lblSystemDay.Text = Properties.Resources.SystemDay + ": " + DateTime.Now.ToString("d");
        }

        public void CenterToPOSScreen()
        {
            if (DLLEntry.Settings != null && 
                DLLEntry.Settings.MainFormInfo.MainWindowHCenter > 0 &&
                DLLEntry.Settings.MainFormInfo.MainWindowVCenter > 0)
            {
                StartPosition = FormStartPosition.Manual;

                Point center = new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter,
                                            DLLEntry.Settings.MainFormInfo.MainWindowVCenter);

                Location = new Point(center.X - (Width / 2), center.Y - Height / 2);
            }
            else
            {
                StartPosition = Owner != null ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            }
        }
    }
}
