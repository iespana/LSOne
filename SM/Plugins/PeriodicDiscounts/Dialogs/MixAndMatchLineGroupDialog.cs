using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MixAndMatchLineGroupDialog : DialogBase
    {
        RecordIdentifier offerID;
        RecordIdentifier lineGroupID;
        MixAndMatchLineGroup group;

        public MixAndMatchLineGroupDialog(RecordIdentifier offerID,RecordIdentifier lineGroupID)
            : this()
        {
            this.offerID = offerID;
            this.lineGroupID = lineGroupID;

            group = Providers.MixAndMatchLineGroupData.Get(PluginEntry.DataModel, new RecordIdentifier(offerID, lineGroupID));

            tbDescription.Text = group.Text;

            ntbNumberOfItemsNeeded.Value = (double)group.NumberOfItemsNeeded;
            cwLineColor.SelectedColor = group.Color;

            CheckEnabled(this, EventArgs.Empty);
        }

        public MixAndMatchLineGroupDialog(RecordIdentifier offerID)
            : this()
        {
            this.offerID = offerID;

            ntbNumberOfItemsNeeded.Value = 1;
        }

        public MixAndMatchLineGroupDialog()
            : base()
        {
            offerID = "";
            lineGroupID = "";
            group = null;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
        }

        

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier OfferID
        {
            get { return offerID; }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (group == null)
            {
                btnOK.Enabled = (ntbNumberOfItemsNeeded.Value > 0) && (tbDescription.Text != "");
            }
            else
            {
                btnOK.Enabled = ntbNumberOfItemsNeeded.Value > 0 && (tbDescription.Text != "") &&
                    (ntbNumberOfItemsNeeded.Value != (double)group.NumberOfItemsNeeded ||
                     cwLineColor.SelectedColor.ToArgb() != group.Color.ToArgb() ||
                     tbDescription.Text != group.Text);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (group == null)
            {
                group = new MixAndMatchLineGroup();
                group.OfferID = offerID;          
                group.LineGroup = RecordIdentifier.Empty;
            }

            group.NumberOfItemsNeeded = (int)ntbNumberOfItemsNeeded.Value;
            group.Color = cwLineColor.SelectedColor;
            group.Text = tbDescription.Text;

            Providers.MixAndMatchLineGroupData.Save(PluginEntry.DataModel, group);

            DialogResult = DialogResult.OK;
            Close();
        }

       

       

       



        
    }
}
