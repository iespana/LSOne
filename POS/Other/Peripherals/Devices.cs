using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Splash;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Peripherals.DialogPages;
using LSOne.Peripherals.Dialogs;
using LSOne.Peripherals.Properties;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Settings;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Utilities.DataTypes;
using LSRetailPosis;
using LSOne.Controls.SupportClasses;

namespace LSOne.Peripherals
{
    static public class Devices
    {
        private const string key = "IcelandPos1944";
        
        static private bool ProcessError(string exceptionMessage)
        {
            Form activeForm = Splasher.ActiveForm();
            if (activeForm != null)
                return LSOne.Controls.TouchMessageDialog.Show(Splasher.ActiveForm(), exceptionMessage + "\r\n" + Properties.Resources.FixProblemAndRetry, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes;

            return LSOne.Controls.TouchMessageDialog.Show(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2), exceptionMessage + "\r\n" + Properties.Resources.FixProblemAndRetry, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes;
        }

        static private bool ProcessHardwareError(string exceptionMessage)
        {
            Form activeForm = Splasher.ActiveForm();
            if (activeForm != null)
                return LSOne.Controls.TouchMessageDialog.Show(Splasher.ActiveForm(), exceptionMessage, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes;

            return LSOne.Controls.TouchMessageDialog.Show(new Point(Screen.PrimaryScreen.Bounds.Width/2, Screen.PrimaryScreen.Bounds.Height/2), exceptionMessage, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes;

        }

        static public string Validate(string validation)
        {
            return HMAC_SHA1.GetValue(validation, key);
        }

        private static void ValidateHardwareProfile(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            List<string> errorList = new List<string>();
            
            if ((settings.HardwareProfile.PrinterDeviceName == "" && settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.OPOS) ||
                (RecordIdentifier.IsEmptyOrNull(settings.HardwareProfile.WindowsPrinterConfigurationID) && settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.Windows))
            {
                errorList.Add(Properties.Resources.PrinterEmpty);
            }

            if (settings.HardwareProfile.DisplayDeviceName == "" &&
                settings.HardwareProfile.LineDisplayDeviceType ==  HardwareProfile.LineDisplayDeviceTypes.OPOS)
            {
                errorList.Add(Properties.Resources.LineDisplayEmpty);
            }

            if (settings.HardwareProfile.DrawerDeviceName == "" &&
                settings.HardwareProfile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS)
            {
                errorList.Add(Properties.Resources.DrawerEmpty);
            }

            if (settings.HardwareProfile.KeyLockDeviceName == "" &&
                settings.HardwareProfile.KeyLockDeviceType == HardwareProfile.DeviceTypes.OPOS)
            {
                errorList.Add(Properties.Resources.KeylockEmpty);
            }

            if (settings.HardwareProfile.ScannerDeviceName == "" &&
                settings.HardwareProfile.ScannerConnected)
            {
                errorList.Add(Properties.Resources.BarcodeReaderEmpty);
            }

            if (settings.HardwareProfile.MsrDeviceName == "" && settings.HardwareProfile.MsrDeviceType == HardwareProfile.DeviceTypes.OPOS)
            {
                errorList.Add(Properties.Resources.CardReaderEmpty);
            }

            if (errorList.Count > 0)
            {
                string errorString = Properties.Resources.HardwareProfileErrorString;
                StringBuilder errors = new StringBuilder();
                errors.Append(Environment.NewLine);
                errors.Append(Environment.NewLine);

                foreach (var error in errorList)
                {
                    errors.Append(error);
                    errors.Append(Environment.NewLine);
                }

                errors.Append(Environment.NewLine);

                errorString = string.Format(errorString, errors);

                Form activeForm = Splasher.ActiveForm();

                if (activeForm != null)
                {
                    TouchMessageDialog.Show(Splasher.ActiveForm(), errorString, MessageBoxButtons.OK, MessageDialogType.Attention);
                }
                else
                {
                    Point centrePoint = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
                    TouchMessageDialog.Show(centrePoint, errorString, MessageBoxButtons.OK, MessageDialogType.Attention);
                }

                throw new Exception(errorString);
            }
        }

        public static void ShowHardwareSetup(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!entry.HasPermission(Permission.ManagePosHardwareProfile))
            {
                Form activeForm = Splasher.ActiveForm();
                if (activeForm != null)
                {
                    if ((TouchMessageDialog.Show(Splasher.ActiveForm(), Resources.NoHardwareProfilePermission + "\r\n\n" + Resources.HardwareProfileSufficientPermissions, MessageBoxButtons.OK, MessageDialogType.Attention)) == DialogResult.OK)
                    {
                        throw new OperationCanceledException();
                    }
                }
                else
                {
                    Point centrePoint = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
                    if ((TouchMessageDialog.Show(centrePoint, Resources.NoHardwareProfilePermission + "\r\n\n" + Resources.HardwareProfileSufficientPermissions, MessageBoxButtons.OK, MessageDialogType.Attention)) == DialogResult.OK)
                    {
                        throw new OperationCanceledException();
                    }
                }
            }

            int currentHardwareStep = 0;
            //Peripheral Setup Dialog
            HardwareConfigurationDialog hardwareDialog = new HardwareConfigurationDialog()
            {
                SqlServerLoginEntry =
                    SettingsContainer<ApplicationSettings>.Instance.CurrentLoginEntry
            };
            HardwareConfigurationStep printer = new HardwareConfigurationStep
            {
                BackAllowed = true,
                ConfigurationSteps = hardwareDialog.HardwareConfigurationSteps,
                NextButtonCaption = Resources.Finish,
                Step = new SetPrinterPage ( "POSPrinter"){ ContainerDialog = hardwareDialog},
                StepIndex = currentHardwareStep++,
                Description = Resources.Printer,
                Container = hardwareDialog

            };
            hardwareDialog.HardwareConfigurationSteps.Add(printer);

            HardwareConfigurationStep drawer = new HardwareConfigurationStep
            {
                BackAllowed = true,
                ConfigurationSteps = hardwareDialog.HardwareConfigurationSteps,
                NextButtonCaption = Resources.Finish,
                Step = new SetDrawerPage  ("CashDrawer"){ ContainerDialog = hardwareDialog },
                StepIndex = currentHardwareStep++,
                Description = Resources.Drawer,
                Container = hardwareDialog

            };

            hardwareDialog.HardwareConfigurationSteps.Add(drawer);

            HardwareConfigurationStep cardReader = new HardwareConfigurationStep
            {
                BackAllowed = false,
                ConfigurationSteps = hardwareDialog.HardwareConfigurationSteps,
                NextButtonCaption = Resources.Finish,
                Step = new SetCardReaderPage ("MSR"){ ContainerDialog = hardwareDialog},
                StepIndex = currentHardwareStep++,
                Description = Resources.CardReader,
                Container = hardwareDialog

            };
            hardwareDialog.HardwareConfigurationSteps.Add(cardReader);

            HardwareConfigurationStep lineDisplay = new HardwareConfigurationStep
            {
                BackAllowed = false,
                ConfigurationSteps = hardwareDialog.HardwareConfigurationSteps,
                NextButtonCaption = Resources.Finish,
                Step = new SetLineDisplayPage("LineDisplay") { ContainerDialog = hardwareDialog },
                StepIndex = currentHardwareStep++,
                Description = Resources.LineDisplay,
                Container = hardwareDialog

            };
            hardwareDialog.HardwareConfigurationSteps.Add(lineDisplay);

            HardwareConfigurationStep scanner = new HardwareConfigurationStep
            {
                BackAllowed = true,
                ConfigurationSteps = hardwareDialog.HardwareConfigurationSteps,
                NextButtonCaption = Resources.Finish,
                Step = new SetBarcodeReaderPage("Scanner") { ContainerDialog = hardwareDialog },
                StepIndex = currentHardwareStep++,
                Description = Resources.BarcodeReader,
                Container = hardwareDialog

            };
            hardwareDialog.HardwareConfigurationSteps.Add(scanner);

            HardwareConfigurationStep eTax = new HardwareConfigurationStep
            {
                BackAllowed = true,
                ConfigurationSteps = hardwareDialog.HardwareConfigurationSteps,
                NextButtonCaption = Resources.Finish,
                Step = new SetETaxFiscalDevicePage("eTax") { ContainerDialog = hardwareDialog },
                StepIndex = currentHardwareStep++,
                Description = Resources.ETax,
                Container = hardwareDialog

            };
            hardwareDialog.HardwareConfigurationSteps.Add(eTax);

            hardwareDialog.Profile = settings.HardwareProfile;
            hardwareDialog.ShowDialog();
            UnloadDevices(entry);

        }

        public static void LoadDevices(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            try
            {

                if (settings.NeedsEFTSetup && entry.HasPermission(Permission.ManagePosHardwareProfile))
                {
                    settings.NeedsEFTSetup = false;

                    Terminal terminal = Providers.TerminalData.Get(entry, entry.CurrentTerminalID, entry.CurrentStoreID);

                    TouchBaseForm dlg = (TouchBaseForm) Services.Interfaces.Services.EFTService(entry).GetEftSetupForm(terminal);

                    if (dlg != null)
                    {
                        DialogResult dialogResult;
                        IEFTSetupForm eftSetupForm = dlg as IEFTSetupForm;

                        eftSetupForm.LoadData(entry, settings);

                        dialogResult = dlg.ShowDialog();

                        if (dialogResult == DialogResult.OK && dlg is IEFTSetupForm)
                        {
                            if (!eftSetupForm.SaveOverride(entry, settings))
                            {

                                try
                                {


                                    Services.Interfaces.Services.SiteServiceService(entry).SetTerminalEFT(
                                        entry,
                                        settings.SiteServiceProfile,
                                        entry.CurrentTerminalID,
                                        entry.CurrentStoreID,
                                        eftSetupForm.IPAddress,
                                        eftSetupForm.EFTStoreId,
                                        eftSetupForm.EFTTerminalID,
                                        eftSetupForm.CustomField1,
                                        eftSetupForm.CustomField2);
                                }
                                catch
                                {
                                    TouchMessageDialog.Show(
                                        new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2),
                                        Resources.HardwareNotSavedOnHO,
                                        MessageBoxButtons.OK, MessageDialogType.Attention);
                                }

                                terminal.IPAddress = eftSetupForm.IPAddress;
                                terminal.EftStoreID = eftSetupForm.EFTStoreId;
                                terminal.EftTerminalID = eftSetupForm.EFTTerminalID;
                                terminal.EftCustomField1 = eftSetupForm.CustomField1;
                                terminal.EftCustomField2 = eftSetupForm.CustomField2;
                                Providers.TerminalData.Save(entry, terminal);
                            }
                        }

                        IEFTService eftService = Services.Interfaces.Services.EFTService(entry);
                        if (eftService is IEFTSession)
                        {
                            ((IEFTSession)eftService).Initialize(entry, settings);
                        }
                    }
                }
                bool carryOnTrying = true;
                LoadInputDevices(entry);
                // Printer
                while (carryOnTrying)
                {
                    try
                    {
                        Printer.Load(entry);
                        carryOnTrying = false;
                    }
                    catch (Exception ex)
                    {
                        if (!ProcessError(Properties.Resources.CouldNotLoadPrinter.Replace("#1", ex.Message)))
                        {
                            throw;
                        }
                    }
                }

                // LineDisplay
                carryOnTrying = true;
                while (carryOnTrying)
                {
                    try
                    {
                        LineDisplay.Load();
                        carryOnTrying = false;
                    }
                    catch (Exception ex)
                    {
                        if (!ProcessError(Properties.Resources.CouldNotLoadLineDisplay.Replace("#1", ex.Message)))
                        {
                            throw;
                        }
                    }
                }
                // Scale
                carryOnTrying = true;
                while (carryOnTrying)
                {
                    try
                    {
                        IScaleService service = (IScaleService)entry.Service(ServiceType.ScaleService);
                        service.LoadPeripheral();
                        carryOnTrying = false;
                    }
                    catch (Exception ex)
                    {
                        if (!ProcessError(Properties.Resources.CouldNotLoadScale.Replace("#1", ex.Message)))
                        {
                            throw;
                        }
                    }
                }
                // Keylock
                carryOnTrying = true;
                while (carryOnTrying)
                {
                    try
                    {
                        Keylock.Load();
                        carryOnTrying = false;
                    }
                    catch (Exception ex)
                    {
                        if (!ProcessError(Properties.Resources.CouldNotLoadKeylock.Replace("#1", ex.Message)))
                        {
                            throw;
                        }
                    }
                }

                // Cash Drawer
                carryOnTrying = true;
                while (carryOnTrying)
                {
                    try
                    {
                        CashDrawer.Load();
                        carryOnTrying = false;
                    }
                    catch (Exception ex)
                    {
                        if (!ProcessError(Properties.Resources.CouldNotLoadCashDrawer.Replace("#1", ex.Message)))
                        {
                            throw;
                        }
                    }
                }

                // Forecourt Manager
                carryOnTrying = true;
                while (carryOnTrying)
                {
                    try
                    {
                        ForecourtManager.Load();
                        carryOnTrying = false;
                    }
                    catch (Exception ex)
                    {
                        if (!ProcessError(Properties.Resources.CouldNotLoadForecourtManager.Replace("#1", ex.Message)))
                        {
                            throw;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, Resources.PeripheralsDevicesLoadDevicesErrorLogger, x);
                if ( !ProcessHardwareError( Resources.CouldNotLoadHardwareProfile.Replace("#1", x.Message + Environment.NewLine)))
                {
                    throw x;
                }
                try
                {
                    UnloadDevices(entry);
                    ShowHardwareSetup(entry);
                    LoadDevices(entry);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, Resources.PeripheralsDevicesLoadDevicesErrorLogger, ex);
                    if ((TouchMessageDialog.Show(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2), Resources.CouldNotLoadHardwareSetupWithError + Environment.NewLine + ex.Message, MessageBoxButtons.OK, MessageDialogType.Attention) == DialogResult.OK))
                    {
                        throw new OperationCanceledException();
                    }
                }
            }
        }

        public static void LoadInputDevices(IConnectionManager entry)
        {
            ValidateHardwareProfile(entry);
            // Scanner
            bool carryOnTrying = true;
            while (carryOnTrying)
            {
                try
                {
                    Scanner.Load();
                    carryOnTrying = false;
                }
                catch (Exception ex)
                {
                    if (!ProcessError(Properties.Resources.CouldNotLoadScanner.Replace("#1", ex.Message)))
                    {
                        throw;
                    }
                }
            }
            // eTax
            carryOnTrying = true;
            while (carryOnTrying)
            {
                try
                {
                    ETax.Load();
                    carryOnTrying = false;
                }
                catch (Exception ex)
                {
                    if (!ProcessError(Properties.Resources.CouldNotLoadETax.Replace("#1", ex.Message)))
                    {
                        throw;
                    }
                }
            }
            // MSR
            carryOnTrying = true;
            while (carryOnTrying)
            {
                try
                {
                    MSR.Load();
                    carryOnTrying = false;
                }
                catch (Exception ex)
                {
                    if (!ProcessError(Properties.Resources.CouldNotLoadMSR.Replace("#1", ex.Message)))
                    {
                        throw;
                    }
                }
            }
            // RFID scanner
            carryOnTrying = true;
            while (carryOnTrying)
            {
                try
                {
                    RFIDScanner.Load();
                    carryOnTrying = false;
                }
                catch (Exception ex)
                {
                    if (!ProcessError(Properties.Resources.CouldNotLoadRFIDScanner.Replace("#1", ex.Message)))
                    {
                        throw;
                    }
                }
            }
            //Dallas key
            carryOnTrying = true;
            while (carryOnTrying)
            {
                try
                {
                    DallasKey.Load();
                    carryOnTrying = false;
                }
                catch (Exception ex)
                {
                    if (!ProcessError(Properties.Resources.CouldNotLoadDallasKey.Replace("#1", ex.Message)))
                    {
                        throw;
                    }
                }
            }
        }

        static public void UnloadDevices(IConnectionManager entry)
        {
            try
            {
                Printer.Unload(entry);
                Scanner.Unload();
                ETax.Unload();
                LineDisplay.Unload();

                IScaleService service = (IScaleService)entry.Service(ServiceType.ScaleService);
                service.UnloadPeripheral();

                Keylock.Unload();
                MSR.Unload();
                CashDrawer.Unload();
                //LSRetail.Forecourt.ForecourtManager.Unload();
                RFIDScanner.Unload();
                DallasKey.UnLoad();
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, Resources.PeripheralsDevicesUnloadDevicesErrorLogger, x);
                throw x;
            }
        }
    }
}
