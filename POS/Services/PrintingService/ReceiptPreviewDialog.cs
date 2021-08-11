using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services
{
    public partial class ReceiptPreviewDialog : TouchBaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public ReceiptPreviewDialog(IConnectionManager entry, string reportLayout)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            textViewer.Text = reportLayout;
            btnPrint.Enabled = entry.HasPermission(Permission.ReprintReceipt);
        }
    }
}