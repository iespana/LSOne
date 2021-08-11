using System;
using System.Windows.Forms;
using LSOne.Utilities.IO;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.Dialogs
{
    public delegate void ExportDelegate(ref bool canceled,ref bool done, ref ExportErrorCodes errorCode,FolderItem destination, object context, object hint,bool simple );

    public partial class ExportExcelProgressDialog : DialogBase
    {
        private Timer timer;
        private System.Threading.Thread thread;
        private bool done;
        private bool canceled;
        private ExportDelegate exportProc;
        private object context;
        private FolderItem destination;
        private ExportErrorCodes errorCode;
        private object hint;
        private bool simple;

        private ExportExcelProgressDialog()
        {
            done = false;
            canceled = false;
            errorCode = ExportErrorCodes.NoError;

            InitializeComponent();
        }

        public ExportExcelProgressDialog(ExportDelegate exportProc, FolderItem destination,object context, object hint, bool simple)
            : this()
        {
            this.exportProc = exportProc;
            this.context = context;
            this.destination = destination;
            this.hint = hint;
            this.simple = simple;
        }

        public ExportErrorCodes ErrorCode
        {
            get
            {
                return errorCode;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            progressBar1.Value = 50;

            timer = new Timer {Interval = 10};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            timer = null;

            thread = new System.Threading.Thread(Export);
            thread.Start();

            timer = new Timer {Interval = 200};
            timer.Tick += doneTimer_Tick;
            timer.Start();
        }

        private void doneTimer_Tick(object sender, EventArgs e)
        {
            if (done)
            {
                timer.Stop();
                timer = null;

                DialogResult = canceled ? DialogResult.Cancel : DialogResult.OK;
                Close();
            }
        }

        void Export()
        {
            if (exportProc != null)
            {
                exportProc(ref canceled, ref done, ref errorCode, destination, context, hint,simple);
            }

            done = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            canceled = true;
        }
    }
}
