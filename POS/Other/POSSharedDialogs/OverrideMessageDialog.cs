using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class OverrideMessageDialog : TouchBaseForm
    {

        public OverrideMessageDialog()
        {
            InitializeComponent();
        }


        public OverrideMessageDialog(string text, string caption)
            : this()
        {
            tdbBanner.BannerText = caption;
            lblMessage.Text = text;
            CenterToPOSScreen();
            RecalculateSize();
        }


        private void CenterToPOSScreen()
        {
            if (DLLEntry.Settings != null &&
                DLLEntry.Settings.MainFormInfo.MainWindowHCenter > 0 &&
                DLLEntry.Settings.MainFormInfo.MainWindowVCenter > 0)
            {
                StartPosition = FormStartPosition.Manual;

                Point center = new Point(DLLEntry.Settings.MainFormInfo.MainWindowHCenter,
                                            DLLEntry.Settings.MainFormInfo.MainWindowVCenter);

                Location = new Point(center.X - (Width / 2), center.Y - Height / 2);
            }
            else
            {
                StartPosition = Owner != null ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            }
        }
        

        private void RecalculateSize()
        {
            Size = new Size(Width, lblMessage.Location.Y + lblMessage.Height + btnKeepCurrent.Height + 24);
        }
    }
}
