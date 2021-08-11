using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.ErrorHandling;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Scheduler.Utils
{
    /// <summary>
    /// Utility functions that help with showing message boxes and errors.
    /// </summary>
    public static class MessageUtils
    {
        public static void ShowError(IApplicationCallbacks framework, string text, string caption, Exception ex = null)
        {
            ShowError(framework, null, text, caption, ex);
        }

        public static void ShowError(IWin32Window owner, string text, string caption, Exception ex = null)
        {
            ShowError(null, owner, text, caption, ex);
        }


        public static void ShowError(IApplicationCallbacks framework, Exception ex)
        {
            ShowError(framework, null, Properties.Resources.UnhandledException, Properties.Resources.ErrorCaption, ex);
        }

        public static void ShowError(IApplicationCallbacks framework, IWin32Window owner, string text, string caption, Exception ex)
        {
            if (framework != null)
            {
                framework.LogMessage(LogMessageType.Error, text, ex);
                if (owner == null)
                {
                    owner = framework.MainWindow;
                }

                if (ex != null)
                {
                    text = string.Format(Properties.Resources.SeeMessageLogFormat, text);
                }
            }
            else
            {
                if (ex != null)
                {
                    text += Environment.NewLine + Environment.NewLine +
                        string.Format(Properties.Resources.ExceptionTypeFormat, ex.GetType().FullName) + Environment.NewLine +
                        string.Format(Properties.Resources.ExceptionMessageFormat, ex.Message);
                }
            }

            MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public static void ShowWarning(IWin32Window owner, string text, string caption)
        {
            MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        public static void ShowStop(IWin32Window owner, string text, string caption)
        {
            MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }



        public static void ShowInfo(IWin32Window owner, string text, string caption)
        {
            MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
