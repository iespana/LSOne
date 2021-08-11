using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class NumpadAmountQtyDialog : TouchBaseForm
    {
        public NumpadAmountQtyDialog()
        {
            InitializeComponent();

            touchDialogBanner1.Location = new Point(1, 1);
            touchDialogBanner1.Width = Width - 2;
            
            if(ntbValue.Value == 0D)
            {
                ntbValue.Text = "";
        }
        }

        /// <summary>
        /// Gets or sets whether a negative value can be entered
        /// </summary>
        public bool AllowNegative
        {
            get { return ntbValue.AllowNegative; }
            set { ntbValue.AllowNegative = value; }
        }

        public double Value
        {
            get
            {
                return ntbValue.Value;
            }
        }

        public void SetMaxInputValue(double max)
        {
            ntbValue.MaxValue = max;
        }

        public string PromptText
        {
            set
            {
                touchDialogBanner1.BannerText = value;
            }
        }

        public string GhostText
        {
            set
            {
                ntbValue.GhostText = value;
            }
        }

        public bool HasDecimals
        {
            get
            {
                return ntbValue.AllowDecimal;
            }
            set
            {
                ntbValue.AllowDecimal = value;
                touchNumPad.DotKeyEnabled = value;
            }
        }

 
        public bool InputRequried
        {
            get
            {
                return !btnCancel.Visible;
            }
            set
            {
                btnCancel.Visible = !value;
            }
        }


        public void ClearValue()
        {
            ntbValue.Text = "";
        }

        /// <summary>
        /// The number of decimals that can be entered into the numpad
        /// </summary>
        public int NumberOfDecimals
        {
            get { return ntbValue.DecimalLetters; }
            set { ntbValue.DecimalLetters = value; }
        }

        public bool HasInput
        {
            get
            {
                return (ntbValue.Text.Length > 0);
            }
        }

        private void touchNumPad_EnterPressed(object sender, EventArgs e)
        {
            if (InputRequried && (ntbValue.Text.Length == 0))
            {
                Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PleaseEnterAValue);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void touchNumPad_ClearPressed(object sender, EventArgs e)
        {
            ntbValue.Text = "";
            ntbValue.Focus();
        }

        private void ntbValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                touchNumPad_EnterPressed(sender, e);
            }
        }
    }
}
