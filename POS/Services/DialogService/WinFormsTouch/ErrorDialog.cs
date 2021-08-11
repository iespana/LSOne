using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    public partial class ErrorDialog : TouchBaseForm
    {
        private bool detailsExpanded;
        private readonly Size normalSize;
        private readonly Size expandedSize;

        public ErrorDialog(string message, string details)
            : this()
        {
            txtMessage.Text = message;
            txtDetails.Text = details;
            detailsExpanded = false;

            if (string.IsNullOrEmpty(details))
            {
                btnViewDetails.Visible = false;
            }

            expandedSize = new Size(Width, btnCopyToClipboard.Location.Y + btnCopyToClipboard.Height + 11);
            normalSize = new Size(Width, btnClose.Location.Y + btnClose.Height + 11);

            SetSizeAndLocation();
        }

        public ErrorDialog()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Don't leave focus in the text box by default
            if (btnViewDetails.Visible)
            {
                btnViewDetails.Focus();
            }
            else
            {
                btnClose.Focus();
            }
        }

        private void SetSizeAndLocation()
        {
            Size = detailsExpanded
                       ? expandedSize
                       : normalSize;

            StartPosition = Owner != null ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            detailsExpanded = !detailsExpanded;

            btnViewDetails.Text = detailsExpanded ? Properties.Resources.HideDetails : Properties.Resources.ViewDetails;
            btnCopyToClipboard.Visible = btnCopyToClipboard.Enabled = detailsExpanded;
            
            SetSizeAndLocation();

            // Invalidate is called since the border around this form is not redrawn unless we invalidate the entire client area
            Invalidate();
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMessage.Text + "\n\n" +  txtDetails.Text);
        }
    }
}
