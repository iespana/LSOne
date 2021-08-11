using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    internal partial class CardSerieDialog : DialogBase
    {
        CardNumberSerie serie;
        RecordIdentifier oldRecord;

        public CardSerieDialog(RecordIdentifier serieFullID)
            : this()
        {
            serie.CardTypeID = (RecordIdentifier)serieFullID.PrimaryID.Clone();

            if (serieFullID.HasSecondaryID)
            {
                serie.CardNumberFrom = (string)serieFullID.SecondaryID;
                serie.CardNumberTo = (string)serieFullID.SecondaryID.SecondaryID;  
            }

            oldRecord = serieFullID;

            ntbFrom.Text = serie.CardNumberFrom;
            ntbTo.Text = serie.CardNumberTo;

            
        }

        public CardSerieDialog()
        {
            this.serie = new CardNumberSerie();

            InitializeComponent();

            
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public CardNumberSerie Serie
        {
            get { return serie; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ntbTo.Text.Length != ntbFrom.Text.Length)
            {
                errorProvider1.SetError(ntbTo, Properties.Resources.SeriesNumberEquallyLong);
                return;
            }

            if (ntbTo.Value < ntbFrom.Value)
            {
                errorProvider1.SetError(ntbTo, Properties.Resources.SeriesNumberToNotSmallerThanFrom);
                return;
            }

         
            serie.CardNumberFrom = ntbFrom.Text;
            serie.CardNumberTo = ntbTo.Text;

            var cardSerieProvider = Providers.CardNumberSerieData;
            if (cardSerieProvider.Exists(
                PluginEntry.DataModel,
                new RecordIdentifier(serie.CardTypeID,new RecordIdentifier(serie.CardNumberFrom,serie.CardNumberTo))))
            {
                errorProvider1.SetError(ntbFrom, Properties.Resources.SerieExist);

                return;
            }

            cardSerieProvider.Save(PluginEntry.DataModel, oldRecord, serie);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckValues(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (serie == null)
            {
                btnOK.Enabled = (ntbFrom.Text.Length > 0 && ntbTo.Text.Length > 0) &&
                    !(ntbFrom.Text == "0" && ntbTo.Text ==  "0");
            }
            else
            {
                btnOK.Enabled = ((ntbFrom.Text.Length > 0 && ntbTo.Text.Length > 0) &&
                    (ntbFrom.Value.ToString() != serie.CardNumberFrom || ntbTo.Value.ToString() != serie.CardNumberTo)) &&
                    !(ntbFrom.Text == "0" && ntbTo.Text == "0"); ;
            }
            
        }
    }
}
