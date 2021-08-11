using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkPriceAndDiscountInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace ="LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkPriceAndDiscountService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Saves a trade agreement
        /// </summary>
        /// <param name="agreement">The <see cref="TradeAgreementEntry"/> to be saved</param>
        [OperationContract]
        void SaveTradeAgreement(TradeAgreementEntry agreement);

        /// <summary>
        /// Saves a list of <see cref="TradeAgreementEntry"/>
        /// </summary>
        /// <param name="agreements">The list of <see cref="TradeAgreementEntry"/> to be saved</param>
        [OperationContract]
        SaveResult SaveTradeAgreementList(List<TradeAgreementEntry> agreements);

        /// <summary>
        /// Saves a single <see cref="PriceDiscountGroup"/> to the database. If it does not exists it will be created.
        /// </summary>        
        /// <param name="priceDiscountGroup">The priceDiscountGroup to save</param>
        [OperationContract]
        void SavePriceDiscountGroup(PriceDiscountGroup priceDiscountGroup);

        /// <summary>
        /// Saves a list of <see cref="PriceDiscountGroup"/> objects to the database. PriceDiscountGroups that do not exist will be created.
        /// </summary>        
        /// <param name="priceDiscountGroup">The list of priceDiscountGroups to save</param>     
        [OperationContract]
        SaveResult SavePriceDiscountGroupList(List<PriceDiscountGroup> priceDiscountGroup);

        /// <summary>
        /// Gets a single <see cref="PriceDiscountGroup"/> from the databse for the given <paramref name="priceDiscountGroupID"/>.
        /// </summary>        
        /// <param name="priceDiscountGroupID">The ID of the priceDiscountGroups to get. </param>
        /// <returns></returns>        
        [OperationContract]
        PriceDiscountGroup GetPriceDiscountGroup(RecordIdentifier priceDiscountGroupID);

        /// <summary>
        /// Deletes the list of priceDiscountGroup with the given <paramref name="priceDiscountGroupID"/> from the database.
        /// </summary>
        /// <param name="priceDiscountGroup">List of price discounts groups.</param>
        [OperationContract]
        void DeletePriceDiscountGroupList(List<PriceDiscountGroup> priceDiscountGroup);


        /// <summary>
        /// Deletes the priceDiscountGroup with the given <paramref name="priceDiscountGroupID"/> from the database.
        /// </summary>
        /// <param name="priceDiscountGroupID">The ID of the item to delete</param>
        /// <returns></returns>        
        [OperationContract]
        void DeletePriceDiscountGroup(RecordIdentifier priceDiscountGroupID);
    }
}
