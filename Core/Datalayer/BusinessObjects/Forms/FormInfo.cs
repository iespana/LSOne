using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.Localization;

namespace LSOne.DataLayer.BusinessObjects.Forms
{
    public class FormInfo
    {

        #region Properties
        public string Header { get; set; }
        public string Details { get; set; }
        public string Footer { get; set; }
        public int HeaderLines { get; set; }
        public bool Reprint { get; set; }
        public int DetailLines { get; set; }
        public int FooterLines { get; set; }
        public bool UseWindowsPrinter { get; set; }
        public string WindowsPrinterName { get; set; }
        public bool PrintAsSlip { get; set; }
        public int FormWidth { get; set; }
        public PrintBehaviors PrintBehavior { get; set; }
        public string FormDescription { get; set; }
        public int NumberOfCopiesToPrint { get; set; }
        public WindowsPrinterConfiguration WindowsPrinterConfiguration { get; set; }

        #endregion

        public FormInfo(bool useWindowsPrinter, string windowsPrinter, bool printAsSlip, PrintBehaviors printBehavior, int formWidth)
            : this()
        {
            UseWindowsPrinter = useWindowsPrinter;
            WindowsPrinterName = windowsPrinter;
            PrintAsSlip = printAsSlip;
            PrintBehavior = printBehavior;
            FormWidth = formWidth;
            FormDescription = "";
        }

        public FormInfo()
        {
            PrintBehavior = PrintBehaviors.AlwaysPrint;
            PrintAsSlip = false;
            WindowsPrinterName = "";
            UseWindowsPrinter = false;
            FooterLines = 0;
            DetailLines = 0;
            Reprint = false;
            HeaderLines = 0;
            Footer = "";
            Details = "";
            Header = "";
            FormWidth = 56;
            FormDescription = "";
            NumberOfCopiesToPrint = 1;
            WindowsPrinterConfiguration = null;
        }
    }
}
