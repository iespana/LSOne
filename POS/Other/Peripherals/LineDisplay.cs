using System;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Peripherals.OPOS;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    static public class LineDisplay
    {
        static internal OPOSLineDisplay posLineDisplay;
        static private LineDisplaySimulator dialog;
        private static bool deviceActive;
        
        static public void Load()
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType != HardwareProfile.LineDisplayDeviceTypes.OPOS || deviceActive)
                {
                    return;
                }
                posLineDisplay = new OPOSLineDisplay();
                posLineDisplay.DeviceName = DLLEntry.Settings.HardwareProfile.DisplayDeviceName;
                posLineDisplay.CharacterSet = DLLEntry.Settings.HardwareProfile.DisplayCharacterSet;
                posLineDisplay.Initialize();
                deviceActive = true;
            }
            catch (Exception x)
            {
                posLineDisplay = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.Load", x);
                throw x;
            }
        }

        static public void Unload()
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS && deviceActive)
                {
                    posLineDisplay.Finalise();
                    deviceActive = false;
                }
            }
            catch (Exception x)
            {
                posLineDisplay = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.Unload", x);
                throw x;
            }
        }

        static public void DisplayText(string text)
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS)
                {
                    posLineDisplay.ClearText();
                    posLineDisplay.DisplayText(text);
                }
                else if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay)
                {
                    SimulateLineDisplay(-1,text);
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.DisplayText", x);
                throw x;
            }
        }

        static public void DisplayWelcomeMessage(string line1Text, string line2Text)
        {
            if (line1Text.Trim() == "" && line2Text.Trim() == "")
            {
                line1Text = Properties.Resources.WelcomeTo;
                line2Text = DLLEntry.Settings.Store.Text;
            }

            DisplayText(line1Text, line2Text);
        }

        static public void DisplayText(string line1Text, string line2Text)
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS)
                {
                    posLineDisplay.ClearText();
                    posLineDisplay.DisplayTextAt(line1Text, 0, 0);
                    posLineDisplay.DisplayTextAt(line2Text, 1, 0);
                }
                else if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay)
                {
                    SimulateLineDisplay(1, line1Text);
                    SimulateLineDisplay(2, line2Text);
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.DisplayText", x);
                throw x;
            }
        }

        static public void ClearText()
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS)
                {
                    posLineDisplay.ClearText(true);
                }
                else if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay)
                {
                    SimulateLineDisplay(1,"");
                    SimulateLineDisplay(2, "");
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.ClearText", x);
                throw x;
            }
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        private static void DisplayScaleItem(SaleLineItem item, int lineLength, out string line1, out string line2)
        {
            IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);
            decimal amtToDisplay = item.GetCalculatedNetAmount(DLLEntry.Settings.Store.DisplayBalanceWithTax);
            string itemAmountStr = rounding.RoundForDisplay(DLLEntry.DataModel,
                                                                    item.Quantity < 0 ? (amtToDisplay * -1) / item.Quantity : amtToDisplay / item.Quantity,
                                                                    true,
                                                                    false,
                                                                    DLLEntry.Settings.Store.Currency,
                                                                    CacheType.CacheTypeTransactionLifeTime) + "/" + item.SalesOrderUnitOfMeasureName;
            itemAmountStr = itemAmountStr.Replace(" ", "");
            if (item.Description.Length > (lineLength - itemAmountStr.Length))
            {
                line1 = item.Description.Substring(0, lineLength - itemAmountStr.Length - 1) + " " + itemAmountStr;
            }
            else
            {
                line1 = item.Description.PadRight(lineLength - itemAmountStr.Length) + " " + itemAmountStr;
            }
            line2 = ((IScaleService)DLLEntry.DataModel.Service(ServiceType.ScaleService)).GetScaleDisplayInformation(DLLEntry.DataModel, item, lineLength);
        }

        static public void DisplayItem(RetailTransaction transaction, SaleLineItem saleLineItem, bool voidItemOperation = false)
        {
            if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.None)
            {
                return;
            }

            // Clone to prevent side-effects on the original item
            // For assembly components, display information for the assembly item it belongs to
            saleLineItem = saleLineItem.IsAssemblyComponent ? (SaleLineItem)saleLineItem.GetRootAssemblyLineItem()?.Clone() : (SaleLineItem)saleLineItem.Clone();
            if (saleLineItem == null)
            { 
                return; 
            }

            int multiply = 1;
            if (voidItemOperation && saleLineItem.Voided)
            {
                saleLineItem.Quantity *= -1;
                multiply = saleLineItem.Quantity > decimal.Zero && saleLineItem.GetCalculatedNetAmount(DLLEntry.Settings.Store.DisplayBalanceWithTax) < decimal.Zero ? 
                    -1 : 
                    saleLineItem.Quantity < decimal.Zero && saleLineItem.GetCalculatedNetAmount(DLLEntry.Settings.Store.DisplayBalanceWithTax) > decimal.Zero ? -1 : 1;
            }

            string line1 = "";
            string line2 = "";
            // Form the lower line 
            int lineLength = (posLineDisplay != null) ? posLineDisplay.Columns : 20;

            IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

            if ((saleLineItem.ScaleItem) || (Math.Abs(saleLineItem.Quantity) != 1))
            {
                DisplayScaleItem(saleLineItem, lineLength, out line1, out line2);
            }
            else
            {
                // line 1 calculation
                line1 = saleLineItem.Description.Length > lineLength ? saleLineItem.Description.Substring(0, lineLength) : saleLineItem.Description;

                // line 2 calculation
                string qty = rounding.RoundQuantity(
                    DLLEntry.DataModel,
                    saleLineItem.Quantity,
                    saleLineItem.SalesOrderUnitOfMeasure,
                    false,
                    DLLEntry.Settings.Store.Currency,
                    true,
                    CacheType.CacheTypeTransactionLifeTime);

                string itemAmount = rounding.RoundForDisplay(
                    DLLEntry.DataModel,
                    saleLineItem.GetCalculatedNetAmount(DLLEntry.Settings.Store.DisplayBalanceWithTax) * multiply, 
                    true, 
                    false, 
                    DLLEntry.Settings.Store.Currency, 
                    CacheType.CacheTypeTransactionLifeTime);

                itemAmount = itemAmount.Replace(" ", "");
                if ((qty.Length + itemAmount.Length) > lineLength)
                {
                    line2 = qty.Substring(0, lineLength - itemAmount.Length) + itemAmount;
                }
                else
                {
                    line2 = qty + itemAmount.PadLeft(lineLength - qty.Length);
                }
            }

            if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS)
            {
                lock (posLineDisplay)
                {
                    // Line removed to prevent items not being shown on the line display.
                    // If you notice flickering on the line display, this line should be re-added but
                    //posLineDisplay.ClearText();
                    posLineDisplay.DisplayTextAt(line1.PadRight(lineLength), 0, 0);
                    posLineDisplay.DisplayTextAt(line2, 1, 0);
                }
            }
            else if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay)  // Virtual line display
            {
                SimulateLineDisplay(1, line1);
                SimulateLineDisplay(2, line2);
            }            
        }

        static public void DisplayTotal(string amount)
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.None)
                {
                    return;
                }

                // Form the lower line 
                int lineLength = (posLineDisplay != null) ? posLineDisplay.Columns : 20;

                string spaceString = "";
                int spaceLength = 0;

                spaceLength = lineLength - amount.Length;
                spaceString = spaceString.PadRight(spaceLength);

                string line = spaceString + amount;

                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS)
                {
                    posLineDisplay.ClearText();
                    posLineDisplay.DisplayTextAt(DLLEntry.Settings.HardwareProfile.DisplayTotalText, 0, 0);
                    posLineDisplay.DisplayTextAt(line, 1, 0);
                }
                else if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay)
                {
                    SimulateLineDisplay(1, DLLEntry.Settings.HardwareProfile.DisplayTotalText);
                    SimulateLineDisplay(2,line);
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.DisplayTotal", x);
                throw x;
            }
        }

        static public void DisplayBalance(string amount)
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.None)
                {
                    return;
                }

                // Form the lower line 
                int lineLength = (posLineDisplay != null) ? posLineDisplay.Columns : 20;

                if (amount.Length > lineLength)
                {
                    if (amount.Length > 4)
                    {
                        amount = amount.Substring(0,lineLength-3) + "...";
                    }
                    else
                    {
                        amount = amount.Substring(0, lineLength);
                    }
                }

                if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.OPOS)
                {
                    posLineDisplay.ClearText();
                    posLineDisplay.DisplayTextAt(DLLEntry.Settings.HardwareProfile.DisplayBalanceText, 0, 0);
                    posLineDisplay.DisplayTextAt(amount.PadLeft(lineLength), 1, 0);
                }
                else if (DLLEntry.Settings.HardwareProfile.LineDisplayDeviceType == HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay)
                {
                    SimulateLineDisplay(1, DLLEntry.Settings.HardwareProfile.DisplayBalanceText);
                    SimulateLineDisplay(2, amount.PadLeft(lineLength));
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.LineDisplay.DisplayBalance", x);
                throw x;
            }
        }

        private static void SimulateLineDisplay(int line, string text)
        {
            if (dialog == null)
            {
                dialog = new LineDisplaySimulator();
                dialog.Show();
            }
            if (line < 0)
            {
                dialog.SetText(text);
            }
            else
            {
                dialog.SetText(line, text);
            }
        }
    }
}
