using System.Windows.Forms;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public class MainFormInfo
    {
        public int MainWindowWidth { get; set;}
        public int MainWindowHeight { get; set; }
        public int OriginalMainWindowHeight { get; set; }
        public int MainWindowLeft { get; set; }
        public int MainWindowTop { get; set; }
        public int OriginalMainWindowTop { get; set; }
        public int MainWindowHCenter { get; set; }
        public int MainWindowVCenter { get; set; }
        public bool AllowUserResize { get; set; }
        public Screen Screen { get; set; }

        public MainFormInfo()
        {
        }
    }
}
