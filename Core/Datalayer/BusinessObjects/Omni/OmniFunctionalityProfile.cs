using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Omni
{
	/// <summary>
	/// LS Commerce-related settings from <see cref="LSOne.DataLayer.BusinessObjects.Profiles.FunctionalityProfile"/>
	/// </summary>
	[LSOneUsage(CodeUsage.LSCommerce)]
	public class OmniFunctionalityProfile
	{
		/// <summary>
		/// ID of the main menu button layout in the  in mobile Inventory.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public RecordIdentifier MainMenu { get; set; }

		/// <summary>
		/// Specified if a line is added or merged with an existing line in an inventory document (Stock counting, purchase order, store transfer) in the mobile Inventory.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public EnteringTypeEnum EnteringType { get; set; }

		/// <summary>
		/// Specifies how the quantity is set in mobile Inventory when a new line is added to an inventory document (Stock counting, purchase order, store transfer) in the inventory app.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public QuantityMethodEnum QuantityMethod { get; set; }

		/// <summary>
		/// Default quantity to be set in mobile Inventory when <see cref="QuantityMethod"></see> is not Ask./>
		/// </summary>
		/// <remarks>Default value is 1.</remarks>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public decimal DefaultQuantity { get; set; }

		/// <summary>
		/// ID of the suspension type used in the mobile POS when suspending a transaction.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public RecordIdentifier SuspensionType { get; set; }

		/// <summary>
		/// ID of the printing station used in the mobile POS when printing receipts.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public RecordIdentifier PrintingStation { get; set; }

		/// <summary>
		/// ID of the special group containing items for which to load images in the mobile POS when replicating Plu.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public RecordIdentifier ItemImageLookupGroup { get; set; }

		/// <summary>
		/// If set it will allow the mobile POS to run in offline mode.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public bool AllowOfflineTransaction { get; set; }

		/// <summary>
		/// If enabled shortcut to the mobile Inventory application is displayed in mobile POS.
		/// </summary>
		[LSOneUsage(CodeUsage.LSCommerce)]
		public bool ShowMobileInventory { get; set; }

		/// <summary>
		/// Initializes a new <see cref="OmniFunctionalityProfile"/> object.
		/// </summary>
		public OmniFunctionalityProfile()
		{
			Init();
		}

		/// <summary>
		/// Initializes <see cref="OmniFunctionalityProfile"/> properties with default values.
		/// </summary>
		protected void Init()
		{
			MainMenu = string.Empty;
			EnteringType = EnteringTypeEnum.AddToQty;
			QuantityMethod = QuantityMethodEnum.Ask;
			DefaultQuantity = 1;

			SuspensionType = string.Empty;
			PrintingStation = string.Empty;
			ItemImageLookupGroup = Guid.Empty;
			AllowOfflineTransaction = false;
			ShowMobileInventory = false;
		}
		
		
		/// <summary>
		/// Returns a deep clone of the existing <see cref="OmniFunctionalityProfile"/> object.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			var clone = new OmniFunctionalityProfile();

			clone.MainMenu = MainMenu;
			clone.EnteringType = EnteringType;
			clone.QuantityMethod = QuantityMethod;
			clone.DefaultQuantity = DefaultQuantity;
			clone.SuspensionType = SuspensionType;
			clone.PrintingStation = PrintingStation;
			clone.ItemImageLookupGroup = ItemImageLookupGroup;
			clone.AllowOfflineTransaction = AllowOfflineTransaction;
			clone.ShowMobileInventory = ShowMobileInventory;
			
			return clone;
		}

		public void ToClass(XElement element, IErrorLog errorLogger = null)
		{
			var elements = element.Elements();
			foreach (XElement current in elements)
			{
				if (!current.IsEmpty)
				{
					try
					{
						switch (current.Name.ToString())
						{
							case "mainMenu":
								MainMenu = new RecordIdentifier(current.Value);
								break;
							case "enteringType":
								EnteringType = (EnteringTypeEnum)Conversion.XmlStringToInt(current.Value);
								break;
							case "quantityMethod":
								QuantityMethod = (QuantityMethodEnum)Conversion.XmlStringToInt(current.Value);
								break;
							case "defaultQuantity":
								DefaultQuantity = Conversion.XmlStringToDecimal(current.Value);
								break;
							case "suspensionType":
								SuspensionType = new RecordIdentifier(current.Value);
								break;
							case "printingStation":
								PrintingStation = new RecordIdentifier(current.Value);
								break;
							case "itemImageLookupGroup":
								ItemImageLookupGroup = new RecordIdentifier(Conversion.XmlStringToGuid(current.Value));
								break;
							case "allowOfflineTransaction":
								AllowOfflineTransaction = Conversion.XmlStringToBool(current.Value);
								break;
							case "showMobileInventory":
								ShowMobileInventory = Conversion.XmlStringToBool(current.Value);
								break;
						}
					}
					catch (Exception ex)
					{
						errorLogger?.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
					}
				}
			}
		}

		public XElement ToXML(IErrorLog errorLogger = null)
		{
			var xml = new XElement("omniFunctionalityProfile",
									new XElement("mainMenu", (string)MainMenu),
									new XElement("enteringType", Conversion.ToXmlString((int)EnteringType)),
									new XElement("quantityMethod", Conversion.ToXmlString((int)QuantityMethod)),
									new XElement("defaultQuantity", Conversion.ToXmlString(DefaultQuantity)),
									new XElement("suspensionType", (string)SuspensionType),
									new XElement("printingStation", (string)PrintingStation),
									new XElement("itemImageLookupGroup", ItemImageLookupGroup),
									new XElement("allowOfflineTransaction", Conversion.ToXmlString(AllowOfflineTransaction)),
									new XElement("showMobileInventory", Conversion.ToXmlString(ShowMobileInventory))
			);

			return xml;
		}
	}
}