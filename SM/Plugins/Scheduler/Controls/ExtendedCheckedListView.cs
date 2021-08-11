using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    /// <summary>
    /// This simple extension of the ExtendedListView control simply disables the check/uncheck
    /// effect of double-clicking when CheckBoxes property is true.
    /// </summary>
    public class ExtendedCheckedListView : ExtendedListView
    {
        private const int WM_LBUTTONDBLCLK = 0x203;

        protected override void WndProc(ref Message m)
        {
            // Filter WM_LBUTTONDBLCLK
            if (m.Msg != WM_LBUTTONDBLCLK)
            {
                base.WndProc(ref m);
            }
            else
            {
                this.OnDoubleClick(EventArgs.Empty);
            }
        }
    }
}
