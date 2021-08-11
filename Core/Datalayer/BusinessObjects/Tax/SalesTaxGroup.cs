using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

using LSOne.Utilities.ErrorHandling;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Tax")]
namespace LSOne.DataLayer.BusinessObjects.Tax
{
	/// <summary>
	/// Sales tax groups are attached to customers, stores, vendors etc... (basically anything with tax that is not an item) 
	/// and contains tax codes (0..*). See TaxCodeInSalesTaxGroup class for tax codes in sales tax group.
	/// </summary>
	[Serializable]
	[DataContract]
	public class SalesTaxGroup : OptimizedUpdateDataEntity
	{
		private string searchField1;
		private string searchField2;
		private bool isForEU;

		/// <summary>
		/// Used to determine how to sort sales tax groups in a list.
		/// </summary>
		public enum SortEnum
		{
			/// <summary>
			/// sort by ID (Column TAXGROUP)
			/// </summary>
			ID,
			/// <summary>
			/// sort by description (Column TAXGROUPNAME)
			/// </summary>
			Description,
			/// <summary>
			/// sort by country/region (Column SEARCHFIELD1)
			/// </summary>
			CountryRegion,
			/// <summary>
			/// sort by county/purpose (Column SEARCHFIELD2)
			/// </summary>
			CountyPurpose,
			/// <summary>
			/// sort by country is in EU or not - IsForEU (Column ISFOREU)
			/// </summary>
			IsForEU
		}

		public SalesTaxGroup()
			: base()
		{
			searchField1 = "";
			searchField2 = "";
			isForEU = false;
		}

		[DataMember]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (base.Text != value)
				{
					PropertyChanged("TAXGROUPNAME", value);
					base.Text = value;
				}
			}
		}

		/// <summary>
		/// NOT USED BY LS POS. Maps against country/region UI field. Only used for information purpose.
		/// </summary>
		public string SearchField1
		{
			get
			{
				return searchField1;
			}
			set
			{
				if (searchField1 != value)
				{
					PropertyChanged("SEARCHFIELD1", value);
					searchField1 = value;
				}
			}
		}

		/// <summary>
		/// NOT USED BY LS POS. Maps against county/purpose UI field. Only used for information purpose.
		/// </summary>
		public string SearchField2 {
			get
			{
				return searchField2;
			}
			set
			{
				if (searchField2 != value)
				{
					PropertyChanged("SEARCHFIELD2", value);
					searchField2 = value;
				}
			}
		}

		/// <summary>
		/// Specifies if this tax group is used for transactions between countries in the European Union. Used by SAP Business One integration.
		/// </summary>
		[DataMember]
		public bool IsForEU
		{
			get
			{
				return isForEU;
			}
			set
			{
				if (isForEU != value)
				{
					PropertyChanged("ISFOREU", value);
					isForEU = value;
				}
			}
		}

		public override void ToClass(XElement element, IErrorLog errorLogger = null)
		{
			var currencyElements = element.Elements();
			foreach (XElement storeElem in currencyElements)
			{
				if (!storeElem.IsEmpty)
				{
					try
					{
						switch (storeElem.Name.ToString())
						{
							case "taxGroupID":
								ID = storeElem.Value;
								break;
							case "taxGroupName":
								Text = storeElem.Value;
								break;
							case "searchField1":
								SearchField1 = storeElem.Value;
								break;
							case "searchField2":
								SearchField2 = storeElem.Value;
								break;
							case "IsForEU":
								IsForEU = Convert.ToBoolean(storeElem.Value);
								break;
						}
					}
					catch (Exception ex)
					{
						if (errorLogger != null)
						{
							errorLogger.LogMessage(LogMessageType.Error,
												   storeElem.Name.ToString(), ex);
						}
					}
				}
			}
		}

		public override XElement ToXML(IErrorLog errorLogger = null)
		{
			XElement xml = new XElement("salesTaxGroup",
						new XElement("taxGroupID", ID),
						new XElement("taxGroupName", Text),
						new XElement("searchField1", SearchField1),
						new XElement("searchField2", SearchField2),
						new XElement("IsForEU", IsForEU)
					);
			return xml;
		}
	}
}
