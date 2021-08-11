using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Utils
{
    /// <summary>
    /// Represents a single operation to be performed when the WinForms application is idle. The operation
    /// is run only once.
    /// </summary>
    public class IdleOneShotProcessing
    {
        public delegate void OneShotProcessing(object arg);

        private OneShotProcessing method;
        private object arg;
        private bool isDone;

        /// <summary>
        /// Initializes one shot idle processing with the specified method and an optional argument.
        /// </summary>
        /// <param name="method">The method to execute once when the application is idle.</param>
        /// <param name="arg">Optional argument to pass to the method.</param>
        public static IdleOneShotProcessing PostRun(OneShotProcessing method, object arg = null)
        {
            return new IdleOneShotProcessing(method, arg);
        }


        private IdleOneShotProcessing(OneShotProcessing method, object arg = null)
        {
            this.method = method;
            this.arg = null;
            Application.Idle += new EventHandler(Application_Idle);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            RunMethod();
        }

        private void RunMethod()
        {
            Application.Idle -= new EventHandler(Application_Idle);
            method(arg);
            isDone = true;
        }

        /// <summary>
        /// Gets a value indicating if the one shot processing method has been run.
        /// </summary>
        public bool IsDone
        {
            get
            {
                return isDone;
            }
        }

        /// <summary>
        /// Force an execution of the one shot processing method. Once completed, the method
        /// will not run again when the application becomes idle.
        /// </summary>
        /// <returns></returns>
        public bool Force()
        {
            if (isDone)
                return false;

            RunMethod();

            return true;
        }
    }
}
