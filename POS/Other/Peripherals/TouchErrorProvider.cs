using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Utilities.ColorPalette;
using LSOne.Controls.SupportClasses;

namespace LSOne.Peripherals
{
    /// <summary>
    /// THIS IS A DUPLICATE OF THE TOUCHERRORPROVIDER FROM POSSHAREDDIALOGS
    /// 
    /// It is required by the HardwareConfigurationDialog and we cannot add a reference to PosSharedDialogs
    /// </summary>
    internal partial class TouchErrorProvider : UserControl
    {
        private string errorText;
        private List<string> errorMessages;

        public TouchErrorProvider()
        {
            InitializeComponent();
            errorMessages = new List<string>();
        }

        [Localizable(true), Browsable(true)]
        public string ErrorText
        {
            get { return errorText; }
            set
            {
                errorText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Add an error message to the list of errors
        /// </summary>
        /// <param name="errorMessage">Error message to add</param>
        public void AddErrorMessage(string errorMessage)
        {
            errorMessages.Add(errorMessage);

            if(HasMultipleErrorMessages())
            {
                ErrorText = Properties.Resources.MultipleErrorMessages;
            }
            else
            {
                ErrorText = errorMessages[0];
            }
        }

        /// <summary>
        /// Clear all error messages
        /// </summary>
        public void Clear()
        {
            errorMessages.Clear();
            ErrorText = "";
        }

        private bool HasMultipleErrorMessages()
        {
            return errorMessages.Count > 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            Size textSize = TextRenderer.MeasureText(g, ErrorText, Font);
            Rectangle textBounds = new Rectangle(Width / 2 - textSize.Width / 2, 0, textSize.Width, Height);

            g.DrawImage(Properties.Resources.Warning_red_24px, new Rectangle(Math.Max(0, textBounds.X - 24), Height / 2 - 12, 24, 24));
            TextRenderer.DrawText(g, ErrorText, Font, textBounds, ColorPalette.NegativeNumber);
        }

        private void TouchErrorProvider_Click(object sender, EventArgs e)
        {
            if(HasMultipleErrorMessages())
            {
                Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(string.Join("\r\n", errorMessages), MessageDialogType.ErrorWarning);
            }
        }
    }
}
