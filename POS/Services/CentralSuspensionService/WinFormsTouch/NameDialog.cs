using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using System;
using System.Collections.Generic;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// Dialog for adding text answer to a suspended transaction
    /// </summary>
    public partial class NameDialog : TouchBaseForm
    {
        private SuspensionTypeAdditionalInfo suspensionTypeInfo;
        private List<DataEntity> prefixes;

        /// <summary>
        /// Default constructor
        /// </summary>
        public NameDialog(IConnectionManager entry, SuspensionTypeAdditionalInfo suspensionTypeInfo)
        {
            InitializeComponent();
            this.suspensionTypeInfo = suspensionTypeInfo;
            touchDialogBanner1.BannerText = suspensionTypeInfo.Text;

            if(suspensionTypeInfo.Required)
            {
                btnCancel.Visible = false;
                btnClear.Left = btnOK.Left;
                btnOK.Left = btnCancel.Left;
            }

            btnOK.Enabled = false;

            prefixes = new List<DataEntity>();
            foreach (string value in entry.Cache.GetNamePrefixes())
            {
                prefixes.Add(new DataEntity("", value));
            }
        }

        /// <summary>
        /// Text answer
        /// </summary>
        public SuspendedTransactionAnswer GetAnswer()
        {
            return new SuspendedTransactionAnswer
            {
                RecordID = Guid.NewGuid(),
                Prompt = suspensionTypeInfo.Text,
                Name = new Utilities.DataTypes.Name(cmbTitle.Text, tbFirstName.Text, "", tbLastName.Text, "")
            };
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbFirstName.Text != "" || tbLastName.Text != "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbFirstName.Text = "";
            tbLastName.Text = "";
            cmbTitle.Text = "";
        }

        private void cmbTitle_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                prefixes,
                null,
                true,
                cmbTitle.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbTitle_RequestClear(object sender, EventArgs e)
        {
            cmbTitle.SelectedData = new DataEntity("", "");
        }
    }
}
