using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using LSOne.KitchenDisplaySystem.KdsCommon;
using LSOne.Utilities.Network;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class KitchenServiceDebugDialog : Form
    {
        private ComDebugCallback callback;
        private ComDebugClient debugconfig;
        private bool debugon;
        private int maxlines = 5000;
        private int maxDebugLevel = 7;

        public string Host = string.Empty;
        public int TCPSPort = 0;

        public KitchenServiceDebugDialog()
        {
            InitializeComponent();
        }

        private void CloseConnection()
        {
            debugon = false;
            btnConnect.Text = Properties.Resources.Connecting;
            Text = string.Format("Ready to Debug on Host {0}", Host);

            if (debugconfig == null)
                return;

            try
            {
                if (debugconfig.State == CommunicationState.Opened)
                    debugconfig.SetDebugMode(0);

                if (callback != null)
                    callback.OnDebugData -= callback_OnDebugData;

                debugconfig.Close();
            }
            catch
            {
            }
            debugconfig = null;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (debugon)
            {
                CloseConnection();
                return;
            }

            callback = new ComDebugCallback();
            callback.OnDebugData += callback_OnDebugData;
            UriBuilder ub = new UriBuilder(Uri.UriSchemeNetTcp, Host, TCPSPort, "Dbg");
            EndpointAddress add = new EndpointAddress(ub.Uri.OriginalString);            
            debugconfig = new ComDebugClient(new InstanceContext(callback), NetworkHelper.GetBinding(), add);

            try
            {
                debugconfig.Open();
                debugconfig.SetDebugMode(maxDebugLevel);
                debugon = true;
                btnConnect.Text = Properties.Resources.Disconnect;
                Text = string.Format("Debugging Connected on Host {0}:{1}", Host, TCPSPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                CloseConnection();
            }
        }

        void callback_OnDebugData(string debugdata)
        {
            if (maxlines <= 0)
                return;

            ListViewItem i = new ListViewItem();
            i.Text = debugdata;
            if (debugdata.Contains("ERROR:"))
                i.ForeColor = Color.Red;

            lvDebug.Items.Add(i);

            if (lvDebug.Items.Count > maxlines)
                lvDebug.Items.RemoveAt(0);
        }

        private void Debug_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Debug.txt";
            saveFileDialog1.Title = "Save a text file";
            saveFileDialog1.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvDebug.Items.Clear();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = saveFileDialog1.FileName;
            File.WriteAllText(fileName, lvDebug.Text);
        }
    }
}
