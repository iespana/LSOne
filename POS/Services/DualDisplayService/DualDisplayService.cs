using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class DualDisplayService : IDualDisplayService
    {
        private DualDisplayForm dualDisplayForm;
        
        private IErrorLog errorLog;
        
        private bool invalid;

        public virtual IErrorLog ErrorLog
        {
            set
            {
                errorLog = value;
            }
        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            Screen screen = GetScreenFromProfile(entry);
            if (!invalid)
            {
                
                dualDisplayForm = new DualDisplayForm(entry, screen);
            }
        }

        public void Show()
        {
            if (!invalid)
            {
                dualDisplayForm.Show();
            }
        }

        public void ShowTransaction(IPosTransaction transaction)
        {
            if (dualDisplayForm != null)
            {
                dualDisplayForm.ShowTransaction((PosTransaction) transaction);
            }
        }

        public void Close()
        {
            if (!invalid)
            {
                dualDisplayForm.Close();
            }
        }

        public void UpdateTotalAmountsLayout(string layoutXML)
        {
            if(dualDisplayForm != null)
            {
                dualDisplayForm.SetTotalsLayout(layoutXML);
            }
        }

        private Screen GetScreenFromProfile(IConnectionManager entry)
        {
            Screen[] allScreens = Screen.AllScreens;

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (settings.HardwareProfile.DualDisplayScreen == HardwareProfile.DualDisplayScreens.Default)
            {
                if (allScreens.Count() == 1)
                {
                    invalid = true;
                    ((IDialogService)entry.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.DisplayScreenUnavailable, Properties.Resources.UnableToShowDualDisplay, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return allScreens[0];
                }
                for(int i = 0; i < allScreens.Count(); i++)
                {
                    if (!allScreens[i].Primary)
                    {
                        return allScreens[i];
                    }
                }
            }

            if ((int) settings.HardwareProfile.DualDisplayScreen + 1 > allScreens.Count())
            {
                ((IDialogService) entry.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.DisplayScreenUnavailable, Properties.Resources.UnableToShowDualDisplay, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                invalid = true;
                return allScreens[0];
            }

            return allScreens[(int)settings.HardwareProfile.DualDisplayScreen];
        }   
    }
}
