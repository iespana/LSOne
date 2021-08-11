using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.WinFormsTouch
{
    public partial class StatusDialog : TouchBaseForm
    {
        internal bool IsModal { get; set; }

        public string Message
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        public StatusDialog()
        {
            InitializeComponent();
        }

        public StatusDialog(string message, string caption)
            : this()
        {
            lblMessage.Text = message;
            tdbBanner.BannerText = String.IsNullOrEmpty(caption) ? "" : caption;
        }

        public StatusDialog(string message, string caption, Control[] buttons)
            : this(message, caption)
        {
            if (buttons != null)
            {
                buttonLayout.SuspendLayout();
                buttonLayout.AutoSize = true;

                foreach (Button button in buttons)
                {
                    button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    buttonLayout.Controls.Add(button);
                }

                buttonLayout.ResumeLayout();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (IsModal)
                // We can't do modal here, since processing will not continue if that is done, so we disable the main window instead
                ((Control)DLLEntry.Settings.POSApp.POSMainWindow).Enabled = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (IsModal)
                ((Control)DLLEntry.Settings.POSApp.POSMainWindow).Enabled = true;
        }
    }
}
