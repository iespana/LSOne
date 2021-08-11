using LSOne.Controls;
using LSOne.Peripherals;
using System.Drawing;

namespace LSOne.Services.WinFormsTouch
{
    public partial class CloseDrawerDialog : TouchBaseForm
    {
        public CloseDrawerDialog(string message)
            : this()
        {
            lblMessage.Text = message;
        }

        public CloseDrawerDialog()
        {
            InitializeComponent();

            CashDrawer.CashDrawerMessageEventX += CashDrawer_CashDrawerMessageEventX;            
        }

        void CashDrawer_CashDrawerMessageEventX(string message)
        {
            // 0 == drawer closed
            // 1 == drawer opened
            if (message == "0")
            {                
                CashDrawer.CashDrawerMessageEventX -= CashDrawer_CashDrawerMessageEventX;
                Close();
            }
        }
    }
}
