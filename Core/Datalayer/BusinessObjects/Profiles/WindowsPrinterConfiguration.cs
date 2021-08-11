using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    [DataContract]
    public class WindowsPrinterConfiguration : DataEntity
    {
        public WindowsPrinterConfiguration()
        {
            PrinterDeviceName = "";
            FontSize = 9.5m;
            WideHighFontSize = 18m;
            FontName = "Courier New";
            LeftMargin = 45;
            RightMargin = 5;
            TopMargin = 5;
            BottomMargin = 60;
            PrintDesignBoxes = false;
            FolderLocation = @"C:\ProgramData\LS Retail\LS One POS\";
        }

        /// <summary>
        /// Name of the printer
        /// </summary>
        [DataMember]
        public string PrinterDeviceName { get; set; }

        /// <summary>
        /// Normal font size. The default value is 9.5
        /// </summary>
        [DataMember]
        public decimal FontSize { get; set; }

        /// <summary>
        /// Wide high font size. The default value is 18
        /// </summary>
        [DataMember]
        public decimal WideHighFontSize { get; set; }

        /// <summary>
        /// Font name. The default value is Courier New
        /// </summary>
        [DataMember]
        public string FontName { get; set; }

        /// <summary>
        /// To test changes to the other Windows printing configurations this property can be set to true to see how the settings are changing
        /// </summary>
        [DataMember]
        public bool PrintDesignBoxes { get; set; }

        /// <summary>
        /// Where the document that is created when printing to a file is going to be saved. The POS and the windows user HAS TO HAVE read and write priviledges to this folder location
        /// </summary>
        [DataMember]
        public string FolderLocation { get; set; }

        /// <summary>
        /// Left margin of the printout. The default value is 45
        /// </summary>
        [DataMember]
        public int LeftMargin { get; set; }

        /// <summary>
        /// Right margin of the printout. The default value is 5
        /// </summary>
        [DataMember]
        public int RightMargin { get; set; }

        /// <summary>
        /// Top margin of the printout. The default value is 5
        /// </summary>
        [DataMember]
        public int TopMargin { get; set; }

        /// <summary>
        /// Bottom margin of the printout. The default value is 60
        /// </summary>
        [DataMember]
        public int BottomMargin { get; set; }

        /// <summary>
        /// True if the configuration is used by a hardware profile or printing station. If true, the configuration cannot be deleted.
        /// </summary>
        [DataMember]
        public bool ConfigurationUsed { get; set; }
    }
}
