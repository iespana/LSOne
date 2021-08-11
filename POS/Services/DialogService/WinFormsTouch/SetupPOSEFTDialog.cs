using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// Generic Window to use to set up EFT for a terminal
    /// </summary>
    public partial class SetupPOSEFTDialog : TouchBaseForm, IEFTSetupForm
    {
        /// <summary>
        /// Connection used to define the terminal used
        /// </summary>
        public SQLServerLoginEntry SqlServerLoginEntry { get; set; }
        
		/// <summary>
        /// Initializes a new instance of SetupPOSEFT
        /// </summary>        
        public SetupPOSEFTDialog(Terminal terminal)        
        {
            InitializeComponent();

            if (terminal != null)
            {
                tbCustomField1.Text = terminal.EftCustomField1;
                tbCustomField2.Text = terminal.EftCustomField2;
                tbStoreID.Text = terminal.EftStoreID;
                tbTerminalID.Text = terminal.EftTerminalID;
                tbIPAddress.Text = terminal.IPAddress;
            }
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            if (ActiveControl is Button)
            {
                ((Button) ActiveControl).PerformClick();
            }
            else if (ActiveControl is TextBox)
            {
                SelectNextControl(ActiveControl, true, true, false, true);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CustomField1 = tbCustomField1.Text;
            CustomField2 = tbCustomField2.Text;
            EFTStoreId = tbStoreID.Text;
            EFTTerminalID = tbTerminalID.Text;
            IPAddress = tbIPAddress.Text;
     
            DialogResult = DialogResult.OK;
            Close();
        }
        
        /// <summary>
        /// Custom data, implementation specific
        /// </summary>
        public string CustomField1
        {
            get; set;
        }
        
        /// <summary>
        /// Custom data, implementation specific
        /// </summary>
        public string CustomField2
        {
            get;
            set;
        }
        
        /// <summary>
        /// Store ID for EFT
        /// </summary>
        public string EFTStoreId
        {
            get;
            set;
        }
        
        /// <summary>
        /// Terminal ID for EFT
        /// </summary>
        public string EFTTerminalID
        {
            get;
            set;
        }

        /// <summary>
        /// IP Address for EFT
        /// </summary>
        public string IPAddress
        {
            get;
            set;
        }

        public bool SaveOverride(IConnectionManager connectionManager, ISettings settings)
        {
            return false;
        }

        public void LoadData(IConnectionManager connectionManager, ISettings settings)
        {
        }
    }
}
