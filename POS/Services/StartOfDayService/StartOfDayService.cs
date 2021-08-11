using System;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.POS.Core;
using LSOne.POS.Processes.AbstractOperations;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public class StartOfDayService : IStartOfDayService
    {
        private IConnectionManager entry;
        private Settings settings;

        public virtual bool FloatEntryRequired { get; set; }
        public virtual IErrorLog ErrorLog { set; private get; }

        public virtual void RunStartOfDay()
        {
            if (settings.FunctionalityProfile.UseStartOfDay && FloatEntryRequired)
            {
                    settings.POSApp.RunOperation(POSOperations.StartOfDay, "inputRequired");
            }
            settings.POSApp.BusinessDateSet();
        }

        public virtual bool BusinessDaySet { get; set; }

        public virtual DateTime BusinessDay
        {
            get { return settings != null ? settings.BusinessDay : DateTime.Now;
            }
            set
            {
                BusinessDaySet = value != DateTime.MinValue;
                settings.BusinessDay = value;
                TransactionProviders.StartOfDayData.SaveBusinessDay(value);
                settings.POSApp.BusinessDateSet();
            }
        }

        public virtual DateTime BusinessSystemDay
        {
            get
            {
                return settings != null ? settings.BusinessSystemDay : DateTime.Now;
            }
            set
            {
                settings.BusinessSystemDay = value;
                TransactionProviders.StartOfDayData.SaveBusinessSystemDay(value);
            }
        }

        public void Init(IConnectionManager entry)
        {
            this.entry = entry;
            ErrorLog = entry.ErrorLogger;
            settings = (Settings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (settings.FunctionalityProfile.UseStartOfDay)
            {
                Operation.PreRunOperation += OperationOnPreRunOperation;
            }
            FloatEntryRequired = TransactionProviders.StartOfDayData.FloatRequired(entry);
            DateTime? loadedDay = TransactionProviders.StartOfDayData.GetBusinessDay();
            DateTime? loadedSystemDay = TransactionProviders.StartOfDayData.GetBusinessSystemDay();
            BusinessDay = loadedDay ?? DateTime.Now;
            BusinessSystemDay = loadedSystemDay ?? DateTime.Now;
        }

        private bool OperationOnPreRunOperation(Operation operation)
        {
            if (FloatEntryRequired && (operation is CustomerOperation || operation is ItemOperation || operation is TenderOperation))
            {
                IDialogService dialog = Interfaces.Services.DialogService(entry);
                
                if (entry.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.StartOfDay))
                {
                    DialogResult result = dialog.ShowMessage(Resources.OperationNotAllowedBeforeStarOfDayRunNow,
                                                             MessageBoxButtons.YesNo, 
                                                             MessageDialogType.Attention);
                    if (result == DialogResult.Yes)
                    {
                        var settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                        settings.POSApp.RunOperation(POSOperations.StartOfDay, null);
                        return true;
                    }
                }
                else
                {
                    dialog.ShowMessage(Resources.OperationNotAllowedBeforeStartOfDay);
                }
                return false;
            }
            return true;
        }
    }
}
