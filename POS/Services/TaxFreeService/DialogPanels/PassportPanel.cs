using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Dialogs.SelectionDialog;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;

namespace LSOne.Services.DialogPanels
{
    public partial class PassportPanel : DialogPageBase
    {
        private RefundDialog parent;
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public override bool BackEnabled
        {
            get { return false; }
        }

        public PassportPanel(IConnectionManager entry)
            : base()
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
        }

        public PassportPanel(IConnectionManager entry, RefundDialog parent)
            : this(entry)
        {
            this.parent = parent;
            dtIssueDate.Value = DateTime.Now;
            dtIssueDate.Text = "";
        }

        public void SetData(Tourist tourist)
        {
            tbPassportNumber.Text = tourist.PassportNumber;
            tbPassportIssuedBy.Text = tourist.PassportIssuedBy;
            dtIssueDate.Value = tourist.PassportIssuedOn;
        }

        public override void GetData(Tourist tourist)
        {
            tourist.PassportNumber = tbPassportNumber.Text;
            tourist.PassportIssuedBy = tbPassportIssuedBy.Text;
            tourist.PassportIssuedOn = dtIssueDate.Value;
        }

        public override bool ValidateData()
        {
            errorProvider.Clear();
            errorProvider.Visible = false;

            if (IsTextFieldEmpty(tbPassportNumber))
            {
                errorProvider.AddErrorMessage(Resources.PassportRequired);
            }

            if (IsTextFieldEmpty(tbPassportIssuedBy))
            {
                errorProvider.AddErrorMessage(Resources.PassportIssuedByRequired);
            }

            if (dtIssueDate.Value == DateTime.MinValue)
            {
                errorProvider.AddErrorMessage(Resources.PassportIssueDateRequired);
            }

            if (errorProvider.ErrorText == "")
            {
                return true;
            }

            errorProvider.Visible = true;
            return false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (parent == null)
                return;
            List<Tourist> current = DataProviderFactory.Instance.Get<ITouristData, Tourist>().GetByPassportID(dlgEntry, tbPassportNumber.Text);
            if (current != null)
            {
                if (current.Count == 1)
                    parent.UpdateTourist(current[0]);
                else if (current.Count > 1)
                {
                    using (SelectionDialog dlg = new SelectionDialog(new DataEntitySelectionList(current.Cast<DataEntity>().ToList()), Resources.SelectPerson, false, true))
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataEntity tourist = (DataEntity)dlg.SelectedItem;
                            parent.UpdateTourist(current.Find(t => t.ID == tourist.ID));
                        }
                    }
                }
            }
        }
    }
}
