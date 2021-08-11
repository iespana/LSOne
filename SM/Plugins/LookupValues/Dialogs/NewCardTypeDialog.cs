using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs 
{
    public partial class NewCardTypeDialog : DialogBase
    {
        RecordIdentifier cardTypeID;

        public NewCardTypeDialog()
        {
            cardTypeID = RecordIdentifier.Empty;

            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier CardTypeID
        {
            get { return cardTypeID; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Providers.CardTypeData.CardNameExists(PluginEntry.DataModel, tbDescription.Text))
            {
                errorProvider1.SetError(tbDescription, Properties.Resources.CardTypeDescriptionExists);
                return;
            }

            CardType cardInfo = new CardType();
            cardInfo.Text = tbDescription.Text;
            switch (cmbType.SelectedIndex)
            {
                case 0: cardInfo.CardTypes = CardTypesEnum.InternationalCreditcard;
                    break;
                case 1: cardInfo.CardTypes = CardTypesEnum.InternationalDebitcard;
                    break;
                case 2: cardInfo.CardTypes = CardTypesEnum.LoyaltyCard;
                    break;
                case 3: cardInfo.CardTypes = CardTypesEnum.CorporateCard;
                    break;
                case 4: cardInfo.CardTypes = CardTypesEnum.CustomerCard;
                    break;
                case 5: cardInfo.CardTypes = CardTypesEnum.EmployeeCard;
                    break;
                case 6: cardInfo.CardTypes = CardTypesEnum.SalespersonCard;
                    break;
                case 7: cardInfo.CardTypes = CardTypesEnum.Unknown;
                    break;
            }
            Providers.CardTypeData.Save(PluginEntry.DataModel, cardInfo);

            cardTypeID = cardInfo.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckEnabled(object sender,EventArgs args)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0 && cmbType.SelectedIndex >= 0);
        }
    }
}
