using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.CustomerOrders
{

    /// <summary>
    /// A business object that holds all settings for a type of customer orders
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public partial class CustomerOrderSettings : DataEntity
    {
        public CustomerOrderSettings()
        {
            ID = Guid.Empty;
            AcceptsDeposits = false;
            MinimumDeposits = 0;
            SelectSource = false;
            SelectDelivery = false;
            NumberSeries = RecordIdentifier.Empty;
            ExpireTimeValue = 1;
            ExpirationTimeUnit = TimeUnitEnum.Month;
            SettingsType = CustomerOrderType.CustomerOrder;
        }

        public bool Empty()
        {
            return ID == Guid.Empty;
        }

        /// <summary>
        /// Uses the settings to create and return an expiration date. If <see cref="ExpirationTimeUnit"/> is None then todays date is returned
        /// </summary>
        /// <returns></returns>
        public DateTime ExpirationDate()
        {
            DateTime date = DateTime.Now;

            switch (ExpirationTimeUnit)
            {
                case TimeUnitEnum.Day:
                    date = date.AddDays(ExpireTimeValue);
                    break;
                case TimeUnitEnum.Week:
                    date = date.AddDays(ExpireTimeValue * 7);
                    break;
                case TimeUnitEnum.Month:
                    date = date.AddMonths(ExpireTimeValue);
                    break;
                case TimeUnitEnum.Year:
                    date = date.AddYears(ExpireTimeValue);
                    break;
            }

            return date;
        }

        public static string[] ExpirationTimeUnitEnumNames()
        {
            string[] result = Enum.GetNames(typeof(TimeUnitEnum));
            result[0] = Properties.Resources.ExpirationTimeUnitEnumDay;
            result[1] = Properties.Resources.ExpirationTimeUnitEnumWeek;
            result[2] = Properties.Resources.ExpirationTimeUnitEnumMonth;
            result[3] = Properties.Resources.ExpirationTimeUnitEnumYear;
            return result;
        }

        public static string AsString(TimeUnitEnum Value)
        {
            switch (Value)
            {
                case TimeUnitEnum.Day:
                    return Properties.Resources.ExpirationTimeUnitEnumDay;
                case TimeUnitEnum.Week:
                    return Properties.Resources.ExpirationTimeUnitEnumWeek;
                case TimeUnitEnum.Month:
                    return Properties.Resources.ExpirationTimeUnitEnumMonth;
                case TimeUnitEnum.Year:
                    return Properties.Resources.ExpirationTimeUnitEnumYear;
                default:
                    return Enum.GetName(typeof(TimeUnitEnum), Value);
            }
        }

        /// <summary>
        /// Gets or sets the expiration time unit for the customer order such as days, weeks, months or years see more info here <see cref="TimeUnitEnum"/>
        /// </summary>
        /// <value>The expiration time unit.</value>
        [DataMember]
        public TimeUnitEnum ExpirationTimeUnit { get; set; }

        /// <summary>
        /// Gets or sets the expire time value which is the number of <see cref="ExpirationTimeUnit"/> selected i.e. 5 days
        /// </summary>
        /// <value>The expire time value.</value>
        [DataMember]
        public int ExpireTimeValue { get; set; }

        /// <summary>
        /// If true then the customer order type requires as deposit when an item is added to it
        /// </summary>
        public  bool AcceptsDeposits { get; set; }

        /// <summary>
        /// If <see cref="AcceptsDeposits"/> is true then this value decides the % of the deposit amount
        /// </summary>
        public decimal MinimumDeposits { get; set; }

        /// <summary>
        /// If true the user must select the source (origin) of the customer order type is created
        /// </summary>
        public bool SelectSource { get; set; }

        /// <summary>
        /// If true the user must select a delivery method when the customer order type is created
        /// </summary>
        public bool SelectDelivery { get; set; }

        /// <summary>
        /// If a number series is selected then this series will be used when a new customer order is created. If empty a global Customer order number series will be used
        /// </summary>
        public RecordIdentifier NumberSeries { get; set; }

        /// <summary>
        /// There is one entry per settings type in the underlying table. The GUIDs for each type are fixed in the CustomerOrderSettings data provider
        /// </summary>
        public CustomerOrderType SettingsType { get; set; }

    }
}
