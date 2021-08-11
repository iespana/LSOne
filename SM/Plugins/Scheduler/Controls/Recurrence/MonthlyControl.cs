using System;
using System.ComponentModel;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class MonthlyControl : DetailBaseControl
    {

        private MonthlySetting setting;
        private bool allowInvalidate;

        public MonthlyControl()
        {
            InitializeComponent();

            PopulateWeekdaySequence(cmbWeekdaySequence);
            PopulateWeekdayCharms(cmbWeekdayCharms);
        }


        private void RecurrenceDetailMonthlyControl_Load(object sender, EventArgs e)
        {
            ApplySettings();
        }


        private void tbDays_Validating(object sender, CancelEventArgs e)
        {
            if (rbDayOfMonth.Checked)
            {
                ValidatingInteger(tbDays, 1, Properties.Resources.Recurrence_DayPositiveIntegerMsg, e);
            }
        }

        private void tbMonth_Validating(object sender, CancelEventArgs e)
        {
            if (rbDayOfMonth.Checked)
            {
                ValidatingInteger(tbMonth, 1, Properties.Resources.Recurrence_MonthsPositiveIntegerMsg, e);
            }
        }

        private void tbWeekMonth_Validating(object sender, CancelEventArgs e)
        {
            if (rbWeekdayOfMonth.Checked)
            {
                ValidatingInteger(tbWeekMonth, 1, Properties.Resources.Recurrence_MonthsPositiveIntegerMsg, e);
            }
        }


        private void tbWeekMonth_Validated(object sender, EventArgs e)
        {
            AllowAutoCheck = false;
            tbMonth.Text = tbWeekMonth.Text;
            AllowAutoCheck = true;
        }

        private void tbMonth_Validated(object sender, EventArgs e)
        {
            AllowAutoCheck = false;
            tbWeekMonth.Text = tbMonth.Text;
            AllowAutoCheck = true;
        }

        private void tbDays_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbDayOfMonth);
        }

        private void tbMonth_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbDayOfMonth);
        }

        private void cmbWeekdaySequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbWeekdayOfMonth);
        }

        private void cmbWeekdayCharms_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbWeekdayOfMonth);
        }

        private void tbWeekMonth_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbWeekdayOfMonth);
        }

        public override SettingBase GetSetting()
        {
            return Setting;
        }


        public override void SetSetting(SettingBase setting)
        {
            Setting = (MonthlySetting)setting;
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MonthlySetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new MonthlySetting();
                    setting.UseDayOfMonth = rbDayOfMonth.Checked;
                    setting.DayOfMonth = int.Parse(tbDays.Text);
                    if (rbDayOfMonth.Checked)
                        setting.Month = int.Parse(tbMonth.Text);
                    setting.WeekdaySequence = (WeekdaySequence)cmbWeekdaySequence.SelectedItem;
                    setting.WeekdayCharm = (WeekdayCharm)cmbWeekdayCharms.SelectedItem;
                    if (rbWeekdayOfMonth.Checked)
                        setting.Month = int.Parse(tbWeekMonth.Text);
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
            DateTime now = DateTime.Now;

            if (setting != null)
            {
                rbDayOfMonth.Checked = setting.UseDayOfMonth;
                tbDays.Text = GetDayOfMonthOrDefault(setting.DayOfMonth, now);
                tbMonth.Text = setting.Month.ToString();
                rbWeekdayOfMonth.Checked = !setting.UseDayOfMonth;
                cmbWeekdaySequence.SelectedItem = GetWeekdaySequenceOrDefault(setting.WeekdaySequence, now);
                cmbWeekdayCharms.SelectedItem = GetWeekdayCharmOrDefault(setting.WeekdayCharm, now);
                tbWeekMonth.Text = setting.Month.ToString();
            }
            else
            {
                rbDayOfMonth.Checked = true;
                tbDays.Text = now.Day.ToString();
                tbMonth.Text = "1";
                rbWeekdayOfMonth.Checked = false;
                cmbWeekdaySequence.SelectedItem = WeekdaySequence.FromDay(now.Day);
                cmbWeekdayCharms.SelectedItem = WeekdayCharm.FromDayOfWeek(now.DayOfWeek);
                tbWeekMonth.Text = tbMonth.Text;
            }
            AllowAutoCheck = true;
            allowInvalidate = true;
        }

        private void InvalidateSetting()
        {
            if (allowInvalidate && Created)
            {
                setting = null;
                OnSettingChanged();
            }
        }

        private void rbDayOfMonth_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void rbWeekdayOfMonth_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }





    }
}
