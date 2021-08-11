using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class WeeklyControl : DetailBaseControl
    {
        private CultureInfo lastShownCulture;
        private CheckBox[] dayCheckBoxes;

        private WeeklySetting setting;
        private bool allowInvalidate;

        public WeeklyControl()
        {
            InitializeComponent();
            dayCheckBoxes = new CheckBox[]
            {
                cbWeekday0,
                cbWeekday1,
                cbWeekday2,
                cbWeekday3,
                cbWeekday4,
                cbWeekday5,
                cbWeekday6,
            };

            // Set each checkbox's Tag property to its DayOfWeek
            DayOfWeek[] daysOfWeek = (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek));
            int currentDayIndex = 1;    // Start on monday
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                DayOfWeek dayOfWeek = daysOfWeek[currentDayIndex];
                dayCheckBoxes[i].Tag = dayOfWeek;
                dayCheckBoxes[i].CheckedChanged += new EventHandler(RecurrenceDetailWeeklyControl_CheckedChanged);
                currentDayIndex = (currentDayIndex + 1) % daysOfWeek.Length;
            }

        }


        private void RecurrenceDetailWeeklyControl_Load(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void RecurrenceDetailWeeklyControl_Paint(object sender, PaintEventArgs e)
        {
            if (lastShownCulture != Thread.CurrentThread.CurrentCulture)
            {
                PopulateWeekdays();
                lastShownCulture = Thread.CurrentThread.CurrentCulture;
            }
        }

        private void PopulateWeekdays()
        {
            foreach (var checkBox in dayCheckBoxes)
            {
                checkBox.Text = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName((DayOfWeek)checkBox.Tag);
            }
        }


        private DayOfWeek[] GetDaysOfWeek()
        {
            List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
            foreach (var checkBox in dayCheckBoxes)
            {
                if (checkBox.Checked)
                {
                    daysOfWeek.Add((DayOfWeek)checkBox.Tag);
                }
            }

            return daysOfWeek.ToArray();
        }


        private void SetDaysOfWeek(DayOfWeek[] daysOfWeek)
        {
            foreach (var checkBox in dayCheckBoxes)
            {
                DayOfWeek boxDayOfWeek = (DayOfWeek)checkBox.Tag;
                checkBox.Checked =
                    daysOfWeek.Any(dow => dow == boxDayOfWeek);
            }
        }

        private void SetDaysOfWeekToday()
        {
            SetDaysOfWeek(new DayOfWeek[] { DateTime.Now.DayOfWeek });
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            // Validate day of week, make sure at least one is checked
            string msg = string.Empty;
            if (!dayCheckBoxes.Any(checkBox => checkBox.Checked))
            {
                msg = Properties.Resources.Recurrence_MustSelectDayMsg;
                e.Cancel = true;
            }

            errorProvider.SetError(dayCheckBoxes[0], msg);
        }


        public override SettingBase GetSetting()
        {
            return Setting;
        }

        public override void SetSetting(SettingBase setting)
        {
            Setting = (WeeklySetting)setting;
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WeeklySetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new WeeklySetting();
                    setting.DaysOfWeek = GetDaysOfWeek();
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
                SetDaysOfWeek(setting.DaysOfWeek);
            }
            else
            {
                SetDaysOfWeekToday();
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

        private void RecurrenceDetailWeeklyControl_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

    }
}
