using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.CustomerOrder
{
    //[Serializable]
    //public class DepositTenderLineItem : LineItem, IDepositTenderLineItem
    //{
    //    /// <summary>
    //    /// Creates a new object that is a copy of the current instance.
    //    /// </summary>
    //    /// <returns>
    //    /// A new object that is a copy of this instance.
    //    /// </returns>
    //    public override object Clone()
    //    {
    //        var item = new DepositTenderLineItem();
    //        Populate(item);
    //        return item;
    //    }

    //    public void Populate(DepositTenderLineItem item)
    //    {
    //        base.Populate(item);
    //    }

    //    public override XElement ToXML(IErrorLog errorLogger = null)
    //    {
    //        try
    //        {
    //            XElement xTender = new XElement("DepositTenderLineItem"//,
    //                //new XElement("tenderTypeId", TenderTypeId),
    //                //new XElement("amount", Amount.ToString()),
    //                //new XElement("foreignCurrencyAmount", ForeignCurrencyAmount.ToString()),
    //                //new XElement("companyCurrencyAmount", CompanyCurrencyAmount.ToString()),
    //                //new XElement("exchrateMST", ExchrateMST.ToString()),
    //                //new XElement("exchangeRate", ExchangeRate.ToString()),
    //                //new XElement("currencyCode", CurrencyCode),
    //                //new XElement("openDrawer", OpenDrawer),
    //                //new XElement("changeTenderID", ChangeTenderID),
    //                //new XElement("minimumChangeAmount", MinimumChangeAmount.ToString()),
    //                //new XElement("aboveMinimumTenderId", AboveMinimumTenderId),
    //                //new XElement("changeBack", ChangeBack),
    //                //new XElement("comment", Comment),
    //                //new XElement("ExpectedTenderDeclarationAmount", ExpectedTenderDeclarationAmount.ToString()),
    //                //LoyaltyPoints.ToXML()
    //            );

    //            xTender.Add(base.ToXML());
    //            return xTender;
    //        }
    //        catch (Exception ex)
    //        {
    //            if (errorLogger != null)
    //            {
    //                errorLogger.LogMessage(LogMessageType.Error, "DepositTenderLineItem.ToXml", ex);
    //            }

    //            throw;
    //        }
    //    }

    //    public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
    //    {
    //        try
    //        {
    //            if (xItem.HasElements)
    //            {
    //                IEnumerable<XElement> classVariables = xItem.Elements();
    //                foreach (XElement xVariable in classVariables)
    //                {
    //                    if (!xVariable.IsEmpty)
    //                    {
    //                        try
    //                        {
    //                            switch (xVariable.Name.ToString())
    //                            {
    //                                //case "tenderTypeId":
    //                                //    TenderTypeId = xVariable.Value.ToString();
    //                                //    break;
    //                                //case "amount":
    //                                //    Amount = Convert.ToDecimal(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "foreignCurrencyAmount":
    //                                //    ForeignCurrencyAmount = Convert.ToDecimal(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "companyCurrencyAmount":
    //                                //    CompanyCurrencyAmount = Convert.ToDecimal(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "exchrateMST":
    //                                //    ExchrateMST = Convert.ToDecimal(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "exchangeRate":
    //                                //    ExchangeRate = Convert.ToDecimal(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "currencyCode":
    //                                //    CurrencyCode = xVariable.Value.ToString();
    //                                //    break;
    //                                //case "openDrawer":
    //                                //    OpenDrawer = Conversion.ToBool(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "changeTenderID":
    //                                //    ChangeTenderID = xVariable.Value.ToString();
    //                                //    break;
    //                                //case "minimumChangeAmount":
    //                                //    MinimumChangeAmount = Convert.ToDecimal(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "aboveMinimumTenderId":
    //                                //    AboveMinimumTenderId = xVariable.Value.ToString();
    //                                //    break;
    //                                //case "changeBack":
    //                                //    ChangeBack = Conversion.ToBool(xVariable.Value.ToString());
    //                                //    break;
    //                                //case "comment":
    //                                //    Comment = xVariable.Value.ToString();
    //                                //    break;
    //                                //case "InfocodeLines":
    //                                //    InfoCodeLines = CreateInfocodeLines(xVariable);
    //                                //    break;
    //                                //case "ExpectedTenderDeclarationAmount":
    //                                //    ExpectedTenderDeclarationAmount = Convert.ToDecimal(xVariable.Value);
    //                                //    break;
    //                                //case LoyaltyItem.XmlElementName:
    //                                //    LoyaltyPoints.ToClass(xVariable);
    //                                //    break;
    //                                default:
    //                                    base.ToClass(xVariable);
    //                                    break;
    //                            }
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            if (errorLogger != null)
    //                            {
    //                                errorLogger.LogMessage(LogMessageType.Error, "DepositTenderLineItem:" + xVariable.Name.ToString(), ex);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (errorLogger != null)
    //            {
    //                errorLogger.LogMessage(LogMessageType.Error, "DepositTenderLineItem.ToClass", ex);
    //            }

    //            throw ex;
    //        }
    //    }
    //}
}
