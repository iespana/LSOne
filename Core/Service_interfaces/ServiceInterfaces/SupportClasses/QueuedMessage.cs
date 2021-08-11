using System;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public delegate void QueuedMessageButtonClick(object sender, DialogResult result);

    [Serializable]
    public class QueuedMessage
    {
        private bool buttonsSet;
        private MessageBoxButtons buttons;

        private bool iconSet;
        private MessageBoxIcon icon;

        public event QueuedMessageButtonClick ButtonClicked;

        public QueuedMessage()
        {
            buttons = MessageBoxButtons.OK;
        }

        public string Message { get; set; }
        public string Caption { get; set; }
        public string Details { get; set; }

        public bool IsError { get; set; }

        public MessageBoxButtons Buttons
        {
            get { return buttons; }
            set
            {
                buttonsSet = true;
                buttons = value;
            }
        }

        public MessageBoxIcon Icon
        {
            get { return icon; }
            set
            {
                iconSet = true;
                icon = value;
            }
        }

        public DialogResult Show(IConnectionManager entry)
        {
            var dlgSrv = Services.DialogService(entry);
            if (IsError)
            {
                dlgSrv.ShowErrorMessage(Message, Details);
                return HandleResult(DialogResult.OK);
            }
             
            if (iconSet)
            {
                if (string.IsNullOrEmpty(Caption))
                    return HandleResult(dlgSrv.ShowMessage(Message, buttons));

                return HandleResult(dlgSrv.ShowMessage(Message, Caption, buttons));
            }

            if (buttonsSet)
            {
                return HandleResult(dlgSrv.ShowMessage(Message, Caption, buttons));
            }

            if (string.IsNullOrEmpty(Caption))
                return HandleResult(dlgSrv.ShowMessage(Message));

            return HandleResult(dlgSrv.ShowMessage(Message, Caption));
        }

        private DialogResult HandleResult(DialogResult result)
        {
            if (ButtonClicked != null)
                ButtonClicked(this, result);
            return result;
        }
    }
}