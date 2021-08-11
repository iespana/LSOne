using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.Services
{
    public enum FormPart { Header = 0, Line = 1, Footer = 2 };

    public enum valign { left, center, right };

    public class FormInfo
    {
        #region Member variables

        #endregion

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

        public PrintBehaviors PrintBehavior { get; set; }

        #endregion

        public FormInfo(bool useWindowsPrinter, string windowsPrinter, bool printAsSlip, PrintBehaviors printBehavior)
            : this()
        {
            UseWindowsPrinter = useWindowsPrinter;
            WindowsPrinterName = windowsPrinter;
            PrintAsSlip = printAsSlip;
            PrintBehavior = printBehavior;
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
        }
    }
}
