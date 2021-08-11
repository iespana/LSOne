using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using System.Drawing;
using LSRetail.StoreController.BusinessObjects;

namespace LSRetail.StoreController.TouchButtons.Datalayer.DataEntities
{
    internal class TouchLayout : DataEntity
    {
        public override string Text
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }

        public TouchLayout()
        {
            Name = "";
            ButtonGrid1 = "";
            ButtonGrid2 = "";
            ButtonGrid3 = "";
            ButtonGrid4 = "";
            ButtonGrid5 = "";
            ReceiptID = "";
            TotalID = "";
            CustomerLayoutID = "";
            CustomerLayoutXML = null;
            ReceiptItemsLayoutXML = null;
            ReceiptPaymentLayoutXML = null ;
            TotalsLayoutXML = null;
            LayoutXML = null;
            CashChangerLayoutXML = null;
            LogoPictureID = -1;

        }

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RecordIdentifier ButtonGrid1 { get; set; }
        public RecordIdentifier ButtonGrid2 { get; set; }
        public RecordIdentifier ButtonGrid3 { get; set; }
        public RecordIdentifier ButtonGrid4 { get; set; }
        public RecordIdentifier ButtonGrid5 { get; set; }
        public string ReceiptID { get; set; }
        public string TotalID { get; set; }
        public string CustomerLayoutID { get; set; }
        public int LogoPictureID { get; set; }
        public string ImgCustomerLayoutXML { get; set; }
        public string ImgReceiptItemsLayoutXML { get; set; }
        public string ImgReceiptPaymentLayoutXML { get; set; }
        public string ImgTotalsLayoutXML { get; set; }
        public string ImgLayoutXML { get; set; }
        public string CustomerLayoutXML { get; set; }
        public string ReceiptItemsLayoutXML { get; set; }
        public string ReceiptPaymentLayoutXML { get; set; }
        public string TotalsLayoutXML { get; set; }
        public string LayoutXML { get; set; }
        public string ImgCashChangerLayoutXML { get; set; }
        public string CashChangerLayoutXML { get; set; }

    }
}
