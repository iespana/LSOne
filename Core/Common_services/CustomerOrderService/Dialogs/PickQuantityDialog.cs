using LSOne.Controls;
using System;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class PickQuantityDialog : TouchBaseForm
    {
        private enum Buttons
        {
            OK,
            Cancel
        }

        public PickQuantityDialog(decimal currentValue, decimal maxValue, int numberOfDecimals)
        {
            InitializeComponent();

            ntbAmount.AllowDecimal = numberOfDecimals > 0;
            ntbAmount.DecimalLetters = numberOfDecimals;
            ntbAmount.Value = (double)currentValue;

            touchScrollButtonPanel1.AddButton(Properties.Resources.All + " (" + maxValue.ToString() + ")", maxValue, "");
            touchScrollButtonPanel1.AddButton(Properties.Resources.None, 0m, "");
            touchScrollButtonPanel1.AddButton(Properties.Resources.OK, Buttons.OK, "", LSOne.Controls.SupportClasses.TouchButtonType.OK, LSOne.Controls.SupportClasses.DockEnum.DockEnd);
            touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, Buttons.Cancel, "", LSOne.Controls.SupportClasses.TouchButtonType.Cancel, LSOne.Controls.SupportClasses.DockEnum.DockEnd);
        }

        public decimal Quantity => (decimal)ntbAmount.Value;

        private void touchScrollButtonPanel1_Click(object sender, Controls.SupportClasses.ScrollButtonEventArguments args)
        {
            if(args.Tag is decimal)
            {
                ntbAmount.Value = (double)(decimal)args.Tag;
            }
            else
            {
                if((Buttons)args.Tag == Buttons.OK)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
        }

        private void touchNumPad1_EnterPressed(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
