using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls.OperationButtons;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    public partial class PosButtonGridMenuPreviewDialog : DialogBase
    {        
        RecordIdentifier posMenuHeaderID;
        OperationButtons opGrid;

        int heightFactor = 160;
        int widthFactor = 260;

        public PosButtonGridMenuPreviewDialog(RecordIdentifier posMenuHeaderID)
        {
            InitializeComponent();

            this.posMenuHeaderID = posMenuHeaderID;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            

            opGrid = new OperationButtons(PluginEntry.DataModel, pnlPreview, OperationButtonClick);            
            opGrid.SetOperationButtons((string)posMenuHeaderID);
            
            PosMenuHeader header = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderID);

            pnlPreview.RowCount = header.Rows;
            pnlPreview.ColumnCount = header.Columns;
            int maxControlCount = header.Rows * header.Columns;

            while (pnlPreview.Controls.Count > maxControlCount)
            {
                pnlPreview.Controls.RemoveAt(pnlPreview.Controls.Count - 1);
            }

            System.Windows.Forms.Form mainForm = (System.Windows.Forms.Form)PluginEntry.Framework.MainWindow;

            int newHeight = header.Rows * heightFactor;
            int newWidth = header.Columns * widthFactor;

            if (header.Rows * heightFactor >= mainForm.Height)
            {
                newHeight = mainForm.Height - 30;
            }

            if (header.Columns * widthFactor >= mainForm.Width)
            {
                newWidth = mainForm.Width - 30;
            }

            Size = new Size(newWidth, header.Rows > 1 ? newHeight : Size.Height);

            this.CenterToParent();
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
            //this.Height = this.Height + pnlBottom.Height + 8;
            this.CenterToParent();
        }
    }
}
