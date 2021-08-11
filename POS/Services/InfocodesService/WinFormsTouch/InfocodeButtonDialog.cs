using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Infocodes;

namespace LSOne.Services.WinFormsTouch
{
    public partial class InfocodeButtonDialog : TouchBaseForm
    {
        private IEnumerable<InfocodeSubcode> subcodes;
        public InfocodeSubcode SelectedInfocode { get; private set; }

        public string InfocodePrompt
        {
            get
            {
                return touchDialogBanner1.BannerText;
            }
            set
            {
                touchDialogBanner1.BannerText = value;
            }
        }

        public bool InputRequired
        {
            get
            {
                return !btnCancel.Visible;
            }
            set
            {
                btnCancel.Enabled = btnCancel.Visible = !value;
            }
        }

        public InfocodeButtonDialog()
        {
            InitializeComponent();
        }

        public InfocodeButtonDialog(IEnumerable<InfocodeSubcode> data)
        {
            InitializeComponent();

            subcodes = data;
            LoadButtons(subcodes);
        }

        private void LoadButtons(IEnumerable<InfocodeSubcode> data)
        {
            tspInfocodes.Clear();
            foreach (InfocodeSubcode infocodeSubcode in data)
            {
                tspInfocodes.AddButton(infocodeSubcode.Text, infocodeSubcode, (string)infocodeSubcode.ID);
            }

            this.Height = touchDialogBanner1.Height + tspInfocodes.TotalHeightInUse + btnCancel.Height + 27;
        }

        private void tspInfocodes_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is InfocodeSubcode)
            {
                SelectedInfocode = (InfocodeSubcode) args.Tag;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
