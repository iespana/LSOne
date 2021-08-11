using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Deletes an image from the image bank
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="pictureID">The ID of the image to delete</param>        
        [OperationContract]
        void DeleteImage(LogonInfo logonInfo, RecordIdentifier pictureID);

        /// <summary>
        /// Deletes multiple images from the image bank
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="pictureIDs">A list of image IDs to delete</param>
        [OperationContract]
        void DeleteImageList(LogonInfo logonInfo, List<RecordIdentifier> pictureIDs);


    }
}
