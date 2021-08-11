using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetail.Forecourt;

namespace LSOne.DataLayer.TransactionObjects.Line.SaleItem
{
    /// <summary>
    /// A fuel item transaction line.
    /// </summary>
    [Serializable]
    public class FuelSalesLineItem : SaleLineItem
    {
        // Fuelling Transaction
        private IFuellingPointTransaction fuellingPointTransaction;

        #region Properties
        /// <summary>
        /// Which Fuelling Point Transaction class is being used; fpTransaction or fuellingPointTransaction
        /// </summary>
        public FuellingPointType FpType { get; set; }
        /// <summary>
        /// A unique id for the fuelingpoint at the store.
        /// </summary>
        public int FuelingPointId { get; set; }
        /// <summary>
        /// The id of the grade that has been fuelled.
        /// </summary>
        public int GradeId { get; set; }
        /// <summary>
        /// The sequence number of the transaction.
        /// </summary>
        public int TransSeqNo { get; set; }
        /// <summary>
        /// A unique id for the logical nozzle.
        /// </summary>
        public int NozzleId { get; set; }
        /// <summary>
        /// The volume of fuel delivered in the fuelling transaction.
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// The price per volume.
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// The total price charged.
        /// </summary>
        public decimal TotalPrice { get; set; }
        public decimal LineNumber { get; set; }

        public IFuellingPointTransaction FuellingPointTransaction
        {
            get { return fuellingPointTransaction; }
            set
            {
                fuellingPointTransaction = value;
                FpType = FuellingPointType.ForecourtManager;
            }
        }
        #endregion


        public FuelSalesLineItem(BarCode barCode, RetailTransaction transaction)
            : base(barCode, transaction)
        {
            this.Found = barCode.Found;
            this.ItemClassType = SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem;
            fuellingPointTransaction = new FuellingPointTransaction();

            if (barCode.Found)
            {
                this.ItemId = (string)barCode.ItemID;
                this.BarcodeId = (string)barCode.ItemBarCode;
                this.ItemDepartmentId = barCode.ItemDepartmentId;
                this.ItemGroupId = barCode.ItemGroupId;
                this.Description = barCode.Description;
                this.NoDiscountAllowed = barCode.NoDiscountAllowed;
                this.IncludedInTotalDiscount = barCode.IncludedInTotalDiscount;
                this.LineDiscountGroup = barCode.LineDiscountGroup;
                this.MultiLineDiscountGroup = barCode.MultiLineDiscountGroup;
                this.SalesOrderUnitOfMeasure = (string)barCode.UnitID;
                this.Blocked = barCode.Blocked;
                this.DateToBeBlocked = barCode.DateToBeBlocked;
                this.DateToActivateItem = new Date(barCode.DateToBeActivated);

                this.ItemType = (ItemTypeEnum)barCode.ItemType;
            }
        }

        public override object Clone()
        {
            FuelSalesLineItem item = new FuelSalesLineItem(new BarCode(), (RetailTransaction)Transaction);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new FuelSalesLineItem(new BarCode(), (RetailTransaction) transaction);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(FuelSalesLineItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.FuelingPointId = FuelingPointId;
            item.GradeId = GradeId;
            item.TransSeqNo = TransSeqNo;
            item.NozzleId = NozzleId;
            item.Volume = Volume;
            item.UnitPrice = UnitPrice;
            item.TotalPrice = TotalPrice;
            item.FpType = FpType;
            item.FuellingPointTransaction = FuellingPointTransaction;
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xFuel = new XElement("FuelSalesLineItem",
                    new XElement("fuellingPointId", FuelingPointId),
                    new XElement("gradeId", GradeId),
                    new XElement("transSeqNo", TransSeqNo),
                    new XElement("nozzleId", NozzleId),
                    new XElement("volume", Volume.ToString()),
                    new XElement("unitPrice", UnitPrice.ToString()),
                    new XElement("totalPrice", TotalPrice.ToString()),
                    new XElement("fpType", (int)FpType)
                );


                if (fuellingPointTransaction != null)
                {
                    xFuel.Add(fuellingPointTransaction.ToXML());
                }

                xFuel.Add(base.ToXML(errorLogger));
                return xFuel;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "FuelSalesLineItem.ToXML", ex);
                }

                throw ex;
            }
        }

        public override void ToClass(XElement xItem,IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "fuellingPointId":
                                        FuelingPointId = Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "gradeId":
                                        GradeId = Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "transSeqNo":
                                        TransSeqNo = Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "nozzleId":
                                        NozzleId = Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "volume":
                                        Volume = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "unitPrice":
                                        UnitPrice = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "totalPrice":
                                        TotalPrice = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "fpType":
                                        FpType = (FuellingPointType)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "FuellingPointTransaction":
                                        fuellingPointTransaction = new FuellingPointTransaction();
                                        fuellingPointTransaction.ToClass(xVariable);
                                        break;
                                    default:
                                        base.ToClass(xVariable,errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "FuelSalesLineItem:" + xVariable.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "FuelSalesLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
