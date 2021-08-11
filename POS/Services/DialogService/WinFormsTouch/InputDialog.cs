using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Peripherals;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services.WinFormsTouch
{
    public partial class InputDialog : TouchBaseForm
    {
        InputTypeEnum inputType;
        /// <summary>
        /// Flag for activating the scanner. Default value is true.
        /// </summary>
        private bool useScanner = true;

        /// <summary>
        /// Parameterless constructor. Scanner is by default enabled.
        /// </summary>
        public InputDialog()
        {
            InitializeComponent();

            touchKeyboard.BuddyControl = tbInput;

            inputType = InputTypeEnum.Normal;
        }

        /// <summary>
        /// Constructor that allows turning the scanner on/off.
        /// </summary>
        /// <param name="useScanner"></param>
        public InputDialog(bool useScanner)
            : this()
        {
            this.useScanner = useScanner;
        }

        public string InputText
        {
            get { return tbInput.Text; }
            set { tbInput.Text = value; }
        }

        public string PromptText
        {
            get { return touchDialogBanner1.BannerText; }
            set { touchDialogBanner1.BannerText = value; }
        }

        public string GhostText
        {
            get { return tbInput.GhostText; }
            set { tbInput.GhostText = value; }
        }

        public int MaxLength
        {
            get { return tbInput.MaxLength; }
            set { tbInput.MaxLength = Math.Max(0, value);}
        }

        public bool InputRequired
        {
            get
            {
                return !btnCancel.Visible;
            }
            set
            {
                btnOK.Location = value ? new Point(btnCancel.Location.X, btnCancel.Location.Y) : new Point(btnCancel.Location.X - btnOK.Size.Width + 1, btnCancel.Location.Y);
                btnCancel.Visible = !value;
            }
        }

        public InputTypeEnum InputType
        {
            get { return inputType; }
            set { inputType = value; }
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                tbInput.Text = scanInfo.ScanDataLabel;
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbInput.Text == "" && InputRequired)
            {
                return;
            }

            if (inputType == InputTypeEnum.Email)
            {
                if (!tbInput.Text.IsEmail())
                {
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.FieldDoesNotContainValidEmail, 
                        MessageBoxButtons.OK, MessageDialogType.Attention);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode != "")
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardLayoutName;
            } 
        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbInput.Text.Length > 0;
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            if (useScanner)
            {
                Scanner.ScannerMessageEvent += ProcessScannedItem;
                Scanner.ReEnableForScan();
            }
        }

        private void InputDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Scanner.ScannerMessageEvent -= ProcessScannedItem;
            Scanner.DisableForScan();
        }
    }
}
