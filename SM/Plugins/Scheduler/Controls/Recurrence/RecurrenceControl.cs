using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public partial class RecurrenceControl : UserControl
    {
        private DetailBaseControl currentDetail;
        private RadioButton[] radioButtons;

        public RecurrenceControl()
        {
            InitializeComponent();
            InitializeDetailControls();
            rbDaily.Checked = true;
        }

        private void InitializeDetailControls()
        {
            rbHourly.Tag = CreateDetailControl<HourlyControl>();
            rbDaily.Tag = CreateDetailControl<DailyControl>();
            rbWeekly.Tag = CreateDetailControl<WeeklyControl>();
            rbMonthly.Tag = CreateDetailControl<MonthlyControl>();
            rbYearly.Tag = CreateDetailControl<YearlyControl>();

            radioButtons = new RadioButton[]
            {
                rbHourly,
                rbDaily,
                rbWeekly,
                rbMonthly,
                rbYearly,
            };
        }

        private T CreateDetailControl<T>() where T : DetailBaseControl, new()
        {
            T control = new T();
            control.Visible = false;
            control.Dock = DockStyle.Fill;
            control.SettingChanged += new EventHandler<EventArgs>(DetailControlSettingChanged);
            this.pnlDetails.Controls.Add(control);

            return control;
        }

        private void DetailControlSettingChanged(object sender, EventArgs e)
        {
            OnValueChanged();
        }

        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            SetVisibleDetail((RadioButton)sender);
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }


        private void SetVisibleDetail(RadioButton radioButton)
        {
            if (currentDetail != null)
            {
                currentDetail.Hide();
            }
            currentDetail = (DetailBaseControl)(radioButton.Tag);
            if (currentDetail != null)
            {
                currentDetail.Show();
                currentDetail.BringToFront();
            }
        }


        private RadioButton GetRadioButtonBySetting(SettingBase setting)
        {
            Type controlType = SettingFactory.GetControlTypeBySettingType(setting.GetType());
            for (int i = 0; i < radioButtons.Length; i++)
            {
                if (radioButtons[i].Tag.GetType() == controlType)
                {
                    return radioButtons[i];
                }
            }

            throw new NotImplementedException("No radio button has this settings type " + setting.GetType().FullName);
        }

        public CronSchedule Value
        {
            get
            {
                if (currentDetail == null)
                {
                    return null;
                }

                SettingBase rangeSetting = rangeControl.GetSetting();
                CronSchedule cron = new CronSchedule();
                rangeSetting.ToCronSchedule(cron);
                SettingBase detailSetting = currentDetail.GetSetting();
                detailSetting.ToCronSchedule(cron);
                if (detailSetting is HourlySetting)
                {
                    cron.RecurrenceType = RecurrenceType.Hourly;
                }
                else if (detailSetting is DailySetting)
                {
                    cron.RecurrenceType = RecurrenceType.Daily;
                }
                else if (detailSetting is WeeklySetting)
                {
                    cron.RecurrenceType = RecurrenceType.Weekly;
                }
                else if (detailSetting is MonthlySetting)
                {
                    cron.RecurrenceType = RecurrenceType.Monthly;
                }
                else
                {
                    cron.RecurrenceType = RecurrenceType.Yearly;
                }
                return cron;
            }

            set
            {
                RangeSetting rangeSetting = null;
                SettingBase detailSetting = null;

                if (value != null)
                {
                    rangeSetting = new RangeSetting
                    {
                        StartDateTime = value.StartDateTime,
                        EndDateTime = value.EndDateTime,
                    };

                    switch (value.RecurrenceType)
                    {
                        case RecurrenceType.Hourly:
                            detailSetting = HourlySetting.FromCronSchedule(value);
                            break;
                        case RecurrenceType.Daily:
                            detailSetting = DailySetting.FromCronSchedule(value);
                            break;
                        case RecurrenceType.Weekly:
                            detailSetting = WeeklySetting.FromCronSchedule(value);
                            break;
                        case RecurrenceType.Monthly:
                            detailSetting = MonthlySetting.FromCronSchedule(value);
                            break;
                        case RecurrenceType.Yearly:
                            detailSetting = YearlySetting.FromCronSchedule(value);
                            break;
                        default:
                            detailSetting = new HourlySetting();
                            break;
                        //throw new ArgumentException("Setting a CronSchedule with RecurrenceType == " + value.RecurrenceType.ToString() + " is invalid.");
                    }

                }
                else
                {
                    detailSetting = new HourlySetting();
                }

                if (Created)
                {
                    rangeControl.Setting = rangeSetting;
                    RadioButton radioButton = GetRadioButtonBySetting(detailSetting);
                    radioButton.Checked = true;
                    ((DetailBaseControl)radioButton.Tag).SetSetting(detailSetting);
                }
            }
        }

        public event EventHandler<EventArgs> ValueChanged;

        private void pnlDetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }

}
