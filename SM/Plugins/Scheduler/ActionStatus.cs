using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler
{
    /// <summary>
    /// Displays a status panel in the center of a specified control. Can be used both to show a background
    /// operation in progress and also the result of that operation. The status panel is non-modal.
    /// </summary>
    public class ActionStatus
    {
        private StatusPanel statusPanel;

        public ActionStatus(Control parentControl, string actionText)
            : this(parentControl, ActionStatusLayout.CenteredAutosize, actionText, null)
        {
        }



        public ActionStatus(Control parentControl, ActionStatusLayout actionStatusLayout, string actionText, Control controlToHide)
        {
            statusPanel = new StatusPanel();
            statusPanel.Visible = false;
            statusPanel.Margin = new Padding(10);

            switch (actionStatusLayout)
            {
                case ActionStatusLayout.CenteredAutosize:
                    statusPanel.AutoLocate = true;
                    statusPanel.AutoSize = true;
                    break;
                case ActionStatusLayout.FillOther:
                    statusPanel.AutoLocate = false;
                    statusPanel.AutoSize = false;
                    statusPanel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
                    statusPanel.Location = controlToHide.Location;
                    statusPanel.Size = new Size(controlToHide.Width, controlToHide.Height);
                    break;
                case ActionStatusLayout.FillParent:
                    statusPanel.AutoLocate = false;
                    statusPanel.AutoSize = false;
                    statusPanel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
                    statusPanel.Location = new Point(0, 0);
                    statusPanel.Size = new Size(parentControl.Width, parentControl.Height);
                    break;
                default:
                    throw new ArgumentException("Unknown ActionStatusLayout", "actionStatusLayout");
            }
            statusPanel.ActionText = actionText;
            parentControl.Controls.Add(statusPanel);
            statusPanel.BringToFront();
        }

        private delegate void SetDelegate(string text, bool showSpinningProgress, bool showAction);
        
        public void Set(string text, bool showSpinningProgress, bool showAction)
        {
            if (statusPanel.InvokeRequired)
            {
                statusPanel.Invoke(new SetDelegate(Set), text, showSpinningProgress, showAction);
            }
            else
            {
                statusPanel.StatusText = text;
                statusPanel.SpinningProgressVisible = showSpinningProgress;
                statusPanel.ActionVisible = showAction;
                statusPanel.Visible = true;
            }
        }

        public void Clear()
        {
            if (statusPanel.InvokeRequired)
            {
                statusPanel.Invoke(new System.Threading.ThreadStart(Clear));
            }
            else
            {
                statusPanel.Visible = false;
            }
        }


        public event EventHandler ActionClicked
        {
            add
            {
                statusPanel.ActionClicked += value;
            }
            remove
            {
                statusPanel.ActionClicked -= value;
            }
        }

        public bool ActionEnabled
        {
            get { return statusPanel.ActionEnabled; }
            set { statusPanel.ActionEnabled = value; }
        }

        public Font StatusFont
        {
            get { return statusPanel.StatusFont; }
            set { statusPanel.StatusFont = value; }
        }
    }

    /// <summary>
    /// Represent the layout of an action satus panel.
    /// </summary>
    public enum ActionStatusLayout
    {
        /// <summary>
        /// The action status panel will be centered in its parent control and will automatically
        /// resize itself based on its content.
        /// </summary>
        CenteredAutosize,
        /// <summary>
        /// The action status panel will be positioned and sized to completely cover the parent.
        /// </summary>
        FillParent,
        /// <summary>
        /// The action status panel will be positioned and sized to completely cover another control (not the parent).
        /// </summary>
        FillOther,
    }
}
