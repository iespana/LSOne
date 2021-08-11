using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace LSOne.Controls.Dialogs
{
    public partial class ScaleDialog : TouchBaseForm
    {
        private ISaleLineItem saleLineItem;
        private Timer readFromScaleTimer;

        private bool shouldNotRetry;
        private bool scaleConnected;

        private IScaleService service;
        
        public decimal Weight { get; private set; }

        private ScaleDialog()
        {
            InitializeComponent();
        }
        
        public ScaleDialog(ISaleLineItem saleLineItem)
            : this()
        {
            this.saleLineItem = saleLineItem;
            touchDialogBanner1.BannerText = saleLineItem.Description;
            
            if (!DesignMode)
            {
                service = (IScaleService)DLLEntry.DataModel.Service(ServiceType.ScaleService);

                service.DataMessage += ScaleOnScaleDataMessageEventX;
                service.ErrorMessage += ScaleOnScaleErrorMessageEventX;      
                
                readFromScaleTimer = new Timer();
                readFromScaleTimer.Interval = 1;
                readFromScaleTimer.Tick += ReadFromScaleTimer_Tick;

                scaleConnected = true;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            service.EnableScale(true);
        }
        
        private void ScaleDialog_Shown(object sender, EventArgs e)
        {
            readFromScaleTimer.Start();
        }
        
        private void ReadFromScaleTimer_Tick(object sender, EventArgs e)
        {
            readFromScaleTimer.Stop();

            bool retry = false;

            int weight = 0;
            decimal price = 0;

            if (Services.Interfaces.Services.ScaleService(DLLEntry.DataModel).ReadFromScaleEx(out weight, 10000, "", saleLineItem.Description, saleLineItem.PriceWithTax, saleLineItem.TareWeight, out price))
            {
                shouldNotRetry = weight != 0; //if weight is zero then most probably the DataEvent did not fire yet so wait for it before displaying retry dialog
                if (weight != 0)
                {
                    Weight = weight / 1000m;

                    service.DataMessage -= ScaleOnScaleDataMessageEventX;
                    service.ErrorMessage -= ScaleOnScaleErrorMessageEventX;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                if(scaleConnected)
                {
                    if (((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Resources.NoItemOnScale + "\n" + Resources.TryAgain, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                    {
                        retry = true;
                    }
                    else
                    {
                        DialogResult = DialogResult.Cancel;
                        AbortScale();
                        Close();
                    }
                    scaleConnected = true;
                }
            }

            if(retry) readFromScaleTimer.Start();
        }

        private void ScaleOnScaleErrorMessageEventX(string message)
        {
            scaleConnected = false;

            if (shouldNotRetry) return;

            // Offer the user to retry
            string retryMessage = message + "\r\n" + "\r\n" + Resources.RetryQuestion;

            if ( Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(retryMessage, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
            {
                shouldNotRetry = false;
                readFromScaleTimer.Start();
            }
            else
            {
                shouldNotRetry = true;
                AbortScale();
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void ScaleOnScaleDataMessageEventX(decimal weight)
        {
            Weight = weight;
            
            service.DataMessage -= ScaleOnScaleDataMessageEventX;
            service.ErrorMessage -= ScaleOnScaleErrorMessageEventX;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AbortScale()
        {
            service.EnableScale(false);

            service.DataMessage -= ScaleOnScaleDataMessageEventX;
            service.ErrorMessage -= ScaleOnScaleErrorMessageEventX;
        }
    }
}
