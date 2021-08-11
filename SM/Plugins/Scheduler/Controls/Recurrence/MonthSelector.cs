using System.Threading;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal class MonthSelector
    {
        public MonthSelector(int month)
        {
            Month = month;
        }

        public int Month { get; private set; }

        public override string ToString()
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        }
    }
}
