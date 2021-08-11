using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.EFT;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.EFT.Common.Validation
{
    public class MappingValidation
    {
        public static bool ValidateMappingFallback(IConnectionManager entry, out string errorCaption, out string errorMessage)
        {
            var mapping = Providers.EFTMappingData.GetForScheme(DLLEntry.DataModel, "*");
            if (mapping == null)
            {
                errorCaption = Properties.Resources.EFTMappingRequired;
                errorMessage = Properties.Resources.EFTMappingFallbackNotDefined;
                return false;
            }

            errorCaption = errorMessage = "";
            return true;
        }

        public static StorePaymentMethod ValidateTenderType(IConnectionManager entry, RecordIdentifier tenderTypeID)
        {
            var tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, tenderTypeID),
                                                        CacheType.CacheTypeApplicationLifeTime);
            if (tenderInfo == null)
            {
                ((IDialogService) entry.Service(ServiceType.DialogService)).ShowMessage(
                    Properties.Resources.TenderInformationNotFound.Replace("#1", (string) tenderTypeID));
                return null;
            }
            if (tenderInfo.ID.SecondaryID == RecordIdentifier.Empty)
            {
                // Invalid tender option
                ((IDialogService) entry.Service(ServiceType.DialogService)).ShowMessage(
                    Properties.Resources.TenderNotProperlyConfigured.Replace("#1", (string) tenderTypeID),
                    MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return null;
            }

            return tenderInfo;
        }
    }
}
