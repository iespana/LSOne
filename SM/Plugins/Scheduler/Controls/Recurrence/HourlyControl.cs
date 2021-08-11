using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class HourlyControl : DetailBaseControl
    {
        private HourlySetting setting;
        private bool allowInvalidate;
        private bool retainError;
        public HourlyControl()
        {
            InitializeComponent();
            errorProvider.Clear();
            if (cmbHoursFrom.SelectedItem == null || cmbHoursFrom.SelectedItem.ToString() == "")
            {
                cmbHoursFrom.SelectedIndex = 0;
            }
            if (cmbHoursTo.SelectedItem == null || cmbHoursTo.SelectedItem.ToString() == "")
            {
                cmbHoursTo.SelectedIndex = 0;
            }
        }

        private void RecurrenceDetailHourlyControl_Load(object sender, EventArgs e)
        {
            ApplySettings();
        }


        private void tbHours_Validating(object sender, CancelEventArgs e)
        {
            ValidatingInteger(tbHours, 1, Properties.Resources.Recurrence_HoursPositiveIntegerMsg, e);
        }

        private void tbMinutes_Validating(object sender, CancelEventArgs e)
        {
            ValidatingInteger(tbMinutes, 1, Properties.Resources.Recurrence_MinutesPositiveIntegerMsg, e);
        }


        private void rbEveryHour_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void tbHours_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbEveryHour);
        }

        private void rbEveryMinute_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void tbMinutes_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbEveryMinute);
        }


        private void InvalidateSetting()
        {
            if (allowInvalidate && Created)
            {
                setting = null;
                OnSettingChanged();
            }
        }


        public override SettingBase GetSetting()
        {
            return Setting;
        }


        public override void SetSetting(SettingBase setting)
        {
            Setting = (HourlySetting)setting;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HourlySetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new HourlySetting();
                    setting.EveryHour = rbEveryHour.Checked;
                    setting.Hours = int.Parse(tbHours.Text);
                    setting.Minutes = int.Parse(tbMinutes.Text);
                    setting.HoursFrom = int.Parse(cmbHoursFrom.SelectedItem.ToString());
                    setting.HoursTo = int.Parse(cmbHoursTo.SelectedItem.ToString());
                    
                }
                return setting;
            }
            set
            {
                setting = value;
                if (Created)
                {
                    ApplySettings();
                }
            }
        }



        private void ApplySettings()
        {
            AllowAutoCheck = false;
            allowInvalidate = false;
            if (setting != null)
            {
                if (setting.HoursFrom != setting.HoursTo)
                {

                    cmbHoursFrom.SelectedItem = setting.HoursFrom < 10 ? "0" + setting.HoursFrom : setting.HoursFrom.ToString();

                    cmbHoursTo.SelectedItem = setting.HoursTo < 10 ? "0" + setting.HoursTo : setting.HoursTo.ToString();
                }

                rbEveryHour.Checked = setting.EveryHour;
                rbEveryMinute.Checked = !setting.EveryHour;
                if (setting.Hours >= 1)
                    tbHours.Text = setting.Hours.ToString();
                else
                    tbHours.Text = "1";
                
                if (setting.Minutes >= 1)
                    tbMinutes.Text = setting.Minutes.ToString();
                else
                    tbMinutes.Text = "1";
            }
            else
            {
                rbEveryHour.Checked = true;
                tbHours.Text = "1";
                tbMinutes.Text = "15";
            }
            AllowAutoCheck = true;
            allowInvalidate = true;
        }

        private void cmbHoursFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidatePeriod())
            {
                InvalidateSetting();
                if (!retainError)
                    errorProvider.Clear();
                retainError = false;
            }
            else
            {
                errorProvider.SetError(cmbHoursTo, "Hours from must be lower than hours to or 00!");
                retainError = true;
                cmbHoursFrom.SelectedItem = "00";
            }
        }

        private void cmbHoursTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidatePeriod())
            {
                InvalidateSetting();
                if (!retainError)
                    errorProvider.Clear();
                retainError = false;
            }
            else
            {
                errorProvider.SetError(cmbHoursTo, "Hours to  must be 00 or higher then hours from!");
                retainError = true;
                cmbHoursTo.SelectedItem = "00";
            }
        }

        private bool ValidatePeriod()
        {
            if (cmbHoursFrom != null && cmbHoursTo != null)
            {
                if (int.Parse((string)cmbHoursFrom.SelectedItem ?? "0") >= (int.Parse((string)cmbHoursTo.SelectedItem ?? "0"))
                    && (string)cmbHoursTo.SelectedItem != "00" && cmbHoursTo.SelectedItem != null)
                {
                   
                    return false;
                }
            }
            
            
            return true;
        }

    }
}
