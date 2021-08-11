using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.KDSBusinessObjects;
using DisplayModeEnum = LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayStation.DisplayModeEnum;


namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    /// <summary>
    /// Wrapper class between UI code and KDS data field properties in a KitchenDisplayLineColumn object.
    /// Internally holds the three parameters that are used by the KDS to determins which field to display
    /// in one column in a chit header/footer or a column in a line display.
    /// </summary>
    public class KitchenDisplayColumnField : IDataEntity
    {
        public enum DataType
        {
            KOTData = 1,
            KDSData = 2
        }

        private enum IdentifierType
        {
            MappingKey,
            OrderProperty,
            None
        };

        public PartTypeEnum KDSType { get; private set; }
        public PartOrderPropertyEnum KDSOrderProperty { get; private set; }
        public string KDSMappingKey { get; private set; }

        /// <summary>
        /// Create an empty column field object
        /// Can be used to indicate no field selected in the UI
        /// 
        /// An object created by this constructor returns an empty string as ID
        /// </summary>
        public KitchenDisplayColumnField()
        {
            KDSType = PartTypeEnum.Generic;
            KDSOrderProperty = PartOrderPropertyEnum.NULL;
            KDSMappingKey = "";
        }

        /// <summary>
        /// Create a column field object and initialize it with parameters from a display line column object
        /// </summary>
        /// <param name="column">display line column object from KDS</param>
        public KitchenDisplayColumnField(KitchenDisplayLineColumn column)
        {
            KDSType = column.Type;
            KDSOrderProperty = column.OrderProperty;
            KDSMappingKey = column.MappingKey ?? "";
        }

        /// <summary>
        /// Create a column field object from the KDS column properties that determine which field to use
        /// </summary>
        /// <param name="type">Determines which field to use. If its value is Generic, 
        /// then the data type is shown as "KOT Data" and the orderProperty and mappingKey 
        /// parameters determine which field to use</param>
        /// <param name="orderProperty">Determines which field to use if the value of the type parameter is Generic 
        /// and the mappingKey parameter is an empty string</param>
        /// <param name="mappingKey">Custom string key which must then also be used 
        /// when the data is sent in the KOT 
        /// (calling SetDataField with this key on the order).
        /// This parameter is ignored unless the value of the type parameter is Generic.</param>
        private KitchenDisplayColumnField(PartTypeEnum type, PartOrderPropertyEnum orderProperty, string mappingKey)
        {
            KDSType = type;
            KDSOrderProperty = orderProperty;
            KDSMappingKey = mappingKey;
        }

        /// <summary>
        /// Type of data field, KOTData or KDSData
        /// </summary>
        public DataType Type => KDSType == PartTypeEnum.Generic ? DataType.KOTData : DataType.KDSData;

        /// <summary>
        /// String value of the data type, for use in the UI
        /// </summary>
        /// <returns>The display name for the data type</returns>
        public string GetDataTypeName()
        {
            switch (Type)
            {
                case DataType.KOTData:
                    return Properties.Resources.KOTData;

                case DataType.KDSData:
                    return Properties.Resources.KDSData;

                default:
                    return "";
            }
        }

        /// <summary>
        /// Identifier for the field to use in the UI code
        /// Calculated from the underlying KDS line column properties
        /// Combined with the Type propertys it gives a unique ID for each field value
        /// </summary>
        public RecordIdentifier ID
        {
            get
            {
                if (Type == DataType.KDSData)
                {
                    return new RecordIdentifier((int)KDSType);
                }
                else if (Type == DataType.KOTData)
                {
                    switch (GetIdType())
                    {
                        case IdentifierType.MappingKey:
                            return new RecordIdentifier((int)IdentifierType.MappingKey, KDSMappingKey);

                        case IdentifierType.OrderProperty:
                            return new RecordIdentifier((int)IdentifierType.OrderProperty, (int)KDSOrderProperty);
                    }
                }

                return new RecordIdentifier("");
            }

            set
            {

            }
        }

        /// <summary>
        /// Display string for use in the UI saying which field this is
        /// </summary>
        public string Text
        {
            get
            {
                if (Type == DataType.KDSData)
                {
                    return PartTypeToText(KDSType);
                }
                else if (Type == DataType.KOTData)
                {
                    switch (GetIdType())
                    {
                        case IdentifierType.MappingKey:
                            return MappingKeyToText(KDSMappingKey);


                        case IdentifierType.OrderProperty:
                            return PartOrderPropertyToText(KDSOrderProperty);
                    }
                }

                return "";
            }

            set { }
        }

        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// Determines whether a mapping key or order property is used as identifier for the field
        /// </summary>
        /// <returns></returns>
        private IdentifierType GetIdType()
        {
            if (KDSType != PartTypeEnum.Generic)
            {
                return IdentifierType.None;
            }

            return KDSMappingKey != "" ? IdentifierType.MappingKey : IdentifierType.OrderProperty;
        }

        /// <summary>
        /// Gives the default value for the Type field - for use in the UI when no type has been selected
        /// </summary>
        /// <returns>a data entity containing the ID and text representation of the default type</returns>
        public static DataEntity GetDefaultType()
        {
            return new DataEntity((int)DataType.KOTData, Properties.Resources.KOTData);
        }

        /// <summary>
        /// Returns a list of all exisitng type values, ready to use as data in a DualDataComboBox.
        /// </summary>
        /// <returns>A list of all type values (id + text)</returns>
        public static IEnumerable<IDataEntity> GetTypeList()
        {
            return new List<IDataEntity> {
                    new DataEntity((int)DataType.KDSData, Properties.Resources.KDSData),
                    new DataEntity((int)DataType.KOTData, Properties.Resources.KOTData)
                };
        }

        /// <summary>
        /// Returns a list of all available field values given the type and display mode,
        /// ordered alphabetically by the field name and ready to use as data in a DualDataComboBox.
        /// </summary>
        /// <param name="fieldType">value of the Type property (KOT data or KDS data)</param>
        /// <param name="displayMode">Line display or Chit display</param>
        /// <returns>A list of all field values for the given combination of type and display mode</returns>
        public static IEnumerable<IDataEntity> GetList(RecordIdentifier fieldType, DisplayModeEnum displayMode)
        {
            List<IDataEntity> data = new List<IDataEntity>();

            switch ((DataType)(int)fieldType)
            {
                case DataType.KOTData:
                    mappingKeys.ForEach(key => data.Add(new KitchenDisplayColumnField(PartTypeEnum.Generic, PartOrderPropertyEnum.NULL, key)));
                    orderProperties.ForEach(prop => data.Add(new KitchenDisplayColumnField(PartTypeEnum.Generic, prop, "")));

                    if (displayMode == DisplayModeEnum.LineDisplay)
                    {
                        orderPropertiesLineDisplayOnly.ForEach(prop => data.Add(new KitchenDisplayColumnField(PartTypeEnum.Generic, prop, "")));
                    }
                    break;

                case DataType.KDSData:
                    kdsDataFields.ForEach(type => data.Add(new KitchenDisplayColumnField(type, PartOrderPropertyEnum.NULL, "")));
                    if (displayMode == DisplayModeEnum.LineDisplay)
                    {
                        kdsDataFieldsLineDisplayOnly.ForEach(type => data.Add(new KitchenDisplayColumnField(type, PartOrderPropertyEnum.NULL, "")));

                    }
                    break;
            }

            return data.OrderBy(o => o.Text);
        }

        private static readonly List<string> mappingKeys = new List<string> { 
            "CustomerName", "EmployeeName", "POSOrderStatus" 
        };

        private static List<PartOrderPropertyEnum> orderProperties = new List<PartOrderPropertyEnum>
        {
            PartOrderPropertyEnum.ID, 
            PartOrderPropertyEnum.TableNumber
        };

        private static readonly List<PartOrderPropertyEnum> orderPropertiesLineDisplayOnly = new List<PartOrderPropertyEnum>
        {
            PartOrderPropertyEnum.ItemModifiers, 
            PartOrderPropertyEnum.ItemQuantity, 
            PartOrderPropertyEnum.ItemText
        };

        private static readonly List<PartTypeEnum> kdsDataFields = new List<PartTypeEnum> 
        {
            PartTypeEnum.CountDownClock, 
            PartTypeEnum.CountDownClockDisplayCountUp, 
            PartTypeEnum.StationStatus
        };

        private static readonly List<PartTypeEnum> kdsDataFieldsLineDisplayOnly = new List<PartTypeEnum> 
        {
            PartTypeEnum.ItemTimeOnDisplayClock 
        };

        private static string PartTypeToText(PartTypeEnum type)
        {
            switch (type)
            {
                case PartTypeEnum.CountDownClock:
                    return Properties.Resources.ProductionTime;
                case PartTypeEnum.CountDownClockDisplayCountUp:
                    return Properties.Resources.ActualProductionTime;
                case PartTypeEnum.StationStatus:
                    return Properties.Resources.StationLetterStatus;
                case PartTypeEnum.ItemTimeOnDisplayClock:
                    return Properties.Resources.ItemTimeOnStation;
                default:
                    return "";
            }
        }

        private static string PartOrderPropertyToText(PartOrderPropertyEnum property)
        {
            switch (property)
            {
                case PartOrderPropertyEnum.ID:
                    return Properties.Resources.ID;
                case PartOrderPropertyEnum.TableNumber:
                    return Properties.Resources.TableNumber;
                case PartOrderPropertyEnum.ItemQuantity:
                    return Properties.Resources.ItemQuantity;
                case PartOrderPropertyEnum.ItemText:
                    return Properties.Resources.ItemName;
                case PartOrderPropertyEnum.ItemModifiers:
                    return Properties.Resources.ItemModifiers;
                default:
                    return "";
            }
        }

        private static string MappingKeyToText(string key)
        {
            switch (key)
            {
                case "CustomerName":
                    return Properties.Resources.CustomerName;
                case "EmployeeName":
                    return Properties.Resources.EmployeeName;
                case "POSOrderStatus":
                    return Properties.Resources.OrderStatus;
                default:
                    return "";
            }
        }

        public virtual string this[int index]
        {
            get { return (index == 0) ? ID.ToString() : ((index == 1) ? Text : ""); }
        }

        public virtual object this[string field] { get { return null; } set { } }

        public UsageIntentEnum UsageIntent
        {
            get
            {
                return UsageIntentEnum.Normal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void ToClass(System.Xml.Linq.XElement xmlAnswer, IErrorLog errorLogger = null)
        {
            throw new NotImplementedException();
        }

        public System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
        {
            throw new NotImplementedException();
        }
    }
}
