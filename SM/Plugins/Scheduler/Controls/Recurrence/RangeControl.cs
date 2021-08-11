using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal partial class RangeControl : DetailBaseControl
    {
        private RangeSetting setting;
        private bool allowInvalidate;

        public RangeControl()
        {
            InitializeComponent();

            AllowAutoCheck = true;
        }

        private void RecurrenceRangeControl_Load(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbEndDate);
        }

        private void dtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
            AutoCheck(rbEndDate);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (rbEndDate.Checked)
            {
                string msg;

                DateTime startDateTime = GetCombinedDateTime(dtpStartDate.Value, dtpStartTime.Value);
                DateTime endDateTime = GetCombinedDateTime(dtpEndDate.Value, dtpEndTime.Value);

                if (endDateTime.Date < startDateTime.Date)
                {
                    msg = Properties.Resources.Recurrence_StartEndDateMsg;
                    e.Cancel = true;
                }
                else
                {
                    msg = string.Empty;
                }
                errorProvider.SetError(dtpEndDate, msg);

                if (msg.Length == 0 && endDateTime <= startDateTime)
                {
                    msg = Properties.Resources.Recurrence_StartEndTimeMsg;
                    e.Cancel = true;
                }
                else
                {
                    msg = string.Empty;
                }
                errorProvider.SetError(dtpEndTime, msg);
            }
            else
            {
                errorProvider.SetError(dtpEndDate, string.Empty);
                errorProvider.SetError(dtpEndTime, string.Empty);
            }

        }


        private DateTime GetCombinedDateTime(DateTime datePart, DateTime timePart)
        {
            return datePart.Date + timePart.TimeOfDay;
        }


        private void SetCombinedDateTime(DateTime dateTime, DateTimePicker dtpDate, DateTimePicker dtpTime)
        {
            dtpDate.Value = dateTime.Date;
            dtpTime.Value = dateTime;
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
            Setting = (RangeSetting)setting;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RangeSetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new RangeSetting();
                    setting.StartDateTime = GetCombinedDateTime(dtpStartDate.Value, dtpStartTime.Value);
                    if (rbEndDate.Checked)
                    {
                        setting.EndDateTime = GetCombinedDateTime(dtpEndDate.Value, dtpEndTime.Value);
                    }
                    else
                    {
                        setting.EndDateTime = null;
                    }
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
                SetCombinedDateTime(setting.StartDateTime, dtpStartDate, dtpStartTime);

                if (setting.EndDateTime.HasValue)
                {
                    rbEndDate.Checked = true;
                    SetCombinedDateTime(setting.EndDateTime.Value, dtpEndDate, dtpEndTime);
                }
                else
                {
                    SetDefaultEndBy();
                }
            }
            else
            {
                SetCombinedDateTime(DateTime.Now.AddMinutes(15), dtpStartDate, dtpStartTime);
                rbEndNone.Checked = true;
                SetDefaultEndBy();
            }
            AllowAutoCheck = true;
            allowInvalidate = true;
        }


        private void SetDefaultEndBy()
        {
            SetCombinedDateTime(DateTime.Now.AddDays(9), dtpEndDate, dtpEndTime);
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void rbEndNone_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

        private void rbEndDate_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateSetting();
        }

    }
}
