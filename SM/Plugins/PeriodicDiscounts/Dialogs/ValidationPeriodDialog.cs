using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class ValidationPeriodDialog : DialogBase
    {
        private enum WeekdayEnum
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        private class Weekday
        {
            public TimeSpan startingTime = new TimeSpan(0, 0, 0);
            public TimeSpan endingTime = new TimeSpan(0, 0, 0);
            public bool timeDisabled = true;
            public bool timeAlways = false;
            public bool timeNever = false;
            public bool timePeriod = false;
            public bool timeWithinBounds = true;
            public bool endingTimeAfterMidnight = false;
        }

        RecordIdentifier discountId;
        DiscountPeriod discount;

        public ValidationPeriodDialog(RecordIdentifier Id)
            : base()
        {
            InitializeComponent();

            discountId = Id;
            
            discount = Providers.DiscountPeriodData.Get(PluginEntry.DataModel, Id);

            DiscountToGUI();

            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Monday;

        }

        public ValidationPeriodDialog()
            : base()
        {
            InitializeComponent();

            discount = new DiscountPeriod();

            DiscountToGUI();

            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Monday;
        }

        
        private void DiscountToGUI()
        {
            tbDescription.Text = discount.Text;
            discount.StartingDate.ToDateControl(dtpDefaultStartingDate);
            discount.EndingDate.ToDateControl(dtpDefaultEndingDate);
            dtpDefaultStartingTime.Value = new DateTime(2010, 1, 1, discount.StartingTime.Hours, discount.StartingTime.Minutes, discount.StartingTime.Seconds);
            dtpDefaultEndingTime.Value = new DateTime(2010, 1, 1, discount.EndingTime.Hours, discount.EndingTime.Minutes, discount.EndingTime.Seconds);
            chkDefaultTimeWithinBounds.Checked = discount.TimeWithinBounds;
            chkDefaultEndingTimeAfterMidnight.Checked = discount.EndingTimeAfterMidnight;

            TimeSpan startOfDay = new TimeSpan(0, 0, 0);
            TimeSpan endofDay = new TimeSpan(1, 0, 0, 0);

            rbDefaultAlways.Checked = (discount.StartingTime == startOfDay) && (discount.EndingTime == endofDay);
            rbDefaultPeriod.Checked = !rbDefaultAlways.Checked;

            WeekdayToGUI((WeekdayEnum)cmbWeekdayDay.SelectedIndex);
            CompareWeekdaysAndDefault();
        }

        private void GUIToDiscount()
        {
            // Default part
            //----------------------------------------------------------------------------------------
            discount.Text = tbDescription.Text;
            discount.StartingDate = Date.FromDateControl(dtpDefaultStartingDate);
            discount.EndingDate = Date.FromDateControl(dtpDefaultEndingDate);

            if (rbDefaultAlways.Checked)
            {
                discount.StartingTime = new TimeSpan(0, 0, 0);
                discount.EndingTime = new TimeSpan(1, 0, 0, 0);
            }
            else
            {
                discount.EndingTime = dtpDefaultEndingTime.Value.TimeOfDay;
                discount.StartingTime = dtpDefaultStartingTime.Value.TimeOfDay;
            }

            discount.TimeWithinBounds = chkDefaultTimeWithinBounds.Checked;
            discount.EndingTimeAfterMidnight = chkDefaultEndingTimeAfterMidnight.Checked;
            //----------------------------------------------------------------------------------------

            // Weekday part
            //----------------------------------------------------------------------------------------
            WeekdayEnum weekdayNumber = (WeekdayEnum)cmbWeekdayDay.SelectedIndex;
            Weekday weekday = GUIToWeekday();

            WeekdayToDiscount(weekday, weekdayNumber);
            //----------------------------------------------------------------------------------------
        }

        private Weekday WeekdayFromDiscount(WeekdayEnum weekdayNumber)
        {
            Weekday weekDay = new Weekday();

            TimeSpan startOfDay = new TimeSpan(0,0,1);
            TimeSpan endofDay = new TimeSpan(23,59,59);
            TimeSpan neverTime = new TimeSpan(0, 0, 1);
            TimeSpan disabledTime = new TimeSpan(0, 0, 0);

            switch (weekdayNumber)
            {
                case WeekdayEnum.Monday:
                    weekDay.startingTime = discount.MonStartingTime;
                    weekDay.endingTime = discount.MonEndingTime;
                    weekDay.timeWithinBounds = discount.MonTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.MonEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.MonStartingTime == disabledTime) && (discount.MonEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.MonStartingTime == startOfDay) && (discount.MonEndingTime == endofDay);
                    weekDay.timeNever = (discount.MonStartingTime == neverTime) && (discount.MonEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
                case WeekdayEnum.Tuesday:
                    weekDay.startingTime = discount.TueStartingTime;
                    weekDay.endingTime = discount.TueEndingTime;
                    weekDay.timeWithinBounds = discount.TueTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.TueEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.TueStartingTime == disabledTime) && (discount.TueEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.TueStartingTime == startOfDay) && (discount.TueEndingTime == endofDay);
                    weekDay.timeNever = (discount.TueStartingTime == neverTime) && (discount.TueEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
                case WeekdayEnum.Wednesday:
                    weekDay.startingTime = discount.WedStartingTime;
                    weekDay.endingTime = discount.WedEndingTime;
                    weekDay.timeWithinBounds = discount.WedTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.WedEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.WedStartingTime == disabledTime) && (discount.WedEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.WedStartingTime == startOfDay) && (discount.WedEndingTime == endofDay);
                    weekDay.timeNever = (discount.WedStartingTime == neverTime) && (discount.WedEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
                case WeekdayEnum.Thursday:
                    weekDay.startingTime = discount.ThuStartingTime;
                    weekDay.endingTime = discount.ThuEndingTime;
                    weekDay.timeWithinBounds = discount.ThuTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.ThuEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.ThuStartingTime == disabledTime) && (discount.ThuEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.ThuStartingTime == startOfDay) && (discount.ThuEndingTime == endofDay);
                    weekDay.timeNever = (discount.ThuStartingTime == neverTime) && (discount.ThuEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
                case WeekdayEnum.Friday:
                    weekDay.startingTime = discount.FriStartingTime;
                    weekDay.endingTime = discount.FriEndingTime;
                    weekDay.timeWithinBounds = discount.FriTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.FriEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.FriStartingTime == disabledTime) && (discount.FriEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.FriStartingTime == startOfDay) && (discount.FriEndingTime == endofDay);
                    weekDay.timeNever = (discount.FriStartingTime == neverTime) && (discount.FriEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
                case WeekdayEnum.Saturday:
                   weekDay.startingTime = discount.SatStartingTime;
                    weekDay.endingTime = discount.SatEndingTime;
                    weekDay.timeWithinBounds = discount.SatTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.SatEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.SatStartingTime == disabledTime) && (discount.SatEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.SatStartingTime == startOfDay) && (discount.SatEndingTime == endofDay);
                    weekDay.timeNever = (discount.SatStartingTime == neverTime) && (discount.SatEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
                case WeekdayEnum.Sunday:
                    weekDay.startingTime = discount.SunStartingTime;
                    weekDay.endingTime = discount.SunEndingTime;
                    weekDay.timeWithinBounds = discount.SunTimeWithinBounds;
                    weekDay.endingTimeAfterMidnight = discount.SunEndingTimeAfterMidnight;
                    weekDay.timeDisabled = (discount.SunStartingTime == disabledTime) && (discount.SunEndingTime == disabledTime);
                    weekDay.timeAlways = (discount.SunStartingTime == startOfDay) && (discount.SunEndingTime == endofDay);
                    weekDay.timeNever = (discount.SunStartingTime == neverTime) && (discount.SunEndingTime == neverTime);
                    weekDay.timePeriod = !weekDay.timeDisabled && !weekDay.timeAlways && !weekDay.timeNever;
                    break;
            }

            return weekDay;
        }

        private void WeekdayToGUI(WeekdayEnum weekdayNumber)
        {
            Weekday weekDay = WeekdayFromDiscount(weekdayNumber);

            dtpWeekdayStartingTime.Value = DateTimeFromTimeSpan(weekDay.startingTime);
            dtpWeekdayEndingTime.Value = DateTimeFromTimeSpan(weekDay.endingTime);

            rbWeekdayDisabled.Checked = weekDay.timeDisabled;
            rbWeekdayAlways.Checked = weekDay.timeAlways;
            rbWeekdayNever.Checked = weekDay.timeNever;
            rbWeekdayPeriod.Checked = weekDay.timePeriod;

            chkWeekdayTimeWithinBounds.Checked = weekDay.timeWithinBounds;
            chkWeekdayEndingTimeAfterMidnight.Checked = weekDay.endingTimeAfterMidnight;

        }

        private Weekday GUIToWeekday()
        {
            WeekdayEnum weekdayNumber = (WeekdayEnum)cmbWeekdayDay.SelectedIndex;

            Weekday weekday = WeekdayFromDiscount(weekdayNumber);

            if (rbWeekdayDisabled.Checked)
            {
                weekday.startingTime = new TimeSpan(0, 0, 0);
                weekday.endingTime = new TimeSpan(0, 0, 0);
            }
            else if (rbWeekdayAlways.Checked)
            {
                weekday.startingTime = new TimeSpan(0, 0, 1);
                weekday.endingTime = new TimeSpan(23, 59, 59);
            }
            else if (rbWeekdayNever.Checked)
            {
                weekday.startingTime = new TimeSpan(0, 0, 1);
                weekday.endingTime = new TimeSpan(0, 0, 1);
            }
            else
            {
                weekday.startingTime = dtpWeekdayStartingTime.Value.TimeOfDay;
                weekday.endingTime = dtpWeekdayEndingTime.Value.TimeOfDay;
            }

            weekday.endingTimeAfterMidnight = chkWeekdayEndingTimeAfterMidnight.Checked;
            weekday.timeWithinBounds = chkWeekdayTimeWithinBounds.Checked;

            return weekday;
        }

        private void WeekdayToDiscount(Weekday weekday, WeekdayEnum weekdayNumber)
        {
            switch (weekdayNumber)
            {
                case WeekdayEnum.Monday:
                    discount.MonStartingTime = weekday.startingTime;
                    discount.MonEndingTime = weekday.endingTime;
                    discount.MonEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.MonTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                case WeekdayEnum.Tuesday:
                    discount.TueStartingTime = weekday.startingTime;
                    discount.TueEndingTime = weekday.endingTime;
                    discount.TueEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.TueTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                case WeekdayEnum.Wednesday:
                    discount.WedStartingTime = weekday.startingTime;
                    discount.WedEndingTime = weekday.endingTime;
                    discount.WedEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.WedTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                case WeekdayEnum.Thursday:
                    discount.ThuStartingTime = weekday.startingTime;
                    discount.ThuEndingTime = weekday.endingTime;
                    discount.ThuEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.ThuTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                case WeekdayEnum.Friday:
                    discount.FriStartingTime = weekday.startingTime;
                    discount.FriEndingTime = weekday.endingTime;
                    discount.FriEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.FriTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                case WeekdayEnum.Saturday:
                    discount.SatStartingTime = weekday.startingTime;
                    discount.SatEndingTime = weekday.endingTime;
                    discount.SatEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.SatTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                case WeekdayEnum.Sunday:
                    discount.SunStartingTime = weekday.startingTime;
                    discount.SunEndingTime = weekday.endingTime;
                    discount.SunEndingTimeAfterMidnight = weekday.endingTimeAfterMidnight;
                    discount.SunTimeWithinBounds = weekday.timeWithinBounds;
                    break;
                default:
                    break;
            }
        }

        
        // Creates a DateTime starting from 01.01.2010 and adds a TimeSpan to it
        private DateTime DateTimeFromTimeSpan(TimeSpan timeSpan)
        {
            return new DateTime(2010, 1, 1 + timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private bool CheckIfWeekdayTimeIsAlright(Weekday weekday)
        {
            if (weekday.timePeriod)
            {
                if (weekday.endingTimeAfterMidnight)
                {
                    if (weekday.endingTime > weekday.startingTime)
                    {
                        return false;
                    }
                }
                else
                {
                    if (weekday.startingTime > weekday.endingTime)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Returns true if the selected weekday has default settings
        private bool CompareAWeekdayAndDefault(WeekdayEnum weekdayNumber)
        {
            Weekday weekday = WeekdayFromDiscount(weekdayNumber);

            if (weekday.endingTime != new TimeSpan(0, 0, 0)) return false;
            if (weekday.startingTime != new TimeSpan(0, 0, 0)) return false;
            if (weekday.endingTimeAfterMidnight != false) return false;
            if (weekday.timeWithinBounds != true) return false;

            return true;
        }

        // Compares settings in all weekdays with the default settings and sets the GUI checkboxes accordingly
        private void CompareWeekdaysAndDefault()
        {
            Array weekdays = System.Enum.GetValues(typeof(WeekdayEnum));

            foreach (WeekdayEnum day in weekdays)
            {
                bool checkValue = CompareAWeekdayAndDefault(day);

                switch (day)
                {
                    case WeekdayEnum.Monday:
                        chkSpecialSettingsMonday.Checked = !checkValue;
                        break;
                    case WeekdayEnum.Tuesday:
                        chkSpecialSettingsTuesday.Checked = !checkValue;
                        break;
                    case WeekdayEnum.Wednesday:
                        chkSpecialSettingsWednesday.Checked = !checkValue;
                        break;
                    case WeekdayEnum.Thursday:
                        chkSpecialSettingsThursday.Checked = !checkValue;
                        break;
                    case WeekdayEnum.Friday:
                        chkSpecialSettingsFriday.Checked = !checkValue;
                        break;
                    case WeekdayEnum.Saturday:
                        chkSpecialSettingsSaturday.Checked = !checkValue;
                        break;
                    case WeekdayEnum.Sunday:
                        chkSpecialSettingsSunday.Checked = !checkValue;
                        break;
                    default:
                        break;
                }
            }

        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public DataEntity DiscountPeriodEntity
        {
            get
            {
                return new DataEntity(discountId,tbDescription.Text);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Default part
            //------------------------------------------------------------------------------------------------------------------------------
            GUIToDiscount();

            // Check if default time values are ok 
            if (rbDefaultPeriod.Checked)
            {
                if (chkDefaultEndingTimeAfterMidnight.Checked)
                {
                    if (dtpDefaultEndingTime.Value.TimeOfDay > dtpDefaultStartingTime.Value.TimeOfDay)
                    {
                        errorProvider1.SetError(chkDefaultEndingTimeAfterMidnight, Properties.Resources.TimeDoesNotEndAfterMidnight);
                        return;
                    }
                }
                else
                {
                    if (dtpDefaultEndingTime.Value.TimeOfDay < dtpDefaultStartingTime.Value.TimeOfDay)
                    {
                        errorProvider1.SetError(chkDefaultEndingTimeAfterMidnight, Properties.Resources.TimeEndsAfterMidnight);
                        return;
                    }
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------

            // Weekdays part
            //------------------------------------------------------------------------------------------------------------------------------
            Array weekdays = System.Enum.GetValues(typeof(WeekdayEnum));

            foreach (WeekdayEnum day in weekdays)
            {
                Weekday weekday = WeekdayFromDiscount(day);
                bool dayIsAlright = CheckIfWeekdayTimeIsAlright(weekday);

                if (!dayIsAlright)
                {
                    switch (day)
                    {
                        case WeekdayEnum.Monday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Monday;
                            break;
                        case WeekdayEnum.Tuesday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Tuesday;
                            break;
                        case WeekdayEnum.Wednesday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Wednesday;
                            break;
                        case WeekdayEnum.Thursday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Thursday;
                            break;
                        case WeekdayEnum.Friday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Friday;
                            break;
                        case WeekdayEnum.Saturday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Saturday;
                            break;
                        case WeekdayEnum.Sunday:
                            cmbWeekdayDay.SelectedIndex = (int)WeekdayEnum.Sunday;
                            break;
                        default:
                            break;
                    }

                    if (chkWeekdayEndingTimeAfterMidnight.Checked)
                    {
                        errorProvider1.SetError(chkWeekdayEndingTimeAfterMidnight, Properties.Resources.TimeDoesNotEndAfterMidnight);
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(chkWeekdayEndingTimeAfterMidnight, Properties.Resources.TimeEndsAfterMidnight);
                        return;
                    }
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------

            Providers.DiscountPeriodData.Save(PluginEntry.DataModel, discount);

            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PeriodicDiscountDashboardItemID);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbWeekdayDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            WeekdayToGUI((WeekdayEnum)cmbWeekdayDay.SelectedIndex);
        }

        private void rbWeekday_CheckChanged(object sender, EventArgs e)
        {
            

            if (rbWeekdayAlways.Checked)
            {
                dtpWeekdayStartingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 1));
                dtpWeekdayEndingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 0));

                chkWeekdayEndingTimeAfterMidnight.Checked = false;
                chkWeekdayTimeWithinBounds.Checked = true;
            }
            else if (rbWeekdayNever.Checked)
            {
                dtpWeekdayStartingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 1));
                dtpWeekdayEndingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 1));

                chkWeekdayEndingTimeAfterMidnight.Checked = false;
                chkWeekdayTimeWithinBounds.Checked = true;
            }
            else if (rbWeekdayDisabled.Checked)
            {
                dtpWeekdayStartingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 0));
                dtpWeekdayEndingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 0));

                chkWeekdayEndingTimeAfterMidnight.Checked = false;
                chkWeekdayTimeWithinBounds.Checked = true;
            }
            
            
            if (rbWeekdayPeriod.Checked)
            {
                lblWeekdayEndingTime.Enabled = true;
                lblWeekdayStartingTime.Enabled = true;
                lblWeekdayEndingTimeAfterMidnight.Enabled = true;
                lblWeekdayTimeWithinBounds.Enabled = true;
                dtpWeekdayEndingTime.Enabled = true;
                dtpWeekdayStartingTime.Enabled = true;
                chkWeekdayEndingTimeAfterMidnight.Enabled = true;
                chkWeekdayTimeWithinBounds.Enabled = true;
            }
            else
            {
                lblWeekdayEndingTime.Enabled = false;
                lblWeekdayStartingTime.Enabled = false;
                lblWeekdayEndingTimeAfterMidnight.Enabled = false;
                lblWeekdayTimeWithinBounds.Enabled = false;
                dtpWeekdayEndingTime.Enabled = false;
                dtpWeekdayStartingTime.Enabled = false;
                chkWeekdayEndingTimeAfterMidnight.Enabled = false;
                chkWeekdayTimeWithinBounds.Enabled = false;
            }

            GUIToDiscount();
            CompareWeekdaysAndDefault();
            errorProvider1.Clear();
        }

        private void rbDefault_CheckChanged(object sender, EventArgs e)
        {
            if (rbDefaultAlways.Checked)
            {
                //Discount item
                discount.StartingTime = new TimeSpan(0, 0, 0);
                discount.EndingTime = new TimeSpan(1, 0, 0, 0);

                //GUI
                dtpDefaultStartingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 0));
                dtpDefaultEndingTime.Value = DateTimeFromTimeSpan(new TimeSpan(0, 0, 0));
                chkDefaultTimeWithinBounds.Checked = true;
                chkDefaultEndingTimeAfterMidnight.Checked = false;
                lblDefaultEndingTime.Enabled = false;
                lblDefaultStartingTime.Enabled = false;
                lblDefaultEndingTimeAfterMidnight.Enabled = false;
                lblDefaultTimeWithinBounds.Enabled = false;
                dtpDefaultEndingTime.Enabled = false;
                dtpDefaultStartingTime.Enabled = false;
                chkDefaultEndingTimeAfterMidnight.Enabled = false;
                chkDefaultTimeWithinBounds.Enabled = false;

            }
            else if (rbDefaultPeriod.Checked)
            {
                lblDefaultEndingTime.Enabled = true;
                lblDefaultStartingTime.Enabled = true;
                lblDefaultEndingTimeAfterMidnight.Enabled = true;
                lblDefaultTimeWithinBounds.Enabled = true;
                dtpDefaultEndingTime.Enabled = true;
                dtpDefaultStartingTime.Enabled = true;
                chkDefaultEndingTimeAfterMidnight.Enabled = true;
                chkDefaultTimeWithinBounds.Enabled = true;
            }
            else
            {
                lblDefaultEndingTime.Enabled = false;
                lblDefaultStartingTime.Enabled = false;
                lblDefaultEndingTimeAfterMidnight.Enabled = false;
                lblDefaultTimeWithinBounds.Enabled = false;
                dtpDefaultEndingTime.Enabled = false;
                dtpDefaultStartingTime.Enabled = false;
                chkDefaultEndingTimeAfterMidnight.Enabled = false;
                chkDefaultTimeWithinBounds.Enabled = false;
            }
            errorProvider1.Clear();
        }

        private void dtpWeekdayTime_ValueChanged(object sender, EventArgs e)
        {
            GUIToDiscount();
            CompareWeekdaysAndDefault();
            errorProvider1.Clear();
        }

        private void ClearError(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
        
        private void CheckOkBtnEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text != "";
        }

    }
}
