using System;
using System.Globalization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Customers
{
    public class CustomerGroup : DataEntity, ICloneable
    {
        public enum PeriodEnum
        {
            Day,
            Week,
            Month,
            Year
        }

        public GroupCategory Category { get; set; }
        public bool Exclusive { get; set; }
        public decimal MaxDiscountedPurchases { get; set; }
        public bool DefaultGroup { get; set; }
        public bool UsesDiscountedPurchaseLimit { get; set; }
        public PeriodEnum Period { get; set; }


        public bool Empty
        {
            get
            {
                return ID == RecordIdentifier.Empty || ID.StringValue == "";
            }
        }

        public CustomerGroup()
        {
            Clear();
        }

        public void Clear()
        {
            ID = RecordIdentifier.Empty; ;
            Text = "";
            Category = new GroupCategory();
            Exclusive = false;
            MaxDiscountedPurchases = decimal.Zero;
            DefaultGroup = false;
            UsesDiscountedPurchaseLimit = true;
        }

        public DateTime CurrentPeriodStart
        {
            get
            {
                switch (Period)
                {
                    case CustomerGroup.PeriodEnum.Day:
                        return DateTime.Today;
                    case CustomerGroup.PeriodEnum.Week:
                        CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                        DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
                        DateTime firstDayOfCurrenWeek = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - firstDayOfWeek));
                        return firstDayOfCurrenWeek;
                    case CustomerGroup.PeriodEnum.Month:
                        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    case CustomerGroup.PeriodEnum.Year:
                        return new DateTime(DateTime.Now.Year, 1, 1);                        
                    default:
                        return DateTime.Now;
                }
            }
        }

        public DateTime CurrentPeriodEnd
        {
            get
            {
                switch (Period)
                {
                    case PeriodEnum.Day:
                        return CurrentPeriodStart.AddDays(1);
                    case PeriodEnum.Week:
                        return CurrentPeriodStart.AddDays(7);
                    case PeriodEnum.Month:
                        return CurrentPeriodStart.AddMonths(1);
                    case PeriodEnum.Year:
                        return CurrentPeriodStart.AddYears(1);
                    default:
                        return DateTime.Now;
                } 
            }
        }

        public string PeriodString
        {
            get
            {
                switch (Period)
                {
                    case PeriodEnum.Day:
                        return Properties.Resources.Day;
                    case PeriodEnum.Week:
                        return Properties.Resources.Week;
                    case PeriodEnum.Month:
                        return Properties.Resources.Month;
                    case PeriodEnum.Year:
                        return Properties.Resources.Year;
                    default:
                        return "";
                }
            }
        }
    }
}
