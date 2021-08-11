using LSOne.Controls;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace LSOne.Services.WinFormsTouch
{
    public partial class SpinnerDialog : TouchBaseForm
    {
        private Task runningTask;
        public Exception TaskEvents { get; set; }
        private SpinnerDialog(string caption, string message)
        {
            InitializeComponent();

            this.SuspendLayout();
            tdbBanner.Location = new Point(1, 1);
            tdbBanner.Width = Width - 2;
            pictureBox.Left = (Width - pictureBox.Width) / 2;
            this.ResumeLayout();

            tdbBanner.BannerText = caption;
            lblMessage.Text = message;
            Visible = false;
        }

        public SpinnerDialog(Task runningTask, string caption, string message)
            : this(caption, message)
        {
            this.runningTask = runningTask;
        }

        private async void SpinnerDialog_Shown(object sender, EventArgs e)
        {
            TaskEvents = null;
            try
            {
                await runningTask;
                Close();
            }
            catch (Exception ex)
            {
                TaskEvents = ex;
                Close();
            }
        }

    }
}
