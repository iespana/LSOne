using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class DetailBaseControl : UserControl
    {
        public DetailBaseControl()
        {
            InitializeComponent();
            AllowAutoCheck = true;
        }


        protected bool AllowAutoCheck { get; set; }

        protected void AutoCheck(RadioButton radioButton)
        {
            if (AllowAutoCheck && !radioButton.Checked)
                radioButton.Checked = true;
        }


        protected static void PopulateWeekdayCharms(ComboBox comboBox)
        {
            bool isFirst = true;

            comboBox.Items.Clear();
            foreach (var charm in WeekdayCharm.AllValues)
            {
                if (!isFirst)
                {
                    comboBox.Items.Add(charm);
                }

                isFirst = false;
            }
        }

        protected static void PopulateWeekdaySequence(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            foreach (var sequence in WeekdaySequence.AllValues)
            {
                comboBox.Items.Add(sequence);
            }
        }


        protected static void PopulateMonths(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            int months = Thread.CurrentThread.CurrentUICulture.Calendar.GetMonthsInYear(DateTime.Now.Year);
            for (int month = 1; month <= months; month++)
            {
                comboBox.Items.Add(new MonthSelector(month));
            }
        }


        protected void OnSettingChanged()
        {
            if (SettingChanged != null)
            {
                SettingChanged(this, EventArgs.Empty);
            }
        }



        protected void ValidatingInteger(TextBox textBox, int minValue, string errorMsg, CancelEventArgs e)
        {
            string msg;

            int value;
            if (!int.TryParse(textBox.Text, out value) || value < minValue)
            {
                msg = errorMsg;
                e.Cancel = true;
            }
            else
            {
                msg = string.Empty;
            }
            errorProvider.SetError(textBox, msg);
        }


        protected string GetDayOfMonthOrDefault(int dayOfMonth, DateTime now)
        {
            if (dayOfMonth >= 1 && dayOfMonth <= 31)
            {
                return dayOfMonth.ToString();
            }
            else
            {
                return now.Day.ToString();
            }
        }

        protected WeekdaySequence GetWeekdaySequenceOrDefault(WeekdaySequence weekdaySequence, DateTime now)
        {
            if (weekdaySequence != null)
            {
                return weekdaySequence;
            }
            else
            {
                return WeekdaySequence.FromDay(now.Day);
            }
        }

        protected WeekdayCharm GetWeekdayCharmOrDefault(WeekdayCharm weekdayCharm, DateTime now)
        {
            if (weekdayCharm != null)
            {
                return weekdayCharm;
            }
            else
            {
                return WeekdayCharm.FromDayOfWeek(now.DayOfWeek);
            }
        }




        public virtual SettingBase GetSetting()
        {
            return null;
        }

        public virtual void SetSetting(SettingBase setting)
        {
        }

        public event EventHandler<EventArgs> SettingChanged;


    }
}
