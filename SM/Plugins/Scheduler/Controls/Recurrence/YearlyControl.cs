using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class YearlyControl : DetailBaseControl
    {
        private YearlySetting setting;
        private bool allowInvalidate;

        public YearlyControl()
        {
            InitializeComponent();

            PopulateMonths(cmbMonth);
            PopulateWeekdaySequence(cmbWeekdaySequence);
            PopulateWeekdayCharms(cmbWeekdayCharms);
            PopulateMonths(cmbWeekdayMonth);
        }

        private void RecurrenceDetailYearlyControl_Load(object sender, EventArgs e)
        {
            ApplySettings();
        }



        private void tbDay_Validating(object sender, CancelEventArgs e)
        {
            ValidatingInteger(tbDay, 1, Properties.Resources.Recurrence_DayPositiveIntegerMsg, e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (rbOnDayOfMonth.Checked)
            {
                string msg = string.Empty;
                int day = int.Parse(tbDay.Text);
                int month = ((MonthSelector)cmbMonth.SelectedItem).Month;

                // Note we only support the minimum amount of days in month, ignoring leap year on purpose.
                if (day > DateTime.DaysInMonth(2010, month))
                {
                    msg = Properties.Resources.Recurrence_InvalidDayOfMonth;
                }

                errorProvider.SetError(tbDay, msg);
                e.Cancel = msg.Length > 0;
            }
        }
        
        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbOnDayOfMonth);
        }

        private void tbDay_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbOnDayOfMonth);
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

        private void cmbWeekdayMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbWeekdayOfMonth);
        }


        private void InvalidateSetting()
        {
            if (allowInvalidate && Created)
            {
                setting = null;
                OnSettingChanged();
            }
        }

        private void rbOnDayOfMonth_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void rbWeekdayOfMonth_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }


        public override SettingBase GetSetting()
        {
            return Setting;
        }


        public override void SetSetting(SettingBase setting)
        {
            Setting = (YearlySetting)setting;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public YearlySetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new YearlySetting();
                    setting.UseDayInMonth = rbOnDayOfMonth.Checked;
                    if (rbOnDayOfMonth.Checked)
                        setting.Month = ((MonthSelector)cmbMonth.SelectedItem).Month;
                    setting.Day = int.Parse(tbDay.Text);
                    setting.WeekdaySequence = (WeekdaySequence)cmbWeekdaySequence.SelectedItem;
                    setting.WeekdayCharm = (WeekdayCharm)cmbWeekdayCharms.SelectedItem;
                    if (rbWeekdayOfMonth.Checked)
                        setting.Month = ((MonthSelector)cmbWeekdayMonth.SelectedItem).Month;

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
                rbOnDayOfMonth.Checked = setting.UseDayInMonth;
                cmbMonth.SelectedIndex = IndexOfMonthSelector(cmbMonth, setting.Month);
                tbDay.Text = GetDayOfMonthOrDefault(setting.Day, now);
                rbWeekdayOfMonth.Checked = !setting.UseDayInMonth;
                cmbWeekdaySequence.SelectedItem = GetWeekdaySequenceOrDefault(setting.WeekdaySequence, now);
                cmbWeekdayCharms.SelectedItem = GetWeekdayCharmOrDefault(setting.WeekdayCharm, now);
                cmbWeekdayMonth.SelectedIndex = IndexOfMonthSelector(cmbMonth, setting.Month);
            }
            else
            {
                rbOnDayOfMonth.Checked = true;
                cmbMonth.SelectedIndex = IndexOfMonthSelector(cmbMonth, now.Month);
                tbDay.Text = now.Day.ToString();
                rbWeekdayOfMonth.Checked = false;
                cmbWeekdaySequence.SelectedItem = WeekdaySequence.FromDay(now.Day);
                cmbWeekdayCharms.SelectedItem = WeekdayCharm.FromDayOfWeek(now.DayOfWeek);
                cmbWeekdayMonth.SelectedIndex = IndexOfMonthSelector(cmbMonth, now.Month);
            }
            AllowAutoCheck = true;
            allowInvalidate = true;
        }

        private int IndexOfMonthSelector(ComboBox comboBox, int month)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                var monthSelector = (MonthSelector)comboBox.Items[i];
                if (monthSelector.Month == month)
                    return i;
            }

            return -1;
        }

    }
}
