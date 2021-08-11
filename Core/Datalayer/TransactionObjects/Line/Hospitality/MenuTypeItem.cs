using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.Hospitality
{
    [Serializable]
    public class MenuTypeItem : IMenuTypeItem
    {
        public string RestaurantID { get; set; }
        public string MenuOrder { get; set; }
        public string Description { get; set; }
        public string CodeOnPos { get; set; }

        public MenuTypeItem()
        {
            RestaurantID = "";
            MenuOrder = "";
            Description = "";
            CodeOnPos = "";
        }

        public MenuTypeItem(MenuTypeItem item)
        {
            this.RestaurantID = item.RestaurantID;
            this.MenuOrder = item.MenuOrder;
            this.Description = item.Description;
            this.CodeOnPos = item.CodeOnPos;
        }

        public MenuTypeItem(IMenuTypeItem item)
        {
            this.RestaurantID = item.RestaurantID;
            this.MenuOrder = item.MenuOrder;
            this.Description = item.Description;
            this.CodeOnPos = item.CodeOnPos;
        }

        public bool Exists
        {
            get { return ((RestaurantID != null) && (MenuOrder != "") && (Description != "") && (CodeOnPos != "")); }
        }

        public bool NotExists
        {
            get { return (Exists == false); }
        }

        public string DisplayText
        {
            get
            {
                string txt = Description;
                if (CodeOnPos != "")
                {
                    txt += " (" + CodeOnPos + ")";
                }

                return txt;
            }
        }

        public virtual object Clone()
        {
            MenuTypeItem item = new MenuTypeItem();
            Populate(item);
            return item;
        }

        protected void Populate(MenuTypeItem item)
        {
            item.RestaurantID = RestaurantID;
            item.MenuOrder = MenuOrder;
            item.Description = Description;
            item.CodeOnPos = CodeOnPos;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xMarkup = new XElement("menuTypeItem",
                    new XElement("restaurantId", RestaurantID),
                    new XElement("menuOrder", MenuOrder),
                    new XElement("description", Description),
                    new XElement("codeOnPos", CodeOnPos)
                );

                return xMarkup;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "MenuTypeItem.ToXML", ex);
                }

                throw ex;
            }
        }

        public void ToClass(XElement xMarkup, IErrorLog errorLogger = null)
        {
            try
            {
                bool isEmpty = true;
                if (xMarkup.HasElements)
                {
                    IEnumerable<XElement> menuTypeElements = xMarkup.Elements();
                    foreach (XElement menuTypeElem in menuTypeElements)
                    {
                        if (!menuTypeElem.IsEmpty)
                        {
                            try
                            {
                                isEmpty = false;
                                switch (menuTypeElem.Name.ToString())
                                {
                                    case "restaurantId":
                                        RestaurantID = menuTypeElem.Value;
                                        break;
                                    case "menuOrder":
                                        MenuOrder = menuTypeElem.Value;
                                        break;
                                    case "description":
                                        Description = menuTypeElem.Value;
                                        break;
                                    case "codeOnPos":
                                        CodeOnPos = menuTypeElem.Value;
                                        break;                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "MenuTypeItem:" + menuTypeElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }

                //When the transaction is rebuilt the transaction default menu type is rebuilt first
                //then when an item doesn't have a menu type the default menu type will overwrite it
                //even though it's not supposed to be filled out
                //Because of this - the following code is necessary
                if (isEmpty)
                {
                    RestaurantID = "";
                    MenuOrder = "";
                    Description = "";
                    CodeOnPos = "";
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "MenuTypeItem.ToClass", ex);
                }

                throw ex;
            }
        }

    }
}
