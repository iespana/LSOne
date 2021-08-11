using System;
using System.Windows.Forms;
using LSOne.Controls.OperationButtons;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class PosMenuPreviewDialog : DialogBase
    {        
        RecordIdentifier posMenuHeaderID;
        OperationButtons opGrid;
        //bool initialFocus;

        public PosMenuPreviewDialog(RecordIdentifier posMenuHeaderID)
        {
            InitializeComponent();

            this.posMenuHeaderID = posMenuHeaderID;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            opGrid = new OperationButtons(PluginEntry.DataModel, pnlPreview, OperationButtonClick);
            opGrid.SetOperationButtons((string)posMenuHeaderID);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OperationButtonClick(object sender, IConnectionManager entry, EventArgs args)
        {

        }

        private void pnlPreview_Resize(object sender, EventArgs e)
        {
            this.Height = this.Height + pnlBottom.Height + 8;
            this.CenterToParent();
        }
    }
}
