using System;
using System.Runtime.Versioning;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public class ScaleService : IScaleService
    {
        private string certificationNumber;        

        public const decimal GramToKiloGramFactor = 0.001M;
        public const decimal KiloGramToGramFactor = 1000M;
        public const decimal OuncesToPoundsFactor = 1 / 12M;
        public const decimal PoundsToOuncesFactor = 12M;
        public const decimal GramsToPounds = 0.002205M;
        public const decimal PoundsToGrams = 453.6M;
        public const decimal KiloGramsToPounds = 2.205M;
        public const decimal PoundsToKiloGrams = 0.4536M;
        public const decimal KiloGramsToOunces = 35.273962M;
        
        public static string NotAvailable = Resources.NotAvailable;

        public IErrorLog ErrorLog { set; private get; }

        public Version GetVersion
        {
            get
            {
                return new Version(1,0);
            }
        }        


        public string CertificationNumber
        {
            get { return string.IsNullOrEmpty(certificationNumber) ? NotAvailable : certificationNumber; }            
        }

        public bool HasCertificationNumber
        {
            get { return !(CertificationNumber == NotAvailable); }
        }

        private int scaleUnit = 0;

        public void Init(IConnectionManager entry)
        {
            DLLEntry.DataModel = entry;
            certificationNumber = "";
        }
        
        public void SetCertificationNumber(string certificationNumber)
        {            
            this.certificationNumber = certificationNumber;            
        }
        

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public string GetScaleDisplayInformation(IConnectionManager entry, ISaleLineItem saleLineItem, int columnLength)
        {
            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            string qty = rounding.RoundQuantity(
                entry,
                saleLineItem.Quantity,
                saleLineItem.SalesOrderUnitOfMeasure,
                false,
                DLLEntry.Settings.Store.Currency,
                true,
                CacheType.CacheTypeTransactionLifeTime);


            string totalAmount = rounding.RoundForDisplay(entry,
                                                          saleLineItem.GetCalculatedNetAmount(DLLEntry.Settings.Store.DisplayBalanceWithTax), 
                                                          true, 
                                                          false, 
                                                          DLLEntry.Settings.Store.Currency, 
                                                          CacheType.CacheTypeTransactionLifeTime);
            totalAmount = totalAmount.Replace(" ", "");

            string retString;

            if (((SaleLineItem)saleLineItem).ScaleItem && ((SaleLineItem)saleLineItem).WeightManuallyEntered)
            {
                qty = "MAN" + qty;
            }

            if (qty.Length + totalAmount.Length > columnLength)
            {
                retString = qty.Substring(0, columnLength - totalAmount.Length) + totalAmount;
            }
            else
                retString = qty + totalAmount.PadLeft(columnLength - (qty).Length);



            return retString;
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public string GetScalePrintInformation(IConnectionManager entry, ISaleLineItem saleLineItem, int columnLength)
        {
            if (((SaleLineItem)saleLineItem).SplitItem || !((SaleLineItem)saleLineItem).ShouldCalculateAndDisplayAssemblyPrice())
            {
                return "";
            }

            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            string qty = rounding.RoundQuantity(
                entry,
                saleLineItem.Quantity,
                saleLineItem.SalesOrderUnitOfMeasure,
                saleLineItem.LimitationSplitChildLineId > 0 || saleLineItem.LimitationSplitParentLineId > 0,
                DLLEntry.Settings.Store.Currency,
                true,
                CacheType.CacheTypeTransactionLifeTime);
            string itemAmount = " x " + rounding.RoundForDisplay(entry, 
                                                                 DLLEntry.Settings.Store.DisplayAmountsWithTax ? saleLineItem.PriceWithTax : saleLineItem.Price, 
                                                                 true, 
                                                                 false, 
                                                                 DLLEntry.Settings.Store.Currency, 
                                                                 CacheType.CacheTypeTransactionLifeTime) 
                                                                 + "/" + ((SaleLineItem)saleLineItem).SalesOrderUnitOfMeasureName;

            string tareWeightString = "";
            if (saleLineItem.TareWeight > 0)
            {
                string scaleUnitKiloGram = (string)DLLEntry.Settings.Parameters.ScaleKiloGramUnit;
                decimal tareWeightWeight = saleLineItem.SalesOrderUnitOfMeasure == scaleUnitKiloGram ? saleLineItem.TareWeight / 1000M : saleLineItem.TareWeight;

                string tareWeight = rounding.RoundQuantity(entry, tareWeightWeight, saleLineItem.SalesOrderUnitOfMeasure, false, DLLEntry.Settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);

                tareWeightString = " " + Properties.Resources.Tare + " " + tareWeight;
            }

            string retString;
            if (qty.Length + itemAmount.Length > columnLength)
            {
                retString = qty.Substring(0, columnLength - itemAmount.Length) + itemAmount;
            }
            else
            {
                retString = qty + itemAmount + tareWeightString;
            }

            return retString;
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public ScaleUnit GetCurrentScaleUnit()
        {
            if (scaleUnit == 0)
            {
                scaleUnit = Scale.GetScaleUnit();
            }
            return (ScaleUnit)scaleUnit;
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public decimal ConvertScaleUnits(ScaleUnit fromUnit, ScaleUnit toUnit, decimal quantity)
        {
            if (fromUnit == toUnit)
            {
                return quantity;
            }
            if (fromUnit == ScaleUnit.Gram && toUnit == ScaleUnit.KiloGram)
            {
                return quantity * GramToKiloGramFactor;
            }
            if (fromUnit == ScaleUnit.KiloGram && toUnit == ScaleUnit.Gram)
            {
                return quantity * KiloGramToGramFactor;
            }
            if (fromUnit == ScaleUnit.Ounce && toUnit == ScaleUnit.Pound)
            {
                return quantity * OuncesToPoundsFactor;
            }
            if (fromUnit == ScaleUnit.Pound && toUnit == ScaleUnit.Ounce)
            {
                return quantity * PoundsToOuncesFactor;
            }
            if (fromUnit == ScaleUnit.Gram && toUnit == ScaleUnit.Pound)
            {
                return quantity * GramsToPounds;
            }
            if (fromUnit == ScaleUnit.Pound && toUnit == ScaleUnit.Gram)
            {
                return quantity * PoundsToGrams;
            }
            if (fromUnit == ScaleUnit.KiloGram && toUnit == ScaleUnit.Pound)
            {
                return quantity * KiloGramsToPounds;
            }
            if (fromUnit == ScaleUnit.Pound && toUnit == ScaleUnit.KiloGram)
            {
                return quantity * PoundsToKiloGrams;
            }
            if (fromUnit == ScaleUnit.KiloGram && toUnit == ScaleUnit.Ounce)
            {
                return quantity * KiloGramsToOunces;
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public ScaleUnit FromRecordIdentifier(RecordIdentifier ID)
        {
            if (ID == DLLEntry.Settings.Parameters.ScaleGramUnit)
            {
                return ScaleUnit.Gram;
            }
            if (ID == DLLEntry.Settings.Parameters.ScaleKiloGramUnit)
            {
                return ScaleUnit.KiloGram;
            }
            if (ID == DLLEntry.Settings.Parameters.ScaleOunceUnit)
            {
                return ScaleUnit.Ounce;
            }
            if (ID == DLLEntry.Settings.Parameters.ScalePoundUnit)
            {
                return ScaleUnit.Pound;
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public bool IsScaleUnit(RecordIdentifier ID)
        {
            return ID == DLLEntry.Settings.Parameters.ScaleGramUnit
                   || ID == DLLEntry.Settings.Parameters.ScaleKiloGramUnit
                   || ID == DLLEntry.Settings.Parameters.ScaleOunceUnit
                   || ID == DLLEntry.Settings.Parameters.ScalePoundUnit;
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public void LoadPeripheral()
        {
            Scale.Load();
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public void UnloadPeripheral()
        {
            Scale.Unload();
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public bool ReadFromScaleEx(out int weight, int timeout, string weightUnit, string description, decimal unitPrice, int tareWeight, out decimal salesPrice)
        {
            return Scale.ReadFromScaleEx(out weight, timeout, weightUnit, description, unitPrice, tareWeight, out salesPrice);
        }

        public void EnableScale(bool enable)
        {
            Scale.EnableScale(enable);
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public bool ScaleDeviceActive
        {
            get { return Scale.DeviceActive; }
        }

        public int GetTareWeight()
        {
            return Scale.GetTareWeight();
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public event WeightResult DataMessage
        {
            add
            {
                Scale.ScaleDataMessageEventX += new Scale.ScaleDataMessageDelegateX(value);
            }
            remove
            {
                Scale.ScaleDataMessageEventX -= new Scale.ScaleDataMessageDelegateX(value);
            }
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public event ScaleMessage ErrorMessage
        {
            add
            {
                Scale.ScaleErrorMessageEventX += new Scale.ScaleErrorMessageDelegateX(value);
            }
            remove
            {
                Scale.ScaleErrorMessageEventX -= new Scale.ScaleErrorMessageDelegateX(value);
            }
        }

        /// <summary>
        /// Exposes the <see cref="Scale"/> properties as json, including the underlying <see cref="LSOne.Services.OPOS.OPOSScale"/>.
        /// Useful for logging and debugging purposes.
        /// </summary>
        /// <returns></returns>
        public string ScaleToJsonString()
        {
            return Scale.ToJsonString();
        }
    }
}
