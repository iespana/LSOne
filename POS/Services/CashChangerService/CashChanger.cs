using System;
using System.Linq;
using System.Windows.Forms;
using cgactivexapi;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Core;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class CashChangerService : ICashChangerService
    {
        protected cgactivexapi.clsCashGuardClass cGActiveXApi = null;
        protected bool cGInitiated = false;
        protected bool autoMode = false;
        public event InsertedAmountDelegate InsertedAmount;
        public event LevelStatusDelegate LevelStatusEvent;
        public event ErrorEventDelegate ErrorEvent;

        #region ICashChanger Members

        public virtual bool GetAutoMode(IConnectionManager entry)
        {
            return autoMode;
        }

        public virtual void SetAutoMode(IConnectionManager entry, bool mode)
        {
            autoMode = mode;
        }

        /// <summary>
        /// Used to create an instance of the ActiveX API and carry out the
        /// initialize funtion.
        /// </summary>
        /// <returns>Returns true if init function returns OK</returns>
        public virtual bool Initialize(IConnectionManager entry)
        {
            ConnectionEntry = entry;

            ConnectionSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            cGInitiated = false;
            try
            {
                if (cGActiveXApi == null)
                {
                    if (!cGInitiated)
                    {
                        POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.InitializingCashGuard); 

                        cGActiveXApi = new cgactivexapi.clsCashGuardClass();
                        cGActiveXApi.CGStatusEvent +=
                            new __clsCashGuard_CGStatusEventEventHandler(cGActiveXApi_CGStatusEvent);
                        cGActiveXApi.CGLevelWarningEvent +=
                            new __clsCashGuard_CGLevelWarningEventEventHandler(cGActiveXApi_CGLevelWarningEvent);
                        cGActiveXApi.CGErrorEvent +=
                            new __clsCashGuard_CGErrorEventEventHandler(cGActiveXApi_CGErrorEvent);
                    }
                }
                if (cGActiveXApi != null)
                {   // instanciated?
                    string cGAnswer = "";
                    CashGuardReturn result = (CashGuardReturn)cGActiveXApi.initCG(
                        ConnectionSettings.HardwareProfile.CashChangerPortSettings,
                        ConnectionSettings.HardwareProfile.CashChangerInitSettings,
                        ref cGAnswer);
                    cGAnswer = cGAnswer.Trim('\0');
                    if (result == CashGuardReturn.CG_STATUS_OK)
                    {
                        cGInitiated = true;
                    }
                    else
                    {
                        LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                        
                    }
                }
                return cGInitiated;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The login function is used to register the user with the Cash guard machine
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalID"></param>
        /// <param name="operatorID"></param>
        /// <returns>Returns whether the function was successfully completed</returns>
        public virtual bool Login(IConnectionManager entry, string terminalID, string operatorID)
        {

            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                string cGAnswer = "";
                CashGuardReturn result = (CashGuardReturn)cGActiveXApi.loginStrCG(terminalID, operatorID, ref cGAnswer);
                cGAnswer = cGAnswer.Trim('\0');
                if (result == CashGuardReturn.CG_STATUS_OK)
                {
                    POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                    return true;
                }
                else
                {                    
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                    ErrorEvent(0, "");
                }
            }
            return false;
        }

        /// <summary>
        /// Logout is the oposite to Login
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Returns whether the function was successfully completed</returns>
        public virtual bool Logout(IConnectionManager entry)
        {
            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                string cGAnswer = "";
                CashGuardReturn result = (CashGuardReturn)cGActiveXApi.logoutCG(ref cGAnswer);
                cGAnswer = cGAnswer.Trim('\0');
                if ((result == CashGuardReturn.CG_STATUS_OK) || (result == CashGuardReturn.CG_STATUS_CLOSED))
                {
                    POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                    return true;
                }
                else
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                }
            }
            return false;
        }

        /// <summary>
        /// This functions registers in to the transaction the amount that has been entered
        /// into the cash machine.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="amountDue">The amount that should be paid by the customer</param>
        /// <param name="receiptID">The receipt ID of the transaction</param>
        /// <param name="amountRest">If not enough amount has been entered, this parameter will specify what is left to pay</param>
        /// <returns>An instance of the CashGuardReturn enum specifying the </returns>
        public virtual CashGuardReturn RegisterAmount(IConnectionManager entry, decimal amountDue, string receiptID, ref decimal amountRest)
        {
            CashGuardReturn result = CashGuardReturn.CG_STATUS_ERROR;

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                string cGAnswer = "";
                int amountRestTemp = (int)amountRest;
                result = (CashGuardReturn)cGActiveXApi.amountDueCG((int)(amountDue * 100), receiptID, ref amountRestTemp, ref cGAnswer);
                amountRest = amountRestTemp / 100;

                cGAnswer = cGAnswer.Trim('\0');
                switch (result)
                {
                    case CashGuardReturn.CG_STATUS_OK:
                        {
                            POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                            return result;
                        }
                    case CashGuardReturn.CG_STATUS_PAYOUT_REST:
                        {
                            if (autoMode == false)
                            {
                                if (cGAnswer.Contains('('))
                                {
                                    IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                                    cGAnswer = cGAnswer.Substring(0, cGAnswer.IndexOf('('));
                                    cGAnswer += " (" + rounding.RoundString(entry, amountRest, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime) + ")";
                                }
                                else
                                {
                                    cGAnswer += "\r\n" + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString());
                                }
                            }
                            LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                            
                            break;
                        }
                    default:
                        {                            
                            cGAnswer += "\r\n" + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString());
                            LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                            
                            break;
                        }
                }
            }
            return result;
        }

        public virtual bool Change(IConnectionManager entry)
        {
            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                string cGAnswer = "";
                CashGuardReturn result = (CashGuardReturn)cGActiveXApi.changeCG(ref cGAnswer);
                cGAnswer = cGAnswer.Trim('\0');
                if (result == CashGuardReturn.CG_STATUS_OK)
                {
                    POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                    return true;
                }
                else
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                }
            }
            return false;
        }

        public virtual bool Reset(IConnectionManager entry)
        {
            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                string cGAnswer = "";
                CashGuardReturn result = (CashGuardReturn)cGActiveXApi.resetCG(ref cGAnswer);
                cGAnswer = cGAnswer.Trim('\0');
                if (result == CashGuardReturn.CG_STATUS_OK)
                {
                    POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                    return true;
                }
                else
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                }
            }
            return false;
        }

        public virtual bool Regret(IConnectionManager entry, CashGuardRegretType regretType)
        {
            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                string cGAnswer = "";
                CashGuardReturn result = CashGuardReturn.CG_STATUS_ERROR;
                switch (regretType)
                {
                    case CashGuardRegretType.REGRETTYPE_ALL:
                        {
                            result = (CashGuardReturn)cGActiveXApi.regretCG((short)CashGuardRegretType.REGRETTYPE_ALL, ref cGAnswer);
                            break;
                        }
                    case CashGuardRegretType.REGRETTYPE_ONE:
                        {
                            result = (CashGuardReturn)cGActiveXApi.regretCG((short)CashGuardRegretType.REGRETTYPE_ONE, ref cGAnswer);
                            break;
                        }
                }

                cGAnswer = cGAnswer.Trim('\0');
                if (result == CashGuardReturn.CG_STATUS_OK)
                {
                    POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                    return true;
                }
                else
                {
                    LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                    
                }
            }
            return false;

        }

        public virtual bool Exit(IConnectionManager entry)
        {
            if ((cGActiveXApi != null) && (cGInitiated == true))
            {
                // Making sure machine return all the money that has been inserted at this point
                if (Regret(entry, CashGuardRegretType.REGRETTYPE_ALL))
                {
                    string cGAnswer = "";
                    // Making sure user is logged out
                    if (Logout(entry))
                    {
                        CashGuardReturn result = (CashGuardReturn)cGActiveXApi.exitCG(ref cGAnswer);
                        cGAnswer = cGAnswer.Trim('\0');
                        if (result == CashGuardReturn.CG_STATUS_OK)
                        {
                            POSFormsManager.ShowPOSStatusPanelText(cGAnswer);
                            return true;
                        }
                        else
                        {
                            LSOne.Services.Interfaces.Services.DialogService(entry).ShowMessage(cGAnswer + " " + String.Format(Properties.Resources.ErrorNumber,((int)result).ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                            
                        }
                    }
                }

                cGActiveXApi.CGStatusEvent -=
                    new __clsCashGuard_CGStatusEventEventHandler(cGActiveXApi_CGStatusEvent);
                cGActiveXApi.CGLevelWarningEvent -=
                    new __clsCashGuard_CGLevelWarningEventEventHandler(cGActiveXApi_CGLevelWarningEvent);
                cGActiveXApi.CGErrorEvent -=
                    new __clsCashGuard_CGErrorEventEventHandler(cGActiveXApi_CGErrorEvent);
                cGActiveXApi = null;
            }
            return true;
        }

        public virtual void ConcludeTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // Releasing cash from the cash changer if there is something there
            if (settings.HardwareProfile.CashChangerDeviceType != HardwareProfile.CashChangerDeviceTypes.None)
            {
                if (posTransaction.GetType() == typeof(RetailTransaction))
                {
                    if (((RetailTransaction)posTransaction).TenderLines.Count > 0)
                    {
                        TenderLineItem lastTender = (TenderLineItem)(((RetailTransaction)posTransaction).TenderLines.Count > 0 ? ((RetailTransaction)posTransaction).TenderLines[((RetailTransaction)posTransaction).TenderLines.Count - 1] : null);
                        // If any other tender than cash
                        if ((lastTender.TenderTypeId != null) && (lastTender.TenderTypeId != "1"))
                        {
                            // I regret all cash form the cash machine
                            IPosTransaction transaction = posTransaction;
                            settings.POSApp.RunOperation(POSOperations.CashChangerRegret, CashGuardRegretType.REGRETTYPE_ALL, ref transaction);
                            if (transaction == null)
                            {
                                // At this point the POS has already suspended the transaction and all we need to do is to cancel the
                                // current transaction.
                                posTransaction.EntryStatus = TransactionStatus.Cancelled;
                            }
                            else
                            {
                                posTransaction = (PosTransaction)transaction;
                            }

                        }
                    }
                }
            }
        }

        #endregion

        #region CGEvents
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalAmount">Total amount that has been inserted into Cash Guard</param>
        /// <param name="status">0 = STATUS_OK;  1 = STATUS_BUSY;  2 = STATUS_ERROR</param>
        /// <param name="mode">0 = Cash guard closed;  1 = CG Open from CR;  2 = CG Open from BO</param>
        protected virtual void cGActiveXApi_CGStatusEvent(int totalAmount, short status, short mode)
        {
            switch ((CashGuardStaus)status)
            {
                case CashGuardStaus.CG_STATUS_OK:
                    {
                        // this is just done in order to set the reset button in it's original state
                        ErrorEvent(9999, "");
                        if (InsertedAmount != null)
                        {
                            decimal amount = (decimal)totalAmount / 100;

                            InsertedAmount(amount, (CashGuardStaus)status, mode);
                            IRoundingService rounding = (IRoundingService)ConnectionEntry.Service(ServiceType.RoundingService);
                            POSFormsManager.ShowPOSStatusPanelText(String.Format(Properties.Resources.XAddedFromCashGuard, rounding.RoundString(ConnectionEntry, amount, ConnectionSettings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime))); 
                        }
                        break;
                    }
                default:
                    {
                        autoMode = false;
                        InsertedAmount(0, (CashGuardStaus)status, mode);
                        switch ((CashGuardStaus)status)
                        {
                            case CashGuardStaus.CG_STATUS_BUSY:
                                {
                                    // signal cash guard as busy
                                    break;
                                }
                            case CashGuardStaus.CG_STATUS_ERROR: break;
                        }
                        break;
                    }

            }
        }

        protected virtual void cGActiveXApi_CGErrorEvent(int errorCode, string errorText, string extInfo)
        {
            if (ErrorEvent != null)
            {
                ErrorEvent(errorCode, errorText);

                errorText = errorText.Trim('\0') + "\r\n";
                if (errorCode == 0)
                    errorText += Properties.Resources.CashGuardIsClosedSelectLogin + "\r\n";
                
                POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.AnErrorStateDetected);               
                Interfaces.Services.DialogService(ConnectionEntry).ShowMessage(errorText + String.Format(Properties.Resources.ErrorNumber, errorCode.ToString()), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        protected virtual void cGActiveXApi_CGLevelWarningEvent(short warningType, int denomination, short numberOf, string typeString, string denominationString, string warningMessage, string extInfo)
        {
            if (LevelStatusEvent != null)
                LevelStatusEvent((CashGuardWarningType)warningType, denomination, numberOf, typeString, denominationString, warningMessage, extInfo);
        }

        #endregion

        protected virtual IConnectionManager ConnectionEntry { get; set; }

        protected virtual ISettings ConnectionSettings { get; set; }

        public virtual IErrorLog ErrorLog
        {
            set { throw new NotImplementedException(); }
        }

        public void Init(IConnectionManager entry)
        {
            ConnectionEntry = entry;
            ConnectionSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }
    }
}
