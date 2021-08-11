using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    public static class StringFunctions
    {
        public static string GetCorrectStringSize(string tempString, int size, PadDirection padDir)
        {
            if (tempString.Length > size)
            {
                if (padDir == PadDirection.Left)
                    tempString = tempString.Substring(tempString.Length - size, size);
                else
                    tempString = tempString.Substring(0, size);
            }
            return tempString;
        }

        public static string FormatString(IConnectionManager entry, ISettings settings, Decimal amount, int padSize, PadDirection direction = PadDirection.Left)
        {
            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            string tempString = rounding.RoundForDisplay(
                entry,
                amount,
                false,
                true,
                settings.Store.Currency,
                CacheType.CacheTypeTransactionLifeTime);

            tempString = GetCorrectStringSize(tempString, padSize, direction);
            return tempString.PadLeft(padSize);
        }

        public static string FormatString(String tempString, int padSize, PadDirection padDir)
        {
            tempString = GetCorrectStringSize(tempString, padSize, padDir);
            if (padDir == PadDirection.Right)
                return tempString.PadRight(padSize);
            else
                return tempString.PadLeft(padSize);
        }

        public static string FormatString(String tempString, int padSize, PadDirection padDir, char paddingChar)
        {
            tempString = GetCorrectStringSize(tempString, padSize, padDir);
            if (padDir == PadDirection.Right)
                return tempString.PadRight(padSize, paddingChar);
            else
                return tempString.PadLeft(padSize, paddingChar);
        }

        public static string FormatString(String tempString, int padSize)
        {
            return FormatString(tempString, padSize, PadDirection.Right);
        }

        public static string FormatString(int number, int padSize, PadDirection padDir)
        {
            string tempString = GetCorrectStringSize(number.ToString(), padSize, padDir);
            if (padDir == PadDirection.Left)
                return tempString.PadLeft(padSize);
            else
                return tempString.PadRight(padSize);
        }
        
    }
}
