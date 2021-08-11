using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.DataDirector;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class DataDirectorDialog : DialogBase
    {
        private JscLocation locationItem;
        private RecordIdentifier design;

        private enum Operation
        {
            Test,                   // this.locationItem
            TablesAndFields,        // this.locationItem
            Tables,                 // this.locationItem
            Fields                  // this.locationItem and this.tableId
        }
        private Operation operation;
        private Guid? tableId;
        private bool updateExistingDesign;
        private string newDescription;
        private string currentUICultureName;

        private bool success;

        private bool cancelButtonPressed;

        // DataDirector reference, only used between async functions
        private DataDirector.DataDirector dataDirector;

        public DataDirectorDialog()
        {
            InitializeComponent();
        }
        
        public void TestConnection(IWin32Window owner, JscLocation location)
        {
            this.operation = Operation.Test;
            this.locationItem = location;
            Header = Properties.Resources.DataDirectorDialogTestConnection;
            ShowDialog(owner);
        }


        public bool ReadTablesAndFieldsDesign(IWin32Window owner, JscLocation location, bool updateExistingDesign, string newDescription, RecordIdentifier existingDesign = null)
        {
            return ReadDesign(owner, Operation.TablesAndFields, location, null, updateExistingDesign, newDescription,existingDesign);
        }



        public bool ReadTablesDesign(IWin32Window owner, JscLocation location, bool updateExistingDesign, string newDescription, RecordIdentifier existingDesign = null)
        {
            return ReadDesign(owner, Operation.Tables, location, null, updateExistingDesign, newDescription,existingDesign);
        }

        public bool ReadFieldDesign(IWin32Window owner, JscLocation location, Guid tableId)
        {
            return ReadDesign(owner, Operation.Fields, location, tableId, true, null);
        }


        private bool ReadDesign(IWin32Window owner, Operation operation, JscLocation location, Guid? tableId, bool updateExistingDesign, string newDescription, RecordIdentifier exixtingDesign = null)
        {
            success = false;
            this.operation = operation;
            this.locationItem = location;
            this.tableId = tableId;
            this.updateExistingDesign = updateExistingDesign;
            this.newDescription = newDescription;
            this.design = exixtingDesign;
            Header = Properties.Resources.DataDirectorDialogReadDesign;

            ShowDialog(owner);

            return success;
        }




        private void DataDirectorDialog_Load(object sender, EventArgs e)
        {
           
            currentUICultureName = Thread.CurrentThread.CurrentUICulture.Name;

            picMessage.Visible = false;
            btnClose.Visible = false;
            btnCancel.Visible = true;
            btnCancel.Enabled = false;
            cancelButtonPressed = false;
            if (operation == Operation.Test)
            {
                IdleOneShotProcessing.PostRun(DoTestConnection);
            }
            else
            {
                IdleOneShotProcessing.PostRun(DoReadDesign);
            }
        }


    
        private bool ProgressView
        {
            get { return panelProgress.Visible; }
            set
            {
                pgbProcess.Visible = value;
                panelProgress.Visible = value;
                panelMessage.Visible = !value;
                if (value)
                {
                    tbProgressMessage.Clear();
                }
            }
        }

        private void DoTestConnection(object arg)
        {
            SetMessage(Properties.Resources.DataDirectorInitializingMsg, null);
            ProgressView = true;
            Application.DoEvents();

            try
            {
                int? port = ParsePort(locationItem.DDPort);
                dataDirector = CreateDataDirector(locationItem.DDHost, locationItem.DDNetMode, port, locationItem.DBConnectionString);
                SetMessage(Properties.Resources.DataDirectorTestingMsg, null);
                dataDirector.StartTestConnection();
                btnCancel.Enabled = true;
            }
            catch
            {
                DisposeDataDirector(ref dataDirector);
                throw;
            }
        }

        private int? ParsePort(string port)
        {
            int numPort;
            if (!int.TryParse(port, out numPort))
                return null;
            return numPort;
        }

        private void OperationCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string message;
            Icon icon;

            success = false;

            if (e.Cancelled && e.Error == null)
            {
                icon = SystemIcons.Warning;
                message = Properties.Resources.DataDirectorOperationCancelledMsg;
            }
            else if (e.Error == null)
            {
                if (e.UserState is Guid)
                {
                    locationItem.DatabaseDesign = (Guid)e.UserState;
                }
                icon = SystemIcons.Information;
                message = Properties.Resources.DataDirectorOperationOkMsg;
                success = true;
            }
            else
            {
                icon = SystemIcons.Error;
                if (e.Error is DataDirectorException)
                {
                    message = string.Format(Properties.Resources.DataDirectorErrorMsg, e.Error.Message);
                }
                else
                {
                    message = string.Format(Properties.Resources.DataDirectorExceptionMsg, e.Error.GetType().Name, e.Error.Message);
                }
            }

            SetMessage(message, icon);
            ProgressView = false;
            btnCancel.Visible = false;
            btnClose.Visible = true;
            DisposeDataDirector(ref dataDirector);
        }



        private void dataDirector_ProgressReported(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new ThreadStart(delegate() { ProgressReported(e); }));
            }
            else
            {
                ProgressReported(e);
            }
        }

        private void ProgressReported(ProgressEventArgs e)
        {
            if (e.Message != null)
            {
                tbProgressMessage.Text = e.Message;
            }

            if (e.Percentage != null)
            {
                if (pgbProcess.Style != ProgressBarStyle.Blocks)
                {
                    pgbProcess.Style = ProgressBarStyle.Blocks;
                }
                pgbProcess.Value = e.Percentage.Value;
            }
            else
            {
                if (pgbProcess.Style != ProgressBarStyle.Marquee)
                {
                    pgbProcess.Style = ProgressBarStyle.Marquee;
                }
            }

            if (cancelButtonPressed)
                e.Cancel = true;
        }





        private void SetMessage(string message, Icon icon)
        {
            tbMessage.Text = message;
            if (icon == null)
                picMessage.Visible = false;
            else
            {
                picMessage.Image = icon.ToBitmap();
                picMessage.Visible = true;
            }
        }


        private DataDirector.DataDirector CreateDataDirector(string host, NetMode netMode, int? port, string connectionString)
        {
            DataDirector.DataDirector dataDirector;

            if (port != null)
                dataDirector = new DataDirector.DataDirector(host, netMode, port.Value, connectionString, currentUICultureName);
            else
                dataDirector = new DataDirector.DataDirector(host, netMode, connectionString, currentUICultureName);

            dataDirector.StartTestConnectionCompleted += OperationCompleted;
            dataDirector.ProgressReported += dataDirector_ProgressReported;
            dataDirector.StartReadDesignCompleted += OperationCompleted;
            dataDirector.StartReadFieldDesignCompleted += OperationCompleted;

            return dataDirector;
        }

        private void DisposeDataDirector(ref DataDirector.DataDirector dataDirector)
        {
            if (dataDirector != null)
            {
                dataDirector.StartTestConnectionCompleted -= OperationCompleted;
                dataDirector.ProgressReported -= dataDirector_ProgressReported;
                dataDirector.StartReadDesignCompleted -= OperationCompleted;
                dataDirector.Dispose();
                dataDirector = null;
            }
        }




        private void DoReadDesign(object arg)
        {
            SetMessage(Properties.Resources.DataDirectorInitializingMsg, null);
            ProgressView = true;
            Application.DoEvents();

            try
            {
                int? port = ParsePort(locationItem.DDPort);
                dataDirector = CreateDataDirector(locationItem.DDHost, locationItem.DDNetMode, port, locationItem.DBConnectionString);
                SetMessage(Properties.Resources.DataDirectorConnectingMsg, null);
                switch (operation)
                {
                    case Operation.TablesAndFields:
                    case Operation.Tables:
                       
                            dataDirector.StartReadDesign((Guid) locationItem.ID, operation == Operation.TablesAndFields,
                                updateExistingDesign, newDescription, design);
                       
                        break;
                    case Operation.Fields:
                        dataDirector.StartReadFieldDesign((Guid)locationItem.ID, tableId.Value);
                        break;
                }
                btnCancel.Enabled = true;
            }
            catch
            {
                DisposeDataDirector(ref dataDirector);
                throw;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelButtonPressed = true;
        }



    }
}
