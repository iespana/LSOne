using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// The 4 scale units that a weighing instrument can weigh
    /// </summary>
    public enum ScaleUnit
    {
        Gram = 1,
        KiloGram = 2,
        Ounce = 3,
        Pound = 4
    }

    /// <summary>
    /// The event that returns the weight to the POS from the scale
    /// </summary>
    /// <param name="message"></param>
    public delegate void ScaleMessage(string message);

    /// <summary>
    /// The event that returns the quantity to the POS from the scale
    /// </summary>
    /// <param name="quantity"></param>
    public delegate void WeightResult(decimal quantity);

    public interface IScaleService : IService
    {
        /// <summary>
        /// Creates a string to display the weight information on the customer and user facing displays
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The sale line that has the weighing item </param>
        /// <param name="columnLength">How long the string can be</param>
        /// <returns></returns>
        string GetScaleDisplayInformation(IConnectionManager entry, ISaleLineItem saleLineItem, int columnLength);
        /// <summary>
        /// Creates a string to print the weight information on the receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The sale line that has the weighing item </param>
        /// <param name="columnLength">How long the string can be</param>
        /// <returns></returns>
        string GetScalePrintInformation(IConnectionManager entry, ISaleLineItem saleLineItem, int columnLength);
        /// <summary>
        /// Returns true if the unit is a weighing (or scale) unit 
        /// </summary>
        /// <param name="ID">The unique ID of the unit</param>
        /// <returns></returns>
        bool IsScaleUnit(RecordIdentifier ID);
        /// <summary>
        /// Returns an object with information about the scale unit from a unique ID of a unit
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ScaleUnit FromRecordIdentifier(RecordIdentifier ID);
        /// <summary>
        /// Returns the current scale unit of the scale
        /// </summary>
        /// <returns></returns>
        ScaleUnit GetCurrentScaleUnit();
        /// <summary>
        /// Converts the weighing in one scale unit to another
        /// </summary>
        /// <param name="fromUnit"></param>
        /// <param name="toUnit"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        decimal ConvertScaleUnits(ScaleUnit fromUnit, ScaleUnit toUnit, decimal quantity);
        /// <summary>
        /// Returns the current tare weight
        /// </summary>
        /// <returns></returns>
        int GetTareWeight();

        /// <summary>
        /// Returns the current version of the Scale service
        /// </summary>
        Version GetVersion { get; }
        /// <summary>
        /// Returns the current scale certification number
        /// </summary>
        string CertificationNumber { get; }
        /// <summary>
        /// Returns true if the scale service includes a scale certification number
        /// </summary>
        bool HasCertificationNumber { get;  }        
        /// <summary>
        /// Sets the certification number to be displayed in the POS
        /// </summary>
        /// <param name="certificationNumber"></param>

        void SetCertificationNumber(string certificationNumber);

        /// <summary>
        /// Loads the OPOS scale 
        /// </summary>
        void LoadPeripheral();
        /// <summary>
        /// Unloads the OPOS scale
        /// </summary>
        void UnloadPeripheral();

        /// <summary>
        /// Reads the weight from the scale and returns the results.
        /// </summary>
        /// <param name="weight">The weight that was read from the scale in grams</param>
        /// <param name="timeout">The timeout to set on the read operation</param>
        /// <param name="weightUnit">The scale unit that the scale should be set to</param>
        /// <param name="description">The description to send to the connected display</param>
        /// <param name="unitPrice">The unit price for the item that should be weighed</param>
        /// <param name="tareWeight">The tare weight for the item to be weighed</param>
        /// <param name="salesPrice">The calculated sales price from the scale</param>
        /// <returns>True if the scale successfully read the weight, false otherwise. If an error occurred it is raised through an error event</returns>
        bool ReadFromScaleEx(out int weight, int timeout, string weightUnit, string description, decimal unitPrice, int tareWeight, out decimal salesPrice);
        /// <summary>
        /// Returns true if the scale is active
        /// </summary>
        bool ScaleDeviceActive { get; }
        /// <summary>
        /// Enabled or disables the scale for weighing
        /// </summary>
        /// <param name="enable"></param>
        void EnableScale(bool enable);

        event WeightResult DataMessage;
        event ScaleMessage ErrorMessage;
		
        /// <summary>
        /// Returns the Scale properties as json, including the underlying LSOne.Services.OPOS.OPOSScale.
        /// Useful for logging and debugging purposes.
        /// </summary>
        /// <returns></returns>
		string ScaleToJsonString();
    }
}
