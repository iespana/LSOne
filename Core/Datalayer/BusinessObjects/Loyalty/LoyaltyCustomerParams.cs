using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
	/// <summary>
	/// A business object that holds information about the loyalty Customer.
	/// </summary>
	public class LoyaltyCustomerParams : DataEntity
	{
		
		/// <summary>
		/// Initializes a new instance of the <see cref="LoyaltyCustomerParams" /> class.
		/// </summary>
		public LoyaltyCustomerParams()
		{
			Key = RecordIdentifier.Empty;
		}

		/// <summary>
		/// Gets or sets the Key.
		/// </summary>
		/// <value>The parameters' unique Key value.</value>
		public RecordIdentifier Key { get; set; }

				/// <summary>
        /// Default store filter when viewing loyalty data in Site Manager
		/// </summary>
		public RecordIdentifier DefaultStore { get; set; } 
		/// <summary>
        /// Default terminal filter when viewing loyalty data in Site Manager
		/// </summary>
		public RecordIdentifier DefaultTerminal { get; set; } 
		/// <summary>
        /// Default loyalty scheme filter when viewing loyalty data in Site Manager
		/// </summary>
		public RecordIdentifier DefaultLoyaltyScheme { get; set; }

        /// <summary>
        /// Default loyalty scheme to use for new loyalty customers created through Omni loyalty app
        /// </summary>
        public RecordIdentifier OmniLoyaltyScheme { get; set; }
        /// <summary>
        /// Starting points for new loyalty customers created through Omni loyalty app
        /// </summary>
        public int OmniLoyaltyPoints { get; set; }
	}
}
