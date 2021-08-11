using System;
using System.ComponentModel;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class DailyControl : DetailBaseControl
    {
        private DailySetting setting;
        private bool allowInvalidate;

        public DailyControl()
        {
            InitializeComponent();
        }


        private void RecurrenceDetailDailyControl_Load(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void tbDays_Validating(object sender, CancelEventArgs e)
        {
            ValidatingInteger(tbDays, 1, Properties.Resources.Recurrence_DayPositiveIntegerMsg, e);
        }


        private void rbEveryDay_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void rbEveryWeekday_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void tbDays_TextChanged(object sender, EventArgs e)
        {
            InvalidateSetting();

            if (!rbEveryDay.Checked)
                rbEveryDay.Checked = true;
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
            Setting = (DailySetting)setting;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DailySetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new DailySetting();
                    setting.EveryDay = rbEveryDay.Checked;
                    setting.Days = int.Parse(tbDays.Text);
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
                rbEveryDay.Checked = setting.EveryDay;
                rbEveryWeekday.Checked = !setting.EveryDay;
                if (setting.Days >= 1)
                    tbDays.Text = setting.Days.ToString();
                else
                    tbDays.Text = "1";
            }
            else
            {
                rbEveryDay.Checked = true;
                tbDays.Text = "1";
                rbEveryWeekday.Checked = false;
            }
            AllowAutoCheck = true;
            allowInvalidate = true;
        }



    }
}
