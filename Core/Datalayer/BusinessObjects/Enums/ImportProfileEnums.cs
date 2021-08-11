using LSOne.DataLayer.BusinessObjects.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Stores the enumaration of import types.
    /// </summary>
    public enum ImportType
    {
        /// <summary>
        /// Stock counting import type.
        /// </summary>
        StockCounting = 0,
        /// <summary>
        /// Serial numbers import type.
        /// </summary>
        SerialNumbers = 1
    }

    public static class ImportTypeHelper
    {
        public static string GetImportTypeString(ImportType importType)
        {
            switch (importType)
            {
                case ImportType.StockCounting:
                    return Resources.StockCountingImport;
                case ImportType.SerialNumbers:
                    return Resources.SerialNumbersImport;
                default:
                    return string.Empty;
            }
        }
    }

    /// <summary>
    /// Stores the enumaration of import fields.
    /// </summary>
    public enum Field
    {
        /// <summary>
        /// Barcode field type
        /// </summary>
        [Description("Barcode")]
        Barcode = 0,

        /// <summary>
        /// ItemId field type
        /// </summary>
        [Description("Item Id")]
        ItemId = 1,

        /// <summary>
        /// UnitId field type
        /// </summary>
        [Description("Unit Id")]
        UnitId = 2,

        /// <summary>
        /// Counted field type
        /// </summary>
        [Description("Counted")]
        Counted = 3,

        /// <summary>
        /// Description field type
        /// </summary>
        [Description("Description")]
        Description = 4,

        /// <summary>
        /// Serial number field
        /// </summary>
        [Description("Serial Number")]
        SerialNumber = 5,

        /// <summary>
        /// Serial number type
        /// </summary>
        [Description("Type")]
        TypeOfSerial = 6
    }

    /// <summary>
    /// Stores the enumeration of import field types.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// String field type
        /// </summary>
        String = 0,

        /// <summary>
        /// Decimal field type
        /// </summary>
        Decimal = 1,
        /// <summary>
        /// Integer field type
        /// </summary>
        Integer = 2

    }
}
