namespace LSOne.DataLayer.BusinessObjects.Tax
{
	/// <summary>
	/// Search filter for tax groups
	/// </summary>
	public enum TaxGroupTypeFilter
	{
		/// <summary>
		/// All tax groups
		/// </summary>
		All,
		/// <summary>
		/// All standard tax groups
		/// </summary>
		Standard,
		/// <summary>
		/// All tax groups that apply only to EU countries.
		/// </summary>
		EU
	}
}
