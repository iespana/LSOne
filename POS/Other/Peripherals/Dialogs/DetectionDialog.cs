using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Peripherals.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace LSOne.Peripherals.Dialogs
{
    public partial class DetectionDialog : TouchBaseForm
    {
        public Queue<string> Messages { get; }

        private HardwareConfigurationDialog parentDialog;
        Timer timer;
        Thread thread;

        private delegate void DetectionStatus(string detectionStatus);
        public bool Cancel { get; set; }
        private bool done;

        public DetectionDialog(HardwareConfigurationDialog parentDlg)
        {
            InitializeComponent();

            parentDialog = parentDlg;
            Screen sc = Screen.PrimaryScreen;
            Location = new Point(sc.Bounds.Left + (sc.Bounds.Width / 2) - (Width / 2), sc.Bounds.Top + (sc.Bounds.Height / 2) - (Height / 2));
            Messages = new Queue<string>();
            Cancel = false;
        }

        public override DialogResult ShowDialog()
        {
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            thread = new Thread(AutoDetectHardware);

            thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;

            lvSelection.Visible = false;
            thread.Start();
            return base.ShowDialog();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (done)
            {
                timer.Stop();
                timer = null;

                lvSelection.Visible = true;
                pictureBox1.Visible = false;
                DisplayMessages();
            }
        }

        private void AutoDetectHardware()
        {
            foreach (var hardwareConfigurationStep in parentDialog.HardwareConfigurationSteps)
            {
                if (hardwareConfigurationStep.Step is IHardwareValidator)
                {
                    IHardwareValidator step = hardwareConfigurationStep.Step as IHardwareValidator;

                    try
                    {
                        step.AutoDetectOPOS(this);
                    }
                    catch (Exception)
                    {
                        Messages.Enqueue(string.Format("Error detecting {0}", step.OPOSType));
                    }

                    Thread.Sleep(1000);
                }
                if (Cancel)
                {
                    break;
                }
            }

            done = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Cancel = true;
            Close();
        }

        private void DisplayMessages()
        {
            Row row = null;

            while(Messages.Count > 0)
            {
                row = new Row();
                row.AddText(Messages.Dequeue());
                lvSelection.AddRow(row);
            }
        }
    }
}
