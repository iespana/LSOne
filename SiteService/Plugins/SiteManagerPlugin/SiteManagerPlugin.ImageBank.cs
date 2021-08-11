using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void DeleteImage(LogonInfo logonInfo, RecordIdentifier pictureID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(pictureID)}: {pictureID}");

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                Providers.ImageData.Delete(dataModel, pictureID);

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
                    dataModel.Connection.RestoreContext();
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void DeleteImageList(LogonInfo logonInfo, List<RecordIdentifier> pictureIDs)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(pictureIDs)}: {pictureIDs}");

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "User context changed");
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                pictureIDs.ForEach(p => Providers.ImageData.Delete(dataModel, p));

                if (logonInfo.UserID != RecordIdentifier.Empty)
                {
                    Utils.Log(this, "Original user context restored");
                    dataModel.Connection.RestoreContext();
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}