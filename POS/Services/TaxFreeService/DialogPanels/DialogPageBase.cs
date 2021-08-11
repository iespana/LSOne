using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.TaxFree;

namespace LSOne.Services.DialogPanels
{
    public class DialogPageBase : UserControl
    {
        public virtual bool NextEnabled { get { return true; } }
        public virtual bool BackEnabled { get { return true; } }
        public virtual bool FinishEnabled { get { return false; } }

        protected DialogPageBase()
        {
            SetStyle(ControlStyles.Selectable, true);
            DoubleBuffered = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
            {
                Control firstEditable = null;
                // Make sure the focus is in an edit control
                foreach (Control control in Controls)
                {
                    if (control is Label || control is Button)
                        continue;
                    
                    if (control.CanFocus)
                        firstEditable = control;
                    if (control.Focused)
                    {
                        firstEditable = null;
                        break;
                    }
                }

                if (firstEditable != null)
                    firstEditable.Focus();
            }
        }

        protected bool IsTextFieldEmpty(LSOne.Controls.ShadeTextBoxTouch textBox)
        {
            return string.IsNullOrEmpty(textBox.Text.Trim());
        }

        public virtual void GetData(Tourist tourist)
        {
            
        }

        public virtual bool ValidateData()
        {
            return true;
        }
    }
}
