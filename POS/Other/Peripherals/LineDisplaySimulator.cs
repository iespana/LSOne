using System;
using System.Reflection;
using System.Windows.Forms;

namespace LSOne.Peripherals
{
    public partial class LineDisplaySimulator : Form
    {
        public LineDisplaySimulator()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            if (text.Length >= 20)
            {
                SetControlPropertyThreadSafe(label1, "Text", text.Substring(0, 20));
                if (text.Length >= 40)
                    SetControlPropertyThreadSafe(label2, "Text", text.Substring(20, 20));
                else
                    SetControlPropertyThreadSafe(label2, "Text", text.Substring(20));
            }
            else
                SetControlPropertyThreadSafe(label1, "Text", text.Substring(0));
        }

        
        public void SetText(int line, string text)
        {
            Control c = (line == 1) ? label1 : label2;
            if (text.Length > 20)
                text = text.Substring(0, 20);
            SetControlPropertyThreadSafe(c, "Text", text);
        }

        private void LineDisplaySimulator_Load(object sender, EventArgs e)
        {
            SetControlPropertyThreadSafe(label1, "Text", "");
            SetControlPropertyThreadSafe(label2, "Text", "");
        }

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
    }
}
