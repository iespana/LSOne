using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService : IService
    {
        /// <summary>
        /// Deletes an image from the image bank
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="pictureID">The ID of the image to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteImage(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier pictureID, bool closeConnection);

        /// <summary>
        /// Deletes multiple images from the image bank
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="pictureIDs">A list of image IDs to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteImageList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> pictureIDs, bool closeConnection);
    }
}
