using System;
using System.Xml.Linq;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces.SupportClasses
{
    /// <summary>
    /// An abstract class that can be customzied to be any class. Used by partners for customizations. See further information in the PartnerObjectDemo project on the LS Retail Partner Portal
    /// </summary>
    [Serializable]
    abstract public class PartnerObjectBase
    {
        public string ToXMLPartnerObjectTag = "PartnerObject";        

        /// <summary>
        /// The partner object needs to be included in the XML-ing of transactions. If the Partner Object includes other classes and/or objects
        /// they need to have ToXML and ToClass functions as well. See more information in the PartnerObjectDemo project on the LS Retail Partner Portal        
        /// </summary>
        /// <returns>The the object in an XML format</returns>
        abstract public XElement ToXML();

        /// <summary>
        /// The partner object needs to be included in the XML-ing of transactions. If the Partner Object includes other classes and/or objects
        /// they need to have ToXML and ToClass functions as well. See more information in the PartnerObjectDemo project on the LS Retail Partner Portal        
        /// </summary>
        /// <param name="xmlPartnerObject">The XML that the object has to recreate itself from</param>
        abstract public void ToClass(XElement xmlPartnerObject);

        /// <summary>
        /// After the transaction has been saved the PartnerObject.Save function is called to allow the PartnerObject to be saved into
        /// the database without having to implement it separately in the PostEndTransactionTrigger.
        /// </summary>
        /// <param name="retailTransaction">The transaction that was just saved to the database</param>
        abstract public void Save(IRetailTransaction retailTransaction);

        /// <summary>
        /// If the partner object should be rebuilt with the transaction f.ex. to be viewed in the Journal or as a copy of the receipt 
        /// in the Site Manager then the Rebuild function needs to be implemented. 
        /// The PartnerObject.Rebuild function is called after theentire transaction has been rebuild
        /// </summary>
        /// <param name="retailTransaction">The transaction that was rebuilt from data</param>
        abstract public void Rebuild(IRetailTransaction retailTransaction);      
    }
}
