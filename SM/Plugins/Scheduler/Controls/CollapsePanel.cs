using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class CollapsePanel : GroupPanel
    {
        private bool hasBeenPainted;
        private bool collapsed;
        private int expandedHeight;
        private int collapsedHeight;

        public CollapsePanel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!hasBeenPainted)
            {
                hasBeenPainted = true;

                // Size-sensitive initialization: We register the heights we use for expanded view and collapsed view
                this.expandedHeight = this.Height;
                this.collapsedHeight = btnCollapse.Top * 2 + btnCollapse.Height;
                UpdateState();
            }

            if (DesignMode)
            {
                // Draw line to indicate where we will collapse
                e.Graphics.DrawLine(SystemPens.ControlDark, 0, this.collapsedHeight, this.ClientRectangle.Width, this.collapsedHeight);
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            if (this.btnCollapse != null)
            {
                // Adjust position of buttons since conventional anchoring refuses to work.
                this.btnCollapse.Left = this.Width - this.btnCollapse.Width - 4;
            }

            if (this.btnExpand != null)
            {
                //this.btnCollapse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.btnExpand.Left = this.Width - this.btnExpand.Width - 4;
                //this.btnExpand.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }
        }

        [DefaultValue(false)]
        public bool Collapsed
        {
            get { return collapsed; }
            set
            {
                if (collapsed == value)
                    return;
                collapsed = value;
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (collapsed)
            {
                this.Height = collapsedHeight;
                btnExpand.Visible = true;
                btnCollapse.Visible = false;
            }
            else
            {
                this.Height = expandedHeight;
                btnExpand.Visible = false;
                btnCollapse.Visible = true;
            }
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            Collapsed = true;
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            Collapsed = false;
        }

    }
}
