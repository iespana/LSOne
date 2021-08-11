using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// A class that holds the current status of the POS i.e. the currently selected item line, tender line, is the POS in return mode and other information
    /// </summary>
    public class OperationInfo
    {
        /// <summary>
        /// If set to false then any properties on this object (f.ex. TenderLineId and ItemLineId) will be used as they set currently on the object instead of getting the actually selected lines from the virtual receipt
        /// This property should ONLY be set to false in customizations and in order to force the POS to run an operation on a specific item within the virtual receipt
        /// </summary>
        public bool RefreshOperationInfo { get; set; }

        /// <summary>
        /// The ID of the blank operation that is being run
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// The parameter provided into the operation when used from the blank operations        
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// The item line that is selected in the virtual receipt in the POS
        /// </summary>
        public int ItemLineId { get; set; }        

        /// <summary>
        /// The payment line that is selected in the virtual receipt in the POS
        /// </summary>
        public int TenderLineId { get; set; }                

        /// <summary>
        /// If set, some operations will use this value within the operation without asking the user for the information
        /// </summary>
        public string NumpadValue { get; set; }                

        /// <summary>
        /// If set, some operations will use this value within the operation without asking the user for the information f.ex. Set Qty operation will set the qty to the value without asking the user
        /// </summary>
        public Decimal NumpadQuantity { get; set; }        
        
        /// <summary>
        /// If true then the POS is in Return item mode
        /// </summary>
        public bool ReturnItems { get; set; }                

        /// <summary>
        /// Used in Hospitality: The ID of the table the order within the POS belongs to
        /// </summary>
        public int HospitalityTableId { get; set; }                

        /// <summary>
        /// If the ItemLineId is a infocode item or linked item then this is the line id of the parent item
        /// </summary>
        public int ParentItemLineId { get; set; }

        /// <summary>
        /// True if a sale operation is a price check or inventory lookup
        /// </summary>
        public bool PriceCheck { get; set; }

        /// <summary>
        /// Set to true if an operation should no longer trigger the result callback that checks if the transaction can be concluded
        /// </summary>
        public bool SkipResultCallback { get; set; }

        public OperationInfo()
        {
            ItemLineId = 0;
            TenderLineId = 0;
            NumpadValue = "";
            NumpadQuantity = 0;
            ReturnItems = false;
            ParentItemLineId = 0;
            RefreshOperationInfo = true;
            OperationId = "";
            Parameter = "";
            SkipResultCallback = false;
        }
    }
}
